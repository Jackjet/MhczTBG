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

namespace MhczTBG.Controls.DragView
{
    /// <summary>
    /// TabChange2xaml.xaml 的交互逻辑
    /// </summary>
    public partial class TabChange2xaml : UserControl
    {
        /// <summary>
        /// 子项集合
        /// </summary>
        Dictionary<Image, FrameworkElement> elementList = new Dictionary<Image, FrameworkElement>();

        /// <summary>
        /// 左右两侧容器尺寸比例
        /// </summary>
        double LEFTRIGHT = 3 / 4;

        /// <summary>
        /// 判定当前子项是否可以拖动
        /// </summary>
        bool isDrag = false;

        /// <summary>
        /// 子项拖动时的起始位置
        /// </summary>
        Point StartPoint = default(Point);

        /// <summary>
        /// 子项拖动后的终止位置
        /// </summary>
        Point EndPoint = default(Point);

        /// <summary>
        /// 所选择的项
        /// </summary>
        FrameworkElement selectFrameElement = null;

        #region 构造函数

        public TabChange2xaml()
        {
            InitializeComponent();
        }

        #region UI事件区域

        #endregion

        #endregion

        #region 辅助方法

        /// <summary>
        /// 添加子项
        /// </summary>
        /// <param name="element">要加载的子项</param>      
        public void ItemsAdd(FrameworkElement element)
        {
            Image image = RenderVisaulToBitmap(element);
            //集合添加元素
            this.elementList.Add(image, element);

            ////加载第一个元素
            //if (this.borLeft.Child == null && element != null) this.borLeft.Child = element;
            //else if (this.gridRight.Children.Count <= 3) RightItemPositionSetting(image);

            //this.ElementDragSetting(image);
        }

        /// <summary>
        /// 右容器添加元素
        /// </summary>
        /// <param name="element"></param>
        public void RightItemPositionSetting(Image element)
        {
            //判断（第一个子项默认位置为0，其他子项位置进行设置）
            Grid.SetRow(element, this.gridRight.Children.Count);
            //右容器添加元素
            this.gridRight.Children.Add(element);
        }

        public void ElementDragSetting(Image element)
        {
            element.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(element_MouseLeftButtonDown);
            element.MouseMove += new MouseEventHandler(element_MouseMove);
            element.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(element_MouseLeftButtonUp);
        }

        #endregion


        #region 子项事件区域

        /// <summary>
        /// 标识是否可以拖动对象（子项点击）
        /// </summary>       
        private void element_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var element = sender as Image;
            //获取父容器

            //如果为右侧容器，则可以拖动
            if (element.Parent.Equals(this.gridRight))
            {
                //标示为可以拖动
                isDrag = true;
                //记录起始位置
                StartPoint = e.GetPosition(this.LayoutRoot);

                selectFrameElement = element;
            }
        }

        /// <summary>
        /// 子项相应鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void element_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrag && (sender as Image).Equals(sender) && (sender as Image).Parent.Equals(this.gridRight))
            {
                EndPoint = e.GetPosition(this.LayoutRoot);

                //计算X、Y轴起始点与终止点之间的相对偏移量
                double y = EndPoint.Y - StartPoint.Y;
                double x = EndPoint.X - StartPoint.X;
                //存储拖动元素的偏移参数
                Thickness margin = default(Thickness);
                if (sender is Image) margin = (sender as Image).Margin;
                else return;

                //计算新的Margin
                Thickness newMargin = new Thickness()
                {
                    Left = margin.Left + x,
                    Top = margin.Top + y,
                    Right = margin.Right - x,
                    Bottom = margin.Bottom - y
                };

                //设置移动效果
                (sender as Image).Margin = newMargin;

                //开始位置变为最终的位置
                StartPoint = EndPoint;
            }
        }

        /// <summary>
        /// 子项拖动完成（鼠标点击弹起事件）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void element_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //鼠标点击释放了，不可再拖动
            isDrag = false;

            //判断是否符合切换的条件
            if (StartPoint.X < this.borLeft.ActualWidth)
            {
                //使用动画
                ElementChangePosition(this.borLeft.Child as FrameworkElement, sender as Image);
            }
            else
            {
                //不符合则自动调整
                (sender as Image).Margin = new Thickness(0);
            }
        }

        #endregion

        /// <summary>
        /// 两元素对换位置
        /// </summary>
        public void ElementChangePosition(FrameworkElement element1, Image element2)
        {
            //存放父容器的引用
            Border LeftParent;
            Grid RightLeftParent;
            //子项交换的条件
            if (element1.Parent != null && element2.Parent != null && element1.Parent is Border && element2.Parent is Grid)
            {
                //获取父容器
                LeftParent = element1.Parent as Border;
                RightLeftParent = element2.Parent as Grid;

                //右侧子项的行位置
                int RowP = Grid.GetRow(element2);

                //各自断开与父容器的连接
                LeftParent.Child = null;
                RightLeftParent.Children.Remove(element2);


                //相互添加对方的子项
                LeftParent.Child = elementList[element2];
                Grid.SetRow(element1, RowP);
                RightLeftParent.Children.Add(element1);

                element1.Margin = new Thickness(0);
                element2.Margin = new Thickness(0);
            }
        }

        //将控件转为位图
        Image RenderVisaulToBitmap(Visual vsual)
        {
            var rtb = new RenderTargetBitmap(400, 400,  96, 96, PixelFormats.Default);
            rtb.Render(vsual);

          Image img = new Image() { Source = rtb ,Stretch = Stretch.Fill};
          this.Content = img;
          return img;
        }
    }
}
