using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace MhczTBG.Common
{
    public class DataOperation
    {
        static ClientContext clientContext = null;

        #region 获取表单所有数据

        /// <summary>
        /// 通过查询语句获取表单所有数据
        /// </summary>
        /// <param name="website">站点地址</param>
        /// <param name="listName">指定列表名称</param>
        /// <param name="querystring">指定查询语句</param>
        /// <returns>返回子项的集合</returns>
        // public List<Dictionary<string, object>> ClientGetDic(string listName, Dictionary<string,string> dicCaml)
        public static DataTable ClientGetDic(string listName, Dictionary<string, string> dicCaml)
        {
            List<Dictionary<string, object>> dicList = new List<Dictionary<string, object>>();
            DataTable dt = new DataTable();

            try
            {
                if (clientContext == null) clientContext = new ClientContext(Proxy.SelectedServiceUri + Proxy.WebSite);
                //创建客户端对象模型          
                using (clientContext)
                {
                    //获取列表
                    List oList = clientContext.Web.Lists.GetByTitle(listName);

                    #region 生成筛选条件

                    QueryCaml query = new QueryCaml();

                    //转换为list
                    List<QueryCaml> querylist = query.dirToListCamll(dicCaml);
                    //转换为query语句
                    string querystring = query.GetQuery(querylist);

                    CamlQuery camlQuery = new CamlQuery();
                    camlQuery.ViewXml = "<View><Query><Where>" + querystring + "</Where></Query></View>";

                    //获取当前列表的所有项
                    Microsoft.SharePoint.Client.ListItemCollection collListItem = oList.GetItems(camlQuery);

                    ////筛选
                    //clientContext.Load(
                    //   collListItem,
                    //   items => items.Include(
                    //         item => item["DingXing"],
                    //           item => item["DingZe"],
                    //             item => item["CheJianMingCheng"],
                    //               item => item["JuDing"],
                    //   item => item["DuanDing"],
                    //   item => item["ShebeiTypeYiJi"]));

                    #endregion

                    //执行
                    clientContext.Load(collListItem);
                    clientContext.ExecuteQuery();

                    int i = 0;

                    List<object> obj = new List<object>();
                    //生成DataTable
                    foreach (ListItem item in collListItem)
                    {
                        if (i == 0)
                        {
                            //指定列标题
                            foreach (System.Collections.Generic.KeyValuePair<string, object> dr in item.FieldValues)
                            {
                                if (dr.Key.Equals("YanShi") || dr.Key.Equals("JingJiSunShi") || dr.Key.Equals("ZeRenDanWeiChengDanSunShiFeiYong") || dr.Key.Equals("YingXiangHuoCheShu") || dr.Key.Equals("YingXiangKeCheShu"))
                                {
                                    dt.Columns.Add(dr.Key, typeof(int));
                                }
                                else if (dr.Key.Equals("startData"))
                                    dt.Columns.Add(dr.Key, typeof(DateTime));
                                else
                                {
                                    dt.Columns.Add(dr.Key);
                                }
                            }
                            i++;
                        }

                        //获取对应字段的值
                        object[] fiedscoll = new object[dt.Columns.Count];
                        for (int j = 0; j < item.FieldValues.Count; j++)
                        {
                            fiedscoll[j] = item.FieldValues.Values.ElementAt(j);
                            obj.Add(fiedscoll[j]);
                        }
                        //dt添加一条数据
                        dt.Rows.Add(fiedscoll);
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(typeof(DataOperation).FullName, "ClientGetDic", ex.ToString(), listName, dicCaml);
            }
            finally
            {
            }
            return dt;
        }

        /// <summary>
        /// 通过查询语句获取表单所有数据
        /// </summary>
        /// <param name="website">站点地址</param>
        /// <param name="listName">指定列表名称</param>
        /// <param name="querystring">指定查询语句</param>
        /// <returns>返回子项的集合</returns>
        // public List<Dictionary<string, object>> ClientGetDic(string listName, Dictionary<string,string> dicCaml)
        public static DataTable ClientGetDic(string listName, Dictionary<string, string> dicCaml,ref List<Dictionary<string, object>> dicResult)
        {
            List<Dictionary<string, object>> dicList = new List<Dictionary<string, object>>();
            DataTable dt = new DataTable();

            try
            {
                if (clientContext == null) clientContext = new ClientContext(Proxy.SelectedServiceUri + Proxy.WebSite);
                //创建客户端对象模型          
                using (clientContext)
                {
                    //获取列表
                    List oList = clientContext.Web.Lists.GetByTitle(listName);

                    #region 生成筛选条件

                    QueryCaml query = new QueryCaml();

                    //转换为list
                    List<QueryCaml> querylist = query.dirToListCamll(dicCaml);
                    //转换为query语句
                    string querystring = query.GetQuery(querylist);

                    CamlQuery camlQuery = new CamlQuery();
                    camlQuery.ViewXml = "<View><Query><Where>" + querystring + "</Where></Query></View>";

                    //获取当前列表的所有项
                    Microsoft.SharePoint.Client.ListItemCollection collListItem = oList.GetItems(camlQuery);

                    ////筛选
                    //clientContext.Load(
                    //   collListItem,
                    //   items => items.Include(
                    //         item => item["DingXing"],
                    //           item => item["DingZe"],
                    //             item => item["CheJianMingCheng"],
                    //               item => item["JuDing"],
                    //   item => item["DuanDing"],
                    //   item => item["ShebeiTypeYiJi"]));

                    #endregion

                    //执行
                    clientContext.Load(collListItem);
                    clientContext.ExecuteQuery();

                    int i = 0;

                    List<object> obj = new List<object>();
                    //生成DataTable
                    foreach (ListItem item in collListItem)
                    {
                        dicList.Add(item.FieldValues);
                        if (i == 0)
                        {
                            //指定列标题
                            foreach (System.Collections.Generic.KeyValuePair<string, object> dr in item.FieldValues)
                            {
                                if (dr.Key.Equals("YanShi") || dr.Key.Equals("JingJiSunShi") || dr.Key.Equals("ZeRenDanWeiChengDanSunShiFeiYong") || dr.Key.Equals("YingXiangHuoCheShu") || dr.Key.Equals("YingXiangKeCheShu"))
                                {
                                    dt.Columns.Add(dr.Key, typeof(int));
                                }
                                else if (dr.Key.Equals("startData"))
                                    dt.Columns.Add(dr.Key, typeof(DateTime));
                                else
                                {
                                    dt.Columns.Add(dr.Key);
                                }
                            }
                            i++;
                        }

                        //获取对应字段的值
                        object[] fiedscoll = new object[dt.Columns.Count];
                        for (int j = 0; j < item.FieldValues.Count; j++)
                        {
                            fiedscoll[j] = item.FieldValues.Values.ElementAt(j);
                            obj.Add(fiedscoll[j]);
                        }
                        //dt添加一条数据
                        dt.Rows.Add(fiedscoll);
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(typeof(DataOperation).FullName, "ClientGetDic", ex.ToString(), listName, dicCaml);
            }
            finally
            {
            }

            dicResult = dicList;
            return dt;
        }

        /// <summary>
        /// 通过查询语句获取表单所有数据
        /// </summary>
        /// <param name="website">站点地址</param>
        /// <param name="listName">指定列表名称</param>
        /// <param name="querystring">指定查询语句</param>
        /// <returns>返回子项的集合</returns>
        // public List<Dictionary<string, object>> ClientGetDic(string listName, Dictionary<string,string> dicCaml)
        public static DataTable ClientGetDicByPropertyName(string listName, Dictionary<string, string> dicCaml,string propertyName, ref List<Dictionary<string, object>> dicResult)
        {
            List<Dictionary<string, object>> dicList = new List<Dictionary<string, object>>();
            DataTable dt = new DataTable();

            try
            {
                if (clientContext == null) clientContext = new ClientContext(Proxy.SelectedServiceUri + Proxy.WebSite);
                //创建客户端对象模型          
                using (clientContext)
                {
                    //获取列表
                    List oList = clientContext.Web.Lists.GetByTitle(listName);

                    #region 生成筛选条件

                    QueryCaml query = new QueryCaml();

                    //转换为list
                    List<QueryCaml> querylist = query.dirToListCamll(dicCaml);
                    //转换为query语句
                    string querystring = query.GetQuery(querylist);

                    CamlQuery camlQuery = new CamlQuery();
                    camlQuery.ViewXml = "<View><Query><Where>" + querystring + "</Where></Query></View>";

                    //获取当前列表的所有项
                    Microsoft.SharePoint.Client.ListItemCollection collListItem = oList.GetItems(camlQuery);

                    //筛选
                    clientContext.Load(
                       collListItem,
                       items => items.Include(
                             item => item[propertyName],
                             item => item["ID"]
                               ));

                    #endregion

                    //执行
                    //clientContext.Load(collListItem);
                    clientContext.ExecuteQuery();

                    int i = 0;
                  
                    //生成DataTable
                    foreach (ListItem item in collListItem)
                    {
                        dicList.Add(item.FieldValues);
                        if (i == 0)
                        {
                            int j = 0;
                            //指定列标题
                            foreach (System.Collections.Generic.KeyValuePair<string, object> dr in item.FieldValues)
                            {
                              
                                if (dr.Key.Equals("startData"))
                                {
                                    dt.Columns.Add(dr.Key, typeof(DateTime));
                                    j++;
                                }   
                                if(dr.Key.Equals("ID"))
                                {
                                     dt.Columns.Add(dr.Key);
                                    j++;
                                }
                                if (j == 2) break;
                            }
                            i++;
                        }

                        //获取对应字段的值
                       object fiedscoll  = item.FieldValues[propertyName];

                        object fieldSoll2  = item.FieldValues["ID"];
                      
                        //dt添加一条数据
                        dt.Rows.Add(fiedscoll, fieldSoll2);
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(typeof(DataOperation).FullName, "ClientGetDic", ex.ToString(), listName, dicCaml);
            }
            finally
            {
            }

            dicResult = dicList;
            return dt;
        }

        /// <summary>
        /// 通过查询语句获取表单所有数据
        /// </summary>
        /// <param name="website">站点地址</param>
        /// <param name="listName">指定列表名称</param>
        /// <param name="querystring">指定查询语句</param>
        /// <returns>返回子项的集合</returns>
        // public List<Dictionary<string, object>> ClientGetDic(string listName, Dictionary<string,string> dicCaml)
        public static List<Dictionary<string, object>> ClientGetDataGridDic(string listName, Dictionary<string, string> dicCaml)
        {
            List<Dictionary<string, object>> dicList = new List<Dictionary<string, object>>();

            try
            {
                if (clientContext == null) clientContext = new ClientContext(Proxy.SelectedServiceUri + Proxy.WebSite);
                //创建客户端对象模型          
                using (clientContext)
                {
                    //获取列表
                    List oList = clientContext.Web.Lists.GetByTitle(listName);

                    #region 生成筛选条件

                    QueryCaml query = new QueryCaml();

                    //转换为list
                    List<QueryCaml> querylist = query.dirToListCamll(dicCaml);
                    //转换为query语句
                    string querystring = query.GetQuery(querylist);

                    CamlQuery camlQuery = new CamlQuery();
                    camlQuery.ViewXml = "<View><Query><Where>" + querystring + "</Where></Query></View>";

                    //获取当前列表的所有项
                    Microsoft.SharePoint.Client.ListItemCollection collListItem = oList.GetItems(camlQuery);


                    #endregion

                    //执行
                    clientContext.Load(collListItem);
                    clientContext.ExecuteQuery();

                    foreach (var item in collListItem)
                    {
                        dicList.Add(item.FieldValues);
                    }


                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(typeof(DataOperation).FullName, "ClientGetDataGridDic", ex.ToString(), listName, dicCaml);
            }
            finally
            {
            }
            return dicList;
        }

        #endregion

        #region 指定列表获取标题

        /// <summary>
        /// 获取标题集合
        /// </summary>
        /// <param name="website">站点</param>
        /// <param name="listName">列表名称</param>
        /// <returns></returns>
        public static List<string> GetTittleList(string listName)
        {
            List<string> tittleList = new List<string>();

            if (clientContext == null) clientContext = new ClientContext(Proxy.SelectedServiceUri + Proxy.WebSite);
            if (clientContext.Url != Proxy.SelectedServiceUri + Proxy.WebSite)
            {

                using (clientContext = new ClientContext(Proxy.SelectedServiceUri + Proxy.WebSite))
                {

                    clientContext.Credentials = new System.Net.NetworkCredential(Proxy.UserName, Proxy.Password, Proxy.Domain);
                    //获取列表
                    List oList = clientContext.Web.Lists.GetByTitle(listName);

                    //获取当前列表的所有项
                    Microsoft.SharePoint.Client.ListItemCollection collListItem = oList.GetItems(new CamlQuery());

                    //执行
                    clientContext.Load(collListItem);
                    clientContext.ExecuteQuery();

                    foreach (var item in collListItem)
                    {
                        tittleList.Add(Convert.ToString(item.FieldValues["Title"]));
                    }
                }
            }
            else
            {
                using (clientContext)
                {

                    clientContext.Credentials = new System.Net.NetworkCredential(Proxy.UserName, Proxy.Password, Proxy.Domain);
                    //获取列表
                    List oList = clientContext.Web.Lists.GetByTitle(listName);

                    //获取当前列表的所有项
                    Microsoft.SharePoint.Client.ListItemCollection collListItem = oList.GetItems(new CamlQuery());

                    //执行
                    clientContext.Load(collListItem);
                    clientContext.ExecuteQuery();

                    foreach (var item in collListItem)
                    {
                        tittleList.Add(Convert.ToString(item.FieldValues["Title"]));
                    }
                }
            }

            return tittleList;
        }

        /// <summary>
        /// 获取标题集合
        /// </summary>
        /// <param name="website">站点</param>
        /// <param name="listName">列表名称</param>
        /// <returns></returns>
        public static Dictionary<string, List<string>> GetTittleList(string listName, params string[] fatherNames)
        {
            Dictionary<string, List<string>> tittleList = new Dictionary<string, List<string>>();

            foreach (var item in fatherNames)
            {
                tittleList.Add(item, new List<string>());
            }

            if (clientContext == null) clientContext = new ClientContext(Proxy.SelectedServiceUri + Proxy.WebSite);

            using (clientContext)
            {
                //获取列表
                List oList = clientContext.Web.Lists.GetByTitle(listName);

                //获取当前列表的所有项
                Microsoft.SharePoint.Client.ListItemCollection collListItem = oList.GetItems(new CamlQuery());

                //执行
                clientContext.Load(collListItem);
                clientContext.ExecuteQuery();

                foreach (var item in collListItem)
                {
                    foreach (var dd in tittleList)
                    {
                        if (dd.Key.Equals(Convert.ToString(item.FieldValues["Father"])))
                        {
                            dd.Value.Add(Convert.ToString(item.FieldValues["Title"]));
                        }
                    }
                }
            }

            return tittleList;
        }

        #endregion

        #region 通过ID获取一项

        public static string GetItemByID(string listName, int ID)
        {
            string result = string.Empty;

            if (clientContext == null) clientContext = new ClientContext(Proxy.SelectedServiceUri + Proxy.WebSite);
            using (clientContext)
            {
                //获取列表
                Microsoft.SharePoint.Client.List oList = clientContext.Web.Lists.GetByTitle(listName);
                //通过ID获取Item
                ListItem listItem = oList.GetItemById(ID);

                //加载
                clientContext.Load(listItem);
                //执行
                clientContext.ExecuteQuery();

                #region 获取指定ID文档下的所有文件名称

                clientContext.Load(oList.RootFolder.Folders);
                clientContext.ExecuteQuery();

                Folder folderGet = null;

                foreach (var item in oList.RootFolder.Folders)
                {
                    if (item.Name.Equals(Proxy.attch))
                    {
                        clientContext.Load(item.Folders);
                        clientContext.ExecuteQuery();

                        foreach (var itemChild in item.Folders)
                        {

                            if (itemChild.Name.Equals(ID.ToString()))
                            {
                                folderGet = itemChild;
                                break;
                            }
                        }
                        break;
                    }
                }

                if (folderGet != null)
                {
                    clientContext.Load(folderGet.Files);
                    clientContext.ExecuteQuery();

                    foreach (var item in folderGet.Files)
                    {
                        var fileName = item.ServerRelativeUrl.Substring(item.ServerRelativeUrl.LastIndexOf('/') + 1);
                        result += fileName + "," + Proxy.SelectedServiceUri + item.ServerRelativeUrl + ";";
                    }
                }

                #endregion
            }
            return result;
        }

        #endregion
    }
}
