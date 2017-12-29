using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using MhczTBG.Common;

namespace MhczTBG.Common.SharePointBook
{
    class TbgMenuItem : MenuItem
    {
        #region 变量

        /// <summary>
        /// 存储默认的背景
        /// </summary>
        Brush brush = null;

        #endregion

        #region 构造函数

        public TbgMenuItem(string header)
        {
            try
            {
                ParamesInit(header);

                #region 注册UI事件区域

                this.Loaded += new RoutedEventHandler(TomMenuItem_Loaded);

                #endregion
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "OKButton_Click", ex.ToString(), header);
            }
            finally
            {
            }
        }

        #endregion

        #region 初始化

        /// <summary>
        /// 参数初始化
        /// </summary>
        /// <param name="header"></param>
        void ParamesInit(string header)
        {
            try
            {
                this.brush = this.Background;
                this.Cursor = System.Windows.Input.Cursors.Hand;
                this.FontSize = 12;
                this.Header = header;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ParamesInit", ex.ToString(), header);
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

        public TbgMenuItem(string header, string imageName)
            : this(header)
        {
            try
            {
                //设置下拉菜单的图标
                this.Icon = new Image() { Width = 20, Height = 20, Source = CommonMethod.GetImageSource(imageName) };
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TbgMenuItem", ex.ToString(), header, imageName);
            }
            finally
            {
            }
        }

        #endregion
    }
}
