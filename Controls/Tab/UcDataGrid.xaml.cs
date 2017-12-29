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
//using RwpLb.Common;
//using SFM.Controls.Media;
//using Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Net;
using System.Diagnostics;
using System.Collections;
using MhczTBG.Common;
//using RwpLb.Controls.File;


namespace MhczTBG.Controls.Tab
{
    /// <summary>
    /// FHDataGrid.xaml 的交互逻辑
    /// </summary>
    public partial class UcDataGrid : UserControl
    {
        #region 自定义事件委托

        public delegate void RowClickEventHandle();
        /// <summary>
        /// 行双击击时激发的事件（内容由自己构建）
        /// </summary>
        public event RowClickEventHandle RowClickEvent = null;

        #endregion

        #region 声明变量

        /// <summary>
        /// 临时数据集
        /// </summary>
        List<object> LinShilist = new List<object>();

        /// <summary>
        /// 数据源
        /// </summary>
        List<object> AllList = new List<object>();

        #endregion

        #region 构造函数

        public UcDataGrid()
        {
            try
            {
                InitializeComponent();

                #region 注册事件区域

                this.txt.TextChanged += new TextChangedEventHandler(txt_TextChanged);

                #endregion
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "UcDataGrid", ex.ToString());
            }
            finally
            {
            }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="list">数据源</param>
        public UcDataGrid(IList list)
            : this()
        {
            try
            {
                //生成数据
                DataInit(list);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "UcDataGrid", ex.ToString(), list);
            }
            finally
            {
            }
        }

        #endregion

        #region 初始化数据
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="list">数据源</param>
        public void DataInit(IList list)
        {
            try
            {
                foreach (var item in list)
                {
                    AllList.Add(item);
                }
                this.datagrid.ItemsSource = this.AllList;

                imgAddItem.Source = CommonMethod.GetImageSource(MhczTBG.Properties.Resources.AddItem);

                imgEdit.Source = CommonMethod.GetImageSource(MhczTBG.Properties.Resources.Edit);

                imgReflesh.Source = CommonMethod.GetImageSource(MhczTBG.Properties.Resources.Reflesh);

                imgSearch.Source = CommonMethod.GetImageSource(MhczTBG.Properties.Resources.Search);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DataInit", ex.ToString(), list);
            }
            finally
            {
            }
        }

        #endregion

        #region 自定义事件激发区域

        /// <summary>
        /// 行点击时激发该事件
        /// </summary>
        public void OnRowClickEvent()
        {
            try
            {
                if (RowClickEvent != null)
                {
                    //内容外部可以指定
                    RowClickEvent();
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "OnRowClickEvent", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion

        #region 查询

        /// <summary>
        /// 点击查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //清空临时收集符合条件的数据集
                LinShilist.Clear();
                //循环给list添加符合条件的数据
                foreach (var item in this.AllList)
                {
                    //获取类型
                    Type type = item.GetType();

                    //获取所有属性集
                    PropertyInfo[] propertyInfoes = type.GetProperties();

                    //遍历属性集
                    foreach (var property in propertyInfoes)
                    {
                        //获取对应属性值
                        object obj = property.GetValue(item, null);
                        if (obj != null)
                        {
                            //判断该属性值,若文本框包含这些内容,则将这条信息加入到临时数据集里
                            string information = obj.ToString();
                            if (information.Contains(this.txt.Text))
                            {
                                LinShilist.Add(item);
                                break;
                            }
                        }
                    }
                }
                this.datagrid.ItemsSource = null;
                this.datagrid.ItemsSource = LinShilist;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "txt_TextChanged", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion

        #region 设置标题
        /// <summary>
        /// 设置标题
        /// </summary>
        /// <param name="type">类型</param>
        public void TitleInit(Type type)
        {
            try
            {
                //清楚列表column
                this.datagrid.Columns.Clear();
                //获取所有标题名称
                List<string> listTittles = GetTittleList(type);
                //循环设置标题
                foreach (var item in listTittles)
                {
                    //创建一个Column
                    DataGridTextColumn column = new DataGridTextColumn();
                    //可以排列
                    column.CanUserSort = true;
                    //可以拖动
                    column.CanUserReorder = true;
                    //设置标题
                    column.Header = item;
                    //标题绑定
                    column.Binding = new Binding(item);
                    //添加标题
                    datagrid.Columns.Add(column);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TitleInit", ex.ToString(), type);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 设置标题（key为标题名称,value为属性的名称）
        /// </summary>
        /// <param name="dicTittles">标题</param>
        public void TitleInit(Dictionary<string, string> dicTittles)
        {
            try
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
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TitleInit", ex.ToString(), dicTittles);
            }
            finally
            {
            }
        }

        #endregion

        #region 获取所有标题

        /// <summary>
        /// 指定类型获取标题
        /// </summary>
        /// <param name="type">指定类型</param>
        /// <returns>返回所有标题</returns>
        List<string> GetTittleList(Type type)
        {
            //创建一个标题集合
            List<string> listTittles = new List<string>();
            try
            {
                //获取指定类型的属性集
                PropertyInfo[] propertyInfoes = type.GetProperties();
                //将所有属性名称收集
                foreach (var property in propertyInfoes)
                {
                    listTittles.Add(property.Name);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "GetTittleList", ex.ToString(), type);
            }
            finally
            {
            }
            //返回属性集合
            return listTittles;
        }

        #endregion

        #region 点击行所要执行哪些内容

        /// <summary>
        /// 当点击单元里面的东西双击的时候将会执行什么
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DGR_Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                OnRowClickEvent();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DGR_Border_MouseLeftButtonDown", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion
    }
}
