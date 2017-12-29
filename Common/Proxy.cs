using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.ServiceModel;

namespace MhczTBG.Common
{
   public static class Proxy
    {
        public static string WebSite = "/SFM";

        public static string UserName = string.Empty;

        public static string ServiceUri1 = "http://192.168.0.154";

        public static string SelectedServiceUri = string.Empty;

        public static string Domain="bjtxd";

        public static string Password =string.Empty;

        //预览附件时临时附件所存放的目录
        public static string StrWatchUrl = "C:\\Temp";

        //附件预览的路径
        public static string strSiteUrl = "/SFM/Lists/TXListGZ/Attachments/";

        //public static string TxtIPadress = "http://192.168.0.154";

        public static string EndPointUri = "/_Layouts/FtxdWcf/txd.svc";

        /// <summary>
        /// 用于连接的指定文档库名称
        /// </summary>
        public static string attch = "Attachments";
       

        public static string ListName="TXListGZ";

        public static string DingXing = "DingXing";

        public static string DingZe = "DingZe";

        public static string Line = "txLine";

        public static string CheJianMingCheng = "TXCheJian";

        public static string DuanDing = "DuanDing";

        public static string JuDing = "JuDing";

        public static string ShebeiTypeYiJi = "ShebeiTypeYiJi";

        public static string SheBeiTypeErJi = "SheBeiTypeErJi";

        public static string GongQu = "TXGongQu";


        public static List<string> DingXingList = null;

        public static List<string> DingZeList = null;

        public static List<string> GongQuList = null;

        public static List<string> LineList = null;

        public static List<string> YanShiList = new List<string>();

        public static List<string> CheJianMingChengList = null;

        public static List<string> DuanDingList = null;

        public static List<string> JuDingList = null;

        public static List<string> ShebeiTypeYiJiList = null;

        public static List<string> SheBeiTypeErJiList = null;

        public static Dictionary<string,List<string>> DicShebeiTypeList = null;

        public static TongXin.wcfTX.ItxdClient Client = null;

        public static string Apartment = string.Empty;

        public static string Position = string.Empty;


        #region 生成客户端配置

        /// <summary>
        /// proxy生成
        /// </summary>
        /// <param name="endPointUri">终结点uri设置</param>
        /// <param name="domain">域名设置</param>
        /// <param name="userName">用户名设置</param>
        /// <param name="password">密码设置</param>
        public static void ConfigurationInit(string endPointUri, string domain, string userName, string password)
        {

            BasicHttpBinding basicHttpBinding = new BasicHttpBinding();

            #region BasicHttpBinding的设置

            ConfigBinding binding = new ConfigBinding();

            basicHttpBinding.Name = binding.StrName;

            basicHttpBinding.CloseTimeout = binding.TspCloseTimeout;
            basicHttpBinding.OpenTimeout = binding.TspOpenTimeout;
            basicHttpBinding.ReceiveTimeout = binding.TspReceiveTimeout;
            basicHttpBinding.SendTimeout = binding.TspSendTimeout;

            basicHttpBinding.AllowCookies = binding.IsAllowCookies;
            basicHttpBinding.BypassProxyOnLocal = binding.IsBypassProxyOnLocal;
            basicHttpBinding.HostNameComparisonMode = binding.EnuHostNameComparisonMode;

            basicHttpBinding.MaxBufferSize = binding.IntMaxBufferSize;
            basicHttpBinding.MaxBufferPoolSize = binding.LonMaxBufferPoolSize;
            basicHttpBinding.MaxReceivedMessageSize = binding.LonMaxReceivedMessageSize;

            basicHttpBinding.MessageEncoding = binding.EnuMessageEncoding;
            basicHttpBinding.TextEncoding = binding.EncTextEncoding;
            basicHttpBinding.TransferMode = binding.TraTransferMode;         

            basicHttpBinding.UseDefaultWebProxy = binding.IsUseDefaultWebProxy;

            basicHttpBinding.ReaderQuotas.MaxDepth = binding.IntMaxDepth;
            basicHttpBinding.ReaderQuotas.MaxStringContentLength = binding.IntMaxStringContentLength;
            basicHttpBinding.ReaderQuotas.MaxArrayLength = binding.IntMaxArrayLength;
            basicHttpBinding.ReaderQuotas.MaxBytesPerRead = binding.IntMaxBytesPerRead;
            basicHttpBinding.ReaderQuotas.MaxNameTableCharCount = binding.IntMaxNameTableCharCount;

            basicHttpBinding.Security.Mode = binding.BasMode;
            basicHttpBinding.Security.Transport.ClientCredentialType = binding.HtpClientCredentialType;
            basicHttpBinding.Security.Transport.ProxyCredentialType = binding.HtpProxyCredentialType;
            basicHttpBinding.Security.Transport.Realm = binding.StrRealm;

            basicHttpBinding.Security.Message.ClientCredentialType = binding.BasMessClientCredentialType;
            basicHttpBinding.Security.Message.AlgorithmSuite = binding.SecAlgorithmSuite;
            #endregion

            #region 终结点与验证设置
            EndpointAddress endpoint =
             new EndpointAddress(endPointUri);           


            Proxy.Client = new TongXin.wcfTX.ItxdClient(basicHttpBinding, endpoint);

            //Proxy = new iRWPClient();
            //Proxy.Endpoint = endpoint;

            //使用工厂模式
            Proxy.Client.ChannelFactory.Credentials.Windows.ClientCredential.Domain = domain;
            Proxy.Client.ChannelFactory.Credentials.Windows.ClientCredential.UserName = userName;
            Proxy.Client.ChannelFactory.Credentials.Windows.ClientCredential.Password = password;

            Proxy.Client.ClientCredentials.Windows.AllowedImpersonationLevel =
              TokenImpersonationLevel.Impersonation;
            #endregion
        }
        #endregion
    }
}
