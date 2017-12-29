using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;

namespace MhczTBG.Common
{
    public class MethodLb
    {

        static string fileRoot = @"C://鸣赫持志";

        static string fileAddress = @"C://鸣赫持志//异常监测日志.txt";

        static string strParmes = string.Empty;

        /// <summary>
        /// 创建异常日志
        /// </summary>
        /// <param name="className"></param>
        /// <param name="Method"></param>
        /// <param name="yiChang"></param>
        /// <param name="mgs"></param>
        public static void CreateLog(string className, string Method, string yiChang, params object[] mgs)
        {
            try
            {
                if (!Directory.Exists(fileRoot))
                {
                    Directory.CreateDirectory(fileRoot);
                }
                if (!System.IO.File.Exists(fileAddress))
                {
                    System.IO.File.Create(fileAddress);
                }
               
                if (mgs != null)
                {
                    foreach (var str in mgs)
                    {
                        strParmes += str + ";";
                    }
                }
                else
                {
                    strParmes = "null";
                }

                using (StreamWriter sr = System.IO.File.AppendText(fileAddress))
                {
                    sr.WriteLine(sr.NewLine + string.Format("  {0}  ,当前类名:{1},当前方法名:{2},所传参数:{3},        捕获的异常{4}      ", DateTime.Now, className, Method, strParmes, yiChang));
                }

                //MessageBox.Show(string.Format("当前类名:{0},当前方法名:{1},所传参数:{2},------- 捕获的异常{3}",className, Method, strParmes, yiChang));
                
            }
            catch (Exception)
            {               
            }
        }
    }
}
