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
using MhczTBG.Controls.CustomWindow;
using MhczTBG.Common;


namespace MhczTBG.Controls.RenYuanGuanLI
{
    /// <summary>
    /// RenYuanGuanLi.xaml 的交互逻辑
    /// </summary>
    public partial class RenYuanGuanLi : UserControl
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

        /// <summary>
        /// 临时存储用户信息
        /// </summary>
        Dictionary<string, object> dicUserInformaion = new Dictionary<string, object>();

        /// <summary>
        /// 临时存储选择之后所存储的部门或人员信息
        /// </summary>
        string UserTag = string.Empty;

        /// <summary>
        /// 新建表单
        /// </summary>
        EditUser editUser = new EditUser();

        /// <summary>
        /// 编辑表单
        /// </summary>
        ViewUser viewUser = new ViewUser();

        /// <summary>
        /// email
        /// </summary>
        string strtTemail = string.Empty;

        /// <summary>
        /// 定义编辑名称
        /// </summary>
        string TagEdit = string.Empty;
        /// <summary>
        /// 父级关系名称
        /// </summary>
        string strTextsd = string.Empty;

        /// <summary>
        /// 选择的用户所属部门
        /// </summary>
        string SelectUserDepart = string.Empty;

        /// <summary>
        /// 树视图根的名称
        /// </summary>
        string strfg = string.Empty;

        /// <summary>
        /// 当前所选择的树节点
        /// </summary>
        TreeViewItem selectTreeItem = null;

        /// <summary>
        /// 查看是表单处于什么状态
        /// </summary>
        string isEdit = string.Empty;

        #endregion

        #region 自定义委托事件

        public delegate void GetTreeViewDataEventHandle(ref List<string> dicTreeData);
        /// <summary>
        /// 获取树视图的数据源
        /// </summary>
        public event GetTreeViewDataEventHandle GetTreeViewDataEvent = null;

        public delegate void GetUserInfomationEventHandle(string Tag, ref Dictionary<string, object> dicUserInformation);
        /// <summary>
        /// 获取人员详细信息
        /// </summary>
        public event GetUserInfomationEventHandle GetUserInfomationEvent = null;

        public delegate void AddApartmentEventHandle(string Tag, ref bool AddApartSuccessed);
        /// <summary>
        /// 添加部门
        /// </summary>
        public event AddApartmentEventHandle AddApartmentEvent = null;


        public delegate void DeleteApartmentEventHandle(string TagEdit, ref bool DeleteApartSuccessed);
        /// <summary>
        /// 删除部门
        /// </summary>
        public event DeleteApartmentEventHandle DeleteApartmentEvent = null;

        public delegate void AddUserEventHandle(Dictionary<string, object> dicUserInfomation, ref bool AddUserSuccessed);
        /// <summary>
        /// 添加用户
        /// </summary>
        public event AddUserEventHandle AddUserEvent = null;

        public delegate void DeleteUserEventHandle(string TagEdit, ref bool DeleteUserSuccessed);
        /// <summary>
        /// 删除用户
        /// </summary>
        public event DeleteUserEventHandle DeleteUserEvent = null;

        public delegate void EditUserEventHandle(Dictionary<string, object> dicUserInfomation);
        /// <summary>
        /// 编辑用户信息
        /// </summary>
        public event EditUserEventHandle EditUserEvent = null;


        public delegate void UpdatePasswordEventHandle(string TagPwd, string NewTagPwd);
        /// <summary>
        /// 修改密码的激发事件
        /// </summary>
        public event UpdatePasswordEventHandle UpdatePasswordEvent = null;

        #endregion

        #region 构造函数

        public RenYuanGuanLi()
        {
            try
            {
                InitializeComponent();

                #region 清除部门、人员信息（树视图）

                TreeViewUsers.Items.Clear();

                #endregion

                #region 清除人员信息（表单）

                this.borMain.Child = viewUser;

                #endregion

                #region 注册事件

                this.editUser.bntOK.Click += new RoutedEventHandler(bntOK_Click);
                this.editUser.bntQuxiao.Click += new RoutedEventHandler(bntQuxiao_Click);
                this.editUser.txtUnitFax.LostFocus += new RoutedEventHandler(txtUnitFax1_LostFocus);
                this.viewUser.txtUnitFax.LostFocus += new RoutedEventHandler(txtUnitFax1_LostFocus1);
                this.Loaded += new RoutedEventHandler(RenYuanGuanLi_Loaded);

                #endregion
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "RenYuanGuanLi", ex.ToString());
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
                        //清空表单数据
                        this.viewUser.Clear();

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
                    //如果不包含这些
                    if (!已经添加的段.Contains(First))
                    {
                        //邮箱号码
                        strtTemail = "@" + item[item.Count() - 2].Split(new char[] { '=' })[1] + ".com";
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
                treeitemFirst.Selected += new RoutedEventHandler(treeitemFirst_Selected);
                Style style1 = this.FindResource("TreeViewItemStyle1") as Style;
                treeitemFirst.Style = style1;
                treeitemFirst.Tag = First;
                strfg = treeitemFirst.Tag.ToString();
                treeitemFirst.IsExpanded = true;
                if (!已经添加的段.Contains(First))
                {
                    ContextMenu cm = new ContextMenu();
                    MenuItem mEditPlan = new MenuItem();
                    mEditPlan.Header = "添加部门和组";
                    mEditPlan.Tag = First;
                    mEditPlan.Click += new RoutedEventHandler(mEditPlan_Click);
                    cm.Items.Add(mEditPlan);
                    ContextMenuService.SetContextMenu(treeitemFirst, cm);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TreeViewParentSetting", ex.ToString(), treeitemFirst, First, 已经添加的段);
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
                treeitemSecond.Tag = FirstAnd + "," + Next;
                treeitemSecond.Selected += new RoutedEventHandler(treeitemSecond_Selected);


                ContextMenu cm1 = new ContextMenu();
                MenuItem mEditPlan1 = new MenuItem();
                mEditPlan1.Header = "添加用户";
                mEditPlan1.Tag = FirstAnd + "," + Next;
                mEditPlan1.Click += new RoutedEventHandler(mEditPlan_Click1);

                MenuItem mEditPlan2 = new MenuItem();
                mEditPlan2.Header = "删除此部门";
                mEditPlan2.Tag = FirstAnd + "," + Next;
                mEditPlan2.Click += new RoutedEventHandler(mEditPlan_Click2);
                cm1.Items.Add(mEditPlan1);
                cm1.Items.Add(mEditPlan2);
                ContextMenuService.SetContextMenu(treeitemSecond, cm1);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TreeApartmentSetting", ex.ToString(), treeitemSecond, FirstAnd, Next);
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
                treeitem.Selected += new RoutedEventHandler(treeitem2_Selected);

                string tag = "";
                for (int m = 0; m < Gongquitem.Length; m++)
                {
                    if (Gongquitem[m].Contains(";"))
                    {
                        tag += Gongquitem[m].Split(new char[] { ';' })[0] + ",";
                    }
                    else
                    {
                        tag += Gongquitem[m] + ",";
                    }
                }
                treeitem.Tag = tag.Substring(0, tag.LastIndexOf(",")).Split(new char[] { ';' })[0];

                ContextMenu cmgongqu = new ContextMenu();
                MenuItem mEditPlan4 = new MenuItem();
                mEditPlan4.Header = "编辑此用户信息";
                mEditPlan4.Tag = treeitem.Tag;
                mEditPlan4.Click += new RoutedEventHandler(mEditPlan_Click3);

                MenuItem mEditPlan5 = new MenuItem();
                mEditPlan5.Header = "删除此用户";
                mEditPlan5.Tag = FirstSecondAnd + "," + Next;
                mEditPlan5.Click += new RoutedEventHandler(mEditPlan_Click4);


                MenuItem mEditPlan6 = new MenuItem();
                mEditPlan6.Header = "修改密码";
                mEditPlan6.Tag = treeitem.Tag;
                mEditPlan6.Click += new RoutedEventHandler(mEditPlan_Click5);

                cmgongqu.Items.Add(mEditPlan4);
                cmgongqu.Items.Add(mEditPlan5);
                cmgongqu.Items.Add(mEditPlan6);
                ContextMenuService.SetContextMenu(treeitem, cmgongqu);
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

        #region 选择树节点激发事件

        /// <summary>
        /// 根节点（一个树视图只能有一个根节点）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void treeitemFirst_Selected(object sender, RoutedEventArgs e)
        {
            try
            {
                //设置选择的节点
                selectTreeItem = (TreeViewItem)sender;

                #region 按钮设置

                //添加部门
                btnAddUserAndApartment.Visibility = Visibility.Visible;
                btnAddUser.Visibility = Visibility.Collapsed;
                btnDeleteApartment.Visibility = Visibility.Collapsed;
                btnEditUserInfo.Visibility = Visibility.Collapsed;
                btnDeleteUser.Visibility = Visibility.Collapsed;
                btnPasswordUpadate.Visibility = Visibility.Collapsed;

                #endregion

                e.Handled = true;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "treeitemFirst_Selected", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 第二层树节点（部门）选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void treeitemSecond_Selected(object sender, RoutedEventArgs e)
        {
            try
            {
                //设置选择的节点
                selectTreeItem = (TreeViewItem)sender;
                //设置当前的绑定信息
                UserTag = selectTreeItem.Tag.ToString();

                #region 按钮设置

                //添加用户           
                btnAddUser.Visibility = Visibility.Visible;
                //删除部门
                btnDeleteApartment.Visibility = Visibility.Visible;
                btnAddUserAndApartment.Visibility = Visibility.Collapsed;
                btnEditUserInfo.Visibility = Visibility.Collapsed;
                btnDeleteUser.Visibility = Visibility.Collapsed;
                btnPasswordUpadate.Visibility = Visibility.Collapsed;

                #endregion

                //标示已处理
                e.Handled = true;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "treeitemSecond_Selected", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 人员选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void treeitem2_Selected(object sender, RoutedEventArgs e)
        {
            try
            {
                //设置选择的节点
                selectTreeItem = (TreeViewItem)sender;

                viewUser.Clear();

                #region 按钮设置

                btnAddUserAndApartment.Visibility = Visibility.Collapsed;
                btnAddUser.Visibility = Visibility.Collapsed;
                btnDeleteApartment.Visibility = Visibility.Collapsed;
                //修改用户信息
                btnEditUserInfo.Visibility = Visibility.Visible;
                //删除用户信息
                btnDeleteUser.Visibility = Visibility.Visible;
                //修改密码
                btnPasswordUpadate.Visibility = Visibility.Visible;

                #endregion

                #region 提供获取个人表单信息所使用

                TagEdit = "LDAP://" + ((TreeViewItem)sender).Tag.ToString();

                #endregion

                #region 提供删除表单信息所使用

                string d = ((TreeViewItem)sender).Tag.ToString();
                if (d.Contains(","))
                {
                    string[] UserList2 = d.Split(new char[] { ',' });
                    //初始化
                    strTextsd = string.Empty;
                    if (UserList2.Count() >= 5)
                    {
                        string parentLianjie = string.Empty;
                        for (int i = UserList2.Count() - 2; i > -1; i--)
                        {
                            if (i > 0)
                            {
                                strTextsd += UserList2[i].Substring(3) + ",";
                            }
                            else strTextsd += UserList2[i].Substring(3);
                        }
                        SelectUserDepart = UserList2[1].Substring(3);
                    }
                }



                #endregion

                #region 显示新建表单

                this.borMain.Child = viewUser;

                #endregion

                if (GetUserInfomationEvent != null && !string.IsNullOrEmpty(TagEdit))
                {
                    //获取用户信息（外部获取）
                    GetUserInfomationEvent(TagEdit, ref dicUserInformaion);
                    //绑定表单用户信息
                    if (dicUserInformaion != null && dicUserInformaion.Count > 0) viewUser.FillData(dicUserInformaion);
                }
                e.Handled = true;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "treeitem2_Selected", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion

        #region UI事件区域

        /// <summary>
        ///  初始化加载
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

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void mEditPlan_Click2(object sender, RoutedEventArgs e)
        {
            try
            {
                DelBuMen(UserTag);
                borMain.Child = viewUser;
                viewUser.Clear();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "mEditPlan_Click2", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 删除此部门
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteApartment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DelBuMen(UserTag);
                borMain.Child = viewUser;
                viewUser.Clear();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btnDeleteApartment_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        ///  添加部门和组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void mEditPlan_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //获取菜单所绑定的Tag值（名称）
                String menuitem = ((MenuItem)sender).Tag.ToString();
                AddDepartment(menuitem);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "mEditPlan_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 添加部门和组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddUserAndApartment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddDepartment(strfg);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btnAddUserAndApartment_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                isEdit = "新建";
                //添加用户设置        
                borMain.Child = editUser;

                string strText = string.Empty;
                if (UserTag.Contains(',')) strText = UserTag.Split(new char[] { ',' })[UserTag.Split(new char[] { ',' }).Count() - 1];
                //新建状态
                editUser.Clear(SelectUserDepart);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btnAddUser_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void mEditPlan_Click1(object sender, RoutedEventArgs e)
        {
            try
            {
                isEdit = "新建";
                //添加用户设置          
                borMain.Child = editUser;

                string strText = string.Empty;
                if (UserTag.Contains(',')) strText = UserTag.Split(new char[] { ',' })[UserTag.Split(new char[] { ',' }).Count() - 1];
                //新建状态
                editUser.Clear(SelectUserDepart);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "mEditPlan_Click1", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 编辑此用户信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditUserInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                isEdit = "编辑";
                //更新窗体
                borMain.Child = editUser;
                editUser.Clear(SelectUserDepart);
                editUser.FillData(viewUser.dicSaveData);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btnEditUserInfo_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 编辑此用户信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void mEditPlan_Click3(object sender, RoutedEventArgs e)
        {
            try
            {
                isEdit = "编辑";
                //更新窗体
                borMain.Child = editUser;
                TagEdit = "LDAP://" + ((MenuItem)sender).Tag.ToString();
                editUser.Clear(SelectUserDepart);
                editUser.FillData(viewUser.dicSaveData);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "mEditPlan_Click3", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 表单保存事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bntOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (isEdit)
                {
                    case "新建":

                        //执行新建用户方法
                        Dictionary<string, object> dicNewData = AddUser();
                        viewUser.FillData(dicNewData);
                        //editUser.Clear();
                        borMain.Child = viewUser;
                        //viewUser.FillData(editUser.dicSaveData);
                        break;

                    case "编辑":
                        //执行编辑更新方法
                        Dictionary<string, object> dicEditData = editUsers();
                        viewUser.FillData(dicEditData);
                        //清空所有值
                        borMain.Child = viewUser;
                        //viewUser.FillData(editUser.dicSaveData);
                        break;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "bntOK_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 新建表单取消事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bntQuxiao_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                viewUser.Clear();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "bntQuxiao_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 删除此用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool isDelete = Deluser(strTextsd);
                if (isDelete)
                {
                    borMain.Child = viewUser;
                    viewUser.Clear();
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btnDeleteUser_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 删除此用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void mEditPlan_Click4(object sender, RoutedEventArgs e)
        {
            try
            {
                bool isDelete = Deluser(strTextsd);
                if (isDelete)
                {
                    borMain.Child = viewUser;
                    viewUser.Clear();
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "mEditPlan_Click4", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void mEditPlan_Click5(object sender, RoutedEventArgs e)
        {
            try
            {
                string tagPwd = "LDAP://" + ((MenuItem)sender).Tag.ToString();
                UpdatePassWord updatePwd = new UpdatePassWord();
                updatePwd.ShowDialog();

                if (!string.IsNullOrEmpty(updatePwd.textBlock1.Text))
                {
                    if (this.UpdatePasswordEvent != null)
                    {
                        this.UpdatePasswordEvent(tagPwd, updatePwd.textBlock1.Text.Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "mEditPlan_Click5", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPasswordUpadate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UpdatePassWord updatePwd = new UpdatePassWord();
                updatePwd.ShowDialog();

                if (!string.IsNullOrEmpty(updatePwd.textBlock1.Text))
                {
                    if (this.UpdatePasswordEvent != null)
                    {
                        this.UpdatePasswordEvent(TagEdit, updatePwd.textBlock1.Text.Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btnPasswordUpadate_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 新建表单中登录名失去焦点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txtUnitFax1_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                //通过登录名添加email
                string txtLogin = editUser.txtUnitFax.Text.Trim();
                editUser.txtEmail.Text = txtLogin + strtTemail;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "txtUnitFax1_LostFocus", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 编辑表单中登录名失去焦点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txtUnitFax1_LostFocus1(object sender, RoutedEventArgs e)
        {
            try
            {
                //通过登录名添加email
                string txtLogin1 = viewUser.txtUnitFax.Text.Trim();
                viewUser.txtEmail.Text = txtLogin1 + strtTemail;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "txtUnitFax1_LostFocus", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion

        #region 辅助方法

        #region 添加部门
        /// <summary>
        /// 添加部门
        /// </summary>
        /// <param name="parentName">部门名称</param>
        void AddDepartment(string parentName)
        {
            try
            {
                //创建添加部门窗体
                AddDepartment adddepartment = new AddDepartment();
                //显示
                adddepartment.ShowDialog();
                //获取要添加的部门名称
                string dePartment = adddepartment.txtDepartment.Text.Trim();

                bool AddDepartSuccessed = false;

                //激发添加部门的事件
                if (AddApartmentEvent != null) AddApartmentEvent(parentName + "," + dePartment, ref AddDepartSuccessed);

                if (selectTreeItem != null && !string.IsNullOrEmpty(parentName) && !string.IsNullOrEmpty(dePartment) && AddDepartSuccessed)
                {
                    TreeViewItem item = new TreeViewItem();
                    //部门设置
                    TreeApartmentSetting(item, parentName, dePartment);
                    //添加部门
                    selectTreeItem.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "AddDepartment", ex.ToString(), parentName);
            }
            finally
            {
            }
        }

        #endregion

        #region 删除部门

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="tag">父级关系</param>
        void DelBuMen(string tag)
        {
            try
            {
                if (UserTag.Length > 0)
                {
                    MessageWindow messageWindow = new MessageWindow("确认删除吗？");
                    messageWindow.CancelButton.Visibility = Visibility.Visible;
                    messageWindow.Closed += (object sender1, EventArgs e1) =>
                    {
                        bool? result = messageWindow.DialogResult;
                        if (result.HasValue && result.Value)
                        {
                            bool DeleteApartSuccessed = false;
                            //激发删除部门的事件
                            if (DeleteApartmentEvent != null) DeleteApartmentEvent(tag, ref DeleteApartSuccessed);

                            if (!string.IsNullOrEmpty(tag) && tag.Contains(",") && DeleteApartSuccessed)
                            {
                                //获取当前选择的部门名称
                                string buMenName = tag.Split(new char[] { ',' })[tag.Split(new char[] { ',' }).Count() - 1];
                                if (selectTreeItem.Parent != null && selectTreeItem.Parent is TreeViewItem)
                                {
                                    TreeViewItem parentTreeItem = selectTreeItem.Parent as TreeViewItem;
                                    //获取父节点（遍历子节点）
                                    foreach (var item in parentTreeItem.Items)
                                    {
                                        if ((item as TreeViewItem).Header.Equals(buMenName))
                                        {
                                            //删除部门
                                            parentTreeItem.Items.Remove(item);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    };
                    messageWindow.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DelBuMen", ex.ToString(), tag);
            }
            finally
            {
            }
        }

        #endregion

        #region 添加用户

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> AddUser()
        {
            Dictionary<string, object> dirParmsDel = new Dictionary<string, object>();
            try
            {
                dirParmsDel.Add("OUroot", Check(UserTag));
                dirParmsDel.Add("DisplayName", Check(editUser.txtUserName.Text));
                dirParmsDel.Add("UserPrincipalName", Check(editUser.txtUnitFax.Text));
                dirParmsDel.Add("PhysicalDeliveryOfficeName", Check(editUser.txtUnitCode.Text));
                dirParmsDel.Add("TelephoneNumber", Check(editUser.txtMobile.Text));
                dirParmsDel.Add("Mail", Check(editUser.txtEmail.Text));
                dirParmsDel.Add("Department", Check(editUser.txtDepart.Text));
                dirParmsDel.Add("Title", Check(editUser.cmbZiWei.SelectionBoxItem.ToString()));

                bool AddUserSuccessed = false;

                if (AddUserEvent != null)
                {
                    AddUserEvent(dirParmsDel, ref AddUserSuccessed);
                }

                string strText = string.Empty;
                if (UserTag.Contains(',') && AddUserSuccessed)
                {
                    var separete = UserTag.Split(new char[] { ',' });
                    strText = separete[separete.Count() - 1];

                    if (selectTreeItem != null && !string.IsNullOrEmpty(editUser.txtUserName.Text) && !string.IsNullOrEmpty(strText))
                    {
                        //创建一个节点
                        TreeViewItem item = new TreeViewItem();

                        if (TreeDataList.Count > 0 && TreeDataList[0].Count() > 1 && TreeDataList[0][0].Contains("=") && TreeDataList[0][1].Contains("="))
                        {
                            ///获取用户的关键字标示
                            var UserKey = TreeDataList[0][0].Substring(0, TreeDataList[0][0].IndexOf('=') + 1);
                            //获取部门的关键字标示
                            var DepartmentKey = TreeDataList[0][1].Substring(0, TreeDataList[0][1].IndexOf('=') + 1);

                            //临时存储新建标示
                            List<string> gongwqu = new List<string>();

                            //用户
                            gongwqu.Add(UserKey + editUser.txtUserName.Text);

                            for (int i = 0; i < separete.Count(); i++)
                            {
                                gongwqu.Add(DepartmentKey + separete[i]);
                            }

                            //添加bjtxd(案例)
                            gongwqu.Add(TreeDataList[0][TreeDataList[0].Count() - 2]);
                            //添加com(案例)
                            gongwqu.Add(TreeDataList[0][TreeDataList[0].Count() - 1]);

                            //父类名称（多重连接）
                            string parentNameAnd = UserTag.Replace(("," + editUser.txtUserName.Text), string.Empty);

                            //用户树设置
                            TreeUserSetting(item, parentNameAnd, editUser.txtUserName.Text, gongwqu.ToArray());

                            //添加用户
                            selectTreeItem.Items.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "AddUser", ex.ToString());
            }
            finally
            {
            }
            return dirParmsDel;
        }

        /// <summary>
        ///字段值检验
        /// </summary>
        /// <param name="strText">字符串</param>
        /// <returns></returns>
        string Check(string strText)
        {
            string result = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(strText))
                    result = " ";
                else
                    result = strText;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Check", ex.ToString(), strText);
            }
            finally
            {
            }
            return result;
        }

        #endregion

        #region 编辑用户信息

        /// <summary>
        /// 编辑用户信息
        /// </summary>
        Dictionary<string, object> editUsers()
        {
            Dictionary<string, object> dirEdUser = new Dictionary<string, object>();
            try
            {
                dirEdUser.Add("LDAP", Check(TagEdit));
                dirEdUser.Add("DisplayName", Check(editUser.txtUserName.Text));
                dirEdUser.Add("UserPrincipalName", Check(editUser.txtUnitFax.Text));
                dirEdUser.Add("PhysicalDeliveryOfficeName", Check(editUser.txtUnitCode.Text));
                dirEdUser.Add("TelephoneNumber", Check(editUser.txtMobile.Text));
                dirEdUser.Add("Mail", Check(editUser.txtEmail.Text));
                dirEdUser.Add("Department", Check(editUser.txtDepart.Text));
                dirEdUser.Add("Title", Check(editUser.cmbZiWei.Text));

                if (this.EditUserEvent != null)
                {
                    this.EditUserEvent(dirEdUser);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "editUsers", ex.ToString());
            }
            finally
            {
            }
            return dirEdUser;
        }

        #endregion

        #region 删除用户信息

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="tag"></param>
        bool Deluser(string tag)
        {
            bool retunResult = false;
            try
            {
                if (strTextsd.Length > 0)
                {
                    MessageWindow messageWindow = new MessageWindow("确认删除吗？");
                    messageWindow.CancelButton.Visibility = Visibility.Visible;
                    messageWindow.Closed += (object sender1, EventArgs e1) =>
                    {
                        bool? result = messageWindow.DialogResult;
                        if (result.HasValue && result.Value)
                        {
                            bool DeleteUserSuccessed = false;

                            //激发删除部门的事件
                            if (DeleteUserEvent != null) DeleteUserEvent(tag, ref DeleteUserSuccessed);

                            if (!string.IsNullOrEmpty(tag) && tag.Contains(",") && DeleteUserSuccessed)
                            {
                                //获取当前选择的部门名称
                                string user = tag.Split(new char[] { ',' })[tag.Split(new char[] { ',' }).Count() - 1];
                                if (selectTreeItem.Parent != null && selectTreeItem.Parent is TreeViewItem)
                                {
                                    TreeViewItem parentTreeItem = selectTreeItem.Parent as TreeViewItem;
                                    //获取父节点（遍历子节点）
                                    foreach (var item in parentTreeItem.Items)
                                    {
                                        if ((item as TreeViewItem).Header.Equals(user))
                                        {
                                            //删除部门
                                            parentTreeItem.Items.Remove(item);
                                            retunResult = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    };
                    messageWindow.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Deluser", ex.ToString(), tag);
            }
            finally
            {
            }
            return retunResult;
        }

        #endregion

        /// <summary>
        /// 皮肤设置
        /// </summary>
        /// <param name="color">指定颜色</param>
        public void SkinChange(Color color)
        {
            try
            {
                this.borTittleLeft.Background = this.gridTittleRight.Background = new SolidColorBrush(color) { Opacity = 0.7 }; 
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
