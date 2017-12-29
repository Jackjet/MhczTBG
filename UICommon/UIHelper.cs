using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TongXin.UIComman
{
    public static class UIHelper
    {
        //加密过程
        public static string tobase(string password)
        {
            byte[] myByte;
            string Base64Str = "";
            Encoding myEncoding = Encoding.GetEncoding("utf-8");
            myByte = myEncoding.GetBytes(password);
            Base64Str = Convert.ToBase64String(myByte);
            return Base64Str;
        }

        //解密过程
        public static string frombase(string password)
        {
            byte[] myByte;
            string factString = "";
            Encoding myEncoding = Encoding.GetEncoding("utf-8");
            myByte = Convert.FromBase64String(password);
            factString = myEncoding.GetString(myByte);
            return factString;

        }
    }
}
