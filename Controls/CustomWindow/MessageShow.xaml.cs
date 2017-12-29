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
using MhczTBG.Common;

namespace MhczTBG.Controls.CustomWindow
{
    public partial class MessageShow : Window
    {
        #region 变量

        string _MessageContent = string.Empty;
        /// <summary>
        /// 存储的提示内容
        /// </summary>
        public string MessageContent
        {
            get { return _MessageContent; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.messageTitle.Text = value;
                }
                _MessageContent = value;
            }
        }

        #endregion

        #region 构造函数
        public MessageShow()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "MessageShow", ex.ToString());
            }
            finally
            {
            }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="title">提示字符串</param>
        public MessageShow(string title)
        {
            try
            {
                InitializeComponent();
                this.messageTitle.Text = title;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "MessageShow", ex.ToString(), title);
            }
            finally
            {
            }
        }
        #endregion

        #region 事件区域
        /// <summary>
        /// 确定事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// 取消事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

