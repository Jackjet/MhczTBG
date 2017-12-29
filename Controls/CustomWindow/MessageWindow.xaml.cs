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
using MhczTBG.Common;

namespace MhczTBG.Controls.CustomWindow
{
    /// <summary>
    /// MessageWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MessageWindow : Window
    {
        #region 构造函数
        public MessageWindow()
        {
            try
            {
                InitializeComponent();

                this.KeyDown += new KeyEventHandler(MessageWindow_KeyDown);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "MessageWindow", ex.ToString());
            }
            finally
            {
            }
        }

        void MessageWindow_KeyDown(object sender, KeyEventArgs e)
        {
          
        }
        public MessageWindow(string text)
        {
            try
            {
                InitializeComponent();
                this.messageTitle.Text = text;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "MessageWindow", ex.ToString(), text);
            }
            finally
            {
            }
        }
        #endregion

        #region 悬浮离开事件区域
        //窗体移动事件
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    DragMove();
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "OKButton_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        //关闭按钮悬浮事件
        private void imgClose_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            try
            {
                imgClose.Source = Common.CommonMethod.GetImageSource("关闭1");
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "imgClose_MouseEnter", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        //关闭按钮离开事件
        private void imgClose_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            try
            {
                imgClose.Source = Common.CommonMethod.GetImageSource("关闭");
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "imgClose_MouseLeave", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }


        //窗体关闭
        private void imgClose_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "imgClose_MouseDown", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion

        #region 事件区域
        //确定事件
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "OKButton_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        //取消事件
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.DialogResult = false;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "CancelButton_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        #endregion
    }
}
