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

namespace MhczTBG.Controls.Load
{
    /// <summary>
    /// LoadingPage.xaml 的交互逻辑
    /// </summary>
    public partial class LoadingPage : UserControl
    {
        #region 变量

        /// <summary>
        /// 控制透明动画的计时器
        /// </summary>
        DispatcherTimer timer = null;

        /// <summary>
        /// 控制关闭动画的计时器
        /// </summary>
        DispatcherTimer tim = null;

        #endregion

        #region 构造函数

        public LoadingPage()
        {
            try
            {
                InitializeComponent();
                ParametInit();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "LoadingPage", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion

        #region 初始化

        void ParametInit()
        {
            try
            {
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(50);
                timer.Tick += new EventHandler(timer_Tick);

                tim = new DispatcherTimer();
                tim.Interval = TimeSpan.FromSeconds(1.2);
                tim.Tick += new EventHandler(tim_Tick);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ParametInit", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion

        #region 辅助方法

        void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (gridTip.Opacity < 0.06) return;
                gridTip.Opacity -= 0.03;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "timer_Tick", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        void tim_Tick(object sender, EventArgs e)
        {
            try
            {
                timer.Stop();
                this.Visibility = System.Windows.Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "tim_Tick", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        public void StartRun()
        {
            try
            {
                tim.Stop();
                this.Visibility = System.Windows.Visibility.Visible;
                this.gridTip.Opacity = 1;
                timer.Start();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "StartRun", ex.ToString());
            }
            finally
            {
            }
        }

        public void StopRun()
        {
            try
            {
                tim.Start();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "StopRun", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion
    }
}
