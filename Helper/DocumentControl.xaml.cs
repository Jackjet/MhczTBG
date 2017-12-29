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
using MhczTBG.Common;
using Microsoft.SharePoint.Client;

namespace MhczTBG.Helper
{
    /// <summary>
    /// DocumentControl.xaml 的交互逻辑
    /// </summary>
    public partial class DocumentControl : UserControl
    {
        ClientContextMethod client = new ClientContextMethod();
        string webSiteUri = null;

        public DocumentControl()
        {
            InitializeComponent();

            this.Unloaded += new RoutedEventHandler(DocumentControl_Unloaded);
        }

        public DocumentControl(string webUri, string userName, string passWord, string doMain)
        {
            InitializeComponent();

            TreeViewParemsInit(webUri, userName, passWord, doMain);

            this.Unloaded += new RoutedEventHandler(DocumentControl_Unloaded);
        }

        void DocumentControl_Unloaded(object sender, RoutedEventArgs e)
        {

            ClientContextMethod.clientContext.Dispose();
        }

        //TreeViewItem Item = new TreeViewItem();
        public void TreeViewParemsInit(string webSiteUrl, string userName, string passWord, string doMain)
        {
            try
            {
                webSiteUri = webSiteUrl;
                ListCollection collection = client.GetAllLists(webSiteUrl, userName, passWord, doMain);
                var d = collection.Count;
                foreach (var item in collection)
                {
                    client.LoadMethod(item);
                    this.Tree_ItemsAdd(item);
                }
                //};               
            }
            catch (Exception )
            {

                throw;
            }
        }

        public void Tree_ItemsAdd(Microsoft.SharePoint.Client.List list)
        {
            TbgTreeItem item = new TbgTreeItem(list.Title, client, list);
            item.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(item_MouseLeftButtonDown);
            this.treeView.Items.Add(item);
        }

        void item_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Microsoft.SharePoint.Client.List list = (sender as TbgTreeItem).List;

            Microsoft.SharePoint.Client.FileCollection fileCollection = (sender as TbgTreeItem).FileCollection;

            this.txtTittle.Text = (sender as TbgTreeItem).Header.ToString();
            if (list.BaseTemplate.Equals(100))
            {
                DocumentDataGrid dataGrid = new DocumentDataGrid(client, list, this.txtTittle.Text, webSiteUri);
                bordMain.Child = dataGrid;
            }
            else
            {
                DocumentDataGrid dataGrid = new DocumentDataGrid(client, fileCollection, this.txtTittle.Text, webSiteUri);
                dataGrid.datagrid.RowStyle = dataGrid.Resources["MenuGet"] as Style;
                bordMain.Child = dataGrid;
            }
        }

        public void Tree_ItemsRemove()
        {
        }

        public void ChangeLeftView()
        {
        }

        public void FileAdd()
        {
        }

        public void FileRemove()
        {
        }
    }

    class TbgTreeItem : TreeViewItem
    {
        string _parentRoot;
        //
        public string ParentRoot
        {
            get { return _parentRoot; }
            set { _parentRoot = value; }
        }

        FolderCollection _folderCollection;
        /// <summary>
        /// 该树节点所包含的文档库集合
        /// </summary>
        public FolderCollection FolderCollection
        {
            get { return _folderCollection; }
            set { _folderCollection = value; }
        }

        FileCollection fileCollection;
        /// <summary>
        /// 该树节点所包含的文档集合
        /// </summary>
        public FileCollection FileCollection
        {
            get { return fileCollection; }
            set { fileCollection = value; }
        }

        Microsoft.SharePoint.Client.List _list;
        /// <summary>
        /// 该树节点所包含的列表
        /// </summary>
        public Microsoft.SharePoint.Client.List List
        {
            get { return _list; }
            set { _list = value; }
        }

        //public TbgTreeItem(string header)
        //{
        //    this.Header = header;
        //}

        /// <summary>
        /// 构造函数2
        /// </summary>
        /// <param name="header">树节点的标题</param>
        /// <param name="_folderCollection">树节点的文档库集合绑定</param>
        /// <param name="_fileCollection">树节点的文档集合绑定</param>
        //public TbgTreeItem(string header, ClientContextMethod client, FolderCollection _folderCollection, FileCollection _fileCollection)
        //{
        //    this.Header = header;
        //    this.FolderCollection = _folderCollection;
        //    this.FileCollection = _fileCollection;
        //}

        /// <summary>
        /// 构造函数3
        /// </summary>
        /// <param name="header">树节点的标题</param>
        /// <param name="listCollection">列表集合</param>
        public TbgTreeItem(string header, ClientContextMethod client, Microsoft.SharePoint.Client.List list)
        {
            this.Header = header;
            this.FolderCollection = list.RootFolder.Folders;
            this.FileCollection = list.RootFolder.Files;
            this.List = list;

            //client.LoadMethod(client.clientContext, FolderCollection);
            //this.ChildInit(this, client, FolderCollection);           
        }



        //public void ChildInit(TreeViewItem rootItem, ClientContextMethod client, FolderCollection folderCollection)
        //{
        //    foreach (var item in folderCollection)
        //    {
        //        TbgTreeItem ChildItem = new TbgTreeItem(item.Name, client, item.Folders, item.Files);
        //        this.Expanded += (object sender, RoutedEventArgs e) =>
        //        {
        //            client.LoadMethod(client.clientContext, item.Folders);
        //            this.ChildInit(ChildItem, client, item.Folders);
        //        };
        //        rootItem.Items.Add(ChildItem);
        //    }
        //}
    }
}
