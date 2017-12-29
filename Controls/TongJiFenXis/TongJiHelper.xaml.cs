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

namespace MhczTBG.Controls.TongJiFenXis
{
    /// <summary>
    /// TongJiHelper.xaml 的交互逻辑
    /// </summary>
    public partial class TongJiHelper : UserControl
    {
        #region 变量

        /// <summary>
        /// 是否需要小计
        /// </summary>
        public bool _isNeedXiaoJi = false;

        /// <summary>
        /// 统计方式
        /// </summary>
        public string _ComputeType = string.Empty;

        /// <summary>
        /// 统计内容
        /// </summary>
        public string _ComputeContent = string.Empty;

        /// <summary>
        /// 计数radio
        /// </summary>
        RadioButton rdBtnCount = null;

        /// <summary>
        /// 第三层默认选项
        /// </summary>
        RadioButton row3FilrstRdBtn = null;

        ///// <summary>
        ///// 第三层是否进行过选择
        ///// </summary>
        //bool row3IsChecked = false;

        #endregion

        #region 构造函数

        public TongJiHelper()
        {
            try
            {
            InitializeComponent();
                
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TongJiHelper", ex.ToString());
            }
        }

        #endregion

        #region UI事件区域

        #region 小计操作

        void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
            _isNeedXiaoJi = false;
                
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "checkBox_Unchecked", ex.ToString());
            }
        }

        void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
            _isNeedXiaoJi = true;
                
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "checkBox_Checked", ex.ToString());
            }
        }

        #endregion

        #region 统计内容

        void btn_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                //如果选择了计数，将
                if (rdBtnCount != null && rdBtnCount == sender)
                    stackPanle3.IsEnabled = false;
                else stackPanle3.IsEnabled = true;

                RadioButton rad = sender as RadioButton;
                this.txtTJ1.Tag = rad.Tag;
                this.txtTJ1.Text = rad.Content.ToString();

                if (this.txtTJ1.Text.Equals("计数"))
                {
                    _ComputeType = "Count";
                    _ComputeContent = "(ID)";
                }
                else
                {
                    if ( row3FilrstRdBtn != null)
                        row3FilrstRdBtn.IsChecked = true;

                    this._ComputeType = this.txtTJ2.Tag.ToString();

                    _ComputeContent = "(" + txtTJ1.Tag + ")";
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "rad_Checked", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion

        #region 统计方式

        void btn3_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                RadioButton rad = sender as RadioButton;
                this.txtTJ2.Text = "（" + rad.Content.ToString() + "）";
                this.txtTJ2.Tag = rad.Tag;
                this._ComputeType = this.txtTJ2.Tag.ToString();
              
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "rad2_Checked", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }


        #endregion

        #endregion

        #region 辅助方法

        public void ItemsRow1Add(string strText)
        {
            try
            {
            CheckBox checkBox = new CheckBox() { Content = strText };
            this.stackPanel1.Children.Add(checkBox);

            if (strText.Equals("是否小计"))
            {
                //等于小计的操作,有相应属性提供
                checkBox.Checked += new RoutedEventHandler(checkBox_Checked);
                checkBox.Unchecked += new RoutedEventHandler(checkBox_Unchecked);
            }
            else
            {
                //扩展其他操作
            }
                
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ItemsRow1Add", ex.ToString());
            }
        }

        /// <summary>
        /// 第二区域子项添加
        /// </summary>
        /// <param name="strText">显示文本</param>
        /// <param name="strTag">绑定值</param>
        public void ItemsRow2Add(string strText, string strTag)
        {
            try
            {
            RadioButton btn2 = new RadioButton() { Content = strText, Tag = strTag };
            this.wrapPanel2.Children.Add(btn2);

            if (strText.Equals("计数"))
                rdBtnCount = btn2;

            btn2.Checked += new RoutedEventHandler(btn_Checked);

            if (this.wrapPanel2.Children.Count == 1) btn2.IsChecked = true;
                
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ItemsRow2Add", ex.ToString(),strText,  strTag);
            }
        }

        /// <summary>
        /// 第三层区域子项添加
        /// </summary>
        /// <param name="strText"></param>
        /// <param name="strTag"></param>
        public void ItemsRow3Add(string strText, string strTag)
        {
            try
            {
            RadioButton btn3 = new RadioButton() { Content = strText, Tag = strTag };
            this.stackPanle3.Children.Add(btn3);

            btn3.Checked += new RoutedEventHandler(btn3_Checked);            

            if (this.stackPanle3.Children.Count == 1) row3FilrstRdBtn = btn3;
                
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ItemsRow3Add", ex.ToString(),strText,  strTag);
            }
        }

        #endregion
    }
}
