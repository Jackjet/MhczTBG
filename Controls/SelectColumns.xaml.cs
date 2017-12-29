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
using System.Windows.Shapes;
using MhczTBG.Common;
using TongXin.UIComman;

namespace MhczTBG.Controls
{
    /// <summary>
    /// SelectColumns.xaml 的交互逻辑自定义视图
    /// </summary>
    public partial class SelectColumns : Window
    {
        #region 声明变量
        //datagrid存放标题的字典
        Dictionary<string, string> columns = new Dictionary<string, string>();
        //选中datagrid标题的字典
        public Dictionary<string, string> VisbleList = new Dictionary<string, string>();
        //读取配置文件
        IniFile IniConfig = new IniFile("C:/Config/iniFile.ini");
        #endregion      

        #region 构造函数
        public SelectColumns(Dictionary<string, string> xianshi)
        {
            InitializeComponent();
            //dataGrid设置标题
            NewMethod();
            //选中已显示在datagrid的标题
            foreach (CheckBox item in wrapPanel1.Children)
            {
                foreach (var item1 in xianshi)
                {
                    if (item.Tag.Equals(item1.Key))
                    {
                        item.IsChecked = true;
                    }
                }
            }
        }
        #endregion

        #region dataGrid设置标题
        //datagrid列的header
        private void NewMethod()
        {
            try
            {
                columns.Add("ID", "ID");
                columns.Add("附件", "附件");
                columns.Add("startData", "故障发生日期时间");
                              
                columns.Add("Line", "线别");
                columns.Add("GuZhangSheBeiMingCheng", "障碍设备名称");
                columns.Add("SheBeiChangJia", "设备厂家");
                columns.Add("GongQuMingCheng", "工区");
                columns.Add("CheJianMingCheng", "责任单位");
                columns.Add("ZhangAiXianXiang", "障碍现象");
                columns.Add("YingXiangFanWei", "影响范围");
                columns.Add("YuanYingFenXi", "原因分析");
                columns.Add("DiDian", "故障地点");
                columns.Add("ShiFouBaoJu", "是否报局");
                columns.Add("DuanDing", "段定");
                columns.Add("JuDing", "局定");
                columns.Add("ShuJuLeiXing", "数据类型");
                columns.Add("DingXing", "定性");
                columns.Add("DingZe", "定责");
                columns.Add("chuliDanWei", "处理单位");
                columns.Add("ShouLiTime", "受理日期时间");
                columns.Add("YanShi", "延时");
                columns.Add("JingJiSunShi", "经济损失");
                columns.Add("KaoHeYiJian", "考核意见");
                columns.Add("ShebeiTypeYiJi", "设备类型一级");
                columns.Add("SheBeiTypeErJi", "设备类型二级");

                foreach (var item in columns)
                {
                    CheckBox box = new CheckBox();
                    box.Margin = new Thickness(10, 10, 0, 0);
                    box.Content = item.Value;
                    box.Tag = item.Key;
                    wrapPanel1.Children.Add(box);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "NewMethod", ex.ToString());
            }
            finally
            {
            }
        }
        #endregion

        #region 确认按钮事件
        string viewMessage = "";
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //将列表中选中的添加到字符串
                foreach (CheckBox item in wrapPanel1.Children)
                {
                    if (item.IsChecked == true)
                    {
                        VisbleList.Add(item.Tag.ToString(), item.Content.ToString());
                        viewMessage += item.Tag + "," + item.Content + ";";
                    }
                }
               
                //判断是否永久保存
                if (yongjiu.IsChecked == true && !string.IsNullOrEmpty(Proxy.UserName))
                {//写入配置文件
                    IniConfig.IniWriteValue(Proxy.UserName, "FuHeChaXunView", UIHelper.tobase(viewMessage.Substring(0, viewMessage.LastIndexOf(';'))));

                }

                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Button_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        #endregion
    }
}
