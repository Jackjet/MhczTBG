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

namespace MhczTBG.Controls.TooltipGuZhangChuLi
{
    /// <summary>
    /// ToolTipGuZhangChuLiJingGuo.xaml 的交互逻辑
    /// </summary>
    public partial class ToolTipGuZhangChuLiJingGuo : UserControl
    {
        #region 变量
        /// <summary>
        ///分隔符1
        /// </summary>
        char split1 = '#';
        /// <summary>
        ///分隔符2
        /// </summary>
        char split2 = '&';

        #endregion

        #region 构造函数

        public ToolTipGuZhangChuLiJingGuo()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ToolTipGuZhangChuLiJingGuo", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion

        #region 样式事件扩展事件

        /// <summary>
        /// 设计规则
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is StackPanel && (sender as StackPanel).Tag != null && (sender as StackPanel).Children.Count > 1)
                {
                    ///获取到承载体（模板里的）
                    var stackPanel = sender as StackPanel;

                    if (stackPanel.Children[0] is TextBox && stackPanel.Children[1] is TextBox)
                    {
                        string strChuLiJinGuo = stackPanel.Tag.ToString();
                        //一条信息的panel(故障经过不为空，而且至少必须有两个&分割符)
                        if (!string.IsNullOrEmpty(strChuLiJinGuo) && strChuLiJinGuo.Count(Item => Item.Equals(split2)) > 1)
                        {
                            //分割（获取时间和内容）
                            string[] str = strChuLiJinGuo.Split(new char[] { split2 });

                            //设置显示时间的文本
                            (stackPanel.Children[0] as TextBox).Text = str[0] + "         " + str[1] + "：";

                            //设置显示内容的文本
                            (stackPanel.Children[1] as TextBox).Text = str[2];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "StackPanel_Loaded", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 子项加载样式（单项加载）
        /// </summary>
        /// <param name="strCuLiJingGuo">故障处理经过</param>
        /// <param name="splitt2">分隔符2</param>
        public void ItemsAdd(string strCuLiJingGuo, char splitt2)
        {
            try
            {
                #region 设置分割符

                this.split2 = splitt2;

                #endregion

                #region 设计子项

                TextBox textBox = new TextBox();
                textBox.Style = this.Resources["textStyle"] as Style;
                textBox.Text = strCuLiJingGuo;

                #endregion

                stackPanel.Children.Add(textBox);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ItemsAdd", ex.ToString(), strCuLiJingGuo, splitt2);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 加载多内容的使用（多项加载）
        /// </summary>
        /// <param name="strCuLiJingGuoList">故障处理经过</param>
        /// <param name="splitt1">分隔符1</param>
        /// <param name="splitt2">分隔符2</param>
        public void ItemsArrageAdd(string strCuLiJingGuoList, char splitt1, char splitt2)
        {
            try
            {
                #region 设置分割符

                this.split1 = splitt1;
                this.split2 = splitt2;

                #endregion

                //分割内容
                string[] strList = strCuLiJingGuoList.Split(new char[] { split1 });
                if (strList.Contains(splitt2.ToString()))
                {
                    //多条加载
                    foreach (var item in strList)
                    {
                        ItemsAdd(item, splitt2);
                    }
                } 
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ItemsArrageAdd", ex.ToString(), strCuLiJingGuoList, splitt1, splitt2);
            }
            finally
            {
            }
        }


        #endregion
    }
}
