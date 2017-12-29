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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;
using System.Net;
using System.Diagnostics;
using System.Collections;
using MhczTBG.Common;
using Microsoft.SharePoint.Client;
using MhczTBG.Common.SharePointBook;

namespace MhczTBG.Helper
{
    /// <summary>
    /// FHDataGrid.xaml 的交互逻辑
    /// </summary>
    public partial class DocumentDataGrid : UserControl
    {
        #region 声明变量

        string fatherFolderName = null;

        /// <summary>
        /// 快捷字符串
        /// </summary>
        //string strQian = Environment.CurrentDirectory.Replace("bin\\Debug", "Image\\");

        List<DataGridItemLb> dataList = new List<DataGridItemLb>();

        string webSiteUri = null;

        /// 菜单(菜单宽度)
        /// </summary>
        ContextMenu menu = new ContextMenu() { Width = 150 };

        ClientContextMethod clientMethod = null;

        #endregion

        #region 构造函数

        public DocumentDataGrid()
        {
            InitializeComponent();
        }

        public DocumentDataGrid(ClientContextMethod client, Microsoft.SharePoint.Client.List list, string folderName, string webSiteUrl)
        {
            InitializeComponent();

            ListType_ParemesInit(client, list);
            this.fatherFolderName = folderName;
            this.clientMethod = client;
            webSiteUri = webSiteUrl;
        }

        public DocumentDataGrid(ClientContextMethod client, Microsoft.SharePoint.Client.FileCollection fileCollection, string folderName, string webSiteUrl)
        {
            InitializeComponent();
            FileType_ParemesInit(client, fileCollection);
            this.fatherFolderName = folderName;
            this.clientMethod = client;
            this.webSiteUri = webSiteUrl;

            //书本快捷子菜单
            TbgMenuItem menuItemRemoveFile = new TbgMenuItem("删除文档", "分配任务");
            //书架快捷子菜单
            TbgMenuItem menuItemDowndLoad = new TbgMenuItem("下载该文档", "查看详情");
            //书架快捷子菜单
            TbgMenuItem menuItemCheckOut = new TbgMenuItem("签出该文档", "查看详情");
            //书架快捷子菜单
            TbgMenuItem menuItemCheckIn = new TbgMenuItem("签入该文档", "查看详情");
          
            //菜单加载子项
            menu.Items.Add(menuItemRemoveFile);
            //菜单加载子项
            menu.Items.Add(menuItemDowndLoad);
            //菜单加载子项
            menu.Items.Add(menuItemCheckOut);
            //菜单加载子项
            menu.Items.Add(menuItemCheckIn);

            //删除文档
            menuItemRemoveFile.Click += new RoutedEventHandler(menuItemRemoveFile_Click);
            //下载文档
            menuItemDowndLoad.Click += new RoutedEventHandler(menuItemDowndLoad_Click);
            //签出文档
            menuItemCheckOut.Click += new RoutedEventHandler(menuItemCheckOut_Click);
            //签入文档
            menuItemCheckIn.Click += new RoutedEventHandler(menuItemCheckIn_Click);
        }

        #region UI事件区域


        /// <summary>
        /// 删除文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void menuItemRemoveFile_Click(object sender, RoutedEventArgs e)
        {
            var d = this.datagrid.SelectedItem as DataGridItemLb;

            //MessageBox.Show("是否删除"+d ,
            this.clientMethod.DeleFile(fatherFolderName, d.str1);
            this.datagrid.Items.RemoveAt(this.datagrid.SelectedIndex);
        }

        /// <summary>
        /// 下载该文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void menuItemDowndLoad_Click(object sender, RoutedEventArgs e)
        {
            var d = this.datagrid.SelectedItem as DataGridItemLb;
            this.clientMethod.DownloadDocument( webSiteUri, fatherFolderName, d.str1);
        }

        /// <summary>
        /// 签出文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void menuItemCheckOut_Click(object sender, RoutedEventArgs e)
        {
            var d = this.datagrid.SelectedItem as DataGridItemLb;
            //签出文档
            string result = this.clientMethod.CheckOut(fatherFolderName, d.str1);
            //提示操作信息
            MessageBox.Show(result);           
        }

        /// <summary>
        /// 签入该文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void menuItemCheckIn_Click(object sender, RoutedEventArgs e)
        {
            var d = this.datagrid.SelectedItem as DataGridItemLb;
            //签出文档
            string result = this.clientMethod.CheckIn( fatherFolderName, d.str1,"");
            //提示操作信息
            MessageBox.Show(result);        
        }

        #endregion

        #endregion

        #region 初始化数据

        Dictionary<string, string> dicTittleList = new Dictionary<string, string>();

        Dictionary<string, string> dicIndentList = new Dictionary<string, string>();

        public void ListType_ParemesInit(ClientContextMethod client, Microsoft.SharePoint.Client.List list)
        {
            this.dicTittleList.Clear();
            this.dicIndentList.Clear();
            this.datagrid.Columns.Clear();
            this.datagrid.Items.Clear();

           
            client.LoadMethod( list);
            Microsoft.SharePoint.Client.ListItemCollection collec = list.GetItems(new CamlQuery());
            client.LoadMethod(collec);
            if (collec.Count > 0)
            {
                client.LoadMethod( collec[0].ContentType.Fields);
                for (int i = 1; i < collec[0].ContentType.Fields.Count; i++)
                {
                    string title = collec[0].ContentType.Fields[i].Title;
                    if (!dicTittleList.ContainsKey(title))
                    {
                        dicTittleList.Add(title, "str" + i);
                    }
                    if (!dicIndentList.ContainsKey(title))
                    {
                        dicIndentList.Add(title, collec[0].ContentType.Fields[i].StaticName);
                    }                   
                }
                TitleInit(dicTittleList);


                ListType_DataInit(collec);
            }
            //string ad = string.Empty;
            //foreach (var item in dicIndentList)
            //{
            //    ad += string.Format("result[”{0}“].ToString()",item.Value) + item.Key + "；";
            //}
        }

        public void FileType_ParemesInit(ClientContextMethod client, Microsoft.SharePoint.Client.FileCollection fileCollection)
        {

            this.dicTittleList.Clear();
            this.datagrid.Columns.Clear();

            client.LoadMethod( fileCollection);
            dicTittleList.Add("文件名称", "str1");
            dicTittleList.Add("创建人", "str2");
            dicTittleList.Add("编辑人", "str3");
            dicTittleList.Add("创建时间", "str4");
            dicTittleList.Add("文件类型", "str5");
            dicTittleList.Add("最新版本号", "str6");
            dicTittleList.Add("版本数量", "str7");
            dicTittleList.Add("最后一次修改时间", "str8");

            TitleInit(dicTittleList);

            FileType_DataInit(client, fileCollection);
        }

        #endregion

        #region 设置标题

        /// <summary>
        /// 设置标题（key为标题名称,value为属性的名称）
        /// </summary>
        /// <param name="dicTittles"></param>
        void TitleInit(Dictionary<string, string> dicTittles)
        {
            //清楚列表column
            this.datagrid.Columns.Clear();
            //循环设置标题
            foreach (var item in dicTittles)
            {
                //创建一个Column
                DataGridTextColumn column = new DataGridTextColumn();
                //可以排列
                column.CanUserSort = true;
                //可以拖动
                column.CanUserReorder = true;
                //设置标题
                column.Header = item.Key;
                //标题绑定
                column.Binding = new Binding(item.Value);
                //添加标题
                datagrid.Columns.Add(column);
            }
        }

        #endregion

        #region 生成数据

        void ListType_DataInit(Microsoft.SharePoint.Client.ListItemCollection collec)
        {

            for (int i = 0; i < collec.Count; i++)
            {
                int propertyCount = 0;
                DataGridItemLb lb = new DataGridItemLb();
                string ad = string.Empty;
                PropertyInfo[] propertyes = lb.GetType().GetProperties();
                for (int j = 0; j < collec[i].FieldValues.Count; j++)
                {
                    if (dicIndentList.ContainsValue(collec[i].FieldValues.Keys.ElementAt(j)))
                    {
                        if (collec[i].FieldValues.Values.ElementAt(j) == null)
                        {
                            propertyes[propertyCount].SetValue(lb, string.Empty, null);
                            propertyCount++;
                        }
                        else
                        {
                            propertyes[propertyCount].SetValue(lb, collec[i].FieldValues.Values.ElementAt(j).ToString(), null);
                            propertyCount++;
                        }
                    }                   
                }

                dataList.Add(lb);
                this.datagrid.Items.Add(lb);
            }
        }

        void FileType_DataInit(ClientContextMethod client, Microsoft.SharePoint.Client.FileCollection fileCollection)
        {

            foreach (var file in fileCollection)
            {
                DataGridItemLb lb = new DataGridItemLb();
                PropertyInfo[] propertyes = lb.GetType().GetProperties();

                //加载文档创建者
                client.LoadMethod( file.Author);
                //加载文档修改者
                client.LoadMethod( file.ModifiedBy);
                //加载文档版本控制
                client.LoadMethod( file.Versions);

                propertyes[0].SetValue(lb, file.Name, null);
                propertyes[1].SetValue(lb, file.Author.Title, null);
                propertyes[2].SetValue(lb, file.ModifiedBy.Title, null);
                propertyes[3].SetValue(lb, file.TimeCreated.ToString(), null);
                propertyes[4].SetValue(lb, System.IO.Path.GetExtension(file.ServerRelativeUrl).Replace(".", ""), null);
                propertyes[5].SetValue(lb, file.UIVersionLabel, null);
                propertyes[6].SetValue(lb, file.Versions.Count.ToString(), null);
                propertyes[7].SetValue(lb, file.TimeLastModified.ToString(), null);

                dataList.Add(lb);
                this.datagrid.Items.Add(lb);
            }
        }

        #endregion


        private void DGR_Border_Loaded(object sender, RoutedEventArgs e)
        {
            ContextMenuService.SetContextMenu(sender as FrameworkElement, menu);
        }
    }
}
