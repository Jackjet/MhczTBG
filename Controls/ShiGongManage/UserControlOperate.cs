using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using MhczTBG.Common;

namespace MhczTBG.Controls.ShiGongManage
{
    class UserControlOperate : List<UserControl>
    {
        #region 变量

        /// <summary>
        /// 开始时间
        /// </summary>
        public static DateTime beginT = default(DateTime);
        /// <summary>
        /// 结束时间
        /// </summary>
        public static DateTime endT = default(DateTime);

        /// <summary>
        /// 临时存储时间
        /// </summary>
        Border border = null;

        /// <summary>
        /// 选择之后的背景设置
        /// </summary>
        Brush SelectBrush = null;
      
        #endregion

        #region 构造函数

        /// <summary>
        /// 具体所生成的虚拟承载表格
        /// </summary>
        public Grid grid = new Grid();

        /// <summary>
        /// 表格、日期构造
        /// </summary>
        public UserControlOperate(Brush brus)
        {
            try
            {
                SelectBrush = brus;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "UserControlOperate", ex.ToString(), brus);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 表格、日期构造
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="row">行数</param>
        public UserControlOperate(DateTime beginTime, DateTime endTime, int row, Brush brush)
            : this(brush)
        {
            try
            {
                grid.Background = new SolidColorBrush(Colors.Transparent);
                //算出起始时间的那一天是星期几
                DayOfWeek week = beginTime.DayOfWeek;
                //往前补，所以减去星期几的数
                beginT = beginTime.AddDays(-1 * Convert.ToInt32(week));

                //算出结束时间的那一天是星期几
                DayOfWeek weekEnd = endTime.DayOfWeek;
                //往后补，所以减去星期剩余的数
                endT = endTime.AddDays(7 - Convert.ToInt32(weekEnd));

                //算出起始时间与结束时间的时间差
                TimeSpan a = endT - beginT;

                //获取时间差的天数
                double d = Convert.ToInt32(a.Days);

                //用总天数去除以7获取总的星期数，也就是星期控件的数量
                double count = d / 7;


                for (int i = 0; i < d; i += 7)
                {
                    //起始时间文本
                    string c = beginT.AddDays(i).ToString("yyyy年MM月dd日");
                    //结束时间文本
                    string c2 = beginT.AddDays(i + 6).ToString("yyyy年MM月dd日");

                    //日期控件（行的数量、时间文本）
                    UserControl control = new DateWeekControl(c + "--" + c2);
                    //添加日期控件
                    this.Add(control);
                }


                //给Grid分列（单元格）（使用者对象承载体）
                for (int j = 0; j < d; j++)
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(70) });
                    Border border = new Border() { BorderBrush = new SolidColorBrush(Colors.Silver), BorderThickness = new Thickness(0, 0, 1, 1) };
                    border.MouseLeftButtonDown += new MouseButtonEventHandler(border_MouseLeftButtonDown);

                    Grid.SetColumn(border, j);
                    grid.Children.Add(border);
                }

                //给Grid分行（单元格）（使用者对象承载体）
                for (int i = 0; i < row; i++)
                {
                    grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(40) });
                    //给Grid分列（单元格）（使用者对象承载体）
                    for (int j = 0; j < d; j++)
                    {
                        Border border = new Border() { Background = new SolidColorBrush(Colors.Transparent), BorderBrush = new SolidColorBrush(Colors.Silver), BorderThickness = new Thickness(0, 0, 1, 1) };
                        border.MouseLeftButtonDown += new MouseButtonEventHandler(border_MouseLeftButtonDown);
                        Grid.SetRow(border, i);
                        Grid.SetColumn(border, j);
                        grid.Children.Add(border);
                    }
                }

            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "UserControlOperate", ex.ToString(), beginTime, endTime, row, brush);
            }
            finally
            {
            }
        }

        #endregion

        #region UI事件区域

        void border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //如果border已经有时间存储值
                if (border != null)
                {
                    //背景设置为透明
                    border.Background = new SolidColorBrush(Colors.Transparent);
                    //如果不为空则设置为空
                    if (border.Child != null)
                    {
                        border.Child = null;
                    }
                }
                //设置border
                border = sender as Border;

                //将border背景变为灰色
                border.Background = SelectBrush == null ? new SolidColorBrush(Colors.LightGray) : SelectBrush;

                //显示时间
                TextBlock txt = new TextBlock() { Cursor = Cursors.Hand, HorizontalAlignment = System.Windows.HorizontalAlignment.Center, VerticalAlignment = System.Windows.VerticalAlignment.Center };
                border.Child = txt;

                DateTime d = UserControlOperate.beginT;
                //按照一定的格式去显示
                DateTime realBeginTime = d.AddDays(Grid.GetColumn(border));
                txt.Text = realBeginTime.ToString("MM-dd");
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "border_MouseLeftButtonDown", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion
    }
}
