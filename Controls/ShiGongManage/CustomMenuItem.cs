using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using MhczTBG.Common;

namespace MhczTBG.Controls.ShiGongManage
{
    public class CustomMenuItem : MenuItem
    {
        #region 变量
        /// <summary>
        /// 画笔
        /// </summary>
        Brush brush = null;

        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="header">标题</param>
        public CustomMenuItem(string header)
        {
            try
            {
                brush = this.Background;
                this.Cursor = Cursors.Hand;
                this.FontSize = 12;
                this.Header = header;
                this.Loaded += new RoutedEventHandler(TomMenuItem_Loaded);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "CustomMenuItem", ex.ToString(), header);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="header">标题</param>
        /// <param name="imageUri">图片URI</param>
        public CustomMenuItem(string header, string imageUri)
            : this(header)
        {
            try
            {
                //设置下拉菜单的图标
                this.Icon = new Image() { Width = 20, Height = 20, Source = CommonMethod.GetImageSource(imageUri) };
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "CustomMenuItem", ex.ToString(), header, imageUri);
            }
            finally
            {
            }
        }
        #endregion

        #region UI事件区域

        void TomMenuItem_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.MouseEnter += new MouseEventHandler(TomMenuItem_MouseEnter);
                this.MouseLeave += new MouseEventHandler(TomMenuItem_MouseLeave);
                this.LostFocus += new RoutedEventHandler(TomMenuItem_LostFocus);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TomMenuItem_Loaded", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        void TomMenuItem_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                //失去焦点还原背景色
                this.Background = brush;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TomMenuItem_LostFocus", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        void TomMenuItem_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                //鼠标离开还原
                this.Background = brush;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TomMenuItem_MouseLeave", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        void TomMenuItem_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                //鼠标进入之后背景换色
                this.Background = new SolidColorBrush(Colors.Blue) { Opacity = 0.2 };
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TomMenuItem_MouseEnter", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

      

        #endregion

    }
}
