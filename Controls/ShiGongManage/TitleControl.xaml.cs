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
using System.Windows.Media.Imaging;
using MhczTBG.Common;

namespace MhczTBG.Controls.ShiGongManage
{
    partial class TitleControl : UserControl
    {
        #region 变量

        /// <summary>
        /// 标题集合体
        /// </summary>
        public static List<string> striList = new List<string>();

        #endregion

        #region 构造函数

        /// <summary>
        /// 左边列标题的设置
        /// </summary>
        /// <param name="uri">标题图标</param>
        /// <param name="txt">标题说明</param>
        public TitleControl(string txt)
            : this()
        {
            try
            {
                //列标题的文本
                row_txt.Text = txt;
                //添加列标题文本
                striList.Add(txt);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TitleContro", ex.ToString(), txt);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 左边列标题的设置
        /// </summary>
        public TitleControl()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TitleContro", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion
    }
}
