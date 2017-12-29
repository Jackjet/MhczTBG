using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MhczTBG.Common
{
    public class SLtxtHelp
    {
        /// <summary>
        /// 获取某月的最后一天
        /// </summary>
        /// <param name="Pdate">具体年月日值</param>
        public string GetEndDay(string Pdate)
        {
            DateTime time = DateTime.Parse(Pdate);
            DateTime start = new DateTime(time.Year, time.Month, 1);
            DateTime end = start.AddMonths(1).AddDays(-1);//月末日期

            return end.ToString("yyyy-MM-dd");
        }
    }
}
