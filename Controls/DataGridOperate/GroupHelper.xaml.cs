using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Collections;
using System.Windows.Data;
using MhczTBG.Common;

namespace MhczTBG.Controls.DataGridOperate
{
    public partial class GroupHelper : UserControl
    {
        #region 变量

        /// <summary>
        /// 组的集合
        /// </summary>
        ReadOnlyObservableCollection<object> groupsList = null;

        /// <summary>
        /// 监控分组索引
        /// </summary>
        int groupIndex = 0;

        /// <summary>
        /// 存储虚拟数据
        /// </summary>
        List<object> listIndent = new List<object>();

        /// <summary>
        /// 真实存储的数据源
        /// </summary>
        public IList DataList = null;

        /// <summary>
        /// 使用分组的那个列表
        /// </summary>
        DataGrid dataGrid = null;

        #endregion

        #region 构造函数

        public GroupHelper()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "GroupHelper", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion

        #region 生成分组表格

        /// <summary>
        /// 生成分组表格
        /// </summary>
        /// <param name="datagrid">指定的datagrid实例</param>
        /// <param name="type">指定所要映射的类型</param>
        /// <param name="list">指定绑定的数据源</param>
        /// <param name="listIndent">指定绑定的映射集</param>
        public void DatatridInit(DataGrid datagrid, Type type, IList list, string specityProperty)
        {
            try
            {
                //设置datagrid行样式
                datagrid.ItemsSource = null;

                //设置datagrid的行样式为一个Expander（启动初始化 BackgroundRectangle_Loaded）
                datagrid.RowStyle = this.Resources["dataGridRow_Style"] as Style;

                #region 获取参数

                this.DataList = list;
                this.dataGrid = datagrid;

                #endregion

                //获取组
                groupsList = GroupIng(list, specityProperty);

                //生成虚拟模式
                listIndent.Clear();
                for (int i = 0; i < groupsList.Count; i++)
                {
                    listIndent.Add(Activator.CreateInstance(type));
                }
                //生成虚拟表格
                datagrid.ItemsSource = listIndent;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DatatridInit", ex.ToString(), datagrid, type, list, specityProperty);
            }
            finally
            {
            }
        }


        /// <summary>
        /// 表格初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackgroundRectangle_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (groupIndex < groupsList.Count)
                {
                    //获取组（通过排序）
                    CollectionViewGroup group = groupsList[groupIndex] as CollectionViewGroup;

                    //组名称
                    (sender as Expander).Header = group.Name + string.Format("({0})", group.Items.Count);

                    //填充内容
                    ModeDataGrid datagrid = new ModeDataGrid() { MaxHeight = 200 };

                    //
                    datagrid.Margin = new Thickness(0);
                    (sender as Expander).Content = datagrid;

                    //设置标题为不可见
                    datagrid.datagrid.ColumnHeaderStyle = this.Resources["TopRightHeaderTemplate"] as Style;

                    //赋予数据源，组集合
                    datagrid.datagrid.ItemsSource = group.Items;
                    //标题填充
                    datagrid.TitleInit(group.Items[0].GetType());
                    //组索引加1
                    groupIndex++;
                    if (groupIndex == groupsList.Count)
                    {
                        //若果组填充完毕，则将组的索引设置为0
                        groupIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "BackgroundRectangle_Loaded", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 指定名称获取组的集合
        /// </summary>
        /// <param name="list">数据源</param>
        /// <param name="PropertyName">指定属性名称</param>
        /// <returns>返回一个组合</returns>
        ReadOnlyObservableCollection<object> GroupIng(IList list, string PropertyName)
        {
            ReadOnlyObservableCollection<object> result = null;
            try
            {
                //可视化列表集
                ListCollectionView view = new ListCollectionView(list);

                //添加分组
                view.GroupDescriptions.Add(new PropertyGroupDescription(PropertyName));

                //返回分组
                result = view.Groups;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "GroupIng", ex.ToString(), list, PropertyName);
            }
            finally
            {
            }
            return result;
        }

        public void ClearGroup()
        {
            try
            {
                //还原样式
                dataGrid.RowStyle = null;

                //清空数据
                dataGrid.ItemsSource = null;

                //绑定回原来的数据
                dataGrid.ItemsSource = DataList;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ClearGroup", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion
    }
}
