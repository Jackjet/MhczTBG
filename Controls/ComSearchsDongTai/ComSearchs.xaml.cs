

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
using TongXin.wcfTX;
using System.Windows.Threading;
using System.Threading;
using MhczTBG.Controls.ComSearchsDongTai;
using SLHelperV1;
using MhczTBG.Controls.CustomWindow;
using MhczTBG.Common;

namespace MhczTBG.Controls
{
    public partial class ComSearchs : UserControl
    {

        #region 一、声明变量
        //服务代理类
        ItxdClient Proxy = new ItxdClient();
        //获取所有二级设备
        List<string> allerjishebei = new List<string>();
       
        List<string> selecterjishebei = new List<string>();
        //存放复合查询条件字典
        Dictionary<string, string> caml = new Dictionary<string, string>();
        //存放复合查询条件字典
        public Dictionary<string, string> camlZong = new Dictionary<string, string>();
        //存放总查询条件字典
        public Dictionary<string, string> camlZ = new Dictionary<string, string>();

        //存放查询条件字典
        Dictionary<string, string> diccaml = new Dictionary<string, string>();
        List<CheckBox> checkList = new List<CheckBox>();

        //用作Excel导出
        List<TXListV2> ListGZ = new List<TXListV2>();
        //辅助类
        txtOperater tHelper = new txtOperater();
        //故障类别全选控件
        AllXuan allGZtype;
        //线别全选控件
        AllXuan allXuanXianBie;
        //车间全选控件
        AllXuan allXuanCheJian;
        //工区全选控件
        AllXuan allXuanGongQu;
        //设备类型一级全选控件
        AllXuan allXuanSheBeiTypeYiJi;
        //设备类型二级全选控件
        AllXuan allXuanSheBeiTypeErJi;
        //定责全选控件
        AllXuan allXuanDingZe;
        //定性全选控件
        AllXuan allXuanDingXing;
        #endregion
       
        #region 二、构造函数
        public ComSearchs()
        {
            InitializeComponent();

            
                #region 注册事件
                cmb是否报局.DropDownClosed += new EventHandler(cmb是否报局_DropDownClosed);
                cmb局定.DropDownClosed += new EventHandler(cmb是否报局_DropDownClosed);
                //cmb局定金额.DropDownClosed += new EventHandler(cmb是否报局_DropDownClosed);
                ckbLeiHai.Click += new RoutedEventHandler(ckbLeiHai_Click);
                cmb间接损失.DropDownClosed += new EventHandler(cmb是否报局_DropDownClosed);
                cmb受理.DropDownClosed += new EventHandler(cmb是否报局_DropDownClosed);
                cmb延时.DropDownClosed += new EventHandler(cmb是否报局_DropDownClosed);
                cmb直接损失.DropDownClosed += new EventHandler(cmb是否报局_DropDownClosed);
                cmb段定.DropDownClosed += new EventHandler(cmb是否报局_DropDownClosed);

                #endregion

                get年月日();

                #region  注册全选点击事件

                allXuanXianBie.cbkAll.Click += new RoutedEventHandler(cbkAll_Click);
                allXuanCheJian.cbkAll.Click += new RoutedEventHandler(cbkAll_Click);
                allXuanGongQu.cbkAll.Click += new RoutedEventHandler(cbkAll_Click);
                allXuanSheBeiTypeYiJi.cbkAll.Click += new RoutedEventHandler(cbkAll_Click);
                allXuanSheBeiTypeErJi.cbkAll.Click += new RoutedEventHandler(cbkAll_Click);
                allXuanDingZe.cbkAll.Click += new RoutedEventHandler(cbkAll_Click);
                allXuanDingXing.cbkAll.Click += new RoutedEventHandler(cbkAll_Click);
                allGZtype.cbkAll.Click += new RoutedEventHandler(cbkAll_Click);
                #endregion
           
         
        }
        #endregion

        #region 三、设置初始值

        #region 1.设置月份年份
        void get年月日()
        {
            cmb年.Items.Clear();  //需要先清除
            cmb年1.Items.Clear();  //需要先清除
            cmb年2.Items.Clear();  //需要先清除
            //添加年份并将年份指定到当前年份
            cmb年.Items.Add("全部");
            for (int i = 0; i < 11; i++)
            {
                cmb年.Items.Add(String.Format("{0}年", (DateTime.Now.Year - i).ToString()));
                cmb年1.Items.Add(String.Format("{0}年", (DateTime.Now.Year - i).ToString()));
                cmb年2.Items.Add(String.Format("{0}年", (DateTime.Now.Year - i).ToString()));

            }
            cmb年.SelectedIndex = 0;
            cmb年1.SelectedIndex = 1;
            cmb年2.SelectedIndex = 0;





            //填充月份
            cmb月.Items.Clear();
            cmb月1.Items.Clear();
            cmb月.Items.Add("全部");
            for (int i = 0; i < 12; i++)
            {
                cmb月.Items.Add(String.Format("{0}月", (i + 1).ToString()));
                cmb月1.Items.Add(String.Format("{0}月", (i + 1).ToString()));

                //默认选中当前月
                if (DateTime.Now.Month == 1)
                {
                    cmb月1.SelectedIndex = 0;
                }
                else
                {
                    if ((i + 1) == DateTime.Now.Month)
                    {

                        cmb月1.SelectedIndex = i;

                    }
                }

            }
            cmb月.SelectedIndex = 0;

            cmb日.Items.Clear();
            cmb日.Items.Add("全部");
            cmb日.SelectedIndex = 0;

            //绑定日期控件DatePicker
            //int myyear = DateTime.Now.Year;
            //int mymonth = DateTime.Now.Month;
            //int myday = DateTime.DaysInMonth(myyear, mymonth);

            //startEndTime1.DPStartData.Text = String.Format("{0}-{1}-1", DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());
            //startEndTime1.DPEndData.Text = String.Format("{0}-{1}-{2}", myyear, mymonth, myday);


            startEndTime1.DPStartData.Text = DateTime.Now.AddMonths(-1).ToLongDateString();
            startEndTime1.DPEndData.Text = DateTime.Now.ToLongDateString();
            get线别();
           getGZtype();
        }
        #endregion

        #region 故障类型
        void getGZtype()
        {

            GZ类别.Items.Clear();
            GZ类别.Items.Add("故障类型");
            allGZtype = new AllXuan();
            allGZtype.cbkAll.Content = "全选";
            allGZtype.cbkAll.Tag = "ShuJuLeiXing";
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
        #endregion

        #region 2.线别
        void get线别()
        {
            cmb线别.Items.Clear();

            cmb线别.Items.Add("选择线别");
            allXuanXianBie = new AllXuan();
            allXuanXianBie.cbkAll.Content = "全选";
            allXuanXianBie.cbkAll.Tag = "Line";
            cmb线别.Items.Add(allXuanXianBie);
            if (MhczTBG.Common.Proxy.LineList != null)
            {
                for (int i = 0; i < MhczTBG.Common.Proxy.LineList.Count; i++)
                {
                    ComboBoxItem comboxitem = new ComboBoxItem();
                    comboxitem.Content = MhczTBG.Common.Proxy.LineList[i];
                    comboxitem.Tag = "Line";
                    comboxitem.Style = this.Resources["ComboBoxItemStyle1"] as Style;
                    cmb线别.Items.Add(comboxitem);
                }
                cmb线别.SelectedIndex = 0;
            }
          
            get车间();
        }
        #endregion

        #region 3.车间
        void get车间()
        {

            cmb车间.Items.Clear();

            cmb车间.Items.Add("选择车间");

            allXuanCheJian = new AllXuan();
            allXuanCheJian.cbkAll.Content = "全选";
            allXuanCheJian.cbkAll.Tag = "CheJianMingCheng";
            cmb车间.Items.Add(allXuanCheJian);
            if (MhczTBG.Common.Proxy.CheJianMingChengList != null)
            {
                for (int i = 0; i < MhczTBG.Common.Proxy.CheJianMingChengList.Count; i++)
                {
                    ComboBoxItem comboxitem = new ComboBoxItem();
                    comboxitem.Content = MhczTBG.Common.Proxy.CheJianMingChengList[i];
                    comboxitem.Tag = "CheJianMingCheng";
                    comboxitem.Style = this.Resources["ComboBoxItemStyle1"] as Style;
                    cmb车间.Items.Add(comboxitem);

                }

                cmb车间.SelectedIndex = 0;
            }
          

            get工区();


        }


        #endregion

        #region 4.工区
        void get工区()
        {
            cmb工区.Items.Clear();

            cmb工区.Items.Add("选择工区");

            allXuanGongQu = new AllXuan();
            allXuanGongQu.cbkAll.Content = "全选";
            allXuanGongQu.cbkAll.Tag = "GongQuMingCheng";
            cmb工区.Items.Add(allXuanGongQu);
            if (ProxyList.CollGongQu!=null)
            {
                for (int i = 0; i < ProxyList.CollGongQu.Count; i++)
                {

                    ComboBoxItem comboxitem = new ComboBoxItem();
                    comboxitem.Content = ProxyList.CollGongQu[i].LinkTitle;
                    comboxitem.Tag = "GongQuMingCheng";
                    comboxitem.Style = this.Resources["ComboBoxItemStyle1"] as Style;
                    cmb工区.Items.Add(comboxitem);

                }

                cmb工区.SelectedIndex = 0;
            }
          

            get设备类型一级();
            get责任类型();
        }
        #endregion

        #region 5.设备类型一级
        void get设备类型一级()
        {

            cmbSheBeiTypeYiJi.Items.Clear();

            cmbSheBeiTypeYiJi.Items.Add("设备类型一级");

            allXuanSheBeiTypeYiJi = new AllXuan();
            allXuanSheBeiTypeYiJi.cbkAll.Content = "全选";
            allXuanSheBeiTypeYiJi.cbkAll.Tag = "ShebeiTypeYiJi";
            cmbSheBeiTypeYiJi.Items.Add(allXuanSheBeiTypeYiJi);
            if (ProxyList.CollSheBeiTypeYiJi!=null)
            {
                for (int i = 0; i < ProxyList.CollSheBeiTypeYiJi.Count; i++)
                {
                    ComboBoxItem comboxitem = new ComboBoxItem();
                    comboxitem.Content = ProxyList.CollSheBeiTypeYiJi[i].LinkTitle;
                    comboxitem.Tag = "ShebeiTypeYiJi";
                    comboxitem.Style = this.Resources["ComboBoxItemStyle1"] as Style;
                    cmbSheBeiTypeYiJi.Items.Add(comboxitem);
                }

                cmbSheBeiTypeYiJi.SelectedIndex = 0;
            }
           

            get设备类型二级();

          

        }
        #endregion

        #region 6.设备类型二级
        void get设备类型二级()
        {
            if (MhczTBG.Common.Proxy.SheBeiTypeErJiList!= null)
            {
                allerjishebei=MhczTBG.Common.Proxy.SheBeiTypeErJiList;
                ////allerjishebei.Add("");
                //string erjishebeifuji = cmbSheBeiTypeYiJi.SelectedItem.ToString();

                //selecterjishebei = allerjishebei.Where(a => a.f父级 == erjishebeifuji).ToList();

                if (allerjishebei.Count >= 1)
                {
                    cmbSheBeiTypeErJi.Items.Clear();
                    cmbSheBeiTypeErJi.Items.Add("设备类型二级");

                    allXuanSheBeiTypeErJi = new AllXuan();
                    allXuanSheBeiTypeErJi.cbkAll.Content = "全选";
                    allXuanSheBeiTypeErJi.cbkAll.Tag = "SheBeiTypeErJi";
                    cmbSheBeiTypeErJi.Items.Add(allXuanSheBeiTypeErJi);
                    for (int i = 0; i < allerjishebei.Count; i++)
                    {

                        ComboBoxItem comboxitem = new ComboBoxItem();
                        comboxitem.Content = allerjishebei[i];
                        comboxitem.Tag = "SheBeiTypeErJi";
                        comboxitem.Style = this.Resources["ComboBoxItemStyle1"] as Style;
                        cmbSheBeiTypeErJi.Items.Add(comboxitem);
                    }

                    cmbSheBeiTypeErJi.SelectedIndex = 0;
                }

            }
          

        
        }


        #endregion

        #region 7.责任类型
        void get责任类型()
        {

            cmb责任定责.Items.Clear();

            cmb责任定责.Items.Add("责任定责");

            allXuanDingZe = new AllXuan();
            allXuanDingZe.cbkAll.Content = "全选";
            allXuanDingZe.cbkAll.Tag = "DingZe";
            cmb责任定责.Items.Add(allXuanDingZe);
            if (Common.Proxy.DingZeList!=null)
            {
                for (int i = 0; i < Common.Proxy.DingZeList.Count; i++)
                {
                    ComboBoxItem comboxitem = new ComboBoxItem();
                    comboxitem.Content = Common.Proxy.DingZeList[i];
                    comboxitem.Tag = "DingZe";
                    comboxitem.Style = this.Resources["ComboBoxItemStyle1"] as Style;
                    cmb责任定责.Items.Add(comboxitem);
                }

                cmb责任定责.SelectedIndex = 0;
            }
            

            get责任定性();


        }
        #endregion

        #region 8.责任定性
        void get责任定性()
        {

            cmb责任定性.Items.Clear();

            cmb责任定性.Items.Add("责任定性");

            allXuanDingXing = new AllXuan();
            allXuanDingXing.cbkAll.Content = "全选";
            allXuanDingXing.cbkAll.Tag = "DingXing";
            cmb责任定性.Items.Add(allXuanDingXing);
            if (Common.Proxy.DingXingList != null)
            {
                for (int i = 0; i < Common.Proxy.DingXingList.Count; i++)
                {
                    ComboBoxItem comboxitem = new ComboBoxItem();
                    comboxitem.Content = Common.Proxy.DingXingList[i];
                    comboxitem.Tag = "DingXing";
                    comboxitem.Style = this.Resources["ComboBoxItemStyle1"] as Style;
                    cmb责任定性.Items.Add(comboxitem);
                }

                cmb责任定性.SelectedIndex = 0;
            }
         


        }

        #endregion
        #endregion

        #region 四 获取数据

        /// <summary>
        /// 下拉项选中
        /// </summary>
        /// <returns></returns>
        Dictionary<string, string> ShuJu()
        {
            diccaml.Clear();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            //获取日期时间值
            if (checkBox1.IsChecked == false && checkBox1.IsEnabled == true)
            {
                if (startEndTime1.DPStartData.Visibility == Visibility.Visible && !String.IsNullOrWhiteSpace(startEndTime1.DPStartData.Text))
                {
                    diccaml.Add("startData", startEndTime1.DPStartData.Text.Replace(".", "-").Replace("/", "-").Replace("年", "-").Replace("月", "-").Replace("日", "").ToString() + "T" + startEndTime1.CMStartTime.SelectionBoxItem + ":00:00Z" + ",#DateTime#Geq");

                }
                else
                {
                    diccaml.Add("startData", DateTime.Now.Year.ToString() + "-" + DateTime.Now.AddMonths(-1).Month.ToString() + "-1" + "T" + startEndTime1.CMStartTime.SelectionBoxItem + ":00:00Z" + ",#DateTime#Geq");
                }

                if (startEndTime1.DPEndData.Visibility == Visibility.Visible && !String.IsNullOrWhiteSpace(startEndTime1.DPEndData.Text))
                {
                    diccaml.Add("EndData", startEndTime1.DPEndData.Text.Replace(".", "-").Replace(".", "-").Replace("/", "-").Replace("年", "-").Replace("月", "-").ToString() + "T" + startEndTime1.CMEndTime.SelectionBoxItem + ":00:00Z" + ",#DateTime#Leq");
                }
                else
                {
                    diccaml.Add("EndData", DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + "T" + startEndTime1.CMStartTime.SelectionBoxItem + ":00:00Z" + ",#DateTime#Leq");
                }
            }
            else if (checkBox1.IsChecked == true && checkBox1.IsEnabled == true)
            {
                if (cmb年.Visibility == Visibility.Visible && cmb年.SelectionBoxItem != null && cmb年.SelectedItem.ToString() != "全部")
                {
                    if (cmb月.Visibility == Visibility.Visible && cmb月.SelectionBoxItem != null && cmb月.SelectedItem.ToString() != "全部")
                    {
                        string year = cmb年.SelectionBoxItem.ToString().Replace("年", "").Trim();
                        string month = cmb月.SelectionBoxItem.ToString().Replace("月", "").Trim();
                        int daymumber = DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month));

                        if (cmb日.Visibility == Visibility.Visible && cmb日.SelectionBoxItem != null && cmb日.SelectedItem.ToString() != "全部")
                        {
                            diccaml.Add("startData", cmb年.SelectedItem.ToString().Replace("年", "").Replace(".", "-").Replace("/", "-").Replace("年", "-").Replace("月", "-").Trim() + "-" + cmb月.SelectedItem.ToString().Replace("月", "").Trim() + "-" + cmb日.SelectedItem.ToString().Replace("日", "").Trim() + "T00:00:00Z" + ",#DateTime#Geq");
                            diccaml.Add("EndData", cmb年.SelectedItem.ToString().Replace("年", "").Replace(".", "-").Replace("/", "-").Replace("年", "-").Replace("月", "-").Trim() + "-" + cmb月.SelectedItem.ToString().Replace("月", "").Trim() + "-" + cmb日.SelectedItem.ToString().Replace("日", "").Trim() + "T23:59:59Z" + ",#DateTime#Leq");
                        }
                        else
                        {
                            diccaml.Add("startData", cmb年.SelectedItem.ToString().Replace("年", "").Replace(".", "-").Replace("/", "-").Replace("年", "-").Replace("月", "-").Trim() + "-" + cmb月.SelectedItem.ToString().Replace("月", "").Trim() + "-1" + "T00:00:00Z" + ",#DateTime#Geq");
                            diccaml.Add("EndData", cmb年.SelectedItem.ToString().Replace("年", "").Replace(".", "-").Replace("/", "-").Replace("年", "-").Replace("月", "-").Trim() + "-" + cmb月.SelectedItem.ToString().Replace("月", "").Trim() + "-" + daymumber.ToString() + "T23:59:59Z" + ",#DateTime#Leq");
                        }
                    }
                    else
                    {
                        diccaml.Add("startData", cmb年.SelectedItem.ToString().Replace("年", "").Replace(".", "-").Replace("/", "-").Replace("年", "-").Replace("月", "-").Trim() + "-1-1" + "T00:00:00Z" + ",#DateTime#Geq");
                        diccaml.Add("EndData", cmb年.SelectedItem.ToString().Replace("年", "").Replace(".", "-").Replace("/", "-").Replace("年", "-").Replace("月", "-").Trim() + "-12-31" + "T23:59:59Z" + ",#DateTime#Leq");
                    }
                }

            }
            //checkbox不可用的时候   年月查询
            else if (checkBox1.IsEnabled == false && cmb年1.Visibility == Visibility.Visible && cmb月1.Visibility == Visibility.Visible)
            {
                int Month1 = Convert.ToInt32(cmb月1.SelectedItem.ToString().Replace("月", "").Trim());
                string year = "";

                if (Month1 == 1)
                {
                    year = cmb年1.SelectedItem.ToString().Replace("年", "").Trim();
                    diccaml.Add("startData", Convert.ToInt32(year) - 1 + "-12-1,#DateTime#Geq");
                    diccaml.Add("EndData", year + "-1-31,#DateTime#Leq");
                }
                else
                {
                    year = cmb年1.SelectedItem.ToString().Replace("年", "").Trim();
                    int fristMonth = Month1 - 1;
                 
                 
                    SLtxtHelp slt = new SLtxtHelp();
                    string date = year + "-" + Month1 + "-1";
                    string enddate = slt.GetEndDay(date);

                    diccaml.Add("startData", year + "-" + fristMonth + "-1,#DateTime#Geq");
                    diccaml.Add("EndData", enddate + ",#DateTime#Leq");
                }
            }
            //同期数据查询  年年 
            else if (checkBox1.IsEnabled == false && cmb年1.Visibility == Visibility.Visible && cmb年2.Visibility == Visibility.Visible)
            {
                string strYear1 = cmb年1.SelectedItem.ToString().Replace("年", "").Replace(".", "-").Replace("/", "-").Replace("年", "-").Replace("月", "-").Trim();
                string strYear2 = cmb年2.SelectedItem.ToString().Replace("年", "").Replace(".", "-").Replace("/", "-").Replace("年", "-").Replace("月", "-").Trim();
                diccaml.Add("startData", strYear1 + "-1-1,#DateTime#Geq");
                diccaml.Add("EndData", strYear2 + "-12-31,#DateTime#Leq");
            }

            //段定
            if (cmb段定.Visibility == Visibility.Visible && cmb段定.SelectionBoxItem != null && !cmb段定.SelectionBoxItem.ToString().Contains("段定"))
            {
                diccaml.Add("DuanDing", cmb段定.SelectionBoxItem.ToString().Trim() + ",#Text#Eq");


            }
            //局定
            if (cmb局定.Visibility == Visibility.Visible && cmb局定.SelectionBoxItem != null && !cmb局定.SelectionBoxItem.ToString().Contains("局定"))
            {
                diccaml.Add("JuDing", cmb局定.SelectionBoxItem.ToString().Trim() + ",#Text#Eq");

            }
            //获取延时
            try
            {

                if (cmb延时.Visibility == Visibility.Visible && cmb延时.SelectionBoxItem != null && cmb延时.SelectionBoxItem.ToString() != "选择延时" && !cmb延时.SelectionBoxItem.ToString().Contains("大于"))
                {
                    diccaml.Add("YanShi", cmb延时.SelectionBoxItem.ToString().Replace("分钟", "").Trim().Split(new char[] { '-' })[0] + ",#Number#Geq");
                    diccaml.Add("YanShi_", cmb延时.SelectionBoxItem.ToString().Replace("分钟", "").Trim().Split(new char[] { '-' })[1] + ",#Number#Leq");

                }

                if (cmb延时.SelectionBoxItem.ToString().Contains("大于"))       
                {
                    diccaml.Add("YanShi", cmb延时.SelectionBoxItem.ToString().Replace("分钟", "").Trim().Split(new char[] { '于' })[1] + ",#Number#Geq");
                    diccaml.Add("YanShi_", "100000,#Number#Leq");

                }

            }
            catch (Exception)
            {

            }
            ////故障类别
            //if (GZ类别.Visibility == Visibility.Visible && GZ类别.SelectionBoxItem != null && !GZ类别.SelectionBoxItem.ToString().Contains("故障类型"))
            //{

            //    diccaml.Add("ShuJuLeiXing", GZ类别.SelectionBoxItem.ToString() + ",#Text#Eq");

            //}

            // guzhangType = GZ类别.SelectionBoxItem.ToString();


            //车站
            if (txt车站.Visibility == Visibility.Visible && txt车站.Text != null && !txt车站.Text.Contains("车站"))
            {
                diccaml.Add("Station", txt车站.Text.Trim() + ",#Text#Eq");
            }
            //区间
            if (txt区间.Visibility == Visibility.Visible && txt区间.Text != null && !txt区间.Text.Contains("区间"))
            {
                diccaml.Add("QuJian", txt区间.Text.Trim() + ",#Text#Eq");
            }


            //设备名称
            if (txt设备名称.Visibility == Visibility.Visible && txt设备名称.Text != null && !txt设备名称.Text.ToString().Contains("设备名称"))
            {
                diccaml.Add("GuZhangSheBeiMingCheng", txt设备名称.Text.Trim() + ",#Text#Eq");

            }

            //报局
            if (cmb是否报局.Visibility == Visibility.Visible && cmb是否报局.SelectionBoxItem != null && !cmb是否报局.SelectionBoxItem.ToString().Contains("是否报局"))
            {
                diccaml.Add("ShiFouBaoJu", cmb是否报局.SelectionBoxItem.ToString().Trim() + ",#Text#Eq");
            }
            //受理
            if (cmb受理.Visibility == Visibility.Visible && cmb受理.SelectionBoxItem != null && !cmb受理.SelectionBoxItem.ToString().Contains("是否受理"))
            {
                diccaml.Add("YNshouLi", cmb受理.SelectionBoxItem.ToString().Trim() + ",#Text#Eq");
            }

            //雷害
            if (ckbLeiHai.Visibility == Visibility.Visible && ckbLeiHai.IsChecked == true && ckbLeiHai.IsEnabled == true)
            {
                diccaml.Add("LeiHai", "是" + ",#Text#Eq");

            }

            //经济损失
            try
            {
                if (cmb直接损失.Visibility == Visibility.Visible && cmb直接损失.SelectionBoxItem != null && cmb直接损失.SelectionBoxItem.ToString() != "直接经济损失" && !cmb直接损失.SelectionBoxItem.ToString().Contains("大于"))
                {
                    diccaml.Add("JingJiSunShi", cmb直接损失.SelectionBoxItem.ToString().Split(new char[] { '-' })[0] + ",#Text#Geq");
                    diccaml.Add("_JingJiSunShi", cmb直接损失.SelectionBoxItem.ToString().Split(new char[] { '-' })[1] + ",#Text#Leq");

                }

                if (cmb间接损失.Visibility == Visibility.Visible && cmb间接损失.SelectionBoxItem != null && cmb间接损失.SelectionBoxItem.ToString() != "间接经济损失" && !cmb间接损失.SelectionBoxItem.ToString().Contains("大于"))
                {
                    diccaml.Add("ZeRenDanWeiChengDanSunShiFeiYong", cmb间接损失.SelectionBoxItem.ToString().Split(new char[] { '-' })[0] + ",#Text#Geq");
                    diccaml.Add("_ZeRenDanWeiChengDanSunShiFeiYong", cmb间接损失.SelectionBoxItem.ToString().Split(new char[] { '-' })[1] + ",#Text#Leq");

                }


                if (cmb直接损失.SelectionBoxItem.ToString().Contains("大于"))
                {
                    diccaml.Add("JingJiSunShi", cmb直接损失.SelectionBoxItem.ToString().Split(new char[] { '于' })[1] + ",#Text#Geq");
                    diccaml.Add("_JingJiSunShi", "100000,#Text#Leq");

                }

                if (cmb间接损失.SelectionBoxItem.ToString().Contains("大于"))
                {
                    diccaml.Add("ZeRenDanWeiChengDanSunShiFeiYong", cmb间接损失.SelectionBoxItem.ToString().Split(new char[] { '于' })[1] + ",#Text#Geq");
                    diccaml.Add("_ZeRenDanWeiChengDanSunShiFeiYong", "100000,#Text#Leq");

                }
            }
            catch (Exception)
            {

            }

            return diccaml;
        }

        #endregion

        #region 四、事件区域
        #region 事件
        /// <summary>
        /// 返回总数据
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> ZongShuJu()
        {
            camlZong.Clear();

            camlZong = ShuJu();
            foreach (var item in caml)
            {               
                camlZong.Add(item.Key, item.Value);
            }
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
            else if (!camlZong.Keys.Contains("SheBeiTypeErJi") && camlZong.Keys.Contains("LeiHai"))
            {
                camlZong.Remove("LeiHai");
                camlZong.Add("SheBeiTypeErJi", "雷害,#Text#Eq");
            }
            else if (camlZong.Keys.Contains("LeiHai"))
            {
                camlZong.Remove("LeiHai");
            }
            return camlZong;
        }
        /// <summary>
        /// 雷害点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ckbLeiHai_Click(object sender, RoutedEventArgs e)
        {
            CheckShuJu();
        }
        /// <summary>
        /// 详细时间点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox1_Click(object sender, RoutedEventArgs e)
        {
            if (checkBox1.IsChecked == true)
            {
                this.cmb月.Visibility = Visibility.Visible;
                this.cmb年.Visibility = Visibility.Visible;
                this.cmb日.Visibility = Visibility.Visible;
                this.startEndTime1.DPStartData.Text = null;
                this.startEndTime1.DPEndData.Text = null;
                this.startEndTime1.Visibility = Visibility.Collapsed;

            }
            else
            {
                get年月日();
                this.cmb月.Visibility = Visibility.Collapsed;
                this.cmb年.Visibility = Visibility.Collapsed;
                this.cmb日.Visibility = Visibility.Collapsed;
                this.startEndTime1.Visibility = Visibility.Visible;
            }
        }
        /// <summary>
        /// 下拉多选菜单的checkbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Click(object sender, RoutedEventArgs e)
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
                        #region 特殊处理
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
                    else if (selectCOB.Tag.ToString() == "ShebeiTypeYiJi" && caml.Keys.Contains("SheBeiTypeErJi"))
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
                try
                {
                    //如果值空了  移除键
                    if (caml[selectCOB.Tag.ToString()] == "#Text#Eq" || caml[selectCOB.Tag.ToString()] == "#Text#Contains")
                    {
                        try
                        {
                            caml.Remove(selectCOB.Tag.ToString());
                            #region 没有任何子项的情况下,全选选中
                            AddAllCheckTrue(selectCOB);
                            #endregion
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            CheckShuJu();
        }
        void AddAllCheckTrue(CheckBox selectCOB) {
            try
            {
                if (selectCOB.Tag.ToString() == "Line")
                {
                    allXuanXianBie.cbkAll.IsChecked = true;
                    allXuanXianBie.cbkAll.Click += new RoutedEventHandler(cbkAll_Click);
                }
                else if (selectCOB.Tag.ToString() == "CheJianMingCheng")
                {
                    allXuanCheJian.cbkAll.IsChecked = true;
                    allXuanCheJian.cbkAll.Click += new RoutedEventHandler(cbkAll_Click);

                }
                else if (selectCOB.Tag.ToString() == "GongQuMingCheng")
                {
                    allXuanGongQu.cbkAll.Click += new RoutedEventHandler(cbkAll_Click);
                    allXuanGongQu.cbkAll.IsChecked = true;

                }

                else if (selectCOB.Tag.ToString() == "SheBeiTypeErJi")
                {
                    allXuanSheBeiTypeErJi.cbkAll.IsChecked = true;
                    allXuanSheBeiTypeErJi.cbkAll.Click += new RoutedEventHandler(cbkAll_Click);
                  

                }
                else if (selectCOB.Tag.ToString() == "DingZe")
                {
                    allXuanDingZe.cbkAll.Click += new RoutedEventHandler(cbkAll_Click);
                    allXuanDingZe.cbkAll.IsChecked = true;

                }
                else if (selectCOB.Tag.ToString() == "DingXing")
                {
                    allXuanDingXing.cbkAll.Click += new RoutedEventHandler(cbkAll_Click);
                    allXuanDingXing.cbkAll.IsChecked = true;

                }
                else if (selectCOB.Tag.ToString() == "ShebeiTypeYiJi")
                {
                    allXuanSheBeiTypeYiJi.cbkAll.Click += new RoutedEventHandler(cbkAll_Click);
                    allXuanSheBeiTypeYiJi.cbkAll.IsChecked = true;

                }
                else if (selectCOB.Tag.ToString() == "ShuJuLeiXing")
                {
                    allGZtype.cbkAll.Click += new RoutedEventHandler(cbkAll_Click);
                    allGZtype.cbkAll.IsChecked = true;
                }
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 判断全选的选中状态
        /// </summary>
        /// <param name="selectCOB"></param>
        void AllCheckStateFlase(CheckBox selectCOB) {
            try
            {
                if (selectCOB.Tag.ToString() == "Line")
                {
                    allXuanXianBie.cbkAll.IsChecked = false;
                    allXuanXianBie.cbkAll.Click += new RoutedEventHandler(cbkAll_Click);
                }
                else if (selectCOB.Tag.ToString() == "CheJianMingCheng")
                {
                    allXuanCheJian.cbkAll.IsChecked = false;
                    allXuanCheJian.cbkAll.Click += new RoutedEventHandler(cbkAll_Click);
                }
                else if (selectCOB.Tag.ToString() == "GongQuMingCheng")
                {
                    allXuanGongQu.cbkAll.IsChecked = false;
                    allXuanGongQu.cbkAll.Click += new RoutedEventHandler(cbkAll_Click);
                }
                else if (selectCOB.Tag.ToString() == "SheBeiTypeErJi")
                {
                    allXuanSheBeiTypeErJi.cbkAll.IsChecked = false;
                    allXuanSheBeiTypeErJi.cbkAll.Click += new RoutedEventHandler(cbkAll_Click);

                }
                else if (selectCOB.Tag.ToString() == "DingZe")
                {
                    allXuanDingZe.cbkAll.IsChecked = false;
                    allXuanDingZe.cbkAll.Click += new RoutedEventHandler(cbkAll_Click);
                }
                else if (selectCOB.Tag.ToString() == "DingXing")
                {
                    allXuanDingXing.cbkAll.IsChecked = false;
                    allXuanDingXing.cbkAll.Click += new RoutedEventHandler(cbkAll_Click);
                }
                else if (selectCOB.Tag.ToString() == "ShebeiTypeYiJi")
                {
                    allXuanSheBeiTypeYiJi.cbkAll.IsChecked = false;
                    allXuanSheBeiTypeYiJi.cbkAll.Click += new RoutedEventHandler(cbkAll_Click);
                }
                else if (selectCOB.Tag.ToString() == "ShuJuLeiXing")
                {
                    allGZtype.cbkAll.IsChecked = false;
                    allGZtype.cbkAll.Click += new RoutedEventHandler(cbkAll_Click);
                }
              
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 集合添加CheckBox
        /// </summary>
        /// <param name="checkBox"></param>
        void ListAddCheckBox(CheckBox checkBox)
        {
            if(!checkList.Contains(checkBox))
            {
                checkList.Add(checkBox);
            }
        }
        /// <summary>
        /// 集合删除checkbox
        /// </summary>
        /// <param name="checkBox"></param>
        void ListRemoveCheckBox(CheckBox checkBox)
        {
            if (checkList.Contains(checkBox))
            {
                checkList.Remove(checkBox);
            }

        }
        /// <summary>
        /// 清空选中项的选中状态
        /// </summary>
        /// <param name="strBiaoShi"></param>
        void ListClearCheckBox(string strBiaoShi)
        {
            foreach (var item in checkList)
            {
                if (item.Tag.ToString().Equals(strBiaoShi))
                {
                    item.IsChecked = false;
                   
                }
            }
        }
        /// <summary>
        /// 清空选中项的选中状态特殊的
        /// </summary>
        /// <param name="strBiaoShi"></param>
        void ListClearCheckBoxT(string strBiaoShi,string content)
        {
            try
            {
                foreach (var item in checkList)
                {
                    if (item.Content.ToString()==content)
                    {
                        if (item.Content.ToString() == "传输设备" || item.Content.ToString() == "雷害" || item.Content.ToString() == "其它")
                        {
                            item.IsChecked = false;
                        }
                    }
                }
            }
            catch (Exception)
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
                if (selectCOB1.Tag.ToString() == "Line")
                {
                    caml.Remove("Line");
                }
                else if (selectCOB1.Tag.ToString() == "CheJianMingCheng")
                {
                    caml.Remove("CheJianMingCheng");

                }
                else if (selectCOB1.Tag.ToString() == "GongQuMingCheng")
                {
                    caml.Remove("GongQuMingCheng");
                }
                else if (selectCOB1.Tag.ToString() == "ShebeiTypeYiJi")
                {
                    caml.Remove("ShebeiTypeYiJi");
                }
                else if (selectCOB1.Tag.ToString() == "SheBeiTypeErJi")
                {
                    caml.Remove("SheBeiTypeErJi");
                }
                else if (selectCOB1.Tag.ToString() == "DingZe")
                {
                    caml.Remove("DingZe");
                }
                else if (selectCOB1.Tag.ToString() == "DingXing")
                {
                    caml.Remove("DingXing");
                }
                else if (selectCOB1.Tag.ToString() == "ShuJuLeiXing")
                {
                    caml.Remove("ShuJuLeiXing");
                }

                ListClearCheckBox(selectCOB1.Tag.ToString());
                CheckShuJu();
            }
            catch (Exception)
            {
            }
        } 
        /// <summary>
        /// 全选事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cbkAll_Click(object sender, RoutedEventArgs e)
        {
            CheckBox selectCOB1 = sender as CheckBox;
            ClearLiatCheckAddRemoveCaml(selectCOB1);
        }
        #endregion
        #region 将字典目录的数据放到txt中
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
                txtXinxi.Text = "";
                if (camlZ.Keys.Contains("startData") && camlZ.Keys.Contains("EndData"))
                {
                    camlZ.Remove("startData");
                    camlZ.Remove("EndData");

                    if (camlZ.Keys.Contains("YanShi") && camlZ.Keys.Contains("YanShi_"))
                    {
                        camlZ.Remove("YanShi");
                        camlZ.Remove("YanShi_");
                        camlZ.Add("延时", cmb延时.SelectionBoxItem.ToString().Trim() + ",");

                    }
                    if (camlZ.Keys.Contains("JingJiSunShi") && camlZ.Keys.Contains("_JingJiSunShi"))
                    {
                        camlZ.Remove("JingJiSunShi");
                        camlZ.Remove("_JingJiSunShi");
                        camlZ.Add("直接损失", cmb直接损失.SelectionBoxItem.ToString() + ",");
                    }
                    if (camlZ.Keys.Contains("ZeRenDanWeiChengDanSunShiFeiYong") && camlZ.Keys.Contains("_ZeRenDanWeiChengDanSunShiFeiYong"))
                    {
                        camlZ.Remove("ZeRenDanWeiChengDanSunShiFeiYong");
                        camlZ.Remove("_ZeRenDanWeiChengDanSunShiFeiYong");
                        camlZ.Add("间接损失", cmb间接损失.SelectionBoxItem.ToString().Trim() + ",");
                    }
                    foreach (var item in camlZ)
                    {
                        txtXinxi.Text += item.Key
                            .Replace("Line", "线别")
                            .Replace("CheJianMingCheng", "车间")
                            .Replace("GongQuMingCheng", "工区")
                            .Replace("ShebeiTypeYiJi", "设备类型一级")
                            .Replace("SheBeiTypeErJi", "设备类型二级")
                            .Replace("DingZe", "责任定责")
                            .Replace("DingXing", "责任定性")
                            .Replace("ShuJuLeiXing", "故障类型")
                            .Replace("Station", "车站")
                            .Replace("QuJian", "区间")
                            .Replace("GuZhangSheBeiMingCheng", "设备名称")
                            .Replace("ShiFouBaoJu", "报局")
                            .Replace("YNshouLi", "受理")
                            .Replace("LeiHai", "雷害")
                            .Replace("DuanDing", "段定")
                            .Replace("JuDing", "局定")
                          .Replace("延时", "延时")
                            + ":" + item.Value.Remove(item.Value.LastIndexOf(","))
                            + "；";
                        if (txtXinxi.Text != "")
                        {
                            borTxt.Visibility = Visibility.Visible;
                            borT.Visibility = Visibility.Visible;

                            txtXinxi.Text = txtXinxi.Text;
                        }

                    }
                    if (txtXinxi.Text == "")
                    {

                        borTxt.Visibility = Visibility.Collapsed;
                        borT.Visibility = Visibility.Collapsed;

                    }
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion
        /// <summary>
        /// 设备类型一级关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbSheBeiTypeYiJi_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                if (caml.Keys.Contains("ShebeiTypeYiJi") && !caml.ContainsKey("SheBeiTypeErJi"))
                {
                    List<string> selecterjishebei临时 = new List<string>();
                    selecterjishebei.Clear();

                    if (caml.Keys.Contains("ShebeiTypeYiJi") && caml["ShebeiTypeYiJi"] != "")
                    {
                        for (int i = 0; i < caml["ShebeiTypeYiJi"].Split(new char[] { ',' }).Length; i++)
                        {
                            if (caml["ShebeiTypeYiJi"].Split(new char[] { ',' })[i] != "")
                            {
                                selecterjishebei临时.Clear();
                                selecterjishebei临时 = allerjishebei.Where(a => a.f父级 == caml["ShebeiTypeYiJi"].Split(new char[] { ',' })[i]).ToList();
                                selecterjishebei.AddRange(selecterjishebei临时);

                            }
                        }
                    }
                    if (selecterjishebei.Count >= 1)
                    {
                        cmbSheBeiTypeErJi.Items.Clear();
                        cmbSheBeiTypeErJi.Items.Add("设备类型二级");
                        allXuanSheBeiTypeErJi = new AllXuan();
                        allXuanSheBeiTypeErJi.cbkAll.Content = "全选";
                        allXuanSheBeiTypeErJi.cbkAll.Tag = "SheBeiTypeErJi";
                        cmbSheBeiTypeErJi.Items.Add(allXuanSheBeiTypeErJi);
                        for (int i = 0; i < selecterjishebei.Count; i++)
                        {
                            ComboBoxItem comboxitem = new ComboBoxItem();
                            comboxitem.Content = selecterjishebei[i];
                            comboxitem.Tag = "SheBeiTypeErJi";
                            comboxitem.Style = this.Resources["ComboBoxItemStyle1"] as Style;
                            cmbSheBeiTypeErJi.Items.Add(comboxitem);
                        }
                        cmbSheBeiTypeErJi.SelectedIndex = 0;
                    }
                }
                else if (caml.Keys.Contains("ShebeiTypeYiJi") && caml["SheBeiTypeErJi"] != "")
                {
                }
                else if (!caml.ContainsKey("ShebeiTypeYiJi") && !caml.ContainsKey("SheBeiTypeErJi"))
                {
                    get设备类型二级();
                }
            }
            catch (Exception)
            {
            }
             
        }
        /// <summary>
        /// 单独年份选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmb年_DropDownClosed(object sender, EventArgs e)
        {
            //填充月份
            cmb月.Items.Clear();
            cmb月.Items.Add("全部");
            for (int i = 0; i < 12; i++)
            {
                cmb月.Items.Add(String.Format("{0}月", (i + 1).ToString()));
            }
            cmb月.SelectedIndex = 0;

            cmb日.Items.Clear();
            cmb日.Items.Add("全部");
            cmb日.SelectedIndex = 0;
        }   
        /// <summary>
        /// 同期数据对比 年份2的关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmb年2_DropDownClosed(object sender, EventArgs e)
        {
            string year1 = cmb年1.SelectionBoxItem.ToString().Replace("年", "").Trim();
            string year2 = cmb年2.SelectionBoxItem.ToString().Replace("年", "").Trim();
            if (Convert.ToInt32(year1) > Convert.ToInt32(year2))
            {
                cmb年2.SelectedIndex = 0;
                MessageShow messageshow = new MessageShow("结束年份需大于开始年份！");
                messageshow.CancelButton.Visibility = Visibility.Collapsed;
                messageshow.Show();
            }
        }
        /// <summary>
        /// 单独月份选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmb月_DropDownClosed(object sender, EventArgs e)
        {
            if (cmb年.SelectedItem.ToString() != "全部")
            {
                cmb日.Items.Clear();
                cmb日.Items.Add("全部");
                string year = cmb年.SelectionBoxItem.ToString().Replace("年", "").Trim();
                string month = ((ComboBox)sender).SelectionBoxItem.ToString().Replace("月", "").Trim();
                if (month != "全部" && year != "全部")
                {
                    int daymumber = DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month));
                    for (int i = 1; i < daymumber + 1; i++)
                    {
                        cmb日.Items.Add(String.Format("{0}日", i.ToString()));
                    }
                }
                cmb日.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("请先选择年份");

                //填充月份
                cmb月.Items.Clear();
                cmb月.Items.Add("全部");
                for (int i = 0; i < 12; i++)
                {
                    cmb月.Items.Add(String.Format("{0}月", (i + 1).ToString()));
                }
                cmb月.SelectedIndex = 0;

                cmb日.Items.Clear();
                cmb日.Items.Add("全部");
                cmb日.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// 故障类别选择时间4
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GZ类别_DropDownClosed(object sender, EventArgs e)
        {
            string typeGZ = ((ComboBox)sender).SelectedValue.ToString();

            if (cmb年.Visibility == Visibility.Visible && cmb年.SelectedItem.ToString() == "全部")
            {
                MessageBox.Show("请选择年份");
                return;
            }
        }
        #endregion
        #endregion

        #region 五、辅助方法
        private void txt车站_MouseEnter(object sender, MouseEventArgs e)
        {
            if (txt车站.Text == "车站")
            {
                txt车站.Text = "";
            }
        }
        private void txt车站_MouseLeave(object sender, MouseEventArgs e)
        {
            if (txt车站.Text == "")
            {
                txt车站.Text = "车站";
            }
        }
        private void txt区间_MouseEnter(object sender, MouseEventArgs e)
        {
            if (txt区间.Text == "区间")
            {
                txt区间.Text = "";
            }
        }
        private void txt区间_MouseLeave(object sender, MouseEventArgs e)
        {
            if (txt区间.Text == "")
            {
                txt区间.Text = "区间";
            }
        }

        private void txt设备名称_MouseEnter(object sender, MouseEventArgs e)
        {
            if (txt设备名称.Text == "设备名称")
            {
                txt设备名称.Text = "";
            }
        }
        private void txt设备名称_MouseLeave(object sender, MouseEventArgs e)
        {
            if (txt设备名称.Text == "")
            {
                txt设备名称.Text = "设备名称";
            }
        }
        private void cmb是否报局_DropDownClosed(object sender, EventArgs e)
        {
            txtXinxi.Text = "";
            CheckShuJu();

        }

        #endregion

      
    }
}

