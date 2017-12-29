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
using MhczTBG.StyleResource;
using MhczTBG.Common;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Reflection;

namespace MhczTBG.Controls.TongXins
{
    /// <summary>
    /// TongXinMain.xaml 的交互逻辑
    /// </summary>
    public partial class TongXinMain : UserControl
    {
        #region 变量

        /// <summary>
        /// 设置样式资源
        /// </summary>
        Style1 resourec = new Style1();

        StackPanel panel;
        /// <summary>
        /// 显示字段的panel
        /// </summary>
        public StackPanel Panel
        {
            get
            {
                //引用左导航承载体
                if (panel == null)
                {
                    panel = new StackPanel();
                    this.borLeft.Child = panel;
                }
                return panel;
            }
            set
            {
                //设置左导航承载体
                if (value != null)
                {
                    this.borLeft.Child = value;
                    panel = value;
                }
            }
        }


        Border borderContent;
        /// <summary>
        /// 显示的内容
        /// </summary>
        public Border BorderContent
        {
            get
            {
                //引用内容承载体
                if (panel == null)
                {
                    borderContent = new Border();
                    this.borMain.Child = panel;
                }
                return borderContent;
            }
            set
            {
                //设置内容承载体
                if (value != null)
                {
                    this.borMain.Child = value;
                    borderContent = value;
                }
            }
        }

        string contentTittle;
        /// <summary>
        /// 内容的标题
        /// </summary>
        public string ContentTittle
        {
            get { return contentTittle; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    txtTittle.Text = value;
                    contentTittle = value;
                }
            }
        }

        /// <summary>
        /// 临时存储提示控件
        /// </summary>
        object elem = null;

        /// <summary>
        /// 控制文字闪烁的计时器
        /// </summary>
        DispatcherTimer timer = null;

        /// <summary>
        /// 导航选择的项
        /// </summary>
        StackPanel stackSlectPanel = null;

        string position;
        /// <summary>
        /// 职位
        /// </summary>
        public string Position
        {
            get { return position; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.txtOfficer.Text = value.Trim();
                    position = value;
                }
            }
        }

        string apartMentName;
        /// <summary>
        /// 部门名称
        /// </summary>
        public string ApartMentName
        {
            get { return apartMentName; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.dep_name.Text = value.Trim();
                    apartMentName = value;
                }
            }
        }

        string userName;
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.user_name.Text = value.Trim();
                    userName = value;
                }
            }
        }

        #endregion

        #region 自定义委托事件

        public delegate void DaoHangSelectEventHandle(string text);
        /// <summary>
        /// 点击导航触发的事件
        /// </summary>
        public event DaoHangSelectEventHandle DaoHangSelectEvent = null;

        public delegate void UpdatePasswordEventHandle();
        /// <summary>
        /// 更改密码
        /// </summary>
        public event UpdatePasswordEventHandle UpdatePasswordEvent = null;

        public delegate void CancelEventHandle();
        /// <summary>
        /// 注销
        /// </summary>
        public event CancelEventHandle CancelEvent = null;

        #endregion

        #region 构造函数

        public TongXinMain()
        {
            try
            {
                InitializeComponent();

                #region 注册事件区域

                ///点击log的时候全屏
                this.full_bd.MouseLeftButtonDown += new MouseButtonEventHandler(full_bd_MouseLeftButtonDown);
                //加载事件
                this.Loaded += new RoutedEventHandler(TongXinMain_Loaded);

                this.bt注销.Click += new RoutedEventHandler(bt注销_Click);

                this.ChangePassword.Click += new RoutedEventHandler(ChangePassword_Click);

                #endregion

                //样式设计
                StyleInit();

                //显示时间
                GetTime();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TongXinMain", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion

        #region UI事件区域

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TongXinMain_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (panel != null)
                {
                    //将所有导航承载控件关联一个点击事件
                    foreach (var item in panel.Children)
                    {
                        if (item is Expander)
                        {
                            (item as Expander).Expanded += new RoutedEventHandler(TongXinMain_Expanded);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TongXinMain_Loaded", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// expander展开事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TongXinMain_Expanded(object sender, RoutedEventArgs e)
        {
            try
            {
                //将其他的项闭合
                foreach (var item in panel.Children)
                {
                    if (item is Expander && sender is Expander && !(item as Expander).Equals(sender as Expander))
                    {
                        (item as Expander).IsExpanded = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TongXinMain_Expanded", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 更改密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (UpdatePasswordEvent != null) UpdatePasswordEvent();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ChangePassword_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bt注销_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CancelEvent != null) CancelEvent();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "bt注销_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion

        #region 样式设计

        /// <summary>
        /// 前台样式初始化设计
        /// </summary>
        void StyleInit()
        {
            try
            {
                ChangePassword.Style = bt注销.Style = resourec.Resources["btnStyle2"] as Style;

            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "StyleInit", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion

        #region 点击Logo全屏

        /// <summary>
        /// 点击Logo全屏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void full_bd_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //获取到最顶层的元素（一定是窗体）
                var window = CommonMethod.GetRootParent(40, this);

                if (window != null && window is Window)
                {
                    //切换主窗体显示状态
                    if ((window as Window).WindowState == WindowState.Maximized)
                    {
                        //窗体还原
                        (window as Window).WindowState = WindowState.Normal;
                    }
                    else
                    {
                        //窗体最大化
                        (window as Window).WindowState = WindowState.Maximized;
                    }
                }
            }
            catch (Exception ex)
            {

                MethodLb.CreateLog(this.GetType().FullName, "full_bd_MouseLeftButtonDown", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion

        #region 获取时间方法

        /// <summary>
        /// 获取时间
        /// </summary>
        void GetTime()
        {
            try
            {
                //当前日期时间
                DateTime t = DateTime.Now;
                //显示日期部分
                this.time1.Text = t.ToString("yyyy年MM月dd日");

                this.time3.Text = CaculateWeekDay(t.Year, t.Month, t.Day);
            }
            catch (Exception ex)
            {

                MethodLb.CreateLog(this.GetType().FullName, "gettime", ex.ToString());
            }
            finally
            {
            }
        }

        /// <summary>
        /// 年月日换算成星期几
        /// </summary>
        /// <param name="y">年</param>
        /// <param name="m">月</param>
        /// <param name="d">日</param>
        /// <returns></returns>
        public static string CaculateWeekDay(int y, int m, int d)
        {
            string weekstr = string.Empty;
            try
            {
                if (m == 1) m = 13;

                if (m == 2) m = 14;

                int week = (d + 2 * m + 3 * (m + 1) / 5 + y + y / 4 - y / 100 + y / 400) % 7 + 1;

                switch (week)
                {
                    case 1: weekstr = "星期一"; break;

                    case 2: weekstr = "星期二"; break;

                    case 3: weekstr = "星期三"; break;

                    case 4: weekstr = "星期四"; break;

                    case 5: weekstr = "星期五"; break;

                    case 6: weekstr = "星期六"; break;

                    case 7: weekstr = "星期日"; break;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(typeof(TongXinMain).FullName, "CaculateWeekDay", ex.ToString(), y, m, d);
            }
            finally
            {
            }
            return weekstr;
        }

        #endregion

        #region 让文字进行闪烁

        /// <summary>
        /// 文字闪烁
        /// </summary>
        void TextFlush()
        {
            try
            {
                if (timer == null)
                {
                    timer = new DispatcherTimer();
                    timer.Interval = TimeSpan.FromMilliseconds(100);
                    timer.Tick += new EventHandler(timer_Tick);
                }
                timer.Start();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TextFlush", ex.ToString());
            }
            finally
            {
            }
        }

        /// <summary>
        /// 控制文字闪烁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (tr.X < Title.ActualWidth + 20)
                {
                    tr.X += 10;
                }
                else
                {
                    tr.X = 0;
                    timer.Stop();
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "timer_Tick", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion

        #region 提示支持
        /// <summary>
        /// 创建提示
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="StartMethodName">名称</param>
        public void SettingTip(Type type, string StartMethodName)
        {
            try
            {
                //创建提示
                if (elem == null) elem = Assembly.GetAssembly(type).CreateInstance(type.FullName);
                //加载提示
                if (elem is FrameworkElement)
                {
                    this.borTip.Child = elem as FrameworkElement;
                }

                //获取到所有方法
                var methods = type.GetMethods();
                if (methods != null && methods.Count() > 0)
                {
                    foreach (var item in methods)
                    {
                        if (item.Name.Equals(StartMethodName))
                        {
                            item.Invoke(elem, null);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "SettingTip", ex.ToString(), type, StartMethodName);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 关闭提示
        /// </summary>
        public void CloseTip(string StopMethodName)
        {
            try
            {
                if (elem != null)
                {
                    //获取到所有方法
                    var methods = elem.GetType().GetMethods();
                    if (methods != null && methods.Count() > 0)
                    {
                        foreach (var item in methods)
                        {
                            if (item.Name.Equals(StopMethodName))
                            {
                                item.Invoke(elem, null);
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "CloseTip", ex.ToString(), StopMethodName);
            }
            finally
            {
            }
        }

        #endregion

        #region 样式事件扩展

        private void StackPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (DaoHangSelectEvent != null && sender is StackPanel && (sender as StackPanel).Children.Count > 1 && (sender as StackPanel).Children[1] is TextBlock)
                {
                    DaoHangSelectEvent(((sender as StackPanel).Children[1] as TextBlock).Text);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "StackPanel_MouseLeftButtonUp", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 导航按钮背景切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTittle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //将之前选择的项设置背景为透明
                if (stackSlectPanel != null)
                {
                    stackSlectPanel.Background = this.Resources["TrasteBrush"] as Brush;
                }
                //设置当前选择项的背景色
                (sender as StackPanel).Background = this.Resources["ItemSelectBrush"] as Brush;

                stackSlectPanel = (sender as StackPanel);

                tr.X = 0;
                TextFlush();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "txtTittle_MouseLeftButtonDown", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 导航按钮进入之后的样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StackPanel_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                if (sender is StackPanel && !sender.Equals(stackSlectPanel))
                {
                    var stackpanel = sender as StackPanel;
                    stackpanel.Background = this.Resources["ItemEnterBrush"] as Brush;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "StackPanel_MouseEnter", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 导航按钮离开之后的样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StackPanel_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                if (sender is StackPanel && !sender.Equals(stackSlectPanel))
                {
                    var stackpanel = sender as StackPanel;
                    stackpanel.Background = this.Resources["TrasteBrush"] as Brush;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "StackPanel_MouseLeave", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion

        #region 皮肤更换

        /// <summary>
        /// 皮肤更换
        /// </summary>
        /// <param name="color">指定颜色</param>
        public void SkinChange(Color color)
        {
            try
            {
                this.Resources.BeginInit();
                (this.Resources["backBrush"] as LinearGradientBrush).GradientStops[0].Color = color;
                (this.Resources["ItemEnterBrush"] as LinearGradientBrush).GradientStops[1].Color = color;
                (this.Resources["ItemSelectBrush"] as LinearGradientBrush).GradientStops[1].Color = color;
                (this.Resources["borTittleBrush"] as LinearGradientBrush).GradientStops[1].Color = color;
                this.Resources.EndInit();
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
    }
}
