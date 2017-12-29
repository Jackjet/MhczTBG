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
using MhczTBG.Common;

namespace MhczTBG.Controls.CustomButton
{
    /// <summary>
    /// UCButtonDaoHang.xaml 的交互逻辑
    /// </summary>
    public partial class UCButtonDaoHang : UserControl
    {

        #region 变量

        /// <summary>
        /// 按钮高度的基数（参照该控件的高度）
        /// </summary>
        const double BtnWidthCan = 7.00 / 13.00;

        /// <summary>
        /// 按钮宽度的基数（参照该控件的宽度）
        /// </summary>
        const double BtnHeightCan = 5.00 / 13.00;

        /// <summary>
        /// 头像尺寸的基数（参照该控件的尺寸）
        /// </summary>
        const double HandCan = 5.00 / 13.00;

        /// <summary>
        /// 向左的偏移（参照该控件的左偏移）
        /// </summary>
        const double PadingLeftCan = 3.00 / 2.00;

        /// <summary>
        /// 向上的偏移（参照该控件的上偏移）
        /// </summary>
        const double PadingTopCan = 9.00 / 10.00;

        /// <summary>
        /// 向下的偏移（参照该控件的下偏移）
        /// </summary>
        const double PadingBottomCan = 6.00 / 25.00;

        string text;
        /// <summary>
        /// 第一个按钮显示的名称
        /// </summary>
        public string Text1
        {
            get { return text; }
            set
            {
                if (!string.IsNullOrEmpty(value) && textList.Count > 0)
                {
                    textList[0].Text = value;
                }
                text = value;
            }
        }

        string text2;
        /// <summary>
        /// 第二个按钮显示的名称
        /// </summary>
        public string Text2
        {
            get { return text2; }
            set
            {
                if (!string.IsNullOrEmpty(value) && textList.Count > 1)
                {
                    textList[1].Text = value;
                }
                text2 = value;
            }
        }

        string text3;
        /// <summary>
        /// 第三个按钮显示的名称
        /// </summary>
        public string Text3
        {
            get { return text3; }
            set
            {
                if (!string.IsNullOrEmpty(value) && textList.Count > 2)
                {
                    textList[2].Text = value;
                }
                text3 = value;
            }
        }

        string text4;
        /// <summary>
        /// 第四个按钮显示的名称
        /// </summary>
        public string Text4
        {
            get { return text4; }
            set
            {
                if (!string.IsNullOrEmpty(value) && textList.Count > 3)
                {
                    textList[3].Text = value;
                }
                text4 = value;
            }
        }

        string text5;
        /// <summary>
        /// 第五个按钮显示的名称
        /// </summary>
        public string Text5
        {
            get { return text5; }
            set
            {
                if (!string.IsNullOrEmpty(value) && textList.Count > 4)
                {
                    textList[4].Text = value;
                }
                text5 = value;
            }
        }
        /// <summary>
        /// 文本集
        /// </summary>
        List<TextBlock> textList = new List<TextBlock>();

        /// <summary>
        /// 选择的项
        /// </summary>
        Path SelectPath = null;

        #endregion

        #region 自定义委托事件

        public delegate void ClckEventHandle(string text);
        /// <summary>
        /// 当点击其中的按钮之后所激发的事件（可以从中获取数据）
        /// </summary>
        public event ClckEventHandle ClckEvent = null;

        #endregion

        #region 构造函数

        public UCButtonDaoHang()
        {
            try
            {
                InitializeComponent();
                this.SizeChanged += new SizeChangedEventHandler(UCButtonDaoHang_SizeChanged);
                this.LayoutUpdated += new EventHandler(UCButtonDaoHang_LayoutUpdated);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "UCButtonDaoHang", ex.ToString());
            }
            finally
            {
            }
        }

        void UCButtonDaoHang_LayoutUpdated(object sender, EventArgs e)
        {
            try
            {
                ///加载的时候重新校验数据
                if (textList.Count > 4)
                {
                    this.textList[0].Text = Text1 == null ? "" : Text1;
                    this.textList[1].Text = Text2 == null ? "" : Text2;
                    this.textList[2].Text = Text3 == null ? "" : Text3;
                    this.textList[3].Text = Text4 == null ? "" : Text4;
                    this.textList[4].Text = Text5 == null ? "" : Text5;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "UCButtonDaoHang_LayoutUpdated", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion

        #region UI事件区域

        void UCButtonDaoHang_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                if (this.Width > 0 && this.Height > 0)
                {
                    #region 设置按钮的尺寸

                    this.btn.Height = this.Width * BtnWidthCan;
                    this.btn.Width = this.Height * BtnHeightCan;

                    #endregion

                    #region 设置头像的尺寸

                    this.ellTou.Height = this.Width * HandCan;
                    this.ellTou.Width = this.Width * HandCan;

                    #endregion


                    //该控件的偏移设置
                    this.Padding = new Thickness(this.btn.Width * PadingLeftCan, this.btn.Width * PadingTopCan, 0, this.btn.Width * PadingBottomCan);

                    //头像的偏移设置
                    this.ellTou.Margin = new Thickness(-this.btn.Width * PadingLeftCan, -this.btn.Width * PadingTopCan, 0, -this.btn.Width * PadingBottomCan);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "UCButtonDaoHang_SizeChanged", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }


        /// <summary>
        /// 鼠标左键按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (this.ClckEvent != null)
                {
                    //恢复原来的颜色
                    if (SelectPath != null) SelectPath.Fill = this.Resources["imageBrush"] as Brush;

                    if (sender is Path && (sender as Path).Tag != null && (sender as Path).Tag is TextBlock)
                    {
                        //获取选中的按钮
                        SelectPath = sender as Path;
                        //选中之后的背景
                        SelectPath.Fill = this.Resources["MouseClck"] as Brush;
                        this.ClckEvent(((sender as Path).Tag as TextBlock).Text);
                    }
                    else if (sender is TextBlock && (sender as TextBlock).Parent is Grid && ((sender as TextBlock).Parent as Grid).Children.Count > 1 && ((sender as TextBlock).Parent as Grid).Children[0] is Path)
                    {
                        //获取选中的按钮
                        var path = ((sender as TextBlock).Parent as Grid).Children[0] as Path;
                        SelectPath = path;
                        //选中之后的背景
                        SelectPath.Fill = this.Resources["MouseClck"] as Brush;
                        this.ClckEvent((sender as TextBlock).Text);
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Grid_MouseLeftButtonDown", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 路径面板鼠标进入事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Path_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                //如果是path,直接改变背景
                if (sender is Path && !sender.Equals(SelectPath))
                {
                    var path = (sender as Path) as Path;
                    path.Fill = this.Resources["MouseEnterLin"] as Brush;
                }
                else if (sender is TextBlock && (sender as TextBlock).Parent is Grid && ((sender as TextBlock).Parent as Grid).Children.Count > 1 && ((sender as TextBlock).Parent as Grid).Children[0] is Path && !(((sender as TextBlock).Parent as Grid).Children[0] as Path).Equals(SelectPath))
                {
                    var path = ((sender as TextBlock).Parent as Grid).Children[0] as Path;
                    path.Fill = this.Resources["MouseEnterLin"] as Brush;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Path_MouseEnter", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 路径面板鼠标离开事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Path_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                //如果是path,直接改变背景
                if (sender is Path && !sender.Equals(SelectPath))
                {
                    var path = (sender as Path) as Path;
                    path.Fill = this.Resources["imageBrush"] as Brush;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Path_MouseLeave", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 路径面板加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Path_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Path && (sender as Path).Parent is Grid && ((sender as Path).Parent as Grid).Children.Count > 1 && ((sender as Path).Parent as Grid).Children[1] is TextBlock)
                {
                    (sender as Path).Tag = ((sender as Path).Parent as Grid).Children[1] as TextBlock;
                    textList.Add((((sender as Path).Parent as Grid).Children[1]) as TextBlock);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Path_Loaded", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion
    }
}
