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
using MhczTBG.Common;

namespace MhczTBG.Controls.RiLi
{

    public partial class RiLi : UserControl
    {
        #region 变量

        string year = string.Empty;
        /// <summary>
        /// 临时存储年
        /// </summary>
        public string Year
        {
            get { return year; }
            set { year = value; }
        }


        string month = string.Empty;
        /// <summary>
        /// 临时存储月
        /// </summary>
        public string Month
        {
            get { return month; }
            set { month = value; }
        }

        /// <summary>
        /// 临时存储完整日期（年月日）
        /// </summary>
        string datetime = string.Empty;

        /// <summary>
        /// 存储日历子项数据
        /// </summary>
        List<IRiLiEntity> riLiEntityList = new List<IRiLiEntity>();

        /// <summary>
        /// 存储日历子项集合
        /// </summary>
        public List<RiLiItem> riLiItemList = new List<RiLiItem>();

        #endregion

        #region 自定义委托事件

        public delegate void ClickGetInfoEventHandle(IRiLiEntity riLiEntity);
        /// <summary>
        /// 当点击之后所激发该事件，能过获取相应的值，并执行额外的方法去展示信息
        /// </summary>
        public event ClickGetInfoEventHandle ClickGetInfoEvent = null;

        public delegate void SetInfoEventHandle(ref IRiLiEntity riLiEntity);
        /// <summary>
        /// 添加一个新的日记所要激发的事件
        /// </summary>
        public event SetInfoEventHandle SetInfoEvent = null;


        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public RiLi()
        {
            try
            {
                InitializeComponent();
                //获取今年
                year = DateTime.Now.Year.ToString();
                //获取这个月
                month = DateTime.Now.Month.ToString();
                //获取完整时间
                datetime = year + "/" + month + "/" + DateTime.Now.Day;
                //将左右的月份绑定到导航按钮的Tag里
                SetTag(year, month);

                BindRiLiItem(year, month);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "RiLi", ex.ToString());
            }
            finally
            {
            }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Year">年</param>
        /// <param name="Month">月</param>
        public RiLi(string Year, string Month)
        {
            try
            {
                InitializeComponent();
                year = Year;
                month = Month;
                //将左右的月份绑定到导航按钮的Tag里
                SetTag(year, month);

                BindRiLiItem(year, month);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "RiLi", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion

        #region 生成日历并绑定数据
        /// <summary>
        /// 生成日历并绑定数据
        /// </summary>
        /// <param name="_Year">年</param>
        /// <param name="_Month">月</param>
        void BindRiLiItem(string _Year, string _Month)
        {
            try
            {
                #region 设置年月

                this.Year = _Year;
                this.Month = _Month;

                #endregion

                //清空所有日历子项
                LayoutRoot.Children.Clear();

                //如果超过五行  删除多余的行   日历默认为五行
                if (LayoutRoot.RowDefinitions.Count > 5)
                {
                    for (int i = 5; i < LayoutRoot.RowDefinitions.Count; i++)
                    {
                        LayoutRoot.RowDefinitions.RemoveAt(i);
                    }
                }
                //如果不足五行  添加一行
                else if (LayoutRoot.RowDefinitions.Count < 5)
                {
                    RowDefinition row = new RowDefinition();
                    LayoutRoot.RowDefinitions.Add(row);
                }
                //绑定外部横向标题
                this.tbMonthDay.Text = string.Format("{0}年{1}月", Year, Month);
                //获取本月所有天数
                int daycount = DateTime.DaysInMonth(Convert.ToInt32(Year), Convert.ToInt32(Month));
                //获取星期
                string weekone = getweek(Convert.ToInt32(Year), Convert.ToInt32(Month));


                //如果当前月一号是星期六，就得新添一行 
                if (daycount + Convert.ToInt32(weekone) > 35)
                {
                    RowDefinition row = new RowDefinition();
                    LayoutRoot.RowDefinitions.Add(row);
                }
                //如果本月1号刚好为周日， 并且是闰月28， 就删除多余行
                else if (daycount + Convert.ToInt32(weekone) == 28)
                {
                    LayoutRoot.RowDefinitions.RemoveAt(4);
                }

                #region 生成日历子项

                for (int i = 0; i < daycount; i++)
                {
                    RiLiItem riliitem = new RiLiItem();
                    //添加到日历子项集合中去
                    riLiItemList.Add(riliitem);
                    riliitem.DayTitle.Content = i + 1 + "日";
                    riliitem.Tag = Year + "/" + Month + "/" + (i + 1);

                    if (riliitem.Tag.Equals(datetime))
                    {
                        riliitem.DayTitle.Background = new SolidColorBrush(Color.FromRgb(13, 124, 193));
                    }
                    //新建右键菜单项
                    ContextMenu cm = new ContextMenu();
                    MenuItem mEditPlan = new MenuItem();
                    mEditPlan.Header = "添加";
                    mEditPlan.Tag = Year + "/" + Month + "/" + (i + 1);
                    mEditPlan.Click += new RoutedEventHandler(mEditPlan_Click);
                    cm.Items.Add(mEditPlan);
                    ContextMenuService.SetContextMenu(riliitem.SpackpanelItem, cm);



                    #region 设置子项的位置

                    //判断位置
                    int gridrow = Convert.ToInt32(weekone) + i;

                    if (gridrow <= 6)
                    {
                        Grid.SetRow(riliitem, 0);
                        Grid.SetColumn(riliitem, gridrow);
                    }
                    else if (gridrow <= 13 && gridrow > 6)
                    {
                        int gridrow1 = gridrow - 7;
                        Grid.SetRow(riliitem, 1);
                        Grid.SetColumn(riliitem, gridrow1);
                    }
                    else if (gridrow <= 20 && gridrow > 13)
                    {
                        int gridrow2 = gridrow - 14;
                        Grid.SetRow(riliitem, 2);
                        Grid.SetColumn(riliitem, gridrow2);
                    }
                    else if (gridrow <= 27 && gridrow > 20)
                    {
                        int gridrow3 = gridrow - 21;
                        Grid.SetRow(riliitem, 3);
                        Grid.SetColumn(riliitem, gridrow3);
                    }
                    else if (gridrow <= 34 && gridrow > 27)
                    {
                        int gridrow4 = gridrow - 28;
                        Grid.SetRow(riliitem, 4);
                        Grid.SetColumn(riliitem, gridrow4);
                    }
                    else if (gridrow <= 41 && gridrow > 34)
                    {
                        int gridrow4 = gridrow - 35;
                        Grid.SetRow(riliitem, 5);
                        Grid.SetColumn(riliitem, gridrow4);
                    }

                    #endregion

                    this.LayoutRoot.Children.Add(riliitem);
                }

                #endregion

                #region 添加日记

                for (int j = 0; j < riLiEntityList.Count; j++)
                {
                    this.ItemsAdd(riLiEntityList[j]);
                }

                #endregion

                //把没有riliitem的地方补上边框
                for (int i = 0; i < Convert.ToInt32(weekone); i++)
                {
                    Border border = new Border();
                    border.BorderThickness = new Thickness(1, 1, 0, 0);
                    border.BorderBrush = new SolidColorBrush(Color.FromRgb(211, 211, 211));


                    Grid.SetRow(border, 0);
                    Grid.SetColumn(border, i);
                    this.LayoutRoot.Children.Add(border);
                }

                //补全没有添加riliitem的地方
                if (daycount + Convert.ToInt32(weekone) > 27 && daycount + Convert.ToInt32(weekone) <= 34)
                {
                    for (int i = 0; i < 35 - (daycount + Convert.ToInt32(weekone)); i++)
                    {
                        Border border = new Border();
                        border.BorderThickness = new Thickness(1, 1, 0, 0);
                        border.BorderBrush = new SolidColorBrush(Color.FromRgb(211, 211, 211));

                        Grid.SetRow(border, 5);
                        Grid.SetColumn(border, daycount + Convert.ToInt32(weekone) + i - 28);
                        this.LayoutRoot.Children.Add(border);
                    }
                }
                //补全没有添加riliitem的地方
                if (daycount + Convert.ToInt32(weekone) > 34 && daycount + Convert.ToInt32(weekone) <= 41)
                {


                    for (int i = 0; i < 42 - (daycount + Convert.ToInt32(weekone)); i++)
                    {
                        Border border = new Border();
                        border.BorderThickness = new Thickness(1, 1, 0, 0);
                        border.BorderBrush = new SolidColorBrush(Color.FromRgb(211, 211, 211));


                        Grid.SetRow(border, 5);
                        Grid.SetColumn(border, daycount + Convert.ToInt32(weekone) + i - 35);
                        this.LayoutRoot.Children.Add(border);
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "BindRiLiItem", ex.ToString(), _Year, _Month);
            }
            finally
            {
            }
        }

        #endregion

        #region 添加一个日记
        /// <summary>
        ///  添加一个日记
        /// </summary>
        /// <param name="entity">日记</param>
        public void ItemsAdd(IRiLiEntity entity)
        {
            try
            {
                if (!riLiEntityList.Contains(entity))
                {
                    riLiEntityList.Add(entity);
                }
                foreach (var riliitem in riLiItemList)
                {
                    if (entity.DateTime == riliitem.Tag.ToString())
                    {
                        Button button = new Button();
                        button.Content = entity.Name;
                        button.Tag = entity;
                        button.ToolTip = entity.Name;
                        button.Margin = new Thickness(0, 1, 0, 1);
                        button.Cursor = Cursors.Hand;
                        button.BorderThickness = new Thickness(0);
                        button.Background = new SolidColorBrush(Colors.Black);
                        button.Background.Opacity = 0;
                        button.Click += new RoutedEventHandler(Item_Click);
                        button.Style = this.Resources["RiLiItemItem"] as Style;
                        riliitem.SpackpanelItem.Children.Add(button);

                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ItemsAdd", ex.ToString(), entity);
            }
            finally
            {
            }
        }

        #endregion

        #region 逻辑事件区域

        private void Item_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ClickGetInfoEvent != null)
                {
                    ClickGetInfoEvent((sender as Button).Tag as IRiLiEntity);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Item_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 快速导航到今天
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void today_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SetTag(datetime.Split(new char[] { '/' })[0], datetime.Split(new char[] { '/' })[1]);
                BindRiLiItem(datetime.Split(new char[] { '/' })[0], datetime.Split(new char[] { '/' })[1]);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "today_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 添加事件到日历中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void itemADD_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 上一个月日历切换按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnoldMonth_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string oldmonth = ((Button)sender).Tag.ToString();
                SetTag(oldmonth.Split(new char[] { ',' })[0], oldmonth.Split(new char[] { ',' })[1]);
                BindRiLiItem(oldmonth.Split(new char[] { ',' })[0], oldmonth.Split(new char[] { ',' })[1]);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btnoldMonth_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 下一个月日历切换按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnnewMonth_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string oldmonth = ((Button)sender).Tag.ToString();
                SetTag(oldmonth.Split(new char[] { ',' })[0], oldmonth.Split(new char[] { ',' })[1]);
                BindRiLiItem(oldmonth.Split(new char[] { ',' })[0], oldmonth.Split(new char[] { ',' })[1]);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "btnnewMonth_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        void mEditPlan_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SetInfoEvent != null)
                {
                    ///添加一个日记
                    IRiLiEntity riLiEntity = null;
                    SetInfoEvent(ref riLiEntity);
                    if (riLiEntity != null)
                    {
                        //获取对应日期
                        string datetime = ((MenuItem)sender).Tag.ToString();
                        //日期是所选择而不是人为的设置的
                        riLiEntity.DateTime = datetime;
                        //添加日记
                        this.ItemsAdd(riLiEntity);
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "mEditPlan_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        #endregion

        #region 辅助类

        /// <summary>
        /// 将导航月份的按钮绑定相应的日期数据（1和12是比较特殊的，1针对左导航，12针对右导航）
        /// </summary>
        /// <param name="year1">年</param>
        /// <param name="month1">月</param>
        private void SetTag(string year1, string month1)
        {
            try
            {
                //如果tag值不为12或者1月份    可以加一或者减一
                if (Convert.ToInt32(month1) != 12 && Convert.ToInt32(month1) != 1)
                {
                    this.btnoldMonth.Tag = year1 + "," + Convert.ToString(Convert.ToInt32(month1) - 1);
                    this.btnnewMonth.Tag = year1 + "," + Convert.ToString(Convert.ToInt32(month1) + 1);
                }
                //当前月份为12月份   
                else if (Convert.ToInt32(month1) == 12)
                {
                    this.btnoldMonth.Tag = year1 + "," + Convert.ToString(Convert.ToInt32(month1) - 1);
                    this.btnnewMonth.Tag = Convert.ToString(Convert.ToInt32(year1) + 1) + ",1";
                }
                //当前如果是一月份  
                else if (Convert.ToInt32(month1) == 1)
                {
                    this.btnoldMonth.Tag = Convert.ToString(Convert.ToInt32(year1) - 1) + ",12";
                    this.btnnewMonth.Tag = year1 + "," + Convert.ToString(Convert.ToInt32(month1) + 1);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "SetTag", ex.ToString(), year1, month1);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 根据所提供的日期获取当前星期
        /// </summary>
        /// <param name="Year">年份</param>
        /// <param name="Mohth">月份</param>
        public static string getweek(int Year, int Mohth)
        {
            string week = string.Empty;
            try
            {
                DateTime datetime = new DateTime(Year, Mohth, 1);
                string weekName = datetime.DayOfWeek.ToString();

                switch (weekName)
                {
                    case "Sunday":
                        week = "0";
                        break;
                    case "Monday":
                        week = "1";
                        break;
                    case "Tuesday":
                        week = "2";
                        break;
                    case "Wednesday":
                        week = "3";
                        break;
                    case "Thursday":
                        week = "4";
                        break;
                    case "Friday":
                        week = "5";
                        break;
                    case "Saturday":
                        week = "6";
                        break;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(typeof(RiLi).ToString(), "OKButton_Click", ex.ToString(), Year, Mohth);
            }
            finally
            {
            }
            return week;
        }

        #endregion
    }
}
