using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MhczTBG.Common
{
    public class IniFile
    {
        public string path;

        public IniFile(string INIPath)
        {
            path = INIPath;
        }

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);


        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string defVal, Byte[] retVal, int size, string filePath);

        /// <summary>
        /// 写INI文件
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        public void IniWriteValue(string Section, string Key, string Value)
        {
            try
            {
                WritePrivateProfileString(Section, Key, Value, this.path);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "IniWriteValue", ex.ToString(), Section, Key, Value);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 读取INI文件
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public string IniReadValue(string Section, string Key)
        {
            string result = string.Empty;
            try
            {
                StringBuilder temp = new StringBuilder(10000);
                int i = GetPrivateProfileString(Section, Key, "", temp, 10000, this.path);
                result = temp.ToString();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "IniReadValue", ex.ToString(), Section, Key);
            }
            finally
            {
            }
            return result;
        }
        public byte[] IniReadValues(string section, string key)
        {
            byte[] temp = new byte[10000];
              try
            {
                int i = GetPrivateProfileString(section, key, "", temp, 10000, this.path);
                    }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "IniReadValues", ex.ToString(), section, key);
            }
            finally
            {
            }
            return temp;
        }


        /// <summary>
        /// 删除ini文件下所有段落
        /// </summary>
        public void ClearAllSection()
        {
            IniWriteValue(null, null, null);
        }
        /// <summary>
        /// 删除ini文件下personal段落下的所有键
        /// </summary>
        /// <param name="Section"></param>
        public void ClearSection(string Section)
        {
            IniWriteValue(Section, null, null);
        }

    }
}
