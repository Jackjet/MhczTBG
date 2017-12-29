using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using MhczTBG.Common;

namespace MhczTBG.Controls.DragView
{
    public partial class DragTab : UserControl
    {
        #region 变量

        /// <summary>
        /// 监测分页数量
        /// </summary>
        double PageAllCount = 0;

        /// <summary>
        /// 当前呈现的页码
        /// </summary>
        int pageCount = 1;

        /// <summary>
        /// 检测翻页的参数
        /// </summary>
        int pageRight = 0;

        /// <summary>
        /// 监控右侧的子项所应在的行标示
        /// </summary>
        int rowPosition = 0;

        /// <summary>
        /// 判断是否可以拖动
        /// </summary>
        private bool isDrag = false;

        /// <summary>
        /// 拖动起始位置
        /// </summary>
        private Point StartPoint;

        /// <summary>
        /// 右侧子项的集合（带第一层包装LBl，第二层包装Grid）
        /// </summary>
        List<Grid> gridList = new List<Grid>();

        /// <summary>
        /// 最原始的的子项（无包装）
        /// </summary>
        List<FrameworkElement> framewokElementList = new List<FrameworkElement>();

        /// <summary>
        /// 临时存储移动源
        /// </summary>
        FrameworkElement element = null;

        /// <summary>
        /// 层叠数设置起始参数
        /// </summary>
        int zIndex = 50;

        #endregion

        #region 构造函数

        public DragTab()
        {
            try
            {
                InitializeComponent();

                #region 注册事件区域

                this.LayoutUpdated += new EventHandler(TomTabChange_LayoutUpdated);

                this.Loaded += new RoutedEventHandler(DragTab_Loaded);

                this.MouseLeftButtonUp += new MouseButtonEventHandler(DragTab_MouseLeftButtonUp);
                #endregion
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DragTab", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion

        #region UI事件区域

        /// <summary>
        /// 初始化加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DragTab_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //分页导航显示
                lblPagePanel.Visibility = System.Windows.Visibility.Visible;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DragTab_Loaded", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 该容器的左键点击释放事件（防止子项拖动释放时鼠标不在子项区域而进行的调整）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DragTab_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (framewokElementList != null && framewokElementList.Count > 0)
                {
                    //调整所有子项的偏移
                    foreach (var item in framewokElementList)
                    {
                        item.Margin = new Thickness(0);
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DragTab_MouseLeftButtonUp", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 跳转到下一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //进4位
                pageRight += 4;
                //如果超出总数量，则还原，并跳出该方法
                if (pageRight >= gridList.Count)
                {
                    pageRight -= 4;
                    return;
                }
                else
                {
                    //清除右边所有子项
                    this.gridRight.Children.Clear();

                    int j = 0;
                    //通过子项集合以及索引根据分页的参数进行调整
                    for (; pageRight < this.gridList.Count; pageRight++)
                    {
                        if (j >= 4) break;
                        var grid = gridList[pageRight];
                        grid.SetValue(Grid.RowProperty, j);
                        this.gridRight.Children.Add(grid);
                        j++;
                    }
                    pageRight -= this.gridRight.Children.Count;
                    pageCount++;
                    this.txtPage.Text = string.Format("{0}/{1}", pageCount, (int)PageAllCount);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btnNext_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 跳转到上一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLast_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //如果小于或等于0，初始临界点
                if (pageRight <= 0)
                {
                    //pageRight -= (4+this.gridRight.Children.Count);
                    return;
                }
                else
                {
                    //退四位
                    pageRight -= 4;
                    int offset = this.gridRight.Children.Count;
                    //清除右边所有子项
                    this.gridRight.Children.Clear();
                    int j = 0;
                    //通过子项集合以及索引根据分页的参数进行调整
                    for (; pageRight < this.gridList.Count; pageRight++)
                    {
                        if (j >= 4) break;
                        var grid = gridList[pageRight];
                        grid.SetValue(Grid.RowProperty, j);
                        this.gridRight.Children.Add(grid);
                        j++;
                    }
                    pageCount--;
                    this.txtPage.Text = string.Format("{0}/{1}", pageCount, (int)PageAllCount);
                    pageRight -= this.gridRight.Children.Count;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btnLast_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 子项尺寸实时调整
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TomTabChange_LayoutUpdated(object sender, EventArgs e)
        {
            try
            {
                //子项集合
                foreach (var item in framewokElementList)
                {
                    //获取包装的那个Label容器
                    FrameworkElement lbl = ((item.Parent) as FrameworkElement);
                    //获取其左或右大容器
                    lbl = lbl.Parent as FrameworkElement;
                    //若右边的容器为null
                    if (this.gridRight.Children.Count <= 0)
                    {
                        //则将左侧的子项全部填充满
                        this.LayoutRoot.ColumnDefinitions[0].Width = new GridLength(this.ActualWidth);
                    }
                    //为左容器的子项
                    if (lbl == gridLeft)
                    {
                        //if (item.Width > 10 && item.Height > 10)
                        //{
                            item.Width = gridLeft.ActualWidth - 10;
                            item.Height = gridLeft.ActualHeight - 10;
                        //}
                    }
                    //右容器的子项
                    else
                    {
                        if (this.ActualWidth - this.gridLeft.ActualWidth > 7)
                        {
                            item.Width = this.ActualWidth - this.gridLeft.ActualWidth - 7;
                            if (this.gridLeft.ActualHeight == 0) return;
                            item.Height = (this.gridLeft.ActualHeight - 45) / this.gridRight.RowDefinitions.Count - 7;
                        }
                    }
                }
                this.LayoutRoot.ColumnDefinitions[1].Width = new GridLength(this.ActualWidth / 4);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TomTabChange_LayoutUpdated", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion

        #region 添加子项

        /// <summary>
        /// 添加子项（用户控件、Grid、Image都可以）
        /// </summary>
        /// <param name="item">子项的实例</param>
        /// <param name="itemGuanLian">希望通过这个子项里的子元素来切换试图</param>
        public void AddItem(DragTabItem item)
        {
            try
            {
                //VerticalContentAlignment = System.Windows.VerticalAlignment.Stretch, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch
                //每一item子项,都放置到Label中
                Label border = new Label() { };
                border.Content = item;

                //第一个加载的子项放置在左边的容器里显示
                if (gridLeft.Children.Count == 0)
                {
                    //左边容器加载子项
                    gridLeft.Children.Add(border);                 
                }
                else
                {
                    //HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch
                    //右边容器的子项除了用labl包装，还需要用Grid包起来
                    Grid gridOne = new Grid() { };
                    gridOne.Children.Add(border);
                    //通过list对所有右边容器的子项进行统一的管理
                    gridList.Add(gridOne);

                    //如果右侧的子项数量不超过最大限度
                    if (gridRight.Children.Count < 4)
                    {
                        //右边容器加载一行
                        gridRight.RowDefinitions.Add(new RowDefinition() { });
                        //将该子项设置到该行显示
                        gridOne.SetValue(Grid.RowProperty, rowPosition);
                        //加载子项
                        gridRight.Children.Add(gridOne);
                        //行标示递增
                        rowPosition++;
                    }

                }

                //将最原始的子项加入到控件集里去管理
                framewokElementList.Add(item);

                //子项鼠标进入事件
                item.MouseEnter += (object sender, MouseEventArgs e) =>
                {
                    //如果不是在左侧的容器中，则激发labl标题的显示（鼠标进入）
                    if ((((sender as UserControl).Parent as FrameworkElement).Parent) as Grid != gridLeft)
                    {
                        (sender as DragTabItem).lbl.Visibility = System.Windows.Visibility.Visible;
                    }
                };
                //给子项关联切换行为
                ElementTuoDong(item);
                //刷新分页的显示
                PageDisplayReflesh();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "AddItem", ex.ToString(), item);
            }
            finally
            {
            }
        }

        #endregion

        #region 子项事件区域

        /// <summary>
        /// 标识是否可以拖动对象（子项点击）
        /// </summary>       
        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            element = sender as FrameworkElement;
            //获取父容器
            var gridLeftOrOther = ((((((sender) as DragTabItem).lbl).Parent as FrameworkElement).Parent as FrameworkElement).Parent as FrameworkElement).Parent as Grid;
            //如果为右侧容器，则可以拖动
            if (gridLeftOrOther != gridLeft)
            {
                //设置子项的父容器Label
                Canvas.SetZIndex(((sender as FrameworkElement).Parent) as Label, zIndex++);
                //标示为可以拖动
                isDrag = true;
                //记录起始位置
                StartPoint = e.GetPosition(this.LayoutRoot);
                //element.CaptureMouse();
            }
        }

        /// <summary>
        /// 子项相应鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StackPanel_MouseMove(object sender, MouseEventArgs e)
        {
            // 拖动终止位置      
            Point EndPoint;

            if (isDrag && (sender as FrameworkElement).Equals(element))
            {
                EndPoint = e.GetPosition(this.LayoutRoot);

                //计算X、Y轴起始点与终止点之间的相对偏移量
                double y = EndPoint.Y - StartPoint.Y;
                double x = EndPoint.X - StartPoint.X;

                Thickness margin = element.Margin;

                //计算新的Margin
                Thickness newMargin = new Thickness()
                {
                    Left = margin.Left + x,
                    Top = margin.Top + y,
                    Right = margin.Right - x,
                    Bottom = margin.Bottom - y
                };
                //设置移动效果
                element.Margin = newMargin;

                //开始位置变为最终的位置
                StartPoint = EndPoint;
            }
        }

        /// <summary>
        /// 子项拖动完成（鼠标点击弹起事件）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StackPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //鼠标点击释放了，不可再拖动
            isDrag = false;
            Label lbl = ((sender) as DragTabItem).lbl;
            //获取父容器
            var gridLeftOrOther = (((lbl.Parent as FrameworkElement).Parent as FrameworkElement).Parent as FrameworkElement).Parent as Grid;
            //右侧容器的子项可以拖动
            if (gridLeftOrOther != gridLeft)
            {
                //获取左侧的子项Label外包装
                Label borderLeft = gridLeft.Children[0] as Label;
                //获取右侧拖动的子项的外包装                
                Label borderRight = (((lbl.Parent as Grid).Parent) as DragTabItem).Parent as Label;

                //右侧子项的行位置
                var RowP = Grid.GetRow(borderRight.Parent as FrameworkElement);
                //动画参数
                //var widthParems = this.gridLeft.ActualWidth - this.gridRight.ActualWidth / 2;
                //判断是否符合切换的条件
                if (StartPoint.X < this.gridLeft.ActualWidth)
                {
                    //使用动画
                    GetMainElementToRight(borderLeft, borderRight, this.ActualWidth - 40, this.LayoutRoot.ColumnDefinitions[1].ActualWidth, RowP * this.gridRight.ActualHeight / this.gridRight.RowDefinitions.Count - 0);
                }
                else
                {
                    //不符合则自动调整
                    element.Margin = new Thickness(0);
                }
                foreach (var item in framewokElementList)
                {
                    item.Margin = new Thickness(0);
                }
            }
        }

        #endregion

        #region 动画实现

        /// <summary>
        /// 向右移动
        /// </summary>
        /// <param name="lblLeft">左侧容器子项（带第一层包装）</param>
        /// <param name="lblRight">右侧容器指定的子项（带第一层包装）</param>
        /// <param name="allWidth"></param>
        /// <param name="rightWidth"></param>
        /// <param name="y"></param>
        void GetMainElementToRight(Label lblLeft, Label lblRight, double allWidth, double rightWidth, double y)
        {
            try
            {
                lblLeft.RenderTransformOrigin = new Point(0, 0);

                double a = GetChu(rightWidth, allWidth - rightWidth);
                double b = GetChu(lblRight.ActualHeight, this.gridLeft.ActualHeight);

                var d = Grid.GetRow(lblRight.Parent as FrameworkElement);
                Grid griRight = lblRight.Parent as Grid;
                griRight.Children.Clear();
                gridLeft.Children.Clear();

                if (lblRight.RenderTransform == null) lblRight.RenderTransform = new ScaleTransform() { ScaleX = 0.25, ScaleY = 0.25 };
                lblRight.RenderTransformOrigin = new Point(0.5, 0.5);

                Label lbl = new Label();
                var dd = lblLeft.Content;
                lblLeft.Content = null;
                lbl.Content = dd;
                griRight.Children.Add(lbl);

                //this.ScaleX(lblRight, 1, 100);

                GetRightElementToLeft(lblRight, lblLeft);
                if (lbl == null || lbl.Content == null) return;

                ((lbl.Content) as FrameworkElement).Width = this.ActualWidth - this.gridLeft.ActualWidth;

                ((lbl.Content) as FrameworkElement).Height = (this.gridLeft.ActualHeight - 45) / this.gridRight.RowDefinitions.Count;

                ((lblRight.Content) as FrameworkElement).Width = this.gridLeft.ActualWidth;
                ((lblRight.Content) as FrameworkElement).Height = this.gridLeft.ActualHeight;

                gridLeft.Children.Add(lblRight);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "GetMainElementToRight", ex.ToString(), lblLeft, lblRight, allWidth, rightWidth, y);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 向左移动
        /// </summary>
        /// <param name="lblLeft">左侧容器子项（带第一层包装）</param>
        /// <param name="lblRight">右侧容器指定的子项（带第一层包装）</param>
        void GetRightElementToLeft(Label lblLeft, Label lblRight)
        {
            try
            {
                (lblLeft.Content as FrameworkElement).Margin = new Thickness(0);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "GetRightElementToLeft", ex.ToString(), lblLeft, lblRight);
            }
            finally
            {
            }
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 清除所有子项
        /// </summary>
        public void ClearAll()
        {
            try
            {
                gridRight.RowDefinitions.Clear();
                gridRight.ColumnDefinitions.Clear();
                gridRight.Children.Clear();
                gridLeft.Children.Clear();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ClearAll", ex.ToString());
            }
            finally
            {
            }
        }

        /// <summary>
        ///获取要缩小的系数
        /// </summary>
        /// <param name="x">长度1</param>
        /// <param name="y">长度2</param>
        /// <returns></returns>
        public double GetChu(double x, double y)
        {
            double result = 0;
            try
            {
                result = Math.Round((double)x / y, 2);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "GetChu", ex.ToString(), x, y);
            }
            finally
            {
            }
            return result;
        }

        /// <summary>
        /// 给子项添加切换功能
        /// </summary>
        /// <param name="element">指定的子项</param>
        /// <param name="LayoutRoot">拖动的范围,整个视图领域</param>
        public void ElementTuoDong(FrameworkElement element)
        {
            //注册拖动事件，其中有开启动画的内容
            element.MouseLeftButtonDown += new MouseButtonEventHandler(StackPanel_MouseLeftButtonDown);
            element.MouseLeftButtonUp += new MouseButtonEventHandler(StackPanel_MouseLeftButtonUp);
            element.MouseMove += new MouseEventHandler(StackPanel_MouseMove);
        }

        /// <summary>
        /// 刷新分页的显示
        /// </summary>
        void PageDisplayReflesh()
        {
            try
            {
                //检测看要分多少页右边容器最大容量为4
                this.PageAllCount = Math.Ceiling((double)this.gridList.Count / 4);
                //若分页的数量小于或大于0，则强行设置为1
                if (PageAllCount <= 0) PageAllCount = 1;
                //显示分页的具提内容
                this.txtPage.Text = string.Format("{0}/{1}", 1, (int)PageAllCount);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "PageDisplayReflesh", ex.ToString());
            }
            finally
            {
            }
        }


        #region 动画控制

        ///// <summary>
        ///// 控制缩变的动画
        ///// </summary>
        //DispatcherTimer timer1 = new DispatcherTimer();

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="obj">要实现的对象</param>
        ///// <param name="xScale">所要达到的缩变状态</param>
        ///// <param name="time2">触发时间的控制</param>
        //void ScaleX(FrameworkElement obj, double xScale, double time2)
        //{
        //    timer1.Interval = TimeSpan.FromMilliseconds(time2);
        //    timer1.Tick += (object sender, EventArgs e) =>
        //        {                 
        //            var render = obj.RenderTransform as ScaleTransform;
        //            if (render != null)
        //            {
        //                render.ScaleX += xScale / 10;
        //                render.ScaleY += xScale / 10;
        //                if (render.ScaleX > xScale) timer1.Stop();
        //            }
        //        };
        //    timer1.Start();
        //}

        #endregion

        #endregion
    }
}
