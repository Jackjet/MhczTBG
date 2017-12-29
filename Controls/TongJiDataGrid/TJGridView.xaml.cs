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
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using System.Threading;
using System.ComponentModel;
using MhczTBG.Common;
//using Microsoft.Windows.Controls;

namespace MhczTBG.Controls.TongJiDataGrid
{
    [Serializable]
    /// <summary>
    /// GridView.xaml 的交互逻辑
    /// </summary>
    public partial class TJGridView : UserControl
    {
        #region 变量

        string _ListTittle;
        /// <summary>
        /// 表头标题
        /// </summary>
        public string ListTittle
        {
            get { return _ListTittle; }
            set
            {
                //标题设置
                if (value != null) this.txtTittle.Text = value;
                _ListTittle = value;
            }
        }

        /// <summary>
        ///DataGrid列表的ScrollViewer控件
        /// </summary>
        ScrollViewer _DataGridScrollViewer = null;

        /// <summary>
        /// 之前所选择的文本
        /// </summary>
        TextBlock _BeforeSelectedText = null;

        /// <summary>
        /// 单元格计数
        /// </summary>
        int _PresentCount = 0;

        int _hengXiaoJiCount = 0;
        /// <summary>
        /// 行坐标小计的数量
        /// </summary>
        public int HengXiaoJiCount
        {
            get { return _hengXiaoJiCount; }
            set { _hengXiaoJiCount = value; }
        }

        /// <summary>
        /// 打印的列标题宽度
        /// </summary>
        double _printItemWidth = 20;

        /// <summary>
        /// 是否为打印标记
        /// </summary>
        bool _isForPrint = false;

        /// <summary>
        /// 新增高度
        /// </summary>
        double _heightAdd = 0;

        /// <summary>
        /// 打印前标题尺寸集合
        /// </summary>
        List<double> _intlist = new List<double>();

        /// <summary>
        /// 总计宽度
        /// </summary>
        double _ZongJiWidth = 85;

        /// <summary>
        /// 存储打印之前的列标题宽度集合
        /// </summary>
        List<double> _columnTextWidthList = new List<double>();

        /// <summary>
        /// 皮肤
        /// </summary>
        Brush brush = default(Brush);



        #endregion

        #region 构造函数

        public TJGridView()
        {
            try
            {
                InitializeComponent();

                #region 注册事件区域

                scroLeft.PreviewMouseWheel += new MouseWheelEventHandler(scroLeft_PreviewMouseWheel);

                #endregion
                //初始化
                _ParamesInit();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TJGridView", ex.ToString());
            }
        }

        #endregion

        #region 初始化

        /// <summary>
        /// 初始化
        /// </summary>
        public void _ParamesInit()
        {
            try
            {
                //窗体呈现事件
                this.SizeChanged += new SizeChangedEventHandler(window_SizeChanged);
                //该控件的偏移属性
                this.dataGrid.Margin = new Thickness(0, -tongjiC._ColumnCellHeight, 0, 0);
                //设置datagird的标题高度
                this.dataGrid.ColumnHeaderHeight = tongjiC._ColumnCellHeight;

                //将列表控件设置为虚拟化
                //this.dataGrid.IsVirtualizing = true;
                //new ListBox().IsVirtualizing = false;

                //设置总计的控件的宽度值
                this.tongjiC.gridLayout.ColumnDefinitions[1].Width = new GridLength(_ZongJiWidth);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "_ParamesInit", ex.ToString());
            }
        }

        #endregion

        #region UI事件区域

        /// <summary>
        /// 禁止鼠标滚动条激活相应事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void scroLeft_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            try
            {
                e.Handled = true;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "scroLeft_PreviewMouseWheel", ex.ToString());
            }
        }

        /// <summary>
        /// 大小更改事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                this.SizeMeasure();

                if (this.tongjiC._dicColumnDefintionList.Count > 0)
                {
                    for (int i = 0; i < this.tongjiC._dicColumnDefintionList[this.tongjiC._dicColumnDefintionList.Count - 1].Count; i++)
                    {
                        if (this.dataGrid.Columns.Count > i)
                            this.tongjiC._dicColumnDefintionList[this.tongjiC._dicColumnDefintionList.Count - 1][i].Width = new GridLength(this.dataGrid.Columns[i].ActualWidth);
                    }
                }

                this.tongjiR.Margin = new Thickness(0, 0, 0, 37);

                this.tongjiC._MesaureBegin();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "window_SizeChanged", ex.ToString());
            }
        }

        /// <summary>
        /// 点击单元格事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tt_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //获取纵坐标
                var y = this.dataGrid.SelectedIndex;
                //获取横坐标
                var x = this.dataGrid.Columns.IndexOf(this.dataGrid.CurrentColumn);
                //获取当前列的标题
                string header = null;
                if (this.dataGrid.CurrentColumn == null) return;
                //判断是否为文本类型
                if (this.dataGrid.CurrentColumn.Header is TextBlock)
                    header = (this.dataGrid.CurrentColumn.Header as TextBlock).Text;
                if (header != null)
                    //最后一列不进行下列操作
                    if (header.ToString().Contains("总计")) return;
                if (y == this.tongjiR._LeftCount) return;
                //获取单元格
                var se = (sender as Border);
                ////获取到单元格的展示容器
                var ss = se.Child as ContentPresenter;
                if (ss == null) return;
                //获取单元格的文本控件
                var pp = ss.Content as TextBlock;
                //获取文本
                var txt = pp.Text;
                //获取列标题所有相关标题
                string[] strArray = this.tongjiC.GetRangeTittle(x);
                //获取行标题所有相关标题
                string[] strArray2 = this.tongjiR.GetRangeTittle(y);

                //将之前所选择的文本字体颜色还原
                if (_BeforeSelectedText != null && !pp.Equals(_BeforeSelectedText)) _BeforeSelectedText.Foreground = new SolidColorBrush(Colors.Black);

                //判断文本是否相同
                if (!pp.Equals(_BeforeSelectedText))
                {
                    //改变字体颜色
                    pp.Foreground = new SolidColorBrush(Colors.Red);
                    //添加文本引用
                    _BeforeSelectedText = pp;
                }


            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "tt_PreviewMouseLeftButtonUp", ex.ToString());
            }
            //#region 测试

            //string column = null;
            //foreach (var item in strArray)
            //{
            //    column += item + "_";
            //}

            //string Row = null;
            //foreach (var item in strArray2)
            //{
            //    Row += item + "_";
            //}

            //if (column != null && Row != null)
            //    MessageBox.Show(string.Format("单元格的值为（{0}）；所属列标题（{1}）；所属行标题（{2}）", txt, column, Row));

            //#endregion
        }

        /// <summary>
        /// 将最后一行字体设置为红色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContentPresenter_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _PresentCount++;
                var AllCount = (this.tongjiR._LeftCount) * this.tongjiC._LeftCount;
                if (_PresentCount >= AllCount) return;

                if ((sender as Border).Child == null) return;
                //获取单元格文本
                var text2 = (((sender as Border).Child) as ContentPresenter).Content as TextBlock;
                if (text2.Text.Equals("0") || text2.Text.Equals(""))
                    (sender as Border).Child = null;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ContentPresenter_Loaded", ex.ToString());
            }
        }

        /// <summary>
        /// 标题宽度更改事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ColumnHeader_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                this.window_SizeChanged(null, null);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ColumnHeader_SizeChanged", ex.ToString());
            }
        }

        /// <summary>
        /// 获取Datagrid的_DataGridScrollViewer控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //获取列表ScrollViewer控件
                _DataGridScrollViewer = GetVisualChild<ScrollViewer>(this.dataGrid);
                //若找到该元素,则进行滚动的绑定
                if (_DataGridScrollViewer != null)
                {
                    //_DataGridScrollViewer.IsDeferredScrollingEnabled = true;
                    //注册dataGrid的ScrollViewer滚动条垂直滚动事件
                    _DataGridScrollViewer.ScrollChanged += new ScrollChangedEventHandler(_DataGridScrollViewer_ScrollChanged);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "dataGrid_Loaded", ex.ToString());
            }
        }

        /// <summary>
        /// 调整垂直位置对齐
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _DataGridScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            try
            {
                //获取到垂直滚动的距离
                //double nowVerOffset = (this._DataGridScrollViewer.VerticalOffset / this._DataGridScrollViewer.ViewportHeight) * (this.scroLeft.ActualHeight);

                //获取Datagrid下拉控件的偏移指数
                double nowVerOffset = this._DataGridScrollViewer.ContentVerticalOffset;

                if (double.IsNaN(nowVerOffset)) return;
                //获取垂直移动的基数
                var AvHeight = Convert.ToInt32(nowVerOffset) * this.tongjiR._RowCellHeight;

                //调整垂直位置对齐
                this.scroLeft.ScrollToVerticalOffset(AvHeight);

                //调整水平位置对齐
                this.scroTop.ScrollToHorizontalOffset(this._DataGridScrollViewer.HorizontalOffset);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "_DataGridScrollViewer_ScrollChanged", ex.ToString());
            }
        }

        #endregion

        #region 恢复默认设置

        /// <summary>
        /// 恢复默认设置
        /// </summary>
        public void ReSet()
        {
            try
            {
                //标题集合
                _ListTittle = null;
                //释放绑定
                dataGrid.ItemsSource = null;
                //清楚列表标题集
                dataGrid.Columns.Clear();
                //
                _BeforeSelectedText = null;
                _hengXiaoJiCount = 1;
                this.tongjiC.Reset();
                this.tongjiR.Reset();
                _PresentCount = 0;
                _printItemWidth = 20;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ReSet", ex.ToString());
            }
        }

        #endregion

        #region 辅助方法

        #region 添加标题

        /// <summary>
        /// 添加列标题
        /// </summary>
        /// <param name="ColumnTittleList">标题集合</param>
        /// <param name="isFinish">是否完成</param>
        public void _ColumnHeaderAdd(List<string> ColumnTittleList, bool isFinish)
        {
            try
            {
                //添加列标题区域
                tongjiC.ItemsAdd(ColumnTittleList, null, isFinish);
                _ColumnSumn(isFinish);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "_ColumnHeaderAdd", ex.ToString(), ColumnTittleList, isFinish);
            }
        }

        /// <summary>
        /// 添加列标题
        /// </summary>
        /// <param name="ColumnTittleList">标题集合</param>
        /// <param name="isFinish">是否完成</param>
        public void _ColumnHeaderAdd(List<string> ColumnTittleList, List<int> intList, bool isFinish)
        {
            try
            {
                //添加列标题区域
                tongjiC.ItemsAdd(ColumnTittleList, intList, isFinish);

                _ColumnSumn(isFinish);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "_ColumnHeaderAdd", ex.ToString(), ColumnTittleList, intList, isFinish);
            }
        }

        void _ColumnSumn(bool isFinish)
        {
            try
            {
                //获取最后一个区域的标题作为Datagrid的列标题
                if (isFinish && this.tongjiC._DicTittleTextList.Count > 0)
                {
                    //获取最后一层标题区域
                    var gridBottom = this.tongjiC._DicTittleTextList[this.tongjiC._DicTittleTextList.Count - 1];

                    this.tongjiC._dicTextList[this.tongjiC._dicTextList.Count - 1].Clear();

                    //遍历添加标题
                    for (int i = 0; i < gridBottom.Count(); i++)
                    {
                        //创建一个列
                        DataGridTextColumn column = new DataGridTextColumn();
                        //绑定字段
                        column.Binding = new Binding(Convert.ToString(i));

                        column.Width = tongjiC._SumCount * tongjiC._oneStringWidth;

                        //标题容器
                        Border border = new Border();
                        //标题文本
                        TextBlock txt = new TextBlock() { Text = gridBottom[i], TextWrapping = TextWrapping.Wrap };
                        this.tongjiC._dicTextList[this.tongjiC._dicTextList.Count - 1].Add(txt);
                        if (txt.Text.Equals("小计"))
                        {
                            txt.Foreground = new SolidColorBrush(Colors.Red);
                            _hengXiaoJiCount++;
                        }
                        //加载标题
                        border.Child = txt;
                        //标题
                        column.Header = border;

                        //添加标题
                        this.dataGrid.Columns.Add(column);


                    }

                    //创建一个列
                    DataGridTextColumn columnSUM = new DataGridTextColumn() { Width = _ZongJiWidth, Header = new TextBlock() { TextWrapping = System.Windows.TextWrapping.Wrap, Text = "总计" } };
                    //绑定总计
                    columnSUM.Binding = new Binding("总计");

                    columnSUM.Foreground = new SolidColorBrush(Colors.Red);

                    //添加标题
                    this.dataGrid.Columns.Add(columnSUM);

                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "_ColumnSumn", ex.ToString(), isFinish);
            }
        }

        /// <summary>
        /// 添加行标题
        /// </summary>
        /// <param name="RowTittleList">标题集合</param>
        public void _RowHeaderAdd(List<string> RowTittleList, bool isFinish)
        {
            try
            {
                //添加行标题区域
                tongjiR.ItemsAdd(RowTittleList, null, isFinish);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "_RowHeaderAdd", ex.ToString(), RowTittleList, isFinish);
            }
        }

        /// <summary>
        /// 添加行标题
        /// </summary>
        /// <param name="RowTittleList">标题集合</param>
        public void _RowHeaderAdd(List<string> RowTittleList, List<int> intList, bool isFinish)
        {
            try
            {
                //添加行标题区域
                tongjiR.ItemsAdd(RowTittleList, intList, isFinish);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "_RowHeaderAdd", ex.ToString(), RowTittleList, intList, isFinish);
            }
        }

        #endregion

        #region 列标题填充

        /// <summary>
        /// 校验列标题区域与列表标题的宽度对齐
        /// </summary>
        public void _MesaureWidth()
        {
            try
            {
                //累计宽度
                double allWidth = 0;

                //通过遍历去给每一个标题重新分配宽度
                for (int i = 0; i < this.dataGrid.Columns.Count - 1; i++)
                {
                    //累计宽度计数
                    allWidth += this.dataGrid.Columns[i].ActualWidth;
                }
                //获取工作区域的宽度
                //var disTance = this.ActualWidth - this.tongjiR.ActualWidth - 18 - this.tongjiC.gridLayout.ColumnDefinitions[1].ActualWidth;
                var disTance = this.ActualWidth - this.tongjiR.ActualWidth - 18 - this._ZongJiWidth;
                //如果实际的表格的标题的所有宽度比工作区域的宽度要小,则填充该剩余空间
                if (allWidth < disTance)
                {
                    //累计宽度初始化
                    allWidth = 0;
                    //通过遍历去给每一个标题重新分配宽度
                    for (int i = 0; i < this.dataGrid.Columns.Count; i++)
                    {
                        if (i < this.dataGrid.Columns.Count - 1)
                        {

                            if (this._isForPrint)
                                this.dataGrid.Columns[i].Width = _printItemWidth;
                            else
                            {
                                //平均分配
                                this.dataGrid.Columns[i].Width = disTance / (this.dataGrid.Columns.Count - 1);
                                //累计宽度重新计数
                                allWidth += this.dataGrid.Columns[i].ActualWidth;
                            }
                        }
                        else
                        {

                            if (this._isForPrint)
                                this.dataGrid.Columns[i].Width = _printItemWidth;
                            else
                                //平均分配
                                this.dataGrid.Columns[i].Width = this._ZongJiWidth;
                        }
                    }
                }
                if (!this._isForPrint)
                    //设置列标题区域宽度与列表标题的宽度对齐
                    this.tongjiC.Width = allWidth + 1 + this.tongjiC.gridLayout.ColumnDefinitions[1].ActualWidth;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "_MesaureWidth", ex.ToString());
            }
        }

        #endregion

        #region 查找子元素

        /// <summary>
        /// 查找子元素
        /// </summary>
        /// <typeparam name="T">指定要查找的子元素类型</typeparam>
        /// <param name="parent">要查找的父容器</param>
        /// <returns>返回指定类型的子元素</returns>
        private T GetVisualChild<T>(Visual parent) where T : Visual
        {
            //指定类型
            T child = default(T);
            try
            {
                //获取父容器子级的数量
                int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
                //通过遍历去获取指定类型的子元素
                for (int i = 0; i < numVisuals; i++)
                {
                    //获取其中一个子元素（按照次序查找）
                    Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                    //转换为指定类型,若非指定类型,转换之后为空
                    child = v as T;
                    //如果为空,查找其子项的子元素
                    if (child == null)
                        child = GetVisualChild<T>(v);
                    //如果不为空,则跳出循环,将指定类型的元素返回
                    if (child != null) break;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "GetVisualChild", ex.ToString());
            }
            //返回指定类型的元素
            return child;
        }

        #endregion

        #region 调整尺寸

        /// <summary>
        /// 调整尺寸
        /// </summary>
        void SizeMeasure()
        {
            try
            {
                //第一列的宽度设置为实际宽度
                this.gridMain.ColumnDefinitions[0].Width = new GridLength(this.tongjiR.ActualWidth);
                //设置第一行的高度为实际高度
                this.gridMain.RowDefinitions[0].Height = new GridLength(this.tongjiC.ActualHeight + _heightAdd);
                //校验列标题区域与列表标题的宽度对齐
                _MesaureWidth();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "SizeMeasure", ex.ToString());
            }
        }

        #endregion

        #region 打印时更改尺寸

        /// <summary>
        /// 改变列表标题样式（适合打印）
        /// </summary>
        /// <param name="height"></param>
        /// <param name="fixedPageWidth"></param>
        public void ChangeForPrint(int height, double fixedPageWidth)
        {
            try
            {
                //获取工作区域
                var workWidth = fixedPageWidth - this.tongjiR.ActualWidth - 40;

                //遍历设置标题列宽度
                if (this.dataGrid.Columns.Count * _printItemWidth < workWidth)
                {
                    foreach (var item in this.dataGrid.Columns)
                    {
                        //保存设置之前的标题列宽度
                        _intlist.Add(item.ActualWidth);
                        //单独设置每一列标题的宽度
                        item.Width = workWidth / this.dataGrid.Columns.Count;
                    }
                    _printItemWidth = workWidth / this.dataGrid.Columns.Count;
                }
                else
                {
                    //设置列标题的宽度
                    foreach (var item in this.dataGrid.Columns)
                    {
                        _intlist.Add(item.ActualWidth);

                        item.Width = _printItemWidth;
                    }
                }
                _columnTextWidthList.Clear();
                foreach (var item in this.tongjiC._dicTextList[this.tongjiC._dicTextList.Count - 1])
                {
                    _columnTextWidthList.Add(item.ActualWidth);
                    item.Width = 20;
                }

                //到时候第二行的列的高度添加像素
                var susm = 20;

                this.dataGrid.ColumnHeaderHeight = height;

                if (this.tongjiC._dicTextList.Count > 1)
                {
                    _heightAdd = height + susm - this.tongjiC._ColumnCellHeight;
                    this.tongjiC.gridMain.RowDefinitions[this.tongjiC._dicTextList.Count - 2].Height = new GridLength(susm + this.tongjiC.gridMain.RowDefinitions[this.tongjiC._dicTextList.Count - 2].ActualHeight);
                    this.tongjiC.gridMain.RowDefinitions[this.tongjiC._dicTextList.Count - 1].Height = new GridLength(0);
                }
                else
                {
                    _heightAdd = height - this.tongjiC._ColumnCellHeight;
                }

                this.dataGrid.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
                this.dataGrid.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;

                _isForPrint = true;
                //该控件的偏移属性
                this.dataGrid.Margin = new Thickness(0, -(height), 20, 20);
                SizeMeasure();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "tongji__ItemCanNotSameExitEvent", ex.ToString(), height, fixedPageWidth);
            }
        }

        /// <summary>
        /// 恢复打印前的样式
        /// </summary>
        /// <param name="height"></param>
        public void ChangeBeforeState(int height)
        {
            try
            {
                for (int i = 0; i < this.dataGrid.Columns.Count; i++)
                {
                    this.dataGrid.Columns[i].Width = new DataGridLength(_intlist[i]);
                }

                for (int i = 0; i < this.tongjiC._dicTextList[this.tongjiC._dicTextList.Count - 1].Count; i++)
                {
                    this.tongjiC._dicTextList[this.tongjiC._dicTextList.Count - 1][i].Width = _columnTextWidthList[i];
                }

                this.dataGrid.ColumnHeaderHeight = height;
                _heightAdd = 0;
                _isForPrint = false;
                //该控件的偏移属性
                this.dataGrid.Margin = new Thickness(0, -(height), 0, 0);

                this.dataGrid.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                this.dataGrid.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
                if (this.tongjiC._dicTextList.Count > 1)
                {
                    this.tongjiC.gridMain.RowDefinitions[this.tongjiC._dicTextList.Count - 2].Height = new GridLength(this.tongjiC._ColumnCellHeight);
                    this.tongjiC.gridMain.RowDefinitions[this.tongjiC._dicTextList.Count - 1].Height = new GridLength(this.tongjiC._ColumnCellHeight);
                }

                SizeMeasure();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ChangeBeforeState", ex.ToString(), height);
            }
        }

        #endregion

        #region 皮肤更改

        /// <summary>
        /// 更改皮肤
        /// </summary>
        /// <param name="brushKeyName"></param>
        public void SkingSetting(string brushKeyName, Brush txtForobrush)
        {
            try
            {
                if (!string.IsNullOrEmpty(brushKeyName))
                {
                    brush = StyleResource.Style1.Instacnce.Resources[brushKeyName] as Brush;
                    if (brush != null)
                    {
                        this.borBiao.Background = brush;
                        this.tongjiC.borMain.Background = brush;
                        this.tongjiR.gridLayout.Background = brush;
                    }
                }

                var style = this.Resources["headerStyle1"] as Style;
                foreach (var item in style.Setters)
                {
                    var setter = item as Setter;
                    if (setter == null) break;
                    if (setter.Property.Name.Equals("Foreground"))
                    {
                        setter.Value = txtForobrush;
                    }
                    else if (setter.Property.Name.Equals("Background"))
                    {
                        setter.Value = brush;
                    }
                }

                var style2 = this.Resources["headerStyle2"] as Style;
                foreach (var item in style2.Setters)
                {
                    var setter = item as Setter;
                    if (setter == null) break;
                    if (setter.Property.Name.Equals("Foreground"))
                    {
                        setter.Value = txtForobrush;
                    }
                    else if (setter.Property.Name.Equals("Background"))
                    {
                        setter.Value = brush;
                    }
                }

                this.tongjiR.txtSum.Foreground = this.tongjiR.Foreground = this.tongjiC.Foreground = txtForobrush;

            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "SkingSetting", ex.ToString(), brushKeyName);
            }
        }

        /// <summary>
        /// 表格边框线设置
        /// </summary>
        /// <param name="color"></param>
        public void borderBrushSetting(Color color)
        {
              try
            {
            var solidbrush = this.tongjiR.Resources["borColor"] as SolidColorBrush;
            var solidbrush2 = this.tongjiC.Resources["borColor"] as SolidColorBrush;

            if (solidbrush != null && solidbrush2 != null)
                solidbrush2.Color = solidbrush.Color = color;
                    }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "borderBrushSetting", ex.ToString(), color);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 更改皮肤
        /// </summary>
        /// <param name="brushKeyName"></param>
        public void SkingSetting(Brush brushResources)
        {
            try
            {
                brush = brushResources;
                if (brush != null)
                {
                    this.borBiao.Background = brush;
                    this.tongjiC.borMain.Background = brush;
                    this.tongjiR.gridLayout.Background = brush;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "SkingSetting", ex.ToString(), brushResources);
            }
        }

        #endregion

        #endregion
    }
}
