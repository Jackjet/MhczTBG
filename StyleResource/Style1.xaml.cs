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
    /// StyleCenterControl.xaml 的交互逻辑
    /// </summary>
    public partial class Style1 : UserControl
    {
        #region 将该实例存储，之后不再创建实例（单例设计模式）

        static Style1 style1;
        public static Style1 Instacnce
        {
            get
            {
                if (style1 == null)
                {
                    style1 = new Style1();
                }
                return style1;
            }
        }

        #endregion

        public Style1()
        {
            InitializeComponent();
        }
    }
}
