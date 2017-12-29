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
using MhczTBG;

namespace MhczTBG.Controls
{
    /// <summary>
    /// DataButton.xaml 的交互逻辑
    /// </summary>
    public partial class DataButton : UserControl
    {

        #region 声明变量

        Button beforeButton = null;

        #endregion

        #region 自定义事件委托

        public delegate void ClickEventHandle(string text);
        /// <summary>
        /// 导航点击事件
        /// </summary>
        public event ClickEventHandle _ClickEvent = null;

        int selectedIndex;
        /// <summary>
        /// 选择项
        /// </summary>
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                if (value > -1 && value < stackMonth.Children.Count)
                {
                    var button = this.stackMonth.Children[value];
                    one_Click(button, null);
                }
                selectedIndex = value;
            }
        }

        #endregion

        #region 构造函数
        public DataButton()
        {
            InitializeComponent();
            ParmesInit();
        }
        #endregion


        #region 事件区域
        /// <summary>
        /// 初始化参数
        /// </summary>

        void ParmesInit()
        {
            try
            {
                this.cboxYear.Style = MhczTBG.StyleResource.TongXinStyle.Instacnce.Resources["ComboBoxStyle1"] as Style;
                this.btnAddGuzhang.Style = this.btnShiTu.Style = MhczTBG.StyleResource.TongXinStyle.Instacnce.Resources["btnStyle9"] as Style;
                for (int i = 0; i < 5; i++)
                {
                    ComboBoxItem conboxitem = new ComboBoxItem();
                    conboxitem.Content = string.Format("{0}年", DateTime.Now.Year - i);
                    conboxitem.Tag = DateTime.Now.Year - i;
                    this.cboxYear.Items.Add(conboxitem);

                }
                this.cboxYear.SelectedIndex = 0;

                one.Style = two.Style = three.Style = fore.Style = five.Style = six.Style = seven.Style = eight.Style = nine.Style = ten.Style
                    = eleven.Style = twelve.Style = MhczTBG.StyleResource.TongXinStyle.Instacnce.Resources["btnStyle10"] as Style;

            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ParmesInit", ex.ToString());
            }
            finally
            {
            }

        }
        #endregion

        private void one_Click(object sender, RoutedEventArgs e)
        {
            var clickButton = (sender as Button);

            if (beforeButton != null)
                beforeButton.IsEnabled = true;

            if (this._ClickEvent != null)
            {
                string message = string.Empty;

                var selectedItem = (this.cboxYear.SelectedItem as ComboBoxItem);

                var selectedYear = Convert.ToString(selectedItem.Content);

                var selectedMonth = Convert.ToString(clickButton.Content);

                message = selectedYear + selectedMonth;

                this._ClickEvent(message);
            }

            clickButton.IsEnabled = false;

            beforeButton = clickButton;
        }

        private void cboxYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this._ClickEvent != null)
            {
                string message = string.Empty;

                var selectedItem = (this.cboxYear.SelectedItem as ComboBoxItem);

                var selectedYear = Convert.ToString(selectedItem.Content);

                var selectedMonth = Convert.ToString(beforeButton.Content);

                message = selectedYear + selectedMonth;

                this._ClickEvent(message);
            }
        }
    }
}
