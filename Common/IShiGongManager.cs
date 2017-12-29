using System;
namespace MhczTBG.Common
{
    public interface IShiGongManager
    {
        string DisPlayName { get; set; }
        string EndTime { get; set; }
        double InZhaungTai { get; set; }    
          string strZhuangTai { get; set; }
        string StartTime { get; set; }
        string ShiGongID
        { get; set; }
    }
}
