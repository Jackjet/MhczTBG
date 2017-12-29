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

namespace MhczTBG.StyleResource
{
    /// <summary>
    /// MyStyle.xaml 的交互逻辑
    /// </summary>
    public partial class MyStyle : UserControl
    {
        #region 将该实例存储，之后不再创建实例（单例设计模式）

        static MyStyle myStyle;
        public static MyStyle Instacnce
        {
            get
            {
                if (myStyle == null)
                {
                    myStyle = new MyStyle();
                }
                return myStyle;
            }
        }

        #endregion

        #region DataGrid的主题

        /// <summary>
        /// 橘黄色的Datagrid样式
        /// </summary>
        public  ResourceDictionary DataGridResourcesOrRange
        {
            get { if (myStyle != null) return myStyle.bordDataGrid.Resources; else return null; }
        }

        /// <summary>
        /// 灰色的Datagrid样式
        /// </summary>
        public ResourceDictionary DataGridResourcesGrey
        {
            get { if (myStyle != null) return myStyle.borDataGrid2.Resources; else return null; }
        }

        #endregion

        #region ComboxBox的样式

        /// <summary>
        /// 获取一个指定样式的实例
        /// </summary>
        public static Style ComboBoxStyle
        {
            get
            {
                return Instacnce.borComboBOx.Resources["ComboxStyle"] as Style;
            }
        }
      
        /// <summary>
        /// 不设计业务的功能扩展
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox.Parent != null && comboBox.Parent is Grid && (comboBox.Parent as Grid).Children.Count > 1 && (comboBox.Parent as Grid).Children[1] is TextBox)
            {
                //将选择的文本赋值给文本框
                var text = (comboBox.Parent as Grid).Children[1] as TextBox;

                var selectObject = comboBox.SelectedItem;

                if (comboBox.SelectedItem != null) text.Text = selectObject.ToString();
            }
        }

        #endregion

        #region 构造函数
        
        public MyStyle()
        {
            InitializeComponent();
        }

        #endregion
    }
}
