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
using System.ComponentModel;
using MhczTBG.Common;

namespace MhczTBG.Controls.DataGridOperate
{
    /// <summary>
    /// CommonDataGridCell.xaml 的交互逻辑
    /// </summary>
    partial class CommonDataGridCell : Label
    {
        #region 变量

        string text;
        /// <summary>
        /// 显示文本
        /// </summary>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        string tagRowHeader;
        /// <summary>
        /// 存储行的标题
        /// </summary>
        public string TagRowHeader
        {
            get { return tagRowHeader; }
            set { tagRowHeader = value; }
        }


        string tagColumnHeader;
        /// <summary>
        /// 存储的列标题
        /// </summary>
        public string TagColumnHeader
        {
            get { return tagColumnHeader; }
            set { tagColumnHeader = value; }
        }

        #endregion

        #region 构造函数

        public CommonDataGridCell()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "CommonDataGridCell", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion
    }
}
