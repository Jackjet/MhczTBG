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
using MhczTBG.Common;


namespace MhczTBG.Controls.RenYuanGuanLI
{
    /// <summary>
    /// UpdatePassWord.xaml 的交互逻辑
    /// </summary>
    public partial class UpdatePassWord : Window
    {

        #region 变量
        /// <summary>
        /// 是否修改标识
        /// </summary>
        bool canUpdate = false;

        #endregion

        #region 构造函数
        public UpdatePassWord()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "UpdatePassWord", ex.ToString());
            }
            finally
            {
            }
        }
        #endregion

        #region 事件区域
        /// <summary>
        /// 确定事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (pwdNew.Password == "")
                {
                    MessageBox.Show("密码不可为空");
                }
                else
                {
                    //执行密码修改方法
                    this.Close();

                    canUpdate = true;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btnOk_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btnClose_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        #endregion

    }
}
