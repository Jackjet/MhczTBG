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


namespace MhczTBG.Controls.TongJiDataGrid
{
    /// <summary>
    /// Row.xaml 的交互逻辑
    /// </summary>
    public partial class TJRow : UserControl
    {
        #region 变量

        /// <summary>
        /// 最小列高度
        /// </summary>
        public double _RowCellHeight = 25;

        //最后一层的数量
        public int _LeftCount = 1;

        /// <summary>
        /// 标题层存储区域(具体每一层标题区域的标题内容)
        /// </summary>
        Dictionary<int, List<string>> _dicTittleList = new Dictionary<int, List<string>>();

        /// <summary>
        /// 每一层标题区域的宽度（具体每一层标题区域的宽度）
        /// </summary>
        Dictionary<int, double> _dicTittleWidthList = new Dictionary<int, double>();

        /// <summary>
        /// 所有标题文本集
        /// </summary>
        public Dictionary<int, List<string>> _DicTittleTextList = new Dictionary<int, List<string>>();

        /// <summary>
        ///所有乘阶数
        /// </summary>
        Dictionary<int, ChenJieEntity> _dicLeftCC = new Dictionary<int, ChenJieEntity>();

        /// <summary>
        /// 存储标题区域
        /// </summary>
        Dictionary<int, List<RowDefinition>> _dicRowDefintionList = new Dictionary<int, List<RowDefinition>>();

        /// <summary>
        /// 一个标题的文字数量
        /// </summary>
        double _SumCount = 0;

        /// <summary>
        /// 一个字符串所占宽度
        /// </summary>
        double _oneStringWidth = 20;

        /// <summary>
        /// 层的偏移
        /// </summary>
        int _rowColumnPosition = 0;

        #endregion

        #region 构造函数

        public TJRow()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TJRow", ex.ToString());
            }
        }

        #endregion

        #region 恢复默认设置

        /// <summary>
        /// 恢复默认属性
        /// </summary>
        public void Reset()
        {
            try
            {
                _LeftCount = 1;
                _dicTittleList.Clear();
                _dicTittleWidthList.Clear();
                _DicTittleTextList.Clear();
                _dicLeftCC.Clear();
                _dicRowDefintionList.Clear();
                _SumCount = 0;
                _rowColumnPosition = 0;

                gridMain.ColumnDefinitions.Clear();
                gridMain.Children.Clear();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Reset", ex.ToString());
            }
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 添加子项
        /// </summary>
        /// <param name="tittleList"></param>
        public void ItemsAdd(List<string> tittleList, List<int> lll, bool isFinish)
        {
            try
            {
                //创建一列
                ColumnDefinition columnDefinition = new ColumnDefinition();
                //加载列
                this.gridMain.ColumnDefinitions.Add(columnDefinition);
                //标题区域承载容器
                Grid gridColumn = new Grid();
                //设置列位置
                Grid.SetColumn(gridColumn, this.gridMain.ColumnDefinitions.Count - 1);
                //加载标题区域
                this.gridMain.Children.Add(gridColumn);

                //添加一个存储区
                _dicTittleList.Add(_rowColumnPosition, new List<string>());
                //添加乘阶数
                _dicLeftCC.Add(_rowColumnPosition, new ChenJieEntity());
                //添加控件存储
                _dicRowDefintionList.Add(_rowColumnPosition, new List<RowDefinition>());
                int p = 0;
                var ct = 0;
                if (lll == null)
                    ct = _LeftCount;
                else
                {
                    _dicLeftCC[_rowColumnPosition - 1].isParent = true;

                    ct = 1;
                    foreach (var item in _dicLeftCC.Values)
                    {
                        if (!item.isParent && item.LeftCC.Count > 0)
                            ct = ct * item.Sumn;
                    }
                }

                for (int i = 0; i < ct; i++)
                {
                    //标题区域填充标题
                    foreach (var item in tittleList)
                    {
                        //创建一行
                        RowDefinition rowDefintion = new RowDefinition();
                        //加载列
                        gridColumn.RowDefinitions.Add(rowDefintion);
                        //添加层
                        _dicRowDefintionList[_rowColumnPosition].Add(rowDefintion);
                        //标题承载控件
                        Border border = new Border() { };
                        //高度设置
                        rowDefintion.Height = new GridLength(this._RowCellHeight);
                        //标题文本
                        TextBlock txt = new TextBlock() { Text = item };
                        //绑定文本,生成文本集所需使用
                        rowDefintion.Tag = item;

                        if (txt.Text.Equals("小计")) txt.Foreground = new SolidColorBrush(Colors.Red);

                        //文本加载
                        border.Child = txt;
                        //设置相应标题的列位置
                        Grid.SetRow(border, gridColumn.RowDefinitions.Count - 1);
                        //标题加载
                        gridColumn.Children.Add(border);
                        //获取文字数量
                        int count = item.Count();

                        //两者取数量最多的作为衡量宽度的标准
                        if (count > _SumCount) _SumCount = count;
                        //设置宽度(一个标题区域的宽度）
                        columnDefinition.Width = new GridLength(_SumCount * _oneStringWidth);
                        //添加标题
                        _dicTittleList[_rowColumnPosition].Add(txt.Text);
                        p++;
                    }
                }

                if (lll == null)
                    //设置乘阶数
                    _dicLeftCC[_rowColumnPosition].LeftCC.Add(tittleList.Count);
                else
                    //设置乘阶数
                    _dicLeftCC[_rowColumnPosition].LeftCC.AddRange(lll);

                _dicLeftCC[_rowColumnPosition].Sumn = tittleList.Count;
                //存储每一标题区域的宽度
                _dicTittleWidthList.Add(_rowColumnPosition, columnDefinition.Width.Value);

                //左乘阶递增
                _LeftCount = p;

                //区域递增
                _rowColumnPosition++;
                //归零
                _SumCount = 0;
                if (isFinish)
                {
                    // 宽度设置
                    _WidthSetting();
                    _MesaureBegin();
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ItemsAdd", ex.ToString(), tittleList, lll, isFinish);
            }
        }


        /// <summary>
        /// 宽度设置
        /// </summary>
        void _WidthSetting()
        {
            try
            {
                //宽度设置
                double width = 0;
                this._dicTittleWidthList.Values.ToList().ForEach(Item => width += Item);
                this.Width = width;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "_WidthSetting", ex.ToString());
            }
        }

        /// <summary>
        /// 调整布局
        /// </summary>
        public void _MesaureBegin()
        {
            try
            {
                //遍历（从倒数第二层开始）
                for (int i = this._dicLeftCC.Count - 2; i > -1; i--)
                {
                    //嵌套关系
                    if (this._dicLeftCC[i + 1].LeftCC.Count == 1)
                    {
                        double height = 0;
                        for (int p = 0; p < this._dicLeftCC[i + 1].LeftCC[0]; p++)
                        {
                            height += this._dicRowDefintionList[i + 1][p].Height.Value;
                        }
                        for (int j = 0; j < this._dicRowDefintionList[i].Count; j++)
                        {
                            this._dicRowDefintionList[i][j].Height = new GridLength(height);
                        }
                    }
                    //父子级关系
                    else
                    {
                        //叠加后一层的模式宽度集
                        List<double> doubelList = new List<double>();

                        var count = 0;

                        //叠加集合
                        List<int> intList = new List<int>();

                        for (int p = 0; p < this._dicLeftCC[i + 1].LeftCC.Count; p++)
                        {
                            count += this._dicLeftCC[i + 1].LeftCC[p];
                            intList.Add(count);
                        }

                        double sss = 0;
                        int kt = 0;
                        for (int t = 0; t < count; t++)
                        {
                            sss += this._dicRowDefintionList[i + 1][t].Height.Value;
                            if (t + 1 == intList[kt])
                            {
                                doubelList.Add(sss);
                                sss = 0;
                                kt++;
                                if (kt >= intList.Count) break;
                            }
                        }

                        var cccleft = 0;
                        for (int j = 0; j < this._dicRowDefintionList[i].Count; j++)
                        {
                            if (cccleft == this._dicLeftCC[i + 1].LeftCC.Count) cccleft = 0;
                            this._dicRowDefintionList[i][j].Height = new GridLength(doubelList[cccleft]);
                            cccleft++;
                        }
                    }
                }

                if (_DicTittleTextList.Count == 0)
                {
                    //生成标题集
                    foreach (var item in this._dicRowDefintionList)
                    {
                        this._DicTittleTextList.Add(item.Key, new List<string>());
                        foreach (var tt in item.Value)
                        {
                            for (int i = 0; i < tt.Height.Value / this._RowCellHeight; i++)
                            {
                                this._DicTittleTextList[item.Key].Add(tt.Tag.ToString());
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "_MesaureBegin", ex.ToString());
            }
        }
        /// <summary>
        /// 获取某行的标题
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public string[] GetRangeTittle(int position)
        {
            string[] strTittleArray = new string[this._DicTittleTextList.Count];
            try
            {
                foreach (var item in this._DicTittleTextList)
                {
                    strTittleArray[item.Key] = item.Value[position];
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "GetRangeTittle", ex.ToString(), position);
            }
            return strTittleArray;
        }

        #endregion
    }
}
