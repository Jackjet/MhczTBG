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
using System.Windows.Media.Effects;
using MhczTBG.Common;

namespace MhczTBG.Controls
{
    /// <summary>
    /// Item.xaml 的交互逻辑
    /// </summary>
    public partial class TongJiItem : UserControl
    {
        #region 变量

        private string strStyle;
        /// <summary>
        /// 样式
        /// </summary>
        public string StrStyle
        {
            get { return strStyle; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    Style stylell = this.Resources[value] as Style;
                    if (stylell != null)
                    {
                        this.borMain.Style = stylell;

                        if (value.Contains("borStyle1"))
                        {
                            path.Style = this.Resources["pathStyle1"] as Style;
                        }
                        else if (value.Contains("borStyle2") || value.Contains("borStyle3"))
                        {
                            path.Style = this.Resources["pathStyle2"] as Style;
                        }
                        strStyle = value;
                    }
                }
            }
        }

        string _tittle;
        /// <summary>
        /// 标题
        /// </summary>
        public string Tittle
        {
            get { return _tittle; }
            set { _tittle = value; }
        }

        string strProperty;
        /// <summary>
        /// 表格字段
        /// </summary>
        public string StrProperty
        {
            get { return strProperty; }
            set { strProperty = value; }
        }

        private TongJiItemChild _itemChild = new TongJiItemChild();
        /// <summary>
        /// 管理字段生成标题项
        /// </summary>
        public TongJiItemChild ItemChild
        {
            get { return _itemChild; }
            set { _itemChild = value; }
        }

        private TongJiItemChild _itemChild2 = null;
        /// <summary>
        /// 管理字段生成标题项
        /// </summary>
        public TongJiItemChild ItemChild2
        {
            get { return _itemChild2; }
            set { _itemChild2 = value; }
        }

        public static List<TongJiItem> TongJiItemList = new List<TongJiItem>();

        #endregion

        #region 构造函数

        public TongJiItem(string strText, string strproperty, bool writeIN)
        {
            InitializeComponent();

            this.ItemInit(strText, strproperty);

            #region 注册事件

            //触发器
            this.borMain.MouseEnter += new MouseEventHandler(borMain_MouseEnter);
            this.borMain.MouseLeave += new MouseEventHandler(borMain_MouseLeave);

            #endregion
            if (writeIN)
                TongJiItemList.Add(this);
        }

        #endregion

        #region 触发区域

        void borMain_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                this.borMain.BorderThickness = new Thickness(0);
                this.borMain.Effect = null;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "borMain_MouseLeave", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        void borMain_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                this.borMain.BorderThickness = new Thickness(2);
                this.borMain.Effect = new BlurEffect() { Radius = 1 };
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "borMain_MouseEnter", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion

        #region 初始化字段

        private void ItemInit(string strText, string strproperty)
        {
            try
            {
                this.txt.Text = strText;
                this.Tittle = strText;
                this.StrProperty = strproperty;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ItemInit", ex.ToString(), strText, strproperty);
            }
            finally
            {
            }
        }

        #endregion

    }

    [Serializable]
    public class TongJiItemChild
    {
        List<string> _propertyList = new List<string>();
        /// <summary>
        /// 绑定的字段集合
        /// </summary>
        public List<string> PropertyList
        {
            get { return _propertyList; }
            set { _propertyList = value; }
        }

        Dictionary<string, List<string>> _dicPropertyCaml = new Dictionary<string, List<string>>();
        /// <summary>
        /// 对应父级的字段
        /// </summary>
        public Dictionary<string, List<string>> DicPropertyCaml
        {
            get { return _dicPropertyCaml; }
            set { _dicPropertyCaml = value; }
        }

        List<int> _intIndenttitys = new List<int>();
        /// <summary>
        /// 子级对应映射点
        /// </summary>
        public List<int> IntIndenttitys
        {
            get { return _intIndenttitys; }
            set { _intIndenttitys = value; }
        }

        string _parentPropertyName = "propertyName111";
        /// <summary>
        /// 父级字段的名称
        /// </summary>
        public string ParentPropertyName
        {
            get { return _parentPropertyName; }
            set { _parentPropertyName = value; }
        }

    }
}
