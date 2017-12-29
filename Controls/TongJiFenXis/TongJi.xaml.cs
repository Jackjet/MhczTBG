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
using System.Windows.Threading;
using System.Threading;
using System.Reflection;
using System.ComponentModel;
using System.Data;
using MhczTBG.Common;
using MhczTBG.Controls.Print;
using MhczTBG.Controls;
using System.Diagnostics;
using System.Runtime.InteropServices;
using MhczTBG.Controls.TongJiDataGrid;
using MhczTBG.Controls.CustomWindow;
using MhczTBG.Controls.DataGridOperate;
//using Microsoft.Office.Interop.Excel;


namespace MhczTBG.Controls.TongJiFenXis
{
    /// <summary>
    /// TongQiTongJi.xaml 的交互逻辑
    /// </summary>
    public partial class TongJi : UserControl
    {
        #region 变量

        /// <summary>
        /// 子项高度的标准
        /// </summary>
        public int intItemHeight = 38;

        /// <summary>
        ///当前页
        /// </summary>
        int intPageNow = 1;

        /// <summary>
        /// 开始下标
        /// </summary>
        int intStart;

        /// <summary>
        /// 结束下标
        /// </summary>
        int intEnd;

        /// <summary>
        /// 剩余
        /// </summary>
        int intPageShengYu;

        /// <summary>
        /// 统计集（统计stackPanel1所包含的子项）
        /// </summary>
        List<TongJiItem> listMain = new List<TongJiItem>();

        /// <summary>
        /// 第一列的宽度
        /// </summary>
        double layoutRootOneWidth = 0;

        /// <summary>
        /// 行标题内部名称集
        /// </summary>
        List<string> rowItemTagList = new List<string>();

        /// <summary>
        /// 列标题内部名称集
        /// </summary>
        List<string> columnItemTagList = new List<string>();

        /// <summary>
        /// 横标题集合
        /// </summary>
        List<string> _hengTittleList = new List<string>();

        /// <summary>
        /// 竖标题集合
        /// </summary>
        List<string> _shuTittleList = new List<string>();

        /// <summary>
        /// 后台数据是否更改
        /// </summary>
        bool _isDataChanged = false;

        /// <summary>
        /// 统计表
        /// </summary>
        TJGridView _dataGrid = null;

        /// <summary>
        /// 统计界面与表单界面切换时禁用按钮的方式（打印，导入Excel）
        /// </summary>
        Selection _ChangeMode = Selection.Hiden;

        /// <summary>
        /// 横标题存储实例
        /// </summary>
        List<DataGridUserEntityItem> _hengLL = new List<DataGridUserEntityItem>();

        /// <summary>
        /// 列标题存储实例
        /// </summary>
        List<DataGridUserEntityItem> _ShuLL = new List<DataGridUserEntityItem>();

        /// <summary>
        /// 存储的列表实例集合
        /// </summary>
        BindingList<DataGridUserEntity> _listDataGridUserEntity = null;

        /// <summary>
        /// 显示列表预览的列表
        /// </summary>
        ListYuLanDataGrid _listDataGrid = null;

        /// <summary>
        /// 提示控件
        /// </summary>
        MessageShow _message = new MessageShow();

        /// <summary>
        /// 当前的列表实体
        /// </summary>
        DataGridUserEntity _NowDataGridUserEntity = null;

        /// <summary>
        /// 模拟生成行标题存储区域
        /// </summary>
        StackPanel stack1 = null;

        /// <summary>
        /// 模拟生成列标题存储区域
        /// </summary>
        StackPanel stack2 = null;

        /// <summary>
        /// excel应用程序
        /// </summary>
        Microsoft.Office.Interop.Excel.Application _excelother = null;

        /// <summary>
        /// 皮肤画刷
        /// </summary>
        string _brushKeyName = string.Empty;

        /// <summary>
        /// 字体颜色
        /// </summary>
        Brush _txtForeground = default(Brush);
        
        #endregion

        #region 自定义事件委托

        public delegate void ItemToCenterEventHandle(TongJiItem item, UIElementCollection ParentChirden, ref bool needDisConnected);
        /// <summary>
        /// 拖动到中间时激发的事件
        /// </summary>
        public event ItemToCenterEventHandle _ItemMoveEvent = null;

        public delegate void ItemCanNotSameExitEventHandle(ref string Property1, ref string property2);
        /// <summary>
        /// 指定属于父子级关系的两个字段不可分别存在于横标题和纵标题中
        /// </summary>
        public event ItemCanNotSameExitEventHandle _ItemCanNotSameExitEvent = null;

        #endregion

        #region 构造函数

        public TongJi()
        {
            try
            {
                InitializeComponent();

                #region 注册事件

                //容器尺寸改变时
                this.SizeChanged += new SizeChangedEventHandler(stackPanel1_SizeChanged);
                this.btnChange.Click += new RoutedEventHandler(btnChange_Click);
                this.btnInit.Click += new RoutedEventHandler(btnInit_Click);

                //打印
                this.comSearch.btf3.Click += new RoutedEventHandler(btf3_Click);

                this.comSearch.btExcel.Click += new RoutedEventHandler(btExcel_Click);

                this.comSearch.btnSearch.Click += new RoutedEventHandler(btnSearch_Click);

                this.comSearch.btnListSave.Click += new RoutedEventHandler(btnListSave_Click);

                Application.Current.Exit += new ExitEventHandler(Current_Exit);

                #endregion

                //初始化统计项
                PageInit();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TongJi", ex.ToString());
            }
        }
    
        #endregion

        #region 统计初始化

        /// <summary>
        /// 初始化统计项，计数为默认选项
        /// </summary>       
        private void PageInit()
        {
            try
            {
                //隐藏标题视图控件
                this.comSearch.btnShiTu.Visibility = System.Windows.Visibility.Collapsed;
                //禁用
                ButtonEnableSettingFalse();
                //启动竖方向打印
                this.comSearch.shuCheck.IsChecked = true;
                //存储表单预览区域可见
                this.comSearch.borListYuLan.Visibility = System.Windows.Visibility.Visible;
                //获取存储表单
                _listDataGridUserEntity = CommonMethod.GetListControl();
                if (_listDataGridUserEntity == null) _listDataGridUserEntity = new BindingList<DataGridUserEntity>();
                //生成显示存储表单的列表
                _listDataGrid = new ListYuLanDataGrid();
                //注册点击事件
                _listDataGrid._ButtonClickEvent += new ListYuLanDataGrid.ButtonClickEventHandle(listDataGrid_ButtonClickEvent);
                //注册移除事件
                _listDataGrid._BtnRemoveEvent += new ListYuLanDataGrid.BtnRemoveEventHandle(_listDataGrid__BtnRemoveEvent);
                //绑定存储表单集
                _listDataGrid.dataGrid.ItemsSource = _listDataGridUserEntity;
                //悬浮控件加载列表
                this.comSearch.fcmListYuLan.AddItem(_listDataGrid);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "PageInit", ex.ToString());
            }
            finally
            {
            }
        }

        /// <summary>
        /// 移除存储列表事件
        /// </summary>
        /// <param name="ID"></param>
        void _listDataGrid__BtnRemoveEvent(int ID)
        {
            try
            {
                //移除当前存储列表
                _listDataGridUserEntity.RemoveAt(ID);

                //重新排序
                for (int i = 0; i < _listDataGridUserEntity.Count; i++)
                {
                    _listDataGridUserEntity[i].ID = i;
                }
                //保存
                CommonMethod.SaveListList(_listDataGridUserEntity);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "_listDataGrid__BtnRemoveEvent", ex.ToString(), ID);
            }
            finally
            {
            }
        }

        #endregion

        #region 点击预览表单按钮生成存储的表单

        /// <summary>
        /// 点击预览表单按钮生成存储的表单
        /// </summary>
        /// <param name="ID"></param>
        void listDataGrid_ButtonClickEvent(int ID)
        {
            try
            {
                TomDisPatcherLb tomdisPatcher = new TomDisPatcherLb(new System.Action(() =>
                        {
                            #region 通过ID生成存储表单,并显示

                            //通过ID号获取相应的存储表单
                            var dataGridUserEntity = _listDataGridUserEntity[ID];

                            //生成统计列表
                            if (_dataGrid == null)
                            {
                                _dataGrid = new TJGridView();
                                _dataGrid.SkingSetting(_brushKeyName, _txtForeground);
                            }
                            else
                                _dataGrid.ReSet();
                            //模拟生成行标题存储区域
                            stack1 = new StackPanel();
                            //模拟生成列标题存储区域
                            stack2 = new StackPanel();
                            //通过遍历添加行标题
                            foreach (var item in dataGridUserEntity.DicHengList)
                            {
                                //生成统计子项
                                TongJiItem tongjiItem = new TongJiItem(item.Tittle, item.StrProperty, false);
                                //一级标题字段
                                tongjiItem.ItemChild = item.ItemChild;
                                //二级标题字段
                                tongjiItem.ItemChild2 = item.ItemChild2;
                                //加载统计子项
                                stack1.Children.Add(tongjiItem);
                            }
                            //通过遍历添加列标题
                            foreach (var item in dataGridUserEntity.DicShuList)
                            {
                                //生成统计子项
                                TongJiItem tongjiItem = new TongJiItem(item.Tittle, item.StrProperty, false);
                                //一级标题字段
                                tongjiItem.ItemChild = item.ItemChild;
                                //二级标题字段
                                tongjiItem.ItemChild2 = item.ItemChild2;
                                //加载统计子项
                                stack2.Children.Add(tongjiItem);
                            }
                            //生成存储表单
                            TitleInit(_dataGrid, stack1, stack2);
                            //加载存储表单
                            this.bordDataGrid.Child = _dataGrid;
                            //初始化设置
                            if (bordDataGrid.Child != null && bordDataGrid.Child is TJGridView)
                            {
                                this.gridTJ.ColumnDefinitions[0].Width = new GridLength(0);
                                this.bordDataGrid.Visibility = System.Windows.Visibility.Visible;
                                this.gridTJ.Visibility = System.Windows.Visibility.Collapsed;
                                //激活按钮
                                this.ButtonEnableSettingTrue();
                            }

                            _NowDataGridUserEntityInit(stack1, stack2);
                            #endregion

                            this.borTip.Visibility = System.Windows.Visibility.Collapsed;
                        }));


                TomDisPatcherLb tomdisPatcher2 = new TomDisPatcherLb(new System.Action(() =>
                  {
                      this.borTip.Visibility = System.Windows.Visibility.Visible;
                      tomdisPatcher.Start();
                  }));

                tomdisPatcher2.Start();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "listDataGrid_ButtonClickEvent", ex.ToString(), ID);
            }
            finally
            {
            }
        }

        #endregion

        #region 保存列表

        /// <summary>
        /// 保存列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnListSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_dataGrid != null)
                {
                    DataGridUserEntity other = new DataGridUserEntity(_NowDataGridUserEntity.HengTittle, _NowDataGridUserEntity.ShuTittle, _NowDataGridUserEntity.DicHengList, _NowDataGridUserEntity.DicShuList);
                    other.ID = _NowDataGridUserEntity.ID;

                    _message = new MessageShow();

                    bool canThrow = true;

                    foreach (var item in _listDataGridUserEntity)
                    {
                        if (item.HengTittle.Equals(other.HengTittle) && item.ShuTittle.Equals(other.ShuTittle))
                        {
                            _message.MessageContent = "该统计已存在";
                            canThrow = false;
                        }
                    }

                    if (canThrow)
                    {
                        //存储表单实例
                        bool result = CommonMethod.SaveListControl(_NowDataGridUserEntity);

                        if (result)
                        {
                            _message.MessageContent = "保存成功";
                            //添加到预览
                            _listDataGridUserEntity.Add(other);
                        }
                        else
                            _message.MessageContent = "保存失败";
                    }
                    _message.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btnListSave_Click", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion

        #region 左侧内容隐藏

        /// <summary>
        /// 左侧内容隐藏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnChange_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //打开左侧
                if (gridTJ.ColumnDefinitions[0].Width.Value == 0)
                {
                    if (bordDataGrid.Child != null && bordDataGrid.Child is TJGridView)
                    {
                        this.gridTJ.ColumnDefinitions[0].Width = new GridLength(layoutRootOneWidth);
                        this.bordDataGrid.Visibility = System.Windows.Visibility.Collapsed;
                        this.gridTJ.Visibility = System.Windows.Visibility.Visible;
                        //禁用按钮
                        this.ButtonEnableSettingFalse();
                    }
                }
                //关闭左侧
                else
                {
                    if (bordDataGrid.Child != null && bordDataGrid.Child is TJGridView)
                    {
                        this.gridTJ.ColumnDefinitions[0].Width = new GridLength(0);
                        this.bordDataGrid.Visibility = System.Windows.Visibility.Visible;
                        this.gridTJ.Visibility = System.Windows.Visibility.Collapsed;
                        //激活按钮
                        this.ButtonEnableSettingTrue();
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btnChange_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        #endregion

        #region 内容切换
        /// <summary>
        /// 内容切换
        /// </summary>
        /// <param name="intstart">开始索引</param>
        /// <param name="intend">结束索引</param>
        void Stack1ViewChange(int intstart, int intend)
        {
            try
            {
                this.stackPanel1.Children.Clear();
                for (int i = intstart; i < intend; i++)
                {
                    this.stackPanel1.Children.Add(listMain[i]);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Stack1ViewChange", ex.ToString(), intstart, intend);
            }
            finally
            {
            }
        }
        #endregion

        #region 改变尺寸调整容器内容

        /// <summary>
        /// 调整容器里的子项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void stackPanel1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                if (this.bordDataGrid.Visibility == System.Windows.Visibility.Collapsed)
                {
                    //容器里每一项的高度总和
                    double douItemsHeight = 0;
                    //叠加
                    foreach (var item in stackPanel1.Children)
                    {
                        douItemsHeight += (item as TongJiItem).ActualHeight;
                    }
                    //如果不是第一次发生，每当改变此存，对容器里子项的数量进行调整
                    if (douItemsHeight > 0)
                    {
                        DataPagerInit();
                    }

                    #region 第一列的宽度调节

                    layoutRootOneWidth = (this.gridTJ.ActualWidth - this.gridTJ.ColumnDefinitions[1].ActualWidth) / 6;
                    if (this.gridTJ.ColumnDefinitions[0].ActualWidth > 0)
                    {
                        this.gridTJ.ColumnDefinitions[0].Width = new GridLength(layoutRootOneWidth);
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "stackPanel1_SizeChanged", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }


        #endregion

        #region 生成子项

        /// <summary>
        /// 添加子项（通过字典目录）
        /// </summary>
        /// <param name="dicList"></param>
        public void ItemAddDic(TongJiItem item)
        {
            try
            {
                item.StrStyle = "borStyle1";
                //给子项注册拖拽事件
                this.SetElementEvent(item);
                //统计集添加子项
                this.listMain.Add(item);
                //调整分页容器
                this.DataPagerInit();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ItemAddDic", ex.ToString(), item);
            }
            finally
            {
            }
        }

        #endregion

        #region 给子项设置拖动功能

        /// <summary>
        /// 给一个子项设置拖动功能
        /// </summary>
        /// <param name="element">要设置的元素</param>
        void SetElementEvent(FrameworkElement element)
        {
            try
            {
                DragManage drago = new DragManage(this.gridTJ, element, this.stackPanel1);
                drago.DragLeftToRightComplete += new DragManage.DragLeftToRightCompleteHandle(drago_DragLeftToRightComplete);
                drago.DragRightToLeftComplete += new DragManage.DragRightToLeftCompleteHandle(drago_DragRightToLeftComplete);
                drago.DragLeftToCenterComplete += new DragManage.DragLeftToCenterCompleteHandle(drago_DragLeftToCenterComplete);
                drago.DragCenterToLeftComplete += new DragManage.DragCenterToLeftCompleteHandle(drago_DragCenterToLeftComplete);
                drago.DragRightToCenterComplete += new DragManage.DragRightToCenterCompleteHandle(drago_DragRightToCenterComplete);
                drago.DragCenterToRightComplete += new DragManage.DragCenterToRightCompleteHandle(drago_DragCenterToRightComplete);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "SetElementEvent", ex.ToString(), element);
            }
            finally
            {
            }
        }


        #endregion

        #region 拖动完成时激发

        /// <summary>
        /// 从左往右
        /// </summary>
        /// <param name="dragManage">拖动管理</param>
        /// <param name="element">拖动管理的元素</param>
        void drago_DragLeftToRightComplete(DragManage dragManage, FrameworkElement element)
        {
            try
            {
                //还原真实的面目
                TongJiItem item = element as TongJiItem;

                #region 父子级相斥
                //指定字段1
                string property1 = string.Empty;
                //指定字段2
                string property2 = string.Empty;
                //父子级排斥事件
                if (_ItemCanNotSameExitEvent != null)
                    _ItemCanNotSameExitEvent(ref property1, ref property2);
                //父子级排斥
                if (item.Tittle.Equals(property1))
                    if (_shuTittleList.Contains(property2)) return;
                //父子级排斥
                if (item.Tittle.Equals(property2))
                    if (_shuTittleList.Contains(property1)) return;

                #endregion

                bool needDisConnected = true;
                //激发给横标题添加字段的事件
                if (_ItemMoveEvent != null)
                    _ItemMoveEvent(item, this.stackPanelColumn.Children, ref needDisConnected);



                item.StrStyle = "borStyle3";
                if (needDisConnected)
                {
                    //断开与父容器的关系
                    (item.Parent as StackPanel).Children.Remove(item);
                    //添加到所移动到的容器
                    this.stackPanelColumn.Children.Add(item);
                }
                //调整分页容器
                DataPagerRemoveInit(item);
                //横标题添加字段
                _hengTittleList.Add(item.Tittle);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "drago_DragLeftToRightComplete", ex.ToString(), dragManage, element);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 从左往中
        /// </summary>
        /// <param name="dragManage">拖动管理</param>
        /// <param name="element">拖动管理的元素</param>
        void drago_DragLeftToCenterComplete(DragManage dragManage, FrameworkElement element)
        {
            try
            {
                //还原类型
                TongJiItem item = element as TongJiItem;

                #region 父子级相斥
                //指定字段1
                string property1 = string.Empty;
                //指定字段2
                string property2 = string.Empty;
                //父子级排斥事件
                if (_ItemCanNotSameExitEvent != null)
                    _ItemCanNotSameExitEvent(ref property1, ref property2);
                //父子级排斥
                if (item.Tittle.Equals(property1))
                    if (_hengTittleList.Contains(property2)) return;
                //父子级排斥
                if (item.Tittle.Equals(property2))
                    if (_hengTittleList.Contains(property1)) return;

                #endregion

                bool needDisConnected = true;
                //激发给竖标题添加字段的事件
                if (_ItemMoveEvent != null)
                    _ItemMoveEvent(item, this.stackPanelRow.Children, ref needDisConnected);

                item.StrStyle = "borStyle2";
                if (needDisConnected)
                {
                    //断开与父容器的关系
                    (item.Parent as StackPanel).Children.Remove(item);
                    //添加到所移动到的容器
                    this.stackPanelRow.Children.Add(item);
                }

                //调整分页容器
                DataPagerRemoveInit(item);
                //竖标题添加字段
                _shuTittleList.Add(item.Tittle);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "drago_DragLeftToCenterComplete", ex.ToString(), dragManage, element);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 从右往左
        /// </summary>
        /// <param name="dragManage">拖动管理</param>
        /// <param name="element">拖动管理的元素</param>
        void drago_DragRightToLeftComplete(DragManage dragManage, FrameworkElement element)
        {
            try
            {
                //还原真实的面目
                TongJiItem item = element as TongJiItem;

                //断开与父容器的关系
                (item.Parent as StackPanel).Children.Remove(item);
                item.StrStyle = "borStyle1";
                this.stackPanel1.Children.Add(item);
                item.Margin = new Thickness(0);
                //调整分页容器
                DataPagerAddInit(item);
                //横标题移除指定字段
                _hengTittleList.Remove(item.Tittle);
                //移除
                if (_hengTittleList.Contains(item.Tittle))
                    _hengTittleList.Remove(item.Tittle);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "drago_DragRightToLeftComplete", ex.ToString(), dragManage, element);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 中-->左
        /// </summary>
        /// <param name="dragManage">拖动管理</param>
        /// <param name="element">拖动管理的元素</param>
        void drago_DragCenterToLeftComplete(DragManage dragManage, FrameworkElement element)
        {
            try
            {
                //还原真实的面目
                TongJiItem item = element as TongJiItem;
                //断开与父容器的关系
                (item.Parent as StackPanel).Children.Remove(item);
                item.StrStyle = "borStyle1"; ;
                this.stackPanel1.Children.Add(item);
                item.Margin = new Thickness(0);
                //调整分页容器
                DataPagerAddInit(item);
                //竖标题移除指定字段              
                if (_shuTittleList.Contains(item.Tittle))
                    _shuTittleList.Remove(item.Tittle);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "drago_DragCenterToLeftComplete", ex.ToString(), dragManage, element);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 中--右
        /// </summary>
        /// <param name="dragManage">拖动管理</param>
        /// <param name="element">拖动管理的元素</param>
        void drago_DragCenterToRightComplete(DragManage dragManage, FrameworkElement element)
        {
            try
            {
                //还原真实的面目
                TongJiItem item = element as TongJiItem;

                #region 父子级相斥
                //指定字段1
                string property1 = string.Empty;
                //指定字段2
                string property2 = string.Empty;
                //父子级排斥事件
                if (_ItemCanNotSameExitEvent != null)
                    _ItemCanNotSameExitEvent(ref property1, ref property2);
                //父子级排斥
                if (item.Tittle.Equals(property1))
                    if (_shuTittleList.Contains(property2)) return;
                //父子级排斥
                if (item.Tittle.Equals(property2))
                    if (_shuTittleList.Contains(property1)) return;

                #endregion

                bool needDisConnected = true;
                //激发给横标题添加字段的事件
                if (_ItemMoveEvent != null)
                    _ItemMoveEvent(item, this.stackPanelColumn.Children, ref needDisConnected);

                item.StrStyle = "borStyle3"; ;
                if (needDisConnected)
                {
                    //断开与父容器的关系
                    (item.Parent as StackPanel).Children.Remove(item);
                    this.stackPanelColumn.Children.Add(item);
                }

                //横标题添加字段
                _hengTittleList.Add(item.Tittle);

                //竖标题移除指定字段              
                if (_shuTittleList.Contains(item.Tittle))
                    _shuTittleList.Remove(item.Tittle);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "drago_DragCenterToRightComplete", ex.ToString(), dragManage, element);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 从右到中
        /// </summary>
        /// <param name="dragManage">拖动管理</param>
        /// <param name="element">拖动管理的元素</param>
        void drago_DragRightToCenterComplete(DragManage dragManage, FrameworkElement element)
        {
            try
            {
                //还原真实的面目
                TongJiItem item = element as TongJiItem;

                #region 父子级相斥
                //指定字段1
                string property1 = string.Empty;
                //指定字段2
                string property2 = string.Empty;
                //父子级排斥事件
                if (_ItemCanNotSameExitEvent != null)
                    _ItemCanNotSameExitEvent(ref property1, ref property2);
                //父子级排斥
                if (item.Tittle.Equals(property1))
                    if (_hengTittleList.Contains(property2)) return;
                //父子级排斥
                if (item.Tittle.Equals(property2))
                    if (_hengTittleList.Contains(property1)) return;

                #endregion

                bool needDisConnected = true;
                //激发给竖标题添加字段的事件
                if (_ItemMoveEvent != null)
                    _ItemMoveEvent(item, this.stackPanelRow.Children, ref needDisConnected);

                item.StrStyle = "borStyle2";
                if (needDisConnected)
                {
                    //断开与父容器的关系
                    (item.Parent as StackPanel).Children.Remove(item);
                    this.stackPanelRow.Children.Add(item);
                }
                _shuTittleList.Add(item.Tittle);

                //竖标题移除指定字段              
                if (_hengTittleList.Contains(item.Tittle))
                    _hengTittleList.Remove(item.Tittle);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "drago_DragRightToCenterComplete", ex.ToString(), dragManage, element);
            }
            finally
            {
            }
        }



        /// <summary>
        /// 判断一个容器里是否包含月字段
        /// </summary>
        /// <param name="stack">容器</param>
        /// <returns></returns>
        bool YueIsEixtOrNo(StackPanel stack)
        {
            bool result = false;
            try
            {
                foreach (var child in stack.Children)
                {
                    TongJiItem item = child as TongJiItem;
                    if (item.Tittle.Contains("月份"))
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "YueIsEixtOrNo", ex.ToString(), stack);
            }
            finally
            {
            }
            return result;
        }

        /// <summary>
        /// 月字段是否可以进入指定容器
        /// </summary>
        /// <param name="stack">指定容器</param>
        /// <returns></returns>
        bool YueCanTo(StackPanel stack)
        {
            bool result = true;
            try
            {
                if (stack.Children.Count > 0)
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "YueCanTo", ex.ToString(), stack);
            }
            finally
            {
            }
            return result;
        }

        #endregion

        #region 调整分页容器
        /// <summary>
        /// 调整分页容器(初始化，调整尺寸)
        /// </summary>
        void DataPagerInit()
        {
            try
            {
                DispatcherTimer diapatcherTimer = new DispatcherTimer();
                diapatcherTimer.Interval = TimeSpan.FromSeconds(0.1);
                diapatcherTimer.Tick += (object sender, EventArgs e) =>
                {
                    //stackPanel1所能装载的数量
                    int pageSize = (int)this.stackPanel1.ActualHeight / intItemHeight;
                    //添加前几个子项（容器所能承载的范围内）
                    if (this.listMain.Count > pageSize)
                    {
                        //添加之间先清除
                        this.stackPanel1.Children.Clear();
                        //添加前几个子项（容器所能承载的范围内）
                        for (int i = 0; i < pageSize; i++)
                        {
                            //从统计里获取
                            this.stackPanel1.Children.Add(listMain[i]);
                        }
                    }
                    //容器足够承载
                    else
                    {
                        //添加之间先清除
                        this.stackPanel1.Children.Clear();
                        //添加统计集里所有子项
                        for (int i = 0; i < listMain.Count; i++)
                        {
                            this.stackPanel1.Children.Add(listMain[i]);
                        }
                    }
                    this.intStart = 0;
                    this.intEnd = this.stackPanel1.Children.Count;
                    this.intPageShengYu = this.listMain.Count - this.stackPanel1.Children.Count;
                    intPageNow = 1;
                    DataPagerDisP(pageSize);
                    #region 旧方案

                    ////统计stackPanel1里的数量
                    //this.txtCount.Text = this.listMain.Count.ToString();
                    //this.txtPage.Text = string.Format("{0}/{1}", intPageNow, (int)Math.Ceiling((double)listMain.Count / pageSize));
                    //调整停止
                    (sender as DispatcherTimer).Stop();
                    #endregion
                };
                diapatcherTimer.Start();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DataPagerInit", ex.ToString());
            }
            finally
            {
            }
        }

        /// <summary>
        /// 调整分页容器(减少一项)
        /// </summary>
        void DataPagerRemoveInit(TongJiItem item)
        {
            try
            {
                //stackPanel1所能装载的数量
                int pageSize = (int)this.stackPanel1.ActualHeight / intItemHeight;
                listMain.Remove(item);

                if (this.stackPanel1.Children.Count == 0 && intStart >= pageSize)
                {
                    this.stackPanel1.Children.Clear();
                    intStart -= pageSize;
                    intEnd--;
                    intPageNow--;
                }
                else
                {
                    if (intPageShengYu > 0)
                    {
                        this.stackPanel1.Children.Clear();
                        intPageShengYu--;
                    }
                    else
                    {
                        this.stackPanel1.Children.Clear();
                        intEnd--;
                    }
                }
                Stack1ViewChange(intStart, intEnd);

                DataPagerDisP(pageSize);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DataPagerRemoveInit", ex.ToString(), item);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 调整分页容器(字段区域增加一项)
        /// </summary>
        /// <param name="item">拖进字段区域的字段</param>
        void DataPagerAddInit(TongJiItem item)
        {
            try
            {
                //stackPanel1所能装载的数量
                int pageSize = (int)this.stackPanel1.ActualHeight / intItemHeight;

                //添加到统计集
                listMain.Add(item);

                //最后一个添加时发生
                if (this.stackPanel1.Children.Count == pageSize && intPageShengYu == 0)
                {
                    this.stackPanel1.Children.Clear();
                    intEnd++;

                }
                //容器饱和
                else if (this.stackPanel1.Children.Count < pageSize)
                {
                    this.stackPanel1.Children.Clear();
                    intEnd++;
                }
                //最后一个添加，并且有剩余项
                else
                {
                    this.stackPanel1.Children.Clear();
                    intStart += pageSize;
                    intEnd++;
                    intPageNow++;
                }

                Stack1ViewChange(intStart, intEnd);

                DataPagerDisP(pageSize);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DataPagerAddInit", ex.ToString(), item);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 分页信息显示
        /// </summary>
        /// <param name="intpagesize">stackPanel1所能装载的数量</param>
        void DataPagerDisP(int intpagesize)
        {
            try
            {
                //调整每个项的偏移
                listMain.ForEach(itemm => itemm.Margin = new Thickness(0));
                //统计stackPanel1里的数量
                //this.txtCount.Text = this.listMain.Count.ToString();

                //if (listMain.Count == 0)
                //{
                //    this.txtPage.Text = "1/1";
                //}
                //else
                //{
                //    this.txtPage.Text = string.Format("{0}/{1}", intPageNow, (int)Math.Ceiling((double)listMain.Count / intpagesize));
                //}

            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DataPagerDisP", ex.ToString(), intpagesize);
            }
        }

        #endregion

        #region 生成表格

        /// <summary>
        /// 生成表格按钮点击生成表格
        /// </summary>
        void btnInit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //确保行标题和列标题都有
                if (this.stackPanelRow.Children.Count > 0 && this.stackPanelColumn.Children.Count > 0)
                {
                    TomDisPatcherLb tomdisPatcher = new TomDisPatcherLb(new System.Action(() =>
                     {
                         #region 生成进行中

                         if (_dataGrid == null)
                         {

                             //生成自由统计表
                             _dataGrid = new TJGridView();
                             this.bordDataGrid.Child = _dataGrid;
                             _dataGrid.SkingSetting(_brushKeyName, _txtForeground);
                             //生成标题
                             TitleInit(_dataGrid, this.stackPanelRow, this.stackPanelColumn);
                         }
                         else
                         {
                             _dataGrid.ReSet();
                             //生成标题
                             TitleInit(_dataGrid, this.stackPanelRow, this.stackPanelColumn);

                         }

                         _NowDataGridUserEntityInit(this.stackPanelRow, this.stackPanelColumn);

                         this.bordDataGrid.Child = _dataGrid;
                         this.btnChange_Click(null, null);


                         #endregion

                         //激活按钮
                         this.ButtonEnableSettingTrue();

                         this.borTip.Visibility = System.Windows.Visibility.Collapsed;
                     }));


                    TomDisPatcherLb tomdisPatcher2 = new TomDisPatcherLb(new System.Action(() =>
                      {
                          this.borTip.Visibility = System.Windows.Visibility.Visible;
                          tomdisPatcher.Start();
                      }));

                    tomdisPatcher2.Start();
                }
                else
                {
                    _message = new MessageShow();
                    this._message.MessageContent = "横轴或竖轴缺少字段";
                    _message.ShowDialog();
                }

            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btnInit_Click", ex.ToString());
            }
        }

        /// <summary>
        /// 搜寻
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.bordDataGrid.Visibility == System.Windows.Visibility.Visible && stack1 != null && stack2 != null && stack1.Children.Count > 0 && stack2.Children.Count > 0)
                {
                    TomDisPatcherLb tomdisPatcher = new TomDisPatcherLb(new System.Action(() =>
                         {
                             _dataGrid.ReSet();
                             TitleInit(_dataGrid, stack1, stack2);
                             this.borTip.Visibility = System.Windows.Visibility.Collapsed;

                         }));

                    TomDisPatcherLb tomdisPatcher2 = new TomDisPatcherLb(new System.Action(() =>
                    {
                        this.borTip.Visibility = System.Windows.Visibility.Visible;
                        tomdisPatcher.Start();
                    }));

                    tomdisPatcher2.Start();
                }
                else
                {
                    //确保行标题和列标题都有
                    if (this.stackPanelRow.Children.Count > 0 && this.stackPanelColumn.Children.Count > 0)
                    {
                        //RectangleFill();
                        TomDisPatcherLb tomdisPatcher = new TomDisPatcherLb(new System.Action(() =>
                           {
                               #region 生成进行中

                               if (_dataGrid == null)
                               {
                                   //生成自由统计表
                                   _dataGrid = new TJGridView();
                                   _dataGrid.SkingSetting(_brushKeyName, _txtForeground);
                                   this.bordDataGrid.Child = _dataGrid;
                                   //生成标题
                                   TitleInit(_dataGrid, this.stackPanelRow, this.stackPanelColumn);
                               }
                               else
                               {
                                   _dataGrid.ReSet();
                                   //生成标题
                                   TitleInit(_dataGrid, this.stackPanelRow, this.stackPanelColumn);
                               }

                               if (bordDataGrid.Child != null && bordDataGrid.Child is TJGridView)
                               {
                                   this.gridTJ.ColumnDefinitions[0].Width = new GridLength(0);
                                   this.bordDataGrid.Visibility = System.Windows.Visibility.Visible;
                                   this.gridTJ.Visibility = System.Windows.Visibility.Collapsed;
                                   //this.borComearch.Margin = new Thickness(35, 0, 0, 0);
                               }

                               this.borTip.Visibility = System.Windows.Visibility.Collapsed;

                               #endregion

                               //激活按钮
                               this.ButtonEnableSettingTrue();

                               _NowDataGridUserEntityInit(this.stackPanelRow, this.stackPanelColumn);

                               this.bordDataGrid.Child = _dataGrid;

                               this.borTip.Visibility = System.Windows.Visibility.Collapsed;
                           }));

                        TomDisPatcherLb tomdisPatcher2 = new TomDisPatcherLb(new System.Action(() =>
                        {
                            this.borTip.Visibility = System.Windows.Visibility.Visible;
                            tomdisPatcher.Start();
                        }));

                        tomdisPatcher2.Start();

                    }
                    else
                    {
                        _message = new MessageShow();
                        this._message.MessageContent = "横轴或竖轴缺少字段";
                        _message.ShowDialog();

                    }
                }

            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btnSearch_Click", ex.ToString());
            }
        }

        /// <summary>
        /// 显示加载提示(开始生成表格)
        /// </summary>
        void TitleInit(TJGridView dataGrid, StackPanel stack1, StackPanel stack2)
        {
            this.stack1 = stack1;
            this.stack2 = stack2;

            try
            {
                rowItemTagList.Clear();
                columnItemTagList.Clear();

                //单元格数据
                List<int> intlist = new List<int>();

                DataTable dt = null;
                if (dt == null || _isDataChanged)
                //获取指定时间的数据           
                {
                    //高级查询条件
                    var ss = comSearch.ZongShuJu();
                    //已完成
                    ss.Add("IsFinish", "是,#Text#Eq");
                    //获取Dtatable
                    dt = DataOperation.ClientGetDic(Proxy.ListName, ss);
                }

                #region 生成行标题

                //是否结束
                bool isRowFinish = false;
                //是否开始添加小计
                bool rowaddXiaoJi = false;

                int stackPanelRowCount = 0;

                stackPanelRowCount = stack1.Children.Count;

                //生成行标题
                for (int i = 0; i < stackPanelRowCount; i++)
                {
                    //获取item对象
                    var item = stack1.Children[i] as TongJiItem;

                    //执行最后一次标记完成
                    if (i == stackPanelRowCount - 1) isRowFinish = true;
                    //是否开始添加小计
                    if (i == stackPanelRowCount - 1 && stackPanelRowCount > 1) rowaddXiaoJi = true;

                    //月份统计
                    if (item.Tittle.Equals("月份"))
                    {
                        this.MonthDealWidth(item);
                        RowTittleInit(dataGrid, item.ItemChild, item.StrProperty, false, false);

                        //是否开始添加小计
                        if (i == stackPanelRowCount - 1 && stackPanelRowCount > 0) rowaddXiaoJi = true;

                        RowTittleInit(dataGrid, item.ItemChild2, item.StrProperty, rowaddXiaoJi, isRowFinish);

                        continue;
                    }
                    RowTittleInit(dataGrid, item.ItemChild, item.StrProperty, rowaddXiaoJi, isRowFinish);
                }


                #endregion

                #region 生成列标题

                //是否生成列标题最后一层
                bool isColumnFinish = false;
                //是否开始添加小计
                bool addXiaoJi = false;

                int stackPanleCollumnCount = stack2.Children.Count;

                //生成列标题
                for (int i = 0; i < stackPanleCollumnCount; i++)
                {
                    //获取item对象
                    var item = stack2.Children[i] as TongJiItem;

                    //通过列表名称获取其标题内容（加载最后一行标题标注为完成）
                    if (i == stackPanleCollumnCount - 1) isColumnFinish = true;
                    //是否开始添加小计
                    if (stackPanleCollumnCount > 1 && i == stackPanleCollumnCount - 1) addXiaoJi = true;

                    //月份处理
                    if (item.Tittle.Equals("月份"))
                    {
                        this.MonthDealWidth(item);
                        //列标题添加
                        ColumnTittleInit(dataGrid, item.ItemChild, item.StrProperty, false, false);

                        //是否开始添加小计
                        if (i == stackPanleCollumnCount - 1 && stackPanleCollumnCount > 0) addXiaoJi = true;
                        //列标题添加
                        ColumnTittleInit(dataGrid, item.ItemChild2, item.StrProperty, addXiaoJi, isColumnFinish);

                        continue;
                    }
                    //列标题添加
                    this.ColumnTittleInit(dataGrid, item.ItemChild, item.StrProperty, addXiaoJi, isColumnFinish);
                }

                #endregion

                #region 生成数据

                //数据表格
                System.Data.DataTable table = new System.Data.DataTable();
                //指定有多少列
                for (int i = 0; i < dataGrid.dataGrid.Columns.Count - 1; i++)
                {
                    table.Columns.Add(Convert.ToString(i));
                    //table.Columns.Add(this._dataGrid.dataGrid.Columns[i].Header.ToString());
                }

                table.Columns.Add("总计");

                if (dt.Columns.Count < 1 || dt.Rows.Count < 1)
                {
                    //给每一行添加数据
                    for (int i = 0; i < dataGrid.tongjiR._LeftCount + 1; i++)
                    {
                        table.Rows.Add();
                    }
                }
                else
                {
                    //给每一行添加数据
                    for (int i = 0; i < dataGrid.tongjiR._LeftCount; i++)
                    {
                        var ps = GetData(dataGrid, dt, dataGrid.tongjiR.GetRangeTittle(i), i);
                        table.Rows.Add(ps);
                    }
                    var ds = GetData(dataGrid, dt);

                    table.Rows.Add(ds);
                }

                //绑定数据源
                dataGrid.dataGrid.ItemsSource = table.DefaultView;


                #endregion

                #region 清理垃圾,释放内存

                //DispatcherTimer timer2 = new DispatcherTimer();
                //timer2.Interval = new TimeSpan(1000);
                //timer2.Tick += (object sender1, EventArgs e1) =>
                //    {


                //        timer2.Stop();
                //    };
                //timer2.Start(); 

                Reflesh();

                #endregion
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TitleInit", ex.ToString(), dataGrid);
            }
        }

        /// <summary>
        /// 行标题添加
        /// </summary>
        /// <param name="itemChild">子项</param>
        /// <param name="itemProperty">内部名称</param>
        /// <param name="addXiaoJi">是否开始添加小计</param>
        /// <param name="isRowFinish">是否完成</param>
        void RowTittleInit(TongJiDataGrid.TJGridView datagrrd, TongJiItemChild itemChild, string itemProperty, bool addXiaoJi, bool isRowFinish)
        {
            try
            {
                //若有小计，去除
                if (itemChild.PropertyList.Contains("小计")) itemChild.PropertyList.Remove("小计");

                //子级关系
                if (itemChild.DicPropertyCaml != null && rowItemTagList.Contains(itemChild.ParentPropertyName))
                {
                    itemChild.IntIndenttitys.Clear();

                    List<string> titleSumList = new List<string>();

                    //遍历进行列标题的添加
                    for (int p = 0; p < itemChild.DicPropertyCaml.Count; p++)
                    {
                        if (itemChild.DicPropertyCaml.Values.ElementAt(p).Contains("小计")) itemChild.DicPropertyCaml.Values.ElementAt(p).Remove("小计");

                        //加一个映射点
                        itemChild.IntIndenttitys.Add(itemChild.DicPropertyCaml.Values.ElementAt(p).Count);

                        //通过列表名称获取其标题内容（加载最后一行标题标注为完成）
                        if (addXiaoJi && tongjiHelper._isNeedXiaoJi)
                        {
                            itemChild.DicPropertyCaml.Values.ElementAt(p).Add("小计");
                            itemChild.IntIndenttitys[p]++;
                        }
                        titleSumList.AddRange(itemChild.DicPropertyCaml.Values.ElementAt(p));
                    }
                    //通过列表名称获取其标题内容
                    datagrrd._RowHeaderAdd(titleSumList, itemChild.IntIndenttitys, isRowFinish);
                }
                else
                {
                    //最后一列有小计
                    if (addXiaoJi && tongjiHelper._isNeedXiaoJi)
                        itemChild.PropertyList.Add("小计");
                    //添加行标题
                    datagrrd._RowHeaderAdd(itemChild.PropertyList, isRowFinish);
                }

                //如SheBeiTypeErJi
                rowItemTagList.Add(itemProperty);

            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "RowTittleInit", ex.ToString(), itemChild, itemProperty, addXiaoJi, isRowFinish);
            }
        }

        /// <summary>
        /// 列标题添加
        /// </summary>
        /// <param name="itemChild">子项</param>
        /// <param name="itemProperty">内部名称</param>
        /// <param name="addXiaoJi">是否开始添加小计</param>
        /// <param name="isRowFinish">是否完成</param>
        void ColumnTittleInit(TJGridView datagrrd, TongJiItemChild itemChild, string itemProperty, bool addXiaoJi, bool isColumnFinish)
        {
            try
            {
                //去除小计
                if (itemChild.PropertyList.Contains("小计"))
                    itemChild.PropertyList.Remove("小计");

                if (itemChild.DicPropertyCaml != null && columnItemTagList.Contains(itemChild.ParentPropertyName))
                {
                    //标题内容
                    List<string> titleSum = new List<string>();

                    //映射数
                    itemChild.IntIndenttitys.Clear();

                    //遍历进行行标题的添加
                    for (int p = 0; p < itemChild.DicPropertyCaml.Count; p++)
                    {

                        if (itemChild.DicPropertyCaml.Values.ElementAt(p).Contains("小计"))
                            itemChild.DicPropertyCaml.Values.ElementAt(p).Remove("小计");
                        //加一个映射点
                        itemChild.IntIndenttitys.Add(itemChild.DicPropertyCaml.Values.ElementAt(p).Count);

                        if (addXiaoJi && tongjiHelper._isNeedXiaoJi)
                        {
                            itemChild.DicPropertyCaml.Values.ElementAt(p).Add("小计");
                            itemChild.IntIndenttitys[p]++;
                        }
                        //添加标题
                        titleSum.AddRange(itemChild.DicPropertyCaml.Values.ElementAt(p));
                    }
                    //通过列表名称获取其标题内容
                    datagrrd._ColumnHeaderAdd(titleSum, itemChild.IntIndenttitys, isColumnFinish);
                }
                else
                {
                    if (addXiaoJi && tongjiHelper._isNeedXiaoJi)
                        itemChild.PropertyList.Add("小计");
                    datagrrd._ColumnHeaderAdd(itemChild.PropertyList, isColumnFinish);
                }
                columnItemTagList.Add(itemProperty);

            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TongJi", ex.ToString(), itemChild, itemProperty, addXiaoJi, isColumnFinish);
            }
        }

        /// <summary>
        /// 处理月份字段
        /// </summary>
        /// <param name="item"></param>
        void MonthDealWidth(TongJiItem item)
        {
            try
            {
                //获取时间控件
                var startTime = this.comSearch.startEndTime1;
                //获取开始日期
                var starData = startTime.DPStartData;
                //获取结束日期
                var endData = startTime.DPEndData;
                //得到相差年份
                var count = (endData.DisplayDate.Year - starData.DisplayDate.Year) * 12;

                DateTime date = starData.DisplayDate;
                //得到相差月份
                count = (endData.DisplayDate.Month + 1 - starData.DisplayDate.Month) + count;

                item.ItemChild2 = new TongJiItemChild();

                item.ItemChild = new TongJiItemChild();

                item.ItemChild2.ParentPropertyName = "startData,EndData";

                List<string> sumTitleList = new List<string>();
                for (int cou = 0; cou < count; cou++)
                {
                    var realYear = date.AddMonths(cou).ToString("yyyy年");
                    var realMonth = date.AddMonths(cou).ToString("MM月");

                    sumTitleList.Add(realYear);
                    item.ItemChild2.PropertyList.Add(realMonth);
                }

                item.ItemChild.PropertyList.AddRange(_DeleteSameProperty(sumTitleList));

                foreach (var chid in item.ItemChild.PropertyList)
                {
                    item.ItemChild2.DicPropertyCaml.Add(chid, new List<string>());
                    for (int cou = 0; cou < count; cou++)
                    {
                        var realYear = date.AddMonths(cou).ToString("yyyy年");
                        var realMonth = date.AddMonths(cou).ToString("MM月");

                        if (chid.Equals(realYear))
                            item.ItemChild2.DicPropertyCaml[chid].Add(realMonth);
                    }
                }

            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "MonthDealWidth", ex.ToString(), item);
            }
        }

        /// <summary>
        /// 删除相同的子项
        /// </summary>
        /// <param name="strList"></param>
        /// <returns></returns>
        List<string> _DeleteSameProperty(List<string> strList)
        {
            List<string> Update_list = new List<string>();
            try
            {
                var ulist = (from li in strList
                             select li).Distinct();
                foreach (var d in ulist)
                {
                    Update_list.Add(d);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "_DeleteSameProperty", ex.ToString(), strList);
            }
            return Update_list;
        }

        /// <summary>
        /// 通过指定条件获取单元格数据
        /// </summary>
        /// <param name="striTittleList"></param>
        /// <returns></returns>
        public object[] GetData(TongJiDataGrid.TJGridView dataGridd, System.Data.DataTable dataTable, string[] listRealRow, int rowPosition)
        {
            object[] data = new object[dataGridd.dataGrid.Columns.Count];
            try
            {
                for (int columnPosition = 0; columnPosition < data.Count(); columnPosition++)
                {
                    object count = 0;

                    string expression = string.Empty;

                    #region 获取表达式

                    if (columnPosition < data.Count() - 1)
                    {
                        for (int i = 0; i < listRealRow.Count(); i++)
                        {
                            var item = listRealRow[i];
                            if (item.Contains("月"))
                            {
                                expression += GetMonthExpression(listRealRow[i - 1] + item, i, this.rowItemTagList);
                            }
                            else if (!item.Equals("小计") && !item.Contains("月") && !item.Contains("年"))
                            {
                                expression += this.rowItemTagList[i] + "=" + "'" + item + "'" + " And ";
                            }
                            else if (listRealRow.Contains("小计") && item.Contains("年"))
                            {
                                expression += GetYearExpression(item, i, this.rowItemTagList);
                            }
                        }

                        for (int i = 0; i < dataGridd.tongjiC.GetRangeTittle(columnPosition).Count(); i++)
                        {
                            var range = dataGridd.tongjiC.GetRangeTittle(columnPosition);
                            var item = range[i];

                            if (item.Contains("月"))
                            {
                                expression += GetMonthExpression(dataGridd.tongjiC.GetRangeTittle(columnPosition)[i - 1] + item, i, this.columnItemTagList);
                            }
                            else if (!item.Equals("小计") && !item.Contains("月") && !item.Contains("年"))
                            {
                                expression += this.columnItemTagList[i] + "=" + "'" + item + "'" + " And ";
                            }
                            else if (range.Contains("小计") && item.Contains("年"))
                            {
                                expression += GetYearExpression(item, i, this.columnItemTagList);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < listRealRow.Count(); i++)
                        {
                            var item = listRealRow[i];
                            if (item.Contains("月"))
                            {
                                expression += GetMonthExpression(listRealRow[i - 1] + item, i, this.rowItemTagList);
                            }
                            else if (!item.Equals("小计") && !item.Contains("月") && !item.Contains("年"))
                            {
                                expression += this.rowItemTagList[i] + "=" + "'" + item + "'" + " And ";
                            }
                            else if (listRealRow.Contains("小计") && item.Contains("年"))
                            {
                                expression += GetYearExpression(item, i, this.rowItemTagList);
                            }
                        }
                    }

                    #endregion

                    string realExpression = string.Empty;
                    if (expression.Contains(" And "))
                        realExpression = expression.Substring(0, expression.LastIndexOf(" And "));

                    var result = dataTable.Compute(tongjiHelper._ComputeType + tongjiHelper._ComputeContent, realExpression);

                    if (result == DBNull.Value)
                    {
                        count = string.Empty;
                    }
                    else
                    {
                        //统计结果
                        var cc = Convert.ToInt32(result);
                        if (cc == 0) count = string.Empty;
                        else count = cc;
                    }

                    data[columnPosition] = count;
                }

            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "GetData", ex.ToString(), dataTable, listRealRow, rowPosition);
            }
            return data;
        }

        /// <summary>
        /// 通过指定条件获取单元格数据
        /// </summary>
        /// <param name="striTittleList"></param>
        /// <returns></returns>
        public object[] GetData(TJGridView dataGridd, System.Data.DataTable dataTable)
        {
            object[] data = new object[dataGridd.dataGrid.Columns.Count];
            try
            {
                for (int columnPosition = 0; columnPosition < data.Count(); columnPosition++)
                {
                    object count = 0;

                    string expression = string.Empty;

                    if (columnPosition < data.Count() - 1)
                    {
                        var titlist = dataGridd.tongjiC.GetRangeTittle(columnPosition);
                        for (int i = 0; i < titlist.Count(); i++)
                        {

                            var item = titlist[i];

                            if (item.Contains("月"))
                            {
                                expression += GetMonthExpression(titlist[i - 1] + item, i, this.columnItemTagList);
                            }
                            else if (!item.Equals("小计") && !item.Contains("月") && !item.Contains("年"))
                            {
                                expression += this.columnItemTagList[i] + "=" + "'" + item + "'" + " And ";
                            }                       
                        }
                    }

                    var realExpression = string.Empty;
                    if (expression.Contains(" And "))
                        realExpression = expression.Substring(0, expression.LastIndexOf(" And "));

                    var result = dataTable.Compute(tongjiHelper._ComputeType + tongjiHelper._ComputeContent, realExpression);

                    if (result == DBNull.Value)
                    {
                        count = "";
                    }
                    else
                    {
                        //统计结果
                        var cc = Convert.ToInt32(result);
                        if (cc == 0) count = "";
                        else count = cc;
                    }

                    data[columnPosition] = count;
                }

            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TongJi", ex.ToString(), dataTable);
            }
            return data;
        }

        /// <summary>
        /// 获取年的表达式
        /// </summary>
        /// <param name="dicItem"></param>
        /// <param name="tagPosition"></param>
        /// <param name="listTag"></param>
        /// <returns></returns>
        string GetYearExpression(string dicItem, int tagPosition, List<string> listTag)
        {
            string yearExpression = string.Empty;
            try
            {
                string[] ss = listTag[tagPosition].Split(new char[] { ',' });
                var start = Convert.ToDateTime(dicItem);
                var end = start.AddYears(1);
                yearExpression = ss[0] + ">=" + "'" + start + "'" + " And " + ss[0] + "<" + "'" + end + "'" + " And ";

            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "GetYearExpression", ex.ToString(), dicItem, tagPosition, listTag);
            }
            return yearExpression;
        }

        /// <summary>
        /// 获取月份表达式
        /// </summary>
        /// <returns></returns>
        string GetMonthExpression(string dicItem, int tagPosition, List<string> listTag)
        {
            string monthExpression = string.Empty;
            try
            {
                //{[startData, 2013/6/20 5:43:00]}  "yyyy/MM/dd HH:mm:ss"
                string[] ss = listTag[tagPosition].Split(new char[] { ',' });
                var start = Convert.ToDateTime(dicItem);
                var end = start.AddMonths(1);
                monthExpression = ss[0] + ">=" + "'" + start + "'" + " And " + ss[0] + "<" + "'" + end + "'" + " And ";
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "GetMonthExpression", ex.ToString(), dicItem, tagPosition, listTag);
            }
            return monthExpression;
        }
      
        /// <summary>
        /// 添加行
        /// </summary>
        /// <param name="intList">数据集合</param>
        /// <returns>返回总计</returns>
        public object[] RowsItemAdd(List<int> intList, List<int> intXiaoJi)
        {
            //转为引用类型的数组
            object[] objs = new object[intList.Count + 1];
            try
            {
                //声明总计
                int Sum = 0;
                //生成总计
                intXiaoJi.ForEach(Item => Sum += Item);


                //加载数据
                for (int i = 0; i < intList.Count(); i++)
                {
                    if (intList[i] != 0)
                        objs[i] = intList[i];
                }

                //加载总计
                objs[intList.Count] = Sum;

            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "RowsItemAdd", ex.ToString(), intList, intXiaoJi);
            }
            return objs;
        }

        #endregion

        #region 导入excel
     
        /// <summary>
        /// 导入到Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.bordDataGrid.Child != null)
                {
                    //RectangleFill();

                    this.borTip.Visibility = System.Windows.Visibility.Visible;

                    ThreadPool.QueueUserWorkItem((o) =>
       {
           int startRealRowPosition = 0;
           int startRealColumnPosition = 0;

           Dictionary<int, List<string>> DicTittleTextList_R = this._dataGrid.tongjiR._DicTittleTextList;
           Dictionary<int, List<string>> DicTittleTextList_C = this._dataGrid.tongjiC._DicTittleTextList;

           var Cstart = DicTittleTextList_C.Count + 1;
           var Rstart = DicTittleTextList_R.Count + 1;

           #region 列出查询条件

           #endregion

           #region 开启Excel并创建工作簿

           if (_excelother == null)
               //创建excel应用程序
               _excelother = new Microsoft.Office.Interop.Excel.Application();

           //创建一个excel工作簿
           Microsoft.Office.Interop.Excel._Worksheet ws = new Microsoft.Office.Interop.Excel.WorksheetClass();
         
           //启用
           _excelother.Application.Workbooks.Add(true);

           _excelother.WorkbookBeforeClose += new Microsoft.Office.Interop.Excel.AppEvents_WorkbookBeforeCloseEventHandler(excelother_WorkbookBeforeClose);
         
           #endregion

           #region 表单标题

           ws = (Microsoft.Office.Interop.Excel.Worksheet)_excelother.ActiveSheet;
           Microsoft.Office.Interop.Excel.Range rangTittle = null;
           rangTittle = ws.get_Range(ws.Cells[1 + startRealRowPosition, 1 + startRealColumnPosition], ws.Cells[1 + startRealRowPosition, Rstart + DicTittleTextList_C[DicTittleTextList_C.Count - 1].Count + startRealColumnPosition]);
           rangTittle.MergeCells = true;
           _excelother.Cells[1 + startRealRowPosition, 1 + startRealColumnPosition] = "统计分析";
           rangTittle.RowHeight = 25;
           rangTittle.Font.Size = 17;
           rangTittle.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
           rangTittle.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

           #endregion

           #region 生成映射树

           //列标题文本对比
           var linColumnTxt = string.Empty;

           //行标题文本对比
           var linRowTxt = string.Empty;

           //存储列标题映射关系
           Dictionary<int, List<int>> dicColumnList = new Dictionary<int, List<int>>();

           //存储行标题映射关系
           Dictionary<int, List<int>> dicRowList = new Dictionary<int, List<int>>();

           #region 生成列映射

           //列标题映射生成
           for (int i = 0; i < DicTittleTextList_C.Count; i++)
           {
               dicColumnList.Add(i, new List<int>());

               for (int j = 0; j < DicTittleTextList_C[i].Count; j++)
               {
                   //获取标题内容
                   var text = DicTittleTextList_C[i][j];

                   if (i < DicTittleTextList_C.Count - 1)
                   {
                       //验证条件
                       if (!linColumnTxt.Equals(text))
                       {
                           //添加临时映射点
                           dicColumnList[i].Add(j + Rstart + startRealColumnPosition);
                           //设置临时文本
                           linColumnTxt = text;
                       }
                   }
               }
           }
           #endregion

           #region 生成行映射

           //列标题映射生成
           for (int i = 0; i < DicTittleTextList_R.Count; i++)
           {
               dicRowList.Add(i, new List<int>());

               for (int j = 0; j < DicTittleTextList_R[i].Count; j++)
               {
                   //获取标题内容
                   var text = DicTittleTextList_R[i][j];

                   if (i < DicTittleTextList_R.Count - 1)
                   {
                       //验证条件
                       if (!linRowTxt.Equals(text))
                       {
                           //添加临时映射点
                           dicRowList[i].Add(j + 1 + Cstart + startRealRowPosition);
                           //设置临时文本
                           linRowTxt = text;
                       }
                   }
               }
           }

           #endregion

           #endregion

           #region 合并单元格

           #region 合并列标题

           foreach (var item in dicColumnList)
           {
               if (item.Value.Count == 0) continue;
               else
               {
                   for (int i = 0; i < item.Value.Count; i++)
                   {
                       Microsoft.Office.Interop.Excel.Range r = null;
                       if (i < item.Value.Count - 1)
                       {
                           r = ws.get_Range(ws.Cells[item.Key + 2 + startRealRowPosition, item.Value[i]], ws.Cells[item.Key + 2 + startRealRowPosition, item.Value[i + 1] - 1]);     //取得合并的区域  
                           r.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                           r.MergeCells = true;
                       }
                       else
                       {
                           r = ws.get_Range(ws.Cells[item.Key + 2 + startRealRowPosition, item.Value[i]], ws.Cells[item.Key + 2 + startRealRowPosition, this._dataGrid.dataGrid.Columns.Count + DicTittleTextList_R.Count - 1 + startRealColumnPosition]);     //取得合并的区域  
                           r.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                           r.MergeCells = true;
                       }
                   }
               }
           }

           #endregion

           #region 合并行标题

           foreach (var item in dicRowList)
           {
               if (item.Value.Count == 0) continue;
               else
               {
                   for (int i = 0; i < item.Value.Count; i++)
                   {
                       Microsoft.Office.Interop.Excel.Range r = null;
                       if (i < item.Value.Count - 1)
                       {
                           r = ws.get_Range(ws.Cells[item.Value[i], item.Key + 1 + startRealColumnPosition], ws.Cells[item.Value[i + 1] - 1, item.Key + 1 + startRealColumnPosition]);     //取得合并的区域  
                           r.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                           r.MergeCells = true;
                       }
                       else
                       {
                           r = ws.get_Range(ws.Cells[item.Value[i], item.Key + 1 + startRealColumnPosition], ws.Cells[this._dataGrid.tongjiR._LeftCount + Cstart + startRealRowPosition, item.Key + 1 + startRealColumnPosition]);     //取得合并的区域  
                           r.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                           r.MergeCells = true;
                       }
                   }
               }
           }

           #endregion

           #endregion

           #region 生成标题

           #region 列标题生成

           //列标题映射生成
           for (int i = 0; i < DicTittleTextList_C.Count; i++)
           {
               for (int j = 0; j < DicTittleTextList_C[i].Count; j++)
               {
                   //获取标题内容
                   var text = DicTittleTextList_C[i][j];
                   //
                   _excelother.Cells[i + 2 + startRealRowPosition, j + Rstart + startRealColumnPosition] = text;

                   //设置标题单元格属性                
                   Microsoft.Office.Interop.Excel.Range r = ws.get_Range(ws.Cells[i + 2 + startRealRowPosition, j + Rstart + startRealColumnPosition], ws.Cells[i + 2 + startRealRowPosition, j + Rstart + startRealColumnPosition]);
                   r.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                   r.Font.Size = 13;
                   r.ColumnWidth = 15;
                   r.RowHeight = 25;
                   r.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
               }
               if (i == DicTittleTextList_C.Count - 1)
               {
                   _excelother.Cells[i + 2 + startRealRowPosition, DicTittleTextList_C[i].Count + Rstart + startRealColumnPosition] = "总计";

                   //设置标题单元格属性                
                   Microsoft.Office.Interop.Excel.Range r = ws.get_Range(ws.Cells[i + 2 + startRealRowPosition, DicTittleTextList_C[i].Count + Rstart + startRealColumnPosition], ws.Cells[i + 2 + startRealRowPosition, DicTittleTextList_C[i].Count + Rstart + startRealColumnPosition]);
                   r.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                   r.Font.Size = 13;
                   r.ColumnWidth = 15;
                   r.RowHeight = 25;
                   r.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
               }
           }

           #endregion

           #region 生成行标题

           //行标题映射生成
           for (int i = 0; i < DicTittleTextList_R.Count; i++)
           {

               for (int j = 0; j < DicTittleTextList_R[i].Count; j++)
               {
                   //获取标题内容
                   var text = DicTittleTextList_R[i][j];
                   //
                   _excelother.Cells[j + Cstart + 1 + startRealRowPosition, i + 1 + startRealColumnPosition] = text;

                   //设置标题单元格属性                
                   Microsoft.Office.Interop.Excel.Range r = ws.get_Range(ws.Cells[j + Cstart + 1 + startRealRowPosition, i + 1 + startRealColumnPosition], ws.Cells[j + Cstart + 1 + startRealRowPosition, i + 1 + startRealColumnPosition]);
                   r.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                   r.Font.Size = 13;
                   r.ColumnWidth = 15;
                   r.RowHeight = 25;
                   r.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
               }
               if (i == DicTittleTextList_R.Count - 1)
               {
                   _excelother.Cells[DicTittleTextList_R[i].Count + DicTittleTextList_C.Count + 2 + startRealRowPosition, i + 1 + startRealColumnPosition] = "总计";

                   //设置标题单元格属性                
                   Microsoft.Office.Interop.Excel.Range r = ws.get_Range(ws.Cells[DicTittleTextList_R[i].Count + DicTittleTextList_C.Count + 2 + startRealRowPosition, i + 1 + startRealColumnPosition], ws.Cells[DicTittleTextList_R[i].Count + DicTittleTextList_C.Count + 2 + startRealRowPosition, i + 1 + startRealColumnPosition]);
                   r.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                   r.Font.Size = 13;
                   r.ColumnWidth = 15;
                   r.RowHeight = 25;
                   r.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
               }
           }

           #endregion

           #endregion

           #region 生成数据

           for (int i = 0; i < this._dataGrid.dataGrid.Items.Count; i++)
           {
               DataRowView row = this._dataGrid.dataGrid.Items[i] as DataRowView;
               var array = row.Row.ItemArray;
               for (int j = 0; j < array.Count(); j++)
               {
                   //if (Convert.ToString(row[j]) == string.Empty) continue;
                   _excelother.Cells[startRealRowPosition + i + Cstart + 1, Rstart + j + startRealColumnPosition] = row[j];

                   //设置标题单元格属性                
                   Microsoft.Office.Interop.Excel.Range r = ws.get_Range(ws.Cells[i + startRealRowPosition + Cstart + 1, j + startRealColumnPosition + Rstart], ws.Cells[i + startRealRowPosition + Cstart + 1, j + startRealColumnPosition + Rstart]);
                   r.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                   r.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
               }
           }

           #endregion

           //完成
           _excelother.Visible = true;
           this.Dispatcher.BeginInvoke(new Action(() =>
               {
                   this.borTip.Visibility = System.Windows.Visibility.Collapsed;

                   this._message = new MessageShow();
                   this._message.MessageContent = "导入成功";
                   this._message.ShowDialog();
               }));
       });
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btExcel_Click", ex.ToString());
            }
        }

        #region 结束Excel进程

        /// <summary>
        /// excel关闭窗体时结束进程
        /// </summary>
        /// <param name="Wb"></param>
        /// <param name="Cancel"></param>
        void excelother_WorkbookBeforeClose(Microsoft.Office.Interop.Excel.Workbook Wb, ref bool Cancel)
        {

            //_NowDataGridUserEntity.
            try
            {
                Wb.Application.WorkbookBeforeClose -= excelother_WorkbookBeforeClose;

               var workBooksCount = Wb.Application.Workbooks.Count;
               if (workBooksCount == 1)
               {                   
                 CommonMethod.  Kill(Wb.Application);
                   this._excelother = null;
               }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "excelother_WorkbookBeforeClose", ex.ToString(), Wb, Cancel);
            }
        }

        /// <summary>
        /// 结束Excel进程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Current_Exit(object sender, ExitEventArgs e)
        {
            if (this._excelother != null)
                CommonMethod.Kill(this._excelother);
        }

        #endregion

        #endregion

        #region 打印

        /// <summary>
        /// 打印表格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btf3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.bordDataGrid.Child != null)
                {
                    //RectangleFill();

                    this.borTip.Visibility = System.Windows.Visibility.Visible;

                    PrintDocument print = new PrintDocument();

                    if (comSearch._isHengWatch)
                        print.HengYuLan();
                    else
                        print.ShuYuLan();


                    this._dataGrid.ChangeForPrint(100, print._size.Width);

                    this._dataGrid.BorderThickness = new Thickness(1);
                    this._dataGrid.BorderBrush = new SolidColorBrush(Colors.Black);

                    print.Items_Add("统计列表", this._dataGrid);

                    print.Closed += (object sender1, EventArgs e1) =>
                    {
                        this._dataGrid.ChangeBeforeState(25);
                        this._dataGrid.BorderThickness = new Thickness(0);
                    };
                    print.Show();
                    this.borTip.Visibility = System.Windows.Visibility.Collapsed;
                }

            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btf3_Click", ex.ToString());
            }
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 生成当前标题实例表单
        /// </summary>
        void _NowDataGridUserEntityInit(StackPanel stack1, StackPanel stack2)
        {
            //横标题实例集初始化
            _hengLL.Clear();
            //竖标题实例集初始化
            _ShuLL.Clear();

            //横标题存储
            string hengTittle = string.Empty;
            //列标题存储
            string shuTittle = string.Empty;
            //遍历行标题区域
            foreach (var item in stack1.Children)
            {
                //对比初始化生成的标题字段
                foreach (var heng in TongJiItem.TongJiItemList)
                {
                    //判断是否相符
                    if (heng.Tittle.Equals((item as TongJiItem).Tittle))
                    {
                        //创建字段实例
                        DataGridUserEntityItem ittheng = new DataGridUserEntityItem(heng.Tittle, heng.StrProperty, heng.ItemChild);
                        //赋予二级字段
                        if (heng.ItemChild2 != null) ittheng.ItemChild2 = heng.ItemChild2;
                        //行标题设置
                        hengTittle += heng.Tittle + "/";
                        //标题字段实例添加
                        _hengLL.Add(ittheng);
                    }
                }
            }
            //遍历列标题区域
            foreach (var item in stack2.Children)
            {
                //对比初始化生成的标题字段
                foreach (var shu in TongJiItem.TongJiItemList)
                {
                    //判断是否相符
                    if (shu.Tittle.Equals((item as TongJiItem).Tittle))
                    {
                        //创建字段实例
                        DataGridUserEntityItem ittShu = new DataGridUserEntityItem(shu.Tittle, shu.StrProperty, shu.ItemChild);
                        //赋予二级字段
                        if (shu.ItemChild2 != null) ittShu.ItemChild2 = shu.ItemChild2;
                        //列标题设置
                        shuTittle += shu.Tittle + "/";
                        //标题字段实例添加
                        _ShuLL.Add(ittShu);
                    }
                }
            }

            string hengRealTittle = hengTittle.Substring(0, hengTittle.LastIndexOf("/"));
            string shuRealTittle = shuTittle.Substring(0, shuTittle.LastIndexOf("/"));

            //生成存储表单实例
            _NowDataGridUserEntity = new DataGridUserEntity(hengRealTittle, shuRealTittle, _hengLL, _ShuLL);
        }

        /// <summary>
        /// 进行排序（子级在前，父级在后）
        /// </summary>
        /// <param name="item"></param>
        /// <param name="ParentChirden"></param>
        /// <param name="firstProperty"></param>
        /// <param name="secondProperty"></param>
        /// <returns></returns>
        public bool _NewSort(TongJiItem item, UIElementCollection ParentChirden, string firstProperty, string secondProperty)
        {
            bool compleate = false;
            try
            {
                if (item.Tittle.Equals(firstProperty) || item.Tittle.Equals(secondProperty))
                {
                    //获取容器数量
                    var count = ParentChirden.Count;

                    //临时存储标题集
                    List<TongJiItem> itemList = new List<TongJiItem>();
                    //是否进入重新排序的操作
                    bool enter = false;

                    //获取是否需要重新排序
                    foreach (var tt in ParentChirden)
                    {
                        if ((tt as TongJiItem).Tittle.Equals(firstProperty) || (tt as TongJiItem).Tittle.Equals(secondProperty))
                        {
                            enter = true;
                            break;
                        }
                    }
                    //重新排序
                    if (enter)
                    {
                        TongJiItem vv = null;

                        var postion = 0;

                        for (int i = count - 1; i > -1; i--)
                        {
                            if ((ParentChirden[i] as TongJiItem).Tittle.Equals(firstProperty))
                            {
                                vv = ParentChirden[i] as TongJiItem;
                                ParentChirden.RemoveAt(i);
                                postion = i;
                            }
                            else if ((ParentChirden[i] as TongJiItem).Tittle.Equals(secondProperty))
                            {

                                vv = ParentChirden[i] as TongJiItem;
                                ParentChirden.RemoveAt(i);
                                postion = i;
                            }
                            else if (postion < i)
                            {
                                itemList.Add(ParentChirden[i] as TongJiItem);
                                ParentChirden.RemoveAt(i);
                            }
                        }

                        //移除原来的连接
                        (item.Parent as StackPanel).Children.Remove(item);

                        if (vv.Tittle.Equals(firstProperty))
                        {
                            ParentChirden.Add(vv);
                            ParentChirden.Add(item);
                        }
                        else
                        {
                            ParentChirden.Add(item);
                            ParentChirden.Add(vv);
                        }

                        for (int i = itemList.Count - 1; i > -1; i--)
                        {
                            ParentChirden.Add(itemList[i]);
                        }
                        compleate = true;
                    }
                }

            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "_NewSort", ex.ToString(), item, ParentChirden, firstProperty, secondProperty);
            }
            return compleate;
        }

        /// <summary>
        /// 提示尺寸变化
        /// </summary>
        void RectangleFill()
        {
            try
            {
                if (bordDataGrid.Visibility == System.Windows.Visibility.Visible)
                    this.borTip.Margin = new Thickness(35, 0, 0, 0);
                else
                    this.borTip.Margin = new Thickness(0);

            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "RectangleFill", ex.ToString());
            }
        }

        /// <summary>
        /// 设置打印和Excel按钮禁用
        /// </summary>
        void ButtonEnableSettingFalse()
        {
            if (_ChangeMode == Selection.Enable)
            {
                this.comSearch.btf3.IsEnabled = false;
                this.comSearch.btExcel.IsEnabled = false;
                this.comSearch.borYuLan.IsEnabled = false;
                this.comSearch.btnListSave.IsEnabled = false;
            }
            else if (_ChangeMode == Selection.Hiden)
            {
                this.comSearch.btf3.Visibility = System.Windows.Visibility.Collapsed;
                this.comSearch.btExcel.Visibility = System.Windows.Visibility.Collapsed;
                this.comSearch.borYuLan.Visibility = System.Windows.Visibility.Collapsed;
                this.comSearch.btnListSave.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        /// <summary>
        /// 设置打印和Excel按钮激活
        /// </summary>
        void ButtonEnableSettingTrue()
        {
            if (_ChangeMode == Selection.Enable)
            {
                this.comSearch.btf3.IsEnabled = true;
                this.comSearch.btExcel.IsEnabled = true;
                this.comSearch.borYuLan.IsEnabled = true;
                this.comSearch.btnListSave.IsEnabled = true;
            }
            else if (_ChangeMode == Selection.Hiden)
            {
                this.comSearch.btf3.Visibility = System.Windows.Visibility.Visible;
                this.comSearch.btExcel.Visibility = System.Windows.Visibility.Visible;
                this.comSearch.borYuLan.Visibility = System.Windows.Visibility.Visible;
                this.comSearch.btnListSave.Visibility = System.Windows.Visibility.Visible;
            }
        }

        /// <summary>
        /// 局部刷新
        /// </summary>
        void Reflesh()
        {
            try
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Reflesh", ex.ToString());
            }
        }
      
        /// <summary>
        /// 更改皮肤
        /// </summary>
        /// <param name="brushKeyName"></param>
        public void SkingSetting(string brushKeyName,Brush txtForeground)
        {
            try
            {
                _brushKeyName = brushKeyName;
                _txtForeground = txtForeground;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "SkingSetting", ex.ToString(), brushKeyName);
            }
        }
      
        #endregion

        #region 辅助类（枚举）

        enum Selection
        {
            Hiden,
            Enable,
        }

        #endregion

        #region 遮挡事件

        private void Border_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        #endregion
    }
}
