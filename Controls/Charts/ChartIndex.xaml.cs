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
using Visifire.Charts;
using System.Data;
using MhczTBG.StyleResource;
using TongXin.UIComman;
using System.Threading;
using MhczTBG.Controls.Print;

namespace MhczTBG.Controls.Charts
{
    /// <summary>
    /// ChartIndex.xaml 的交互逻辑
    /// </summary>
    public partial class ChartIndex : UserControl
    {
        #region 变量

        /// <summary>
        /// 存储条件
        /// </summary>
        Dictionary<string, string> _dicCaml = new Dictionary<string, string>();

        /// <summary>
        /// 存储源数据
        /// </summary>
        List<Dictionary<string, object>> _dicData = new List<Dictionary<string, object>>();

        /// <summary>
        /// 存储选择月的日的集合
        /// </summary>
        List<string> _dayList = new List<string>();

        /// <summary>
        /// 当前年月
        /// </summary>
        string _nowYearAndMonth = string.Empty;

        /// <summary>
        /// 默认内部字段
        /// </summary>
        string _PropertyName = Proxy.ShebeiTypeYiJi;

        /// <summary>
        /// 默认字段集
        /// </summary>
        List<string> _strList = Proxy.ShebeiTypeYiJiList;

        /// <summary>
        /// 默认内部字段
        /// </summary>
        string _PropertyName2 = Proxy.ShebeiTypeYiJi;

        /// <summary>
        /// 默认字段集
        /// </summary>
        List<string> _strList2 = Proxy.ShebeiTypeYiJiList;

        /// <summary>
        /// 默认内部字段
        /// </summary>
        string _PropertyName3 = "CheJianMingCheng";

        /// <summary>
        /// 默认字段集
        /// </summary>
        List<string> _strList3 = Proxy.CheJianMingChengList;

        /// <summary>
        /// 附件图片路径
        /// </summary>
        string _hasFuJianImageUri = string.Empty;

        /// <summary>
        /// 无附件图片路径
        /// </summary>
        string _noFuJIanImageUri = string.Empty;

        /// <summary>
        /// 标题集
        /// </summary>
        Dictionary<string, string> _titlelist = new Dictionary<string, string>();

        /// <summary>
        /// 存储的数据（列表所用）
        /// </summary>
        List<FHFormLb> _fhFormList = new List<FHFormLb>();

        /// <summary>
        /// 存储的数据（打印,导入Excel所用）
        /// </summary>
        List<FHFormLb> _fhFormListSave = new List<FHFormLb>();

        /// <summary>
        /// 配置文件存放位置
        /// </summary>
        IniFile IniConfig = new IniFile("C:/Config/iniFile.ini");

        /// <summary>
        /// 图表2数据
        /// </summary>
        Dictionary<string, object> _dicChartData2 = new Dictionary<string, object>();

        /// <summary>
        /// 图表3数据
        /// </summary>
        Dictionary<string, object> _dicChartData3 = new Dictionary<string, object>();

        /// <summary>
        /// table表单
        /// </summary>
        DataTable dtMain = null;

        /// <summary>
        /// excel应用程序
        /// </summary>
        Microsoft.Office.Interop.Excel.Application _excelother = null;  

        #endregion

        #region 自定义委托事件

        public delegate void AppointPropertyEventHandle(ref string PropertyName, ref List<string> listPropertyValue);
        /// <summary>
        /// 指定字段
        /// </summary>
        public event AppointPropertyEventHandle _AppointPropertyEvent = null;

        /// <summary>
        /// 指定字段2
        /// </summary>
        public event AppointPropertyEventHandle _AppointPropertyEvent2 = null;

        /// <summary>
        /// 指定字段3
        /// </summary>
        public event AppointPropertyEventHandle _AppointPropertyEvent3 = null;

        public delegate void SettingFuJiJianImageUriEventHandle(ref string HasFuJianUri, ref string NoFuJianUri);
        /// <summary>
        /// 附件图片
        /// </summary>
        public event SettingFuJiJianImageUriEventHandle _SettingFuJiJianImageUriEvent = null;

        #endregion

        #region 构造函数

        public ChartIndex()
        {
            try
            {
                InitializeComponent();

                #region UI事件区域

                this.dtDaoHang._ClickEvent += new MhczTBG.Controls.DataButton.ClickEventHandle(dateSelect1__ClickEvent);
                this.dtDaoHang.btnAddGuzhang.Click += new RoutedEventHandler(btnAddGuzhang_Click);
                this.dtDaoHang.btnExcel.Click += new RoutedEventHandler(btnExcel_Click);
                this.dtDaoHang.btnPrint.Click += new RoutedEventHandler(btnPrint_Click);
                this.dtDaoHang.btnShiTu.Click += new RoutedEventHandler(btnShiTu_Click);

                this.dataGrid.PreviewMouseWheel += new MouseWheelEventHandler(dataGrid_PreviewMouseWheel);

                System.Windows.Application.Current.Exit += new ExitEventHandler(Current_Exit);

                #endregion

                if (!string.IsNullOrEmpty(Proxy.UserName))
                    this.dtDaoHang.SelectedIndex = DateTime.Now.Month - 1;

                this.Resources.MergedDictionaries.Add(MyStyle.Instacnce.borDataGrid3.Resources);

                ParaMersInit();

                TitleInit(_titlelist.Values.ToList());
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ChartIndex", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion

        #region 初始化

        void ParaMersInit()
        {
            try
            {
                //读取配置文件
                string FuHeChaXunView = UIHelper.frombase(IniConfig.IniReadValue(Proxy.UserName, "FuHeChaXunView").ToString());
                if (FuHeChaXunView != "")
                {
                    foreach (string item in FuHeChaXunView.Split(new char[] { ';' }))
                    {
                        //将配置文件中的字段添加到标题字段字典
                        _titlelist.Add(item.Split(new char[] { ',' })[0], item.Split(new char[] { ',' })[1]);
                    }
                }
                else
                {
                    _titlelist.Add("ID", "ID");
                    _titlelist.Add("附件", "附件");
                    _titlelist.Add("startData", "故障发生日期时间");
                    _titlelist.Add("Line", "线别");
                    _titlelist.Add("GuZhangSheBeiMingCheng", "障碍设备名称");
                    _titlelist.Add("GongQuMingCheng", "工区");
                    _titlelist.Add("CheJianMingCheng", "责任单位");
                    _titlelist.Add("ZhangAiXianXiang", "障碍现象");
                    _titlelist.Add("YingXiangFanWei", "影响范围");
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ParaMersInit", ex.ToString());
            }
        }

        #endregion

        #region Ui事件区域

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

        void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TomDisPatcherLb tomdisPatcher = new TomDisPatcherLb(new System.Action(() =>
                {
                    ThreadPool.QueueUserWorkItem((o) =>
                    {
                        this.Dispatcher.BeginInvoke(new System.Action(() =>
                        {
                            if (this.dataGrid.Items.Count > 0)
                            {
                                PrintDocument print = new PrintDocument();
                                print.ContentRendered += new EventHandler(print_ContentRendered);
                                var tittle = this.txtTittle2PropertyName.Text + " : " + this.txtTittle2Year + this.txtTittle2Month + this.txtTittle2Day.Text + "故障列表";
                                CommonMethod.FHDataGrid_btn_打印(print, tittle, DataPager1, this.dataGrid, this._fhFormListSave);
                            }
                        }));
                    });
                }));

                TomDisPatcherLb tomdisPatcher2 = new TomDisPatcherLb(new System.Action(() =>
                {
                    this.borTip2.Visibility = System.Windows.Visibility.Visible;

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
                this.borTip2.Visibility = System.Windows.Visibility.Collapsed;

            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "print_ContentRendered", ex.ToString());
            }
        }

        void btnExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.borTip2.Visibility = System.Windows.Visibility.Visible;
                ThreadPool.QueueUserWorkItem((o) =>
                {
                    try
                    {
                        if (this.dataGrid.Items.Count > 0)
                        {
                            var tittle = this.txtTittle2PropertyName.Text + " : " + this.txtTittle2Year + this.txtTittle2Month + this.txtTittle2Day.Text + "故障列表";
                            CommonMethod.FHDataGrid_btn_导入Excel(tittle , this.dataGrid, this._fhFormListSave, ref    _excelother);
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
                        this.borTip2.Visibility = System.Windows.Visibility.Collapsed;
                    }));
                });
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btExcel_Click", ex.ToString());
            }
        }

        void Current_Exit(object sender, ExitEventArgs e)
        {
            if (_excelother != null)
                CommonMethod.Kill(_excelother);
        }

        void btnAddGuzhang_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ID_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

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

        private void StartTime_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        void dataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
        }

        #endregion

        #region 附加方法

        /// <summary>
        /// 导航事件
        /// </summary>
        /// <param name="text"></param>
        void dateSelect1__ClickEvent(string text)
        {
            try
            {
                this._nowYearAndMonth = text;

                DateTime startData = Convert.ToDateTime(text);
                DateTime endData = startData.AddMonths(1).AddSeconds(-1);

                this.txtTittle3Year.Text = this.txtTittle2Year.Text = this.txtTittleYear.Text = startData.Year + "年";
                this.txtTittle3Month.Text = this.txtTittle2Month.Text = this.txtTittleMonth.Text = startData.Month + "月";

                this.txtTittle2PropertyName.Text = this.txtTittle2Day.Text = this.txtTittle3Day.Text = string.Empty;

                this._dicCaml.Clear();

                this._dicCaml.Add("startData", startData.ToShortDateString() + "T" + startData.ToLongTimeString() + "Z,#DateTime#Geq");
                this._dicCaml.Add("EndData", endData.ToShortDateString() + "T" + endData.ToLongTimeString() + "Z,#DateTime#Leq");

                _dicCaml.Add("IsFinish", "是,#Text#Eq");

                Thread thread = new Thread(() =>
                    {
                        this.dtMain = DataOperation.ClientGetDic(Proxy.ListName, _dicCaml, ref _dicData);

                        this._dayList.Clear();
                        var dayCount = endData.Day;
                        for (int i = 1; i <= dayCount; i++)
                            _dayList.Add(i + "日");
                        this.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                Chart1Init();
                            }));
                    });
                thread.Start();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "dateSelect1__ClickEvent", ex.ToString(), text);
            }
            finally
            {
            }
        }

        #region 生成图表1

        /// <summary>
        /// 生成图表1
        /// </summary>
        /// <param name="dtMain"></param>
        void Chart1Init()
        {
            try
            {
                TomDisPatcherLb tomdisPatcher = new TomDisPatcherLb(new System.Action(() =>
               {
                   #region 生成中

                   this.chart1.Series.Clear();

                   if (this._AppointPropertyEvent != null)
                       this._AppointPropertyEvent(ref _PropertyName, ref _strList);

                   if (dtMain.Rows.Count > 0)
                   {
                       #region 图表2，图表3数据

                       this._dicChartData2.Clear();
                       this._dicChartData3.Clear();

                       if (this._AppointPropertyEvent2 != null)
                           this._AppointPropertyEvent2(ref _PropertyName2, ref _strList2);

                       foreach (var item in _strList2)
                       {
                           object count = 0;
                           //条件表达式
                           string Expression = _PropertyName2 + "='" + item + "'";
                           count = dtMain.Compute("Count(ID)", Expression);
                           this._dicChartData2.Add(item, count);
                       }

                       if (this._AppointPropertyEvent3 != null)
                           this._AppointPropertyEvent3(ref _PropertyName3, ref _strList3);

                       foreach (var item in _strList3)
                       {
                           object count = 0;
                           //条件表达式
                           string Expression = _PropertyName3 + "='" + item + "'";
                           count = dtMain.Compute("Count(ID)", Expression);
                           this._dicChartData3.Add(item, count);
                       }

                       #endregion
                   }

                   #region 生成图表1

                   for (int i = 0; i <= _strList.Count; i++)
                   {
                       DataSeries ds = new DataSeries();

                       ds.RenderAs = RenderAs.Line;

                       // 显示Lable   
                       ds.LabelStyle = LabelStyles.OutSide;
                       ds.LabelEnabled = true;

                       if (i == _strList.Count)
                           ds.Name = "全部";
                       else
                           ds.Name = _strList[i];

                       foreach (var day in this._dayList)
                       {
                           object count = 0;
                           if (dtMain.Rows.Count > 0)
                           {
                               //起始时间和终止时间
                               var start = Convert.ToDateTime(_nowYearAndMonth + day);
                               var end = start.AddDays(1).AddSeconds(-1);
                               //条件表达式
                               string Expression = string.Empty;

                               if (i == _strList.Count)
                                   Expression = "startData" + ">=" + "'" + start + "'" + " And " + "startData" + "<" + "'" + end + "'";
                               else
                                   Expression = "startData" + ">=" + "'" + start + "'" + " And " + "startData" + "<" + "'" + end + "'" + " And " + _PropertyName + "='" + _strList[i] + "'";

                               count = dtMain.Compute("Count(ID)", Expression);
                           }

                           DataPoint dp1 = new DataPoint();
                           dp1.AxisXLabel = day;
                           dp1.YValue = Convert.ToDouble(count);
                           dp1.Tag = ds.Name;
                           dp1.MouseLeftButtonDown += new MouseButtonEventHandler(dp1_MouseLeftButtonDown);

                           ds.DataPoints.Add(dp1);
                       }
                       this.chart1.Series.Add(ds);
                   }

                   #endregion

                   #endregion

                   #region 生成列表

                   List<FHFormLb> listResult = new List<FHFormLb>();
                   this._dicData.ForEach(item => listResult.Add(new FHFormLb(item)));
                   ListInit(listResult);

                   #endregion

                   Chart2Init();

                   this.borTip1.Visibility = System.Windows.Visibility.Collapsed;
               }));

                TomDisPatcherLb tomdisPatcher2 = new TomDisPatcherLb(new System.Action(() =>
                {
                    this.borTip1.Visibility = System.Windows.Visibility.Visible;

                    tomdisPatcher.Start();
                }));

                tomdisPatcher2.Start();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Chart1Init", ex.ToString(), dtMain);
            }
            finally
            {
            }
        }

        void dp1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var dataPoint = (sender as DataPoint);

            var day = Convert.ToString(dataPoint.AxisXLabel);

            var startDate = Convert.ToDateTime(this._nowYearAndMonth + day);

            var endDate = startDate.AddDays(1).AddSeconds(-1);

            var dsName = Convert.ToString(dataPoint.Tag);

            this.txtTittle2Day.Text = this.txtTittle3Day.Text = day;
            this.txtTittle2PropertyName.Text = dsName;

            #region 图表2，图表3数据

            _dicChartData2.Clear();
            _dicChartData3.Clear();

            if (this._AppointPropertyEvent2 != null)
                this._AppointPropertyEvent2(ref _PropertyName2, ref _strList2);

            foreach (var item in _strList2)
            {
                object count = 0;
                //条件表达式
                string Expression = "startData" + ">=" + "'" + startDate + "'" + " And " + "startData" + "<" + "'" + endDate + "'" + " And " + _PropertyName2 + "='" + item + "'";
                count = dtMain.Compute("Count(ID)", Expression);
                _dicChartData2.Add(item, count);
            }

            if (this._AppointPropertyEvent3 != null)
                this._AppointPropertyEvent3(ref _PropertyName3, ref _strList3);

            foreach (var item in _strList3)
            {
                object count = 0;
                //条件表达式
                string Expression = "startData" + ">=" + "'" + startDate + "'" + " And " + "startData" + "<" + "'" + endDate + "'" + " And " + _PropertyName3 + "='" + item + "'";
                count = dtMain.Compute("Count(ID)", Expression);
                _dicChartData3.Add(item, count);
            }

            List<FHFormLb> listResult = new List<FHFormLb>();

            foreach (var item in _dicData)
            {
                if (!dsName.Equals("全部"))
                {
                    if (Convert.ToDateTime(item["startData"]) >= startDate && Convert.ToDateTime(item["startData"]) < endDate && Convert.ToString(item[_PropertyName]).Equals(dsName))
                        listResult.Add(new FHFormLb(item));
                }
                else
                {
                    if (Convert.ToDateTime(item["startData"]) >= startDate && Convert.ToDateTime(item["startData"]) < endDate )
                        listResult.Add(new FHFormLb(item));
                }
            }
            ListInit(listResult);

            Chart2Init();

            #endregion
        }

        #endregion

        #region 生成标题

        public void TitleInit(List<string> titles)
        {
            try
            {
                this.dataGrid.Columns.Clear();
                foreach (var item in titles)
                {
                    if (item.Contains("附件"))
                    {
                        DataGridTemplateColumn column1 = new DataGridTemplateColumn();
                        column1.CanUserSort = true;
                        column1.CanUserReorder = true;
                        column1.Header = "附件";
                        column1.CellTemplate = (DataTemplate)this.TryFindResource("DataTemplate1");
                        dataGrid.Columns.Add(column1);
                    }
                    else if (item.Contains("ID"))
                    {
                        DataGridTemplateColumn column3 = new DataGridTemplateColumn();
                        column3.CanUserSort = true;
                        column3.CanUserReorder = true;
                        column3.Header = "ID";
                        column3.CellTemplate = (DataTemplate)this.TryFindResource("DataTemplate2");
                        dataGrid.Columns.Add(column3);
                    }
                    else if (item.Contains("故障发生日期时间"))
                    {
                        DataGridTemplateColumn column = new DataGridTemplateColumn();
                        column.CanUserSort = true;
                        column.CanUserReorder = true;
                        column.Header = "故障发生日期时间";
                        column.CellTemplate = (DataTemplate)this.TryFindResource("DataTemplate3");
                        dataGrid.Columns.Add(column);
                    }
                    else if (item.Contains("责任单位"))
                    {
                        DataGridTemplateColumn column = new DataGridTemplateColumn();
                        column.CanUserSort = true;
                        column.CanUserReorder = true;
                        column.Header = "责任单位";
                        column.CellTemplate = (DataTemplate)this.TryFindResource("DataTemplate4");
                        dataGrid.Columns.Add(column);
                    }
                    else
                    {
                        DataGridTextColumn column = new DataGridTextColumn();
                        column.CanUserSort = true;
                        column.CanUserReorder = true;
                        column.Header = item;
                        column.Binding = new Binding(item);
                        dataGrid.Columns.Add(column);
                    }
                }

            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TitleInit", ex.ToString(), titles);
            }
        }

        #endregion

        #region 生成列表

        /// <summary>
        /// 二次列表生成
        /// </summary>
        /// <param name="listResult"></param>
        void ListInit(List<FHFormLb> listResult)
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
                    }

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

                        _fhFormListSave.Add(form);
                    }
                    #endregion

                    #region 件数、总延时、平均延时设置

                    count故障件数.Text = _fhFormListSave.Count.ToString();

                    double sumYanShi = 0.0;

                    double singleYanShi = 0.0;

                    foreach (var item in _fhFormListSave)
                    {
                        if (!string.IsNullOrEmpty(item.延时) && Double.TryParse(item.延时, out singleYanShi))
                        {
                            sumYanShi += singleYanShi;
                        }
                    }
                    sum故障延时.Text = sumYanShi.ToString();

                    if (Double.IsNaN(sumYanShi / _fhFormListSave.Count))
                        平均延时.Text = "0";
                    else
                        平均延时.Text = (sumYanShi / _fhFormListSave.Count).ToString("0.00");

                    #endregion

                    #region 数据填充

                    ListCollectionView collection = new ListCollectionView(_fhFormList);

                    DataPager1.listInit(this.dataGrid, _fhFormList, 5);

                    #endregion

                    #endregion

                    this.borTip2.Visibility = System.Windows.Visibility.Collapsed;

                }));

                TomDisPatcherLb tomdisPatcher2 = new TomDisPatcherLb(new System.Action(() =>
                {
                    this.borTip2.Visibility = System.Windows.Visibility.Visible;

                    tomdisPatcher.Start();
                }));

                tomdisPatcher2.Start();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ListInit", ex.ToString(), listResult);
            }
            finally
            {
            }
        }

        #endregion

        #region 生成图表2

        void Chart2Init()
        {
            try
            {
                TomDisPatcherLb tomdisPatcher = new TomDisPatcherLb(new System.Action(() =>
                {
                    #region 图表2生成

                    this.chart2.Series.Clear();

                    DataSeries ds2 = new DataSeries();

                    ds2.RenderAs = RenderAs.Pie;

                    // 显示Lable   
                    ds2.LabelStyle = LabelStyles.OutSide;
                    ds2.LabelEnabled = true;

                    foreach (var item in _dicChartData2)
                    {
                        var count = Convert.ToDouble(item.Value);
                        if (count > 0)
                        {
                            DataPoint dp1 = new DataPoint();
                            dp1.YValue = count;
                            dp1.AxisXLabel = item.Key;
                            ds2.DataPoints.Add(dp1);
                        }
                    }
                    this.chart2.Series.Add(ds2);

                    #endregion

                    #region 图表3生成

                    this.chart3.Series.Clear();

                    DataSeries ds3 = new DataSeries();

                    ds3.RenderAs = RenderAs.Pie;

                    // 显示Lable   
                    ds3.LabelStyle = LabelStyles.OutSide;
                    ds3.LabelEnabled = true;

                    foreach (var item in _dicChartData3)
                    {
                        var count = Convert.ToDouble(item.Value);
                        if (count > 0)
                        {
                            DataPoint dp1 = new DataPoint();
                            dp1.AxisXLabel = item.Key;
                            dp1.YValue = count;
                            ds3.DataPoints.Add(dp1);
                        }
                    }
                    this.chart3.Series.Add(ds3);

                    #endregion

                    this.borTip3.Visibility = System.Windows.Visibility.Collapsed;
                }));

                TomDisPatcherLb tomdisPatcher2 = new TomDisPatcherLb(new System.Action(() =>
                {
                    this.borTip3.Visibility = System.Windows.Visibility.Visible;

                    tomdisPatcher.Start();
                }));

                tomdisPatcher2.Start();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Chart2Init", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion

        #endregion


    }
}
