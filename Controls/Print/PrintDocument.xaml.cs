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
using System.Windows.Markup;
using MhczTBG.Common;
using MhczTBG.Controls.Load;
using System.Collections;

namespace MhczTBG.Controls.Print
{
    /// <summary>
    /// PrintDocument.xaml 的交互逻辑
    /// </summary>
    public partial class PrintDocument : Window
    {
        #region 变量

        //统一管理视图
        List<Viewbox> listViewBox = new List<Viewbox>();
       
        /// <summary>
        /// 打印预览尺寸
        /// </summary>
        public Size _size = new Size(96 * 8.5, 96*11);

        /// <summary>
        /// 宽预览参数
        /// </summary>
        double _height = 11;

        /// <summary>
        /// 高预览参数
        /// </summary>
        double _width = 8.5;

        //打印页
        int printPage = 0;

        //是否打印图表
        public bool isShuPutDown = false;


        int douPageCount = 1;
        //当前页
        public int DouPageCount
        {
            get { return douPageCount; }
            set
            {
                if (value != douPageCount)
                {
                    this.txtPage.Text = value.ToString();
                    douPageCount = value;
                }
            }
        }

        //ViewBox之前的宽度
        double douBeforeWidth;

        #endregion

        #region 构造函数

        public PrintDocument()
        {
            try
            {
                InitializeComponent();

                //初始化打印设置
                printInit();

                #region 注册事件

                this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
                //字体填充
                this.cmbView.SelectionChanged += new SelectionChangedEventHandler(cmbView_SelectionChanged);
                //第一页
                this.btnFirst.Click += new RoutedEventHandler(btnFirst_Click);
                //上一页
                this.btnUp.Click += new RoutedEventHandler(btnUp_Click);
                //下一页
                this.btnNext.Click += new RoutedEventHandler(btnNext_Click);
                //最后一页
                this.btnLast.Click += new RoutedEventHandler(btnLast_Click);
                //页面设置跳转
                this.txtPage.KeyDown += new KeyEventHandler(txtPage_KeyDown);
                //字体填充设置跳转
                this.txtViewScale.KeyDown += new KeyEventHandler(txtViewScale_KeyDown);

                #endregion
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "PrintDocument", ex.ToString());
            }
            finally
            {
            }
        }

        /// <summary>
        /// 打印DataGrid(具有页眉页脚)
        /// </summary>
        /// <param name="strTitle">标题</param>
        /// <param name="element">要打印的Datagrid实例</param>
        /// <param name="intCount">以多少页为基准进行分配</param>
        public PrintDocument(string strTitle, DataGrid element, int intCount)
            : this()
        {
            try
            {
                List<PrintDataGrid> commonDataGridList = new List<PrintDataGrid>();

                int intPage = (int)Math.Ceiling((double)element.Items.Count / intCount);

                int j = 0;

                for (int i = 0; i < intPage; i++)
                {
                    List<object> listLinShi = new List<object>();

                    for (; j < intCount * (i + 1); j++)
                    {
                        if (j < element.Items.Count)
                        {
                            listLinShi.Add(element.Items[j]);
                        }
                        else break;
                    }

                    PrintDataGrid datagrid = new PrintDataGrid();
                    datagrid.TitleInit(element.Columns);
                    datagrid.datagrid.ItemsSource = listLinShi;
                    commonDataGridList.Add(datagrid);

                    Border border = new Border();
                    border.Child = datagrid;
                }

                foreach (var commandDataGrid in commonDataGridList)
                {
                    var d = commandDataGrid.datagrid.Items.Count;
                    this.Items_Add(strTitle, commandDataGrid);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "PrintDocument", ex.ToString(), strTitle, element, intCount);
            }
            finally
            {
            }
        }

        #endregion

        #region 事件区域

        /// <summary>
        /// 获取viewbox实际大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listViewBox.Count > 0)
                {
                    //获取当前viewbox的实际大小
                    douBeforeWidth = listViewBox[0].ActualWidth;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "MainWindow_Loaded", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 改变字体填充
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txtViewScale_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //如果按的是enter键
                if (e.Key == Key.Enter)
                {
                    double douScale;
                    //将输入文本框的数值转换为百分比
                    if (double.TryParse(this.txtViewScale.Text, out douScale))
                    {
                        ChangeView(douScale / 100);
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "txtViewScale_KeyDown", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 改变页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txtPage_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //如果按的是enter键
                if (e.Key == Key.Enter)
                {
                    //将输入文本框的值转为要跳转的页码
                    if (int.TryParse(this.txtPage.Text, out douPageCount))
                    {
                        //当前页大于0，并且小于或等于页面的总数量
                        if (douPageCount > 0 && douPageCount <= view.PageCount)
                        {
                            //跳转至某一页
                            view.GoToPage(douPageCount);
                            //改变页面导航的样式
                            PageCountChanged();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "txtPage_KeyDown", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 最后一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnLast_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //设置当前页为最后一页
                DouPageCount = view.PageCount;
                //跳转到最后一页
                view.LastPage();
                //改变页面导航的样式
                PageCountChanged();
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
        /// 下一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnNext_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //当前页小于页面的总数量
                if (DouPageCount < view.PageCount)
                {
                    //当前页加1
                    DouPageCount++;
                    //跳转到下一页
                    view.NextPage();
                    //改变页面导航的样式
                    PageCountChanged();
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
        /// 上一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //当前页大于1
                if (DouPageCount > 1)
                {
                    //当前页减1
                    DouPageCount--;
                    //跳转到上一页
                    view.LastPage();
                    //改变页面导航的样式
                    PageCountChanged();
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btnUp_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 第一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnFirst_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //设置当前页为第一页
                DouPageCount = 1;
                //跳转到第一页
                view.FirstPage();
                //改变页面导航的样式
                PageCountChanged();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btnFirst_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }


        /// <summary>
        /// 改变字体填充
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cmbView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //获取选择项的值
                ComboBox com = (sender as ComboBox);
                string selet = com.SelectedItem.ToString();
                double douScale;

                if (gridScale.Visibility == System.Windows.Visibility.Visible)
                {
                    gridScale.Visibility = System.Windows.Visibility.Collapsed;
                }

                if (selet.Contains("%"))
                {
                    //将获取到的值转为double
                    douScale = Convert.ToDouble(selet.Replace("%", "")) / 100;
                    //改变页面视图的大小
                    ChangeView(douScale);
                }
                //如果选择"缩小字体填充"，则百分百的填充
                else if (selet.Contains("缩小字体填充"))
                {
                    //改变页面视图的大小
                    ChangeView(1);
                }
                else
                {
                    gridScale.Visibility = System.Windows.Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "cmbView_SelectionChanged", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion

        #region 打印初始化

        //初始化打印设置
        private void printInit()
        {

            try
            {
                //页面大小改变的参数值
                cmbView.ItemsSource = new List<string>(){
              "缩小字体填充", "30%","50%","60%","70%","80%","85%","90%","95%","100%","125%","150%","200%","自定义"
           };
                //设置为第一页
                this.txtPage.Text = douPageCount.ToString();
                //打印预览窗体居中
                this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                //打印预览窗体最大化
                this.WindowState = System.Windows.WindowState.Maximized;
                //初始化页面导航的样式
                PageCountChanged();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "printInit", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion

        #region 添加要打印的元素


        /// <summary>
        /// 添加所要打印的文件（标题、打印目标）
        /// </summary>
        /// <param name="strTitle">标题</param>
        /// <param name="element">打印目标</param>
        public void Items_Add(string strTitle, FrameworkElement element)
        {
            //当前打印页的页码
            printPage++;
            try
            {
                #region 打印承载体初始化

                PageContent pageContent = new PageContent();
                //为页面提供内容
                FixedPage fixedPage = new FixedPage();

                fixedPage.Width = this._size.Width;
                fixedPage.Height = this._size.Height;

                //_FixedPageList.Add(fixedPage);

                //修饰容器
                Viewbox box = new Viewbox();
                //修饰容器集合，统一更改视图
                listViewBox.Add(box);
                //将打印页加载
                doc.Pages.Add(pageContent);

                ((IAddChild)pageContent).AddChild(fixedPage);

                #endregion

                #region 更换父容器（更换对子元素的连接）

                if (element.Parent != null)
                {
                    //需要打印的元素的父元素
                    DependencyObject parent = element.Parent as DependencyObject;

                    #region 设置打印状态

                    //设置边距
                    element.Margin = new Thickness(7, 20, 10, 7);
                    //打印状态不可编辑
                    element.IsEnabled = false;

                    #endregion

                    //提示控件
                    LoadingPage loading1 = new LoadingPage();
                    loading1.waitingText.Text = "执行中。。。。。。请稍等";
                    box.Child = null;

                    #region 打开窗体，元素替换操作

                    //如果父元素是border，直接替换
                    if (parent is Border)
                    {
                        (parent as Border).Child = loading1;
                    }
                    //如果父元素是grid,先移出要打印的元素,替换为等待提示
                    else if (parent is Grid)
                    {
                        (parent as Grid).Children.Remove(element);
                        (parent as Grid).Children.Add(loading1);
                    }
                    //如果为其它父元素,大可以使用下列方式
                    else
                    {
                        parent.SetValue(ContentPresenter.ContentProperty, loading1);
                    }

                    #endregion

                    #region 打印目标

                    //打印表格（c3,高铁,普通）
                    if (element.Tag != null && element.Tag.ToString().Contains("GZLB"))
                    {
                        Border border = new Border();
                        border.Child = element;
                        box.Child = border;
                    }
                    //打印index折线图
                    else if (isShuPutDown)
                    {
                        element.RenderTransformOrigin = new Point(0.5, 0.5);
                        TransformGroup transformGroup = new TransformGroup();
                        transformGroup.Children.Add(new RotateTransform(90));
                        transformGroup.Children.Add(new ScaleTransform(1, 0.7));
                        element.RenderTransform = transformGroup;
                        element.Margin = new Thickness(-250, 350, 0, 0);

                        box.Child = element;
                    }
                    //打印其他（index故障列表,datagrid）
                    else
                    {
                        #region 属性设置

                        element.Margin = new Thickness(7, 20, 10, 7);
                        element.IsEnabled = false;

                        #endregion

                        PrintItem item = new PrintItem(strTitle, element);
                        item.Width = this._size.Width;
                        item.txtPageFoot.Text = printPage.ToString();
                        box.Child = item;
                    }

                    #endregion

                    //打印窗体关闭时还原
                    this.Closed += (object sender2, EventArgs e2) =>
                    {
                        loading1.waitingText.Text = "正在关闭打印窗体";

                        if (isShuPutDown) return;

                        #region 内存回收

                        GC.WaitForPendingFinalizers();
                        GC.Collect();
                        GC.WaitForPendingFinalizers();

                        #endregion
                    
                        #region 还原状态设置

                        element.Margin = new Thickness(0);
                        element.IsEnabled = true;

                        #endregion

                        (element.Parent as Border).Child = null;

                        #region 窗体关闭，还原为初始状态

                        if (parent is Border)
                        {
                            (parent as Border).Child = element;
                        }
                        else if (parent is Grid)
                        {
                            try
                            {

                                (parent as Grid).Children.Remove(loading1);
                                (parent as Grid).Children.Add(element);
                            }
                            catch (Exception)
                            {

                                throw;
                            }

                        }
                        else if (parent is ScrollViewer)
                        {
                            try
                            {
                                (parent as ScrollViewer).Content = element;
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                        }
                        else
                        {
                            parent.SetValue(ContentPresenter.ContentProperty, element);
                        }

                        #endregion
                    };
                }
                #endregion

                fixedPage.Children.Add(box);
                this.txtAllCount.Text = view.PageCount.ToString();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Items_Add", ex.ToString(), strTitle, element);
            }
            finally
            {
            }
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 改变视图的显示比例
        /// </summary>
        /// <param name="douScale">显示元素</param>
        void ChangeView(double douScale)
        {
            try
            {
                foreach (Viewbox box in listViewBox)
                {
                    box.Width = douBeforeWidth * douScale;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ChangeView", ex.ToString(), douScale);
            }
            finally
            {
            }
        }

        // 页面改变时发生（可用不可用）
        void PageCountChanged()
        {
            try
            {
                //最后一页 最后一页、下一页按钮屏蔽
                if (DouPageCount == view.PageCount)
                {
                    btnNext.Background = btnLast.Background = this.Resources["color2"] as SolidColorBrush;
                    btnNext.IsEnabled = btnLast.IsEnabled = false;
                }
                else
                {
                    btnNext.Background = btnLast.Background = this.Resources["color1"] as SolidColorBrush;
                    btnNext.IsEnabled = btnLast.IsEnabled = true;
                }
                //第一页 第一页、上一页按钮屏蔽
                if (DouPageCount == 1)
                {
                    btnFirst.Background = btnUp.Background = this.Resources["color2"] as SolidColorBrush;
                    btnFirst.IsEnabled = btnLast.IsEnabled = false;

                }
                else
                {
                    btnFirst.Background = btnUp.Background = this.Resources["color1"] as SolidColorBrush;
                    btnNext.IsEnabled = btnLast.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "PageCountChanged", ex.ToString());
            }
            finally
            {
            }
        }

        /// <summary>
        /// 横向预览
        /// </summary>   
        public void HengYuLan()
        {
          
            _size = new Size(96 * _height, 96 * _width);
        }

        /// <summary>
        /// 纵向预览
        /// </summary>
        public void ShuYuLan()
        {
            _size = new Size(96 * _width, 96 * _height);
        }
        #endregion

       
    }
}
