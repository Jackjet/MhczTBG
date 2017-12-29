using System;
namespace MhczTBG.Controls.RiLi
{
    public interface IRiLiEntity
    {
        /// <summary>
        /// 时间
        /// </summary>
        string DateTime { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        string Note { get; set; }
    }
}
