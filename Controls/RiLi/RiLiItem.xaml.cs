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

namespace MhczTBG.Controls.RiLi
{
    partial class RiLiItem : UserControl
    {
        #region 构造函数
        /// <summary>
        /// 构造函数 
        /// </summary>
        public RiLiItem()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "RiLiItem", ex.ToString());
            }
            finally
            {
            }
        } 
        #endregion
    }
}
