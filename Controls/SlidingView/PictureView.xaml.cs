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
using MhczTBG.Controls.CustomButton;
using MhczTBG.Controls.Modules;

namespace MhczTBG.Controls.SlidingView
{
    public partial class PictureView : UserControl
    {
        #region 变量

        /// <summary>
        /// 水平向左的偏移量（衡量向左偏移的量，以负数为基准)
        /// </summary>
        double horLLLeft = 0;

        /// <summary>
        /// 每一页所要滑动的宽度标准
        /// </summary>
        double horOffset = 0;

        /// <summary>
        /// 鼠标点击开始坐标
        /// </summary>
        Point starPosition;

        /// <summary>
        /// 鼠标点击释放结束坐标
        /// </summary>
        Point endPosition;

        /// <summary>
        /// 坐标X滑动参数
        /// </summary>
        double WidhtX = 50;

        int intNow = 1;
        /// <summary>
        /// 默认所在的页
        /// </summary>
        public int IntNow
        {
            get { return intNow; }
            set
            {
                if (value <= pagelist.Items.Count && value > 0)
                {
                    SelectSetting(pagelist.Items[value - 1] as ListBoxItem);
                }
                //this.txtItemNow.Text = value.ToString();
                intNow = value;
            }
        }

        /// <summary>
        /// 选择的项（导航）
        /// </summary>
        ListBoxItem selectBoxItem = null;

        List<FrameworkElement> items;
        /// <summary>
        /// 子项的集合
        /// </summary>
        public List<FrameworkElement> Items
        {
            get
            {
                //子项集合
                if (items == null) items = new List<FrameworkElement>();
                return items;
            }
        }

        /// <summary>
        /// 导航按钮集合
        /// </summary>
        List<ListBoxItem2> listBoxItemList = new List<ListBoxItem2>();

        #endregion

        #region 构造函数

        /// <summary>
        /// 
        /// </summary>
        /// <param name="canSliding">是否可以鼠标切换滑动</param>
        /// <param name="canScroling">是否可以通过鼠标滚动轮滑动</param>
        public PictureView(bool canSliding, bool canScroling)
        {
            try
            {
                InitializeComponent();

                #region 注册事件区域

                //大小更改，调整元素的尺寸
                this.SizeChanged += new SizeChangedEventHandler(MainPage_SizeChanged);

                if (canSliding)
                {
                    //通过鼠标切换滑动
                    this.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(PictureView_PreviewMouseLeftButtonDown);

                    this.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(PictureView_MouseLeftButtonUp);
                }

                if (canScroling)
                {
                    //转动鼠标滚动轮进行预览
                    this.PreviewMouseWheel += new MouseWheelEventHandler(LeftToRight1_PreviewMouseWheel);
                }

                #endregion
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "PictureView", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion

        #region UI事件区域

        /// <summary>
        /// 鼠标进入切换导航按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                (sender as FrameworkElement).Opacity = 1;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Button_MouseEnter", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 鼠标离开切换导航按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                (sender as FrameworkElement).Opacity = 0.6;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Button_MouseLeave", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }


        /// <summary>
        /// 鼠标左键点击按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PictureView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //设置开始坐标
            starPosition = e.GetPosition(this);
        }

        /// <summary>
        /// 鼠标左键点击释放事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PictureView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //设置结束坐标
                endPosition = e.GetPosition(this);
                //获取X轴的像素差
                double distanceX = endPosition.X - starPosition.X;
                //如果像素差大于指定参数则向左滑动
                if (distanceX > WidhtX)
                {
                    Button_Click_1(null, null);
                }
                //如果像素差小于指定参数则向右滑动
                if (distanceX < -WidhtX)
                {
                    Button_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "PictureView_MouseLeftButtonUp", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 滚动鼠标轮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LeftToRight1_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            try
            {
                //每一页所要滑动的宽度标准
                double limit = -(this.spMain.ActualWidth - this.scro.ActualWidth);

                //如果偏移量为最左或最右的临界点则不再执行
                if (horLLLeft < limit || horLLLeft > 0) return;
                //创建一个动画
                TomAnimation tom = new TomAnimation();
                //获取偏移量
                horLLLeft += e.Delta;

                //(两个极端)
                //如果偏移量大于所限制的量，则进行调整
                if (horLLLeft < limit)
                {
                    //调整到右侧临界点
                    horLLLeft = limit;
                    //偏移到右侧临界点
                    tom.MovingX(spMain, horLLLeft, 0.2);
                }
                else if (horLLLeft >= 0)
                {
                    //设置偏移量为0
                    horLLLeft = 0;
                    //快速移动到0的位置（最左侧），一种校准的方式
                    tom.MovingX(spMain, horLLLeft, 0.2);
                }
                else
                {
                    //水平移动（向左或向右）
                    tom.MovingX(spMain, horLLLeft, 0.5);
                }

                //翻页信息
                if (this.scro.ActualWidth != 0) IntNow = (int)(horLLLeft / -this.scro.ActualWidth) + 1;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "LeftToRight1_PreviewMouseWheel", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 父容器大小更改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                SizeInit();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "MainPage_SizeChanged", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 向右滑动一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //最大的偏移量，以负数为基准
                double limit = -(this.spMain.ActualWidth - this.scro.ActualWidth);
                //向右偏移一页，减去一页的宽度
                horLLLeft -= horOffset;
                //创建一个动画
                TomAnimation tom = new TomAnimation();
                //如果偏移量大于所限制的量，则进行调整
                if (horLLLeft < limit)
                {
                    //调整到临界点
                    horLLLeft = limit;
                    //偏移到临界点
                    tom.MovingX(spMain, horLLLeft, 0.2);
                }
                else
                {
                    //向右滑动（指定位移）
                    tom.MovingX(spMain, horLLLeft, 0.5);
                    IntNow++;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Button_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 向左滑动一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                //实例化一个动画类
                TomAnimation tom = new TomAnimation();

                //如果剩余的宽度大于或等于0,,
                if (horLLLeft >= 0)
                {
                    //设置偏移量为0
                    horLLLeft = 0;
                    //快速移动到0的位置（最左侧），一种校准的方式
                    tom.MovingX(spMain, horLLLeft, 0.2);
                }
                else
                {
                    //向左滑动
                    tom.MovingX(spMain, horLLLeft += horOffset, 0.5);
                    IntNow--;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Button_Click_1", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 桌面切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var parent = CommonMethod.GetRootParent(100, this);               
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "window_MouseLeftButtonDown", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion

        #region 添加子项

        /// <summary>
        /// 添加一个元素
        /// </summary>
        /// <param name="item">子项</param>
        public void ItemsAdd(FrameworkElement item)
        {
            try
            {
                if (this.spMain.Children.Contains(item)) return;
                this.items.Add(item);
                this.spMain.Children.Add(item);

                //使用里面的方法进行调整子元素的尺寸
                SizeInit();

                ListBoxItem2 li = null;
                if (this.spMain.Children.Count > 0) li = new ListBoxItem2() { Content = this.spMain.Children.Count.ToString(),Foreground = this.Resources["btnBrush"] as Brush };

                if (item.Tag != null)
                {
                    if (item.Tag.ToString().Contains("$"))
                    {
                        li.Tag = item.Tag.ToString().Split(new char[] { '$' })[0];
                        var d = item.Tag.ToString().Split(new char[] { '$' })[1];
                        li.TagDisplay = new MyButton() { Height = 80, Width = 60, Content = li.Tag, Background = CommonMethod.GetCurrentImagebrush(item.Tag.ToString().Split(new char[] { '$' })[1]) };
                    }
                    else li.Tag = item.Tag;
                }
                listBoxItemList.Add(li);
                li.MouseEnter += new MouseEventHandler(ListBoxItem_MouseEnter);
                li.MouseLeave += new MouseEventHandler(ListBoxItem_MouseLeave);
                li.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(li_PreviewMouseLeftButtonDown);
                this.pagelist.Items.Add(li);

                if (this.spMain.Children.Count == 1) SelectSetting(li);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ItemsAdd", ex.ToString(), item);
            }
            finally
            {
            }
        }

        #endregion

        #region 动画类

        /// <summary>
        /// 动画管理类
        /// </summary>
        public class TomAnimation
        {

            //时间线
            Storyboard storyMovingX = new Storyboard();

            //动画处理
            DoubleAnimationUsingKeyFrames doubllMovingX = new DoubleAnimationUsingKeyFrames();

            /// <summary>
            /// 完成动画时所要执行的一些内容
            /// </summary>
            public EventHandler CompleteEvent;

            /// <summary>
            /// 水平移动
            /// </summary>
            /// <param name="obj">要进行动画的元素</param>
            /// <param name="xDistance">距离</param>
            /// <param name="time">时间</param>
            public void MovingX(FrameworkElement obj, double xDistance, double time)
            {
                try
                {
                    //设置该属性才可以进行动画的启动
                    if (obj.RenderTransform is MatrixTransform)
                    {
                        obj.RenderTransform = new TranslateTransform();
                    }
                    //帧的实例
                    EasingDoubleKeyFrame key1MovingX = new EasingDoubleKeyFrame();
                    //指定这一帧所执行的时间
                    key1MovingX.KeyTime = TimeSpan.FromSeconds(time);
                    //移动的距离
                    key1MovingX.Value = xDistance;
                    //移动帧的集合
                    doubllMovingX.KeyFrames.Add(key1MovingX);

                    //时间线添加帧的集合
                    storyMovingX.Children.Add(doubllMovingX);
                    //执行动画的源
                    Storyboard.SetTarget(doubllMovingX, obj);
                    //帧所对应的更改属性
                    Storyboard.SetTargetProperty(doubllMovingX, new PropertyPath("(UIElement.RenderTransform).( TranslateTransform.X)"));
                    //动画开启
                    storyMovingX.Begin();
                    //完成动画之后执行这个事件，该事件可在外部绑定相应的方法
                    storyMovingX.Completed += (object sender, EventArgs e) =>
                    {
                        if (CompleteEvent != null)
                        {
                            CompleteEvent(null, null);
                            //释放动画资源
                            storyMovingX = null;
                        }
                    };
                }
                catch (Exception ex)
                {
                    MethodLb.CreateLog(this.GetType().FullName, "MovingX", ex.ToString(), obj, xDistance, time);
                }
                finally
                {
                }
            }
        }

        #endregion

        #region 触发器设置

        void li_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                SelectSetting(sender as ListBoxItem);

                ItemToPostion(selectBoxItem);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "li_PreviewMouseLeftButtonDown", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        void ItemToPostion(ListBoxItem listboxItem)
        {
            int selecInt = 0;
            ///获取到导航标题
            if (listboxItem.Content != null && int.TryParse(listboxItem.Content.ToString(), out selecInt))
            {
                //获取偏移位置
                horLLLeft = -(selecInt - 1) * this.scro.ActualWidth;

                //horLLLeft = 

                //创建一个动画
                TomAnimation tom = new TomAnimation();
                tom.MovingX(spMain, horLLLeft, 0.2);

                IntNow = selecInt;
            }
        }

        void SelectSetting(ListBoxItem sender)
        {
            try
            {
                #region 样式设置

                if (selectBoxItem != null) selectBoxItem.Foreground = this.Resources["btnBrush"] as Brush;
                selectBoxItem = sender as ListBoxItem;
                selectBoxItem.Foreground = this.Resources["linSelct"] as Brush;

                if (sender.Tag != null) this.txtTittle.Text = sender.Tag.ToString();

                #endregion
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "OKButton_Click", ex.ToString(), sender);
            }
            finally
            {
            }
        }

        private void ListBoxItem_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                if (!(sender as ListBoxItem).Equals(selectBoxItem))
                {
                    (sender as ListBoxItem).Foreground = this.Resources["linEnter"] as Brush;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ListBoxItem_MouseEnter", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        private void ListBoxItem_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                if (!(sender as ListBoxItem).Equals(selectBoxItem))
                {
                    (sender as ListBoxItem).Foreground = this.Resources["btnBrush"] as Brush;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ListBoxItem_MouseLeave", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 调整各元素的尺寸（并按照尺寸进行设置参数标准）
        /// </summary>
        void SizeInit()
        {
            try
            {
                //每一页所要滑动的宽度标准
                horOffset = this.scro.ActualWidth;

                //设置每一个元素使用父容器的大小
                foreach (var item in spMain.Children)
                {
                    if (scro.ActualWidth > 0 && scro.ActualHeight > 0)
                    {
                        //调整每一个元素的尺寸
                        (item as FrameworkElement).Width = this.scro.ActualWidth;

                        (item as FrameworkElement).Height = this.scro.ActualHeight;

                        if (selectBoxItem != null)
                        {
                            ItemToPostion(selectBoxItem);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "SizeInit", ex.ToString());
            }
            finally
            {
            }
        }

        #region 皮肤更换

        /// <summary>
        /// 皮肤更换
        /// </summary>
        /// <param name="color">指定颜色</param>
        public void SkinChange(Color color)
        {
            try
            {                
                (this.Resources["backBrush"] as LinearGradientBrush).GradientStops[1].Color = color;
                (this.Resources["ArrowBrush"] as SolidColorBrush).Color = color;
                (this.Resources["btnBrush"]as LinearGradientBrush).GradientStops[1].Color  = color;
                foreach (var item in listBoxItemList)
                {
                    if (item != selectBoxItem) item.Foreground = this.Resources["btnBrush"] as LinearGradientBrush;
                }               
                (this.Resources["linEnter"] as LinearGradientBrush).GradientStops[1].Color = color;
                (this.Resources["daohangBrush"] as LinearGradientBrush).GradientStops[0].Color = color;
                (this.Resources["daohangBrush"] as LinearGradientBrush).GradientStops[2].Color = color;
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

        #endregion
    }

    #region 子项类
    
    class ListBoxItem2 : ListBoxItem
    {
        public static readonly DependencyProperty TagDisplayProperty = DependencyProperty.Register(
  "TagDisplay",
  typeof(object),
  typeof(ListBoxItem2),
  new FrameworkPropertyMetadata(null,
      FrameworkPropertyMetadataOptions.AffectsRender, null)
);
        public object TagDisplay
        {
            get { return (object)GetValue(TagDisplayProperty); }
            set { SetValue(TagDisplayProperty, value); }
        }
    }
    
    #endregion
}
