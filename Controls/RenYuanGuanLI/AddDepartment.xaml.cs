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
    /// AddDepartment.xaml 的交互逻辑
    /// </summary>
    public partial class AddDepartment : Window
    {
        #region 变量
        /// <summary>
        /// 判断添加部门
        /// </summary>
        bool CanAddDepartMent = false;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fatherName">父级名称</param>
        /// <param name="treeItem">父级节点</param>
        public AddDepartment()
        {
            try
            {
                InitializeComponent();
                this.KeyDown += new KeyEventHandler(AddDepartment_KeyDown);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "AddDepartment", ex.ToString());
            }
            finally
            {
            }
        }

        void AddDepartment_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    if (txtDepartment.Text == "")
                    {
                        txtThiSi.Text = "部门不能为空！";
                    }
                    else
                    {
                        this.Close();
                        CanAddDepartMent = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "AddDepartment_KeyDown", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion

        #region UI事件区域
        //确定按钮事件
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtDepartment.Text == "")
                {
                    txtThiSi.Text = "部门不能为空！";
                }
                else
                {
                    this.Close();
                    CanAddDepartMent = true;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btnOK_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        //取消按钮事件
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btnCancel_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        //文本框获取焦点事件
        private void txtDepartment_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                txtThiSi.Text = string.Empty;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "txtDepartment_GotFocus", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        //窗体移动
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
        #endregion

    }
}
