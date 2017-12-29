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
using Microsoft.Office.Interop.Excel;
using MhczTBG.Common;
using MhczTBG.Controls;
using TongXin.UIComman;
using MhczTBG.Controls.Print;
using MhczTBG.StyleResource;

namespace MhczTBG.Controls
{
    /// <summary>
    /// FuHeChaXun.xaml 的交互逻辑
    /// </summary>
    public partial class ChaXun : UserControl
    {
        #region 声明变量

        /// <summary>
        /// 临时存储标题
        /// </summary>
        Dictionary<string, string> titlelist = new Dictionary<string, string>();

        /// <summary>
        /// 用于填充列表使用
        /// </summary>
        List<FHFormLb> fhFormList = new List<FHFormLb>();

        /// <summary>
        /// 用于打印，导入Excel
        /// </summary>
        List<FHFormLb> fhFormListSave = new List<FHFormLb>();

        //配置文件存放位置
         IniFile IniConfig = new IniFile("C:/Config/iniFile.ini");

        /// <summary>
        /// 是否为复合查询
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

        #endregion

        #region 自定义委托事件

        public delegate void SettingFuJiJianImageUriEventHandle(ref string HasFuJianUri,ref string NoFuJianUri);
        /// <summary>
        /// 附件图片
        /// </summary>
        public event SettingFuJiJianImageUriEventHandle SettingFuJiJianImageUriEvent = null;

        #endregion

        #region 构造函数

        public ChaXun()
        {
            try
            {
                InitializeComponent();

                #region 注册事件

                this.comS.btnShiTu.Click += new RoutedEventHandler(btnShiTu_Click);
                this.comS.btnSearch.Click += new RoutedEventHandler(btnSearch_Click);
                this.comS.btf3.Click += new RoutedEventHandler(btf3_Click);
                this.comS.btExcel.Click += new RoutedEventHandler(btExcel_Click);

                #endregion

                ParaMersInit();

                TitleInit(titlelist.Values.ToList());

                DataInit();

                System.Windows.Application.Current.Exit += new ExitEventHandler(Current_Exit);

            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ChaXun", ex.ToString());
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
                        titlelist.Add(item.Split(new char[] { ',' })[0], item.Split(new char[] { ',' })[1]);
                    }
                }
                else
                {
                    titlelist.Add("ID", "ID");
                    titlelist.Add("附件", "附件");
                    titlelist.Add("startData", "故障发生日期时间");
                    titlelist.Add("Line", "线别");
                    titlelist.Add("GuZhangSheBeiMingCheng", "障碍设备名称");
                    titlelist.Add("GongQuMingCheng", "工区");
                    titlelist.Add("CheJianMingCheng", "责任单位");
                    titlelist.Add("ZhangAiXianXiang", "障碍现象");
                    titlelist.Add("YingXiangFanWei", "影响范围");
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
                comS.expander.IsExpanded = false;

                TomDisPatcherLb tomdisPatcher = new TomDisPatcherLb(new System.Action(() =>
                    {
                        DataInit();


                    }));

                TomDisPatcherLb tomdisPatcher2 = new TomDisPatcherLb(new System.Action(() =>
                     {
                         this.borTip.Visibility = System.Windows.Visibility.Visible;

                         CommonMethod.ReFresh();

                         tomdisPatcher.Start();
                     }));

                tomdisPatcher2.Start();

            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btnSearch_Click", ex.ToString());
            }
        }

        #endregion

        #region 自定义视图事件

        void btnShiTu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectColumns selection = new SelectColumns(titlelist);
                selection.Closed += (object sender1, EventArgs e1) =>
                {
                    SelectColumns quanxian2 = ((SelectColumns)sender1);

                    bool? result = quanxian2.DialogResult;
                    if (result.HasValue && result.Value)
                    {
                        titlelist.Clear();
                        foreach (var item in quanxian2.VisbleList)
                        {
                            titlelist.Add(item.Key, item.Value);
                        }

                        TitleInit(titlelist.Values.ToList());
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

        void Current_Exit(object sender, ExitEventArgs e)
        {
            if (_excelother != null)
                CommonMethod.Kill(_excelother);
        }

        #endregion

        #region 辅助方法
 
        #region 生成标题

        public void TitleInit(List<string> titles)
        {
            try
            {
                this._dataGrid.Columns.Clear();
                foreach (var item in titles)
                {
                    if (item.Contains("附件"))
                    {
                        DataGridTemplateColumn column1 = new DataGridTemplateColumn();
                        column1.CanUserSort = true;
                        column1.CanUserReorder = true;
                        column1.Header = "附件";
                        column1.CellTemplate = (DataTemplate)this.TryFindResource("DataTemplate1");
                        _dataGrid.Columns.Add(column1);
                    }
                    else if (item.Contains("ID"))
                    {
                        DataGridTemplateColumn column3 = new DataGridTemplateColumn();
                        column3.CanUserSort = true;
                        column3.CanUserReorder = true;
                        column3.Header = "ID";
                        column3.CellTemplate = (DataTemplate)this.TryFindResource("DataTemplate2");
                        _dataGrid.Columns.Add(column3);
                    }
                    else if (item.Contains("故障发生日期时间"))
                    {
                        DataGridTemplateColumn column = new DataGridTemplateColumn();
                        column.CanUserSort = true;
                        column.CanUserReorder = true;
                        column.Header = "故障发生日期时间";
                        column.CellTemplate = (DataTemplate)this.TryFindResource("DataTemplate3");
                        _dataGrid.Columns.Add(column);
                    }
                    else if (item.Contains("责任单位"))
                    {
                        DataGridTemplateColumn column = new DataGridTemplateColumn();
                        column.CanUserSort = true;
                        column.CanUserReorder = true;
                        column.Header = "责任单位";

                        column.CellTemplate = (DataTemplate)this.TryFindResource("DataTemplate4");
                        _dataGrid.Columns.Add(column);
                    }
                    else
                    {
                        DataGridTextColumn column = new DataGridTextColumn();
                        column.CanUserSort = true;
                        column.CanUserReorder = true;
                        column.Header = item;
                        column.Binding = new Binding(item);
                        _dataGrid.Columns.Add(column);
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
                ///查询方法
                var dicCaml = this.comS.ZongShuJu();

                if (_isComplete && dicCaml != null)
                {
                    dicCaml.Add("IsFinish", "是,#Text#Eq");
                }

                Thread thread = new Thread(() =>
                {
                    #region 数据源处理

                    var dicLIst = DataOperation.ClientGetDataGridDic(Proxy.ListName, dicCaml);
                    fhFormList.Clear();
                    fhFormListSave.Clear();

                    #endregion


                    this.Dispatcher.BeginInvoke(new System.Action(() =>
                        {
                            dicLIst.ForEach(Item => fhFormList.Add(new FHFormLb(Item)));

                            #region 附件前台设置

                            foreach (var form in fhFormList)
                            {
                                bool isHave = Convert.ToBoolean(form.是否有附件);
                                if (this.SettingFuJiJianImageUriEvent != null)
                                {
                                    this.SettingFuJiJianImageUriEvent(ref _hasFuJianImageUri, ref _noFuJIanImageUri);
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

                                fhFormListSave.Add(form);
                            }
                            #endregion

                            #region 件数、总延时、平均延时设置

                            count故障件数.Text = fhFormListSave.Count.ToString();

                            double sumYanShi = 0.0;

                            double singleYanShi = 0.0;

                            foreach (var item in fhFormListSave)
                            {
                                if (!string.IsNullOrEmpty(item.延时) && Double.TryParse(item.延时, out singleYanShi))
                                {
                                    sumYanShi += singleYanShi;
                                }
                            }
                            sum故障延时.Text = sumYanShi.ToString();
                          
                            if (Double.IsNaN(sumYanShi / fhFormListSave.Count ))
                                平均延时.Text = "0";
                            else
                                平均延时.Text = (sumYanShi / fhFormListSave.Count).ToString("0.00");


                            #endregion

                            #region 数据填充

                            ListCollectionView collection = new ListCollectionView(fhFormList);

                            DataPager1.listInit(this._dataGrid, fhFormList, 20);

                            #endregion

                            this.borTip.Visibility = System.Windows.Visibility.Collapsed;
                        }));

                });
                thread.Start();

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
                   ThreadPool.QueueUserWorkItem((o) =>
 {
     this.Dispatcher.BeginInvoke(new System.Action(() =>
         {
             if (this._dataGrid.Items.Count > 0)
             {
                 PrintDocument print = new PrintDocument();
                 print.ContentRendered += new EventHandler(print_ContentRendered);
                 CommonMethod.FHDataGrid_btn_打印(print, "历史故障---" + DateTime.Now.ToShortDateString(), DataPager1, this._dataGrid, this.fhFormListSave);
             }
         }));
 });
               }));

                TomDisPatcherLb tomdisPatcher2 = new TomDisPatcherLb(new System.Action(() =>
                {
                    this.borTip.Visibility = System.Windows.Visibility.Visible;

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
                this.borTip.Visibility = System.Windows.Visibility.Collapsed;

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
                this.borTip.Visibility = System.Windows.Visibility.Visible;
                ThreadPool.QueueUserWorkItem((o) =>
                {
                    try
                    {
                        if (this._dataGrid.Items.Count > 0)
                        {
                            CommonMethod.FHDataGrid_btn_导入Excel("历史故障---" + DateTime.Now.ToShortDateString(), this._dataGrid, this.fhFormListSave,ref    _excelother);
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
                      this.borTip.Visibility = System.Windows.Visibility.Collapsed;
                  }));
                });

            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btExcel_Click", ex.ToString());
            }
        }

        #endregion

        #endregion
    }
}
