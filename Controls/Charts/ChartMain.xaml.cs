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
using System.Threading;
using MhczTBG.Controls.Print;
using TongXin.UIComman;
using MhczTBG.StyleResource;
using Visifire.Charts;
using System.Data;

namespace MhczTBG.Controls.Charts
{
    /// <summary>
    /// ChartMode1.xaml 的交互逻辑
    /// </summary>
    public partial class ChartMain : UserControl
    {
        #region 声明变量

        /// <summary>
        /// 临时存储标题
        /// </summary>
        Dictionary<string, string> _titlelist = new Dictionary<string, string>();

        /// <summary>
        /// 用于填充列表使用
        /// </summary>
        List<FHFormLb> _fhFormList = new List<FHFormLb>();

        /// <summary>
        /// 用于打印，导入Excel
        /// </summary>
        List<FHFormLb> _fhFormListSave = new List<FHFormLb>();

        //配置文件存放位置
        protected IniFile _IniConfig = new IniFile("C:/Config/iniFile.ini");

        /// <summary>
        /// 统计数据是否是已完成
        /// </summary>
        public bool _isComplete = false;

        /// <summary>
        /// 附件图片路径
        /// </summary>
        string _hasFuJianImageUri = string.Empty;

        /// <summary>
        /// 无附件图片路径
        /// </summary>
        string _noFuJIanImageUri = string.Empty;

        /// <summary>
        /// excel应用程序
        /// </summary>
        Microsoft.Office.Interop.Excel.Application _excelother = null;

        /// <summary>
        /// 顶层元数据
        /// </summary>
        List<Dictionary<string, object>> _dicDataList = null;

        /// <summary>
        /// 数据筛选集
        /// </summary>
        List<Dictionary<string, object>> _dicDataListFor2 = new List<Dictionary<string, object>>();

        /// <summary>
        /// 左侧区域内部名称
        /// </summary>
        string _propertyName1 = string.Empty;

        /// <summary>
        /// 左侧区域字段集
        /// </summary>
        List<string> _proList1 = null;

        /// <summary>
        /// 右侧区域内部名称
        /// </summary>
        string _propertyName2 = string.Empty;

        /// <summary>
        /// 右侧区域字段集
        /// </summary>
        List<string> _proList2 = null;

        /// <summary>
        /// 右侧图表类型
        /// </summary>
        RenderAs _renderType1 = default(RenderAs);

        /// <summary>
        /// 右侧图表类型
        /// </summary>
        RenderAs _renderType2 = default(RenderAs);

        /// <summary>
        /// 是否继续更新列表
        /// </summary>
        bool _isLeadingAgain = true;

        /// <summary>
        /// 局部模式
        /// </summary>
        Modern1Type _slection = Modern1Type.Modern1Type1;

        /// <summary>
        /// 横标题
        /// </summary>
        string _hengTittle = string.Empty;

        /// <summary>
        /// 竖标题
        /// </summary>
        string _shuTittle = string.Empty;

        /// <summary>
        /// 图形
        /// </summary>
        RenderAs TongQiChartType = RenderAs.Column;

        /// <summary>
        /// 图形
        /// </summary>
        RenderAs HuanBiChartType = RenderAs.Column;

        #endregion

        #region 自定义委托事件

        public delegate void SettingFuJiJianImageUriEventHandle(ref string HasFuJianUri, ref string NoFuJianUri);
        /// <summary>
        /// 附件图片
        /// </summary>
        public event SettingFuJiJianImageUriEventHandle _SettingFuJiJianImageUriEvent = null;

        public delegate void RangeRuleSettingEventHandle(string strInformation, ref int range1, ref int range2);
        /// <summary>
        /// 设置int值范围规则
        /// </summary>
        public event RangeRuleSettingEventHandle _RangeRuleSettingEvent = null;

        public delegate void ChildernSameEventHandle(string childernName, ref string parentName, ref Dictionary<string, List<string>> parentList);
        /// <summary>
        /// 子项名称一致，设置
        /// </summary>
        public event ChildernSameEventHandle _ChildernSameEvent = null;

        public delegate void OtherConditionEventHandle(string Tittle, ref string conPropertyName, ref string conPropertyValue);
        /// <summary>
        /// 设置附加条件
        /// </summary>
        public event OtherConditionEventHandle _OtherConditionEvent = null;

        #endregion

        #region 构造函数

        public ChartMain()
        {
            try
            {
                InitializeComponent();

                #region 注册事件

                this.comS.btnShiTu.Click += new RoutedEventHandler(btnShiTu_Click);
                this.comS.btnSearch.Click += new RoutedEventHandler(btnSearch_Click);
                this.comS.btf3.Click += new RoutedEventHandler(btf3_Click);
                this.comS.btExcel.Click += new RoutedEventHandler(btExcel_Click);

                this.comS.btnSearchTongQi.Click += new RoutedEventHandler(btnSearchTongQi_Click);

                this.comS.btnSearchHuanBi.Click += new RoutedEventHandler(btnSearchHuanBi_Click);

                this.chratModern3.com.SelectionChanged += new SelectionChangedEventHandler(com_SelectionChanged);

                this.chratModern4.com.SelectionChanged += new SelectionChangedEventHandler(com_SelectionChanged2);

                #endregion

                ParaMersInit();

                TitleInit(_titlelist.Values.ToList());

                DataInit();

                System.Windows.Application.Current.Exit += new ExitEventHandler(Current_Exit);

            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ChartMode1", ex.ToString());
            }
        }

        #endregion

        #region 初始化

        void ParaMersInit()
        {
            try
            {
                //读取配置文件
                string FuHeChaXunView = UIHelper.frombase(_IniConfig.IniReadValue(Proxy.UserName, "FuHeChaXunView").ToString());
                if (FuHeChaXunView != "")
                {
                    foreach (string item in FuHeChaXunView.Split(new char[] { ';' }))
                    {
                        //将配置文件中的字段添加到标题字段字典
                        this._titlelist.Add(item.Split(new char[] { ',' })[0], item.Split(new char[] { ',' })[1]);
                    }
                }
                else
                {
                    this._titlelist.Add("ID", "ID");
                    this._titlelist.Add("附件", "附件");
                    this._titlelist.Add("startData", "故障发生日期时间");
                    this._titlelist.Add("Line", "线别");
                    this._titlelist.Add("GuZhangSheBeiMingCheng", "障碍设备名称");
                    this._titlelist.Add("GongQuMingCheng", "工区");
                    this._titlelist.Add("CheJianMingCheng", "责任单位");
                    this._titlelist.Add("ZhangAiXianXiang", "障碍现象");
                    this._titlelist.Add("YingXiangFanWei", "影响范围");
                }

            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ParaMersInit", ex.ToString());
            }
        }

        #endregion

        #region UI事件区域

        #region 搜索事件

        void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.comS.expander.IsExpanded = false;

                this._isLeadingAgain = false;

                this.DataInit();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btnSearch_Click", ex.ToString());
            }
        }

        #endregion

        #region 环比搜索

        void btnSearchHuanBi_Click(object sender, RoutedEventArgs e)
        {
            ChartModern3HuanBiCreate(_hengTittle, _shuTittle);
        }

        #endregion

        #region 同期搜索

        /// <summary>
        /// 同期搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSearchTongQi_Click(object sender, RoutedEventArgs e)
        {
            ChartModern4TongQiCreate();
        }

        #endregion

        #region 自定义视图事件

        void btnShiTu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectColumns selection = new SelectColumns(_titlelist);
                selection.Closed += (object sender1, EventArgs e1) =>
                {
                    SelectColumns quanxian2 = ((SelectColumns)sender1);

                    bool? result = quanxian2.DialogResult;
                    if (result.HasValue && result.Value)
                    {
                        _titlelist.Clear();
                        foreach (var item in quanxian2.VisbleList)
                        {
                            _titlelist.Add(item.Key, item.Value);
                        }

                        TitleInit(_titlelist.Values.ToList());
                    }
                };
                selection.ShowDialog();

            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btnShiTu_Click", ex.ToString());
            }
        }

        #endregion

        /// <summary>
        /// 环比改变视图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void com_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((sender as ComboBox).SelectedItem as ComboBoxItem).Content.ToString() == "折线图")
                HuanBiChartType = RenderAs.Line;
            else
                HuanBiChartType = RenderAs.Column;

            ChartModernHuanBiInit();
        }

        /// <summary>
        /// 同期改变视图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void com_SelectionChanged2(object sender, SelectionChangedEventArgs e)
        {
            if (((sender as ComboBox).SelectedItem as ComboBoxItem).Content.ToString() == "折线图")
                TongQiChartType = RenderAs.Line;
            else
                TongQiChartType = RenderAs.Column;
            ChartModernTongQiInit();
        }

        void Current_Exit(object sender, ExitEventArgs e)
        {
            if (_excelother != null)
                CommonMethod.Kill(_excelother);
        }

        #endregion

        #region 辅助方法

        #region 生成图表

        #region 图表第一种模式

        /// <summary>
        /// 生成饼图
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="proList"></param>
        public void ChartLeftInit(string propertyName, List<string> proList, RenderAs renderType)
        {
            try
            {
                TomDisPatcherLb tomdisPatcher = new TomDisPatcherLb(new System.Action(() =>
                  {
                      this.TittleReset();

                      this.chratModern1.txtList4.Text = _fhFormListSave.Count.ToString();

                      this._slection = Modern1Type.Modern1Type1;

                      this.Modern1Type12Init();

                      #region 生成进行中

                      this._propertyName1 = propertyName;

                      this._proList1 = proList;

                      this._renderType1 = renderType;

                      this.chratModern1.chartLeft.Series.Clear();

                      this._dicDataListFor2.Clear();

                      //创建一个DataSeries
                      DataSeries ds = new DataSeries();
                      ds.RenderAs = renderType;
                      ds.LabelStyle = LabelStyles.OutSide;
                      ds.LabelEnabled = true;

                      List<Dictionary<string, object>> RealDicData = new List<Dictionary<string, object>>();

                      if (this._OtherConditionEvent != null)
                      {
                          string proper = string.Empty;

                          string value = string.Empty;
                          this._OtherConditionEvent(this.chratModern1.txtChartLeft.Text, ref proper, ref   value);
                          if (!string.IsNullOrEmpty(proper) && !string.IsNullOrEmpty(value) && _dicDataList != null)
                          {
                              foreach (var item in _dicDataList)
                              {
                                  if (Convert.ToString(item[proper]).Equals(value))
                                      RealDicData.Add(item);
                              }
                          }
                          else
                              RealDicData = _dicDataList;
                      }
                      else
                          RealDicData = _dicDataList;

                      ds.MouseLeftButtonDown += (object sender, MouseButtonEventArgs e) =>
                      {
                          this._dicDataListFor2 = RealDicData.Where(item => Convert.ToString(item[propertyName]).Contains((sender as DataPoint).AxisXLabel)).ToList<Dictionary<string, object>>();

                          this.ChartRightInit(_propertyName2, _proList2, _renderType2);

                          List<FHFormLb> fhList = new List<FHFormLb>();
                          this._dicDataListFor2.ForEach(Item => fhList.Add(new FHFormLb(Item)));

                          this.DataInitCaseByChart(fhList);

                          this.chratModern1.txtChartRight2.Text = chratModern1.txtList2.Text = (sender as DataPoint).AxisXLabel;
                          this.chratModern1.txtChartRight3.Text = fhList.Count.ToString();

                          this.chratModern1.txtList3.Text = string.Empty;
                      };
                      if (RealDicData != null && RealDicData.Count > 0)
                      {
                          RealDicData.ForEach(item => _dicDataListFor2.Add(item));

                          for (int i = 0; i < proList.Count; i++)
                          {
                              var count = RealDicData.Count(item => Convert.ToString(item[propertyName]).Contains(proList[i]));
                              if (count > 0)
                              {
                                  //创建datapoint
                                  DataPoint dp = new DataPoint();
                                  //设置datapoint名称
                                  dp.AxisXLabel = proList[i];
                                  //设置datapoint的值
                                  dp.YValue = count;
                                  ds.DataPoints.Add(dp);
                              }
                          }
                          this.chratModern1.chartLeft.Series.Add(ds);

                          if (this._isLeadingAgain)
                          {
                              List<FHFormLb> fhList2 = new List<FHFormLb>();
                              RealDicData.ForEach(Item => fhList2.Add(new FHFormLb(Item)));

                              this.DataInitCaseByChart(fhList2);
                          }
                          this._isLeadingAgain = true;
                      }

                      #endregion

                      this.chratModern1.borTip1.Visibility = System.Windows.Visibility.Collapsed;
                  }));

                TomDisPatcherLb tomdisPatcher2 = new TomDisPatcherLb(new System.Action(() =>
                {
                    this.chratModern1.borTip1.Visibility = System.Windows.Visibility.Visible;

                    CommonMethod.ReFresh();

                    tomdisPatcher.Start();
                }));

                tomdisPatcher2.Start();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ChartLeftInit", ex.ToString(), propertyName, proList, renderType);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 生成饼图(内部值为int,计时)
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="proList"></param>
        public void ChartLeftRangeInit(string propertyName, List<string> proList, RenderAs renderType)
        {
            try
            {
                TomDisPatcherLb tomdisPatcher = new TomDisPatcherLb(new System.Action(() =>
                {
                    this.TittleReset();

                    chratModern1.txtList4.Text = _fhFormListSave.Count.ToString();

                    _slection = Modern1Type.Modern1Type2;

                    Modern1Type12Init();

                    #region 生成进行中

                    _propertyName1 = propertyName;

                    _proList1 = proList;

                    _renderType1 = renderType;

                    chratModern1.chartLeft.Series.Clear();

                    _dicDataListFor2.Clear();

                    //创建一个DataSeries
                    DataSeries ds = new DataSeries();
                    ds.RenderAs = renderType;
                    ds.LabelStyle = LabelStyles.OutSide;
                    ds.LabelEnabled = true;

                    ds.MouseLeftButtonDown += (object sender, MouseButtonEventArgs e) =>
                    {
                        _dicDataListFor2.Clear();
                        foreach (var item in _dicDataList)
                        {
                            int cValue = 0;
                            if (int.TryParse(Convert.ToString(item[propertyName]), out cValue))
                            {
                                int rang1 = 0;
                                int rang2 = -1;

                                if (_RangeRuleSettingEvent != null)
                                {
                                    _RangeRuleSettingEvent((sender as DataPoint).AxisXLabel, ref rang1, ref rang2);

                                    if (rang2 != -1 && cValue >= rang1 && cValue <= rang2)
                                        _dicDataListFor2.Add(item);
                                    else if (rang2 == -1 && cValue > rang1)
                                        _dicDataListFor2.Add(item);
                                }
                            }
                        }

                        ChartRightInit(_propertyName2, _proList2, _renderType2);


                        List<FHFormLb> fhList = new List<FHFormLb>();
                        _dicDataListFor2.ForEach(Item => fhList.Add(new FHFormLb(Item)));

                        DataInitCaseByChart(fhList);

                        chratModern1.txtChartRight2.Text = chratModern1.txtList2.Text = (sender as DataPoint).AxisXLabel;
                        chratModern1.txtChartRight3.Text = fhList.Count.ToString();

                        chratModern1.txtList3.Text = string.Empty;
                    };

                    if (_dicDataList != null && _dicDataList.Count > 0)
                    {
                        _dicDataList.ForEach(item => _dicDataListFor2.Add(item));

                        for (int i = 0; i < proList.Count; i++)
                        {
                            int result = 0;
                            foreach (var item in _dicDataList)
                            {
                                int cValue = 0;
                                if (int.TryParse(Convert.ToString(item[propertyName]), out cValue))
                                {
                                    int rang1 = 0;
                                    int rang2 = -1;

                                    if (_RangeRuleSettingEvent != null)
                                    {
                                        _RangeRuleSettingEvent(proList[i], ref rang1, ref rang2);

                                        if (rang2 != -1 && cValue >= rang1 && cValue <= rang2)
                                        {
                                            result++;
                                            _dicDataListFor2.Add(item);
                                        }
                                        else if (rang2 == -1 && cValue > rang1)
                                        {
                                            result++;
                                            _dicDataListFor2.Add(item);
                                        }
                                    }
                                }
                            }

                            if (result > 0)
                            {
                                //创建datapoint
                                DataPoint dp = new DataPoint();
                                //设置datapoint名称
                                dp.AxisXLabel = proList[i];
                                //设置datapoint的值
                                dp.YValue = result;
                                ds.DataPoints.Add(dp);
                            }
                        }
                        chratModern1.chartLeft.Series.Add(ds);
                        //是否需要更新列表
                        if (_isLeadingAgain)
                        {
                            List<FHFormLb> fhList2 = new List<FHFormLb>();
                            _dicDataList.ForEach(Item => fhList2.Add(new FHFormLb(Item)));

                            DataInitCaseByChart(fhList2);
                        }
                        _isLeadingAgain = true;
                    }

                    #endregion

                    this.chratModern1.borTip1.Visibility = System.Windows.Visibility.Collapsed;
                }));

                TomDisPatcherLb tomdisPatcher2 = new TomDisPatcherLb(new System.Action(() =>
                {
                    this.chratModern1.borTip1.Visibility = System.Windows.Visibility.Visible;

                    tomdisPatcher.Start();
                }));

                tomdisPatcher2.Start();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ChartLeftRangeInit", ex.ToString(), propertyName, proList, renderType);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 生成饼图
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="proList"></param>
        public void ChartOneInit(string propertyName, RenderAs renderType)
        {
            try
            {
                TomDisPatcherLb tomdisPatcher = new TomDisPatcherLb(new System.Action(() =>
                {
                    this.TittleReset();

                    chratModern1.txtList4.Text = _fhFormListSave.Count.ToString();

                    _slection = Modern1Type.Modern1Type3;

                    Modern1Type3Init();

                    #region 生成进行中

                    _propertyName1 = propertyName;

                    _renderType1 = renderType;

                    chratModern1.chartLeft.Series.Clear();

                    _dicDataListFor2.Clear();

                    //创建一个DataSeries
                    DataSeries ds = new DataSeries();
                    ds.RenderAs = renderType;
                    ds.LabelStyle = LabelStyles.OutSide;
                    ds.LabelEnabled = true;

                    List<string> proList = new List<string>();

                    foreach (var item in _dicDataList)
                    {
                        string strValue = Convert.ToString(item[propertyName]);
                        if (!string.IsNullOrEmpty(strValue) && !proList.Contains(strValue))
                            proList.Add(strValue);
                    }

                    ds.MouseLeftButtonDown += (object sender, MouseButtonEventArgs e) =>
                    {
                        _dicDataListFor2 = _dicDataList.Where(item => Convert.ToString(item[propertyName]).Equals((sender as DataPoint).AxisXLabel)).ToList<Dictionary<string, object>>();

                        List<FHFormLb> fhList = new List<FHFormLb>();
                        _dicDataListFor2.ForEach(Item => fhList.Add(new FHFormLb(Item)));

                        DataInitCaseByChart(fhList);

                        chratModern1.txtChartRight2.Text = chratModern1.txtList2.Text = (sender as DataPoint).AxisXLabel;
                        chratModern1.txtChartRight3.Text = fhList.Count.ToString();

                        chratModern1.txtList3.Text = string.Empty;
                    };
                    if (_dicDataList != null && _dicDataList.Count > 0)
                    {
                        _dicDataList.ForEach(item => _dicDataListFor2.Add(item));

                        for (int i = 0; i < proList.Count; i++)
                        {
                            var count = _dicDataList.Count(item => Convert.ToString(item[propertyName]).Equals(proList[i]));
                            if (count > 0)
                            {
                                //创建datapoint
                                DataPoint dp = new DataPoint();
                                //设置datapoint名称
                                dp.AxisXLabel = proList[i];
                                //设置datapoint的值
                                dp.YValue = count;
                                ds.DataPoints.Add(dp);
                            }
                        }
                        chratModern1.chartLeft.Series.Add(ds);

                        if (_isLeadingAgain)
                        {
                            List<FHFormLb> fhList2 = new List<FHFormLb>();
                            _dicDataList.ForEach(Item => fhList2.Add(new FHFormLb(Item)));

                            DataInitCaseByChart(fhList2);
                        }
                        _isLeadingAgain = true;
                    }

                    #endregion

                    this.chratModern1.borTip1.Visibility = System.Windows.Visibility.Collapsed;
                }));

                TomDisPatcherLb tomdisPatcher2 = new TomDisPatcherLb(new System.Action(() =>
                {
                    this.chratModern1.borTip1.Visibility = System.Windows.Visibility.Visible;

                    tomdisPatcher.Start();
                }));

                tomdisPatcher2.Start();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ChartOneInit", ex.ToString(), propertyName, renderType);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 生成柱状图
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="proList"></param>
        public void ChartRightInit(string propertyName, List<string> proList, RenderAs renderType)
        {
            try
            {
                TomDisPatcherLb tomdisPatcher = new TomDisPatcherLb(new System.Action(() =>
                   {
                       #region 生成进行中

                       _propertyName2 = propertyName;
                       _proList2 = proList;
                       _renderType2 = renderType;

                       chratModern1.chartRight.Series.Clear();

                       //创建一个DataSeries
                       DataSeries ds = new DataSeries();
                       ds.RenderAs = renderType;
                       ds.LabelStyle = LabelStyles.OutSide;
                       ds.LabelEnabled = true;

                       ds.MouseLeftButtonDown += (object sender, MouseButtonEventArgs e) =>
                       {
                           //声明一个字典集
                           List<Dictionary<string, object>> diclist = null;

                           //获取相符的数据
                           diclist = _dicDataListFor2.Where(item => Convert.ToString(item[propertyName]).Contains((sender as DataPoint).AxisXLabel)).ToList<Dictionary<string, object>>();
                           List<FHFormLb> fhList = new List<FHFormLb>();
                           diclist.ForEach(Item => fhList.Add(new FHFormLb(Item)));

                           DataInitCaseByChart(fhList);

                           chratModern1.txtList3.Text = (sender as DataPoint).AxisXLabel;
                           chratModern1.txtList4.Text = fhList.Count.ToString();
                       };

                       if (_dicDataListFor2.Count > 0)
                       {

                           #region 二级有重复（处理）

                           List<int> ct = new List<int>();

                           for (int i = 0; i < proList.Count; i++)
                           {
                               ct.Add(proList.Count(Item => Item.Equals(proList[i])));
                           }

                           int count = 0;

                           string parentName = string.Empty;

                           Dictionary<string, List<string>> parentList = null;

                           ///证明有重复的
                           if (ct.Count(Item => Item > 1) > 0)
                           {
                               if (_ChildernSameEvent != null)
                                   _ChildernSameEvent(propertyName, ref parentName, ref parentList);

                               if (!string.IsNullOrEmpty(parentName))
                               {
                                   foreach (var item in parentList)
                                   {
                                       foreach (var itt in item.Value)
                                       {
                                           count = _dicDataListFor2.Count(dicItem => Convert.ToString(dicItem[parentName]) == item.Key && Convert.ToString(dicItem[propertyName]).Equals(itt));

                                           if (count > 0)
                                           {
                                               //创建datapoint
                                               DataPoint dp = new DataPoint();
                                               //设置datapoint名称
                                               dp.AxisXLabel = itt;
                                               //设置datapoint的值
                                               dp.YValue = count;
                                               ds.DataPoints.Add(dp);
                                           }
                                       }
                                   }
                               }
                           }
                           else
                           {
                               for (int i = 0; i < proList.Count; i++)
                               {
                                   count = _dicDataListFor2.Count(item => Convert.ToString(item[propertyName]).Contains(proList[i]));

                                   if (count > 0)
                                   {
                                       //创建datapoint
                                       DataPoint dp = new DataPoint();
                                       //设置datapoint名称
                                       dp.AxisXLabel = proList[i];
                                       //设置datapoint的值
                                       dp.YValue = count;
                                       ds.DataPoints.Add(dp);
                                   }
                               }
                           }
                           #endregion

                           chratModern1.chartRight.Series.Add(ds);
                           //件数
                           chratModern1.txtChartRight3.Text = _dicDataListFor2.Count.ToString();
                       }

                       #endregion

                       this.chratModern1.borTip2.Visibility = System.Windows.Visibility.Collapsed;
                   }));

                TomDisPatcherLb tomdisPatcher2 = new TomDisPatcherLb(new System.Action(() =>
                {
                    this.chratModern1.borTip2.Visibility = System.Windows.Visibility.Visible;

                    tomdisPatcher.Start();
                }));

                tomdisPatcher2.Start();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ChartRightInit", ex.ToString(), propertyName, proList, renderType);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 二次列表生成
        /// </summary>
        /// <param name="listResult"></param>
        void DataInitCaseByChart(List<FHFormLb> listResult)
        {
            try
            {
                TomDisPatcherLb tomdisPatcher = new TomDisPatcherLb(new System.Action(() =>
                {
                    //清除原来的数据
                    _fhFormList.Clear();
                    _fhFormListSave.Clear();

                    #region 生成进行中

                    foreach (var form in listResult)
                    {
                        _fhFormList.Add(form);
                        _fhFormListSave.Add(form);
                    }

                    #region 件数、总延时、平均延时设置

                    chratModern1.count故障件数.Text = _fhFormListSave.Count.ToString();

                    double sumYanShi = 0.0;

                    double singleYanShi = 0.0;

                    foreach (var item in _fhFormListSave)
                    {
                        if (!string.IsNullOrEmpty(item.延时) && Double.TryParse(item.延时, out singleYanShi))
                        {
                            sumYanShi += singleYanShi;
                        }
                    }
                    chratModern1.sum故障延时.Text = sumYanShi.ToString();

                    if (Double.IsNaN(sumYanShi / _fhFormListSave.Count))
                        chratModern1.平均延时.Text = "0";
                    else
                        chratModern1.平均延时.Text = (sumYanShi / _fhFormListSave.Count).ToString("0.00");

                    #endregion

                    #region 数据填充

                    ListCollectionView collection = new ListCollectionView(_fhFormList);

                    chratModern1.DataPager1.listInit(this.chratModern1._dataGrid, _fhFormList, 3);

                    #endregion

                    #endregion

                    this.chratModern1.borTip3.Visibility = System.Windows.Visibility.Collapsed;

                    chratModern1.txtList4.Text = _fhFormListSave.Count.ToString();
                }));

                TomDisPatcherLb tomdisPatcher2 = new TomDisPatcherLb(new System.Action(() =>
                {
                    this.chratModern1.borTip3.Visibility = System.Windows.Visibility.Visible;

                    tomdisPatcher.Start();
                }));

                tomdisPatcher2.Start();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DataInitCaseByChart", ex.ToString(), listResult);
            }
            finally
            {
            }
        }

        #endregion

        #region 环比

        public void ChartModern3HuanBiCreate(string heng, string shu)
        {
            try
            {
                this._hengTittle = heng;

                this._shuTittle = shu;

                TomDisPatcherLb tomdisPatcher = new TomDisPatcherLb(new System.Action(() =>
                        {
                            #region 生成中

                            this.HuanBiInit();

                            this.chratModern3.scv.ReSet();

                            this.chratModern3.Create(heng, shu, this.comS);

                            this.chratModern3.scv.dataGrid.Columns[this.chratModern3.scv.dataGrid.Columns.Count - 1].Width = new DataGridLength(50);

                            this.ChartModernHuanBiInit();

                            _isLeadingAgain = true;

                            #endregion

                            this.chratModern3.borTip1.Visibility = System.Windows.Visibility.Collapsed;
                        }));
                TomDisPatcherLb tomdisPatcher2 = new TomDisPatcherLb(new System.Action(() =>
                {
                    this.chratModern3.borTip1.Visibility = System.Windows.Visibility.Visible;

                    CommonMethod.ReFresh();

                    tomdisPatcher.Start();
                }));

                tomdisPatcher2.Start();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ChartModern3HuanBiCreate", ex.ToString(), heng, shu);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 生成图表
        /// </summary>
        public void ChartModernHuanBiInit()
        {
            try
            {
                this.chratModern3.chart3.Series.Clear();
                TomDisPatcherLb tomdisPatcher = new TomDisPatcherLb(new System.Action(() =>
                        {
                            #region 生成中

                            for (int i = 0; i < this.chratModern3.scv.tongjiR._DicTittleTextList[0].Count + 1; i++)
                            {
                                DataSeries ds = new DataSeries();

                                    ds.RenderAs = HuanBiChartType;

                                    // 显示Lable   
                                    ds.LabelStyle = LabelStyles.OutSide;
                                    ds.LabelEnabled = true;

                                string tittle = string.Empty;

                                if (i == this.chratModern3.scv.tongjiR._DicTittleTextList[0].Count)
                                    tittle = "增量";
                                else
                                    tittle = this.chratModern3.scv.tongjiR._DicTittleTextList[0][i] + this.chratModern3.scv.tongjiR._DicTittleTextList[1][i];


                                ds.Name = tittle;

                                for (int j = 0; j < this.chratModern3.scv.tongjiC._DicTittleTextList[0].Count; j++)
                                {
                                    var rowResult = this.chratModern3.scv.dataGrid.Items[i] as DataRowView;

                                    double count = 0;
                                    double.TryParse(rowResult.Row.ItemArray[j].ToString(), out count);

                                    DataPoint dp1 = new DataPoint();

                                    dp1.AxisXLabel = this.chratModern3.scv.tongjiC._DicTittleTextList[0][j];

                                    dp1.YValue = Convert.ToDouble(count);
                                    ds.DataPoints.Add(dp1);
                                }
                                this.chratModern3.chart3.Series.Add(ds);
                            }

                            #endregion

                            this.chratModern3.borTip2.Visibility = System.Windows.Visibility.Collapsed;
                        }));
                TomDisPatcherLb tomdisPatcher2 = new TomDisPatcherLb(new System.Action(() =>
                {
                    this.chratModern3.borTip2.Visibility = System.Windows.Visibility.Visible;

                    tomdisPatcher.Start();
                }));

                tomdisPatcher2.Start();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ChartModernHuanBiInit", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion

        #region 同期

        public void ChartModern4TongQiCreate()
        {
            try
            {
                TomDisPatcherLb tomdisPatcher = new TomDisPatcherLb(new System.Action(() =>
                {
                    #region 生成中

                    this._isLeadingAgain = true;

                    this.TongQiInit();

                    this.chratModern4.ParametersInit(this.comS);

                    this.chratModern4._ListCompleteEvent += () =>
                        {
                            this.ChartModernTongQiInit();
                        };

                    this.chratModern4.borTip1.Visibility = System.Windows.Visibility.Collapsed;

                    #endregion
                }));
                TomDisPatcherLb tomdisPatcher2 = new TomDisPatcherLb(new System.Action(() =>
                {
                    this.chratModern4.borTip1.Visibility = System.Windows.Visibility.Visible;

                    CommonMethod.ReFresh();

                    tomdisPatcher.Start();
                }));
                tomdisPatcher2.Start();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ChartModern4TongQiCreate", ex.ToString());
            }
            finally
            {
            }
        }

        /// <summary>
        /// 生成图表
        /// </summary>
        public void ChartModernTongQiInit()
        {
            try
            {
                TomDisPatcherLb tomdisPatcher = new TomDisPatcherLb(new System.Action(() =>
                {
                    #region 生成中

                    this.chratModern4.chart3.Series.Clear();

                    for (int i = 0; i < this.chratModern4.dataGridList.Items.Count; i++)
                    {
                        DataSeries ds = new DataSeries();

                        ds.RenderAs = TongQiChartType;

                        // 显示Lable   
                        ds.LabelStyle = LabelStyles.OutSide;
                        ds.LabelEnabled = true;

                        string tittle = (this.chratModern4.dataGridList.Items[i] as DataRowView)[0].ToString();

                        ds.Name = tittle;

                        for (int j = 1; j < this.chratModern4.dataGridList.Columns.Count - 1; j++)
                        {
                            var rowResult = (this.chratModern4.dataGridList.Items[i] as DataRowView)[j].ToString();

                            double count = 0;
                            double.TryParse(rowResult, out count);

                            DataPoint dp1 = new DataPoint();

                            dp1.AxisXLabel = this.chratModern4.dataGridList.Columns[j].Header.ToString();

                            dp1.YValue = Convert.ToDouble(count);
                            ds.DataPoints.Add(dp1);
                        }
                        this.chratModern4.chart3.Series.Add(ds);
                    }

                    #endregion

                    this.chratModern4.borTip2.Visibility = System.Windows.Visibility.Collapsed;
                }));
                TomDisPatcherLb tomdisPatcher2 = new TomDisPatcherLb(new System.Action(() =>
                {
                    this.chratModern4.borTip2.Visibility = System.Windows.Visibility.Visible;

                    tomdisPatcher.Start();
                }));

                tomdisPatcher2.Start();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ChartModernTongQiInit", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion

        #endregion

        #region 生成标题

        public void TitleInit(List<string> titles)
        {
            try
            {
                this.chratModern1._dataGrid.Columns.Clear();
                foreach (var item in titles)
                {
                    if (item.Contains("附件"))
                    {
                        DataGridTemplateColumn column1 = new DataGridTemplateColumn();
                        column1.CanUserSort = true;
                        column1.CanUserReorder = true;
                        column1.Header = "附件";
                        column1.CellTemplate = (DataTemplate)this.TryFindResource("DataTemplate1");
                        chratModern1._dataGrid.Columns.Add(column1);
                    }
                    else if (item.Contains("ID"))
                    {
                        DataGridTemplateColumn column3 = new DataGridTemplateColumn();
                        column3.CanUserSort = true;
                        column3.CanUserReorder = true;
                        column3.Header = "ID";
                        column3.CellTemplate = (DataTemplate)this.TryFindResource("DataTemplate2");
                        chratModern1._dataGrid.Columns.Add(column3);
                    }
                    else if (item.Contains("故障发生日期时间"))
                    {
                        DataGridTemplateColumn column = new DataGridTemplateColumn();
                        column.CanUserSort = true;
                        column.CanUserReorder = true;
                        column.Header = "故障发生日期时间";
                        column.CellTemplate = (DataTemplate)this.TryFindResource("DataTemplate3");
                        chratModern1._dataGrid.Columns.Add(column);
                    }
                    else if (item.Contains("责任单位"))
                    {
                        DataGridTemplateColumn column = new DataGridTemplateColumn();
                        column.CanUserSort = true;
                        column.CanUserReorder = true;
                        column.Header = "责任单位";
                        column.CellTemplate = (DataTemplate)this.TryFindResource("DataTemplate4");
                        this.chratModern1._dataGrid.Columns.Add(column);
                    }
                    else
                    {
                        DataGridTextColumn column = new DataGridTextColumn();
                        column.CanUserSort = true;
                        column.CanUserReorder = true;
                        column.Header = item;
                        column.Binding = new Binding(item);
                        chratModern1._dataGrid.Columns.Add(column);
                    }
                }

            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TitleInit", ex.ToString(), titles);
            }
        }

        #endregion

        #region 生成数据

        void DataInit()
        {
            try
            {
                TomDisPatcherLb tomdisPatcher = new TomDisPatcherLb(new System.Action(() =>
              {
                  #region 生成进行中

                  ///查询方法
                  var dicCaml = this.comS.ZongShuJu();

                  if (_isComplete && dicCaml != null)
                  {
                      dicCaml.Add("IsFinish", "是,#Text#Eq");
                  }
                  Thread thread2 = new Thread(new ThreadStart(() =>
                      {
                          #region 数据源处理

                          _dicDataList = DataOperation.ClientGetDataGridDic(Proxy.ListName, dicCaml);
                          _fhFormList.Clear();
                          _fhFormListSave.Clear();

                          #endregion

                          this.Dispatcher.BeginInvoke(new System.Action(() =>
                          {
                              if (this._OtherConditionEvent != null)
                              {
                                  string proper = string.Empty;

                                  string value = string.Empty;
                                  this._OtherConditionEvent(this.chratModern1.txtChartLeft.Text, ref proper, ref   value);
                                  if (!string.IsNullOrEmpty(proper) && !string.IsNullOrEmpty(value))
                                  {
                                      foreach (var item in _dicDataList)
                                      {
                                          if (Convert.ToString(item[proper]).Equals(value))
                                              _fhFormList.Add(new FHFormLb(item));
                                      }
                                  }
                                  else
                                      _dicDataList.ForEach(Item => _fhFormList.Add(new FHFormLb(Item)));
                              }
                              else
                                  _dicDataList.ForEach(Item => _fhFormList.Add(new FHFormLb(Item)));

                              #region 附件前台设置

                              foreach (var form in _fhFormList)
                              {
                                  bool isHave = Convert.ToBoolean(form.是否有附件);
                                  if (this._SettingFuJiJianImageUriEvent != null)
                                  {
                                      this._SettingFuJiJianImageUriEvent(ref _hasFuJianImageUri, ref _noFuJIanImageUri);
                                      if (!string.IsNullOrEmpty(_hasFuJianImageUri) && !string.IsNullOrEmpty(_noFuJIanImageUri))
                                      {
                                          if (isHave)
                                          {
                                              form.附件 = _hasFuJianImageUri;
                                          }
                                          else
                                          {
                                              form.附件 = _noFuJIanImageUri;
                                          }
                                      }
                                  }

                                  if (form.责任单位.Contains(";"))
                                  {
                                      if (!string.IsNullOrEmpty(form.责任单位.Split(new char[] { ';' })[1]))
                                      {
                                          form.责任单位背景 = Style1.Instacnce.Resources["lin1"] as Brush;
                                      }
                                  }
                              }
                              #endregion

                              #region 件数、总延时、平均延时设置

                              chratModern1.count故障件数.Text = _fhFormListSave.Count.ToString();

                              double sumYanShi = 0.0;

                              double singleYanShi = 0.0;

                              foreach (var item in _fhFormListSave)
                              {
                                  if (!string.IsNullOrEmpty(item.延时) && Double.TryParse(item.延时, out singleYanShi))
                                  {
                                      sumYanShi += singleYanShi;
                                  }
                              }

                              chratModern1.sum故障延时.Text = sumYanShi.ToString();

                              if (Double.IsNaN(sumYanShi / _fhFormListSave.Count))
                                  chratModern1.平均延时.Text = "0";
                              else
                                  chratModern1.平均延时.Text = (sumYanShi / _fhFormListSave.Count).ToString("0.00");


                              #endregion

                              #region 数据填充

                              ListCollectionView collection = new ListCollectionView(_fhFormList);

                              chratModern1.DataPager1.listInit(this.chratModern1._dataGrid, _fhFormList, 3);

                              #endregion

                              #region 图表1模式选择

                              this.chratModern1.borTip3.Visibility = System.Windows.Visibility.Collapsed;

                              if (!string.IsNullOrEmpty(_propertyName1) && _proList1 != null)
                              {
                                  if (_slection == Modern1Type.Modern1Type1)
                                  {
                                      Modern1Type12Init();
                                      ChartLeftInit(_propertyName1, _proList1, _renderType1);

                                      if (!string.IsNullOrEmpty(_propertyName2) && _proList2 != null)
                                          ChartRightInit(_propertyName2, _proList2, this._renderType2);


                                  }
                                  else if (_slection == Modern1Type.Modern1Type2)
                                  {
                                      Modern1Type12Init();
                                      ChartLeftRangeInit(_propertyName1, _proList1, _renderType1);

                                      if (!string.IsNullOrEmpty(_propertyName2) && _proList2 != null)
                                          ChartRightInit(_propertyName2, _proList2, this._renderType2);
                                  }

                                  else if (_slection == Modern1Type.Modern1Type3)
                                  {
                                      Modern1Type3Init();
                                      ChartOneInit(_propertyName1, _renderType1);
                                  }
                              }

                              #endregion

                          }));
                      }));
                  thread2.Start();

                  #endregion

              }));

                TomDisPatcherLb tomdisPatcher2 = new TomDisPatcherLb(new System.Action(() =>
                {
                    this.chratModern1.borTip3.Visibility = System.Windows.Visibility.Visible;

                    tomdisPatcher.Start();
                }));

                tomdisPatcher2.Start();

            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DataInit", ex.ToString());
            }
        }

        #endregion

        #region 表单数据点击事件

        /// <summary>
        /// 点击发生日期激发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartTime_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        /// <summary>
        /// 点击ID激发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ID_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        /// <summary>
        /// 点击附件激发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgexit2_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //获取相应ID
                int ID = Convert.ToInt32((sender as FrameworkElement).Tag);

                MhczTBG.Common.CommonMethod.WatchImage(ID);

            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "imgexit2_MouseLeftButtonUp", ex.ToString());
            }
        }

        #endregion

        #region 打印

        void btf3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TomDisPatcherLb tomdisPatcher = new TomDisPatcherLb(new System.Action(() =>
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback((o) =>
                    {
                        this.Dispatcher.BeginInvoke(new System.Action(() =>
                        {
                            if (this.chratModern1._dataGrid.Items.Count > 0)
                            {
                                PrintDocument print = new PrintDocument();
                                print.ContentRendered += new EventHandler(print_ContentRendered);
                                CommonMethod.FHDataGrid_btn_打印(print, "历史故障---" + DateTime.Now.ToShortDateString(), chratModern1.DataPager1, this.chratModern1._dataGrid, this._fhFormListSave);
                            }
                        }));
                    }));
                }));

                TomDisPatcherLb tomdisPatcher2 = new TomDisPatcherLb(new System.Action(() =>
                {
                    this.chratModern1.borTip3.Visibility = System.Windows.Visibility.Visible;

                    tomdisPatcher.Start();
                }));

                tomdisPatcher2.Start();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btf3_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }

        }

        /// <summary>
        /// 打印控件显示事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void print_ContentRendered(object sender, EventArgs e)
        {
            try
            {
                this.chratModern1.borTip3.Visibility = System.Windows.Visibility.Collapsed;

            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "print_ContentRendered", ex.ToString());
            }
        }

        #endregion

        #region 导入Excel

        void btExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.chratModern1.borTip3.Visibility = System.Windows.Visibility.Visible;
                ThreadPool.QueueUserWorkItem(new WaitCallback((o) =>
                {
                    try
                    {
                        if (this.chratModern1._dataGrid.Items.Count > 0)
                        {
                            CommonMethod.FHDataGrid_btn_导入Excel("历史故障---" + DateTime.Now.ToShortDateString(), this.chratModern1._dataGrid, this._fhFormListSave, ref    _excelother);
                        }
                    }
                    catch (Exception ex)
                    {
                        MethodLb.CreateLog(this.GetType().FullName, "btExcel_Click", ex.ToString(), sender, e);
                    }
                    finally
                    {
                    }
                    this.Dispatcher.BeginInvoke(new System.Action(() =>
                    {
                        this.chratModern1.borTip3.Visibility = System.Windows.Visibility.Collapsed;
                    }));
                }));

            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btExcel_Click", ex.ToString());
            }
        }

        #endregion

        #region 标题设置

        public void TittleInit(string txtchartLeft, string txtchartRight)
        {
            //设置一级标题
            this.chratModern1.txtChartLeft.Text = txtchartLeft;
            //设置二级标题
            this.chratModern1.txtChartRight.Text = txtchartRight;
        }


        public void TittleHuanBiInit(string txtchartTop, string txtchartBottom)
        {
            //列表标题
            this.chratModern3.txtChartTop.Text = txtchartTop;
            //图表标题
            this.chratModern3.txtChartBottom.Text = txtchartBottom;
        }

        public void TittleTongQiInit(string txtchartTop, string txtchartBottom)
        {
            //列表标题
            this.chratModern4.txtChartTop.Text = txtchartTop;
            //图表标题
            this.chratModern4.txtChartBottom.Text = txtchartBottom;
        }


        /// <summary>
        /// 还原默认值
        /// </summary>
        public void TittleReset()
        {
            chratModern1.txtList2.Text = string.Empty;

            chratModern1.txtChartRight2.Text = string.Empty;

            chratModern1.txtChartRight3.Text = "0";

            chratModern1.txtList2.Text = string.Empty;
            chratModern1.txtList3.Text = string.Empty;
            chratModern1.txtList4.Text = "0";
        }

        #endregion

        #region 局部模式变更

        void Modern1Type12Init()
        {
            this.chratModern1.gridMain.ColumnDefinitions[0].Width = new GridLength(0.6, GridUnitType.Star);
            this.chratModern1.gridMain.ColumnDefinitions[1].Width = new GridLength(0.4, GridUnitType.Star);
            CommonInit();
        }

        void Modern1Type3Init()
        {

            this.chratModern1.gridMain.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
            this.chratModern1.gridMain.ColumnDefinitions[1].Width = new GridLength(0);
            CommonInit();
        }

        #endregion

        #region 全局模式变更

        void CommonInit()
        {
            this.chratModern1.Visibility = System.Windows.Visibility.Visible;
            this.chratModern3.Visibility = System.Windows.Visibility.Collapsed;
            this.chratModern4.Visibility = System.Windows.Visibility.Collapsed;

            this.comS.gridCommon.Visibility = System.Windows.Visibility.Visible;
            this.comS.gridHuanBi.Visibility = System.Windows.Visibility.Collapsed;
            this.comS.gridTongQi.Visibility = System.Windows.Visibility.Collapsed;
        }

        void HuanBiInit()
        {
            this.chratModern1.Visibility = System.Windows.Visibility.Collapsed;
            this.chratModern3.Visibility = System.Windows.Visibility.Visible;
            this.chratModern4.Visibility = System.Windows.Visibility.Collapsed;

            this.comS.gridCommon.Visibility = System.Windows.Visibility.Collapsed;
            this.comS.gridHuanBi.Visibility = System.Windows.Visibility.Visible;
            this.comS.gridTongQi.Visibility = System.Windows.Visibility.Collapsed;
        }

        void TongQiInit()
        {
            this.chratModern1.Visibility = System.Windows.Visibility.Collapsed;
            this.chratModern3.Visibility = System.Windows.Visibility.Collapsed;
            this.chratModern4.Visibility = System.Windows.Visibility.Visible;

            this.comS.gridCommon.Visibility = System.Windows.Visibility.Collapsed;
            this.comS.gridHuanBi.Visibility = System.Windows.Visibility.Collapsed;
            this.comS.gridTongQi.Visibility = System.Windows.Visibility.Visible;
        }

        #endregion

        #endregion

    }

    public enum Modern1Type
    {
        Modern1Type1,
        Modern1Type2,
        Modern1Type3,
    }


}
