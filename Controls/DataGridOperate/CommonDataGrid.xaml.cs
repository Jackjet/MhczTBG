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
using System.Collections;
using System.Reflection;
using System.ComponentModel;
using Microsoft.Office.Interop.Word;
using Microsoft.Office.Interop.Excel;
using MhczTBG.Common;
using MhczTBG.Controls.Print;
using Microsoft.Windows.Themes;

namespace MhczTBG.Controls.DataGridOperate
{
    /// <summary>
    /// CommonDataGrid.xaml 的交互逻辑
    /// </summary>
    public partial class CommonDataGrid : UserControl
    {
        #region 变量

        /// <summary>
        /// 临时映射（大标题对小标题）
        /// </summary>
        internal List<int> listYinShe = new List<int>();

        /// <summary>
        /// 行的高度标准
        /// </summary>
        public int ROWHEIGHT = 30;

        /// <summary>
        /// 所有的实体元素
        /// </summary>
        public List<DataGridItemLb> listSource = new List<DataGridItemLb>();

        /// <summary>
        /// 行标识
        /// </summary>
        public List<string> listDataIndent = new List<string>();

        /// <summary>
        /// 基本命名空间
        /// </summary>
        const string XMLNS = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";

        /// <summary>
        /// sdk命名空间
        /// </summary>
        const string XMLNS_SDK = "http://schemas.microsoft.com/winfx/2006/xaml/presentation/sdk";

        /// <summary>
        /// 第一行标题
        /// </summary>
        List<string> listTitle1 = new List<string>();

        /// <summary>
        /// 第二行标题
        /// </summary>
        public List<string> listTitle2 = new List<string>();

        /// <summary>
        ///竖映射数集合
        /// </summary>
        public List<int> listShuYinSheInt = null;

        /// <summary>
        /// 竖映射名称集合
        /// </summary>
        public List<string> listShuYinShe = null;

        //标题一级分割父级集合
        public List<string> listTitle1Indent = new List<string>();

        //是否显示0
        public bool StrZeroToNull = true;

        //统计方式（同期统计，自由统计）
        string strTongJiType = string.Empty;

        /// <summary>
        /// 临时存储数据矩阵
        /// </summary>
        List<List<string>> DoubleList = new List<List<string>>();

        /// <summary>
        /// 单元格生成文本值列监控（第一个列为标题（0），所有默认值为1）
        /// </summary>
        int intCellColumn = 1;

        /// <summary>
        /// 单元格生成文本值行监控
        /// </summary>
        int intCellRow = 0;

        /// <summary>
        /// 临时获取之前的选择单元格控件
        /// </summary>
        CommonDataGridCell leaveLabel = null;

        ///// <summary>
        ///// 默认的字体颜色
        ///// </summary>
        //public Brush beforeBrush = new SolidColorBrush(Colors.Blue);
        ///// <summary>
        ///// 单元格选中之后的字体颜色
        ///// </summary>
        //public Brush selectedBrudh = new SolidColorBrush(Colors.Red);

        /// <summary>
        /// 标题组件集合
        /// </summary>
        List<DataGridHeaderBorder> headerBorderList = new List<DataGridHeaderBorder>();

        /// <summary>
        /// 所选择的的行
        /// </summary>
        System.Windows.Controls.Border selectedItem = null;

        #endregion

        #region 自定义委托事件

        public delegate void ClickGetInfoEventHandle(params string[] info);
        /// <summary>
        /// 当点击之后所激发该事件，能过获取相应的值，并执行额外的方法去展示信息
        /// </summary>
        public event ClickGetInfoEventHandle ClickGetInfoEvent = null;

        #endregion

        #region 构造函数(public)
        /// <summary>
        /// 构造函数
        /// </summary>
        public CommonDataGrid()
        {
            try
            {
                InitializeComponent();

                #region 注册事件

                this.LayoutUpdated += new EventHandler(CommonDataGrid_LayoutUpdated);
                this.Unloaded += new RoutedEventHandler(CommonDataGrid_Unloaded);

                #endregion
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "CommonDataGrid", ex.ToString());
            }
            finally
            {
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dicN">资源字典</param>
        public CommonDataGrid(Dictionary<string, object> dicN)
            : this()
        {
            try
            {
                //生成列表bing
                DataGridInit(dicN);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "CommonDataGrid", ex.ToString(), dicN);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dicN">资源字典</param>
        /// <param name="intTopTitleHeight">标题的高度</param>
        public CommonDataGrid(Dictionary<string, object> dicN, int intTopTitleHeight)
            : this()
        {
            try
            {
                //设置第一行标题的高度
                ROWHEIGHT = intTopTitleHeight;
                //生成列表
                DataGridInit(dicN);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "CommonDataGrid", ex.ToString(), dicN, intTopTitleHeight);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dicN">资源字典</param>
        /// <param name="ZeroToNull">是否去掉0</param>
        public CommonDataGrid(Dictionary<string, object> dicN, bool ZeroToNull)
            : this()
        {
            try
            {
                //是否去零
                StrZeroToNull = ZeroToNull;
                //生成列表
                DataGridInit(dicN);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "CommonDataGrid", ex.ToString(), dicN, ZeroToNull);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dicN">资源字典</param>
        /// <param name="ZeroToNull">是否去掉0</param>
        /// <param name="tongJiType">统计类型</param>
        public CommonDataGrid(Dictionary<string, object> dicN, bool ZeroToNull, string tongJiType)
            : this()
        {
            try
            {
                //是否去零
                StrZeroToNull = ZeroToNull;
                //数据统计类型
                strTongJiType = tongJiType;
                //生成列表
                DataGridInit(dicN);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "CommonDataGrid", ex.ToString(), dicN, ZeroToNull, tongJiType);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dicN">资源字典</param>
        /// <param name="intTopTitleHeight">第一行标题高度</param>
        /// <param name="ZeroToNull">是否去掉0</param>
        /// <param name="tongJiType">统计类型</param>
        public CommonDataGrid(Dictionary<string, object> dicN, int intTopTitleHeight, bool ZeroToNull, string tongJiType)
            : this()
        {
            try
            {
                //设置第一行标题的高度
                ROWHEIGHT = intTopTitleHeight;
                //是否去零
                StrZeroToNull = ZeroToNull;
                //统计类型
                strTongJiType = tongJiType;
                //生成列表
                DataGridInit(dicN);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "CommonDataGrid", ex.ToString(), dicN, intTopTitleHeight, ZeroToNull, tongJiType);
            }
            finally
            {
            }
        }

        #endregion

        #region 生成自由统计表

        /// <summary>
        /// 生成自由统计表
        /// </summary>
        /// <param name="dicN">一定格式的数据</param>
        public void DataGridInit(Dictionary<string, object> dicN)
        {
            try
            {
                Dictionary<string, object> dicNumGet = new Dictionary<string, object>();
                //生成标题不用数据
                for (int i = 1; i < dicN.Count; i++)
                {
                    dicNumGet.Add(dicN.ElementAt(i).Key, dicN.ElementAt(i).Value);
                }

                //设置标题
                Title_Init(dicN);

                //设置数据
                SetData(dicNumGet);

                //添加数据
                ItemsAddRange(listSource);

                //生成左侧标识列
                OneColumnInit(dicN);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DataGridInit", ex.ToString(), dicN);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 生成自由统计表
        /// </summary>
        /// <param name="dicN">一定格式的数据</param>
        public void DataGridInit(Dictionary<string, object> dicN, bool ZeroToNull, string tongJiType)
        {
            try
            {
                //是否去零
                StrZeroToNull = ZeroToNull;
                //数据统计类型
                strTongJiType = tongJiType;

                this.DataGridInit(dicN);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DataGridInit", ex.ToString(), dicN, tongJiType);
            }
            finally
            {
            }
        }

        #endregion

        #region UI事件区域

        /// <summary>
        /// 显示释放没用的资源            
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CommonDataGrid_Unloaded(object sender, RoutedEventArgs e)
        {
            //垃圾回收
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        #endregion

        #region 生成表格

        #region 标题设置
        /// <summary>
        /// 生成一级未处理标题
        /// </summary>
        /// <param name="title">字典集合</param>
        public void Title_Init(Dictionary<string, object> title)
        {
            try
            {
                string strTittles = title.Keys.ElementAt(0) + ";" + title.Values.ElementAt(0);


                if (strTittles.Contains(";"))
                {
                    string[] listPart = strTittles.Split(new char[] { ';' });
                    foreach (var item in listPart)
                    {

                        //两级
                        if (item.Contains("_"))
                        {

                            listTitle1Indent.Add(item.Split(new char[] { '_' })[0]);
                            if (item.Split(new char[] { '_' }).Length > 2)
                            {
                                listTitle2.Add(item.Split(new char[] { '_' })[1] + "_" + item.Split(new char[] { '_' })[2]);
                            }
                            else
                            {
                                listTitle2.Add(item.Split(new char[] { '_' })[1]);
                            }
                        }
                        //”标题“和”总计“
                        else
                        {
                            listTitle1Indent.Add(string.Empty);
                            listTitle2.Add(item);
                        }
                    }
                }
                listTitle1 = SetYingSHe2(listTitle1Indent, out listYinShe);
                TopTitleInit(listTitle1);
                DataGridTitleInit(listTitle2);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Title_Init", ex.ToString(), title);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 生成一级行标题
        /// </summary>
        /// <param name="strFirstTitle">第一个标题（key）</param>
        /// <param name="listTitle">标题集合</param>
        void TopTitleInit(List<string> listTitle)
        {
            try
            {
                gridTitle.Height = ROWHEIGHT;
                //循环添加第一个标题
                for (int i = 0; i < listTitle.Count; i++)
                {
                    string strTitle = listTitle[i];
                    //添加一列
                    this.gridTitle.ColumnDefinitions.Add(new ColumnDefinition());
                    //标题载体
                    System.Windows.Controls.Border border = new System.Windows.Controls.Border()
                    {
                        BorderBrush = new SolidColorBrush(Colors.Gray),
                        BorderThickness = new Thickness(0, 0, 1, 0),
                    };
                    //标题
                    TextBlock txtTittle = new TextBlock()
                    {
                        VerticalAlignment = System.Windows.VerticalAlignment.Center,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                        TextWrapping = TextWrapping.Wrap,
                    };
                    //加载标题
                    border.Child = txtTittle;
                    txtTittle.Text = listTitle[i];
                    border.SetValue(Grid.ColumnProperty, i);
                    this.gridTitle.Children.Add(border);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TopTitleInit", ex.ToString(), listTitle);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 生成一行标题
        /// </summary>
        /// <param name="listTitle">标题集合</param>
        void DataGridTitleInit(List<string> listTitle)
        {
            try
            {
                for (int i = 0; i < listTitle.Count; i++)
                {
                    string strTitle = listTitle[i];

                    string strBinding = string.Format("str{0}", i + 1);

                    if (strTitle.Contains("总计"))
                    {
                        DataGridTextColumn column = new DataGridTextColumn();
                        column.CanUserSort = true;
                        column.CanUserReorder = true;
                        column.Header = strTitle;
                        column.Binding = new Binding(strBinding);
                        column.Foreground = this.Resources["zonJiBrush"] as SolidColorBrush;
                        dataGrid.Columns.Add(column);
                    }
                    else if (strTitle.Contains("标题"))
                    {
                        DataGridTextColumn column = new DataGridTextColumn();
                        column.CanUserSort = true;
                        column.CanUserReorder = true;
                        column.Header = strTitle;
                        column.Binding = new Binding(strBinding);
                        column.Foreground = new SolidColorBrush(Colors.Black);
                        dataGrid.Columns.Add(column);
                    }
                    else
                    {
                        DataGridTextColumn column = new DataGridTextColumn();
                        column.CanUserSort = true;
                        column.CanUserReorder = true;
                        column.Header = strTitle;
                        column.Binding = new Binding(strBinding);
                        column.Foreground = new SolidColorBrush(Colors.Black);
                        dataGrid.Columns.Add(column);
                        column.CellStyle = this.Resources["cellStyle1"] as System.Windows.Style;

                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DataGridTitleInit", ex.ToString(), listTitle);
            }
            finally
            {
            }

        }

        #endregion

        #region 数据设置


        /// <summary>
        /// 生成数据
        /// </summary>
        /// <param name="dicNum">字典集合</param>
        public void SetData(Dictionary<string, object> dicNum)
        {
            try
            {
                //获取实体类的属性信息
                PropertyInfo[] propertyInfoes = typeof(DataGridItemLb).GetProperties();

                //遍历创建实体对象
                foreach (var subItem in dicNum)
                {
                    //创建一个实体对象
                    DataGridItemLb item = new DataGridItemLb();
                    //创建一个属性集合
                    List<string> listData = new List<string>();

                    if (strTongJiType.Contains("同期统计"))
                    {
                        if (subItem.Key.Contains("_"))
                        {
                            if (subItem.Key.Split(new char[] { '_' }).Length > 2)
                            {
                                string str1 = subItem.Key.Split(new char[] { '_' })[2] + "_" + subItem.Key.Split(new char[] { '_' })[1];
                                listData.Add(str1);
                            }
                            else
                            {
                                string str1 = subItem.Key.Split(new char[] { '_' })[1];
                                listData.Add(str1);
                            }
                        }
                        else
                        {
                            listData.Add(subItem.Key);
                        }
                    }
                    else
                    {
                        if (subItem.Key.Contains("_"))
                        {
                            string str1 = subItem.Key.Split(new char[] { '_' })[1];

                            listData.Add(str1);
                        }
                        else
                        {
                            listData.Add(subItem.Key);
                        }
                    }
                    List<string> list = new List<string>();
                    //获取value的值并集中到属性集合里去
                    listData.AddRange(GetOneData(subItem.Value.ToString()));
                    //给item（实体类对象）每一个属性赋值
                    for (int i = 0; i < listData.Count; i++)
                    {
                        var name = propertyInfoes[i].Name;
                        if (!StrZeroToNull)
                        {
                            if (listData[i].Equals("0"))
                            {
                                listData[i] = string.Empty;
                            }
                            propertyInfoes[i].SetValue(item, listData[i], null);
                        }
                        else
                        {
                            propertyInfoes[i].SetValue(item, listData[i], null);
                        }
                        list.Add(listData[i]);
                    }
                    //添加到表格里去
                    //ItemsAdd(item);
                    listSource.Add(item);
                    DoubleList.Add(list);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "SetData", ex.ToString(), dicNum);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="strData">一条记录</param>
        /// <returns>返回一条数据</returns>
        List<string> GetOneData(string strData)
        {

            //返回一条数据
            List<string> listData = new List<string>();
            try
            {
                //数据分割
                string[] NumZ = strData.Split(new char[] { ';' });

                if (strData != null)
                {
                    foreach (var item in NumZ)
                    {
                        listData.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "GetOneData", ex.ToString(), strData);
            }
            finally
            {
            }
            return listData;
        }

        ///// <summary>
        ///// 添加一列
        ///// </summary>
        ///// <param name="obj">实体对象</param>
        //void ItemsAdd(object obj)
        //{
        //    //this.dataGrid.Items.Add(obj);
        //}

        /// <summary>
        /// 添加数据源
        /// </summary>
        /// <param name="list">list集合</param>
        void ItemsAddRange(IList list)
        {
            try
            {
                this.dataGrid.ItemsSource = list;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ItemsAddRange", ex.ToString(), list);
            }
            finally
            {
            }
        }
        #endregion

        #region 左侧一级标识

        /// <summary>
        /// 左侧标识列生成
        /// </summary>
        /// <param name="dicNm">数据</param>
        void OneColumnInit(Dictionary<string, object> dicNm)
        {
            try
            {
                Canvas.SetZIndex(this.gridLeft, 10);
                Canvas.SetZIndex(this.borLeft, 10);

                for (int i = 1; i < dicNm.Count; i++)
                {
                    if (dicNm.ElementAt(i).Key.Contains("_"))
                    {
                        listDataIndent.Add(dicNm.ElementAt(i).Key.Split(new char[] { '_' })[0]);
                    }

                    else
                    {
                        listDataIndent.Add(string.Empty);
                    }
                }
                if (strTongJiType.Contains("同期统计"))
                {
                    listShuYinShe = SetYingSHe2(listDataIndent, out listShuYinSheInt);
                }
                else
                {
                    listShuYinShe = SetYingSHe(listDataIndent, out listShuYinSheInt);
                }
                int j = 0;
                for (int i = 0; i < listShuYinShe.Count; i++)
                {
                    gridLeft.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(listShuYinSheInt[i] * 30) });
                    System.Windows.Controls.Border border = new System.Windows.Controls.Border() { BorderBrush = new SolidColorBrush(Colors.Gray), BorderThickness = new Thickness(0, 0, 1, 1) };
                    TextBlock txt = new TextBlock() { Text = listShuYinShe[i], VerticalAlignment = System.Windows.VerticalAlignment.Center };
                    if (!string.IsNullOrEmpty(txt.Text))
                    {
                        txt.Margin = new Thickness(10, 0, 10, 0);
                    }
                    border.Child = txt;
                    border.SetValue(Grid.RowProperty, j);
                    gridLeft.Children.Add(border);
                    j++;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "OneColumnInit", ex.ToString(), dicNm);
            }
            finally
            {
            }
        }

        #endregion

        #region 映射

        /// <summary>
        /// 设置映射(返回映射目录)
        /// </summary>
        /// <param name="listLinShiTitle1">映射源</param>
        /// <param name="listYin">映射目录对应的数量</param>
        List<string> SetYingSHe(List<string> listLinShiTitle1, out List<int> listYin)
        {
            List<string> list = new List<string>();
            listYin = new List<int>();
            int j = 1;
            for (int i = 0; i < listLinShiTitle1.Count; i++)
            {
                if (i < listLinShiTitle1.Count - 1)
                {
                    if (listLinShiTitle1[i] == listLinShiTitle1[i + 1] && i < listLinShiTitle1.Count - 2)
                    {
                        j++;
                        continue;
                    }
                    else
                    {
                        if ((i - 1) > -1 && (i + 1) < listLinShiTitle1.Count)
                        {
                            if (listLinShiTitle1[i] != listLinShiTitle1[i - 1] && listLinShiTitle1[i] != listLinShiTitle1[i + 1])
                            {
                                listYin.Add(1);
                                list.Add(listLinShiTitle1[i]);


                            }
                            else
                            {
                                listYin.Add(j);
                                j = 1;
                                list.Add(listLinShiTitle1[i]);
                            }
                        }
                        else
                        {
                            if (i == 0)
                            {
                                listYin.Add(1);
                                list.Add(listLinShiTitle1[i]);
                            }
                            else if (i == listLinShiTitle1.Count - 2 && listLinShiTitle1[listLinShiTitle1.Count - 1] == listLinShiTitle1[listLinShiTitle1.Count - 2])
                            {
                                listYin.Add(2);
                                list.Add(listLinShiTitle1[i]);
                            }
                            else
                            {
                                listYin.Add(1);
                                list.Add(listLinShiTitle1[i]);
                                listYin.Add(1);
                                list.Add(listLinShiTitle1[i + 1]);
                            }
                        }
                    }

                }
                else
                {
                    listYin.Add(1);
                    list.Add(listLinShiTitle1[i]);
                }
            }

            return list;
        }

        /// <summary>
        /// 设置映射(返回映射目录)
        /// </summary>
        /// <param name="listLinShiTitle1">映射源</param>
        /// <param name="listYin">映射目录对应的数量</param>
        List<string> SetYingSHe2(List<string> listLinShiTitle1, out List<int> listYin)
        {
            List<string> list = new List<string>();
            listYin = new List<int>();
            int j = 1;
            for (int i = 0; i < listLinShiTitle1.Count; i++)
            {
                if (i < listLinShiTitle1.Count - 1)
                {
                    if (listLinShiTitle1[i] == listLinShiTitle1[i + 1] && i < listLinShiTitle1.Count - 2)
                    {
                        j++;
                        continue;
                    }
                    else
                    {
                        if ((i - 1) > -1 && (i + 1) < listLinShiTitle1.Count)
                        {
                            if (listLinShiTitle1[i] != listLinShiTitle1[i - 1] && listLinShiTitle1[i] != listLinShiTitle1[i + 1])
                            {
                                listYin.Add(1);
                                list.Add(listLinShiTitle1[i]);


                            }
                            else
                            {
                                listYin.Add(j);
                                j = 1;
                                list.Add(listLinShiTitle1[i]);
                            }
                        }
                        else
                        {
                            if (i == 0)
                            {
                                listYin.Add(1);
                                list.Add(listLinShiTitle1[i]);
                            }
                            else if (i == listLinShiTitle1.Count - 2 && listLinShiTitle1[listLinShiTitle1.Count - 1] == listLinShiTitle1[listLinShiTitle1.Count - 2])
                            {
                                listYin.Add(2);
                                list.Add(listLinShiTitle1[i]);
                            }
                            else
                            {
                                listYin.Add(1);
                                list.Add(listLinShiTitle1[i]);
                                listYin.Add(1);
                                list.Add(listLinShiTitle1[i + 1]);
                            }
                        }
                    }

                }
                else
                {
                    if (listLinShiTitle1[i] == listLinShiTitle1[i - 1])
                    {
                        listYin[listYin.Count - 1] = listYin[listYin.Count - 1] + 1;
                    }
                }

            }

            return list;
        }
        #endregion

        #endregion

        #region 导入到Excel
        /// <summary>
        /// 导入到Excel
        /// </summary>
        /// <param name="pageTittle">标题</param>
        public void CommonDataGrid_Excel(string pageTittle)
        {
            try
            {
                int rowDistance = 1;


                //创建excel应用程序
                Microsoft.Office.Interop.Excel.Application excelother = new Microsoft.Office.Interop.Excel.Application();
                //创建一个excel工作簿
                Microsoft.Office.Interop.Excel._Worksheet ws = new Microsoft.Office.Interop.Excel.WorksheetClass();
                //启用
                excelother.Application.Workbooks.Add(true);

                #region 设置页面标题

                ws = (Microsoft.Office.Interop.Excel.Worksheet)excelother.ActiveSheet;
                Microsoft.Office.Interop.Excel.Range rangTittle = null;
                rangTittle = ws.get_Range(ws.Cells[1, 1], ws.Cells[1, this.listTitle1Indent.Count + 1]);
                rangTittle.MergeCells = true;
                excelother.Cells[1, 1] = pageTittle;
                rangTittle.RowHeight = 25;
                rangTittle.Font.Size = 17;
                rangTittle.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;

                rangTittle.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;


                #endregion

                #region 生成数据

                //生成数据
                PropertyInfo[] propertyInes = typeof(DataGridItemLb).GetProperties();
                for (int x = 0; x < this.listTitle1Indent.Count; x++)
                {
                    for (int y = 0; y < this.listSource.Count; y++)
                    {
                        string strText = propertyInes[x].GetValue(this.listSource[y], null).ToString();
                        excelother.Cells[y + 3 + rowDistance, x + 2] = strText;
                    }
                }

                #endregion

                #region 单元格属性设置

                ws = (Microsoft.Office.Interop.Excel.Worksheet)excelother.ActiveSheet;

                string strAllTopTitleColumnHasValue = string.Empty;
                //设置大标题属性
                for (int i = 0; i < this.listTitle1Indent.Count; i++)
                {
                    //设置标题单元格属性              
                    Microsoft.Office.Interop.Excel.Range r = ws.get_Range(ws.Cells[1 + rowDistance, i + 2], ws.Cells[1 + rowDistance, i + 2]);
                    r.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    r.Font.Size = 14;
                    r.RowHeight = 25;
                    r.Font.Bold = true;
                    r.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;

                    strAllTopTitleColumnHasValue += this.listTitle1Indent[i];
                }
                Microsoft.Office.Interop.Excel.Range rOneLeft = ws.get_Range(ws.Cells[2, 1], ws.Cells[2, 1]);
                if (string.IsNullOrEmpty(strAllTopTitleColumnHasValue))
                {
                    rOneLeft.RowHeight = 0;
                }
                else
                {
                    rOneLeft.RowHeight = 25;
                }




                //设置小标题属性
                for (int i = 0; i < this.listTitle2.Count; i++)
                {
                    //设置标题单元格属性                
                    Microsoft.Office.Interop.Excel.Range r = ws.get_Range(ws.Cells[2 + rowDistance, i + 2], ws.Cells[2 + rowDistance, i + 2]);
                    r.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    r.Font.Size = 13;
                    r.ColumnWidth = 15;
                    r.RowHeight = 25;
                    r.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;


                }

                string strAllCoulmHasValue = string.Empty;
                //标识列属性
                for (int j = 0; j < this.listDataIndent.Count; j++)
                {
                    //设置标题单元格属性             
                    Microsoft.Office.Interop.Excel.Range r = ws.get_Range(ws.Cells[j + 2 + rowDistance, 1], ws.Cells[j + 2 + rowDistance, 1]);
                    r.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    r.Font.Size = 14;
                    r.RowHeight = 25;
                    r.Font.Bold = true;
                    r.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                    //左侧最后一个用bordr框起
                    if (j == this.listDataIndent.Count - 1)
                    {
                        r = ws.get_Range(ws.Cells[j + 3 + rowDistance, 1], ws.Cells[j + 3 + rowDistance, 1]);
                        r.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    }

                    strAllCoulmHasValue += listDataIndent[j];
                }


                Microsoft.Office.Interop.Excel.Range rOneTop = ws.get_Range(ws.Cells[1, 1], ws.Cells[1, 1]);
                if (string.IsNullOrEmpty(strAllCoulmHasValue))
                {
                    rOneTop.ColumnWidth = 0;
                }
                else
                {
                    rOneTop.ColumnWidth = 21;
                }


                //数据属性
                for (int x = 0; x < this.listTitle1Indent.Count; x++)
                {
                    for (int y = 0; y < this.listSource.Count; y++)
                    {
                        //设置数据单元格属性                   
                        Microsoft.Office.Interop.Excel.Range r = ws.get_Range(ws.Cells[y + 3 + rowDistance, x + 2], ws.Cells[y + 2 + rowDistance, x + 2]);
                        r.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                        r.RowHeight = 25;
                        r.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                        if (x == 0)
                        {
                            r.Font.Size = 13;
                        }
                    }
                }

                #endregion

                #region Excel标题合并

                int intCellHengCount = 3;
                for (int i = 1; i < this.listYinShe.Count; i++)
                {

                    try
                    {
                        //ws = (Microsoft.Office.Interop.Excel._Worksheet)excelother.ActiveSheet;
                        Microsoft.Office.Interop.Excel.Range r = null;

                        r = ws.get_Range(ws.Cells[1 + rowDistance, intCellHengCount], ws.Cells[1 + rowDistance, intCellHengCount - 1 + this.listYinShe[i]]);     //取得合并的区域  
                        intCellHengCount += this.listYinShe[i];
                        r.MergeCells = true;
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }


                #endregion

                #region 标识列合并

                int intCellShuCount = 3;
                for (int i = 0; i < this.listShuYinSheInt.Count; i++)
                {
                    Microsoft.Office.Interop.Excel.Range r = null;
                    r = ws.get_Range(ws.Cells[intCellShuCount + rowDistance, 1], ws.Cells[intCellShuCount - 1 + this.listShuYinSheInt[i] + rowDistance, 1]);     //取得合并的区域  
                    intCellShuCount += this.listShuYinSheInt[i];
                    r.MergeCells = true;
                }

                #endregion

                //生成大标题
                for (int i = 0; i < this.listTitle1Indent.Count; i++)
                {
                    excelother.Cells[1 + rowDistance, i + 2] = this.listTitle1Indent[i];
                    var d = this.listYinShe;
                }

                //生成小标题
                for (int i = 0; i < this.listTitle2.Count; i++)
                {
                    excelother.Cells[2 + rowDistance, i + 2] = this.listTitle2[i];
                }

                //生成标识列
                for (int j = 0; j < this.listDataIndent.Count; j++)
                {
                    excelother.Cells[j + 3 + rowDistance, 1] = this.listDataIndent[j];
                }

                excelother.Visible = true;

                #region 旧方案（单元格属性设置）

                //((Microsoft.Office.Interop.Excel.Range)ws.Cells[1, 1]).Merge(Missing.Value);   


                //    ((Microsoft.Office.Interop.Excel.Range)ws.Cells[1, 1]).get_Range(ws.Cells[1, 1], ws.Cells[1, 2]);
                //    ((Microsoft.Office.Interop.Excel.Range)ws.Cells[1, 1]).MergeCells = true;

                ////设置粗体
                //((Microsoft.Office.Interop.Excel.Range)ws.Cells[1, 1]).Font.Bold = true;
                ////设置字体大小哦
                //((Microsoft.Office.Interop.Excel.Range)ws.Cells[1, 1]).Font.Size = 14;

                //((Microsoft.Office.Interop.Excel.Range)ws.Cells[2, 1]).Borders.LineStyle = XlLineStyle.xlContinuous;
                //((Microsoft.Office.Interop.Excel.Range)ws.Cells[2, 2]).Borders.LineStyle = XlLineStyle.xlContinuous;



                //////灰色"#808080"; 淡灰色"#DCDCDC"; 暗灰色"#696969";橙色"49407";
                ////((Microsoft.Office.Interop.Excel.Range)ws.Cells[1, 1]).Interior.Color = "#DCDCDC";


                ////单元格居中
                //((Microsoft.Office.Interop.Excel.Range)ws.Cells[1, 1]).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;

                #endregion

            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "CommonDataGrid_Excel", ex.ToString(), pageTittle);
            }
            finally
            {
            }
        }

        #endregion

        #region 打印表格(public)

        //环比图表
        public void CommmonDataGrid_Print(string tittle, Dictionary<string, object> diclist, int topTittleHeight, bool strZeroToNull, string strTongJiType)
        {
            try
            {
                List<CommonDataGrid> commonDataGridList = new List<CommonDataGrid>();
                PrintDocument printDocument = new PrintDocument();

                int intCount = 30;

                int intPage = (int)Math.Ceiling((double)diclist.Count / intCount);

                Dictionary<string, object> listLinShi = new Dictionary<string, object>();


                int j = 0;


                for (int i = 0; i < intPage; i++)
                {
                    listLinShi.Clear();


                    listLinShi.Add(diclist.Keys.ElementAt(0), diclist.Values.ElementAt(0));

                    for (; j < intCount * (i + 1); j++)
                    {
                        if (j < diclist.Count)
                        {
                            if (j == 0) continue;
                            listLinShi.Add(diclist.Keys.ElementAt(j), diclist.Values.ElementAt(j));
                        }
                        else
                        {

                            break;
                        }
                    }

                    commonDataGridList.Add(new CommonDataGrid(listLinShi, topTittleHeight, strZeroToNull, strTongJiType));
                }


                foreach (var commandDataGrid in commonDataGridList)
                {

                    printDocument.Items_Add(tittle, commandDataGrid.bordMain);
                }

                foreach (var item in commonDataGridList)
                {
                    item.dataGrid.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
                    item.dataGrid.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;

                    item.scro.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
                    item.scro.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
                    item.dataGrid.ColumnHeaderStyle = item.gridMain.Resources["headerStyle2"] as System.Windows.Style;
                    item.TitleShou();
                }

                printDocument.Closing += (object sender2, CancelEventArgs e2) =>
                {
                    foreach (var item in commonDataGridList)
                    {
                        item.dataGrid.ColumnHeaderStyle = item.gridMain.Resources["headerStyle1"] as System.Windows.Style;
                        item.TitleOpen();
                    }
                    GC.Collect();
                };
                printDocument.ShowDialog();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "CommmonDataGrid_Print", ex.ToString(), tittle, diclist, topTittleHeight, strZeroToNull, strTongJiType);
            }
            finally
            {
            }
        }

        #endregion

        #region 打印表格（附带图表）

        //public void CommmonDataGrid_Print2(string tittle, Dictionary<string, string> diclist, int topTittleHeight, RwpLb.Controls.Chart chart)
        //{
        //    try
        //    {
        //        List<CommonDataGrid> commonDataGridList = new List<CommonDataGrid>();
        //        PrintDocument printDocument = new PrintDocument();

        //        int intCount = 30;

        //        int intPage = (int)Math.Ceiling((double)diclist.Count / intCount);

        //        Dictionary<string, object> listLinShi = new Dictionary<string, object>();


        //        int j = 0;


        //        for (int i = 0; i < intPage; i++)
        //        {
        //            listLinShi.Clear();


        //            listLinShi.Add(diclist.Keys.ElementAt(0), diclist.Values.ElementAt(0));

        //            for (; j < intCount * (i + 1); j++)
        //            {
        //                if (j < diclist.Count)
        //                {
        //                    if (j == 0) continue;
        //                    listLinShi.Add(diclist.Keys.ElementAt(j), diclist.Values.ElementAt(j));
        //                }
        //                else
        //                {

        //                    break;
        //                }
        //            }

        //            commonDataGridList.Add(new CommonDataGrid(listLinShi, topTittleHeight));
        //        }

        //        RenderTargetBitmap _NewBitmap = RenderVisaulToBitmap(chart, (int)chart.ActualWidth, (int)chart.ActualHeight);
        //        ImageSource imageSource = _NewBitmap;
        //        System.Windows.Controls.Image image = new System.Windows.Controls.Image() { Source = imageSource, Height = chart.ActualHeight, Width = 800 };

        //        System.Windows.Controls.Border border = new System.Windows.Controls.Border();
        //        StackPanel stackPanle = new StackPanel();
        //        if (commonDataGridList.Count == 1)
        //        {
        //            stackPanle.Children.Add(commonDataGridList[0]);
        //            stackPanle.Children.Add(image);
        //        }
        //        border.Child = stackPanle;
        //        printDocument.Items_Add(tittle, stackPanle);

        //        foreach (var item in commonDataGridList)
        //        {
        //            item.dataGrid.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
        //            item.dataGrid.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;

        //            item.scro.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
        //            item.scro.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
        //            item.dataGrid.ColumnHeaderStyle = item.gridMain.Resources["headerStyle2"] as System.Windows.Style;
        //            item.TitleShou();
        //        }

        //        printDocument.Closing += (object sender2, CancelEventArgs e2) =>
        //        {
        //            foreach (var item in commonDataGridList)
        //            {
        //                item.dataGrid.ColumnHeaderStyle = item.gridMain.Resources["headerStyle1"] as System.Windows.Style;
        //                item.TitleOpen();
        //            }
        //            GC.Collect();
        //        };
        //        printDocument.ShowDialog();
        //    }
        //    catch (Exception ex)
        //    {
        //        MethodLb.CreateLog(this.GetType().FullName, "CommmonDataGrid_Print2", ex.ToString(), tittle, diclist, topTittleHeight, chart);
        //    }
        //    finally
        //    {
        //    }
        //}

        #endregion

        #region 辅助方法

        //将控件转为位图
        RenderTargetBitmap RenderVisaulToBitmap(Visual vsual, int width, int height)
        {
            RenderTargetBitmap rtb = null;
            try
            {
                rtb = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Default);
                rtb.Render(vsual);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "RenderVisaulToBitmap", ex.ToString(), vsual, width, height);
            }
            finally
            {
            }
            return rtb;
        }

        #region 调整大标题宽度（生成表格时使用）

        void CommonDataGrid_LayoutUpdated(object sender, EventArgs e)
        {
            try
            {
                int intColumn = 0;
                //有大标题的情况下
                if (listYinShe.Count > 0)
                {
                    //第一行标题
                    for (int i = 0; i < listTitle1.Count; i++)
                    {
                        //获取合并标题的宽度
                        double douWidth = 0;
                        for (int j = 0; j < listYinShe[i]; j++)
                        {
                            //标题宽度叠加
                            douWidth += this.dataGrid.Columns[intColumn].ActualWidth;
                            intColumn++;
                        }
                        //设置合并标题的宽度
                        this.gridTitle.ColumnDefinitions[i].Width = new GridLength(douWidth);
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "CommonDataGrid_LayoutUpdated", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion

        #region 标题宽度改变（打印时使用）

        public void TitleShou()
        {
            try
            {
                dataGrid.Loaded += (object sender, RoutedEventArgs e) =>
                {
                    double tittleColumn = 0;
                    //循环遍历列表的列集合
                    foreach (var column in this.dataGrid.Columns)
                    {
                        //标题宽度设置
                        if (column.Header.ToString().Contains("标题"))
                        {
                            tittleColumn = column.ActualWidth;
                            continue;
                        }

                        //列的宽度（平均分割）
                        column.Width = new DataGridLength((800 - gridLeft.ActualWidth - tittleColumn) / (listTitle2.Count - 1));

                        this.borLeft.Margin = new Thickness(0, 0, 0, -110);
                        this.gridSplitter1.Height = 140;
                        this.gridLeft.Margin = new Thickness(0, 110, 0, 0);
                    }
                };
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TitleShou", ex.ToString());
            }
            finally
            {
            }
        }

        public void TitleOpen()
        {
            try
            {
                //循环遍历列表的列集合
                foreach (var column in this.dataGrid.Columns)
                {
                    if (column.Header.ToString().Contains("标题")) continue;
                    //column.Width = new DataGridLength(column.ActualWidth + 30);

                    this.borLeft.Margin = new Thickness(0, 0, 0, -30);
                    this.gridSplitter1.Height = 60;
                    this.gridLeft.Margin = new Thickness(0, 30, 0, 0);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TitleOpen", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion


        /// <summary>
        /// 点击单元格时所触发的点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Label_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //获取单元格里的元素
                CommonDataGridCell cell = (sender as CommonDataGridCell);
                //有数子才可以进行预览
                if (!string.IsNullOrEmpty(cell.txtName.Text))
                {
                    #region 设置选择之后的样式

                    //若用户第一次点击列表
                    if (leaveLabel != null)
                    {
                        //将其设置为
                        leaveLabel.Foreground = this.Resources["txtColor"] as Brush;
                        //设置选中之后的文本颜色
                        cell.Foreground = this.Resources["txtColorSelect"] as Brush;
                    }
                    else
                    {
                        //设置为选中的颜色
                        cell.Foreground = this.Resources["txtColorSelect"] as Brush;
                    }

                    leaveLabel = cell;

                    #endregion

                    if (ClickGetInfoEvent != null)
                    {
                        ClickGetInfoEvent(cell.TagRowHeader, cell.TagColumnHeader, cell.txtName.Text);
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Label_MouseLeftButtonUp_1", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 单元格加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Label_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //获取初始化的单元格
                CommonDataGridCell cell = sender as CommonDataGridCell;

                //通过循环遍历设置显示的内容和存储相对应的行标题和列标题
                for (; intCellColumn < listTitle2.Count - 1; )
                {
                    //存储一级标题和二级标题
                    cell.TagColumnHeader = listTitle1Indent[intCellColumn] + "_" + listTitle2[intCellColumn];
                    //显示文本值
                    cell.txtName.Text = DoubleList[intCellRow][intCellColumn];
                    //如果不为最后一个值，这里默认-2，因为最后一个值为统计的，所以不算入
                    if (intCellColumn.Equals(listTitle2.Count - 2))
                    {
                        //列检测格式化(之后会有递增，所以设置为0，默认值为1)
                        intCellColumn = 0;
                        //在行检测参数范围之内可以进行递增
                        if (intCellRow < DoubleList.Count - 1)
                        {
                            intCellRow++;
                        }
                        //当达到参数的临界时，格式化行的检测
                        else if (intCellRow == DoubleList.Count - 1)
                        {
                            intCellRow = 0;
                        }
                    }
                    intCellColumn++;
                    break;
                }
                cell.TagRowHeader = DoubleList[intCellRow][0];
                //加载内容
                cell.Foreground = this.Resources["txtColor"] as Brush;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Label_Loaded", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 皮肤更换
        /// </summary>
        /// <param name="color">指定颜色</param>
        public void SkinChange(Color color)
        {
            try
            {
                this.Resources.BeginInit();
                (this.Resources["lin2"] as LinearGradientBrush).GradientStops[1].Color = color;
                (this.Resources["selectedBrush"] as LinearGradientBrush).GradientStops[1].Color = color;
                (this.Resources["linEnter"] as SolidColorBrush).Color = color;
                this.Resources.EndInit();
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

        #region Datagrid样式触发事件

        /// <summary>
        /// 鼠标进入该行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DGR_Border_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                if (sender is System.Windows.Controls.Border && sender != selectedItem)
                {
                    var border = sender as System.Windows.Controls.Border;
                    border.Background = this.Resources["linEnter"] as Brush;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DGR_Border_MouseEnter", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 鼠标离开该行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DGR_Border_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                if (sender is System.Windows.Controls.Border && sender != selectedItem)
                {
                    var border = sender as System.Windows.Controls.Border;
                    border.Background = this.Resources["translateBrush"] as Brush;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DGR_Border_MouseLeave", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 选择一行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DGR_Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (selectedItem != null) selectedItem.Background = this.Resources["translateBrush"] as Brush;

                if (sender is System.Windows.Controls.Border)
                {
                    var border = sender as System.Windows.Controls.Border;
                    border.Background = this.Resources["selectedBrush"] as Brush;

                    this.selectedItem = border;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DGR_Border_MouseLeftButtonDown", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
      
        /// <summary>
        /// 获取所有标题组件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridHeaderBorder_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is DataGridHeaderBorder)
                {
                    (sender as DataGridHeaderBorder).Background = this.Resources["lin2"] as Brush;
                    headerBorderList.Add(sender as DataGridHeaderBorder);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DataGridHeaderBorder_Loaded", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        
        #endregion
       
    }
}
