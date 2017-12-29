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
    partial class DateWeekControl : UserControl
    {
        #region 变量

        string text;
        /// <summary>
        /// 日期控件的文字说明
        /// </summary>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        /// <summary>
        /// 日期控件（星期一至星期天）
        /// </summary>
        public DateWeekControl()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DateWeekControl", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 日期控件（星期一至星期天）
        /// </summary>
        /// <param name="num">需要设置的行数</param>
        /// <param name="text">日期控件的文字说明</param>
        public DateWeekControl(string text)
            : this()
        {
            try
            {
                Text = text;
                this.Loaded += new RoutedEventHandler(DateWeekControl_Loaded);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DateWeekControl", ex.ToString(), text);
            }
            finally
            {
            }
        }

        #endregion

        #region 注册事件区域

        void DateWeekControl_Loaded(object sender, RoutedEventArgs e)
        {
              try
            {
            //日期控件的文字说明
            this.data_txt.Text = this.text;
                    }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DateWeekControl_Loaded", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {
              try
            {
            e.Handled = true;
                    }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "OnMouseRightButtonDown", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion
    }
}
