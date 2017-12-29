using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MhczTBG.Common
{
    /// <summary>
    /// 设置表格的布局
    /// </summary>
    class ChenJieEntity
    {
        //int Number = 0;

        public bool isParent = false;

        //所有乘阶数
        public List<int> LeftCC = new List<int>();

        public int Sumn = 0;
    }
}
