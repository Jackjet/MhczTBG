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

namespace MhczTBG.Controls.ShiGongManage
{
    public partial class ToolTip_Control : UserControl
    {
        #region 变量

        StackPanel panel;
        /// <summary>
        /// 显示字段的panel
        /// </summary>
        public StackPanel Panel
        {
            get
            {
                //获取时,若没有则新建个实例，并且留下引用
                if (panel == null)
                {
                    panel = new StackPanel();
                    this.bordMain.Child = panel;                    
                }
                return panel;
            }
            set 
            {
                //设置
                if (value != null )
                {
                    this.bordMain.Child = value;
                    panel = value;
                }
            }
        }

        #endregion

        #region 构造函数
        
        /// <summary>
        /// 提示框的构造函数
        /// </summary>
        public ToolTip_Control()
        {
              try
            {
            InitializeComponent();
                    }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ToolTip_Control", ex.ToString());
            }
            finally
            {
            }

        }
        #endregion
    }
}

