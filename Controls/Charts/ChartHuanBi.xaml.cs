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
using MhczTBG.Controls.TongJiDataGrid;
using System.Data;
using MhczTBG.Controls.ComSearchsDongTai;

namespace MhczTBG.Controls.Charts
{
    /// <summary>
    /// ChartModern3.xaml 的交互逻辑
    /// </summary>
    public partial class ChartHuanBi : UserControl
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
        public List<Dictionary<string, object>> _dicList = null;

        /// <summary>
        /// 是否需要小计
        /// </summary>
        bool _IsNeedXiaoJi = false;

        public ChartHuanBi()
        {
            InitializeComponent();

            //自动显示
            this.scv.dataGrid.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

            this.scv.lineColumn.Visibility = System.Windows.Visibility.Collapsed;
        }

        public void Create(string hengTittle, string TopTittle, ComSearch comSearch)
        {
            #region 通过ID生成存储表单,并显示

            //模拟生成行标题存储区域
            StackPanel stack1 = new StackPanel();
            //模拟生成列标题存储区域
            StackPanel stack2 = new StackPanel();
            //通过遍历添加行标题
            foreach (var item in TongJiItem.TongJiItemList)
            {
                if (item.Tittle == hengTittle)
                {
                    //生成统计子项
                    TongJiItem tongjiItem = new TongJiItem(item.Tittle, item.StrProperty, false);
                    //一级标题字段
                    tongjiItem.ItemChild = item.ItemChild;
                    //二级标题字段
                    tongjiItem.ItemChild2 = item.ItemChild2;
                    //加载统计子项
                    stack1.Children.Add(tongjiItem);
                    break;
                }
            }

            //通过遍历添加列标题
            foreach (var item in TongJiItem.TongJiItemList)
            {
                if (item.Tittle == TopTittle)
                {
                    //生成统计子项
                    TongJiItem tongjiItem = new TongJiItem(item.Tittle, item.StrProperty, false);
                    //一级标题字段
                    tongjiItem.ItemChild = item.ItemChild;
                    //二级标题字段
                    tongjiItem.ItemChild2 = item.ItemChild2;
                    //加载统计子项
                    stack2.Children.Add(tongjiItem);
                    break;
                }
            }
            //生成存储表单
            TitleInit(scv, stack1, stack2, comSearch);

            #endregion
        }

        /// <summary>
        /// 显示加载提示(开始生成表格)
        /// </summary>
        void TitleInit(TJGridView dataGrid, StackPanel stack1, StackPanel stack2, ComSearch comSearch)
        {
            try
            {
                _comSearch = comSearch;

                rowItemTagList.Clear();
                columnItemTagList.Clear();

                //单元格数据
                List<int> intlist = new List<int>();

                DataTable dt = null;
                if (dt == null && _comSearch != null)
                //获取指定时间的数据           
                {
                    //高级查询条件
                    var ss = _comSearch.GetHuanBiShuJu();

                    //已完成
                    ss.Add("IsFinish", "是,#Text#Eq");

                    //获取Dtatable
                    dt = DataOperation.ClientGetDic(Proxy.ListName, ss, ref _dicList);
                }

                #region 生成行标题

                //是否结束
                bool isRowFinish = false;
                //是否开始添加小计
                bool rowaddXiaoJi = false;

                int stackPanelRowCount = 0;

                stackPanelRowCount = stack1.Children.Count;

                //生成行标题
                for (int i = 0; i < stackPanelRowCount; i++)
                {
                    //获取item对象
                    var item = stack1.Children[i] as TongJiItem;

                    //执行最后一次标记完成
                    if (i == stackPanelRowCount - 1) isRowFinish = true;
                    //是否开始添加小计
                    if (i == stackPanelRowCount - 1 && stackPanelRowCount > 1) rowaddXiaoJi = true;

                    //月份统计
                    if (item.Tittle.Equals("月份"))
                    {
                        this.MonthDealWidth(item);
                        RowTittleInit(dataGrid, item.ItemChild, item.StrProperty, false, false);

                        //是否开始添加小计
                        if (i == stackPanelRowCount - 1 && stackPanelRowCount > 0) rowaddXiaoJi = true;

                        RowTittleInit(dataGrid, item.ItemChild2, item.StrProperty, rowaddXiaoJi, isRowFinish);

                        continue;
                    }
                    RowTittleInit(dataGrid, item.ItemChild, item.StrProperty, rowaddXiaoJi, isRowFinish);
                }

                #endregion

                #region 去掉年份

                this.scv.tongjiR.gridMain.ColumnDefinitions[0].Width = new GridLength(0);

                this.scv.tongjiR.Width = this.scv.tongjiR.gridMain.ColumnDefinitions[1].Width.Value;

                this.scv.tongjiR.txtSum.Text = "增量";

                #endregion

                #region 生成列标题

                //是否生成列标题最后一层
                bool isColumnFinish = false;
                //是否开始添加小计
                bool addXiaoJi = false;

                int stackPanleCollumnCount = stack2.Children.Count;

                //生成列标题
                for (int i = 0; i < stackPanleCollumnCount; i++)
                {
                    //获取item对象
                    var item = stack2.Children[i] as TongJiItem;

                    //通过列表名称获取其标题内容（加载最后一行标题标注为完成）
                    if (i == stackPanleCollumnCount - 1) isColumnFinish = true;
                    //是否开始添加小计
                    if (stackPanleCollumnCount > 1 && i == stackPanleCollumnCount - 1) addXiaoJi = true;

                    //月份处理
                    if (item.Tittle.Equals("月份"))
                    {
                        this.MonthDealWidth(item);
                        //列标题添加
                        ColumnTittleInit(dataGrid, item.ItemChild, item.StrProperty, false, false);

                        //是否开始添加小计
                        if (i == stackPanleCollumnCount - 1 && stackPanleCollumnCount > 0) addXiaoJi = true;
                        //列标题添加
                        ColumnTittleInit(dataGrid, item.ItemChild2, item.StrProperty, addXiaoJi, isColumnFinish);

                        continue;
                    }
                    //列标题添加
                    this.ColumnTittleInit(dataGrid, item.ItemChild, item.StrProperty, addXiaoJi, isColumnFinish);
                }

                #endregion

                #region 生成数据

                //数据表格
                System.Data.DataTable table = new System.Data.DataTable();
                //指定有多少列
                for (int i = 0; i < dataGrid.dataGrid.Columns.Count - 1; i++)
                {
                    table.Columns.Add(Convert.ToString(i));
                }

                table.Columns.Add("总计");

                Dictionary<int, object[]> dicData = new Dictionary<int, object[]>();

                if (dt.Columns.Count < 1 || dt.Rows.Count < 1)
                {
                    //给每一行添加数据
                    for (int i = 0; i < dataGrid.tongjiR._LeftCount + 1; i++)
                    {
                        object[] list = new object[dataGrid.dataGrid.Columns.Count];
                        for (int y = 0; y < dataGrid.dataGrid.Columns.Count; y++)
                        {
                            list[y] = 0;
                        }
                        table.Rows.Add(list);
                    }
                }
                else
                {
                    //给每一行添加数据
                    for (int i = 0; i < dataGrid.tongjiR._LeftCount; i++)
                    {
                        var ps = GetData(dataGrid, dt, dataGrid.tongjiR.GetRangeTittle(i), i);
                        table.Rows.Add(ps);
                        dicData.Add(i, ps);
                    }
                    var ds = GetData(dataGrid, dicData);

                    table.Rows.Add(ds);
                }

                //绑定数据源
                dataGrid.dataGrid.ItemsSource = table.DefaultView;

                #endregion

                #region 清理垃圾,释放内存


                #endregion
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TitleInit", ex.ToString(), dataGrid);
            }
        }

        /// <summary>
        /// 行标题添加
        /// </summary>
        /// <param name="itemChild">子项</param>
        /// <param name="itemProperty">内部名称</param>
        /// <param name="addXiaoJi">是否开始添加小计</param>
        /// <param name="isRowFinish">是否完成</param>
        void RowTittleInit(TongJiDataGrid.TJGridView datagrrd, TongJiItemChild itemChild, string itemProperty, bool addXiaoJi, bool isRowFinish)
        {
            try
            {
                //若有小计，去除
                if (itemChild.PropertyList.Contains("小计")) itemChild.PropertyList.Remove("小计");

                //子级关系
                if (itemChild.DicPropertyCaml != null && rowItemTagList.Contains(itemChild.ParentPropertyName))
                {
                    itemChild.IntIndenttitys.Clear();

                    List<string> titleSumList = new List<string>();

                    //遍历进行列标题的添加
                    for (int p = 0; p < itemChild.DicPropertyCaml.Count; p++)
                    {
                        if (itemChild.DicPropertyCaml.Values.ElementAt(p).Contains("小计")) itemChild.DicPropertyCaml.Values.ElementAt(p).Remove("小计");

                        //加一个映射点
                        itemChild.IntIndenttitys.Add(itemChild.DicPropertyCaml.Values.ElementAt(p).Count);

                        //通过列表名称获取其标题内容（加载最后一行标题标注为完成）
                        if (addXiaoJi && _IsNeedXiaoJi)
                        {
                            itemChild.DicPropertyCaml.Values.ElementAt(p).Add("小计");
                            itemChild.IntIndenttitys[p]++;
                        }
                        titleSumList.AddRange(itemChild.DicPropertyCaml.Values.ElementAt(p));
                    }
                    //通过列表名称获取其标题内容
                    datagrrd._RowHeaderAdd(titleSumList, itemChild.IntIndenttitys, isRowFinish);
                }
                else
                {
                    //最后一列有小计
                    if (addXiaoJi && _IsNeedXiaoJi)
                        itemChild.PropertyList.Add("小计");
                    //添加行标题
                    datagrrd._RowHeaderAdd(itemChild.PropertyList, isRowFinish);
                }

                //如SheBeiTypeErJi
                rowItemTagList.Add(itemProperty);

            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "RowTittleInit", ex.ToString(), itemChild, itemProperty, addXiaoJi, isRowFinish);
            }
        }

        /// <summary>
        /// 列标题添加
        /// </summary>
        /// <param name="itemChild">子项</param>
        /// <param name="itemProperty">内部名称</param>
        /// <param name="addXiaoJi">是否开始添加小计</param>
        /// <param name="isRowFinish">是否完成</param>
        void ColumnTittleInit(TJGridView datagrrd, TongJiItemChild itemChild, string itemProperty, bool addXiaoJi, bool isColumnFinish)
        {
            try
            {
                //去除小计
                if (itemChild.PropertyList.Contains("小计"))
                    itemChild.PropertyList.Remove("小计");

                if (itemChild.DicPropertyCaml != null && columnItemTagList.Contains(itemChild.ParentPropertyName))
                {
                    //标题内容
                    List<string> titleSum = new List<string>();

                    //映射数
                    itemChild.IntIndenttitys.Clear();

                    //遍历进行行标题的添加
                    for (int p = 0; p < itemChild.DicPropertyCaml.Count; p++)
                    {

                        if (itemChild.DicPropertyCaml.Values.ElementAt(p).Contains("小计"))
                            itemChild.DicPropertyCaml.Values.ElementAt(p).Remove("小计");
                        //加一个映射点
                        itemChild.IntIndenttitys.Add(itemChild.DicPropertyCaml.Values.ElementAt(p).Count);

                        if (addXiaoJi && _IsNeedXiaoJi)
                        {
                            itemChild.DicPropertyCaml.Values.ElementAt(p).Add("小计");
                            itemChild.IntIndenttitys[p]++;
                        }
                        //添加标题
                        titleSum.AddRange(itemChild.DicPropertyCaml.Values.ElementAt(p));
                    }
                    //通过列表名称获取其标题内容
                    datagrrd._ColumnHeaderAdd(titleSum, itemChild.IntIndenttitys, isColumnFinish);
                }
                else
                {
                    if (addXiaoJi && _IsNeedXiaoJi)
                        itemChild.PropertyList.Add("小计");
                    datagrrd._ColumnHeaderAdd(itemChild.PropertyList, isColumnFinish);
                }
                columnItemTagList.Add(itemProperty);

            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TongJi", ex.ToString(), itemChild, itemProperty, addXiaoJi, isColumnFinish);
            }
        }

        /// <summary>
        /// 处理月份字段
        /// </summary>
        /// <param name="item"></param>
        void MonthDealWidth(TongJiItem item)
        {
            try
            {
                if (_comSearch == null) return;

                ////获取时间控件
                //var startTime = this._comSearch.startEndTime1;
                //获取开始日期
                var starData = this._comSearch.HuanBistartData;
                //获取结束日期
                var endData = this._comSearch.HuanBiEndData;


                //得到相差年份
                var count = (endData.Year - starData.Year) * 12;

                DateTime date = starData;
                //得到相差月份
                count = (endData.Month + 1 - starData.Month) + count;

                item.ItemChild2 = new TongJiItemChild();

                item.ItemChild = new TongJiItemChild();

                item.ItemChild2.ParentPropertyName = "startData,EndData";

                List<string> sumTitleList = new List<string>();
                for (int cou = 0; cou < count; cou++)
                {
                    var realYear = date.AddMonths(cou).ToString("yyyy年");
                    var realMonth = date.AddMonths(cou).ToString("MM月");

                    sumTitleList.Add(realYear);
                    item.ItemChild2.PropertyList.Add(realMonth);
                }

                item.ItemChild.PropertyList.AddRange(_DeleteSameProperty(sumTitleList));

                foreach (var chid in item.ItemChild.PropertyList)
                {
                    item.ItemChild2.DicPropertyCaml.Add(chid, new List<string>());
                    for (int cou = 0; cou < count; cou++)
                    {
                        var realYear = date.AddMonths(cou).ToString("yyyy年");
                        var realMonth = date.AddMonths(cou).ToString("MM月");

                        if (chid.Equals(realYear))
                            item.ItemChild2.DicPropertyCaml[chid].Add(realMonth);
                    }
                }

            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "MonthDealWidth", ex.ToString(), item);
            }
        }

        /// <summary>
        /// 通过指定条件获取单元格数据
        /// </summary>
        /// <param name="striTittleList"></param>
        /// <returns></returns>
        public object[] GetData(TongJiDataGrid.TJGridView dataGridd, System.Data.DataTable dataTable, string[] listRealRow, int rowPosition)
        {
            object[] data = new object[dataGridd.dataGrid.Columns.Count];
            try
            {
                for (int columnPosition = 0; columnPosition < data.Count(); columnPosition++)
                {
                    object count = 0;

                    string expression = string.Empty;

                    #region 获取表达式

                    if (columnPosition < data.Count() - 1)
                    {
                        for (int i = 0; i < listRealRow.Count(); i++)
                        {
                            var item = listRealRow[i];
                            if (item.Contains("月"))
                            {
                                expression += GetMonthExpression(listRealRow[i - 1] + item, i, this.rowItemTagList);
                            }
                            else if (!item.Equals("小计") && !item.Contains("月") && !item.Contains("年"))
                            {
                                expression += this.rowItemTagList[i] + "=" + "'" + item + "'" + " And ";
                            }
                            else if (listRealRow.Contains("小计") && item.Contains("年"))
                            {
                                expression += GetYearExpression(item, i, this.rowItemTagList);
                            }
                        }

                        for (int i = 0; i < dataGridd.tongjiC.GetRangeTittle(columnPosition).Count(); i++)
                        {
                            var range = dataGridd.tongjiC.GetRangeTittle(columnPosition);
                            var item = range[i];

                            if (item.Contains("月"))
                            {
                                expression += GetMonthExpression(dataGridd.tongjiC.GetRangeTittle(columnPosition)[i - 1] + item, i, this.columnItemTagList);
                            }
                            else if (!item.Equals("小计") && !item.Contains("月") && !item.Contains("年"))
                            {
                                expression += this.columnItemTagList[i] + "=" + "'" + item + "'" + " And ";
                            }
                            else if (range.Contains("小计") && item.Contains("年"))
                            {
                                expression += GetYearExpression(item, i, this.columnItemTagList);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < listRealRow.Count(); i++)
                        {
                            var item = listRealRow[i];
                            if (item.Contains("月"))
                            {
                                expression += GetMonthExpression(listRealRow[i - 1] + item, i, this.rowItemTagList);
                            }
                            else if (!item.Equals("小计") && !item.Contains("月") && !item.Contains("年"))
                            {
                                expression += this.rowItemTagList[i] + "=" + "'" + item + "'" + " And ";
                            }
                            else if (listRealRow.Contains("小计") && item.Contains("年"))
                            {
                                expression += GetYearExpression(item, i, this.rowItemTagList);
                            }
                        }
                    }

                    #endregion

                    string realExpression = string.Empty;
                    if (expression.Contains(" And "))
                        realExpression = expression.Substring(0, expression.LastIndexOf(" And "));

                    var result = dataTable.Compute("Count(ID)", realExpression);

                    if (result == DBNull.Value)
                    {
                        count = "0";
                    }
                    else
                    {
                        //统计结果
                        var cc = Convert.ToInt32(result);
                        if (cc == 0) count = "0";
                        else count = cc;
                    }

                    data[columnPosition] = count;
                }

            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "GetData", ex.ToString(), dataTable, listRealRow, rowPosition);
            }
            return data;
        }

        /// <summary>
        /// 获取年的表达式
        /// </summary>
        /// <param name="dicItem"></param>
        /// <param name="tagPosition"></param>
        /// <param name="listTag"></param>
        /// <returns></returns>
        string GetYearExpression(string dicItem, int tagPosition, List<string> listTag)
        {
            string yearExpression = string.Empty;
            try
            {
                string[] ss = listTag[tagPosition].Split(new char[] { ',' });
                var start = Convert.ToDateTime(dicItem);
                var end = start.AddYears(1);
                yearExpression = ss[0] + ">=" + "'" + start + "'" + " And " + ss[0] + "<" + "'" + end + "'" + " And ";

            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "GetYearExpression", ex.ToString(), dicItem, tagPosition, listTag);
            }
            return yearExpression;
        }

        /// <summary>
        /// 获取月份表达式
        /// </summary>
        /// <returns></returns>
        string GetMonthExpression(string dicItem, int tagPosition, List<string> listTag)
        {
            string monthExpression = string.Empty;
            try
            {
                //{[startData, 2013/6/20 5:43:00]}  "yyyy/MM/dd HH:mm:ss"
                string[] ss = listTag[tagPosition].Split(new char[] { ',' });
                var start = Convert.ToDateTime(dicItem);
                var end = start.AddMonths(1);
                monthExpression = ss[0] + ">=" + "'" + start + "'" + " And " + ss[0] + "<" + "'" + end + "'" + " And ";
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "GetMonthExpression", ex.ToString(), dicItem, tagPosition, listTag);
            }
            return monthExpression;
        }

        /// <summary>
        /// 通过指定条件获取单元格数据
        /// </summary>
        /// <param name="striTittleList"></param>
        /// <returns></returns>
        public object[] GetData(TJGridView dataGridd, Dictionary<int, object[]> dicData)
        {
            object[] data = new object[dataGridd.dataGrid.Columns.Count];
            try
            {
                for (int i = 0; i < dicData[0].Count(); i++)
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

        /// <summary>
        /// 删除相同的子项
        /// </summary>
        /// <param name="strList"></param>
        /// <returns></returns>
        List<string> _DeleteSameProperty(List<string> strList)
        {
            List<string> Update_list = new List<string>();
            try
            {
                var ulist = (from li in strList
                             select li).Distinct();
                foreach (var d in ulist)
                {
                    Update_list.Add(d);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "_DeleteSameProperty", ex.ToString(), strList);
            }
            return Update_list;
        }
    }
}
