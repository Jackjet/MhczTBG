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
using MhczTBG.Controls.ComSearchsDongTai;
using MhczTBG.Common;
using System.Data;
using MhczTBG.Controls.TongJiDataGrid;
using System.Threading;

namespace MhczTBG.Controls.Charts
{
    /// <summary>
    /// ChartTongQi.xaml 的交互逻辑
    /// </summary>
    public partial class ChartTongQi : UserControl
    {
        /// <summary>
        /// 行标题内部名称集
        /// </summary>
        List<string> rowItemTagList = new List<string>();

        /// <summary>
        /// 列标题内部名称集
        /// </summary>
        List<string> columnItemTagList = new List<string>();

        /// <summary>
        /// 高级查询
        /// </summary>
        ComSearch _comSearch = null;

        /// <summary>
        /// dic数据源
        /// </summary>
        public List<Dictionary<string, object>> _dicList1 = null;

        /// <summary>
        /// dic数据源
        /// </summary>
        public List<Dictionary<string, object>> _dicList2 = null;

        #region 自定义事件委托

        public delegate void ListCompleteEventHandle();
        /// <summary>
        /// 列表是否生成完毕
        /// </summary>
        public event ListCompleteEventHandle _ListCompleteEvent = null;

        #endregion

        public ChartTongQi()
        {
            try
            {
                InitializeComponent();

                this.Resources.MergedDictionaries.Add(StyleResource.MyStyle.Instacnce.DataGridResourcesGrey);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ChartTongQi", ex.ToString());
            }
            finally
            {
            }
        }

        public void ParametersInit(ComSearch coms)
        {
            try
            {
                this.dataGridList.ItemsSource = null;

                this._comSearch = coms;

                var ss = this._comSearch.GetTongQiShuJu1();

                var ss2 = this._comSearch.GetTongQiShuJu2();

                Thread thread2 = new Thread(new ThreadStart(() =>
                         {
                             DataTable datable = new DataTable();

                             //已完成
                             ss.Add("IsFinish", "是,#Text#Eq");

                             //获取Dtatable
                             var dt = DataOperation.ClientGetDicByPropertyName(Proxy.ListName, ss, "startData", ref _dicList1);

                             //已完成
                             ss2.Add("IsFinish", "是,#Text#Eq");
                             //获取Dtatable
                             var dt2 = DataOperation.ClientGetDicByPropertyName(Proxy.ListName, ss2, "startData", ref _dicList2);

                             this.Dispatcher.BeginInvoke(new Action(() =>
                                 {
                                     #region 生成进行中

                                     if (dt.Columns.Count > 0)
                                     {
                                         datable = dt;
                                         foreach (var item in dt2.Rows)
                                             datable.Rows.Add((item as DataRow).ItemArray);
                                     }
                                     else if (dt2.Columns.Count > 0)
                                     {
                                         datable = dt2;
                                         foreach (var item in dt.Rows)
                                             datable.Rows.Add((item as DataRow).ItemArray);
                                     }

                                     Dictionary<int, object[]> objList = new Dictionary<int, object[]>();

                                     DataTable dtMain = new DataTable();

                                     this.dataGridList.ItemsSource = dtMain.DefaultView;

                                     for (int i = 0; i < 14; i++)
                                     {
                                         if (i == 0)
                                             dtMain.Columns.Add("年份");
                                         else if (i == 13)
                                             dtMain.Columns.Add("总计");

                                         else
                                             dtMain.Columns.Add(i + "月");
                                     }
                                     string year1 = this._comSearch.cmbTongQiYear1.SelectedItem.ToString();
                                     if (_dicList1.Count > 0)
                                     {
                                         var year1Data = GetData(datable, year1);
                                         dtMain.Rows.Add(year1Data);
                                         objList[0] = year1Data;
                                     }
                                     else
                                     {
                                         object[] intlist = new object[14];
                                         for (int i = 0; i < 14; i++)
                                         {
                                             if (i == 0)
                                                 intlist[i] = year1;
                                             else
                                                 intlist[i] = 0;
                                         }
                                         dtMain.Rows.Add(intlist);
                                         objList[0] = intlist;
                                     }

                                     string year2 = this._comSearch.cmbTongQiYear2.SelectedItem.ToString();

                                     if (_dicList2.Count > 0)
                                     {
                                         var year2Data = GetData(datable, year2);
                                         dtMain.Rows.Add(year2Data);
                                         objList[1] = year2Data;
                                     }
                                     else
                                     {
                                         object[] intlist = new object[14];
                                         for (int i = 0; i < 14; i++)
                                         {
                                             if (i == 0)
                                                 intlist[i] = year2;
                                             else
                                                 intlist[i] = 0;
                                         }
                                         dtMain.Rows.Add(intlist);

                                         objList[1] = intlist;
                                     }

                                       #region 增量
                                     var AddCC = this.GetData(this.dataGridList, objList);
                                     dtMain.Rows.Add(AddCC);


                                     //this.dataGridList.ItemsSource = dtMain.DefaultView;
                                    
                                     #endregion
                                   
                                     #endregion

                                     if (_ListCompleteEvent != null)
                                         _ListCompleteEvent();
                                 }));
                         }));
                thread2.Start();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ParametersInit", ex.ToString(), coms);
            }
            finally
            {
            }
        }

        object[] GetData(DataTable dt, string year)
        {
            object[] datas = new object[14];
            try
            {
                string expression = string.Empty;
                datas[0] = year;

                for (int i = 1; i <= 12; i++)
                {
                    var startData = Convert.ToDateTime(year + i + "月");

                    var endData = Convert.ToDateTime(year + i + "月").AddMonths(1).AddSeconds(-1);

                    expression = "startData" + ">=" + "'" + startData + "'" + " And " + "startData" + "<" + "'" + endData + "'";

                    var count = dt.Compute("Count(ID)", expression);

                    int realCount = 0;

                    int.TryParse(Convert.ToString(count), out realCount);

                    datas[i] = realCount;
                }
                var Sumn = 0;
                foreach (var item in datas)
                {
                    var ccCount = 0;

                    if (int.TryParse(Convert.ToString(item), out ccCount))
                    {
                        Sumn += ccCount;
                    }
                }
                datas[datas.Count() - 1] = Sumn;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "GetData", ex.ToString(), dt, year);
            }
            finally
            {
            }
            return datas;
        }

        /// <summary>
        /// 通过指定条件获取单元格数据
        /// </summary>
        /// <param name="striTittleList"></param>
        /// <returns></returns>
        public object[] GetData(DataGrid dataGridd, Dictionary<int, object[]> dicData)
        {
            object[] data = new object[dataGridd.Columns.Count];
            try
            {
                data[0] = "增量";

                for (int i = 1; i < dicData[0].Count(); i++)
                {
                    int count1 = 0;
                    int count2 = 0;

                    int.TryParse(Convert.ToString(dicData[0][i]), out count1);

                    int.TryParse(Convert.ToString(dicData[1][i]), out count2);

                    data[i] = count2 - count1;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TongJi", ex.ToString(), dicData);
            }
            return data;
        }
    }
}
