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
using Microsoft.Win32;
using System.IO;

namespace MhczTBG.Helper
{
    /// <summary>
    /// GuZhangShouli_New.xaml 的交互逻辑
    /// </summary>
    public partial class GuZhangShouLi_New : UserControl
    {
        #region 一、声明变量

        //用于提交日志而收集的字典集合(创建表单日志)
        Dictionary<string, string> dicNotesCollect = new Dictionary<string, string>();

        #endregion

        #region 二、构造函数

        public GuZhangShouLi_New()
        {
            InitializeComponent();

            ////得到当前用户信息
            //this.GetCurrentUser();

            ////初始化表单
            //this.FormInit();

            ////绑定参数
            //this.GetComboBoxParmes();

            #region 注册事件

            //this.btnCancel.Click += new RoutedEventHandler(btnCancel_Click);
            //this.btn附件.Click += new RoutedEventHandler(btn附件_Click);
            //this.btnSubmit.Click += new RoutedEventHandler(btnSubmit_Click);

            //this.cmb申告单位.SelectionChanged += new SelectionChangedEventHandler(cmb申告单位_SelectionChanged);
            //this.cmb责任单位.SelectionChanged += new SelectionChangedEventHandler(cmb责任单位_SelectionChanged);
            //this.cmb设备类型一级.DropDownClosed += new EventHandler(cmb设备类型一级_DropDownClosed);

            //this.time修复时间.LostFocus += new RoutedEventHandler(time日期时间_LostFocus);
            //this.time修复时间.MouseLeave += new MouseEventHandler(time日期时间_LostFocus);

            //this.dp修复日期.LostFocus += new RoutedEventHandler(time日期时间_LostFocus);

            //this.dp发生日期.LostFocus += new RoutedEventHandler(time日期时间_LostFocus);

            //this.time发生时间.LostFocus += new RoutedEventHandler(time日期时间_LostFocus);
            //this.time发生时间.MouseLeave += new MouseEventHandler(time日期时间_LostFocus);

            //this.YN受理.Click += new RoutedEventHandler(YN受理_Click);
            //this.btn选择人员.Click += new RoutedEventHandler(btn选择人员_Click);

            #endregion
        }

        #endregion

       
    }
}
