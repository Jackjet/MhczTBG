using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Security;
using MhczTBG.Common;

namespace MhczTBG.Common
{
    /// <summary>
    /// WCF服务的客户端配置信息
    /// </summary>
    public class ConfigBinding
    {

        #region binding

        string strName;
        /// <summary>
        /// 绑定的名称
        /// </summary>
        public string StrName
        {
            get { return strName; }
            set { strName = value; }
        }

        TimeSpan tspCloseTimeout;
        /// <summary>
        /// 传输引发异常之前可用于关闭连接的时间间隔
        /// </summary>
        public TimeSpan TspCloseTimeout
        {
            get { return tspCloseTimeout; }
            set { tspCloseTimeout = value; }
        }

        TimeSpan tspReceiveTimeout;
        /// <summary>
        /// 在撤消之前保持非活动状态的最大时间间隔
        /// </summary>
        public TimeSpan TspReceiveTimeout
        {
            get { return tspReceiveTimeout; }
            set { tspReceiveTimeout = value; }
        }

        TimeSpan tspSendTimeout;
        /// <summary>
        /// 传输引发异常之前可用于完成写入操作的时间间隔
        /// </summary>
        public TimeSpan TspSendTimeout
        {
            get { return tspSendTimeout; }
            set { tspSendTimeout = value; }
        }

        TimeSpan tspOpenTimeout;
        /// <summary>
        /// 在传输引发异常之前可用于打开连接的时间间隔
        /// </summary>
        public TimeSpan TspOpenTimeout
        {
            get { return tspOpenTimeout; }
            set { tspOpenTimeout = value; }
        }

        bool isAllowCookies;
        /// <summary>
        /// 客户端是否接受 Cookie
        /// </summary>
        public bool IsAllowCookies
        {
            get { return isAllowCookies; }
            set { isAllowCookies = value; }
        }

        bool isBypassProxyOnLocal;
        /// <summary>
        /// 是否对本地地址不使用代理服务器
        /// </summary>
        public bool IsBypassProxyOnLocal
        {
            get { return isBypassProxyOnLocal; }
            set { isBypassProxyOnLocal = value; }
        }

        HostNameComparisonMode enuHostNameComparisonMode;
        /// <summary>
        /// 在对 URI 进行匹配时，是否使用主机名来访问服务
        /// </summary>
        public HostNameComparisonMode EnuHostNameComparisonMode
        {
            get { return enuHostNameComparisonMode; }
            set { enuHostNameComparisonMode = value; }
        }

        int intMaxBufferSize;
        /// <summary>
        /// 该缓冲区用于接收来自通道的消息
        /// </summary>
        public int IntMaxBufferSize
        {
            get { return intMaxBufferSize; }
            set { intMaxBufferSize = value; }
        }


        long lonMaxBufferPoolSize;
        /// <summary>
        /// 从通道接收消息的消息缓冲区管理器分配并供其使用的最大内存量
        /// </summary>
        public long LonMaxBufferPoolSize
        {
            get { return lonMaxBufferPoolSize; }
            set { lonMaxBufferPoolSize = value; }
        }

        long lonMaxReceivedMessageSize;
        /// <summary>
        /// 配置了此绑定的通道上可以接收的消息的最大大小
        /// </summary>
        public long LonMaxReceivedMessageSize
        {
            get { return lonMaxReceivedMessageSize; }
            set { lonMaxReceivedMessageSize = value; }
        }

        WSMessageEncoding enuMessageEncoding;
        /// <summary>
        /// 是使用 MTOM 还是文本对 SOAP 消息进行编码
        /// </summary>
        public WSMessageEncoding EnuMessageEncoding
        {
            get { return enuMessageEncoding; }
            set { enuMessageEncoding = value; }
        }

        Encoding encTextEncoding;
        /// <summary>
        /// 用于消息文本的字符编码
        /// </summary>
        public Encoding EncTextEncoding
        {
            get { return encTextEncoding; }
            set { encTextEncoding = value; }
        }

        TransferMode traTransferMode;
        /// <summary>
        /// 通过缓冲处理还是流处理来发送消息
        /// </summary>
        public TransferMode TraTransferMode
        {
            get { return traTransferMode; }
            set { traTransferMode = value; }
        }

        bool isUseDefaultWebProxy;
        /// <summary>
        /// 是否应使用系统的自动配置 HTTP 代理（如果可用）
        /// </summary>
        public bool IsUseDefaultWebProxy
        {
            get { return isUseDefaultWebProxy; }
            set { isUseDefaultWebProxy = value; }
        }
        #endregion

        #region readerQuotas

        int intMaxDepth;
        /// <summary>
        /// 最大嵌套节点深度
        /// </summary>
        public int IntMaxDepth
        {
            get { return intMaxDepth; }
            set { intMaxDepth = value; }
        }

        int intMaxStringContentLength;
        /// <summary>
        /// 读取器返回的最大字符串长度
        /// </summary>
        public int IntMaxStringContentLength
        {
            get { return intMaxStringContentLength; }
            set { intMaxStringContentLength = value; }
        }

        int intMaxArrayLength;
        /// <summary>
        /// 允许的最大数组长度
        /// </summary>
        public int IntMaxArrayLength
        {
            get { return intMaxArrayLength; }
            set { intMaxArrayLength = value; }
        }

        int intMaxBytesPerRead;
        /// <summary>
        /// 允许每次读取返回的最大字节数
        /// </summary>
        public int IntMaxBytesPerRead
        {
            get { return intMaxBytesPerRead; }
            set { intMaxBytesPerRead = value; }
        }

        int intMaxNameTableCharCount;
        /// <summary>
        /// 表名称中允许的最大字符数
        /// </summary>
        public int IntMaxNameTableCharCount
        {
            get { return intMaxNameTableCharCount; }
            set { intMaxNameTableCharCount = value; }
        }
        #endregion

        #region security

        BasicHttpSecurityMode basMode;
        /// <summary>
        /// basicHttpBinding 绑定的安全模式
        /// </summary>
        public BasicHttpSecurityMode BasMode
        {
            get { return basMode; }
            set { basMode = value; }
        }


        #region transport
        HttpClientCredentialType htpClientCredentialType;
        /// <summary>
        /// 身份验证模式
        /// </summary>
        public HttpClientCredentialType HtpClientCredentialType
        {
            get { return htpClientCredentialType; }
            set { htpClientCredentialType = value; }
        }

        HttpProxyCredentialType htpProxyCredentialType;
        /// <summary>
        /// 获取或设置要用于针对代理进行身份验证的客户端凭据的类型
        /// </summary>
        public HttpProxyCredentialType HtpProxyCredentialType
        {
            get { return htpProxyCredentialType; }
            set { htpProxyCredentialType = value; }
        }


        string strRealm;
        /// <summary>
        /// 
        /// </summary>
        public string StrRealm
        {
            get { return strRealm; }
            set { strRealm = value; }
        }

        #endregion
        #endregion

        #region message

        BasicHttpMessageCredentialType basMessClientCredentialType;
        /// <summary>
        /// 客户端用以进行身份验证的凭据的类型
        /// </summary>
        public BasicHttpMessageCredentialType BasMessClientCredentialType
        {
            get { return basMessClientCredentialType; }
            set { basMessClientCredentialType = value; }
        }

        SecurityAlgorithmSuite secAlgorithmSuite;
        /// <summary>
        /// 要与 System.ServiceModel.BasicHttpMessageSecurity 一起使用的算法组
        /// </summary>
        public SecurityAlgorithmSuite SecAlgorithmSuite
        {
            get { return secAlgorithmSuite; }
            set { secAlgorithmSuite = value; }
        }


        #endregion

        #region 构造函数

        public ConfigBinding()
        {
            try
            {
                //绑定的名称
                StrName = "BasicHttpBinding_Irwp";
                //传输引发异常之前可用于关闭连接的时间间隔
                TspCloseTimeout = new TimeSpan(0, 1, 0);
                //在传输引发异常之前可用于打开连接的时间间隔
                TspOpenTimeout = new TimeSpan(0, 1, 0);
                //在撤消之前保持非活动状态的最大时间间隔
                TspReceiveTimeout = new TimeSpan(0, 10, 0);
                //传输引发异常之前可用于完成写入操作的时间间隔
                TspSendTimeout = new TimeSpan(0, 1, 0);
                //客户端是否接受 Cookie
                IsAllowCookies = true;

                //是否对本地地址不使用代理服务器
                IsBypassProxyOnLocal = false;
                //在对 URI 进行匹配时，是否使用主机名来访问服务
                EnuHostNameComparisonMode = HostNameComparisonMode.StrongWildcard;

                //该缓冲区用于接收来自通道的消息
                IntMaxBufferSize = 665536;
                //从通道接收消息的消息缓冲区管理器分配并供其使用的最大内存量
                LonMaxBufferPoolSize = 6524288;
                //配置了此绑定的通道上可以接收的消息的最大大小
                LonMaxReceivedMessageSize = 665536;
                //是使用 MTOM 还是文本对 SOAP 消息进行编码
                EnuMessageEncoding = WSMessageEncoding.Text;
                //用于消息文本的字符编码
                EncTextEncoding = Encoding.UTF8;
                //通过缓冲处理还是流处理来发送消息
                TraTransferMode = TransferMode.Buffered;
                //是否应使用系统的自动配置 HTTP 代理（如果可用）
                IsUseDefaultWebProxy = false;

                //最大嵌套节点深度
                IntMaxDepth = 632;
                //读取器返回的最大字符串长度
                IntMaxStringContentLength = 68192;
                //允许的最大数组长度
                IntMaxArrayLength = 616384;
                //允许每次读取返回的最大字节数
                IntMaxBytesPerRead = 64096;
                //表中允许的最大字符数
                IntMaxNameTableCharCount = 616384;

                //绑定的安全模式
                BasMode = BasicHttpSecurityMode.TransportCredentialOnly;
                //身份验证模式
                HtpClientCredentialType = HttpClientCredentialType.Ntlm;
                //获取或设置要用于针对代理进行身份验证的客户端凭据的类型
                HtpProxyCredentialType = HttpProxyCredentialType.None;

                StrRealm = "";

                // 客户端用以进行身份验证的凭据的类型
                BasMessClientCredentialType = BasicHttpMessageCredentialType.UserName;
                //// 要与 System.ServiceModel.BasicHttpMessageSecurity 一起使用的算法组
                SecAlgorithmSuite = SecurityAlgorithmSuite.Default;

            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ConfigBinding", ex.ToString(), null);
            }
        }

        #endregion
    }
}
