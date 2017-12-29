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
    /// Column.xaml 的交互逻辑
    /// </summary>
    public partial class TJColumn : UserControl
    {
        #region 变量

        /// <summary>
        /// 最小列高度
        /// </summary>
        public double _ColumnCellHeight = 25;

        //最后一层的数量
        public int _LeftCount = 1;

        /// <summary>
        /// 标题层存储区域(具体每一层标题区域的标题内容)
        /// </summary>
        Dictionary<int, List<string>> _dicTittleList = new Dictionary<int, List<string>>();

        /// <summary>
        /// 所有标题文本集
        /// </summary> 
        public readonly Dictionary<int, List<string>> _DicTittleTextList = new Dictionary<int, List<string>>();

        /// <summary>
        ///所有乘阶数
        /// </summary>
        Dictionary<int, ChenJieEntity> _dicLeftCC = new Dictionary<int, ChenJieEntity>();

        /// <summary>
        /// 存储标题区域
        /// </summary>
        public Dictionary<int, List<ColumnDefinition>> _dicColumnDefintionList = new Dictionary<int, List<ColumnDefinition>>();

        /// <summary>
        /// 标题集合
        /// </summary>
        public Dictionary<int, List<TextBlock>> _dicTextList = new Dictionary<int, List<TextBlock>>();

        /// <summary>
        /// 层的偏移
        /// </summary>
        int _rowColumnPosition = 0;

        /// <summary>
        /// 一个标题的文字数量
        /// </summary>
        public double _SumCount = 0;

        /// <summary>
        /// 一个字符串所占宽度
        /// </summary>
        public double _oneStringWidth = 15;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public TJColumn()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TJColumn", ex.ToString());
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
                _DicTittleTextList.Clear();
                _dicLeftCC.Clear();
                _dicColumnDefintionList.Clear();
                _rowColumnPosition = 0;
                gridMain.RowDefinitions.Clear();
                gridMain.Children.Clear();
                _dicTextList.Clear();
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
                RowDefinition rowDefinition = new RowDefinition();
                //加载列
                this.gridMain.RowDefinitions.Add(rowDefinition);
                //标题区域承载容器
                Grid gridColumn = new Grid();
                //设置列位置
                Grid.SetRow(gridColumn, this.gridMain.RowDefinitions.Count - 1);
                //加载标题区域
                this.gridMain.Children.Add(gridColumn);

                //添加一个存储区
                _dicTittleList.Add(_rowColumnPosition, new List<string>());
                //添加乘阶数
                _dicLeftCC.Add(_rowColumnPosition, new ChenJieEntity());
                //添加控件存储
                _dicColumnDefintionList.Add(_rowColumnPosition, new List<ColumnDefinition>());

                _dicTextList.Add(_rowColumnPosition, new List<TextBlock>());
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
                        ColumnDefinition columnDefinition = new ColumnDefinition();
                        //加载列
                        gridColumn.ColumnDefinitions.Add(columnDefinition);
                        //添加层
                        _dicColumnDefintionList[_rowColumnPosition].Add(columnDefinition);
                        //标题承载控件
                        Border border = new Border() { };

                        //标题文本
                        TextBlock txt = new TextBlock() { Text = item, TextWrapping = TextWrapping.Wrap };
                        _dicTextList[_rowColumnPosition].Add(txt);

                        //绑定文本,生成文本集所需使用
                        columnDefinition.Tag = item;

                        if (txt.Text.Equals("小计")) txt.Foreground = new SolidColorBrush(Colors.Red);

                        var sezCount = 0;
                        if (isFinish)
                        {
                            sezCount = txt.Text.Length;
                            if (sezCount > _SumCount)
                                _SumCount = sezCount;
                        }

                        //文本加载
                        border.Child = txt;
                        //设置相应标题的列位置
                        Grid.SetColumn(border, gridColumn.ColumnDefinitions.Count - 1);
                        //标题加载
                        gridColumn.Children.Add(border);
                        //获取文字数量
                        int count = item.Count();

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

                //左乘阶递增
                _LeftCount = p;

                //区域递增
                _rowColumnPosition++;

                if (isFinish)
                {
                    this.Height = (_rowColumnPosition) * _ColumnCellHeight;
                    foreach (var item in this._dicColumnDefintionList[this._dicColumnDefintionList.Count - 1])
                    {
                        item.Width = new GridLength(this._ColumnCellHeight);
                    }

                    _MesaureBegin();
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ItemsAdd", ex.ToString(), tittleList, lll, isFinish);
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
                        double[] widthArray = new double[this._dicColumnDefintionList[i].Count];

                        int top = 1;

                        double width = 0;

                        for (int p = 0; p < this._dicColumnDefintionList[i + 1].Count; p++)
                        {
                            width += this._dicColumnDefintionList[i + 1][p].Width.Value;

                            if (top == (p + 1) / this._dicLeftCC[i + 1].LeftCC[0])
                            {
                                widthArray[top - 1] = width;
                                width = 0;
                                top++;
                            }
                        }
                        for (int j = 0; j < this._dicColumnDefintionList[i].Count; j++)
                        {
                            this._dicColumnDefintionList[i][j].Width = new GridLength(widthArray[j]);
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

                        for (int d = 0; d < this._dicColumnDefintionList[i + 1].Count / this._dicLeftCC[i + 1].Sumn; d++)
                        {
                            for (int p = 0; p < this._dicLeftCC[i + 1].LeftCC.Count; p++)
                            {
                                count += this._dicLeftCC[i + 1].LeftCC[p];
                                intList.Add(count);
                            }
                        }

                        int pp = 0;
                        double sss = 0;

                        for (int j = 0; j < this._dicColumnDefintionList[i + 1].Count; j++)
                        {
                            sss += this._dicColumnDefintionList[i + 1][j].Width.Value;
                            if (j + 1 == intList[pp])
                            {
                                doubelList.Add(sss);
                                sss = 0;
                                pp++;
                            }
                        }

                        for (int j = 0; j < this._dicColumnDefintionList[i].Count; j++)
                        {
                            this._dicColumnDefintionList[i][j].Width = new GridLength(doubelList[j]);
                        }
                    }
                }

                if (this._DicTittleTextList.Count == 0)
                {
                    TittleIndentWidthCollect();
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "_MesaureBegin", ex.ToString());
            }
        }

        /// <summary>
        /// 列标题集合
        /// </summary>
        public void TittleIndentWidthCollect()
        {
            try
            {
                //生成标题集合
                foreach (var item in this._dicColumnDefintionList)
                {
                    this._DicTittleTextList.Add(item.Key, new List<string>());
                    foreach (var tt in item.Value)
                    {
                        for (int i = 0; i < tt.Width.Value / this._ColumnCellHeight; i++)
                        {
                            this._DicTittleTextList[item.Key].Add(tt.Tag.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TittleIndentWidthCollect", ex.ToString());
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
                    if (position < item.Value.Count)
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
