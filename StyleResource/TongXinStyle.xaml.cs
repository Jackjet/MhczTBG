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
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class TongXinStyle : UserControl
    {
        #region 将该实例存储，并不在创建实例（单例设计模式）

        static TongXinStyle tongXinStyle;
        public static TongXinStyle Instacnce
        {
            get
            {
                if (tongXinStyle == null)
                {
                    tongXinStyle = new TongXinStyle();
                }
                return tongXinStyle;
            }
        }

        #endregion
       
        #region 构造函数

        public TongXinStyle()
        {
            InitializeComponent();
        }

        #endregion
    }
}
