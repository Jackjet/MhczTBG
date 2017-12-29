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

namespace MhczTBG.Controls.Print
{
    /// <summary>
    /// PrintDataGrid.xaml 的交互逻辑
    /// </summary>
    partial class PrintDataGrid : UserControl
    {      
        #region 构造函数
        public PrintDataGrid()
        {
              try
            {
            InitializeComponent();  
                    }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "PrintDataGrid", ex.ToString());
            }
            finally
            {
            } 
        }
        #endregion
       
        #region 复制标题（打印时用）
        /// <summary>
        /// 复制标题（打印时用）
        /// </summary>
        /// <param name="columns">DataGridColumn</param>
        public void TitleInit(System.Collections.ObjectModel.ObservableCollection<DataGridColumn> columns)
        {
            try
            {
                datagrid.Columns.Clear();

                //DataGridTemplateColumn column1 = new DataGridTemplateColumn();
                //column1.CanUserSort = true;
                //column1.CanUserReorder = true;
                //column1.Header = "附件";
                //column1.CellTemplate = (DataTemplate)this.TryFindResource("DataTemplate1");
                //datagrid.Columns.Add(column1);
                foreach (var columnData in columns)
                {
                    //if (columnData.Header.ToString().Contains("附件")) continue;
                    DataGridTextColumn column = new DataGridTextColumn();
                    column.CanUserSort = true;
                    column.CanUserReorder = true;
                    column.Header = columnData.Header;
                    column.Binding = new Binding(columnData.Header.ToString());
                    datagrid.Columns.Add(column);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TitleInit", ex.ToString(), columns);
            }
            finally
            {
            }
        }

        #endregion

        #region 添加Datagrid子项
        /// <summary>
        /// 添加Datagrid子项
        /// </summary>
        /// <param name="item">子项</param>
        public void ItemsAdd(object item)
        {
            try
            {
                this.datagrid.Items.Add(item);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ItemsAdd", ex.ToString(), item);
            }
            finally
            {
            }
        }

        #endregion
    }
}
