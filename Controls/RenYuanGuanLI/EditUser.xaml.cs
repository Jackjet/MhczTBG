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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MhczTBG.Common;

namespace MhczTBG.Controls.RenYuanGuanLI
{
    /// <summary>
    /// NewUser.xaml 的交互逻辑
    /// </summary>
    public partial class EditUser : UserControl
    {
        /// <summary>
        /// 数据资源
        /// </summary>
        public Dictionary<string, object> dicSaveData = null;

        public EditUser()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "EditUser", ex.ToString());
            }
            finally
            {
            }
        }

        public void Clear(string strText)
        {
            try
            {
                //清空所有值
                this.txtUserName.Text = string.Empty;
                this.txtEmail.Text = string.Empty;
                this.txtMobile.Text = string.Empty;
                this.txtDepart.Text = strText;//给组赋值
                this.cmbZiWei.SelectedIndex = -1;
                this.txtUnitFax.Text = string.Empty;
                this.txtUnitCode.Text = string.Empty;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Clear", ex.ToString(), strText);
            }
            finally
            {
            }
        }

        public void FillData(Dictionary<string, object> dicINformatoin)
        {
            try
            {
                dicSaveData = dicINformatoin;
                //绑定参数
                if (dicINformatoin.ContainsKey("DisplayName"))
                {
                    var DisplayName = dicINformatoin["DisplayName"];
                    if (DisplayName != null) this.txtUserName.Text = dicINformatoin["DisplayName"].ToString();
                    else this.txtUserName.Text = string.Empty;
                }
                if (dicINformatoin.ContainsKey("Mail"))
                {
                    var Mail = dicINformatoin["Mail"];
                    if (Mail != null) this.txtEmail.Text = dicINformatoin["Mail"].ToString();
                    else this.txtEmail.Text = string.Empty;

                }
                if (dicINformatoin.ContainsKey("TelephoneNumber"))
                {
                    var TelephoneNumber = dicINformatoin["TelephoneNumber"];
                    if (TelephoneNumber != null) this.txtMobile.Text = dicINformatoin["TelephoneNumber"].ToString();
                    else this.txtMobile.Text = string.Empty;
                }
                if (dicINformatoin.ContainsKey("Department"))
                {
                    var Department = dicINformatoin["Department"];
                    if (Department != null) this.txtDepart.Text = dicINformatoin["Department"].ToString();
                    else this.txtDepart.Text = string.Empty;
                }
                if (dicINformatoin.ContainsKey("Title"))
                {
                    var Title = dicINformatoin["Title"];
                    if (Title != null) this.cmbZiWei.Text = dicINformatoin["Title"].ToString();
                    else this.cmbZiWei.Text = string.Empty;
                }
                if (dicINformatoin.ContainsKey("UserPrincipalName"))
                {
                    var UserPrincipalName = dicINformatoin["UserPrincipalName"];
                    if (UserPrincipalName != null) this.txtUnitFax.Text = dicINformatoin["UserPrincipalName"].ToString();
                    else this.txtUnitFax.Text = string.Empty;
                }
                if (dicINformatoin.ContainsKey("PhysicalDeliveryOfficeName"))
                {
                    var PhysicalDeliveryOfficeName = dicINformatoin["PhysicalDeliveryOfficeName"];
                    if (PhysicalDeliveryOfficeName != null) this.txtUnitCode.Text = dicINformatoin["PhysicalDeliveryOfficeName"].ToString();
                    else this.txtUnitCode.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "FillData", ex.ToString(), dicINformatoin);
            }
            finally
            {
            }
        }
    }
}
