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

namespace MhczTBG.Controls.CustomButton
{
    /// <summary>
    /// Shu.xaml 的交互逻辑
    /// </summary>
    public partial class BtnDaoHang : UserControl
    {
        #region 变量

        /// <summary>
        /// 临时存储所选择的项
        /// </summary>
        UIElement _SelectElement = null;


        StackPanel panel;
        /// <summary>
        /// 装载子项的容器
        /// </summary>
        public StackPanel _Panel
        {
            get { return panel; }
            set
            {
                if (value != null && value is StackPanel)
                {
                    borMain.Child = value;
                    panel = value;
                }
            }
        }

        /// <summary>
        /// 选择项集合
        /// </summary>
        List<Grid> gridList = new List<Grid>();

        int selectIndex;
        /// <summary>
        /// 默认选择项
        /// </summary>
        public int SelectIndex
        {
            get { return selectIndex; }
            set
            {
                selectIndex = value;
            }
        }

        #endregion


        public delegate void ClickEventHandle(string text);
        /// <summary>
        /// 导航按钮点击事件
        /// </summary>
        public event ClickEventHandle _ClickEvent = null;

        #region 构造函数

        public BtnDaoHang()
        {
            try
            {
                InitializeComponent();
              
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "BtnDaoHang", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion


        #region UI事件区域

        /// <summary>
        /// 导航点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void g_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                #region 锁定按钮

                if (_SelectElement != null) _SelectElement.Visibility = System.Windows.Visibility.Collapsed;
                if (sender is Grid && (sender as Grid).Children.Count > 0)
                {
                    var clickGrid = sender as Grid;
                    clickGrid.Children[0].Visibility = System.Windows.Visibility.Visible;
                    clickGrid.Children[0].Opacity = 0.8;
                    _SelectElement = clickGrid.Children[0];
                }

                #endregion

                if (_ClickEvent != null)
                {
                    //获取绑定的文本，返回给调用者
                    if (sender is Grid && (sender as Grid).Tag != null)
                    {
                        _ClickEvent((sender as Grid).Tag.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "g_MouseLeftButtonDown", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 鼠标进入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void g_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                if (sender is Grid && (sender as Grid).Children.Count > 0 && _SelectElement != (sender as Grid).Children[0])
                {
                    var EnterGrid = sender as Grid;
                    EnterGrid.Children[0].Visibility = System.Windows.Visibility.Visible;
                    EnterGrid.Children[0].Opacity = 0.4;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "g_MouseEnter", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 鼠标离开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void g_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                if (sender is Grid && (sender as Grid).Children.Count > 0 && _SelectElement != (sender as Grid).Children[0])
                {
                    var EnterGrid = sender as Grid;
                    EnterGrid.Children[0].Visibility = System.Windows.Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "g_MouseLeave", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        int alreadySelected = -1;

        /// <summary>
        /// 选择项加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void g_Loaded(object sender, RoutedEventArgs e)
        {
            gridList.Add(sender as Grid);

            if (alreadySelected != selectIndex)
            {
                if (gridList.Count > selectIndex)
                    g_MouseLeftButtonDown(gridList[selectIndex], null);
                alreadySelected = selectIndex;
            }
        }

        #endregion


    }
}
