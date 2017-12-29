using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using MhczTBG.Controls.TongJiDataGrid;
using MhczTBG.Controls;
using System.ComponentModel;

namespace MhczTBG.Common
{
    [Serializable]
    public class DataGridUserEntity : INotifyPropertyChanged
    {
        int _ID;
        /// <summary>
        /// 序号
        /// </summary>
        public int ID
        {
            get { return _ID; }
            set
            {
                _ID = value;
                OnPropertyChanged("ID");
            }
        }
     
        public string HengTittle { get; set; }

        public string ShuTittle { get; set; }

        public List<DataGridUserEntityItem> DicHengList { get; set; }

        public List<DataGridUserEntityItem> DicShuList { get; set; }

        public DataGridUserEntity(string hengTittle, string shuTittle, List<DataGridUserEntityItem> dicHengList, List<DataGridUserEntityItem> dicShuList)
        {
            this.HengTittle = hengTittle;
            this.ShuTittle = shuTittle;
            this.DicHengList = dicHengList;
            this.DicShuList = dicShuList;
        }

        public DataGridUserEntity()
        {
        }

        /// <summary>
        /// 属性更改事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 属性更改激发
        /// </summary>
        /// <param name="property"></param>
        public void OnPropertyChanged(string strProperty)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(strProperty));
            }
        }
    }

    [Serializable]
    public class DataGridUserEntityItem
    {
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

       

        public DataGridUserEntityItem(string tittle, string propertt, TongJiItemChild child)
        {
            this.Tittle = tittle;
            this.StrProperty = propertt;
            this.ItemChild = child;
        }
    }

    [Serializable]
    public class BindlistCustomDataGridItemENtity : BindingList<DataGridUserEntity>
    {
        public BindlistCustomDataGridItemENtity()
        {
        }
    }
}
