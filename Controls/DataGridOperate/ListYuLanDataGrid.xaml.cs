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
using MhczTBG.Controls.CustomWindow;

namespace MhczTBG.Controls.DataGridOperate
{
    /// <summary>
    /// ListYuLanDataGrid.xaml 的交互逻辑
    /// </summary>
    public partial class ListYuLanDataGrid : UserControl
    {
        public delegate void ButtonClickEventHandle(int ID);
        /// <summary>
        /// 打开存储列表事件
        /// </summary>
        public event ButtonClickEventHandle _ButtonClickEvent = null;

        public delegate void BtnRemoveEventHandle(int ID);
        /// <summary>
        /// 移除存储列表事件
        /// </summary>
        public event BtnRemoveEventHandle _BtnRemoveEvent = null;


        public ListYuLanDataGrid()
        {
            try
            {
                InitializeComponent();

                this.Resources.MergedDictionaries.Add(StyleResource.MyStyle.Instacnce.DataGridResourcesGrey);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ListYuLanDataGrid", ex.ToString());
            }
            finally
            {
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int selectId = this.dataGrid.SelectedIndex;
                if (_ButtonClickEvent != null)
                {
                    _ButtonClickEvent(selectId);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Button_Click", ex.ToString());
            }
            finally
            {
            }
        }

        private void Button_Loaded(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            btn.Style = StyleResource.TongXinStyle.Instacnce.Resources["btnStyle9"] as Style;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageShow window = new MessageShow();
            window.MessageContent = "是否删除";
            window.CancelButton.Visibility = System.Windows.Visibility.Visible;

           var result = window.ShowDialog();
           if (result == true)
           {
               int selectId = this.dataGrid.SelectedIndex;
               if (_BtnRemoveEvent != null)
                   _BtnRemoveEvent(selectId);
           }            
        }

        private void Button_Loaded_1(object sender, RoutedEventArgs e)
        {
             var btn = sender as Button;
             btn.Style = StyleResource.TongXinStyle.Instacnce.Resources["btnStyle7"] as Style;
            
        }
    }
}
