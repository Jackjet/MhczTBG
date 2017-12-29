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
using Microsoft.Windows.Themes;

namespace MhczTBG.Controls.DataGridOperate
{
    /// <summary>
    /// FHDataGrid.xaml 的交互逻辑
    /// </summary>
    public partial class ModeDataGrid : UserControl
    {
        #region 声明变量

        /// <summary>
        /// 数据源
        /// </summary>
        List<object> AllList = new List<object>();

        /// <summary>
        /// 标题组件集合
        /// </summary>
        List<DataGridHeaderBorder> headerBorderList = new List<DataGridHeaderBorder>();

        /// <summary>
        /// 所选择的的行
        /// </summary>
        System.Windows.Controls.Border selectedItem = null;

        #endregion

        #region 构造函数

        public ModeDataGrid()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="list">数据</param>
        public ModeDataGrid(IList list)
            : this()
        {
            try
            {
                //生成数据
                DataInit(list);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ModeDataGrid", ex.ToString());
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
        /// <param name="list">数据</param>
        public void DataInit(IList list)
        {
            try
            {

                //_datagrid = datagrid;
                //将数据源设置为全局使用的数据源
                foreach (var item in list)
                {
                    AllList.Add(item);
                }
                this.datagrid.ItemsSource = this.AllList;
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

        #region 设置标题

        /// <summary>
        /// 通过自定类型来设置标题
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
        /// <param name="dicTittles">标题字典</param>
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

        #region 辅助方法

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
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="item">数据项</param>
        public void ItemsAdd(object item)
        {
            try
            {
                this.AllList.Add(item);
                this.datagrid.ItemsSource = AllList;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ItemsAdd", ex.ToString(), item);
            }
            finally
            {
            }
        }

        public void SkinChange(Color color)
        {
            try
            {
                //设置标题颜色
                (this.Resources["linTittle"] as LinearGradientBrush).GradientStops[1].Color = color;
                headerBorderList.ForEach(Item => Item.Background = (this.Resources["linTittle"] as LinearGradientBrush));
                //设置列表触发颜色（鼠标进入该区域）
                (this.Resources["linEnter"] as SolidColorBrush).Color = color;
                //设置列表触发颜色（选择）
                (this.Resources["linRowSelect"] as LinearGradientBrush).GradientStops[1].Color = color;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "SkinChange", ex.ToString(), color);
            }
            finally
            {
            }
        }

        #endregion

        #region Datagrid样式触发事件
     
        /// <summary>
        /// 鼠标进入该行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DGR_Border_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                if (sender is Border && sender != selectedItem)
                {
                    var border = sender as Border;
                    border.Background = this.Resources["linEnter"] as Brush;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DGR_Border_MouseEnter", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 鼠标离开该行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DGR_Border_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                if (sender is Border && sender != selectedItem)
                {
                    var border = sender as Border;
                    border.Background = this.Resources["translateBrush"] as Brush;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DGR_Border_MouseLeave", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 选择一行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DGR_Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (selectedItem != null) selectedItem.Background = this.Resources["translateBrush"] as Brush;

                if (sender is Border)
                {
                    var border = sender as Border;
                    border.Background = this.Resources["linRowSelect"] as Brush;

                    this.selectedItem = border;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DGR_Border_MouseLeftButtonDown", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
     
        /// <summary>
        /// 获取所有标题组件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridHeaderBorder_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is DataGridHeaderBorder)
                {
                    (sender as DataGridHeaderBorder).Background = this.Resources["linTittle"] as Brush;
                    headerBorderList.Add(sender as DataGridHeaderBorder);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DataGridHeaderBorder_Loaded", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion




    }
}
