using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Net;
using System.IO;
using Microsoft.Win32;
using System.Threading;
using MhczTBG.Common;

namespace MhczTBG.Controls.Files
{
    /// <summary>
    /// FileDown.xaml 的交互逻辑
    /// </summary>
    public partial class FileDown : Window
    {
        #region 变量

        //判断是否需要下载
        public bool needDownload = false;

        #endregion

        #region 构造函数

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="UserName">用户名称</param>
        /// <param name="PassWord">密码</param>
        /// <param name="Doamin">域名</param>
        public FileDown(string filePath, string UserName, string PassWord, string Doamin)
        {
              try
            {
            InitializeComponent();

            //通过指定路径来下载文件
            Download(filePath, UserName, PassWord, Doamin);

            #region UI注册事件区域
            //关闭时释放内存
            this.Closed += new EventHandler(FileDown_Closed);
            //确定关闭
            this.btnOK.Click += new RoutedEventHandler(btnOK_Click);

            #endregion
                    }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "FileDown", ex.ToString(),filePath,  UserName,  PassWord,  Doamin);
            }
            finally
            {
            }
        }

        #endregion

        #region UI事件区域

        //结束下载后释放内存
        void FileDown_Closed(object sender, EventArgs e)
        {
              try
            {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
                    }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "FileDown_Closed", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 关闭下载框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
              try
            {
            this.Close();
                    }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btnOK_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion

        #region 下载文件

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="UserName">用户名</param>
        /// <param name="PassWord">密码</param>
        /// <param name="Doamin">站点</param>
        public void Download(string filePath, string UserName, string PassWord, string Doamin)
        {
              try
            {
            WebClient client = new WebClient();
            //通过验证
            client.Credentials = new NetworkCredential(UserName, PassWord, Doamin);

            //保存对话框
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            //扩展名
            string extension = System.IO.Path.GetExtension(filePath);

            ////默认文件名称
            string fileName = filePath.Substring(filePath.LastIndexOf("/") + 1);
            saveFileDialog.FileName += fileName;

            //保存对话框对应的类型
            saveFileDialog.Filter = string.Format("*{0}| *{0}", extension);

            //对话框选择确定，则标记需要下载
            if (saveFileDialog.ShowDialog() == true)
            {
                needDownload = true;
            }
           
            if (needDownload)
            {
                //对话框的进度为0
                this.progressBar1.Value = 0;

                #region 初始化化对话框
                //进度条最大值设置
                this.progressBar1.Maximum = 100;

                this.hbFileName.Content = fileName;
                this.hbFileName.Tag = filePath;

                this.btnOK.IsEnabled = false;
                this.btnCancel.IsEnabled = true;

                #endregion

                TomDisPatcherLb timer2 = new TomDisPatcherLb(new Action(() =>
                    {
                        //下载文件
                        client.DownloadFile(filePath, saveFileDialog.FileName);
                        //下载完成之后
                        client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                        client.DownloadFileAsync(new Uri(filePath), Dns.GetHostName());
                        //下载中
                        client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                    }));
                timer2.Start();
            }
                    }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Download", ex.ToString(),filePath,  UserName,  PassWord,  Doamin);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 下载监控
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
              try
            {
            //进度量
            this.progressBar1.Value = e.ProgressPercentage;
                    }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "client_DownloadProgressChanged", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 下载完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
              try
            {
            //进度为最大值
            this.progressBar1.Value = this.progressBar1.Maximum;

            #region 按钮设置
            this.btnOK.IsEnabled = true;
            this.btnCancel.IsEnabled = false;
            #endregion
                    }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "client_DownloadFileCompleted", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion
    }
}
