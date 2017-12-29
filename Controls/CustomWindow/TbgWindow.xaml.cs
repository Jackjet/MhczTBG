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
using System.Drawing;
using MhczTBG.Common;

namespace MhczTBG.Controls.CustomWindow
{
    /// <summary>
    /// TbgWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TbgWindow : Window
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public TbgWindow()
        {
            try
            {
                InitializeComponent();
                this.Loaded += new RoutedEventHandler(TbgWindow_Loaded);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TbgWindow", ex.ToString());
            }
            finally
            {
            }
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TbgWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.min.Source = CommonMethod.GetImageSource(MhczTBG.Properties.Resources.最小化);
                this.huan.Source = CommonMethod.GetImageSource(MhczTBG.Properties.Resources.Huan);
                this.close.Source = CommonMethod.GetImageSource(MhczTBG.Properties.Resources.关闭);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TbgWindow_Loaded", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion

        #region UI事件区域

        /// <summary>
        /// 向下拉时，对高度进行调整
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            try
            {
                System.Windows.Point position = Mouse.GetPosition(this);
                if (position.X > 10 && position.Y > 10)
                {
                    this.Height = position.Y;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Thumb_DragDelta", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 向右拉时，对高度进行调整
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Thumb_DragDelta_1(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            try
            {
                System.Windows.Point position = Mouse.GetPosition(this);
                if (position.X > 10 && position.Y > 10)
                {
                    this.Width = position.X;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Thumb_DragDelta_1", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 向右下角拉时，调整宽度和高度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Thumb_DragDelta_2(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            try
            {
                System.Windows.Point position = Mouse.GetPosition(this);
                if (position.X > 10 && position.Y > 10)
                {
                    this.Width = position.X;
                    this.Height = position.Y;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Thumb_DragDelta_2", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 通过这个border可以拖动本窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    DragMove();
                }

                if (e.ClickCount >= 2)
                {
                    huan_MouseLeftButtonDown(null, null);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Border_MouseDown", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 最小化按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void min_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.WindowState = System.Windows.WindowState.Minimized;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "min_MouseLeftButtonDown", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 还原|最大化按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void huan_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (this.WindowState == System.Windows.WindowState.Maximized)
                {
                    this.WindowState = System.Windows.WindowState.Normal;
                    this.huan.Source = CommonMethod.GetImageSource(MhczTBG.Properties.Resources.Huan);
                }
                else
                {
                    this.WindowState = System.Windows.WindowState.Maximized;
                    this.huan.Source = CommonMethod.GetImageSource(MhczTBG.Properties.Resources.Max);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "huan_MouseLeftButtonDown", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void close_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "close_MouseLeftButtonDown", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 窗体边框颜色设置
        /// </summary>
        /// <param name="brushKeyName">画刷资源的key名称（定义在当前窗体前台Xaml里）</param>
        /// <param name="intOpacity">透明度</param>
        public void WindowBorderBrushSetting(string brushKeyName, double intOpacity)
        {
            try
            {
                //获取画刷
                System.Windows.Media.Brush bBrush = (System.Windows.Media.Brush)this.Resources[brushKeyName];
                //设置背景
                this.borRound.BorderBrush = this.borTittle.Background = bBrush;
                //设置透明度
                this.borRound.Opacity = this.borTittle.Opacity = intOpacity;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "WindowBorderBrushSetting", ex.ToString(), brushKeyName, intOpacity);
            }
            finally
            {
            }
        }
        /// <summary>
        ///  窗体边框颜色设置
        /// </summary>
        /// <param name="brush">画刷资源的key名称（定义在当前窗体前台Xaml里）</param>
        /// <param name="intOpacity">透明度</param>
        public void WindowBorderBrushSetting(System.Windows.Media.Brush brush, double intOpacity)
        {
            try
            {
                //获取画刷
                System.Windows.Media.Brush bBrush = brush;
                //设置背景
                this.borRound.BorderBrush = this.borTittle.Background = bBrush;
                //设置透明度
                this.borRound.Opacity = this.borTittle.Opacity = intOpacity;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "WindowBorderBrushSetting", ex.ToString(), brush, intOpacity);
            }
            finally
            {
            }
        }

        #region 旧方案（截图）

        //System.Windows.Forms.PictureBox GetPart(string imageUri, CutPostion cutPosition)
        //{
        //    System.Drawing.Image image = System.Drawing.Image.FromFile(Environment.CurrentDirectory.Replace("bin\\Debug", imageUri));

        //    int imageWidth = 0;
        //    int imageHeigh = 0;

        //    int cutX = 0;
        //    int cutY = 0;

        //    switch (cutPosition)
        //    {
        //        case CutPostion.Left:
        //            imageWidth = image.Width / 4;
        //            imageHeigh = image.Height;
        //            break;

        //        case CutPostion.Top:
        //            imageWidth = image.Width;
        //            imageHeigh = image.Height / 4;
        //            break;

        //        case CutPostion.Right:
        //            imageWidth = image.Width / 4;
        //            imageHeigh = image.Height;
        //            cutX = 3 * image.Width / 4;
        //            break;

        //        case CutPostion.Bottom:
        //            imageWidth = image.Width;
        //            imageHeigh = image.Height / 4;
        //            cutY = 3 * image.Height / 4;
        //            break;
        //    }


        //    System.Windows.Forms.PictureBox pic = new System.Windows.Forms.PictureBox();

        //    pic.BackgroundImage = new Bitmap(this.CutPartImage(image, imageWidth, imageHeigh, cutX, cutY), new System.Drawing.Size(pic.Width, pic.Height * 2));

        //    return pic;
        //}

        //Bitmap CutPartImage(System.Drawing.Image image, int width, int height, int offsetX, int offsetY)
        //{
        //    Bitmap bitmap = new Bitmap(image);

        //    Bitmap _realbitmap = null;
        //    _realbitmap = new Bitmap(width, height);
        //    using (Graphics g = Graphics.FromImage(_realbitmap))
        //    {
        //        System.Drawing.Rectangle recbit = new System.Drawing.Rectangle(0, 0, width, height);
        //        System.Drawing.Rectangle recreal = new System.Drawing.Rectangle(0 + offsetX, 0 + offsetY, width, height);
        //        g.DrawImage(bitmap, recbit, recreal, GraphicsUnit.Pixel);
        //    }
        //    bitmap.Dispose();
        //    return _realbitmap;
        //}

        #endregion


        #endregion
    }

    #region 旧方案（截图）

    //public enum CutPostion
    //{
    //    Left,
    //    Top,
    //    Right,
    //    Bottom,
    //}

    #endregion
}
