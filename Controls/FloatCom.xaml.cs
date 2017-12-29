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
using System.Collections;
using MhczTBG.Common;

namespace MhczTBG.Controls
{
    /// <summary>
    /// FloatCom.xaml 的交互逻辑
    /// </summary>
    public partial class FloatCom : UserControl
    {
        #region 变量

        string text;
        /// <summary>
        /// 显示文本
        /// </summary>
        public string Text
        {
            get { return text; }
            set
            {
                this.btn.Content = value;
                text = value;
            }
        }

        int selectIndex;
        /// <summary>
        /// 选择项的索引
        /// </summary>
        public int SelectIndex
        {
            get { return selectIndex; }
            set
            {
                SetIndex(value);
                selectIndex = value;
            }
        }

        /// <summary>
        /// 选择项
        /// </summary>
        public RadioButton SelectItem
        {
            get { return this.stackPanel.Children[SelectIndex] as RadioButton; }
        }

        IList source;
        /// <summary>
        /// 数据源
        /// </summary>
        public IList Source
        {
            get { return source; }
            set
            {
                this.stackPanel.Children.Clear();
                foreach (var item in value)
                {
                    this.AddItem(item.ToString());
                }
                source = value;
            }
        }

        #endregion

        #region 自定义委托事件

        public delegate void SelectionChangedHandle(string SelectedText);
        /// <summary>
        /// 当默认子项为RadioButton时，选择其中一个，除了radio_Click执行一些必要的操作（radio_Click的扩展）
        /// </summary>
        public event SelectionChangedHandle SelectionChanged = null;


        #endregion

        #region 构造函数

        public FloatCom()
        {
            try
            {
                InitializeComponent();

                ParametsInit();

                #region 注册事件区域

                //this.Foreground.Changed += new EventHandler(Foreground_Changed);

                #endregion
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "FloatCom", ex.ToString());
            }
            finally
            {

            }
        }

        #endregion

        #region 初始化

        void ParametsInit()
        {
            try
            {
                //点击其他地方Popuy则关闭
                this.P.StaysOpen = false;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ParametsInit", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion

        #region UI事件区域

        /// <summary>
        /// 点击子项的事件
        /// </summary>
        /// <remarks></remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void radio_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                e.Handled = true;
                //设置选择的索引
                SelectIndex = GetIndex(sender);
                //设置选择项的标题
                Text = SelectItem.Content.ToString();

                if (SelectionChanged != null)
                {
                    SelectionChanged(Text);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "radio_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 点击该控件打开悬浮列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.P.IsOpen)
                    this.P.IsOpen = false;
                else
                    //打开
                    this.P.IsOpen = true;


                //标示已处理
                e.Handled = true;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btn_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 将该控件的前景颜色设置为该字体的颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Foreground_Changed(object sender, EventArgs e)
        {
            try
            {
                //将该控件的前景颜色设置为该字体的颜色
                if (this.Foreground != null)
                {
                    btn.Foreground = this.Foreground;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Foreground_Changed", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion

        #region 添加子项

        /// <summary>
        /// 添加子项
        /// </summary>
        /// <remarks></remarks>
        /// <param name="strText">文本</param>
        public void AddItem(string strText)
        {
            try
            {
                //显示的子项元素
                RadioButton radio = new RadioButton() { Content = strText };
                //点击的事件
                radio.Click += new RoutedEventHandler(radio_Click);
                //加载子项
                this.stackPanel.Children.Add(radio);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "AddItem", ex.ToString(), strText);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 添加子项
        /// </summary>
        /// <param name="strText">文本</param>
        public void AddItem(FrameworkElement element)
        {
            try
            {
                //加载子项
                this.stackPanel.Children.Add(element);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "AddItem", ex.ToString(), element);
            }
            finally
            {
            }
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 获取索引
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        int GetIndex(object sender)
        {
            int result = 0;
            try
            {
                result = stackPanel.Children.IndexOf(sender as UIElement);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "GetIndex", ex.ToString(), sender);
            }
            finally
            {
            }
            return result;
        }

        /// <summary>
        /// 设置索引
        /// </summary>
        /// <param name="i">索引</param>
        void SetIndex(int i)
        {
            try
            {
                (stackPanel.Children[i] as RadioButton).IsChecked = true;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "SetIndex", ex.ToString(), i);
            }
            finally
            {
            }
        }

        #endregion
    }
}
