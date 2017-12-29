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
using System.Windows.Threading;
using MhczTBG.Common;

namespace MhczTBG.Controls.Tab
{
    /// <summary>
    /// TabChange.xaml 的交互逻辑
    /// </summary>
    public partial class TabChange : UserControl
    {
        #region 变量

        /// <summary>
        /// 默认的第一个项(使用DataGridModeStart方法自动填充)
        /// </summary>
        public UcDataGrid ucDataGrid = null;

        /// <summary>
        /// 默认的第二个项(使用DataGridModeStart方法自动填充)
        /// </summary>
        public UcPanel ucPanel = null;

        /// <summary>
        /// 子项位置偏移
        /// </summary>
        double offset = 0;

        /// <summary>
        /// 选择项，默认为0
        /// </summary>
        int selectIndex = 0;

        /// <summary>
        /// 是否可以向左滑动
        /// </summary>
        bool CanMovingLeft = true;

        /// <summary>
        /// 是否可以向右滑动
        /// </summary>
        bool CanMovingRight = true;

        /// <summary>
        /// 管理向右滑动的计时器
        /// </summary>
        DispatcherTimer timerRight = new DispatcherTimer();

        /// <summary>
        /// 管理向左滑动的计时器
        /// </summary>
        DispatcherTimer timerLeft = new DispatcherTimer();


        double douSpeed = 10;
        /// <summary>
        /// 控制滑动的速度，以10为默认值
        /// </summary>
        public double DouSpeed
        {
            get { return douSpeed; }
            set { douSpeed = value; }
        }

        #endregion

        #region 构造函数

        public TabChange()
        {
            try
            {
                InitializeComponent();

                //初始化（构造计时器）
                ParamesInit();

                #region 注册事件区域
                //调整精确度
                this.scro.ScrollChanged += new ScrollChangedEventHandler(scro_ScrollChanged);

                #endregion
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TabChange", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion

        #region 初始化

        /// <summary>
        /// 初始化
        /// </summary>
        void ParamesInit()
        {
            try
            {
                //计时器的执行频率
                timerRight.Interval = timerLeft.Interval = TimeSpan.FromMilliseconds(10);
                //计时器所执行的任务
                timerRight.Tick += new EventHandler(timerRight_Tick);
                timerLeft.Tick += new EventHandler(timerLeft_Tick);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ParamesInit", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion

        #region UI事件区域

        /// <summary>
        /// 大小更改事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabChange_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                //遍历子项
                foreach (var item in this.stackPanel1.Children)
                {

                    //调整宽度（这个20是因为bor设置了Pading的缘故）
                    (item as FrameworkElement).Width = this.bor.ActualWidth;

                    //调整切换的精确度
                    if (selectIndex == 1)
                    {
                        scro.ScrollToHorizontalOffset(this.bor.ActualWidth);
                    }
                    else
                    {
                        scro.ScrollToHorizontalOffset(0);
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TabChange_SizeChanged", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 调整精确度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void scro_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            try
            {
                if (CanMovingLeft && CanMovingRight)
                {
                    //调整切换的精确度
                    if (selectIndex == 1)
                    {
                        scro.ScrollToHorizontalOffset(this.bor.ActualWidth);
                    }
                    else
                    {
                        scro.ScrollToHorizontalOffset(0);
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "scro_ScrollChanged", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 控制向左滑动的计时器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timerLeft_Tick(object sender, EventArgs e)
        {
            try
            {
                //偏移量大于0，才可以向左移动
                if (offset > 0)
                {
                    //启动偏移
                    scro.ScrollToHorizontalOffset(offset);
                    //偏移量减小（衡量的标准是左面的切点和向右的偏移）
                    offset -= this.scro.ActualWidth / 20;
                }
                else
                {
                    //到了临界点，切换标志为0
                    selectIndex = 0;
                    //精确调整位置
                    scro.ScrollToHorizontalOffset(0);
                    //关闭计时器
                    timerLeft.Stop();
                    CanMovingLeft = true;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "timerLeft_Tick", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 控制向右滑动的计时器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timerRight_Tick(object sender, EventArgs e)
        {
            try
            {
                //偏移量小于所见到的容器宽度，便可继续向左偏移
                if (offset < this.bor.ActualWidth)
                {
                    //启动向左偏移
                    scro.ScrollToHorizontalOffset(offset);
                    //偏移位置增加
                    offset += this.scro.ActualWidth / 20;
                }
                else
                {
                    //到了临界点，切换标志为1
                    selectIndex = 1;
                    //精确调整位置
                    scro.ScrollToHorizontalOffset(this.bor.ActualWidth);
                    //关闭计时器
                    timerRight.Stop();
                    CanMovingRight = true;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "timerRight_Tick", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion

        #region 选择默认模式（可以使用，可以不使用）

        /// <summary>
        /// 选择默认模式
        /// </summary>
        public void DataGridModeStart()
        {
            try
            {
                //创建第一个子项（特殊列表）
                ucDataGrid = new UcDataGrid() { MaxHeight = 655 };
                //创建第二个子项（特殊Panel）
                ucPanel = new UcPanel() { MaxHeight = 655 };

                //加载子项
                this.ItemsAdd(ucDataGrid);
                this.ItemsAdd(ucPanel);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DataGridModeStart", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion

        #region 添加子项(因为涉及到切换，该容器只能添加两个项)

        /// <summary>
        /// 添加子项
        /// </summary>
        /// <param name="control"></param>
        public void ItemsAdd(FrameworkElement control)
        {
            try
            {
                //该容器只能放置两个项
                if (this.stackPanel1.Children.Count < 3)
                {
                    //宽度调整

                    control.Width = this.bor.ActualWidth;

                    //加载子项
                    this.stackPanel1.Children.Add(control);
                    //第二个子项定义大小变化
                    if (this.stackPanel1.Children.Count == 2)
                    {
                        this.bor.SizeChanged += new SizeChangedEventHandler(TabChange_SizeChanged);
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ItemsAdd", ex.ToString(), control);
            }
            finally
            {
            }
        }

        #endregion

        #region 自动切换（控制左右切换的方法）

        #region 向右移动

        /// <summary>
        /// 切换到第二个子项
        /// </summary>
        public void ScrollToRight()
        {
            try
            {
                CanMovingRight = false;
                timerRight.Start();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ScrollToRight", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion

        #region 向左移动

        /// <summary>
        /// 切换到第一个子项
        /// </summary>
        public void ScrollToLeft()
        {
            try
            {
                CanMovingLeft = false;
                timerLeft.Start();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ScrollToLeft", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion

        #endregion
    }
}
