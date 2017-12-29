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
    /// ComSearch.xaml 的交互逻辑
    /// </summary>
    public partial class ComSearch : UserControl
    {
        #region 声明变量
        /// <summary>
        /// 加载列数
        /// </summary>
        bool isJiaZaiLie = false;
        /// <summary>
        /// 类集合
        /// </summary>
        List<ComSearchlei> list = new List<ComSearchlei>();
        /// <summary>
        /// 区域集合
        /// </summary>
        List<string> camls = new List<string>();
        /// <summary>
        /// 获取所有二级设备
        /// </summary>
        List<string> allerjishebei = new List<string>();
        /// <summary>
        /// 选中的二级设备
        /// </summary>
        List<string> selecterjishebei = new List<string>();
        /// <summary>
        /// 存储多选的checkbox项
        /// </summary>
        List<CheckBox> checkList = new List<CheckBox>();
        /// <summary>
        /// ComboBox集合
        /// </summary>
        List<ComboBox> comboboxList = new List<ComboBox>();
        /// <summary>
        /// TextBox集合
        /// </summary>
        List<TextBox> textBoxList = new List<TextBox>();
        /// <summary>
        /// Chechbox集合
        /// </summary>
        List<CheckBox> leihaiCheckBox = new List<CheckBox>();
        /// <summary>
        /// 存放多选复合查询条件字典
        /// </summary>
        Dictionary<string, string> caml = new Dictionary<string, string>();
        /// <summary>
        /// 存放总复合查询条件字典
        /// </summary>
        public Dictionary<string, string> camlZong = new Dictionary<string, string>();
        /// <summary>
        /// 存放显示字段查询条件字典
        /// </summary>
        public Dictionary<string, string> camlZ = new Dictionary<string, string>();
        /// <summary>
        /// 存放其他查询条件字典
        /// </summary>
        Dictionary<string, string> diccaml = new Dictionary<string, string>();
        /// <summary>
        /// 全选控件
        /// </summary>
        AllXuan allXuan;
        /// <summary>
        /// 全选集合
        /// </summary>
        List<AllXuan> listAllXuan = new List<AllXuan>();
        /// <summary>
        /// 故障类别全选控件
        /// </summary>
        AllXuan allGZtype;
        /// <summary>
        /// 全局行集合数据
        /// </summary>
        List<string> listQuJu = new List<string>();
        /// <summary>
        /// 全局列生成控件及绑定数据
        /// </summary>
        Dictionary<string, List<string>> dicCamlQuJu = new Dictionary<string, List<string>>();
        /// <summary>
        /// 类集合
        /// </summary>
        List<ComSearchlei> listCom = new List<ComSearchlei>();
        /// <summary>
        /// 参数数量集合
        /// </summary>
        Dictionary<int, int> dicint = new Dictionary<int, int>();

        /// <summary>
        /// 环比开始时间
        /// </summary>
        public DateTime HuanBistartData = default(DateTime);

        /// <summary>
        /// 环比结束时间
        /// </summary>
        public DateTime HuanBiEndData = default(DateTime);
     
        /// <summary>
        /// 开始日期
        /// </summary>
        string startData = "startData";

        #endregion

        #region 自定义事件委托

        public delegate void RegeditEventHandle(ref string cmbName,ref ComboBox cmb,ComSearch coms);
        /// <summary>
        /// cmb注册事件
        /// </summary>
        public event RegeditEventHandle RegeditEvent = null;

        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="list">行集合</param>
        /// <param name="dicCaml">列参数集合</param>
        public ComSearch(List<string> list, Dictionary<string, List<string>> dicCaml)
        {
            try
            {
                InitializeComponent();
                listQuJu = list;
                dicCamlQuJu = dicCaml;
                //设置初始值
                ParmesInit();
                //初始化控件方法
                InitMethod(list, dicCaml);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ComSearch", ex.ToString(), list, dicCaml);
            }
            finally
            {
            }
        }

        public ComSearch()
        {
            InitializeComponent();
        }

        #endregion

        #region 生成窗体方法
        /// <summary>
        /// 生成窗体方法
        /// </summary>
        /// <param name="list">行集合</param>
        /// <param name="dicCaml">列参数集合</param>
        public void DataInit(List<string> list, Dictionary<string, List<string>> dicCaml)
        {
            try
            {
                listQuJu = list;
                dicCamlQuJu = dicCaml;
                //设置初始值
                ParmesInit();
                InitMethod(list, dicCaml);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DataInit", ex.ToString(), list, dicCaml);
            }
            finally
            {
            }
        }

        #endregion

        #region 窗体初始化
        /// <summary>
        /// 初始化时间
        /// </summary>
        void ParmesInit()
        {
            try
            {
                #region 同期\环比参数

                var nowYear = DateTime.Now.Year;

                for (int i = nowYear; i>nowYear-10 ; i--)
                {
                    this.cmbTongQiYear1.Items.Add(i +"年");
                    this.cmbTongQiYear2.Items.Add(i + "年");

                    this.CmbHuanBiYear.Items.Add(i + "年");
                }

                for (int i = 1; i < 13; i++)
                {
                    this.CmbHuanBiMonth.Items.Add(i + "月");

                    if (i == DateTime.Now.Month)
                        this.CmbHuanBiMonth.SelectedIndex = i - 1;
                }

                #endregion


                startEndTime1.DPStartData.Text = DateTime.Now.AddMonths(-1).ToLongDateString();
                startEndTime1.DPEndData.Text = DateTime.Now.ToLongDateString();
                getGZtype();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ParmesInit", ex.ToString());
            }
            finally
            {
            }
        }
        /// <summary>
        /// 初始化故障类型
        /// </summary>
        void getGZtype()
        {
            try
            {
                GZ类别.Items.Clear();
                GZ类别.Items.Add("故障类型");
                allGZtype = new AllXuan();
                allGZtype.cbkAll.Content = "全选";
                allGZtype.cbkAll.Tag = "ShuJuLeiXing";
                allGZtype.cbkAll.Click += new RoutedEventHandler(cbkAll_Click);
                GZ类别.Items.Add(allGZtype);

                ComboBoxItem comboxitem = new ComboBoxItem();
                comboxitem.Content = "普通";
                comboxitem.Tag = "ShuJuLeiXing";
                comboxitem.Style = this.Resources["ComboBoxItemStyle1"] as Style;
                GZ类别.Items.Add(comboxitem);

                ComboBoxItem comboxitem1 = new ComboBoxItem();
                comboxitem1.Content = "C3";
                comboxitem1.Tag = "ShuJuLeiXing";
                comboxitem1.Style = this.Resources["ComboBoxItemStyle1"] as Style;
                GZ类别.Items.Add(comboxitem1);

                ComboBoxItem comboxitem2 = new ComboBoxItem();
                comboxitem2.Content = "高铁";
                comboxitem2.Tag = "ShuJuLeiXing";
                comboxitem2.Style = this.Resources["ComboBoxItemStyle1"] as Style;
                GZ类别.Items.Add(comboxitem2);
                GZ类别.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "getGZtype", ex.ToString());
            }
            finally
            {
            }
        }
        /// <summary>
        /// 初始化窗体方法
        /// </summary>
        /// <param name="list">行参数集合</param>
        /// <param name="dicCaml">列参数集合</param>
        private void InitMethod(List<string> list, Dictionary<string, List<string>> dicCaml)
        {
            try
            {
                //实例化grid
                Grid grid = new Grid();
                grid.HorizontalAlignment = HorizontalAlignment.Stretch;
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                //grid.ShowGridLines = true;
                for (int i = 0; i < list.Count; i++)
                {
                    #region 创建行及区域标识
                    grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(35) });
                    Border border = new Border();
                    border.BorderBrush = new SolidColorBrush(Colors.Gray);
                    border.BorderThickness = new Thickness(1, 0, 1, 1);
                    TextBlock text = new TextBlock();
                    text.Text = list[i].Split(new Char[] { ',' })[0].ToString() + ":";
                    //text.Text = list[i].ToString() + ":";
                    text.Margin = new Thickness(10, 0, 0, 0);
                    text.HorizontalAlignment = HorizontalAlignment.Left;
                    text.VerticalAlignment = VerticalAlignment.Center;
                    Grid.SetRow(border, i);
                    border.Child = text;
                    grid.Children.Add(border);
                    #endregion
                    //获取区域的列的数量
                    int Shu = Convert.ToInt32(list[i].Split(new Char[] { ',' })[1]);
                    //添加带字典目录中
                    dicint.Add(i, Shu);
                    //取最大列数
                    int max = dicint.Values.Max();
                    //判断是否加载列
                    if (!isJiaZaiLie)
                    {
                        for (int l = 0; l < max; l++)
                        {
                            //创建列
                            grid.ColumnDefinitions.Add(new ColumnDefinition());
                        }
                        isJiaZaiLie = true;
                    }
                    //将区域对应的字典集合的数据转为list
                    listCom.Clear();
                    foreach (var item in dicCaml)
                    {
                        string key = item.Key;
                        string[] keys = key.Split(new Char[] { ';' });
                        if (list[i].Split(new Char[] { ',' })[0].ToString() == keys[0])
                        {
                            ComSearchlei comlei = new ComSearchlei(keys[0], keys[1], keys[2], keys[3], keys[4], item.Value);
                            listCom.Add(comlei);
                        }
                    }
                    //循环参数项及绑定数据
                    for (int j = 1; j < listCom.Count + 1; j++)
                    {
                        //判断list.区域等于区域数据
                        if (listCom[j - 1].Quyu.Equals(list[i].Split(new Char[] { ',' })[0].ToString()))
                        {
                            //判断字段类型为TextBox
                            if (listCom[j - 1].Type == "TextBox")
                            {
                                Border border2 = new Border();
                                border2.BorderBrush = new SolidColorBrush(Colors.Gray);
                                border2.BorderThickness = new Thickness(1, 0, 1, 1);
                                TextBox txt = new TextBox();
                                txt.Margin = new Thickness(8, 5, 8, 5);
                                txt.HorizontalAlignment = HorizontalAlignment.Stretch;
                                txt.VerticalAlignment = VerticalAlignment.Stretch;
                                txt.Text = listCom[j - 1].DisplayName;
                                txt.Tag = "txt" + listCom[j - 1].DisplayName;
                                border2.Child = txt;
                                textBoxList.Add(txt);
                                Grid.SetColumn(border2, j);
                                Grid.SetRow(border2, i);
                                grid.Children.Add(border2);
                            }//判断字段类型为ComboBox
                            else if (listCom[j - 1].Type == "ComboBox")
                            {
                                //判断是否多选
                                if (listCom[j - 1].IsDuoXuan == "是")
                                {
                                    Border border1 = new Border();
                                    border1.BorderBrush = new SolidColorBrush(Colors.Gray);
                                    border1.BorderThickness = new Thickness(1, 0, 1, 1);


                                    ComboBox cmb = new ComboBox();
                                    cmb.HorizontalAlignment = HorizontalAlignment.Stretch;
                                    cmb.VerticalAlignment = VerticalAlignment.Stretch;
                                    cmb.Style = this.Resources["ComboBoxStyle1"] as Style;
                                    cmb.Margin = new Thickness(8, 5, 8, 5);

                                    ComboBoxItem item = new ComboBoxItem();
                                    item.Content = "选择" + listCom[j - 1].DisplayName;
                                    cmb.Tag = "cmb" + listCom[j - 1].DisplayName;

                                    string tag = cmb.Tag.ToString();

                                    //注册事件
                                    if (RegeditEvent != null)
                                        RegeditEvent(ref tag, ref cmb,this);

                                    cmb.Items.Add(item);
                                    allXuan = new AllXuan();
                                    allXuan.Tag = listCom[j - 1].Ziduan;
                                    allXuan.cbkAll.Content = "全选";
                                    allXuan.cbkAll.Tag = listCom[j - 1].Ziduan;
                                    allXuan.cbkAll.Click += new RoutedEventHandler(cbkAll_Click);
                                    listAllXuan.Add(allXuan);
                                    cmb.Items.Add(allXuan);
                                    List<string> listShu = listCom[j - 1].ListShuJu;
                                    foreach (var items in listShu)
                                    {
                                        ComboBoxItem comboxitem = new ComboBoxItem();
                                        comboxitem.Content = items;
                                        comboxitem.Tag = listCom[j - 1].Ziduan;
                                        comboxitem.Style = this.Resources["ComboBoxItemStyle1"] as Style;
                                        cmb.Items.Add(comboxitem);
                                    }
                                    border1.Child = cmb;
                                    cmb.SelectedIndex = 0;


                                    Grid.SetColumn(border1, j);
                                    Grid.SetRow(border1, i);
                                    grid.Children.Add(border1);
                                }//单选的ComboBox
                                else
                                {
                                    Border border1 = new Border();
                                    border1.BorderBrush = new SolidColorBrush(Colors.Gray);
                                    border1.BorderThickness = new Thickness(1, 0, 1, 1);
                                    ComboBox cmb = new ComboBox();
                                    cmb.HorizontalAlignment = HorizontalAlignment.Stretch;
                                    cmb.VerticalAlignment = VerticalAlignment.Stretch;
                                    cmb.Style = this.Resources["ComboBoxStyle1"] as Style;
                                    cmb.Margin = new Thickness(8, 5, 8, 5);
                                    ComboBoxItem item = new ComboBoxItem();
                                    item.Content = "选择" + listCom[j - 1].DisplayName;
                                    cmb.Tag = "cmb" + listCom[j - 1].DisplayName;
                                    cmb.Items.Add(item);
                                    List<string> listShu = listCom[j - 1].ListShuJu;
                                    foreach (var items in listShu)
                                    {
                                        ComboBoxItem comboxitem = new ComboBoxItem();
                                        comboxitem.Content = items;
                                        cmb.Items.Add(comboxitem);
                                    }
                                    cmb.DropDownClosed += new EventHandler(cmb_DropDownClosed);
                                    border1.Child = cmb;
                                    cmb.SelectedIndex = 0;
                                    comboboxList.Add(cmb);
                                    Grid.SetColumn(border1, j);
                                    Grid.SetRow(border1, i);
                                    grid.Children.Add(border1);
                                }
                            }//判断字段类型为CheckBox
                            else if (listCom[j - 1].Type == "CheckBox")
                            {
                                Border border2 = new Border();
                                border2.BorderBrush = new SolidColorBrush(Colors.Gray);
                                border2.BorderThickness = new Thickness(1, 0, 1, 1);
                                CheckBox ckb = new CheckBox();
                                ckb.HorizontalAlignment = HorizontalAlignment.Stretch;
                                ckb.VerticalAlignment = VerticalAlignment.Stretch;
                                ckb.Margin = new Thickness(8, 10, 8, 5);
                                ckb.Click += new RoutedEventHandler(ckb_Click);
                                ckb.Content = "         " + listCom[j - 1].DisplayName;
                                ckb.Tag = "ckb" + listCom[j - 1].DisplayName;
                                border2.Child = ckb;
                                leihaiCheckBox.Add(ckb);
                                Grid.SetColumn(border2, j);
                                Grid.SetRow(border2, i);
                                grid.Children.Add(border2);
                            }
                        }
                    }
                    expander1.Child = grid;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "InitMethod", ex.ToString());
            }
            finally
            {
            }
        }
        #endregion

        #region  获取数据

        #region 获取数据 包括下拉或输入的信息
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns>diccaml</returns>
        Dictionary<string, string> ShuJu()
        {
            try
            {
                diccaml.Clear();
                #region 获取日期和时间
                //获取日期时间值
                if (checkBox1.IsChecked == false && checkBox1.IsEnabled == true)
                {
                    try
                    {
                        if (startEndTime1.DPStartData.Visibility == Visibility.Visible && !String.IsNullOrWhiteSpace(startEndTime1.DPStartData.Text))
                        {
                            diccaml.Add(startData, startEndTime1.DPStartData.Text.Replace(".", "-").ToString() + "T" + startEndTime1.CMStartTime.SelectionBoxItem + ":00:00Z" + ",#DateTime#Geq");
                        }
                        else
                        {
                            diccaml.Add(startData, DateTime.Now.Year.ToString() + "-" + DateTime.Now.AddMonths(-1).Month.ToString() + "-1" + "T" + startEndTime1.CMStartTime.SelectionBoxItem + ":00:00Z" + ",#DateTime#Geq");
                        }
                        if (startEndTime1.DPEndData.Visibility == Visibility.Visible && !String.IsNullOrWhiteSpace(startEndTime1.DPEndData.Text))
                        {
                            diccaml.Add("EndData", startEndTime1.DPEndData.Text.Replace(".", "-").ToString() + "T" + startEndTime1.CMEndTime.SelectionBoxItem + ":59:59Z" + ",#DateTime#Leq");
                        }
                        else
                        {
                            diccaml.Add("EndData", DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + "T" + startEndTime1.CMStartTime.SelectionBoxItem + ":59:59Z" + ",#DateTime#Leq");
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                else if (checkBox1.IsChecked == true && checkBox1.IsEnabled == true)
                {
                    try
                    {
                        if (startEndTime1.DPStartData.Visibility == Visibility.Visible && !String.IsNullOrWhiteSpace(startEndTime1.DPStartData.Text))
                        {
                            diccaml.Add(startData, startEndTime1.DPStartData.Text.Replace(".", "-").ToString() + "T" + startEndTime1.CMStartTime.SelectionBoxItem + ":00:00Z" + ",#DateTime#Geq");
                        }
                        else
                        {
                            diccaml.Add(startData, DateTime.Now.Year.ToString() + "-" + DateTime.Now.AddMonths(-1).Month.ToString() + "-1" + "T" + startEndTime1.CMStartTime.SelectionBoxItem + ":00:00Z" + ",#DateTime#Geq");
                        }
                        if (startEndTime1.DPEndData.Visibility == Visibility.Visible && !String.IsNullOrWhiteSpace(startEndTime1.DPEndData.Text))
                        {
                            diccaml.Add("EndData", startEndTime1.DPEndData.Text.Replace(".", "-").ToString() + "T" + startEndTime1.CMEndTime.SelectionBoxItem + ":59:59Z" + ",#DateTime#Leq");
                        }
                        else
                        {
                            diccaml.Add("EndData", DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + "T" + startEndTime1.CMStartTime.SelectionBoxItem + ":59:59Z" + ",#DateTime#Leq");
                        }
                    }
                    catch (Exception)
                    {
                    }

                }
                #endregion

                #region 将字段数据转为List
                list.Clear();
                foreach (var items in dicCamlQuJu)
                {
                    string key = items.Key;
                    string[] keys = key.Split(new Char[] { ';' });
                    ComSearchlei comlei = new ComSearchlei(keys[0], keys[1], keys[2], keys[3], keys[4], items.Value);
                    list.Add(comlei);
                }
                #endregion

                #region 循环收集单选的数据
                //循环收集单选的数据
                foreach (var item in comboboxList)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (item.Tag.ToString() == "cmb" + list[i].DisplayName && !item.SelectionBoxItem.ToString().Contains("选择" + list[i].DisplayName) && item.Visibility == Visibility.Visible && item.SelectionBoxItem != null && !item.SelectionBoxItem.ToString().Contains("大于") && !item.SelectionBoxItem.ToString().Contains("-"))
                        {
                            diccaml.Add(list[i].Ziduan, item.SelectionBoxItem.ToString().Trim() + ",#Text#Eq");
                        }
                        else if (item.Tag.ToString() == "cmb" + list[i].DisplayName && !item.SelectionBoxItem.ToString().Contains("选择" + list[i].DisplayName) && item.Visibility == Visibility.Visible && item.SelectionBoxItem != null && item.SelectionBoxItem.ToString().Contains("-"))
                        {
                            if (item.SelectionBoxItem.ToString().Contains("分钟"))
                            {
                                diccaml.Add(list[i].Ziduan, item.SelectionBoxItem.ToString().Replace("分钟", "").Trim().Split(new char[] { '-' })[0] + ",#Number#Geq");
                                diccaml.Add(list[i].Ziduan + "_", item.SelectionBoxItem.ToString().Replace("分钟", "").Trim().Split(new char[] { '-' })[1] + ",#Number#Leq");
                            }
                            else
                            {
                                diccaml.Add(list[i].Ziduan, item.SelectionBoxItem.ToString().Trim().Split(new char[] { '-' })[0] + ",#Number#Geq");
                                diccaml.Add("_" + list[i].Ziduan, item.SelectionBoxItem.ToString().Trim().Split(new char[] { '-' })[1] + ",#Number#Leq");
                            }
                        }
                        else if (item.Tag.ToString() == "cmb" + list[i].DisplayName && !item.SelectionBoxItem.ToString().Contains("选择" + list[i].DisplayName) && item.Visibility == Visibility.Visible && item.SelectionBoxItem != null && item.SelectionBoxItem.ToString().Contains("大于"))
                        {
                            if (item.SelectionBoxItem.ToString().Contains("分钟"))
                            {
                                diccaml.Add(list[i].Ziduan, item.SelectionBoxItem.ToString().Replace("分钟", "").Trim().Split(new char[] { '于' })[1] + ",#Number#Geq");
                                diccaml.Add(list[i].Ziduan + "_", "100000,#Number#Leq");
                            }
                            else
                            {
                                diccaml.Add(list[i].Ziduan, item.SelectionBoxItem.ToString().Split(new char[] { '于' })[1] + ",#Text#Geq");
                                diccaml.Add("_" + list[i].Ziduan, "100000,#Text#Leq");
                            }
                        }
                    }
                }
                #endregion

                #region 循环收集文本框的值
                //循环收集文本框的值
                foreach (var item in textBoxList)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (item.Tag.ToString() == "txt" + list[i].DisplayName && item.Visibility == Visibility.Visible && item.Text != null && !item.Text.Contains(list[i].DisplayName))
                        {
                            diccaml.Add(list[i].Ziduan, item.Text.Trim() + ",#Text#Eq");
                        }
                    }
                }
                #endregion

                #region 循环收集Checkbox的值
                //循环收集Checkbox的值
                foreach (var item in leihaiCheckBox)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (item.Tag.ToString() == "ckb" + list[i].DisplayName && item.Visibility == Visibility.Visible && item.IsChecked == true && item.IsEnabled == true)
                        {
                            diccaml.Add(list[i].Ziduan, "是" + ",#Text#Eq");
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ShuJu", ex.ToString());
            }
            finally
            {
            }
            return diccaml;
        }
        #endregion

        #region 获取环比数据
    
        public Dictionary<string, string> GetHuanBiShuJu()
        {          
            /// <summary>
            /// 环比参数
            /// </summary>
            Dictionary<string, string> dicHuanbiZhongShuJu = new Dictionary<string, string>();

            var year = this.CmbHuanBiYear.SelectedItem.ToString() ;

            var month = this.CmbHuanBiMonth.SelectedItem.ToString();

            HuanBistartData = Convert.ToDateTime(year + month).AddMonths(-1);

            HuanBiEndData = Convert.ToDateTime(year + month).AddMonths(1).AddSeconds(-1);

            dicHuanbiZhongShuJu.Add("startData", HuanBistartData.ToShortDateString() + "T" + HuanBistartData.ToLongTimeString() + "Z,#DateTime#Geq");
            dicHuanbiZhongShuJu.Add("EndData", HuanBiEndData.ToShortDateString() + "T" + HuanBiEndData.ToLongTimeString() + "Z,#DateTime#Leq");

            return dicHuanbiZhongShuJu;
        }

        #endregion

        #region 获取同期数据
      
        public Dictionary<string, string> GetTongQiShuJu1()
        {
            /// <summary>
            /// 同期参数
            /// </summary>
            Dictionary<string, string> dicTongQiZhongShuJu = new Dictionary<string, string>();

            var year1 = this.cmbTongQiYear1.SelectedItem.ToString();

            var start = Convert.ToDateTime(year1);

            var end = Convert.ToDateTime(year1).AddYears(1).AddSeconds(-1);

            dicTongQiZhongShuJu.Add("startData", start.ToShortDateString() + "T" + start.ToLongTimeString() + "Z,#DateTime#Geq");
            dicTongQiZhongShuJu.Add("EndData", end.ToShortDateString() + "T" + end.ToLongTimeString() + "Z,#DateTime#Leq");
          
            return dicTongQiZhongShuJu;
        }

        public Dictionary<string, string> GetTongQiShuJu2()
        {
            /// <summary>
            /// 同期参数
            /// </summary>
            Dictionary<string, string> dicTongQiZhongShuJu = new Dictionary<string, string>();

            var year2 = this.cmbTongQiYear2.SelectedItem.ToString();

            var start = Convert.ToDateTime(year2);

            var end = Convert.ToDateTime(year2).AddYears(1).AddSeconds(-1);

            dicTongQiZhongShuJu.Add("startData", start.ToShortDateString() + "T" + start.ToLongTimeString() + "Z,#DateTime#Geq");
            dicTongQiZhongShuJu.Add("EndData", end.ToShortDateString() + "T" + end.ToLongTimeString() + "Z,#DateTime#Leq");

            return dicTongQiZhongShuJu;
        }

        /// <summary>
        /// 搜索条件完整的字典目录 {[startData, 2014/2/27T0:00:00Z,#DateTime#Geq]}  {[EndData, 2014/3/27T23:59:59Z,#DateTime#Leq]}
        /// </summary>
        /// <returns>Dictionary</returns>
        public Dictionary<string, string> ZongShuJu()
        {

            camlZong.Clear();
            try
            {
                //收集数据
                camlZong = ShuJu();
                //循环添加多选数据
                foreach (var item in caml)
                {
                    camlZong.Add(item.Key, item.Value);
                }
                //判断字典中包括leihai和shebeileixingerji
                if (camlZong.Keys.Contains("LeiHai") && camlZong.Keys.Contains("SheBeiTypeErJi"))
                {
                    if (camlZong["SheBeiTypeErJi"].Contains("雷害"))
                    {
                        camlZong.Remove("LeiHai");
                    }
                    else
                    {
                        camlZong.Remove("LeiHai");
                        camlZong["SheBeiTypeErJi"] = camlZong["SheBeiTypeErJi"].Replace(",#", ",雷害,#");
                    }
                }
                //判断字典中不包括leihai,并且包含leihai
                else if (!camlZong.Keys.Contains("SheBeiTypeErJi") && camlZong.Keys.Contains("LeiHai"))
                {
                    camlZong.Remove("LeiHai");
                    camlZong.Add("SheBeiTypeErJi", "雷害,#Text#Eq");
                }
                else if (camlZong.Keys.Contains("LeiHai"))
                {
                    camlZong.Remove("LeiHai");
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ZongShuJu", ex.ToString());
            }
            finally
            {
            }
            return camlZong;
        }

        #endregion

        #endregion

        #region 多选操作
        /// <summary>
        /// 点击CheckBox事件
        /// </summary>
        /// <param name="sender">CheckBox</param>
        /// <param name="e">RoutedEventArgs</param>
        private void checkBox_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MoreCheck(sender);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "checkBox_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 多选方法
        /// </summary>
        /// <param name="sender">CheckBox</param>
        private void MoreCheck(object sender)
        {
            try
            {
                CheckBox selectCOB = ((CheckBox)sender) as CheckBox;
                //如果选中
                if (selectCOB.IsChecked == true)
                {
                    #region 判断全选状态
                    AllCheckStateFlase(selectCOB);
                    #endregion
                    //如果存在，直接追加
                    if (caml.Keys.Contains(selectCOB.Tag.ToString()))
                    {
                        string linkmessage = caml[selectCOB.Tag.ToString()];
                        caml[selectCOB.Tag.ToString()] = linkmessage.Insert(linkmessage.IndexOf("#"), selectCOB.Content.ToString() + ",");

                    }
                    //第一次加入
                    else
                    {
                        if (selectCOB.Tag.ToString().Equals("CheJianMingCheng") || selectCOB.Tag.ToString().Equals("GongQuMingCheng"))
                        {
                            caml.Add(selectCOB.Tag.ToString(), selectCOB.Content.ToString() + ",#Text#Contains");
                        }
                        else
                        {
                            caml.Add(selectCOB.Tag.ToString(), selectCOB.Content.ToString() + ",#Text#Eq");
                        }
                    }
                    //checkList.Clear();
                    ListAddCheckBox(selectCOB);
                }
                //选择取消项
                else
                {
                    ListRemoveCheckBox(selectCOB);
                    //如果存在直接替换
                    if (caml.Keys.Contains(selectCOB.Tag.ToString()))
                    {
                        if (selectCOB.Tag.ToString() == "DingZe")
                        {
                            #region 定责
                            string[] dingzelinshi = caml[selectCOB.Tag.ToString()].Split(new char[] { '#' })[0].Remove(caml[selectCOB.Tag.ToString()].Split(new char[] { '#' })[0].LastIndexOf(",")).Split(new char[] { ',' });

                            List<string> dingze = dingzelinshi.ToList();

                            string aa = "";

                            try
                            {
                                foreach (string item in dingzelinshi)
                                {

                                    if (item == selectCOB.Content.ToString())
                                    {
                                        dingze.Remove(selectCOB.Content.ToString());
                                    }
                                    else
                                    {
                                        aa += item + ",";
                                    }
                                }
                            }
                            catch (Exception)
                            {
                            }
                            caml[selectCOB.Tag.ToString()] = aa + "#Text#Eq";
                            #endregion
                        }
                        else if (selectCOB.Tag.ToString() == "SheBeiTypeYiJi" && caml.Keys.Contains("SheBeiTypeErJi"))
                        {
                            string linkmessage = caml[selectCOB.Tag.ToString()];
                            caml[selectCOB.Tag.ToString()] = linkmessage.Replace(selectCOB.Content.ToString() + ",", "");
                        }
                        else if (caml.Keys.Contains("SheBeiTypeErJi"))
                        {
                            try
                            {
                                string linkmessage = caml[selectCOB.Tag.ToString()];
                                //判断项的选中状态
                                if (linkmessage.Contains("传输设备") || linkmessage.Contains("雷害") || linkmessage.Contains("其它"))
                                {
                                    ListClearCheckBoxT(selectCOB.Tag.ToString(), selectCOB.Content.ToString());
                                }
                                #region 特殊处理
                                string[] SheBeiTypeErJilinshi = caml[selectCOB.Tag.ToString()].Split(new char[] { '#' })[0].Remove(caml[selectCOB.Tag.ToString()].Split(new char[] { '#' })[0].LastIndexOf(",")).Split(new char[] { ',' });

                                List<string> dingze = SheBeiTypeErJilinshi.ToList();

                                string SheBeiTypeErJitext = "";
                                try
                                {
                                    foreach (string item in SheBeiTypeErJilinshi)
                                    {

                                        if (item == selectCOB.Content.ToString())
                                        {
                                            dingze.Remove(selectCOB.Content.ToString());
                                        }
                                        else
                                        {
                                            SheBeiTypeErJitext += item + ",";
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                }
                                caml[selectCOB.Tag.ToString()] = SheBeiTypeErJitext + "#Text#Eq";
                                #endregion
                            }
                            catch (Exception)
                            {
                            }
                        }
                        else
                        {
                            string linkmessage = caml[selectCOB.Tag.ToString()];
                            caml[selectCOB.Tag.ToString()] = linkmessage.Replace(selectCOB.Content.ToString() + ",", "");

                        }
                    }
                    //如果值空了  移除键
                    if (caml[selectCOB.Tag.ToString()] == "#Text#Eq" || caml[selectCOB.Tag.ToString()] == "#Text#Contains")
                    {
                        caml.Remove(selectCOB.Tag.ToString());
                        #region 没有任何子项的情况下,全选选中
                        AddAllCheckTrue(selectCOB);
                        #endregion
                    }
                }
                CheckShuJu();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "MoreCheck", ex.ToString(), sender);
            }
            finally
            {
            }
        }
        #endregion
     
        #region 传统日期验证方法
        /// <summary>
        /// 传统日期验证方法
        /// </summary>
        /// <returns>bool</returns>
        public bool checkChuanTongDate()
        {
            bool boolCheck = true;
            try
            {
                if (checkBox1.IsChecked == true && checkBox1.IsEnabled == true)
                {
                    //开始时间
                    DateTime StartDates = Convert.ToDateTime(startEndTime1.DPStartData.Text.Trim());
                    //结束时间
                    DateTime EndDates = Convert.ToDateTime(startEndTime1.DPEndData.Text.Trim());
                    //结束时间>开始时间
                    if (EndDates > StartDates)
                    {
                        //开始月份
                        int intMouth = StartDates.Month;
                        //开始日期
                        int intDay = StartDates.Day;
                        //结束月份
                        int intMouth2 = EndDates.Month;
                        //结束日期
                        int intDay2 = EndDates.Day;
                        //开始月份不等于结束月份
                        if (intMouth != intMouth2)
                        {
                            //日期差
                            int chaDay = intDay - intDay2;
                            if (chaDay != 1)
                            {
                                boolCheck = false;
                            }
                        }//开始月份等于结束月份
                        else if (intMouth == intMouth2)
                        {
                            if (intDay != 1 && intDay2 != 30)
                            {
                                boolCheck = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "checkChuanTongDate", ex.ToString());
            }
            finally
            {
            }
            return boolCheck;
        }
        #endregion

        #region  将字典目录的数据放到搜索信息提示中
        /// <summary>
        /// 将字典目录的数据放到搜索信息提示中
        /// </summary>
        void CheckShuJu()
        {
            try
            {
                camlZ.Clear();
                //总的字典目录
                camlZ = ShuJu();
                foreach (var item in caml)
                {
                    camlZ.Add(item.Key, item.Value);
                }
                //总的字典目录数据
                txtXinxi.Text = "";
                if (camlZ.Keys.Contains(startData) && camlZ.Keys.Contains("EndData"))
                {
                    camlZ.Remove(startData);
                    camlZ.Remove("EndData");
                    list.Clear();
                    foreach (var items in dicCamlQuJu)
                    {
                        string key = items.Key;
                        string[] keys = key.Split(new Char[] { ';' });
                        ComSearchlei comlei = new ComSearchlei(keys[0], keys[1], keys[2], keys[3], keys[4], items.Value);
                        list.Add(comlei);
                    }
                    //循环判断点选字段,并且有"_"范围性数据
                    foreach (var item in comboboxList)
                    {
                        for (int j = 0; j < list.Count; j++)
                        {
                            if (item.Tag.ToString() == "cmb" + list[j].DisplayName && item.Visibility == Visibility.Visible && item.SelectionBoxItem != null && item.SelectionBoxItem.ToString() != "选择" + list[j].DisplayName)
                            {
                                if (camlZ.Keys.Contains(list[j].Ziduan) && camlZ.Keys.Contains(list[j].Ziduan + "_"))
                                {
                                    camlZ.Remove(list[j].Ziduan);
                                    camlZ.Remove(list[j].Ziduan + "_");
                                    camlZ.Add(list[j].Ziduan, item.SelectionBoxItem.ToString().Trim() + ",");
                                }
                                else if (camlZ.Keys.Contains(list[j].Ziduan) && camlZ.Keys.Contains("_" + list[j].Ziduan))
                                {
                                    camlZ.Remove(list[j].Ziduan);
                                    camlZ.Remove("_" + list[j].Ziduan);
                                    camlZ.Add(list[j].Ziduan, item.SelectionBoxItem.ToString().Trim() + ",");
                                }
                            }
                        }
                    }
                    //循环判断显示在信息提示栏中
                    foreach (var item in camlZ)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (item.Key == list[i].Ziduan)
                            {
                                txtXinxi.Text += item.Key
                                .Replace(list[i].Ziduan, list[i].DisplayName)
                                + ":" + item.Value.Remove(item.Value.LastIndexOf(","))
                                + ";";
                                if (txtXinxi.Text != "")
                                {
                                    borTxt.Visibility = Visibility.Visible;
                                    borT.Visibility = Visibility.Visible;
                                    txtXinxi.Text = txtXinxi.Text;
                                }
                            }
                        }//判断数据类型,替换显示
                        if (item.Key == "ShuJuLeiXing")
                        {
                            txtXinxi.Text += item.Key
                                       .Replace("ShuJuLeiXing", "故障类型")
                                       + ":" + item.Value.Remove(item.Value.LastIndexOf(","))
                                       + "；";
                            if (txtXinxi.Text != "")
                            {
                                borTxt.Visibility = Visibility.Visible;
                                borT.Visibility = Visibility.Visible;
                                txtXinxi.Text = txtXinxi.Text;
                            }
                        }
                    }
                    if (txtXinxi.Text == "")
                    {
                        borTxt.Visibility = Visibility.Collapsed;
                        borT.Visibility = Visibility.Collapsed;
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "CheckShuJu", ex.ToString());
            }
            finally
            {
            }
        }
        #endregion

        #region 设备类型一级下拉关闭事件
        void cmbSheBeiTypeYiJi_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                FuHuoQuZi();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "cmbSheBeiTypeYiJi_DropDownClosed", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 通过父级获取子级
        /// </summary>
        private void FuHuoQuZi()
        {
            try
            {
                #region 暂时的设备类型一级下拉关闭事件
                //try
                //{
                //    if (caml.Keys.Contains("SheBeiTypeYiJi") && !caml.ContainsKey("SheBeiTypeErJi"))
                //    {
                //        List<string> selecterjishebei临时 = new List<string>();

                //        cmbSheBeiTypeErJi.Items.Clear();
                //        selecterjishebei.Clear();
                //        if (caml.Keys.Contains("SheBeiTypeYiJi") && caml["SheBeiTypeYiJi"] != "")
                //        {
                //            for (int i = 0; i < caml["SheBeiTypeYiJi"].Split(new char[] { ',' }).Length; i++)
                //            {
                //                if (caml["SheBeiTypeYiJi"].Split(new char[] { ',' })[i] != "")
                //                {
                //                    selecterjishebei临时.Clear();
                //                    foreach (var item in allerjishebei)
                //                    {
                //                        if (item.Contains(caml["SheBeiTypeYiJi"].Split(new char[] { ',' })[i]))
                //                        {
                //                            selecterjishebei临时.Add(item.Split(new char[] { ',' })[0]);
                //                        }
                //                    }
                //                    selecterjishebei.AddRange(selecterjishebei临时);
                //                }
                //            }
                //        }
                //        if (selecterjishebei.Count >= 1)
                //        {
                //            cmbSheBeiTypeErJi.Items.Clear();
                //            cmbSheBeiTypeErJi.Items.Add("设备类型二级");
                //            allXuanSheBeiTypeErJi = new AllXuan();
                //            allXuanSheBeiTypeErJi.cbkAll.Content = "全选";
                //            allXuanSheBeiTypeErJi.cbkAll.Tag = "SheBeiTypeErJiAll";
                //            cmbSheBeiTypeErJi.Items.Add(allXuanSheBeiTypeErJi);
                //            for (int i = 0; i < selecterjishebei.Count; i++)
                //            {
                //                ComboBoxItem comboxitem = new ComboBoxItem();
                //                comboxitem.Content = selecterjishebei[i].ToString();
                //                comboxitem.Tag = "SheBeiTypeErJi";
                //                comboxitem.Style = this.Resources["ComboBoxItemStyle1"] as Style;
                //                cmbSheBeiTypeErJi.Items.Add(comboxitem);
                //            }
                //            cmbSheBeiTypeErJi.SelectedIndex = 0;
                //        }
                //    }
                //    else if (caml.Keys.Contains("SheBeiTypeYiJi") && caml["SheBeiTypeErJi"] != "")
                //    {
                //    }
                //    else if (!caml.ContainsKey("SheBeiTypeYiJi") && !caml.ContainsKey("SheBeiTypeErJi"))
                //    {
                //        get设备类型二级();
                //    }
                //}
                //catch (Exception)
                //{
                //} 
                #endregion
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "FuHuoQuZi", ex.ToString());
            }
            finally
            {
            }
        }
        #endregion

        #region 与全选有关的事件
        /// <summary>
        /// 修改CheckBox的选中状态为true
        /// </summary>
        /// <param name="selectCOB">CheckBox</param>
        void AddAllCheckTrue(CheckBox selectCOB)
        {
            try
            {
                for (int i = 0; i < listAllXuan.Count; i++)
                {
                    if (selectCOB.Tag.ToString() == listAllXuan[i].Tag.ToString())
                    {
                        listAllXuan[i].cbkAll.IsChecked = true;
                        listAllXuan[i].cbkAll.Click += new RoutedEventHandler(cbkAll_Click);
                    }
                }
                if (selectCOB.Tag.ToString() == "ShuJuLeiXing")
                {
                    allGZtype.cbkAll.IsChecked = true;
                    allGZtype.cbkAll.Click += new RoutedEventHandler(cbkAll_Click);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "AddAllCheckTrue", ex.ToString(), selectCOB);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 判断全选的选中状态为false
        /// </summary>
        /// <param name="selectCOB">CheckBox</param>
        void AllCheckStateFlase(CheckBox selectCOB)
        {
            try
            {
                for (int i = 0; i < listAllXuan.Count; i++)
                {
                    if (selectCOB.Tag.ToString() == listAllXuan[i].Tag.ToString())
                    {
                        listAllXuan[i].cbkAll.IsChecked = false;
                        listAllXuan[i].cbkAll.Click += new RoutedEventHandler(cbkAll_Click);
                    }
                }
                if (selectCOB.Tag.ToString() == "ShuJuLeiXing")
                {
                    allGZtype.cbkAll.IsChecked = false;
                    allGZtype.cbkAll.Click += new RoutedEventHandler(cbkAll_Click);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "AllCheckStateFlase", ex.ToString(), selectCOB);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 集合添加CheckBox
        /// </summary>
        /// <param name="checkBox">CheckBox</param>
        void ListAddCheckBox(CheckBox checkBox)
        {
            try
            {
                if (!checkList.Contains(checkBox))
                {
                    checkList.Add(checkBox);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ListAddCheckBox", ex.ToString(), checkBox);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 集合删除checkbox
        /// </summary>
        /// <param name="checkBox">CheckBox</param>
        void ListRemoveCheckBox(CheckBox checkBox)
        {
            try
            {
                if (checkList.Contains(checkBox))
                {
                    checkList.Remove(checkBox);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ListRemoveCheckBox", ex.ToString(), checkBox);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 清空选中项的选中状态
        /// </summary>
        /// <param name="strBiaoShi">标志</param>
        void ListClearCheckBox(string strBiaoShi)
        {
            try
            {
                foreach (var item in checkList)
                {
                    if (item.Tag.ToString().Equals(strBiaoShi))
                    {
                        item.IsChecked = false;

                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ListClearCheckBox", ex.ToString(), strBiaoShi);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 清空选中项的选中状态特殊的
        /// </summary>
        /// <param name="strBiaoShi">标志</param>
        void ListClearCheckBoxT(string strBiaoShi, string content)
        {
            try
            {
                foreach (var item in checkList)
                {
                    if (item.Content.ToString() == content)
                    {
                        if (item.Content.ToString() == "传输设备" || item.Content.ToString() == "雷害" || item.Content.ToString() == "其它")
                        {
                            item.IsChecked = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ListClearCheckBoxT", ex.ToString(), strBiaoShi, content);
            }
            finally
            {
            }
        }

        #region 全选清空参数并且去掉子项
        /// <summary>
        /// 清空集合项并且删除参数字典
        /// </summary>
        /// <param name="selectCOB1">CheckBox</param>
        void ClearLiatCheckAddRemoveCaml(CheckBox selectCOB1)
        {
            try
            {
                list.Clear();
                foreach (var items in dicCamlQuJu)
                {
                    string key = items.Key;
                    string[] keys = key.Split(new Char[] { ';' });
                    ComSearchlei comlei = new ComSearchlei(keys[0], keys[1], keys[2], keys[3], keys[4], items.Value);
                    list.Add(comlei);
                }
                for (int i = 0; i < list.Count; i++)
                {
                    if (selectCOB1.Tag.ToString() == list[i].Ziduan)
                    {
                        caml.Remove(list[i].Ziduan);
                    }
                }
                if (selectCOB1.Tag.ToString() == "ShuJuLeiXing")
                {
                    caml.Remove("ShuJuLeiXing");
                }
                ListClearCheckBox(selectCOB1.Tag.ToString());
                CheckShuJu();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ClearLiatCheckAddRemoveCaml", ex.ToString(), selectCOB1);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 全选事件
        /// </summary>
        /// <param name="sender">CheckBox</param>
        /// <param name="e">RoutedEventArgs</param>
        void cbkAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox selectCOB1 = sender as CheckBox;
                ClearLiatCheckAddRemoveCaml(selectCOB1);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "cbkAll_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion
        #endregion

        #region 辅助事件
        /// <summary>
        /// 单选下拉关闭事件
        /// </summary>
        /// <param name="sender">ComonBox</param>
        /// <param name="e">EventArgs</param>
        void cmb_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                CheckShuJu();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "cmb_DropDownClosed", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        /// <summary>
        /// checkbox选中事件
        /// </summary>
        /// <param name="sender">CheckBox</param>
        /// <param name="e">RoutedEventArgs</param>
        void ckb_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckShuJu();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ckb_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        #endregion

        #region 预览模式

        /// <summary>
        /// 预览模式
        /// </summary>
        public bool _isHengWatch = false;

        /// <summary>
        /// 横向预览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            _isHengWatch = true;
        }

        /// <summary>
        /// 纵向预览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioButton_Click_1(object sender, RoutedEventArgs e)
        {
            _isHengWatch = false;
        }

        #endregion

        #region 辅助方法
        public void cmbChange(ComboBox cmb,string propertyName,string propertyValue,List<string> listShuJu)
        {         
            ComboBoxItem item = new ComboBoxItem();
            item.Content = "选择" + propertyName;
            cmb.Tag = "cmb" + propertyName;

            string tag = cmb.Tag.ToString();

            //注册事件
            //if (RegeditEvent != null)
            //    RegeditEvent(ref tag, ref cmb,this);

            cmb.Items.Add(item);
            allXuan = new AllXuan();
            allXuan.Tag = propertyValue;
            allXuan.cbkAll.Content = "全选";
            allXuan.cbkAll.Tag = propertyValue;
            allXuan.cbkAll.Click += new RoutedEventHandler(cbkAll_Click);
            listAllXuan.Add(allXuan);
            cmb.Items.Add(allXuan);
            List<string> listShu = listShuJu;
            foreach (var items in listShu)
            {
                ComboBoxItem comboxitem = new ComboBoxItem();
                comboxitem.Content = items;
                comboxitem.Tag = propertyValue;
                comboxitem.Style = this.Resources["ComboBoxItemStyle1"] as Style;
                cmb.Items.Add(comboxitem);
            }           
            cmb.SelectedIndex = 0;
        }

        #endregion
       
    }
}
