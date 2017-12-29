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

namespace MhczTBG.Helper
{
    /// <summary>
    /// SpIDisPaly.xaml 的交互逻辑
    /// </summary>
    public partial class SpIDisPaly : UserControl
    {
        public SpIDisPaly()
        {
            InitializeComponent();
        }

        public SpIDisPaly(Microsoft.SharePoint.Client.File file)
            : this()
        {
            InitializeComponent();

            txtDisplay.Text = file.Name;
            txtCreate.Text = file.Author.Title;
            txtEdit.Text = file.ModifiedBy.Title;

            txtFilePath.Text = file.ServerRelativeUrl;
            txtCreationTime.Text = file.TimeCreated.ToString();
            txtModifyTime.Text = file.TimeLastModified.ToString();
            txtFileType.Text = System.IO.Path.GetExtension(file.ServerRelativeUrl).Replace(".", "");
            txtFileVersion.Text = file.UIVersionLabel;
            txtFileVersionsCount.Text = file.Versions.Count.ToString();
        }
    }
}
