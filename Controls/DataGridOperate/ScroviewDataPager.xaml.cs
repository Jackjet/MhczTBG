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
using MhczTBG.Common;

namespace MhczTBG.Controls.DataGridOperate
{
    /// <summary>
    /// ScroviewChange.xaml 的交互逻辑
    /// </summary>
    public partial class ScroviewDataPager : UserControl
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

        ListCollectionView collection;
        /// <summary>
        /// 数据集合视图
        /// </summary>
        public ListCollectionView Collection
        {
            get { return collection; }
            set { collection = value; }
        }

        /// <summary>
        /// 数据源
        /// </summary>
        public ArrayList list = new ArrayList();


        int intPageNow = 1;
        /// <summary>
        /// 总页数
        /// </summary>
        public int intPageCount;


        /// <summary>
        /// 控制加载页面的计时器
        /// </summary>
        DispatcherTimer timer = new DispatcherTimer();

        #endregion

        #region 构造函数

        public ScroviewDataPager()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ScroviewDataPager", ex.ToString());
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
       /// <param name="pageSize">一页所显示的数据</param>
        public void listInit(DataGrid datagrid, IList lists, int pageSize)
        {
            try
            {
                #region 添加加载页功能

                datagrid.AddHandler(ScrollViewer.ScrollChangedEvent, new ScrollChangedEventHandler(datagrid_ScrollChanged));
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += new EventHandler(timer_Tick);

                #endregion

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
                    //this.txtCount.Text = "1";
                    //当前页显示
                    //this.txtNow.Text = "1";

                    #endregion
                    //填充数据源
                    for (int i = 0; i < collection.Count; i++)
                    {
                        list.Add(collection.GetItemAt(i));
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

                    //this.txtCount.Text = intPageCount.ToString();
                    //当前页
                    //this.txtNow.Text = intPageNow.ToString();
                    txtAllcount.Text = list.Count.ToString();

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

        #region 加载下一页

        /// <summary>
        /// 下一页
        /// </summary>
        public void Next()
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
                    //this.txtNow.Text = intPageNow.ToString();
                    //给分配数据填充
                    for (int i = 0; i < IntPageSize * intPageNow; i++)
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

        #region 加载页面方法

        /// <summary>
        /// 控件页面加载的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Tick(object sender, EventArgs e)
        {
              try
            {
            //执行添加下一页
            this.Next();
            //计时器停止
            timer.Stop();
            //关闭加载提示
            stackPanel.Visibility = System.Windows.Visibility.Collapsed;
            //已加载的数量
            txtEnd.Text = Collection.Count.ToString();
                    }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "timer_Tick", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 加载状态控制变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void datagrid_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
              try
            {
            //当滚动条到达底端时执行加载页面
            if ((e.ViewportHeight + e.VerticalOffset) == e.ExtentHeight && e.ExtentHeight <= list.Count)
            {

                //当所有加载完，无需再加载
                if (e.ExtentHeight == list.Count)
                {
                    //关闭提示
                    stackPanel.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    //开始执行
                    timer.Start();
                    //开启提示
                    stackPanel.Visibility = System.Windows.Visibility.Visible;
                }
            }
                    }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "datagrid_ScrollChanged", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion

    }
}
