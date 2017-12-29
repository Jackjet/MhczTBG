using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using MhczTBG.Controls;

namespace MhczTBG.Common
{
    public class DragManage
    {
        System.Windows.Controls.Button btn = new System.Windows.Controls.Button();

        #region 自定义完成事件

        public delegate void DragRightToLeftCompleteHandle(DragManage dragManage, FrameworkElement element);
        /// <summary>
        ///元素从左拖到右完成事件
        /// </summary>
        public event DragRightToLeftCompleteHandle DragRightToLeftComplete;

        public delegate void DragLeftToRightCompleteHandle(DragManage dragManage, FrameworkElement element);
        /// <summary>
        ///元素从右拖到左完成事件
        /// </summary>
        public event DragLeftToRightCompleteHandle DragLeftToRightComplete;

        public delegate void DragLeftToCenterCompleteHandle(DragManage dragManage, FrameworkElement element);
        /// <summary>
        ///左 -->中
        /// </summary>
        public event DragLeftToCenterCompleteHandle DragLeftToCenterComplete;

        public delegate void DragCenterToLeftCompleteHandle(DragManage dragManage, FrameworkElement element);
        /// <summary>
        ///中 ———>左
        /// </summary>
        public event DragCenterToLeftCompleteHandle DragCenterToLeftComplete;

        public delegate void DragCenterToRightCompleteHandle(DragManage dragManage, FrameworkElement element);
        /// <summary>
        /// 中-->右
        /// </summary>     
        public event DragCenterToRightCompleteHandle DragCenterToRightComplete;

        public delegate void DragRightToCenterCompleteHandle(DragManage dragManage, FrameworkElement element);
        /// <summary>
        /// 右到中
        /// </summary>
        public event DragRightToCenterCompleteHandle DragRightToCenterComplete;

        #endregion

        #region 变量
        /// <summary>
        /// 是否允许拖动
        /// </summary>
        private bool isDrag = false;

        /// <summary>
        /// 拖动起始坐标
        /// </summary>
        private System.Windows.Point StartPoint;

        /// <summary>
        /// 拖动终止坐标
        /// </summary>
        private System.Windows.Point EndPoint;


        /// <summary>
        /// 关联到的父级区域
        /// </summary>
        Grid gridLayouRoot;

        /// <summary>
        /// 字段区域
        /// </summary>
        StackPanel stackPanel1;

        #endregion

        #region 构造函数

        /// <summary>
        /// 拖动管理的构造函数
        /// </summary>
        /// <param name="LayouRoot">拖动对象的终极父容器</param>
        /// <param name="element">拖动的对象</param>
        /// <param name="stack1">字段区域</param>
        public DragManage(Grid LayouRoot, FrameworkElement element, StackPanel stack1)
        {
            //控件关联
            this.ControlInit(LayouRoot, stack1);

            //给拖动项注册拖动事件
            this.DropEventInit(element);
        }

        #endregion

        #region 引用外部控件

        /// <summary>
        /// 控件关联
        /// </summary>
        /// <param name="LayouRoot">拖动对象的终极父容器</param>
        /// <param name="stack1">字段区域</param>
        private void ControlInit(Grid LayouRoot, StackPanel stack1)
        {
            try
            {
                //拖动对象的终极父容器
                this.gridLayouRoot = LayouRoot;
                //字段区域
                this.stackPanel1 = stack1;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ControlInit", ex.ToString(), LayouRoot, stack1);
            }
        }

        #endregion

        #region 拖动事件

        /// <summary>
        /// 元素点击按下时
        /// </summary>
        void element_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                FrameworkElement element = sender as FrameworkElement;
                //拖动起始坐标
                StartPoint = e.GetPosition(gridLayouRoot);
                //鼠标强制捕获此元素
                element.CaptureMouse();
                //允许拖动
                isDrag = true;

                element.Margin = new Thickness(10, 0, 10, 10);
                element.Cursor = Cursors.Hand;
                GetRootParent(element, 4).SetValue(Canvas.ZIndexProperty, 100);
                element.SetValue(Canvas.ZIndexProperty, 100);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "element_MouseLeftButtonDown", ex.ToString(), sender, e);
            }
        }

        /// <summary>
        /// 鼠标对着元素移动时
        /// </summary>
        void element_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                FrameworkElement element = sender as FrameworkElement;

                if (isDrag)
                {
                    EndPoint = e.GetPosition(gridLayouRoot);

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
                MethodLb.CreateLog(this.GetType().FullName, "element_MouseMove", ex.ToString(), sender, e);
            }
        }

        /// <summary>
        /// 元素点击弹回时
        /// </summary>
        void element_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
               
                if (sender is TongJiItem)
                {
                    TongJiItem item = sender as TongJiItem;

                    isDrag = false;

                    #region 拖动完毕时发生变化

                    item.Cursor = Cursors.Arrow;
                    GetRootParent(item, 4).SetValue(Canvas.ZIndexProperty, 0);
                    item.SetValue(Canvas.ZIndexProperty, 0);

                    if (item.StrStyle == "borStyle1")
                    {
                        //拖到行区域
                        if (item.Margin.Left > gridLayouRoot.ColumnDefinitions[0].ActualWidth + gridLayouRoot.ColumnDefinitions[2].ActualWidth)
                        {
                            if (DragLeftToRightComplete != null)
                            {
                                DragLeftToRightComplete(this, item);
                            }
                        }
                        //拖到列区域
                        else if (item.Margin.Left > gridLayouRoot.ColumnDefinitions[0].ActualWidth)
                        {
                            DragLeftToCenterComplete(this, item);
                        }
                        else
                        {
                            item.Margin = new Thickness(0);
                        }

                    }
                    else if (item.StrStyle == "borStyle2")
                    {
                        //还原
                        if (item.Margin.Right > gridLayouRoot.ColumnDefinitions[2].ActualWidth)
                        {
                            if (DragCenterToLeftComplete != null)
                            {
                                DragCenterToLeftComplete(this, item);
                            }

                        }
                        else if (item.Margin.Left > gridLayouRoot.ColumnDefinitions[2].ActualWidth)
                        {
                            if (DragCenterToRightComplete != null)
                            {
                                DragCenterToRightComplete(this, item);
                            }
                        }
                        else
                        {
                            item.Margin = new Thickness(0);
                        }
                    }
                    else
                    {
                        //还原
                        if (item.Margin.Right > gridLayouRoot.ColumnDefinitions[3].ActualWidth + gridLayouRoot.ColumnDefinitions[4].ActualWidth + gridLayouRoot.ColumnDefinitions[5].ActualWidth)
                        {
                            if (DragRightToLeftComplete != null)
                            {
                                DragRightToLeftComplete(this, item);
                            }
                        }
                        else if (item.Margin.Right > gridLayouRoot.ColumnDefinitions[4].ActualWidth + gridLayouRoot.ColumnDefinitions[5].ActualWidth)
                        {
                            if (DragRightToCenterComplete != null)
                            {
                                DragRightToCenterComplete(this, item);
                            }
                        }
                        else
                        {
                            item.Margin = new Thickness(0);
                        }
                    }

                    #endregion

                    //释放焦点
                    item.ReleaseMouseCapture();

                    item.Margin = new Thickness(0);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "element_MouseLeftButtonUp", ex.ToString(), sender, e);
            }
        }

        #endregion

        #region 获取父容器

        /// <summary>
        /// 获取到最终的父容器
        /// </summary>
        FrameworkElement GetRootParent(FrameworkElement element, int intCount)
        {
            FrameworkElement fraParent = null;
            try
            {
                for (int i = 0; i < intCount; i++)
                {
                    element = element.Parent as FrameworkElement;
                    string strName = element.Name;
                }
                fraParent = element;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "GetRootParent", ex.ToString(), element, intCount);
            }
            return fraParent;
        }
        #endregion

        #region 给拖动项注册拖动事件

        /// <summary>
        /// 给拖动项注册拖动事件
        /// </summary>
        /// <param name="item"></param>
        public void DropEventInit(FrameworkElement item)
        {
            try
            {
                item.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(element_MouseLeftButtonDown);
                item.PreviewMouseMove += new MouseEventHandler(element_MouseMove);
                item.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(element_MouseLeftButtonUp);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DropEventInit", ex.ToString(), item);
            }
        }

        #endregion
    }
}
