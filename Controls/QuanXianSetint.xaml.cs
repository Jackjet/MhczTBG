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


namespace MhczTBG.Controls
{
    /// <summary>
    /// RenYuanGuanLi.xaml 的交互逻辑
    /// </summary>
    public partial class QuanXianSetint : UserControl
    {
        #region 申明变量

        /// <summary>
        /// 最初的数据源
        /// </summary>
        public List<string> UserList = new List<string>();

        /// <summary>
        /// 用于存放树视图用到的数据源（通过规则将UserList转换为该类型的数据）
        /// </summary>
        List<string[]> TreeDataList = new List<string[]>();

        //收集的所有人员信息
        List<string> Allquanxian = new List<string>();

        /// <summary>
        /// 当前所选择的树节点
        /// </summary>
        TreeViewItem selectTreeItem = null;

        //存放读取权限的用户组 用于绑定listbox
        public List<string> listRead = new List<string>();

        //临时赋值字段
        string TreeHeader = string.Empty;

        //删除已有用户信息
        List<string> listdelete = new List<string>();

        //存放编辑权限的用户组 用于绑定listbox
        public List<string> listEdit = new List<string>();

        //已有权限列表
        List<string> listdemo = new List<string>();

        /// <summary>
        /// 存储根节点的名称
        /// </summary>
        string RootName = string.Empty;

        #endregion

        #region 自定义委托事件

        public delegate void GetTreeViewDataEventHandle(ref List<string> dicTreeData);
        /// <summary>
        /// 获取树视图的数据源
        /// </summary>
        public event GetTreeViewDataEventHandle GetTreeViewDataEvent = null;

        public delegate void SubmitEventHandle(List<string> quanXianList);
        /// <summary>
        /// 提交权限时激发该事件
        /// </summary>
        public event SubmitEventHandle SubmitEvent = null;


        #endregion

        #region 构造函数

        public QuanXianSetint()
        {
            try
            {
                InitializeComponent();

                #region 清除部门、人员信息（树视图）

                TreeViewUsers.Items.Clear();

                #endregion

                #region 注册事件

                this.Loaded += new RoutedEventHandler(RenYuanGuanLi_Loaded);

                #endregion
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "QuanXianSetint", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion

        #region 获取树状图信息

        /// <summary>
        /// 获取组织框架的信息源
        /// </summary>
        public void GetTreeData()
        {
            try
            {
                if (GetTreeViewDataEvent != null)
                {
                    //清空树视图所用的数据
                    UserList.Clear();

                    ///激发获取树视图数据源的事件
                    GetTreeViewDataEvent(ref UserList);

                    //获取到的数据不为空（则生成树视图）
                    if (this.UserList.Count > 0)
                    {

                        #region 生成树视图

                        foreach (var item in UserList)
                        {
                            //按照规则进行切割
                            TreeDataList.Add(item.Split(new char[] { ',' }));
                        }
                        //生成树视图
                        UserTreeInit(TreeDataList);

                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "GetTreeData", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion

        #region 生成树视图(部门、人员)

        /// <summary>
        /// 生成树视图
        /// </summary>
        /// <param name="treeDataList">数据源</param>
        public void UserTreeInit(List<string[]> treeDataList)
        {
            try
            {
                //清空树
                TreeViewUsers.Items.Clear();
                //已经添加的最顶层名称
                string 已经添加的段 = string.Empty;
                //已经添加的车间组
                string 已经添加的车间 = string.Empty;
                //已经添加的人员或者工区组
                string 已经添加的工区或车间人员 = string.Empty;
                //已经添加的工区人员
                string 已经添加的工区人员 = string.Empty;

                foreach (string[] item in treeDataList)
                {
                    #region 获取根节点
                    //申明一个树变量
                    TreeViewItem treeitemFirst = new TreeViewItem();
                    //获取到最顶层名称//北京通信段
                    string First = item[item.Count() - 3].Substring(3);
                    //父节点名称获取
                    RootName = First;
                    //如果不包含这些
                    if (!已经添加的段.Contains(First))
                    {
                        //父节点设置
                        TreeViewParentSetting(treeitemFirst, First, 已经添加的段);
                        //添加父节点
                        TreeViewUsers.Items.Add(treeitemFirst);

                        已经添加的段 += First;

                        #region 添加车间节点

                        foreach (string[] Chejianitem in treeDataList)
                        {
                            for (int i = Chejianitem.Count(); i > 0; i--)
                            {
                                //获取到第二层名称
                                string Second = Chejianitem[Chejianitem.Count() - 4].Substring(3);

                                if (!已经添加的车间.Contains(Second))
                                {
                                    //部门节点生成
                                    TreeViewItem treeitemSecond = new TreeViewItem();
                                    //部门节点设置
                                    TreeApartmentSetting(treeitemSecond, First, Second);
                                    //将部门节点加载到父节点里
                                    treeitemFirst.Items.Add(treeitemSecond);
                                    已经添加的车间 += Second;

                                    #region 添加车间人员或工区节点

                                    foreach (string[] Gongquitem in treeDataList)
                                    {
                                        if (Gongquitem.Count() > 4)
                                        {
                                            for (int j = Gongquitem.Count(); j > 0; j--)
                                            {
                                                string FatherThree = Gongquitem[Gongquitem.Count() - 4].Substring(3);
                                                string Three = Gongquitem[Gongquitem.Count() - 5].Substring(3);
                                                if (!已经添加的工区或车间人员.Contains(Three) && treeitemSecond.Header.ToString() == FatherThree)
                                                {
                                                    int a = Gongquitem.Count();
                                                    //工区组
                                                    if (Gongquitem.Count() > 5)
                                                    {
                                                        TreeViewItem treeitemThree1 = new TreeViewItem();
                                                        TreeApartmentSetting(treeitemThree1, First + "," + Second, Three);
                                                        treeitemSecond.Items.Add(treeitemThree1);
                                                        已经添加的工区或车间人员 += Three;

                                                        #region 添加工区用户
                                                        foreach (string[] GongquUseritem in treeDataList)
                                                        {
                                                            if (GongquUseritem.Count() > 5)
                                                            {
                                                                for (int k = GongquUseritem.Count(); k > 0; k--)
                                                                {
                                                                    string FatherFore = GongquUseritem[GongquUseritem.Count() - 5].Substring(3);
                                                                    string Fore = GongquUseritem[GongquUseritem.Count() - 6].Substring(3);

                                                                    if (!已经添加的工区人员.Contains(Fore) && treeitemThree1.Header.ToString() == FatherFore)
                                                                    {
                                                                        TreeViewItem treeitemFore = new TreeViewItem();
                                                                        //设置用户节点
                                                                        TreeUserSetting(treeitemFore, First + "," + Second + "," + Three, Fore, Gongquitem);
                                                                        //加载
                                                                        treeitemThree1.Items.Add(treeitemFore);
                                                                        已经添加的工区人员 += Fore;
                                                                    }

                                                                }
                                                            }
                                                        }
                                                        #endregion

                                                    }
                                                    else
                                                    {
                                                        if (!Three.Contains("组"))
                                                        {
                                                            TreeViewItem treeitemThree = new TreeViewItem();
                                                            //设置用户节点
                                                            TreeUserSetting(treeitemThree, First + "," + Second, Three, Gongquitem);
                                                            //加载
                                                            treeitemSecond.Items.Add(treeitemThree);
                                                            已经添加的工区或车间人员 += Three;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                }
                            }
                        }
                        #endregion
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "UserTreeInit", ex.ToString(), treeDataList);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 创建父节点（根）
        /// </summary>
        /// <param name="treeitemFirst">父节点</param>
        /// <param name="First">标示</param>
        /// <param name="已经添加的段">已经添加的段</param>
        void TreeViewParentSetting(TreeViewItem treeitemFirst, string First, string 已经添加的段)
        {
            try
            {
                treeitemFirst.Header = First;
                Style style1 = this.FindResource("TreeViewItemStyle1") as Style;
                treeitemFirst.Style = style1;
                treeitemFirst.Tag = First;
                treeitemFirst.IsExpanded = true;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TreeViewParentSetting", ex.ToString());
            }
            finally
            {
            }
        }

        /// <summary>
        /// 部门节点设置
        /// </summary>
        /// <param name="treeitemSecond">部门节点</param>
        /// <param name="FirstAnd">标示1</param>
        /// <param name="Next">标示2</param>     
        void TreeApartmentSetting(TreeViewItem treeitemSecond, string FirstAnd, string Next)
        {
            try
            {
                treeitemSecond.Header = Next;
                Style style2 = this.FindResource("TreeViewItemStyle2") as Style;
                treeitemSecond.Style = style2;
                treeitemSecond.Tag = Next + "," + FirstAnd;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "OKButton_Click", ex.ToString(), treeitemSecond, FirstAnd, Next);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 添加用户节点
        /// </summary>
        /// <param name="treeitem">树节点</param>
        /// <param name="First">标识</param>
        /// <param name="Second">标识1</param>
        /// <param name="Next">标识2</param>
        /// <param name="Gongquitem">工区</param>
        void TreeUserSetting(TreeViewItem treeitem, string FirstSecondAnd, string Next, string[] Gongquitem)
        {
            try
            {
                treeitem.Header = Next;
                Style style5 = this.FindResource("TreeViewItemStyle3") as Style;
                treeitem.Style = style5;

                treeitem.Tag = Next + "," + FirstSecondAnd;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TreeUserSetting", ex.ToString(), treeitem, FirstSecondAnd, Next, Gongquitem);
            }
            finally
            {
            }
        }

        #endregion

        #region UI事件区域

        /// <summary>
        /// 初始化加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void RenYuanGuanLi_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                GetTreeData();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "RenYuanGuanLi_Loaded", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #region 删除用户
        /// <summary>
        /// 删除已有权限
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgexit_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (listdemo.Count > 0)
                {
                    if (lboxUsers.SelectedIndex > -1 && listdemo[lboxUsers.SelectedIndex].Contains("/"))
                    {
                        //将需要删除的用户添加到删除组
                        listdelete.Add("," + listdemo[lboxUsers.SelectedIndex].Split(new char[] { '/' })[0] + "/删除");
                        //从已有用户组里面删除选择的用户
                        listdemo.RemoveAt(lboxUsers.SelectedIndex);

                        lboxUsers.Items.Clear();
                        foreach (var item in listdemo)
                        {
                            if (item.Contains(",")) lboxUsers.Items.Add(item.Split(new char[] { ',' })[0]);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "imgexit_MouseLeftButtonUp", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 删除编辑权限
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgexit2_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (EditList.SelectedIndex > -1)
                {
                    listEdit.RemoveAt(EditList.SelectedIndex);

                    EditList.Items.Clear();
                    foreach (var item in listEdit)
                    {
                        if (item.Contains(",")) EditList.Items.Add(item.Split(new char[] { ',' })[0]);
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "imgexit2_MouseLeftButtonUp_1", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 删除读取权限
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgexit3_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (ReadList.SelectedIndex > -1)
                {
                    listRead.RemoveAt(ReadList.SelectedIndex);

                    ReadList.Items.Clear();
                    foreach (var item in listRead)
                    {
                        if (item.Contains(",")) ReadList.Items.Add(item.Split(new char[] { ',' })[0]);
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "imgexit3_MouseLeftButtonUp", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        #endregion

        /// <summary>
        /// 添加编辑权限
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listEdit.Contains(TreeHeader + "/编辑") || listdemo.Contains(TreeHeader.Split(new char[] { ',' })[0] + "/编辑"))
                {
                    MessageBox.Show("已存在该部门");
                }
                else if (TreeHeader != "")
                {

                    listEdit.Add(TreeHeader + "/编辑");

                }
                EditList.Items.Clear();
                foreach (var item in listEdit)
                {
                    EditList.Items.Add(item.Split(new char[] { ',' })[0]);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btnAddEdit_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listdemo.Count > 0)
                {
                    //将需要删除的用户添加到删除组
                    listdelete.Add("," + listdemo[lboxUsers.SelectedIndex].Split(new char[] { '/' })[0] + "/删除");
                    //从已有用户组里面删除选择的用户
                    listdemo.RemoveAt(lboxUsers.SelectedIndex);

                    lboxUsers.Items.Clear();
                    foreach (var item in listdemo)
                    {
                        lboxUsers.Items.Add(item.Split(new char[] { ',' })[0]);
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btnCancel_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 提交权限表单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Allquanxian.Clear();
                Allquanxian.AddRange(listEdit);
                Allquanxian.AddRange(listRead);
                Allquanxian.AddRange(listdelete);
                if (SubmitEvent != null)
                {
                    SubmitEvent(Allquanxian);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btnSubmit_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 设置只读权限
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnreadAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listRead.Contains(TreeHeader + "/读取")
               || listdemo.Contains(TreeHeader.Split(new char[] { ',' })[0] + "/读取")
               || listEdit.Contains(TreeHeader + "/编辑"))
                {
                    MessageBox.Show("已存在该部门");
                }
                else if (TreeHeader != "")
                {

                    listRead.Add(TreeHeader + "/读取");

                }
                ReadList.Items.Clear();
                foreach (var item in listRead)
                {
                    ReadList.Items.Add(item.Split(new char[] { ',' })[0]);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btnreadAdd_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 树节点选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeviewUsers_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                TreeViewItem treeview = (TreeViewItem)((TreeView)sender).SelectedItem;

                if (treeview != null && treeview.Tag != null && treeview.Tag.ToString() != RootName)
                {
                    TreeHeader = treeview.Tag.ToString();
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "treeviewUsers_SelectedItemChanged", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion

        #region 皮肤更换

        /// <summary>
        /// 皮肤更换
        /// </summary>
        /// <param name="color">指定颜色</param>
        public void SkinChange(Color color)
        {
            try
            {
                this.gridTittle.Background = new SolidColorBrush(color) { Opacity = 0.7 };
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "SkinChange", ex.ToString(), color);
            }
            finally
            {
            }
        }

        #endregion
    }
}
