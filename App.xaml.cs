using MhczTBG.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace MhczTBG
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {

        public App()
        {
            SharePointListWindow SHARE = new SharePointListWindow("http://spt/sites/hometest/Storage/SitePages/test3.aspx", "Administrator", "Password2015", "t.com");
            SHARE.Show();
        }
    }
}
