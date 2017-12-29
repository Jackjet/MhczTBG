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
using System.Threading;
using System.Runtime.Remoting.Messaging;
using System.ComponentModel;
using System.Reflection;
using MhczTBG.Controls.DataGridOperate;
using MhczTBG.Common;

namespace MhczTBG.Controls.TongJiBaoBiao
{
    /// <summary>
    /// FuHeChaXun.xaml 的交互逻辑
    /// </summary>
    public partial class FenXiBaoGao : UserControl
    {
        #region 声明变量

        /// <summary>
        /// 报表标题集合
        /// </summary>
        List<string> titleList = new List<string>();

        /// <summary>
        /// 报表内容集合
        /// </summary>
        Dictionary<string, object> dicContent = new Dictionary<string, object>();

        /// <summary>
        /// 存储生成列表的数据源
        /// </summary>
        Dictionary<string, object> dicData = new Dictionary<string, object>();

        /// <summary>
        /// 临时存储所选择的字段
        /// </summary>
        string strZiduan = string.Empty;

        /// <summary>
        /// 临时存储自由统计表
        /// </summary>
        public CommonDataGrid commonDataGrid = null;

        /// <summary>
        /// 默认表格颜色
        /// </summary>
        Color defaultColor = default(Color);

        #endregion

        #region 自定义委托事件

        public delegate void BaoBiaoInitEventHandle(string ziduan, ref Dictionary<string, object> dicData);
        /// <summary>
        /// 生成报告的激发事件
        /// </summary>
        public event BaoBiaoInitEventHandle BaoBiaoInitEvent = null;

        public delegate void BaoGaoInitEventHandle(string ziduan, ref string tittle1, ref string tittle2, ref Dictionary<string, object> dicContent);
        /// <summary>
        /// 生成报表的激发事件
        /// </summary>
        public event BaoGaoInitEventHandle BaoGaoInitEvent = null;

        #endregion

        #region 构造函数

        public FenXiBaoGao()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "FenXiBaoGao", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion

        #region UI事件区域

        #region 生成报告
        /// <summary>
        /// 生成报告
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBaogao_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                borDataGrid.Visibility = Visibility.Collapsed;
                GridBaoBiao.Visibility = Visibility.Visible;

                string tittle1 = null;

                string tittle2 = null;

                Dictionary<string, object> dicConent = null;

                if (BaoGaoInitEvent != null && !string.IsNullOrEmpty(strZiduan))
                {
                    BaoGaoInitEvent(strZiduan, ref tittle1, ref tittle2, ref dicConent);

                    if (!string.IsNullOrEmpty(tittle1) && !string.IsNullOrEmpty(tittle2) && dicConent != null && dicConent.Count > 0)
                    {
                        #region 设置报表标题

                        titleList.Clear();
                        titleList.Add(tittle1);
                        titleList.Add(tittle2);

                        #endregion

                        dicContent = dicConent;
                        documentview.titleList = titleList;
                        documentview.dic = dicContent;
                        documentview.Run();
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btnBaogao_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        #endregion

        #region 生成报表
        /// <summary>
        /// 生成报表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBaoBiao_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                borDataGrid.Visibility = Visibility.Visible;
                GridBaoBiao.Visibility = Visibility.Collapsed;

                if (BaoBiaoInitEvent != null)
                {
                    BaoBiaoInitEvent(strZiduan, ref dicData);

                    if (dicData != null && dicData.Count > 0)
                    {
                      commonDataGrid = new CommonDataGrid(dicData, false, "同期统计");
                      if (defaultColor != default(Color)) this.SkinChange(defaultColor);
                        this.borDataGrid.Child = commonDataGrid;
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btnBaoBiao_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion

        #endregion

        #region 辅助方法

        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="ziDuanList">字典字段</param>
        public void ZiduanItemsAdd(Dictionary<string, string> ziDuanList)
        {
            try
            {
                foreach (var item in ziDuanList)
                {
                    //创建一个字段
                    CheckBox checkBox = new CheckBox() { Content = item.Key, Tag = item.Value, Foreground = new SolidColorBrush(Colors.White) };
                    //注册字段选择事件
                    checkBox.Checked += new RoutedEventHandler(checkBox_Checked);
                    checkBox.Unchecked += new RoutedEventHandler(checkBox_Unchecked);
                    //加载字段
                    ZiDuan.Children.Add(checkBox);
                }
                if (ZiDuan.Children.Count > 0 && ZiDuan.Children[0] is CheckBox) (ZiDuan.Children[0] as CheckBox).IsChecked = true;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ZiduanItemsAdd", ex.ToString(), ziDuanList);
            }
            finally
            {
            }
        }

        void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                CollectZiDuan();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "checkBox_Unchecked", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// chek事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                CollectZiDuan();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "checkBox_Checked", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        void CollectZiDuan()
        {
            try
            {
                //临时存储所选择的字段       
                strZiduan = string.Empty;
                foreach (var item in ZiDuan.Children)
                {
                    if (item is CheckBox && (item as CheckBox).Tag != null && (item as CheckBox).IsChecked == true)
                    {
                        strZiduan += (item as CheckBox).Tag.ToString() + ",";
                    }
                }
                if (strZiduan.Contains(",")) strZiduan = strZiduan.Substring(0, strZiduan.Length - 1);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "CollectZiDuan", ex.ToString());
            }
            finally
            {
            }
        }

        /// <summary>
        /// 皮肤更换
        /// </summary>
        /// <param name="color">指定的颜色</param>
        public void SkinChange(Color color)
        {
            try
            {
                this.defaultColor = color;
                if (commonDataGrid != null) commonDataGrid.SkinChange(color);
                (this.Resources["baoGaoBrush"] as SolidColorBrush).Color = color;             
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "SkinChange", ex.ToString(), color);
            }
            finally
            {
            }
        }

        #endregion
    }
}
