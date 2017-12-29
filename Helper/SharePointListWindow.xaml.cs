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
using Microsoft.SharePoint.Client;

namespace MhczTBG.Helper
{
    /// <summary>
    /// MainWindow3.xaml 的交互逻辑
    /// </summary>
    public partial class SharePointListWindow : Window
    {
        public SharePointListWindow(string strWebSite, string userName, string password, string doMain)
        {
            InitializeComponent();
        
            gridMain.Children.Add(new DocumentControl(strWebSite, userName, password, doMain));
        }

    }
}
