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
    /// BtnDaoHangItem.xaml 的交互逻辑
    /// </summary>
    public partial class BtnDaoHangItem : UserControl
    {
      

        private string tittle;
        /// <summary>
        /// 文本
        /// </summary>
        public string _Tittle
        {
            get { return tittle; }
            set
            {               
                tittle = value;
            }
        }


        public BtnDaoHangItem()
        {
            InitializeComponent();

            this.Loaded +=new RoutedEventHandler(BtnDaoHangItem_Loaded); 
        }

        void BtnDaoHangItem_Loaded(object sender, RoutedEventArgs e)
        {
            this.btn.Content = tittle;
        }
       
        #region 自定义委托事件       

        #endregion
    }
}
