using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MhczTBG.Common
{
    public class DocumentLb
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string StrMingCheng { get; set; }

        /// <summary>
        /// 文件数量
        /// </summary>
        public string StrCount { get; set; }

        /// <summary>
        /// 文件字节数组
        /// </summary>
        public byte[] BytFilebyte { get; set; }

        /// <summary>
        /// 文件版本
        /// </summary>
        public string RunVersion { get; set; }
    }
}
