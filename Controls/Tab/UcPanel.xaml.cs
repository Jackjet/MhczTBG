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

namespace MhczTBG.Controls.Tab
{
    /// <summary>
    /// UcPanel.xaml 的交互逻辑
    /// </summary>
    public partial class UcPanel : UserControl
    {
        #region 构造函数
        
        public UcPanel()
        {
            InitializeComponent();

            #region 注册事件区域

            this.SizeChanged += new SizeChangedEventHandler(UcPanel_SizeChanged);
            this.Loaded += new RoutedEventHandler(UcPanel_Loaded);

            #endregion

        }
        
        #endregion

        #region UI事件区域

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void UcPanel_Loaded(object sender, RoutedEventArgs e)
        {
            this.btnOk.Background = CommonMethod.GetImagebrush("save");
            this.btnClose.Background = CommonMethod.GetImagebrush("close");
        }

        /// <summary>
        /// 调整bord的大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void UcPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.scroMain.Height = this.borMain.ActualHeight;
        }

        #endregion

        #region UI样式设计

        /// <summary>
        /// 鼠标进入时,编辑的样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StackPanel_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as Panel).Background = this.Resources["soloBlue"] as SolidColorBrush;
        }

        /// <summary>
        /// 鼠标进入时,取消的样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StackPanel_MouseEnter_1(object sender, MouseEventArgs e)
        {
            (sender as Panel).Background = this.Resources["soloYellow"] as SolidColorBrush;
        }

        /// <summary>
        /// 鼠标离开时，编辑的样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StackPanel_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Panel).Background = this.Resources["linBlue"] as LinearGradientBrush;
        }

        /// <summary>
        /// 鼠标离开时，取消的样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StackPanel_MouseLeave_1(object sender, MouseEventArgs e)
        {
            (sender as Panel).Background = this.Resources["linYellow"] as LinearGradientBrush;
        }

        #endregion

        #region 标题设置（public）

        /// <summary>
        /// 标题设置
        /// </summary>
        /// <param name="strOperate">操作信息</param>
        /// <param name="strTittle1">一级标题</param>
        /// <param name="strTittle2">二级标题</param>
        public void SetHeader(string strOperate, string strTittle1, string strTittle2)
        {
            this.txtOperate.Text = strOperate;
            this.txtTittl1.Text = strTittle1;
            this.txtTittl2.Text = strTittle2;
        }

        #endregion
    }
}
