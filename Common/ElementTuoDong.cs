using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

namespace MhczTBG.Common
{
    public class ElementTuoDong
    {
        #region 变量

        /// <summary>
        /// 指定子元素的拖动范围
        /// </summary>
        Grid layout = null;

        /// <summary>
        /// 标识是否可以拖动对象
        /// </summary>
        private bool isDrag = false;

        /// <summary>
        /// 拖动的起始位置
        /// </summary>
        private Point StartPoint;

        /// <summary>
        /// 拖动的结束位置
        /// </summary>
        private Point EndPoint;

        #endregion

        public ElementTuoDong(UIElement element, Grid LayoutRoot)
        {
            try
            {
                element.MouseLeftButtonDown += new MouseButtonEventHandler(StackPanel_MouseLeftButtonDown);
                element.MouseLeftButtonUp += new MouseButtonEventHandler(StackPanel_MouseLeftButtonUp);
                element.MouseMove += new MouseEventHandler(StackPanel_MouseMove);
                layout = LayoutRoot;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ElementTuoDong", ex.ToString(), element, LayoutRoot);
            }
            finally
            {
            }
        }



        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                FrameworkElement element = sender as FrameworkElement;
                StartPoint = e.GetPosition(layout);
                element.CaptureMouse();
                isDrag = true;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "StackPanel_MouseLeftButtonDown", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        private void StackPanel_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                FrameworkElement element = sender as FrameworkElement;
                if (isDrag)
                {
                    EndPoint = e.GetPosition(layout);

                    //计算X、Y轴起始点与终止点之间的相对偏移量
                    double y = EndPoint.Y - StartPoint.Y;
                    double x = EndPoint.X - StartPoint.X;

                    Thickness margin = element.Margin;

                    //计算新的Margin
                    Thickness newMargin = new Thickness()
                    {
                        Left = margin.Left + x,
                        Top = margin.Top + y,
                        Right = margin.Right - x,
                        Bottom = margin.Bottom - y
                    };
                    element.Margin = newMargin;
                    StartPoint = EndPoint;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "StackPanel_MouseMove", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        private void StackPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //MethodLb.CreateLog(this.GetType().FullName, "StackPanel_MouseLeftButtonUp", ex.ToString(), sender, e);
        }
    }


}
