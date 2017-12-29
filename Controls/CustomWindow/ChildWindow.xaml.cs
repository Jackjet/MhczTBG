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
    /// Interaction logic for ChildWindow.xaml
    /// </summary>
    public partial class ChildWindow : Window
    {
        #region 变量

        string tittle;
        /// <summary>
        /// 标题
        /// </summary>
        public string Tittle
        {
            get { return tittle; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.title.Text = value;

                    tittle = value;
                }
            }
        }

        FrameworkElement child;
        /// <summary>
        /// 加载视图内容
        /// </summary>
        public FrameworkElement Child
        {
            get { return child; }
            set
            {
                if (value != null)
                {
                    this.borMain.Child = Child;
                    child = value;
                }
            }
        }
        /// <summary>
        /// 判断最大化和默认显示状态
        /// </summary>
        bool flag = true;

        #endregion

        #region 构造函数

        public ChildWindow()
        {
            try
            {
                InitializeComponent();

                this.title.Text = this.Title; 
                this.min_btn.Click += new RoutedEventHandler(min_btn_Click);
                this.max_btn.Click += new RoutedEventHandler(max_btn_Click);
                this.close_btn.Click += new RoutedEventHandler(close_btn_Click); 
                this.header.MouseLeftButtonDown += new MouseButtonEventHandler(header_MouseLeftButtonDown);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ChildWindow", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion

        #region UI事件区域

        /// <summary>
        /// 鼠标左键拖动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "header_MouseLeftButtonDown", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 关闭按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void close_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "close_btn_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 最大化按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void max_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (flag)
                {
                    this.WindowState = WindowState.Maximized;
                }
                else
                {
                    this.WindowState = WindowState.Normal;
                }
                flag = !flag;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "max_btn_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 最小化按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void min_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.WindowState = WindowState.Minimized;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "min_btn_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

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
                (this.Resources["backBrush"] as LinearGradientBrush).GradientStops[1].Color = color;
                (this.Resources["backBorder"] as LinearGradientBrush).GradientStops[0].Color = color;
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
