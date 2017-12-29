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
using System.IO;
using System.Windows.Media.Animation;
using System.Net;
using System.Data;
using System.Reflection;
using MhczTBG.Common;
using System.Windows.Threading;

namespace MhczTBG.Controls.CustomWindow
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        #region 变量

        /// <summary>
        /// 存放文件路径
        /// </summary>
        string fileRoot = string.Empty;

        /// <summary>
        /// 设置配置文件
        /// </summary>
        IniFile IniConfig = null;

        /// <summary>
        /// ip集合
        /// </summary>
        List<string> ipAddressList = null;

        #endregion

        #region 自定义委托事件

        public delegate void BeginLoginEventHandle(string loginNames, string loginPwd,ref bool isSuccessedFull);
        /// <summary>
        /// 登陆事件
        /// </summary>
        public event BeginLoginEventHandle BeginLoginEvent = null;

        #endregion

        #region 构造函数

        public LoginWindow(string fileRootPath, List<string> IpaddressList)
        {
            try
            {
                InitializeComponent();

                this.PramesInit(fileRootPath, IpaddressList);

                #region 注册事件区域

                this.Loaded += new RoutedEventHandler(LoginWindow_Loaded);

                this.KeyDown += new KeyEventHandler(LoginWindow_KeyDown);

                #endregion
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "LoginWindow", ex.ToString(), fileRootPath, IpaddressList);
            }
            finally
            {
            }
        }
     
        #endregion

        #region 参数初始化

        /// <summary>
        /// 参数初始化
        /// </summary>
        /// <param name="fileRootPath">文件路径</param>
        /// <param name="IpaddressList">ip集合</param>
        private void PramesInit(string fileRootPath, List<string> IpaddressList)
        {
            try
            {
                fileRoot = fileRootPath;
                IniConfig = new IniFile(fileRootPath);
                ipAddressList = IpaddressList;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "PramesInit", ex.ToString(), fileRootPath, IpaddressList);
            }
            finally
            {
            }
        }

        #endregion

        #region 窗体加载事件
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LoginWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //创建配置文件
                CreatConfig();

                // 读取配置文件用户名
                string defaultUser = CommonMethod.frombase(IniConfig.IniReadValue("DefaultUser", "DefaultUser").ToString());
                //获取用户配置文件
                FirstUserInfo(defaultUser);
                //读取所有用户名称
                ReadAllUsers();

                //设置备用服务器地址
                txtIpAddress.Items.Clear();              
                foreach (var item in ipAddressList)
                {                                          
                    txtIpAddress.Items.Add(item);
                    if (item.Equals(Proxy.ServiceUri1))
                        txtIpAddress.SelectedItem = Proxy.ServiceUri1;
                   
                }
              
                //txtIpAddress.Text = CommonMethod.frombase(IniConfig.IniReadValue("IPAddress", "IPAddress").ToString());
                if (!txtIpAddress.Items.Contains(txtIpAddress.Text))
                {
                    txtIpAddress.Items.Add(txtIpAddress.Text);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "LoginWindow_Loaded", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion

        #region 读取默认用户信息
        /// <summary>
        /// 读取默认用户信息
        /// </summary>
        /// <param name="defaultUser">默认用户</param>
        private void FirstUserInfo(string defaultUser)
        {
            try
            {
                if (defaultUser != "")
                {
                    string Autodenglu = CommonMethod.frombase(IniConfig.IniReadValue(defaultUser, "AutoLogin").ToString());
                    string username = CommonMethod.frombase(IniConfig.IniReadValue(defaultUser, "RemmemberPassword").ToString());
                    string RemeberClose = CommonMethod.frombase(IniConfig.IniReadValue(defaultUser, "RemeberClose").ToString());


                    //仅记住密码
                    if (username != "" && Autodenglu == "false")
                    {
                        this.ckremeber.IsChecked = true;
                        this.CmbUserName.Text = username.Split(new char[] { '#' })[0];
                        this.txtpassword.Password = username.Split(new char[] { '#' })[1];
                    }
                    //记住密码以及自动登录
                    else if (username != "" && Autodenglu == "true")
                    {
                        this.ckremeber.IsChecked = true;
                        this.ckaotudenglu.IsChecked = true;
                        this.CmbUserName.Text = username.Split(new char[] { '#' })[0];
                        this.txtpassword.Password = username.Split(new char[] { '#' })[1];
                        LoginMothed();
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "FirstUserInfo", ex.ToString(), defaultUser);
            }
            finally
            {
            }
        }

        #endregion

        #region 显示多用户
        /// <summary>
        /// 显示多用户
        /// </summary>
        private void ReadAllUsers()
        {
            try
            {
                //获取所有用户
                string Allusers = CommonMethod.frombase(IniConfig.IniReadValue("AllUsers", "AllUsers").ToString());
                CmbUserName.Items.Clear();
                foreach (string item in Allusers.Substring(Allusers.IndexOf("#") + 1).Split(new char[] { '#' }))
                {
                    ComboBoxItem cmbitem = new ComboBoxItem();
                    cmbitem.Content = item;
                    CmbUserName.Items.Add(cmbitem);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ReadAllUsers", ex.ToString());
            }
            finally
            {
            }
        }
        #endregion

        #region 判断登陆方法
        /// <summary>
        /// 判断登陆方法
        /// </summary>
        private void LoginMothed()
        {
            try
            {
                //登陆动画
                Storyboard st = this.Resources["OnClick1"] as Storyboard;
                st.Begin();
                动画border.Visibility = Visibility.Visible;
                this.btnLogin.Content = "    登  录  中";
                this.btnLogin.IsEnabled = false;


                //获取本地ip地址
                //string strHostName = Dns.GetHostName();
                //string clientIPAddress = System.Net.Dns.GetHostAddresses(strHostName).GetValue(1).ToString();

                bool isSuccessedFull = false;

                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1.5);
                timer.Tick += (object sender, EventArgs e) =>
                    {
                        if (this.BeginLoginEvent != null)
                            this.BeginLoginEvent(this.CmbUserName.Text, this.txtpassword.Password, ref isSuccessedFull);

                        if (isSuccessedFull)
                            this.writepassword(this.CmbUserName.Text, this.txtpassword.Password);

                        timer.Stop();
                    };
                timer.Start();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "LoginMothed", ex.ToString());
            }
            finally
            {
            }
        }
        #endregion

        #region 窗体事件逻辑事件
        /// <summary>
        /// 移动窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    DragMove();
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Window_MouseDown", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 窗体最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgMin_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                this.WindowState = System.Windows.WindowState.Minimized;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "imgMin_MouseDown", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgClose_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {             
                this.Close();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "imgClose_MouseDown", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                // 在此处添加事件处理程序实现。
                string username = ((Image)sender).Tag.ToString();
                string AllUsers = CommonMethod.frombase(IniConfig.IniReadValue("AllUsers", "AllUsers").ToString());
                if (AllUsers.Contains(username))
                {
                    AllUsers = AllUsers.Replace("#" + username, "");
                    IniConfig.IniWriteValue("AllUsers", "AllUsers", CommonMethod.tobase(AllUsers));
                    IniConfig.ClearSection(username);
                }
                ReadAllUsers();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Image_MouseDown", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 选择多用户控件事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbUserName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if ((ComboBoxItem)((ComboBox)sender).SelectedItem != null)
                {
                    string username = ((ComboBoxItem)((ComboBox)sender).SelectedItem).Content.ToString();
                    FirstUserInfo(username);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "CmbUserName_SelectionChanged", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 登录按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CmbUserName.Text.Trim() == "")
                {
                    this.username提示.Visibility = Visibility.Visible;

                }
                else if (txtpassword.Password.Trim() == "")
                {
                    this.password提示.Visibility = Visibility.Visible;
                }
                else
                {
                    LoginMothed();
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btnLogin_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 按回车键进行登陆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LoginWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoginMothed();
            }
        }

        /// <summary>
        /// 选择记住密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckremeber_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox ckbox = (CheckBox)sender;
                if (ckaotudenglu.IsChecked == true && ckbox.IsChecked == false)
                {
                    ckaotudenglu.IsChecked = false;
                    ckremeber.IsChecked = false;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ckremeber_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 选择自动登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckaotudenglu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox ckbox = (CheckBox)sender;
                if (ckbox.IsChecked == true)
                {
                    ckremeber.IsChecked = true;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ckaotudenglu_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        #endregion

        #region 配置文件辅助方法
        /// <summary>
        /// 创建配置文件
        /// </summary>
        private void CreatConfig()
        {
            try
            {
                if (!string.IsNullOrEmpty(fileRoot) && fileRoot.Contains("/"))
                {
                    string[] rootList = fileRoot.Split(new char[] { '/' });

                    for (int i = 1; i < rootList.Count() - 1; i++)
                    {
                        string Path = rootList[0];
                        Path += "/" + rootList[i];
                        //判断是否存在 如果不存在创建
                        if (!Directory.Exists(Path))
                        {
                            Directory.CreateDirectory(Path);
                        }
                    }

                    //判断要复制的文件是否存在
                    if (!File.Exists(fileRoot))
                    {
                        string fielTemp = @fileRoot;
                        FileStream fs01 = File.Create(fielTemp);
                        fs01.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "CreatConfig", ex.ToString());
            }
            finally
            {
            }
        }

        /// <summary>
        /// 写入配置文件
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        private void writepassword(string username, string password)
        {
            try
            {
                if (ckaotudenglu.IsChecked == true)
                {
                    string Base64 = CommonMethod.tobase(username + "#" + password);
                    IniConfig.IniWriteValue(username, "AutoLogin", CommonMethod.tobase("true"));
                    IniConfig.IniWriteValue(username, "RemmemberPassword", Base64);

                }
                else if (ckremeber.IsChecked == true && ckaotudenglu.IsChecked == false)
                {
                    string Base64 = CommonMethod.tobase(username + "#" + password);
                    IniConfig.IniWriteValue(username, "AutoLogin", CommonMethod.tobase("false"));
                    IniConfig.IniWriteValue(username, "RemmemberPassword", Base64);
                }
                else
                {
                    IniConfig.IniWriteValue(username, "AutoLogin", CommonMethod.tobase("false"));
                    IniConfig.IniWriteValue(username, "RemmemberPassword", "");
                }
                //验证通过后将成为默认用户
                IniConfig.IniWriteValue("DefaultUser", "DefaultUser", CommonMethod.tobase(username));

                //将验证通过的用户添加到用户组里边
                string AllUsers = CommonMethod.frombase(IniConfig.IniReadValue("AllUsers", "AllUsers").ToString());
                if (!AllUsers.Contains(username))
                {
                    AllUsers += "#" + username;
                    IniConfig.IniWriteValue("AllUsers", "AllUsers", CommonMethod.tobase(AllUsers));
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "writepassword", ex.ToString(), username, password);
            }
            finally
            {
            }
        }

        #endregion

        #region 配置窗体按钮事件
        /// <summary>
        /// 配置窗体按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnsetting确定_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtIpAddress.Text != "")
                {
                    string Base64 = CommonMethod.tobase(txtIpAddress.Text);
                    IniConfig.IniWriteValue("IPAddress", "IPAddress", Base64);
                }
                this.grid1.Visibility = System.Windows.Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btnsetting确定_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
       
        #endregion

        #region 按钮悬浮离开事件
        /// <summary>
        /// 关闭按钮悬浮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgClose_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            try
            {
                imgClose.Source = CommonMethod.GetImageSource(MhczTBG.Properties.Resources.关闭1);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "imgClose_MouseEnter", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 关闭按钮离开事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgClose_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            try
            {
                imgClose.Source = CommonMethod.GetImageSource(MhczTBG.Properties.Resources.关闭);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "imgClose_MouseLeave", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 最小化悬浮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgMin_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            try
            {
                imgMin.Source = CommonMethod.GetImageSource(MhczTBG.Properties.Resources.最小化1);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "imgMin_MouseEnter", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 最小化离开事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgMin_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            try
            {
                imgMin.Source = CommonMethod.GetImageSource(MhczTBG.Properties.Resources.最小化);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "imgMin_MouseLeave", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 设置按钮悬浮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgSetting_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            try
            {
                imgSetting.Source = CommonMethod.GetImageSource(MhczTBG.Properties.Resources.Setting1);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "imgSetting_MouseEnter", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 设置按钮离开事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgSetting_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            try
            {
                imgSetting.Source = CommonMethod.GetImageSource(MhczTBG.Properties.Resources.Setting);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "imgSetting_MouseLeave", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 设置窗体关闭按钮悬浮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgClose1_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            try
            {
                imgClose1.Source = CommonMethod.GetImageSource(MhczTBG.Properties.Resources.关闭1);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "imgClose1_MouseEnter", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 设置窗体关闭按钮离开事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgClose1_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            try
            {
                imgClose1.Source = CommonMethod.GetImageSource(MhczTBG.Properties.Resources.关闭);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "imgClose1_MouseLeave", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 设置窗体最小化悬浮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgMin1_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            try
            {
                imgMin1.Source = CommonMethod.GetImageSource(MhczTBG.Properties.Resources.最小化1);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "imgMin1_MouseEnter", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 设置窗体最小化离开事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgMin1_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            try
            {
                imgMin1.Source = CommonMethod.GetImageSource(MhczTBG.Properties.Resources.最小化);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "imgMin1_MouseLeave", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 用户名框获取焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbUserName_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                this.username提示.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "CmbUserName_GotFocus", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 密码获取焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtpassword_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                // 在此处添加事件处理程序实现。
                this.password提示.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "txtpassword_GotFocus", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        #endregion

        #region 附加方法
        
        private void btnsetting取消_Click(object sender, RoutedEventArgs e)
        {

            this.grid1.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void settingnetwork_Click(object sender, RoutedEventArgs e)
        {
            this.grid1.Visibility = System.Windows.Visibility.Visible;
        }

        public void LoaginFalied()
        {
            Storyboard st = this.Resources["OnClick1"] as Storyboard;
            st.Stop();
            this.动画border.Visibility = Visibility.Collapsed;
            this.btnLogin.Content = "登               录";
            this.btnLogin.IsEnabled = true;
        }

        #endregion
    }
}
