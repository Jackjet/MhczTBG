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

namespace MhczTBG.Helper
{
    /// <summary>
    /// TemplateDisPlay.xaml 的交互逻辑
    /// </summary>
    public partial class TemplateDisPlay : UserControl
    {
        string strType = string.Empty;
        /// <summary>
        /// 文本描述（类型）
        /// </summary>
        public string StrType
        {
            get { return strType; }
            set { strType = value; }
        }

        string strKey = string.Empty;
        /// <summary>
        /// 文本描述2
        /// </summary>
        public string StrKey
        {
            get { return strKey; }
            set { strKey = value; }
        }

        Border disContent;
        /// <summary>
        /// 要展示的
        /// </summary>
        public Border DisContent
        {
            get { return disContent; }
            set { disContent = value; }
        }

        public TemplateDisPlay()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(TemplateDisPlay_Loaded);
        }

        void TemplateDisPlay_Loaded(object sender, RoutedEventArgs e)
        {
            this.text.Text = strType;
            this.text.Tag ="调用："+ strKey;
            if (disContent != null) this.borContent.Child = disContent;
        }


    }
}
