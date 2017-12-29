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
using System.Windows.Media.Animation;
using System.Windows.Threading;
using MhczTBG.Common;

namespace MhczTBG.Controls.Load
{
    /// <summary>
    /// Loading.xaml 的交互逻辑
    /// </summary>
    public partial class Loading : UserControl
    {
        #region 变量

        //运行动画的计时器
        DispatcherTimer timer = new DispatcherTimer();

        #endregion

        #region 构造函数

        public Loading()
        {
            try
            {
                InitializeComponent();
                ParmeterInit();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Loading", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion

        #region 初始化

        /// <summary>
        /// 运行初始化
        /// </summary>
        void ParmeterInit()
        {
            try
            {
                timer.Interval = TimeSpan.FromMilliseconds(200);
                timer.Tick += new EventHandler(timer_Tick);
                timer.Start();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ParmeterInit", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 开启计时器（运行动画）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                rrrr.Angle += 20;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "timer_Tick", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 停止运行
        /// </summary>
        public void StopRun()
        {
            try
            {
                new TomDisPatcherLb(0.5, new Action(() =>
                    {
                        timer.Stop();
                        this.Visibility = System.Windows.Visibility.Collapsed;
                    })).Start();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "StopRun", ex.ToString());
            }
            finally
            {
            }
        }

        /// <summary>
        /// 开始运行
        /// </summary> 
        public void StartRun()
        {
            try
            {
              
                this.Visibility = System.Windows.Visibility.Visible;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "StartRun", ex.ToString());
            }
            finally
            {
            }
        }

        /// <summary>
        /// 皮肤设置
        /// </summary>
        /// <param name="color">要设置的颜色</param>
        public void SkinChange(Color color)
        {
            try
            {
                 this.Resources.BeginInit();
                (this.Resources["loadBrush"] as SolidColorBrush).Color = color;
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
