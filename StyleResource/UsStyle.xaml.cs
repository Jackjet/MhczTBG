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

namespace MhczTBG.StyleResource
{
    /// <summary>
    /// UsStyle.xaml 的交互逻辑
    /// </summary>
    public partial class UsStyle : UserControl
    {
        #region 将该实例存储，并不在创建实例（单例设计模式）

        static UsStyle usStyle;
        public static UsStyle Instacnce
        {
            get
            {
                if (usStyle == null)
                {
                    usStyle = new UsStyle();
                }
                return usStyle;
            }
        }

        #endregion

        #region 构造函数
        
        public UsStyle()
        {
            InitializeComponent();
        }

        #endregion

        #region 资源调用

      


        #endregion


        //FrameworkElement element = null;
        //private void select_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (element != null) element.Visibility = System.Windows.Visibility.Visible;

        //        element = sender as FrameworkElement;
        //        element.Visibility = System.Windows.Visibility.Collapsed;
        //}
    }
}
