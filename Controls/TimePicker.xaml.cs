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
using System.Windows.Threading;
using MhczTBG.Common;

namespace MhczTBG.Controls
{
    /// <summary>
    /// TomTimePicker.xaml 的交互逻辑
    /// </summary>
    public partial class TimePicker : UserControl
    {
        #region 变量

        string value;
        /// <summary>
        /// 时间控件的值
        /// </summary>
        public string Value
        {
            get { return this.value; }
            set
            {
                //将赋予的值转为datetime类型，然后获取其中具体时间
                DateTime dateValue = default(DateTime);
                if (DateTime.TryParse(value, out dateValue))
                {
                    this.value = value;
                    //显示具体时间
                    txtTime.Text = dateValue.ToShortTimeString();
                }
                if (value == null)
                {
                    //为空则赋予null
                    this.value = value;
                    //显示为空
                    txtTime.Text = null;
                }
            }
        }

        #endregion

        #region 构造函数

        public TimePicker()
        {
            try
            {
                InitializeComponent();

                #region 注册事件区域

                this.txtTime.LostFocus += new RoutedEventHandler(txtTime_LostFocus);
                this.bor.MouseLeftButtonDown += new MouseButtonEventHandler(bor_MouseLeftButtonDown);
                this.txtTime.QueryCursor += new QueryCursorEventHandler(txtUp_QueryCursor);
                this.txtTime.TextChanged += new TextChangedEventHandler(txtTime_TextChanged);
                this.txtTime.KeyDown += new KeyEventHandler(txtTime_KeyDown);
                this.griMain.MouseLeave += new MouseEventHandler(TimePicker_MouseLeave);

                #endregion
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TimePicker", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion

        #region UI事件区域

        /// <summary>
        /// 鼠标离开该时间控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TimePicker_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                //悬浮的时间对照框隐藏
                this.bor.Visibility = System.Windows.Visibility.Collapsed;
                //如果不为空将时间对照框里的值赋予时间文本框
                if (!string.IsNullOrEmpty(this.txtTime.Text))
                {
                    //显示
                    this.txtTime.Text = this.txtUp.Text;
                    //赋值
                    Value = this.txtUp.Text;
                }
                else
                {
                    //输入为空，则赋值为空
                    Value = null;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TimePicker_MouseLeave", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 失去焦点时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txtTime_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                //悬浮的时间对照框隐藏
                this.bor.Visibility = System.Windows.Visibility.Collapsed;
                //如果不为空将时间对照框里的值赋予时间文本框
                if (!string.IsNullOrEmpty(this.txtTime.Text))
                {
                    //显示
                    this.txtTime.Text = this.txtUp.Text;
                    //赋值
                    Value = this.txtUp.Text;
                }
                else
                {
                    //输入为空，则赋值为空
                    Value = null;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "txtTime_LostFocus", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 显示框左键按下，隐藏并赋值给文本框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bor_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //悬浮的时间对照框隐藏
                this.bor.Visibility = System.Windows.Visibility.Collapsed;
                //显示
                this.txtTime.Text = this.txtUp.Text;
                //赋值
                Value = this.txtUp.Text;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "bor_MouseLeftButtonDown", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 文本框修改时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txtTime_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //获取当前输入的值
                string strTime = this.txtTime.Text;
                if (strTime.Equals(";"))
                {
                    strTime = ":";
                }
                //如果不带":"则进行特殊处理
                if (!strTime.Contains(":"))
                {
                    //显示时间对照框
                    this.bor.Visibility = System.Windows.Visibility.Visible;

                    #region 进行验证


                    if (strTime.Length == 1)
                    {
                        strTime = string.Format("{0}:00", strTime);
                    }
                    else if (strTime.Length == 2)
                    {
                        int intTime0 = Convert.ToInt32(strTime[0].ToString());
                        int intTime1 = Convert.ToInt32(strTime[1].ToString());
                        //如果第一个数为1或为0，所有后面的所有都可以
                        if (intTime0 == 0 || intTime0 == 1)
                        {
                            strTime = string.Format("{0}{1}:00", strTime[0], strTime[1]);
                        }
                        //如果第一个数为2，第二位应该比4小
                        else if (intTime0 == 2 && intTime1 < 4)
                        {
                            strTime = string.Format("{0}{1}:00", strTime[0], strTime[1]);
                        }
                        else if ((intTime0 > 2 && intTime0 < 6) || ((intTime0 == 2 && intTime1 >= 4)))
                        {
                            strTime = string.Format("00:{0}{1}", strTime[0], strTime[1]);
                        }
                        else
                        {
                            strTime = DateTime.Now.ToShortTimeString();
                        }
                    }
                    else if (strTime.Length == 3)
                    {
                        int intTime0 = Convert.ToInt32(strTime[0].ToString());
                        int intTime1 = Convert.ToInt32(strTime[1].ToString());
                        int intTime2 = Convert.ToInt32(strTime[2].ToString());

                        if (intTime1 < 6)
                        {
                            strTime = string.Format("0{0}:{1}{2}", strTime[0], strTime[1], strTime[2]);
                        }
                        else
                        {
                            strTime = DateTime.Now.ToShortTimeString();
                        }
                    }
                    else if (strTime.Length == 4)
                    {
                        int intTime0 = Convert.ToInt32(strTime[0].ToString());
                        int intTime1 = Convert.ToInt32(strTime[1].ToString());
                        int intTime2 = Convert.ToInt32(strTime[2].ToString());
                        int intTime3 = Convert.ToInt32(strTime[3].ToString());

                        if (intTime0 == 0 || intTime0 == 1)
                        {
                            if (intTime2 < 6)
                            {
                                strTime = string.Format("{0}{1}:{2}{3}", strTime[0], strTime[1], strTime[2], strTime[3]);
                            }
                            else
                            {
                                strTime = DateTime.Now.ToShortTimeString();
                            }
                        }
                        else if (intTime0 == 2)
                        {
                            if (intTime1 < 4 && intTime2 < 6)
                            {
                                strTime = string.Format("{0}{1}:{2}{3}", strTime[0], strTime[1], strTime[2], strTime[3]);
                            }
                            else
                            {
                                strTime = DateTime.Now.ToShortTimeString();
                            }
                        }
                        else
                        {
                            strTime = DateTime.Now.ToShortTimeString();
                        }
                    }
                    else
                    {
                        strTime = DateTime.Now.ToShortTimeString();
                    }

                    #endregion

                    //不为空显示输入的内容
                    if (!string.IsNullOrEmpty(strTime))
                    {
                        this.txtUp.Text = strTime;
                    }
                    else
                    {
                        this.txtUp.Text = DateTime.Now.ToShortTimeString();
                    }
                }
                else
                {
                    if (strTime.Length > 3)
                    {
                        DateTime dtTime = DateTime.Now;
                        if (DateTime.TryParse(strTime, out dtTime))
                        {
                            this.txtUp.Text = dtTime.ToShortTimeString();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "txtTime_TextChanged", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 获得光标的时候发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txtUp_QueryCursor(object sender, QueryCursorEventArgs e)
        {
            try
            {
                //如果输入时间输入框里的值为空
                if (string.IsNullOrEmpty(this.txtTime.Text))
                {
                    //将现在时间赋予时间对照框
                    this.txtUp.Text = DateTime.Now.ToShortTimeString();
                    //悬浮的时间对照框显示
                    this.bor.Visibility = System.Windows.Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "txtUp_QueryCursor", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 屏蔽非法按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txtTime_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //屏蔽非法按键
                if ((e.Key >= Key.D0 && e.Key <= Key.D9) || e.Key == Key.OemSemicolon || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
                {

                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "txtTime_KeyDown", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 获取顶级的父容器
        /// </summary>
        /// <param name="count">指定遍历的层数</param>
        /// <param name="element">指定子元素</param>
        /// <returns>返回顶级容器</returns>
        public FrameworkElement GetRootParent(int count, FrameworkElement element, Action action)
        {

            //定义一个变量,每次遍历存储更高级的父元素
            FrameworkElement rootParent = element;
            try
            {
                //一层一层遍历
                for (int i = 0; i < count; i++)
                {
                    //如果达到最顶层，则跳出
                    if (rootParent.Parent == null) break;
                    rootParent = rootParent.Parent as FrameworkElement;
                    rootParent.MouseLeftButtonDown += new MouseButtonEventHandler(rootParent_MouseLeftButtonDown);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "GetRootParent", ex.ToString(), count, element, action);
            }
            finally
            {
            }
            //返回顶层元素
            return rootParent;
        }

        void rootParent_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.bor.Visibility = System.Windows.Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "rootParent_MouseLeftButtonDown", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion
    }
}
