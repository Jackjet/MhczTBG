using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.ComponentModel;
using System.Resources;


namespace MhczTBG.Common
{
    public class FileInformationLb : INotifyPropertyChanged
    {

        private string strTypeImageUri;
        /// <summary>
        /// 类型名称
        /// </summary>
        public string StrTypeImageUri
        {
            get { return strTypeImageUri; }
            set { strTypeImageUri = value; }
        }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string StrFileName { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public string StrFileSize { get; set; }



        ControlTemplate temUpLoadStatu = null;
        /// <summary>
        /// 下载状态（模板）
        /// </summary>
        public ControlTemplate TemUpLoadStatu
        {
            get { return temUpLoadStatu; }
            set
            {
                if (value != null)
                {
                    temUpLoadStatu = value;

                    OnPropertyChanged("TemUpLoadStatu");
                }
            }
        }



        /// <summary>
        /// 下载信息初始化
        /// </summary>
        /// <param name="typeName">文件类型名称</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="fileSize">文件大小（/MB或/KB）</param>
        /// <param name="upLoadStatu">文件下载状态（模板）</param>
        public FileInformationLb(string strTypeName, string strFileName, string strFileSize, ControlTemplate temUpLoadStatu)
        {
            try
            {
                System.Resources.ResourceManager rm = MhczTBG.Properties.Resources.ResourceManager;
                StrFileName = strFileName;
                StrFileSize = strFileSize;
                TemUpLoadStatu = temUpLoadStatu;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "FileInformationLb", ex.ToString(), strTypeName, strFileName, strFileSize, temUpLoadStatu);
            }
            finally
            {
            }
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

}
