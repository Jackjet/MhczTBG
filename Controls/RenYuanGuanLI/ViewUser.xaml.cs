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
    /// EditUser.xaml 的交互逻辑
    /// </summary>
    public partial class ViewUser : UserControl
    {
        #region 声明变量
        /// <summary>
        /// 字典数据
        /// </summary>
        public Dictionary<string, object> dicSaveData = null; 
        #endregion

        #region 构造函数
        public ViewUser()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ViewUser", ex.ToString());
            }
            finally
            {
            }
        } 
        #endregion

        #region 填充数据
        /// <summary>
        /// 清空方法
        /// </summary>
        public void Clear()
        {
            try
            {
                this.txtUserName.Text = string.Empty;
                this.txtEmail.Text = string.Empty;
                this.txtMobile.Text = string.Empty;
                this.txtDepart.Text = string.Empty;
                this.txtZhiWei.Text = string.Empty;
                this.txtUnitFax.Text = string.Empty;
                this.txtUnitCode.Text = string.Empty;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Clear", ex.ToString());
            }
            finally
            {
            }
        }
        /// <summary>
        /// 填充数据
        /// </summary>
        /// <param name="dicINformatoin">数据</param>
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
                    if (Title != null) this.txtZhiWei.Text = dicINformatoin["Title"].ToString();
                    else this.txtZhiWei.Text = string.Empty;
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
        #endregion
    }
}
