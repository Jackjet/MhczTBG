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

namespace MhczTBG.Controls.Charts
{
    /// <summary>
    /// ChartModern1.xaml 的交互逻辑
    /// </summary>
    public partial class ChartCommon : UserControl
    {
        public ChartCommon()
        {
            InitializeComponent();

            this._dataGrid.PreviewMouseWheel += new MouseWheelEventHandler(_dataGrid_PreviewMouseWheel);
        }

        void _dataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
        }
    }
}
