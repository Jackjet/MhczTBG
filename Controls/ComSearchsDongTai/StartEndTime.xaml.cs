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

namespace MhczTBG.Controls.ComSearchsDongTai
{
    /// <summary>
    /// StartEndTime.xaml 的交互逻辑
    /// </summary>
    partial class StartEndTime : UserControl
    {
        #region 声明变量
        /// <summary>
        /// 开始日期
        /// </summary>
        string startdata;
        /// <summary>
        /// 开始时间
        /// </summary>
        string starttime;
        /// <summary>
        /// 结束日期
        /// </summary>
        string enddata;
        /// <summary>
        /// 结束时间
        /// </summary>
        string endtime;
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public StartEndTime()
        {
            try
            {
                InitializeComponent();

                CMEndTime.Style = CMStartTime.Style = StyleResource.TongXinStyle.Instacnce.Resources["ComboBoxStyle1"] as Style;
                CMStartTime.Items.Clear();
                CMEndTime.Items.Clear();
                for (int i = 0; i < 24; i++)
                {
                    CMStartTime.Items.Add(i.ToString());
                    CMEndTime.Items.Add(i.ToString());
                }

                if (CMStartTime.Items.Count > 0)
                {
                    CMStartTime.SelectedIndex = 0;
                }
                if (CMEndTime.Items.Count > 0)
                {
                    CMEndTime.SelectedIndex = 23;
                }

                #region 注册时间选择事件

                this.DPStartData.SelectedDateChanged += new EventHandler<SelectionChangedEventArgs>(DPStartData_SelectedDateChanged);
                this.DPEndData.SelectedDateChanged += new EventHandler<SelectionChangedEventArgs>(DPEndData_SelectedDateChanged);
                this.CMStartTime.SelectionChanged += new SelectionChangedEventHandler(CMStartTime_SelectionChanged);
                this.CMEndTime.SelectionChanged += new SelectionChangedEventHandler(CMEndTime_SelectionChanged);
                #endregion
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "StartEndTime", ex.ToString());
            }
            finally
            {
            }
        }
        #endregion

        #region 事件区域
        /// <summary>
        /// 结束时间选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CMEndTime_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                this.Data时间日期改变();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "CMEndTime_SelectionChanged", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 开始时间选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CMStartTime_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                this.Data时间日期改变();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "CMStartTime_SelectionChanged", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 结束日期选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DPEndData_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                this.Data时间日期改变();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DPEndData_SelectedDateChanged", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 开始日期选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DPStartData_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                this.Data时间日期改变();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DPStartData_SelectedDateChanged", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 时间判断,开始时间不可以大于结束时间
        /// </summary>
        void Data时间日期改变()
        {
            try
            {
                if (!String.IsNullOrEmpty(DPStartData.Text) && !String.IsNullOrEmpty(DPEndData.Text) && !String.IsNullOrEmpty(CMStartTime.Text) && !String.IsNullOrEmpty(CMEndTime.Text))
                {
                    DateTime startdt = Convert.ToDateTime(DPStartData.Text + " " + CMStartTime.SelectedValue.ToString() + ":00:00");
                    DateTime enddt = Convert.ToDateTime(DPEndData.Text + " " + CMEndTime.SelectedValue.ToString() + ":59:59");

                    TimeSpan ds = enddt - startdt;

                    if (ds.TotalSeconds > 0)
                    {

                        //开始日期
                        startdata = DPStartData.Text;

                        //开始时间 
                        starttime = CMStartTime.SelectedValue.ToString();

                        //结束日期
                        enddata = DPEndData.Text;

                        //结束时间
                        endtime = CMEndTime.SelectedValue.ToString();
                    }
                    else
                    {
                        MessageBox.Show("结束时间不能小于开始时间");

                        DPStartData.Text = startdata;

                        CMStartTime.SelectedItem = starttime;

                        DPEndData.Text = enddata;

                        CMEndTime.SelectedItem = endtime;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
        }
        #endregion
    }
}
