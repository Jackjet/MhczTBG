using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MhczTBG.Common;

namespace MhczTBG.Controls.ComSearchsDongTai
{
    class ComSearchlei
    {
        #region 变量

        private string _quyu;

        public string Quyu
        {
            get { return _quyu; }
            set { _quyu = value; }
        }
        private string _type;

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }
        private string _displayName;

        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }

        private string _ziduan;

        public string Ziduan
        {
            get { return _ziduan; }
            set { _ziduan = value; }
        }

        private string _isDuoXuan;

        public string IsDuoXuan
        {
            get { return _isDuoXuan; }
            set { _isDuoXuan = value; }
        }
        private List<string> _listShuJu;

        public List<string> ListShuJu
        {
            get { return _listShuJu; }
            set { _listShuJu = value; }
        }

        #endregion

        #region 构造函数

        public ComSearchlei(string quyu, string type, string displayname, string ziduan, string isduoxuan, List<string> listshuju)
        {
            try
            {
                this._quyu = quyu;
                this._type = type;
                this._displayName = displayname;
                this._ziduan = ziduan;
                this._isDuoXuan = isduoxuan;
                this._listShuJu = listshuju;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ComSearchlei", ex.ToString(), quyu, type, displayname, ziduan, isduoxuan, listshuju);
            }
            finally
            {
            }
        }

        #endregion
    }
}
