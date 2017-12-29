using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
//using RwpLb.sRWP;
using System.Threading;
using System.ComponentModel;
using ol = Microsoft.SharePoint.Client;
using System.Net;
using Microsoft.Win32;
using System.IO;
using MhczTBG.Common;

namespace MhczTBG.Controls.Files
{
    /// <summary>
    /// FileUp.xaml 的交互逻辑
    /// </summary>
    public partial class FileUp
    {
        #region 变量

        /// <summary>
        /// 要上传的文件
        /// </summary>
        List<DocumentLb> listDocuments = new List<DocumentLb>();

        //分块总大小
        int filePartAllCount;

        //判断是否进行分块上传
        int PartLongh = 2097152;

        //分块大小
        int BURRFERLEN = 512000;//2097152; 

        //判断是否启用合并文件方法的指标
        System.Collections.ObjectModel.ObservableCollection<string> fileParts = new System.Collections.ObjectModel.ObservableCollection<string>();

        /// <summary>
        /// 标记附件是否上传完成
        /// </summary>
        public bool isFilePartsUpLoadFinish = false;

        //整个文件的大小
        long lonSize = 0;

        //当前完整时间
        string TimeNow = DateTime.Now.ToLongDateString().Replace(",", "") + DateTime.Now.ToShortTimeString().Replace(":", "时") + "分";

        /// <summary>
        /// datagrid使用的数据源（选择附件时临时生成）
        /// </summary>
        public List<FileInformationLb> FileInformationLbList = new List<FileInformationLb>();

        /// <summary>
        /// 判断是否要上传
        /// </summary>
        public bool isNeedUpLoad = false;

        /// <summary>
        /// 用于存储权限编辑参数
        /// </summary>
        Dictionary<string, object> webParms = new Dictionary<string, object>();

        /// <summary>
        /// 子项的ID号
        /// </summary>
        string GzID = string.Empty;

        /// <summary>
        /// 列表名称
        /// </summary>
        string ListName = string.Empty;

        /// <summary>
        /// 是否有编辑权限
        /// </summary>
        bool IsEdit;

        #endregion

        #region 自定义委托事件

        public delegate void GetPermissionEventHandle(ref bool isEdit);
        /// <summary>
        /// 如果没有权限请在使用上传之前将这个定义好，关联到获取权限的方法里（若当前用户没有编辑权限，则激发该事件）
        /// </summary>
        public event GetPermissionEventHandle GetPermissionEvent = null;

        public delegate void SumFileEventHandle(string ListName, string ItemID, string FileSCount);
        /// <summary>
        /// 如果文件超过指定值必须进行合并文件的操作，将此事件关联到合并文件的方法（当大文件循环上传完毕则激发该事件
        /// </summary>
        public event SumFileEventHandle SumFileEvent = null;

        public delegate void DeletePermissionEventHandle(ref bool isEdit);
        /// <summary>
        /// 如果上传之前没有编辑权限，获取权限，并完成上传之后，去掉编辑权限
        /// </summary>
        public event DeletePermissionEventHandle DeletePermissionEvent = null;

        #endregion

        #region 构造函数

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="SiteUri">网站站点</param>
        /// <param name="UserName">用户名</param>
        /// <param name="PassWord">密码</param>
        /// <param name="Domain">域名</param>
        /// <param name="documentListName">文档名称</param>
        /// <param name="GZID">文档的ID号</param>
        /// <param name="documentListURL">文档的路径</param>
        public FileUp(string SiteUri, string UserName, string PassWord, string Domain, string documentListName, int GZID, string documentListURL, bool isEdit)
        {
            try
            {
                InitializeComponent();

                #region 临时存储权限

                IsEdit = isEdit;

                #endregion

                if (!IsEdit)
                {
                    if (GetPermissionEvent != null)
                    {
                        GetPermissionEvent(ref IsEdit);
                    }
                }
                if (IsEdit)
                {

                    #region 设置临时数据（列表名称,ID号）

                    this.GzID = Convert.ToString(GZID);
                    this.ListName = documentListName;

                    #endregion

                    //选择文件
                    this.SelectFiles();
                    //释放资源
                    this.Closed += new EventHandler(FileUp_Closed);
                    //呈现下载窗体之后开始下载
                    this.ContentRendered += (object sender, EventArgs e) =>
                        {
                            //通过异步委托进行
                            this.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                if (isNeedUpLoad)
                                {
                                    UploadDocument(SiteUri, UserName, PassWord, Domain, documentListName, GZID, documentListURL);
                                }
                            }));
                        };
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "FileUp", ex.ToString(), SiteUri, UserName, PassWord, Domain, documentListName, GZID, documentListURL, isEdit);
            }
            finally
            {
            }
        }

        #endregion

        #region UI事件区域

        /// <summary>
        /// 取消按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btnOk_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 释放内存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void FileUp_Closed(object sender, EventArgs e)
        {
            try
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "FileUp_Closed", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion

        #region 文件上传

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="SiteUri">网站站点</param>
        /// <param name="UserName">用户名</param>
        /// <param name="PassWord">密码</param>
        /// <param name="Domain">域名</param>
        /// <param name="documentListName">文档名称</param>
        /// <param name="GZID">文档的ID号</param>
        /// <param name="documentListURL">文档的路径</param>
        public void UploadDocument(string SiteUri, string UserName, string PassWord, string Domain, string documentListName, int GZID, string documentListURL)
        {
            try
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {

                    using (ol.ClientContext clientContext = new ol.ClientContext(SiteUri))
                    {
                        clientContext.Credentials = new NetworkCredential(UserName, PassWord, Domain);
                        ol.List documentsList = clientContext.Web.Lists.GetByTitle(documentListName);
                        clientContext.Load(documentsList);

                        #region 上载文件信息初始化（datagrid）

                        this.DatagridInit(listDocuments);

                        #endregion

                        //后台工作者，实时更新ui控件值
                        BackgroundWorker worker = new BackgroundWorker();
                        //更新事件    
                        worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
                        //更新完毕
                        worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
                        //BackgroundWorker.WorkerReportsProgress 获取或设置一个值，该值指示 BackgroundWorker 能否报告进度更新。
                        worker.WorkerReportsProgress = true;
                        //后台工作者
                        worker.DoWork += (obj, e) =>
                        {
                            fileParts.Clear();
                            ////根据文件总长度设置块的大小
                            if (((lonSize / 1024) / 500) < 200)  //如果总文件数小于200个
                            {
                                BURRFERLEN = 512000;
                            }
                            else
                            {
                                BURRFERLEN = 1024000;
                            }

                            #region 循环上传文件
                            for (int i = 0; i < listDocuments.Count; i++)
                            {
                                //每次上传一个文件更新一次
                                ((BackgroundWorker)obj).ReportProgress(i);

                                var fileCreationInformation = new ol.FileCreationInformation();

                                //判断 文件的大小
                                #region 执行分块文件上传方法

                                if (listDocuments[i].BytFilebyte.Length >= PartLongh)
                                {
                                    //分好块，得到sum块
                                    int filepartCount = (int)Math.Ceiling((double)listDocuments[i].BytFilebyte.Length / BURRFERLEN);

                                    filePartAllCount += filepartCount;


                                    byte[] filePart = new byte[BURRFERLEN];
                                    //byte[] partEnd;

                                    for (int k = 1; k <= filepartCount; k++)
                                    {
                                        //将文件块的名称添加到准备给后台传的参数里
                                        fileParts.Add("TEMP" + (k - 1).ToString() + "_." + listDocuments[i].StrMingCheng);

                                        //最后一段
                                        if (k == filepartCount)
                                        {
                                            filePart = new byte[listDocuments[i].BytFilebyte.Length - BURRFERLEN * (filepartCount - 1)];
                                            Array.Copy(listDocuments[i].BytFilebyte, BURRFERLEN * (k - 1), filePart, 0, filePart.Length);
                                        }
                                        else
                                            if (k == 1)
                                            {
                                                Array.Copy(listDocuments[i].BytFilebyte, BURRFERLEN * (k - 1), filePart, 0, BURRFERLEN);
                                            }
                                            else
                                            {
                                                Array.Copy(listDocuments[i].BytFilebyte, BURRFERLEN * (k - 1), filePart, 0, BURRFERLEN);
                                            }
                                        #region `fileCreationInformation设置

                                        //文件流
                                        fileCreationInformation.Content = filePart;
                                        //允许文档覆盖 
                                        fileCreationInformation.Overwrite = true;
                                        //文件名称
                                        fileCreationInformation.Url = SiteUri + documentListURL + GZID + "/" + "TEMP" + (k - 1).ToString() + "_." + listDocuments[i].StrMingCheng;

                                        #endregion

                                        //加载并更新列表
                                        clientContext.Load(documentsList);
                                        clientContext.ExecuteQuery();

                                        //生成加载文件
                                        ol.File uploadFile = documentsList.RootFolder.Files.Add(fileCreationInformation);

                                        //加载并更新文件
                                        clientContext.Load(uploadFile);
                                        clientContext.ExecuteQuery();
                                    }
                                }
                                #endregion


                                #region 执行小文件直接上传方法


                                else  //小于3M直接提交
                                {

                                    //指定内容 byte[]数组，这里是 documentStream 
                                    fileCreationInformation.Content = listDocuments[i].BytFilebyte;
                                    //允许文档覆盖 
                                    fileCreationInformation.Overwrite = true;
                                    //文件名称
                                    fileCreationInformation.Url = SiteUri + documentListURL + GZID + "/" + "txddd" + "_" + TimeNow + "_" + listDocuments[i].StrMingCheng;

                                    //加载并更新列表
                                    clientContext.Load(documentsList);
                                    clientContext.ExecuteQuery();

                                    //生成加载文件
                                    ol.File uploadFile = documentsList.RootFolder.Files.Add(fileCreationInformation);

                                    //加载并更新文件
                                    clientContext.Load(uploadFile);
                                    clientContext.ExecuteQuery();
                                }
                                #endregion
                            }
                            #endregion

                            //通过判断上传文件中是否有大于1M的文件,决定是否启用大文件合并方法
                            #region 上传完成后处理情况
                            if (fileParts.Count > 0)
                            {
                                Thread thread附件合并 = new Thread(SumFile);
                                //根据文件大小判断 阻塞时间
                                int s = fileParts.Count / PartLongh;
                                Thread.Sleep(s * 1000);
                                thread附件合并.Start();
                            }
                            else  //没有大文件
                            {
                                //调用完成设置信息  并同时调用 设置isFirst方法
                                SetInfo();
                            }
                            #endregion
                        };
                        //后台工作者开始工作（开始执行DoWork）   
                        worker.RunWorkerAsync();
                    }
                }));
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "UploadDocument", ex.ToString(), SiteUri, UserName, PassWord, Domain, documentListName, GZID, documentListURL);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 更新时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                //第几个文件
                int documentIndex = e.ProgressPercentage;
                //滚动条加载
                this.progressBar1.Value += listDocuments[documentIndex].BytFilebyte.Length;

                //开始上传状态
                FileInformationLbList[documentIndex].TemUpLoadStatu = (ControlTemplate)this.Resources["InTemplate"];
                //开始上传状态
                if (documentIndex > 0)
                {
                    FileInformationLbList[documentIndex - 1].TemUpLoadStatu = (ControlTemplate)this.Resources["CompleateTemplate"];
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "worker_ProgressChanged", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                //更新完毕
                this.progressBar1.Value = this.progressBar1.Maximum;
                //已上传的文件数量
                this.txtCompleteCount.Text = FileInformationLbList.Count.ToString();
                //在有数据的情况下开启上传的状态标示
                if (FileInformationLbList.Count > 0)
                {
                    FileInformationLbList[FileInformationLbList.Count - 1].TemUpLoadStatu = (ControlTemplate)this.Resources["CompleateTemplate"];
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "worker_RunWorkerCompleted", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion

        #region 生成上传清单

        /// <summary>
        /// 初始化表格
        /// </summary>
        /// <param name="max">整体文件的大小</param>
        /// <param name="one">每一块的大小</param>
        void DatagridInit(List<DocumentLb> documents)
        {
            try
            {
                //设置文件大小
                documents.ForEach(item => lonSize += item.BytFilebyte.Length);

                //遍历每一个文件
                foreach (var document in documents)
                {
                    //获取文件名称
                    string fileName = document.StrMingCheng;
                    //文件扩展名
                    string kuoZhanMing = fileName.Substring(fileName.LastIndexOf(".") + 1, (fileName.Length - fileName.LastIndexOf(".") - 1));

                    //文件的长度（小于0.1MB的用KB作为单位，否则用MB为单位）
                    string fileLength = null;

                    //文件的长度（具体长度）
                    int length = document.BytFilebyte.Length;

                    //小于0.1MB的用KB作为单位
                    if (length > 10486)
                    {
                        fileLength = GetMBString(length);
                    }
                    else
                    {
                        //大于0.1MB的直接用MB作为单位
                        fileLength = GetKBString(length);
                    }
                    //开始状态为未完成
                    this.ItemsAdd(new FileInformationLb(kuoZhanMing, fileName, fileLength, (ControlTemplate)this.Resources["NoBeginTemplate"]));
                }
                //最大值设置（整个文件的大小）
                this.progressBar1.Maximum = lonSize;
                //获取总的大小（如果小于0.1MB（10486字节），否则用KB来作为单位）
                if (lonSize > 10486)
                {
                    this.txtAllSize.Text = GetMBString(lonSize);
                }
                else
                {
                    this.txtAllSize.Text = GetKBString(lonSize);
                }
                //显示文件的具体数量
                this.txtAllCount.Text = documents.Count.ToString();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DatagridInit", ex.ToString(), documents);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 添加子项
        /// </summary>
        /// <param name="file">文件</param>
        void ItemsAdd(FileInformationLb file)
        {
            try
            {
                FileInformationLbList.Add(file);
                file.PropertyChanged += new PropertyChangedEventHandler(file_PropertyChanged);
                this.dataGrid1.Items.Add(file);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ItemsAdd", ex.ToString(), file);
            }
            finally
            {
            }
        }

        /// <summary>
        ///属性更改事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void file_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                var sd = (sender as FileInformationLb).TemUpLoadStatu;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "file_PropertyChanged", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion

        #region 辅助方法

        #region 打开对话框选择要上传的文件

        /// <summary>
        /// 选择文件
        /// </summary>
        void SelectFiles()
        {
            try
            {
                listDocuments.Clear();
                OpenFileDialog dialog = new OpenFileDialog();
                //是否允许上传多个文件
                dialog.Multiselect = true;

                List<ListBoxItem> listItems = new List<ListBoxItem>();
                if (dialog.ShowDialog().Value)
                {
                    //确实要上传
                    isNeedUpLoad = true;
                    //当前对话框选择文件的总大小
                    long sumByte = 0;
                    List<FileInfo> listFiles = new List<FileInfo>();
                    //判断文件大小 （不大于98M）
                    foreach (string item in dialog.FileNames)
                    {
                        FileInfo file = new FileInfo(item);
                        listFiles.Add(file);
                        sumByte += file.Length;
                    }
                    //如果文件总大小 大于 98M，则不充许上传
                    listDocuments.ForEach(item => lonSize += item.BytFilebyte.Length);
                    if (sumByte + lonSize > 102760448)
                    {
                        MessageBox.Show("您要上传的文件已超过允许上限 (上限98M),请重新选择文件，或分批上传！");
                    }
                    else
                    {
                        foreach (FileInfo file in listFiles)
                        {

                            // 选择上传的文件 
                            Stream stream = file.OpenRead();
                            stream.Position = 0;
                            byte[] buffer = new byte[stream.Length];
                            //将文件读入字节数组
                            stream.Read(buffer, 0, buffer.Length);
                            String fileExtention = file.Extension;

                            DocumentLb document = new DocumentLb();
                            //直接取文件名
                            document.StrMingCheng = file.Name;
                            document.StrCount = file.Length.ToString();
                            document.BytFilebyte = buffer;

                            ListBoxItem item = new ListBoxItem();
                            item.Content = file.Name;

                            listItems.Add(item);

                            listDocuments.Add(document);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "SelectFiles", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion

        #region 合并文件方法

        void SumFile()
        {
            try
            {
                if (SumFileEvent != null)
                {
                    //激发合并文件的事件
                    SumFileEvent(ListName, GzID, filePartAllCount.ToString());
                    SetInfo();
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "SumFile", ex.ToString());
            }
            finally
            {
            }
        }
        #endregion

        #region 上传文件完成提示方法

        /// <summary>
        /// 上传文件完成提示方法
        /// </summary>
        void SetInfo()
        {
            try
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    //总大小已完成
                    this.txtAllSize.Text = "总大小: " + (lonSize / 1024).ToString() + "KB/ 已处理:"
                                    + (lonSize / 1024).ToString() + "KB;" + " 已全部上载并处理完成";
                    //字体变为红色
                    this.txtAllSize.Foreground = new SolidColorBrush(Colors.Red);
                    //标记完成
                    this.Tbinfo.Text = "已完成!";

                    #region 按钮设置

                    //ok按钮可以使用
                    this.btnOk.IsEnabled = true;
                    //按钮文本显示
                    this.btnOk.Content = "  确 定 ";

                    #endregion

                    //停止动画旋转
                    this.StaticSync.Visibility = Visibility.Collapsed;

                    //if (!isEdit)
                    //{
                    //    集合加入故障列表权限(intGZID, new Dictionary<string, string>() { { ProxyListLb.StrUserName, "删除" } });
                    //    集合加入故障列表权限(intGZID, new Dictionary<string, string>() { { ProxyListLb.StrUserName, "读取" } });
                    //}

                    if (this.DeletePermissionEvent != null)
                    {
                        this.DeletePermissionEvent(ref IsEdit);
                    }
                }));
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "SetInfo", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion

        #region 字节流换算方法

        /// <summary>
        /// 将字节转为MB
        /// </summary>
        /// <param name="length">字节长度</param>
        /// <returns></returns>
        string GetMBString(long length)
        {
            string result = string.Empty;
            try
            {
                result = string.Format("{0:F}MB", ((float)length / (1024 * 1024)));
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "GetMBString", ex.ToString(), length);
            }
            finally
            {
            }
            return result;
        }

        /// <summary>
        /// 将字节转为KB
        /// </summary>
        /// <param name="length">字节长度</param>
        /// <returns></returns>
        string GetKBString(long length)
        {
            string result = string.Empty;
              try
            {
            result = string.Format("{0:F}KB", ((float)length / 1024));
                    }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "GetKBString", ex.ToString(), length);
            }
            finally
            {
            }
            return result;
        }

        #endregion

        #region 合并文件（旧方案）

        //try
        //{
        //    if (isFilePartsUpLoadFinish == false)
        //    {
        //        webParms.Clear();
        //        foreach (var item in ProxyListLb.dicSFMParm)
        //        {
        //            webParms.Add(item.Key, item.Value);
        //        }
        //        webParms.Add("ListName", "TXListGZ");
        //        webParms.Add("ItemID", GZID.ToString());
        //        webParms.Add("FileSCount", (filePartAllCount).ToString());

        //        ProxyListLb.Proxy.dirFCompleted += new EventHandler<dirFCompletedEventArgs>(Proxy_FileSumCompleted);
        //        ProxyListLb.Proxy.dirFAsync("大文件合并", webParms);
        //    }
        //}
        //catch (Exception ex)
        //{
        //    MethodLb.CreateLog(this.GetType().FullName, "SumFile", ex.ToString());
        //}
        //finally
        //{
        //}
        //}

        //void Proxy_FileSumCompleted(object sender, dirFCompletedEventArgs e)
        //{
        //    ProxyListLb.Proxy.dirFCompleted -= Proxy_FileSumCompleted;
        //    //出现错误的情况
        //    if (e.Error == null)
        //    {
        //        this.Dispatcher.BeginInvoke(new Action(() =>
        //        {
        //            //取得返回的字节数
        //            //string bityLong = (long.Parse(e.Result.ToString().Split('_')[0]) / 1024).ToString();                   

        //            if (e.Result.Count > 0 && !Convert.ToBoolean(e.Result.Values.ElementAt(0)))
        //            {
        //                isFilePartsUpLoadFinish = false;
        //                this.txtAllSize.Text = "正在处理中-----  总大小:  " + (size / 1024).ToString() + "KB";
        //                //this.txtIsFinish.Text = "/已处理:" + bityLong + "KB";
        //                this.txtIsFinish.Foreground = new SolidColorBrush(Colors.Red);
        //                SumFile();
        //            }
        //            else
        //            {
        //                isFilePartsUpLoadFinish = true;

        //                this.txtAllSize.Text = "总大小:" + (size / 1024).ToString() + "KB / 已处理:"
        //                    + (size / 1024).ToString() + "KB;" + " 已全部上载并处理完成";

        //                this.txtAllSize.Foreground = new SolidColorBrush(Colors.Red);
        //                this.Tbinfo.Text = "已完成!";
        //                this.txtIsFinish.Visibility = Visibility.Collapsed;

        //                #region 按钮设置

        //                this.btnOk.IsEnabled = true;
        //                this.btnOk.Content = "     确      定    ";

        //                #endregion

        //                this.StaticSync.Visibility = Visibility.Collapsed;
        //if (!isEdit)
        //{
        //    集合加入故障列表权限(intGZID, new Dictionary<string, string>() { { ProxyListLb.StrUserName, "删除" } });
        //    集合加入故障列表权限(intGZID, new Dictionary<string, string>() { { ProxyListLb.StrUserName, "读取" } });
        //}
        //                MessageBox.Show("附件上传完成!");
        //            }
        //        }));
        //}

        //    else
        //    {
        //        this.Dispatcher.BeginInvoke(new Action(() =>
        //        {
        //            isFilePartsUpLoadFinish = true;
        //            MessageBox.Show("文件上载失败,请重新上传!");
        //        }));
        //    }
        //}
        #endregion

        #region 集合加入故障列表权限（旧方案）

        /// <summary>
        /// 同步更改权限
        /// </summary>
        /// <param name="intGZID"></param>
        /// <param name="listQuanXianReasult"></param>
        protected void 集合加入故障列表权限(int intGZID, Dictionary<string, string> listQuanXianReasult)
        {
            try
            {
                //webParms.Clear();
                //foreach (var item in ProxyListLb.dicSFMParm)
                //{
                //    webParms.Add(item.Key, item.Value);
                //}
                //webParms.Add("ListName", "TXListGZ");
                //webParms.Add("CollItemsID", intGZID.ToString());
                //string strQuanXian = string.Empty;
                //foreach (var item in listQuanXianReasult)
                //{
                //    strQuanXian += ";" + item.Key + "," + item.Value;
                //}
                //strQuanXian = strQuanXian.Substring(1);
                //webParms.Add("UsersAndPermissions", strQuanXian);

                //try
                //{
                //    var result = ProxyListLb.Proxy.dirF("设置Item用户权限", webParms);
                //}
                //catch (Exception)
                //{
                //    MessageBox.Show("权限设置失败");
                //    return;
                //}
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "FormPublic_集合加入故障列表权限", ex.ToString(), intGZID, listQuanXianReasult);
            }
            finally
            {
            }
        }

        #endregion

        #endregion
    }
}
