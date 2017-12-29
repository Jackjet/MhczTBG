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
using Hardcodet.Wpf.TaskbarNotification;
using System.Windows.Controls.Primitives;
using System.Net;
using MhczTBG.Common;

namespace MhczTBG.Controls.TongZhi
{
    /// <summary>
    /// TongZhi.xaml 的交互逻辑
    /// </summary>
    public partial class TongZhi : UserControl
    {
        #region 变量

        /// <summary>
        /// 是否关闭的标示
        /// </summary>
        private bool isClosing = false;

        /// <summary>
        /// 通知描述
        /// </summary>
        public static readonly DependencyProperty BalloonTextProperty =
            DependencyProperty.Register("BalloonText",
                                        typeof(string),
                                        typeof(TongZhi),
                                        new FrameworkPropertyMetadata(""));

        /// <summary>
        ///指示标题
        /// </summary>
        public string BalloonText
        {
            get { return (string)GetValue(BalloonTextProperty); }
            set { SetValue(BalloonTextProperty, value); }
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="popupAnimation">弹出的动画方式</param>
        /// <param name="tittle">标题</param>
        /// <param name="element">要盛放的元素</param>
        public TongZhi(string tittle, FrameworkElement element, PopupAnimation popupAnimation)
        {
            try
            {
                InitializeComponent();

                //生成通知
                ParamesInit(popupAnimation, tittle);
                //把资源清除
                this.borMain.Resources.Clear();
                this.borMain.Child = element;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TongZhi", ex.ToString(), tittle, element, popupAnimation);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="popupAnimation">弹出的动画方式</param>
        /// <param name="tittle">标题</param>
        /// <param name="content">显示内容</param>
        public TongZhi(string tittle, string content, PopupAnimation popupAnimation)
        {
            try
            {
                InitializeComponent();

                //生成通知
                ParamesInit(popupAnimation, tittle);
                //指定通知内容
                this.borMain.Child = new TextBlock() { Text = content };
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TongZhi", ex.ToString(), tittle, content, popupAnimation);
            }
            finally
            {
            }
        }

        #endregion

        #region 初始化

        /// <summary>
        ///  初始化生成
        /// </summary>
        /// <param name="popupAnimation">动画</param>
        /// <param name="tittle">标题</param>
        private void ParamesInit(PopupAnimation popupAnimation, string tittle)
        {
            try
            {
                //生成标题
                this.txtTittle.Text = tittle;
                TaskbarIcon.AddBalloonClosingHandler(this, OnBalloonClosing);
                TaskbarIcon MyNotifyIcon = new TaskbarIcon();
                MyNotifyIcon.ShowCustomBalloon(this, popupAnimation, 60000);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ParamesInit", ex.ToString(), popupAnimation, tittle);
            }
            finally
            {
            }
        }

        #endregion

        #region UI事件区域

        /// <summary>
        ///
        /// </summary>
        private void OnBalloonClosing(object sender, RoutedEventArgs e)
        {
            try
            {
                e.Handled = true;
                isClosing = true;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "OnBalloonClosing", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        ///关闭通知
        /// </summary>
        private void imgClose_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var window = Common.CommonMethod.GetRootParent(30, this);
                if (window is Popup)
                {
                    (window as Popup).IsOpen = false;
                }
                TaskbarIcon taskbarIcon = TaskbarIcon.GetParentTaskbarIcon(this);
                taskbarIcon.CloseBalloon();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "imgClose_MouseDown", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 触发事件（当鼠标进入该区域，将通知弹出）
        /// </summary>
        private void grid_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                //如果是
                if (isClosing) return;
                //the tray icon assigned this attached property to simplify access
                TaskbarIcon taskbarIcon = TaskbarIcon.GetParentTaskbarIcon(this);
                taskbarIcon.ResetBalloonCloseTimer();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "grid_MouseEnter", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 关闭虚浮框
        /// </summary>
        private void OnFadeOutCompleted(object sender, EventArgs e)
        {
            try
            {
                Popup pp = (Popup)Parent;
                pp.IsOpen = false;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "OnFadeOutCompleted", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        //点击弹出通知列表
        private void btnMessage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TaskbarIcon taskbarIcon = TaskbarIcon.GetParentTaskbarIcon(this);
                taskbarIcon.CloseBalloon();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btnMessage_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion
    }
}
