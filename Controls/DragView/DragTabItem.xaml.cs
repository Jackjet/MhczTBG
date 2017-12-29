using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MhczTBG.Common;

namespace MhczTBG.Controls.DragView
{
    public partial class DragTabItem : UserControl
    {

        private FrameworkElement element;

        public FrameworkElement Element
        {
            get { return element; }
            set
            {
                if (value != null && value is FrameworkElement)
                {
                    value.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                    value.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                    this.borderMain.Child = value;
                    element = value;
                }
            }
        }

        string tittle;
        /// <summary>
        /// 设置标题
        /// </summary>
        public string Tittle
        {
            get { return tittle; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.txtTitle.Text = value;
                    tittle = value;
                }
            }
        }

        public DragTabItem()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DragTabItem", ex.ToString());
            }
            finally
            {
            }
        }

        private void lbl_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                lbl.Visibility = System.Windows.Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "OKButton_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }


    }
}
