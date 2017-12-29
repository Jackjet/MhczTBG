using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;
using System.IO;
using Microsoft.Win32;
using System.Windows;
using System.Threading;

namespace MhczTBG.Common
{
    /// <summary>
    /// 客户端对象方法
    /// </summary>
    public class ClientContextMethod
    {
        private static ClientContext _clientContext;
        /// <summary>
        /// 客户端对象模型(使用该类的方法时，先创建该实例，然后设置验证凭据)
        /// </summary>
        public static ClientContext clientContext
        {
            get { return ClientContextMethod._clientContext; }
            set { ClientContextMethod._clientContext = value; }
        }

        #region 获取所有文档库和列表

        /// <summary>
        /// 获取指定站点下的所有文档库
        /// </summary>
        /// <param name="siteUrl">站点地址</param>      
        /// <returns>返回的所有文档库</returns>
        public FolderCollection GetAllFolders()
        {
            FolderCollection folderList = null;
            try
            {
                if (clientContext != null)
                {
                    //创建客户端对象模型
                    using (clientContext)
                    {
                        //获取站点下的所有文档库
                        folderList = clientContext.Web.Folders;

                        LoadMethod(clientContext.Web.Lists);
                        LoadMethod(clientContext.Web.Folders);
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "GetAllFolders", ex.ToString());
            }
            finally
            {

            }
            //返回所有文档
            return folderList;
        }

        /// <summary>
        /// 获取指定站点下的所有列表
        /// </summary>
        /// <param name="siteUrl">站点地址</param>      
        /// <returns>返回的所有列表</returns>
        public ListCollection GetAllLists(string webSiteUrl, string userName, string passWord, string doMain)
        {
            ListCollection listCollection = null;
            try
            {
                if (clientContext != null)
                {
                    //创建客户端对象模型
                    using (clientContext)
                    {
                        //获取站点下的所有文档库
                        listCollection = clientContext.Web.Lists;
                        LoadMethod(clientContext.Web.Lists);
                    }
                }
                else
                {
                    //创建客户端对象模型
                    using (clientContext = new ClientContext(webSiteUrl))
                    {
                        clientContext.Credentials = new System.Net.NetworkCredential(userName, passWord, doMain);
                        //获取站点下的所有文档库
                        listCollection = clientContext.Web.Lists;
                        LoadMethod(clientContext.Web.Lists);
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "GetAllLists", ex.ToString(), userName, passWord, doMain);
            }
            finally
            {
            }
            //返回所有文档
            return listCollection;
        }

        #endregion

        #region 新建一个文档库

        /// <summary>
        /// 新建一个文档库
        /// </summary>
        /// <param name="siteUrl">站点的uri地址</param>
        /// <param name="DocumentLibraryName">文档库的显示名称</param>        
        /// <returns>返回操作的信息</returns>
        public List CreateDocumentLibrary(string folderName, ListTemplateType folderType)
        {
            //新建一个List
            Microsoft.SharePoint.Client.List newList = null;
            try
            {
                if (clientContext != null)
                {
                    //文档库创建信息操作类
                    ListCreationInformation newListInfo = new ListCreationInformation();
                    //文档库的显示标题
                    newListInfo.Title = folderName;
                    //文档库
                    newListInfo.TemplateType = (int)folderType;

                    //创建客户端对象模型
                    using (clientContext)
                    {
                        //设置默认凭据

                        //获取站点(site)
                        Web site = clientContext.Web;
                        //添加文档库
                        newList = site.Lists.Add(newListInfo);
                        //加载文档库并执行
                        clientContext.Load(newList);
                        clientContext.ExecuteQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "CreateDocumentLibrary", ex.ToString(), folderName, folderType);
            }
            finally
            {
            }
            return newList;
        }

        #endregion

        #region 删除一个文档库

        /// <summary>
        /// 指定一个文档库的名称并将其删除
        /// </summary>
        /// <param name="siteUrl">站点</param>
        /// <param name="DocumentLibraryName">文档库的名称</param>
        /// <returns>返回操作的信息</returns>
        public string DeleteDocumentLibrary(string folderName)
        {
            string result = string.Empty;
            try
            {
                if (clientContext != null)
                {
                    //创建客户端对象模型
                    using (clientContext)
                    {
                        //设置默认凭据

                        //获取站点(site)
                        Web site = clientContext.Web;
                        //通过标题获取文档库
                        Microsoft.SharePoint.Client.List existList = site.Lists.GetByTitle(folderName);
                        //删除文档库对象
                        existList.DeleteObject();
                        //执行操作
                        clientContext.ExecuteQuery();
                        result = "删除成功";
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DeleteDocumentLibrary", ex.ToString(), folderName);
            }
            finally
            {
            }
            return result;
        }

        #endregion

        #region 上传一个文档

        /// <summary>
        /// 上传一个文档
        /// </summary>
        /// <param name="siteUrl">指定站点</param>
        /// <param name="documentListName">指定文件夹名称</param>
        /// <returns>返回操作提示</returns>
        public Microsoft.SharePoint.Client.File UploadFileToDocumntLibrary(string siteUrl, string folderName)
        {
            Microsoft.SharePoint.Client.File uploadFile = null;
            try
            {
                if (clientContext != null)
                {
                    //使用打开对话框
                    Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
                    //声明一个缓冲区
                    byte[] documentStream = null;

                    //文件名称
                    string documentName = null;

                    //点击确定
                    if (dialog.ShowDialog() == true)
                    {
                        //获取指定名称
                        documentName = dialog.SafeFileName;
                        //获取文件流
                        Stream stream = dialog.OpenFile();
                        //创建一个缓冲区
                        documentStream = new byte[stream.Length];
                        //将流写入指定缓冲区
                        stream.Read(documentStream, 0, documentStream.Length);
                        //上传文档              

                        using (clientContext)
                        {
                            //设置默认凭据

                            //通过标题获取文档库
                            Microsoft.SharePoint.Client.List documentsList = clientContext.Web.Lists.GetByTitle(folderName);

                            //加载并更新列表
                            LoadMethod(documentsList);

                            //
                            LoadMethod(documentsList.RootFolder);

                            //获取文档库url
                            var d = documentsList.RootFolder.ServerRelativeUrl;
                            string a = d.Replace(documentsList.ParentWebUrl + "/", "");

                            //新建一个文件信息类
                            var fileCreationInformation = new FileCreationInformation();
                            //设置文件内容
                            fileCreationInformation.Content = documentStream;
                            //可以覆盖
                            fileCreationInformation.Overwrite = true;
                            //文件信息的uri
                            fileCreationInformation.Url = siteUrl + "/" + a + "/" + documentName;
                            //加载文档
                            uploadFile = documentsList.RootFolder.Files.Add(fileCreationInformation);
                            //加载并更新文件
                            LoadMethod(uploadFile);
                            stream.Dispose();
                            stream.Close();
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "UploadFileToDocumntLibrary", ex.ToString(), siteUrl, folderName);
            }
            finally
            {
            }
            return uploadFile;
        }

        /// <summary>
        /// 上传一个文档
        /// </summary>
        /// <param name="siteUrl">指定站点</param>
        /// <param name="documentListName">指定文档库名称</param>
        /// <param name="documentListURL">指定文档库的Url</param>
        /// <param name="documentName">设置上传文件的名称</param>
        /// <param name="documentStream">缓冲区</param>
        /// <returns>返回操作提示</returns>
        public string UploadFileToDocumntLibrary(string siteUrl, string folderName, string fileName, byte[] documentStream)
        {
            string result = string.Empty;
            try
            {
                if (clientContext != null)
                {
                    using (clientContext)
                    {

                        Microsoft.SharePoint.Client.List documentsList = clientContext.Web.Lists.GetByTitle(folderName);

                        LoadMethod(documentsList);

                        var fileCreationInformation = new FileCreationInformation();



                        fileCreationInformation.Content = documentStream;
                        fileCreationInformation.Overwrite = true;

                        LoadMethod(documentsList.RootFolder);
                        //获取文档库url
                        var d = documentsList.RootFolder.ServerRelativeUrl;
                        string a = d.Replace(documentsList.ParentWebUrl + "/", "");

                        //文件信息的uri
                        fileCreationInformation.Url = siteUrl + "/" + a + "/" + fileName;

                        Microsoft.SharePoint.Client.File uploadFile = documentsList.RootFolder.Files.Add(fileCreationInformation);
                        uploadFile.ListItemAllFields.Update();
                        clientContext.ExecuteQuery();
                        result = "上传成功";
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "UploadFileToDocumntLibrary", ex.ToString(), siteUrl, folderName, documentStream);
            }
            finally
            {
            }
            return result;
        }

        #endregion

        #region 删除一个文档

        /// <summary>
        /// 删除一个文档
        /// </summary>
        /// <param name="siteURL">指定站点地址</param>
        /// <param name="documentListName">指定文档库名称</param>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public string DeleFile(string folderName, string FileName)
        {
            string result = string.Empty;
            try
            {

                //创建客户端对象模型
                using (clientContext)
                {
                    //设置默认凭据

                    //通过标题获取文档库
                    Microsoft.SharePoint.Client.List documentsList = clientContext.Web.Lists.GetByTitle(folderName);
                    LoadMethod(documentsList);
                    LoadMethod(documentsList.RootFolder.Files);
                    foreach (var item in documentsList.RootFolder.Files)
                    {
                        if (item.Name.Equals(FileName))
                        {
                            item.DeleteObject();
                            clientContext.ExecuteQuery();

                            result = "删除成功";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DeleFile", ex.ToString(), folderName, FileName);
            }
            finally
            {
            }
            return result;
        }

        #endregion

        #region 下载一个文档

        /// <summary>
        /// 下载一个文档
        /// </summary>
        /// <param name="siteURL">指定站点</param>
        /// <param name="documentListName">指定文档库名称</param>
        /// <param name="documentName">指定文档名称</param>
        /// <returns></returns>
        public string DownloadDocument(string siteURL, string documentListName, string documentName)
        {
            //操作结果
            string result = string.Empty;
            try
            {
                if (clientContext != null)
                {                                      
                    //声明一个缓冲区
                    byte[] bt = null;
                    //获取Item项
                    Microsoft.SharePoint.Client.ListItem item = GetDocumentFromSP(siteURL, documentListName, documentName);

                    if (item != null)
                    {
                        //创建客户端对象模型
                        using (clientContext)
                        {
                            //设置默认凭据

                            //创建一个文件信息对象
                            FileInformation fInfo = Microsoft.SharePoint.Client.File.OpenBinaryDirect(clientContext, item["FileRef"].ToString());
                            //获取文件信息的流
                            Stream s = fInfo.Stream;
                            //获取字节数组
                            bt = ReadFully(s, 0);
                        }
                    }
                    //创建一个保存对话框
                    SaveFileDialog saveDialog = new SaveFileDialog();
                    //设置保存的文件名称
                    saveDialog.FileName += documentName;
                    //扩展名
                    string extension = System.IO.Path.GetExtension(saveDialog.FileName);

                    //保存对话框对应的类型
                    saveDialog.Filter = string.Format("*{0}| *{0}", extension);

                    //如果选择为OK
                    if (saveDialog.ShowDialog() == true)
                    {
                        //创建一个文件
                        using (FileStream fs = System.IO.File.Create(saveDialog.FileName))
                        {

                            //填充字节形成文件内容
                            if (bt != null && bt.Length > 0)
                            {
                                //通过文件流将缓冲区的内容写入文件
                                fs.Write(bt, 0, bt.Length);
                                result = "下载完成";
                            }
                        }
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }

        /// <summary>
        /// 获取一个Item
        /// </summary>
        /// <param name="siteURL">指定站点Uri</param>
        /// <param name="documentListName">指定文档库名称</param>
        /// <param name="documentName">指定文档名称</param>
        /// <returns></returns>
        Microsoft.SharePoint.Client.ListItem GetDocumentFromSP(string siteURL, string documentListName, string documentName)
        {
            ListItem listItem = null;
            try
            {
                if (clientContext != null)
                {
                    // 获取Item的集合
                    Microsoft.SharePoint.Client.ListItemCollection listItems = GetListItemCollectionFromSP(documentListName, "FileLeafRef",
                    documentName, 1);
                    //返回item的集合中的第一个Item
                    listItem = (listItems != null && listItems.Count == 1) ? listItems[0] : null;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "GetDocumentFromSP", ex.ToString(), siteURL, documentListName, documentName);
            }
            finally
            {
            }
            return listItem;
        }



        /// <summary>
        /// 获取Item的集合
        /// </summary>
        /// <param name="siteURL">指定站点Uri</param>
        /// <param name="documentListName">指定文档库名称</param>
        /// <param name="name">指定字段引用</param>
        /// <param name="value">指定文件名称</param>       
        /// <param name="rowLimit">指定限定限制</param>
        /// <returns>返回Item的集合</returns>
        Microsoft.SharePoint.Client.ListItemCollection GetListItemCollectionFromSP(string documentListName, string name, string value, int rowLimit)
        {
            //声明一个item的集合
            Microsoft.SharePoint.Client.ListItemCollection listItems = null;
            try
            {
                if (clientContext != null)
                {
                    //创建客户端对象模型
                    using (clientContext)
                    {
                        //设置默认凭据

                        //通过文档库名称获取文档库
                        Microsoft.SharePoint.Client.List documentsList = clientContext.Web.Lists.GetByTitle(documentListName);
                        //创建一个查询
                        CamlQuery camlQuery = new CamlQuery();
                        camlQuery.ViewXml =
                        @"<View>
                <Query>
                <Where>
                <Eq>
                <FieldRef Name=""" + name + @"""/>
                <Value Type="" + type + "">" + value + @"</Value>
                </Eq> </Where> 
                <RowLimit>" + rowLimit.ToString() + @"</RowLimit>
                </Query>
                </View>";
                        //通过查询获取item的集合
                        listItems = documentsList.GetItems(camlQuery);
                        //执行
                        clientContext.Load(documentsList);
                        clientContext.Load(listItems);
                        clientContext.ExecuteQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "GetListItemCollectionFromSP", ex.ToString(), documentListName, name, value, rowLimit);
            }
            finally
            {
            }
            return listItems;
        }

        /// <summary>
        /// 获取字节数组
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="initialLength"></param>
        /// <returns></returns>
        byte[] ReadFully(Stream stream, int initialLength)
        {
            byte[] ret = null;
            try
            {
                if (clientContext != null)
                {
                    //如果小于1，则需要给其设置一个标准
                    if (initialLength < 1)
                    {
                        initialLength = 32768;
                    }
                    //创建一个缓冲区
                    byte[] buffer = new byte[initialLength];
                    //从零开始读取
                    int read = 0;
                    //读取出来的长度
                    int chunk;
                    //循环读取
                    while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
                    {
                        //读取位置偏移到已读取的地方
                        read += chunk;

                        //判断是否读到末尾
                        if (read == buffer.Length)
                        {
                            //进一步判断是否读到末尾
                            int nextByte = stream.ReadByte();
                            //如果读完则返回
                            if (nextByte == -1)
                            {
                                return buffer;
                            }
                            //创建一个长度两倍于buffer的缓冲区
                            byte[] newBuffer = new byte[buffer.Length * 2];
                            //并将buffer里的内容拷贝到newBuffer里
                            Array.Copy(buffer, newBuffer, buffer.Length);
                            //开始读的位置已是末尾,
                            newBuffer[read] = (byte)nextByte;
                            //将newBuffer给buffer
                            buffer = newBuffer;
                            read++;
                        }
                    }
                    //返回的缓冲字节数组
                    ret = new byte[read];
                    //填充内容
                    Array.Copy(buffer, ret, read);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ReadFully", ex.ToString(), stream, initialLength);
            }
            finally
            {
            }
            return ret;
        }


        #endregion

        #region 文档签出、签入

        /// <summary>
        /// 文档签出
        /// </summary>
        /// <param name="siteUrl">指定站点地址</param>
        /// <param name="folderName">指定文档库名称</param>
        /// <param name="fileName">指定文档名称</param>
        /// <returns>返回操作提示</returns>
        public string CheckOut(string folderName, string fileName)
        {
            //声明一个操作提示
            string result = null;
            try
            {
                if (clientContext != null)
                {
                    //创建客户端对象模型
                    using (clientContext)
                    {
                        //通过标题获取文档库
                        Microsoft.SharePoint.Client.List documentsList = clientContext.Web.Lists.GetByTitle(folderName);
                        //加载该文档库
                        LoadMethod( documentsList);
                        //加载该文档库的文档集合
                        LoadMethod( documentsList.RootFolder.Files);
                        //循环遍历该文档库里的文档
                        foreach (var item in documentsList.RootFolder.Files)
                        {
                            //获取指定名称的文档
                            if (item.Name.Equals(fileName))
                            {
                                //加载文档的版本控制
                                LoadMethod(item.Versions);
                                //签出文档
                                item.CheckOut();
                                //执行
                                clientContext.ExecuteQuery();
                                //生成提示
                                result = "签出成功";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = "文档已被签出";
                MethodLb.CreateLog(this.GetType().FullName, "CheckOut", ex.ToString(), folderName, fileName);
            }
            finally
            {
            }
            return result;
        }

        /// <summary>
        /// 文档签入
        /// </summary>
        /// <param name="siteUrl">指定站点地址</param>
        /// <param name="folderName">指定文档库名称</param>
        /// <param name="fileName">指定文档名称</param>
        /// <param name="comment">签入的注释</param>
        /// <returns>返回操作提示</returns>
        public string CheckIn(string folderName, string fileName, string comment)
        {
            //声明一个操作提示
            string result = null;
            try
            {
                if (clientContext != null)
                {
                    //创建客户端对象模型
                    using (clientContext)
                    {
                        //设置默认凭据

                        //通过标题获取文档库
                        Microsoft.SharePoint.Client.List documentsList = clientContext.Web.Lists.GetByTitle(folderName);
                        //加载该文档库
                        LoadMethod(documentsList);
                        //加载该文档库的文档集合
                        LoadMethod(documentsList.RootFolder.Files);
                        //循环遍历该文档库里的文档
                        foreach (var item in documentsList.RootFolder.Files)
                        {
                            //获取指定名称的文档
                            if (item.Name.Equals(fileName))
                            {
                                //加载文档的版本控制
                                LoadMethod(item.Versions);
                                //签入文档
                                item.CheckIn(comment, CheckinType.MajorCheckIn);
                                //执行操作
                                clientContext.ExecuteQuery();
                                //生成提示
                                result = "签入成功";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //生成提示
                result = "该文档已签入";
                MethodLb.CreateLog(this.GetType().FullName, "CheckIn", ex.ToString(), folderName, fileName, comment);
            }
            finally
            {
            }
            return result;
        }

        #endregion



        #region 获取表单所有数据

        /// <summary>
        /// 通过查询语句获取表单所有数据
        /// </summary>
        /// <param name="website">站点地址</param>
        /// <param name="listName">指定列表名称</param>
        /// <param name="querystring">指定查询语句</param>
        /// <returns>返回子项的集合</returns>
        public  List<Dictionary<string, object>> ClientGetDic(string website, string listName, string querystring)
        {
            List<Dictionary<string, object>> dicList = new List<Dictionary<string, object>>();
            try
            {
                if (clientContext != null)
                {
                    //创建客户端对象模型          
                    using (clientContext)
                    {
                        //获取列表
                        List oList = clientContext.Web.Lists.GetByTitle(listName);

                        CamlQuery camlQuery = new CamlQuery();
                        camlQuery.ViewXml = querystring;

                        //获取当前列表的所有项
                        Microsoft.SharePoint.Client.ListItemCollection collListItem = oList.GetItems(camlQuery);
                        //执行
                        clientContext.Load(collListItem);
                        clientContext.ExecuteQuery();

                        foreach (var item in collListItem)
                        {
                            dicList.Add(item.FieldValues);
                        }
                    }
                }
             }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ClientGetDic", ex.ToString(), website, listName, querystring);
            }
            finally
            {
            }

            return dicList;
        }
        #endregion

        #region 新建一个文档

        public string CreateFile(string siteUrl, string folderName, string fileName)
        {
            string result = string.Empty;
            try
            {
                if (clientContext != null)
                {
                    using (clientContext)
                    {
                        //设置默认凭据

                        //通过标题获取文档库
                        Microsoft.SharePoint.Client.List documentsList = clientContext.Web.Lists.GetByTitle(folderName);

                        //加载并更新列表
                        LoadMethod( documentsList);

                        //
                        LoadMethod(documentsList.RootFolder);

                        //获取文档库url
                        var d = documentsList.RootFolder.ServerRelativeUrl;
                        string a = d.Replace(documentsList.ParentWebUrl + "/", "");

                        //新建一个文件信息类
                        var fileCreationInformation = new FileCreationInformation();

                        FileStream fs = System.IO.File.Create("C:\\" + fileName);

                        byte[] data = new byte[fs.Length];

                        fs.Read(data, 0, data.Length);


                        //设置文件内容
                        fileCreationInformation.Content = data;
                        //可以覆盖
                        fileCreationInformation.Overwrite = true;
                        //文件信息的uri
                        fileCreationInformation.Url = siteUrl + "/" + a + "/" + fileName;
                        //加载文档
                        Microsoft.SharePoint.Client.File uploadFile = documentsList.RootFolder.Files.Add(fileCreationInformation);
                        //加载并更新文件
                        LoadMethod(uploadFile);

                        fs.Dispose();
                        System.IO.File.Delete("C:\\" + fileName);
                        result = "创建成功";
                    }

                }
            }
            catch (Exception ex)
            {
                result = "创建失败";
                MethodLb.CreateLog(this.GetType().FullName, "CreateFile", ex.ToString(), siteUrl, folderName, fileName);
            }
            finally
            {
            }
            return result;
        }

        #endregion

        #region 移动文档

        /// <summary>
        /// 移动文档
        /// </summary>
        /// <param name="siteUrl">指定站点地址</param>
        /// <param name="folderName1">指定文档库名称（移动源）</param>
        /// <param name="folderName2">指定文档库名称（移动目标）</param>
        /// <param name="fileName">指定文档名称</param>
        /// <returns>返回操作提示</returns>
        public string MoveFile(string folderName1, string folderName2, string fileName)
        {
            //操作结果提示
            string result = null;
            try
            {
                if (clientContext != null)
                {
                    //创建客户端对象模型
                    using (clientContext)
                    {
                        //设置默认凭据

                        //通过标题获取文档库
                        Microsoft.SharePoint.Client.List documentsList = clientContext.Web.Lists.GetByTitle(folderName1);
                        LoadMethod(documentsList);

                        LoadMethod(documentsList.RootFolder.Files);

                        Microsoft.SharePoint.Client.File file = null;
                        foreach (var item in documentsList.RootFolder.Files)
                        {
                            if (item.Name.Equals(fileName))
                            {
                                file = item;
                            }
                        }
                        //通过标题获取文档库
                        Microsoft.SharePoint.Client.List documentsList2 = clientContext.Web.Lists.GetByTitle(folderName2);
                        LoadMethod(documentsList2);
                        LoadMethod(documentsList2.RootFolder);
                        //获取文档库url
                        var d = documentsList2.RootFolder.ServerRelativeUrl;
                        file.MoveTo(d + "/" + fileName, MoveOperations.Overwrite);
                        clientContext.ExecuteQuery();
                        result = "文档移动成功";
                    }
                }
            }
            catch (Exception ex)
            {
                result = "文档移动失败";
                MethodLb.CreateLog(this.GetType().FullName, "MoveFile", ex.ToString(), folderName1, folderName2, fileName);
            }
            finally
            {
            }
            return result;
        }

        #endregion

        #region 拷贝文档

        /// <summary>
        /// 复制文档
        /// </summary>
        /// <param name="siteUrl">指定站点地址</param>
        /// <param name="folderName1">指定文档库名称（复制源）</param>
        /// <param name="folderName2">指定文档库名称（复制目标）</param>
        /// <param name="fileName">指定文档名称</param>
        /// <returns>返回操作提示</returns>
        public string CopyFile(string folderName1, string folderName2, string fileName)
        {
            //操作结果提示
            string result = null;
            try
            {
                if (clientContext != null)
                {
                    //创建客户端对象模型
                    using (clientContext)
                    {
                        //设置默认凭据

                        //通过标题获取文档库
                        Microsoft.SharePoint.Client.List documentsList = clientContext.Web.Lists.GetByTitle(folderName1);
                        LoadMethod(documentsList);

                        LoadMethod(documentsList.RootFolder.Files);

                        Microsoft.SharePoint.Client.File file = null;
                        foreach (var item in documentsList.RootFolder.Files)
                        {
                            if (item.Name.Equals(fileName))
                            {
                                file = item;
                            }
                        }
                        //通过标题获取文档库
                        Microsoft.SharePoint.Client.List documentsList2 = clientContext.Web.Lists.GetByTitle(folderName2);
                        LoadMethod(documentsList2);
                        LoadMethod(documentsList2.RootFolder);
                        //获取文档库url
                        var d = documentsList2.RootFolder.ServerRelativeUrl;
                        //string a = d.Replace(documentsList.ParentWebUrl + "/", "");
                        file.CopyTo(d + "/" + fileName, true);
                        clientContext.ExecuteQuery();
                        result = "文档拷贝成功";
                    }
                }
            }
            catch (Exception ex)
            {
                result = "文档拷贝失败";
                MethodLb.CreateLog(this.GetType().FullName, "CopyFile", ex.ToString(), folderName1, folderName2, fileName);
            }
            finally
            {
            }
            return result;
        }

        #endregion

        #region 获取文档

        /// <summary>
        /// 获取文档
        /// </summary>
        /// <param name="siteUrl">指定站点地址</param>
        /// <param name="folderName">指定文档库名称</param>
        /// <returns>返回当前文档库里的所有文档</returns>
        public List<Microsoft.SharePoint.Client.File> GetFiles(string siteUrl, string folderName)
        {
            List<Microsoft.SharePoint.Client.File> fileList = new List<Microsoft.SharePoint.Client.File>();
            try
            {
                if (clientContext != null)
                {
                    //创建客户端对象模型
                    using (clientContext)
                    {
                        //设置默认凭据

                        //通过标题获取文档库
                        Microsoft.SharePoint.Client.List documentsList = clientContext.Web.Lists.GetByTitle(folderName);
                        //加载该文档库
                        LoadMethod(documentsList);
                        //加载该文档库的文档集合
                        LoadMethod(documentsList.RootFolder.Files);
                        //循环遍历该文档库里的文档
                        foreach (var item in documentsList.RootFolder.Files)
                        {
                            //加载文档创建者
                            LoadMethod(item.Author);
                            //加载文档修改者
                            LoadMethod(item.ModifiedBy);
                            //加载文档版本控制
                            LoadMethod(item.Versions);
                            //加载文档
                            fileList.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "GetFiles", ex.ToString(), siteUrl, folderName);
            }
            finally
            {
            }
            //返回该文档库中所有文档
            return fileList;
        }

        /// <summary>
        /// 获取文档
        /// </summary>
        /// <param name="siteUrl">指定站点地址</param>
        /// <param name="folderName">指定文档库名称</param>
        /// <param name="fileName">指定文档名称</param>
        /// <returns>返回指定名称的文档</returns>
        public Microsoft.SharePoint.Client.File GetFile(string folderName, string fileName)
        {
            //声明将要返回的文档
            Microsoft.SharePoint.Client.File file = null;
            try
            {
                if (clientContext != null)
                {
                    //创建客户端对象模型
                    using (clientContext)
                    {
                        //设置默认凭据

                        //通过标题获取文档库
                        Microsoft.SharePoint.Client.List documentsList = clientContext.Web.Lists.GetByTitle(folderName);
                        //加载该文档库
                        LoadMethod(documentsList);
                        //加载该文档库的文档集合
                        LoadMethod(documentsList.RootFolder.Files);
                        //循环遍历该文档库里的文档
                        foreach (var item in documentsList.RootFolder.Files)
                        {
                            //获取指定名称的文档
                            if (item.Name.Equals(fileName))
                            {
                                //加载文档的版本控制
                                LoadMethod(item.Versions);
                                file = item;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "GetFiles", ex.ToString(), folderName, fileName);
            }
            finally
            {
            }
            return file;
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 加载快捷方法
        /// </summary>
        /// <param name="clientContext">客户端对象模型</param>
        /// <param name="obj">要加载的对象</param>
        public void LoadMethod(ClientObject obj)
        {
            try
            {
                if (clientContext != null)
                {
                    //加载该对象
                    clientContext.Load(obj);
                    //执行操作
                    clientContext.ExecuteQuery();
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "LoadMethod", ex.ToString(), obj);
            }
            finally
            {
            }
        }

        #endregion

        #region 通过客户端对象模型上传一个文件（有关sharepoint详细的操作方法在ClientContextMethod里有）

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="siteURL">网站url</param>
        /// <param name="documentListName">列表名称</param>
        /// <param name="documentListURL">文件地址</param>
        /// <param name="documentName">文件名称</param>
        /// <param name="documentStream">字节数组</param>
        public void UploadDocument(string siteURL, string documentListName, string documentListURL, string documentName, byte[] documentStream)
        {
            try
            {
                if (clientContext != null)
                {
                    //通过客户端对象模型来上传文件   
                    using (clientContext)
                    {

                        //获取文档库
                        List documentsList = clientContext.Web.Lists.GetByTitle(documentListName);

                        //文件创建信息     
                        var fileCreationInformation = new FileCreationInformation();

                        //指定内容 byte[]数组，这里是 documentStream
                        fileCreationInformation.Content = documentStream;

                        //允许文档覆盖
                        fileCreationInformation.Overwrite = true;

                        //上载 URL地址
                        fileCreationInformation.Url = siteURL + documentListURL + documentName;

                        Microsoft.SharePoint.Client.File uploadFile = documentsList.RootFolder.Files.Add(fileCreationInformation);

                        //更新元数据信息，这里是一个显示名为“描述”的字段，其字段名为“Description0”
                        uploadFile.ListItemAllFields["Description0"] = "内容";

                        //先更新一下
                        uploadFile.ListItemAllFields.Update();
                        //然后再执行
                        clientContext.ExecuteQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "UploadDocument", ex.ToString(), siteURL, documentListName, documentListURL, documentName, documentStream);
            }
            finally
            {
            }
        }

        #endregion


        #region 获取一个列表字段的详细信息(测试)

        ///// <summary>
        ///// 需要管理员的权限
        ///// </summary>
        //void GetOneFeldInfo()
        //{
        //    //添加一个字段
        //    new Thread(() =>
        //    {
        //        if (clientContext != null)
        //        {
        //            using (clientContext)
        //            {
        //                //clientContext.Credentials = new System.Net.NetworkCredential("Administrator", "Password2012", "bjtxd");

        //                //获取列表
        //                Microsoft.SharePoint.Client.List oList = clientContext.Web.Lists.GetByTitle("TXListGZ");

        //                //Microsoft.SharePoint.Client.ListItem listItem = oList.GetItemById("1293");

        //                clientContext.Load(oList);
        //                clientContext.Load(oList.Fields);
        //                clientContext.ExecuteQuery();

        //                foreach (var item in oList.Fields)
        //                {
        //                    if (item.Title.Equals("Location"))
        //                    {
        //                        var tittle = item.Title;
        //                        var interName = item.InternalName;
        //                        var type = item.TypeAsString;
        //                        var displayname = item.TypeDisplayName;
        //                    }
        //                }
        //            }
        //        }
        //    }).Start();
        //}

        #endregion

        #region 给Sharepoint列表添加一个字段（测试）

        //void AddOneField()
        //{
        //    //添加一个字段
        //    new Thread(() =>
        //    {
        //        using ( clientContext)
        //        {
        //            clientContext.Credentials = new System.Net.NetworkCredential("Administrator", "Password2012", "bjtxd");

        //            //获取列表
        //            Microsoft.SharePoint.Client.List oList = clientContext.Web.Lists.GetByTitle("TXListGZ");
        //            //显示名称 tittle设置的值最终一样
        //            oList.Fields.AddFieldAsXml("<Field DisplayName='新建列表项' Type='Text' InternalName='nono' />", true, AddFieldOptions.AddToAllContentTypes);
        //            oList.Update();
        //            clientContext.ExecuteQuery();
        //        }
        //    }).Start();
        //}

        #endregion       

        #region 用客户段对象模型获取所有列表下的附件名称（测试）

        //void GetListItemFileNames()
        //{
            //string b = "http://192.168.0.154/";

            //using (ClientContext clientContext = new ClientContext("http://192.168.0.154/SFM"))
            //{
            //    clientContext.Credentials = new System.Net.NetworkCredential("txddd", "123", "bjtxd");
            //    //获取列表
            //    Microsoft.SharePoint.Client.List oList = clientContext.Web.Lists.GetByTitle("TXListGZ");

            //    clientContext.Load(oList);
            //    clientContext.ExecuteQuery();

            //    Microsoft.SharePoint.Client.ListItem listItem = oList.GetItemById(1283);

            //    clientContext.Load(listItem);
            //    clientContext.ExecuteQuery();

            //    var data = listItem.FieldValues["startData"];

            //    DateTime dt = DateTime.Parse(data.ToString()).ToLocalTime();


            //    var qu = dt.ToShortTimeString();


            //    string ByIDGetAllFileNames = string.Empty;

            //    #region 获取指定ID文档下的所有文件名称

            //    clientContext.Load(oList.RootFolder.Folders);
            //    clientContext.ExecuteQuery();

            //    Folder folderGet = null;

            //    foreach (var item in oList.RootFolder.Folders)
            //    {
            //        if (item.Name.Equals("Attachments"))
            //        {
            //            clientContext.Load(item.Folders);
            //            clientContext.ExecuteQuery();

            //            foreach (var itemChild in item.Folders)
            //            {
            //                if (itemChild.Name.Equals("1283"))
            //                {
            //                    folderGet = itemChild;
            //                    break;
            //                }
            //            }
            //            break;
            //        }
            //    }

            //    if (folderGet != null)
            //    {
            //        clientContext.Load(folderGet.Files);
            //        clientContext.ExecuteQuery();

            //        foreach (var item in folderGet.Files)
            //        {
            //            ByIDGetAllFileNames += b + item.ServerRelativeUrl + ";";
            //        }
            //    }

            //    #endregion
            //}
        //}

        #endregion

    }
}
