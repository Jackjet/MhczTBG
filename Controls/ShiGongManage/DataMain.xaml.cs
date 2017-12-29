using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Data;
using MhczTBG.Common;
using System.Collections;

namespace MhczTBG.Controls.ShiGongManage
{
    public partial class DataMain : UserControl
    {
        #region 变量

        /// <summary>
        /// 标题集合
        /// </summary>
        List<TitleControl> rolist = new List<TitleControl>();

        /// <summary>
        /// 菜单(菜单宽度)
        /// </summary>
        ContextMenu menu1 = new ContextMenu() { Width = 150 };

        /// <summary>
        /// 菜单项
        /// </summary>
        List<MenuItem> items1 = new List<MenuItem>();


        /// <summary>
        /// 数据控件（甘特图）
        /// </summary>
        List<DataControl> dataList = new List<DataControl>();

        /// <summary>
        /// 选择单元格背景
        /// </summary>
        public Brush SelectBorderBursh = null;


        /// <summary>
        /// 代理表格、日期生成
        /// </summary>
        UserControlOperate enitt = null;


        Border tittle = null;
        /// <summary>
        /// 标题
        /// </summary>
        public Border Tittle
        {
            get { return tittle; }
            set
            {
                if (value != null)
                {
                    this.borTittle.Child = value;
                    tittle = value;
                }
            }
        }

        Border tittle2 = null;
        /// <summary>
        /// 时间轴里的标题
        /// </summary>
        public Border Tittle2
        {
            get { return tittle2; }
            set
            {
                if (value != null)
                {
                    this.borTittle2.Child = value;
                    tittle = value;
                }
            }
        }

        int intMouthLong = 1;
        /// <summary>
        /// 时间轴的展示长度（以月为基数,默认值为1个月）
        /// </summary>
        public int IntMouthLong
        {
            get { return intMouthLong; }
            set
            {
                if (value <= 0) value = 1;
                intMouthLong = value;
            }
        }

        /// <summary>
        /// 默认颜色
        /// </summary>
        Color DefaultColor = default(Color);

        #endregion

        #region 自定义委托事件

        public delegate void GetTargetInformationEventHandle(IShiGongManager shiGongManager);
        /// <summary>
        /// 点击查询菜单获取到信息之后需要执行的操作
        /// </summary>
        public event GetTargetInformationEventHandle GetTargetInformationEvent = null;

        public delegate void SetTargetInformationEventHandle(ref IShiGongManager shiGongManager);
        /// <summary>
        /// 当指派某项任务时所要执行的额外方法
        /// </summary>
        public event SetTargetInformationEventHandle SetTargetInformationEvent = null;

        public delegate void SetToolTipContentEventHandle(ref UserControl usercontrol, IShiGongManager shiGongManager);
        /// <summary>
        /// 设置悬浮内容
        /// </summary>
        public event SetToolTipContentEventHandle SetToolTipContentEvent = null;

        #endregion

        #region 构造函数

        public DataMain()
        {
            try
            {
                InitializeComponent();

                #region 注册事件区域

                //数据名称集合体选择更改事件
                //tomCom.SelectionChanged += new SelectionChangedEventHandler(com_SelectionChanged);
                //起始年份选择更改事件
                t1.com.SelectionChanged += new SelectionChangedEventHandler(com2_SelectionChanged);
                //起始月份集合体选择更改事件
                t2.com.SelectionChanged += new SelectionChangedEventHandler(com2_SelectionChanged);

                #endregion
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DataMain", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion

        #region 初始化

        /// <summary>
        /// 初始化生成
        /// </summary>
        /// <param name="listColumn">集合</param>
        /// <param name="shiGongManagerList">施工集合</param>
        /// <param name="is注册">注册</param>
        public void DataInit(List<string> listColumn, List<IShiGongManager> shiGongManagerList, ref string is注册)
        {
            try
            {
                #region 格式化原有内容

                this.rolist.Clear();
                this.items1.Clear();
                this.dataList.Clear();
                this.sp1.Children.Clear();

                #endregion

                //菜单生成
                MenuInit();
                //使两个Scrovell同事进行
                ScrollBinding(ref is注册);

                // 添加列
                this.RowInitLeft(listColumn);

                //数据添加
                foreach (var item in shiGongManagerList)
                {
                    if (item != null && item is IShiGongManager)
                    {
                        DataControl dataControl = new DataControl(item as IShiGongManager);
                        dataControl.Tag = item;
                        dataList.Add(dataControl);
                    }
                }
                //生成bord
                enitt = new UserControlOperate(SelectBorderBursh);

                //添加使用对象(数据)
                this.UserAdd(dataList);
                //使用者名称集合
                List<string> strList = new List<string>();
                //使用者名称集合添加内容
                foreach (var item in dataList)
                {
                    item.MouseRightButtonDown += (object sender, MouseButtonEventArgs e) =>
                    {
                        menu1.Tag = sender;
                    };
                    strList.Add(item.Text);
                }

                Flush(DateTime.Now.Year.ToString() + "年", DateTime.Now.Month.ToString("00") + "月");
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DataInit", ex.ToString(), listColumn, shiGongManagerList, is注册);
            }
            finally
            {
            }
        }

        #endregion

        #region UI事件区域

        /// <summary>
        /// 查看
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainPage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //获取数据
                var data = ((sender as FrameworkElement).Parent as FrameworkElement).Tag;

                if (data == null || data.GetType() != typeof(DataControl))
                {
                    menu1.Tag = null;
                    return;
                }
                //显示对象
                DataControl dataSelf = data as DataControl;
                if (GetTargetInformationEvent != null && dataSelf != null && dataSelf.Tag != null && dataSelf.Tag is IShiGongManager)
                {
                    GetTargetInformationEvent(dataSelf.Tag as IShiGongManager);
                }
                menu1.Tag = null;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "MainPage_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }


        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainPage_Click2(object sender, RoutedEventArgs e)
        {
            try
            {
                var data = ((sender as FrameworkElement).Parent as FrameworkElement).Tag;
                if (data == null || data.GetType() != typeof(Point))
                {
                    menu1.Tag = null;
                    return;
                }
                //获取其中一个点
                Point point = (Point)data;

                if (SetTargetInformationEvent != null)
                {
                    //获取开始时间
                    DateTime d = UserControlOperate.beginT;
                    DateTime realBeginTime = d.AddDays(point.X);

                    IShiGongManager shiGonManager = null;
                    //获取到返回的任务人
                    SetTargetInformationEvent(ref shiGonManager);

                    //如果任务人不为null则执行安排
                    if (shiGonManager != null)
                    {
                        #region 时间获取

                        //获取结束时间
                        DateTime endTime = default(DateTime);
                        if (!DateTime.TryParse(shiGonManager.EndTime, out endTime)) return;

                        //如果时间没设置或设置错误,则使用默认的勾选模式
                        DateTime starTime = default(DateTime);
                        if (!DateTime.TryParse(shiGonManager.StartTime, out starTime))
                        {
                            //开始时间设置
                            shiGonManager.StartTime = realBeginTime.ToString("yyyy-MM-dd");
                        }

                        //如果周期超出界限，不再执行
                        if (endTime > realBeginTime.AddMonths(IntMouthLong)) return;

                        #endregion

                        #region 设置列

                        if (!TitleControl.striList.Contains(shiGonManager.ShiGongID))
                        {
                            //设置列
                            shiGonManager.ShiGongID = TitleControl.striList[Convert.ToInt32(point.Y)];
                        }

                        #endregion

                        //生成任务
                        AddItem(shiGonManager);
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "MainPage_Click2", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="shiGonManager">施工者</param>
        public void AddItem(IShiGongManager shiGonManager)
        {
            try
            {
                //生成任务
                DataControl datacontrol = new DataControl(shiGonManager);
                datacontrol.Tag = shiGonManager;
                dataList.Add(datacontrol);
                datacontrol.MouseRightButtonDown += (object sender2, MouseButtonEventArgs e2) =>
                {
                    menu1.Tag = sender2;
                };
                Flush(t1.com.SelectedValue, t2.com.SelectedValue);
                menu1.Tag = null;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "AddItem", ex.ToString(), shiGonManager);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 时间发生更改
        /// </summary>
        void com2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Flush(t1.com.SelectedValue, t2.com.SelectedValue);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "com2_SelectionChanged", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 鼠标右键点击时间表格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grr_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //如果对应的是监测对象,则返回
                if (menu1.Tag is DataControl)
                {
                    return;
                }
                else
                {
                    Grid grid = sender as Grid;
                    //存储坐标，给创建监测者对象提供依据
                    Point a = e.GetPosition(sender as Grid);
                    Point aEnd = new Point((int)a.X / 70, (int)a.Y / 40);
                    menu1.Tag = aEnd;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "grr_MouseRightButtonDown", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion

        #region 添加子项

        /// <summary>
        /// 添加监测对象
        /// </summary>
        /// <param name="list">集合实例</param>
        void UserAdd(List<DataControl> list)
        {
            try
            {
                //添加使用对象
                foreach (var item in list)
                {
                    if (this.SetToolTipContentEvent != null && item != null && item.Tag is IShiGongManager)
                    {
                        //将外界的模型元素给任务实例的悬浮提示
                        UserControl userControl = null;
                        this.SetToolTipContentEvent(ref userControl, item.Tag as IShiGongManager);
                        if (userControl != null)
                        {
                            //提示
                            ToolTip too = new ToolTip();
                            //提示内容
                            too.Content = userControl;
                            //提示框的格式
                            too.Style = (Style)System.Windows.Application.Current.Resources["ToolTipStyle1"];
                            //绑定提示
                            ToolTipService.SetToolTip(item, too);
                            //设置为透明
                            too.Background = new SolidColorBrush(Colors.Transparent);
                            //边框厚度为0
                            too.BorderThickness = new Thickness(0);
                        }
                    }
                    var d = item.Tag;

                    //添加使用者对象
                    this.enitt.grid.Children.Add(item);

                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "UserAdd", ex.ToString(), list);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 添加列
        /// </summary>
        public void RowInitLeft(List<string> listColumn)
        {
            try
            {
                //添加列
                foreach (var item in listColumn)
                {
                    TitleControl control = new TitleControl(item);
                    this.sp1.Children.Add(control);
                    rolist.Add(control);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "RowInitLeft", ex.ToString(), listColumn);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 添加日期和表格
        /// </summary>
        /// <param name="d1">起始时间</param>
        /// <param name="d2">终止时间</param>
        private void DateGridInit(DateTime d1, DateTime d2)
        {
            try
            {
                if (enitt != null)
                {
                    //删除日期
                    this.sp2.Children.Clear();
                    //删除表格日期
                    this.enitt.Clear();
                    //删除表格列
                    this.enitt.grid.ColumnDefinitions.Clear();
                    //删除表格行
                    this.enitt.grid.RowDefinitions.Clear();
                    //删除表格内容
                    this.enitt.grid.Children.Clear();
                }

                this.enitt = new UserControlOperate(d1, d2, rolist.Count, SelectBorderBursh);
                //如果皮肤有修改记录，则进行皮肤设置
                if (DefaultColor != default(Color)) SkinChange(DefaultColor);

                //添加日期
                foreach (var item in this.enitt)
                {
                    //添加日期
                    this.sp2.Children.Add(item);
                }

                //添加表格
                grr.Children.Add(this.enitt.grid);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DateGridInit", ex.ToString(), d1, d2);
            }
            finally
            {
            }
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 刷新界面
        /// </summary>
        public void Flush(object p1, object p2)
        {
            try
            {

                //两者必须有，才能符合格式转换为时间
                if (p1 != null && p2 != null)
                {
                    //将月份和时间组合
                    string pStr = p1.ToString() + p2.ToString();
                    //起始时间
                    DateTime dateBegin = Convert.ToDateTime(pStr);

                    //（结束时间）设定时间的范围
                    DateTime dateEnd = dateBegin.AddMonths(IntMouthLong);
                    //获取在指定范围时间内的使用者对象
                    List<DataControl> a = dataList.Where(item => item.BeginTime >= dateBegin && item.EndTime <= dateEnd).ToList();
                    //添加日期和表格
                    this.DateGridInit(dateBegin, dateEnd);
                    // 添加使用对象
                    this.UserAdd(a);
                    //临时的文本集合
                    List<string> strList = new List<string>();
                    //文本集合添加内容
                    foreach (var item in a)
                    {
                        strList.Add(item.Text);
                    }

                    reng1.Text = string.Format("{0}", dateEnd.ToString("yyyy年MM月"));
                    //导航文本下拉框绑定集合
                    //tomCom.ItemsSource =strList;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Flush", ex.ToString(), p1, p2);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 绑定偏移
        /// </summary>
        private void ScrollBinding(ref string is注册)
        {
            try
            {
                if (is注册.Equals("未注册"))
                {
                    this.SetScrolliverBinding(gridCenter, this.scro);
                    this.VerticalScrollChanged += new EventHandler(MainPage_VerticalScrollChanged);

                    this.SetScrolliverHorBinding(gridCenter, this.scro);
                    this.HorcalScrollChanged += new EventHandler(MainPage_HorcalScrollChanged);
                    is注册 = "已注册";
                }
                else
                {
                    if (VerticalScrollChanged != null)//如果有注册，则驱动它 
                        VerticalScrollChanged(gridCenter, EventArgs.Empty);

                    if (HorcalScrollChanged != null)//如果有注册，则驱动它 
                        HorcalScrollChanged(gridCenter, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ScrollBinding", ex.ToString(), is注册);
            }
            finally
            {
            }
        }

        #region 生成菜单
        /// <summary>
        /// 生成菜单
        /// </summary>
        public void MenuInit()
        {
            try
            {
                items1.Add(new CustomMenuItem("查看详情", "查看详情"));
                items1.Add(new CustomMenuItem("分配任务", "分配任务"));
                //加载菜单子项
                foreach (var item in items1)
                {
                    menu1.Items.Add(item);
                }
                items1[0].Click += new RoutedEventHandler(MainPage_Click);
                items1[1].Click += new RoutedEventHandler(MainPage_Click2);
                ContextMenuService.SetContextMenu(grr, menu1);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "MenuInit", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion

        #endregion

        #region 皮肤设计

        /// <summary>
        /// 皮肤更换
        /// </summary>
        /// <param name="color"></param>
        public void SkinChange(Color color)
        {
            try
            {
                //边框颜色
                (this.Resources["borderBrush"] as SolidColorBrush).Color = color;
                //(this.Resources["backBrush"] as SolidColorBrush).Color = color;
                //列标题颜色
                rolist.ForEach(Item => (Item.Resources["linear1"] as LinearGradientBrush).GradientStops[1].Color = color);
                //表格颜色
                foreach (var item in enitt)
                {
                    (item.Resources["blueWhiteColor"] as LinearGradientBrush).GradientStops[0].Color = color;
                    (item.Resources["BlueColor"] as LinearGradientBrush).GradientStops[1].Color = color;
                }
                (this.Resources["borTTT"] as LinearGradientBrush).GradientStops[0].Color = color;
                (this.Resources["borTTT"] as LinearGradientBrush).GradientStops[2].Color = color;    
                DefaultColor = color;
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

        #region 旧方案

        /// <summary>
        /// 垂直偏移发生时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainPage_VerticalScrollChanged(object sender, EventArgs e)
        {
            try
            {
                this.scroLeft.ScrollToVerticalOffset(this.scro.VerticalOffset);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "MainPage_VerticalScrollChanged", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }


        /// <summary>
        /// 水平偏移发生时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainPage_HorcalScrollChanged(object sender, EventArgs e)
        {
            try
            {
                this.scroTop.ScrollToHorizontalOffset(this.scro.HorizontalOffset);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "MainPage_HorcalScrollChanged", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        public event EventHandler VerticalScrollChanged;
        public void SetScrolliverBinding(FrameworkElement element, ScrollViewer scroviell)
        {
            try
            {
                element.SetBinding(//绑定 
                DependencyProperty.RegisterAttached("VerticalOffset",//注册一个附加属性 
                typeof(double),//这个属性的类型是 double 
                element.GetType(),//需要注册到自己 
                new PropertyMetadata((d, e) =>//元数据，注册变化时的事件 
                {
                    if (VerticalScrollChanged != null)//如果有注册，则驱动它 
                        VerticalScrollChanged(element, EventArgs.Empty);
                })),
              new Binding("VerticalOffset") { Source = scroviell });//绑定的源和路径 
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "SetScrolliverBinding", ex.ToString(), element, scroviell);
            }
            finally
            {
            }
        }



        public event EventHandler HorcalScrollChanged;
        public void SetScrolliverHorBinding(FrameworkElement element, ScrollViewer scroviell)
        {
            try
            {
                element.SetBinding(//绑定 
                DependencyProperty.RegisterAttached("HorizontalOffset",//注册一个附加属性 
                typeof(double),//这个属性的类型是 double 
                element.GetType(),//需要注册到自己 
                new PropertyMetadata((d, e) =>//元数据，注册变化时的事件 
                {
                    if (HorcalScrollChanged != null)//如果有注册，则驱动它 
                        HorcalScrollChanged(element, EventArgs.Empty);
                })),
              new Binding("HorizontalOffset") { Source = scroviell });//绑定的源和路径 
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "SetScrolliverHorBinding", ex.ToString(), element, scroviell);
            }
            finally
            {
            }
        }

        #endregion
    }
}
