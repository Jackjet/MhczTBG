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
    public enum Display
    {
        Year = 0,
        Month = 1,
        Timer = 2,

    }
    partial class DataPicker : UserControl
    {
        #region 变量

        /// <summary>
        /// 设置该时间控件显示年还是月，如果为true的话,显示月
        /// </summary>
        public Display SelectionMode { get; set; }

        /// <summary>
        /// 临时存储数据源
        /// </summary>
        List<string> dataList = new List<string>();

        #endregion

        #region 构造函数

        /// <summary>
        /// 新的时间控件
        /// </summary>
        public DataPicker()
        {
            try
            {
                InitializeComponent();
                //加载事件
                this.Loaded += new RoutedEventHandler(TomPicker_Loaded);

                //this.com.Style = MhczTBG.StyleResource.MyStyle.ComboBoxStyle; 
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DataPicker", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion

        #region UI事件区域

        /// <summary>
        /// 加载事件
        /// </summary>
        void TomPicker_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                dataList.Clear();
                switch (SelectionMode)
                {
                    case Display.Year:
                        //循环添加具体的年（2000--2030）
                        for (int i = 2000; i < 2030; i++)
                        {
                            dataList.Add(i + "年");
                        }
                        this.com.ItemsSource = dataList;
                        this.com.SelectedValue = DateTime.Now.Year.ToString() + "年";
                        break;

                    case Display.Month:
                        //循环添加月（1--12）
                        for (int i = 1; i < 13; i++)
                        {
                            if (i < 10)
                            {
                                dataList.Add("0" + i + "月");
                            }
                            else
                            {
                                dataList.Add(i + "月");
                            }
                        }
                        this.com.ItemsSource = dataList;
                        this.com.SelectedValue = DateTime.Now.Month.ToString("00") + "月";
                        break;

                    case Display.Timer:
                        //循环添加时间（1--23）
                        for (int i = 0; i < 24; i++)
                        {
                            if (i < 10)
                            {
                                dataList.Add("0" + i + "点");
                            }
                            else
                            {
                                dataList.Add(i + "点");
                            }
                            this.com.ItemsSource = dataList;
                            this.com.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
                            this.com.ItemContainerStyle = (Style)Application.Current.Resources["comboBoxItemsStyle"];
                        }
                        break;

                    default:
                        break;
                }
                //选定项的索引（默认为-1）  
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TomPicker_Loaded", ex.ToString(), sender, e);
            }
            finally
            {
                
            }
        }


        #endregion

        private void path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
