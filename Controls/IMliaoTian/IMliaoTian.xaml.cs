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

namespace MhczTBG.Controls.IMliaoTian
{
    /// <summary>
    /// IMliaoTian.xaml 的交互逻辑
    /// </summary>
    public partial class IMliaoTian : UserControl
    {
        #region 变量

        TabControl tabControl = null;
        /// <summary>
        /// 添加导航页
        /// </summary>
        public TabControl TabControl
        {
            get { return tabControl; }
            set
            {
                if (value != null)
                {
                    //给整个容器设计样式
                    value.Style = this.Resources["tabStyle"] as Style;
                    this.borMain.Child = value;
                    tabControl = value;
                }
            }
        }

        #endregion

        #region 自定义委托事件

        public delegate void SelectEventHandle(string text);
        /// <summary>
        /// 导航选择事件
        /// </summary>
        public event SelectEventHandle SelectEvent = null;

        #endregion

        #region 构造函数

        public IMliaoTian()
        {
            try
            {
                InitializeComponent();

                #region 注册UI事件区域

                this.Loaded += new RoutedEventHandler(IMliaoTian_Loaded);

                #endregion
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "IMliaoTian", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion

        #region UI事件区域
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void IMliaoTian_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ///每一个子项绑定样式
                if (TabControl != null && TabControl.Items.Count > 0)
                {
                    foreach (var item in TabControl.Items)
                    {
                        if (item is TabItem) (item as TabItem).Style = this.Resources["tabItemStyle"] as Style;
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "IMliaoTian_Loaded", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion

        #region 事件样式扩展

        /// <summary>
        /// 导航选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bd_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (SelectEvent != null)
                {
                    if (sender is Border && (sender as Border).Child != null && (sender as Border).Child is TextBlock && !string.IsNullOrEmpty(((sender as Border).Child as TextBlock).Text))
                    {
                        ///获取指定的导航信息
                        var text = ((sender as Border).Child as TextBlock).Text;
                        SelectEvent(text);
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Bd_MouseLeftButtonDown", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion
    }
}
