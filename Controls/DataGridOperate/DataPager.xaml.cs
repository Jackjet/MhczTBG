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
using System.Windows.Threading;
using System.Threading;
using MhczTBG.Common;

namespace MhczTBG.Controls.DataGridOperate
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class DataPager : UserControl
    {
        #region 变量

        int intPageSize = 3;
        /// <summary>
        /// 一页所显示数据的条数
        /// </summary>
        public int IntPageSize
        {
            get { return intPageSize; }
            set { intPageSize = value; }
        }


        /// <summary>
        /// 数据集合视图
        /// </summary>
        ListCollectionView Collection = null;

        /// <summary>
        /// 数据源
        /// </summary>
        public ArrayList list = new ArrayList();

        /// <summary>
        /// 当前页
        /// </summary>
        int intPageNow = 1;

        /// <summary>
        /// 总页数
        /// </summary>
        public int intPageCount;


        #endregion

        #region 构造函数

        public DataPager()
        {
            try
            {
                InitializeComponent();

                #region 注册事件

                this.btnUp.Click += new RoutedEventHandler(btnUp_Click);
                this.btnNext.Click += new RoutedEventHandler(btnNext_Click);
                this.btnFirst.Click += new RoutedEventHandler(btnFirst_Click);
                this.btnLast.Click += new RoutedEventHandler(btnLast_Click);

                #endregion
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DataPager", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion

        #region 事件区域

        /// <summary>
        /// 最后一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnLast_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Last();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btnLast_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 第一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnFirst_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                First();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btnFirst_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 上一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Up();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btnUp_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 下一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnNext_Click(object sender, RoutedEventArgs e)
        {
              try
            {
            Next();
  }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btnNext_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion

        #region 数据集合视图初始化
        /// <summary>
        /// 数据集合视图初始化
        /// </summary>
        /// <param name="datagrid">DataGrid</param>
        /// <param name="lists">数据</param>
        /// <param name="pageSize">一页所显示的数据条数</param>
        public void listInit(DataGrid datagrid, IList lists, int pageSize)
        {
            try
            {
                this.IntPageSize = pageSize;
                ListCollectionView coll = new ListCollectionView(lists);
                this.Collection = coll;

                if (Collection != null)
                {

                    #region datapager初始化
                    //当前页
                    this.intPageNow = 1;
                    //总页数
                    this.intPageCount = 0;
                    //数据源
                    this.list.Clear();
                    //总页数数量显示
                    this.txtCount.Text = "1";
                    //当前页显示
                    this.txtNow.Text = "1";

                    #endregion
                    //填充数据源
                    for (int i = 0; i < Collection.Count; i++)
                    {
                        list.Add(Collection.GetItemAt(i));
                    }

                    //清除分配数据
                    ClearAll();

                    //如果分页的数量比总数量小的话，直接传分页的数量
                    if (IntPageSize < list.Count)
                    {
                        for (int i = 0; i < IntPageSize; i++)
                        {
                            Collection.AddNewItem(list[i]);
                        }
                    }
                    //如果分页的数量比总页大的话，直接传总页的数量
                    else
                    {
                        for (int i = 0; i < list.Count; i++)
                        {

                            Collection.AddNewItem(list[i]);
                        }
                    }

                    #region 设置信息显示（总页数，当前页位置）

                    //获取总页数
                    intPageCount = (int)Math.Ceiling((double)list.Count / IntPageSize);

                    this.txtCount.Text = intPageCount.ToString();
                    //当前页
                    this.txtNow.Text = intPageNow.ToString();

                    #endregion

                    Collection.AddNewItem(null);
                    Collection.CancelNew();
                }
                datagrid.ItemsSource = coll;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "listInit", ex.ToString());
            }
            finally
            {
            }
        }
        #endregion

        #region 导航方法

        #region 第一页

        void First()
        {
            try
            {
                //总页数必须大于1，翻页才有效
                if (this.intPageCount > 1)
                {
                    //清除分配数据
                    ClearAll();
                    //当前页为1
                    intPageNow = 1;
                    //当前页显示
                    this.txtNow.Text = intPageNow.ToString();

                    //给分配数据填充
                    for (int i = (intPageNow - 1) * IntPageSize; i < IntPageSize * intPageNow; i++)
                    {
                        if (i < list.Count)
                        {
                            Collection.AddNewItem(list[i]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "First", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion

        #region 上一页
        /// <summary>
        /// 上一页
        /// </summary>
        void Up()
        {
            try
            {
                //总页数必须大于1，翻页才有效 ，当前页需大于1
                if (this.intPageCount > 1 && intPageNow > 1)
                {
                    //清除分配数据
                    ClearAll();
                    //当前页去1
                    intPageNow -= 1;
                    //当前页显示
                    this.txtNow.Text = intPageNow.ToString();
                    //给分配数据填充
                    for (int i = (intPageNow - 1) * IntPageSize; i < IntPageSize * intPageNow; i++)
                    {
                        if (i < list.Count)
                        {
                            Collection.AddNewItem(list[i]);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Up", ex.ToString());
            }
            finally
            {
            }
        }
        #endregion

        #region 下一页

        /// <summary>
        /// 下一页
        /// </summary>
        void Next()
        {
            try
            {
                //总页数必须大于1，翻页才有效 ，当前页需小于总页数
                if (this.intPageCount > 1 && intPageNow < intPageCount)
                {
                    //清除分配数据
                    ClearAll();
                    //当前页加1
                    intPageNow += 1;
                    //当前页显示
                    this.txtNow.Text = intPageNow.ToString();
                    //给分配数据填充
                    for (int i = (intPageNow - 1) * IntPageSize; i < IntPageSize * intPageNow; i++)
                    {
                        if (i < list.Count)
                        {
                            Collection.AddNewItem(list[i]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Next", ex.ToString());
            }
            finally
            {
            }

        }
        #endregion

        #region 最后一页

        void Last()
        {
            try
            {
                //总页数必须大于1
                if (this.intPageCount > 1)
                {
                    //清除分配数据
                    ClearAll();
                    //当前页为总页数，也就是最后一页
                    intPageNow = intPageCount;
                    //显示页数
                    this.txtNow.Text = intPageNow.ToString();
                    // //给分配数据填充
                    for (int i = (intPageNow - 1) * IntPageSize; i < IntPageSize * intPageNow; i++)
                    {
                        if (i < list.Count)
                        {
                            Collection.AddNewItem(list[i]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Last", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion

        #endregion

        #region 视图表单格式化


        /// <summary>
        /// 将展现出的视图源进行清空，以备重新填充
        /// </summary>
        void ClearAll()
        {
            try
            {
                if (Collection != null && Collection.Count > 0)
                {
                    //是否正在执行添加子项
                    if (Collection.IsAddingNew)
                    {
                        //禁止添加子项
                        Collection.CancelNew();
                    }
                    //是否可以移除子项
                    if (Collection.CanRemove)
                    {
                        for (int i = Collection.Count - 1; i > -1; i--)
                        {
                            Collection.RemoveAt(i);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ClearAll", ex.ToString());
            }
            finally
            {
            }
        }
        #endregion
    }
}
