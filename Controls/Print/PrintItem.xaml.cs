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

namespace MhczTBG.Controls.Print
{
    /// <summary>
    /// PrintItem.xaml 的交互逻辑
    /// </summary>
    partial class PrintItem : UserControl
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="strTitle">标题</param>
        /// <param name="element">目标</param>
        public PrintItem(string strTitle, FrameworkElement element)
        {
            try
            {
                InitializeComponent();
                this.txtTitle.Text = strTitle;
                this.bordMain.Child = element;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "PrintItem", ex.ToString(), strTitle, element);
            }
            finally
            {
            }
        } 
        #endregion
    }
}
