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
using System.Windows.Media.Imaging;
using MhczTBG.Common;

namespace MhczTBG.Controls.ShiGongManage
{
    partial class DataControl : UserControl
    {
        #region 变量

        int leftData;
        /// <summary>
        /// 向左偏移的量
        /// </summary>
        public int LeftData
        {
            get { return leftData; }
            set
            {
                if (value != leftData)
                {
                    //向左偏移的量通过设置列的位置
                    Grid.SetColumn(this, Convert.ToInt32(value));
                    leftData = value;
                }
            }
        }

        double complte;
        /// <summary>
        /// 完成情况
        /// </summary>
        public double Complte
        {
            get { return complte; }
            set { complte = value; }
        }

        int length;
        /// <summary>
        /// 具体长度
        /// </summary>
        public int Length
        {
            get { return length; }
            set
            {
                //具体的长度通过获取单元表单的宽度来设置
                this.Width = value * 70;
                //夸越列的数量
                Grid.SetColumnSpan(this, value);
                length = value;
            }
        }

        string text;
        /// <summary>
        /// 名称
        /// </summary>
        public string Text
        {
            get { return text; }
            set
            {
                if (value != text)
                {
                    //数据名称的设置（显示）
                    this.datatext.Text = value.ToString();
                    text = value;
                }
            }
        }

        string jobContent;
        /// <summary>
        /// 具体工作内容
        /// </summary>
        public string JobContent
        {
            get { return jobContent; }
            set { jobContent = value; }
        }


        string trainName;
        /// <summary>
        /// 所属列名称
        /// </summary>
        public string TrainName
        {
            get { return trainName; }
            set { trainName = value; }
        }


        int trainPosition;
        /// <summary>
        /// 列的位置
        /// </summary>
        public int TrainPosition
        {
            get { return trainPosition; }
            set
            {
                if (value != trainPosition)
                {
                    //设置列的位置
                    Grid.SetRow(this, value);
                    trainPosition = value;
                }
            }
        }

        DateTime beginTime;
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginTime
        {
            get { return beginTime; }
            set { beginTime = value; }
        }

        DateTime endTime;
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

        /// <summary>
        /// 是否可以生成该元素
        /// </summary>
        bool canInit = false;

        #endregion

        #region 构造函数

        /// <summary>
        /// 数据构造函数（火车头）
        /// </summary>
        /// <param name="leftdata">占有条里左边边界的距离</param>
        /// <param name="color">颜色</param>
        /// <param name="length">占用条长度</param>
        /// <param name="value">车辆编号</param>
        /// <param name="status">
        ///     1:资源尚未开始占用
        ///     2:资源占用中
        ///     3:资源结束占用
        /// </param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="txt">名称</param>
        /// <param name="trainName">所属列</param>
        /// <param name="complete">完成情况</param>
        public DataControl(IShiGongManager person)
        {
            try
            {
                LoadInit(person, ref canInit);
                if (canInit)
                {
                    InitializeComponent();

                    //使用者对象的文本
                    Text = person.DisPlayName;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DataControl", ex.ToString(), person);
            }
            finally
            {
            }
        }

        #endregion

        #region 初始化

        void LoadInit(IShiGongManager person, ref bool canInitt)
        {
            try
            {
                #region 时间设置

                DateTime dT = default(DateTime);
                if (string.IsNullOrEmpty(person.StartTime) || !DateTime.TryParse(person.StartTime, out(dT))) return;
                if (string.IsNullOrEmpty(person.ShiGongID)) return;
                if (string.IsNullOrEmpty(person.EndTime) || !DateTime.TryParse(person.EndTime, out(dT))) return;


                if (Convert.ToDateTime(beginTime) > Convert.ToDateTime(person.EndTime)) return;
                //开始时间与结束时间的时间差
                TimeSpan k = Convert.ToDateTime(person.EndTime) - Convert.ToDateTime(person.StartTime);
                //开始时间（每一个使用者对象）
                this.BeginTime = Convert.ToDateTime(person.StartTime);
                //结束时间（每一个使用者对象）
                this.EndTime = Convert.ToDateTime(person.EndTime);

                if (k.Days < 0) return;
                //每一个使用者对象的长度（比如5月1至5月4日）
                Length = k.Days + 1;

                #endregion

                #region 设置状态

                if (person.InZhaungTai < 0 || person.InZhaungTai > 1) return;
                if (person.InZhaungTai == 0) person.strZhuangTai = "未进行";
                else if (person.InZhaungTai == 1) person.strZhuangTai = "已完成";
                else person.strZhuangTai = "进行中";

                #endregion

                #region 对应的列

                //通过标题集合中该对象名称的索引来设置列的位置
                int postion = TitleControl.striList.IndexOf(person.ShiGongID);
                //下标为-1，则说明不在这个区域
                if (postion < 0) return;
                //设置条件为大于0
                if (postion > 0)
                {
                    TrainPosition = postion;
                }

                #endregion

                //完成情况
                Complte = person.InZhaungTai;

                //所属列名称
                TrainName = person.ShiGongID;
                // 加载事件
                this.Loaded += new RoutedEventHandler(DataControl_Loaded);

                //能一直执行到这里，说明允许生成该任务
                canInitt = true;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "LoadInit", ex.ToString(), person, canInitt);
            }
            finally
            {
            }
        }

        #endregion

        #region UI事件区域

        /// <summary>
        /// 加载事件
        /// </summary>
        void DataControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //设置使用者对象的偏移量（日期控件的开始时间与使用者对象的开始时间）
                TimeSpan k1 = Convert.ToDateTime(beginTime) - UserControlOperate.beginT;
                //天数即为偏移的量
                LeftData = k1.Days;

                //完成情况如果是有完成的情况


                if (complte > 0 && complte <= 1)
                {
                    //第一个颜色条的长度
                    this.b1.Width = this.Width * complte;
                    //第一个颜色条上色
                    b1.Background = this.Resources["BlueColor"] as Brush;

                    //第二个颜色条的长度
                    this.b2.Width = this.Width * (1 - complte);
                    //第二个颜色条上色
                    b2.Background = this.Resources["YellowColor"] as Brush;

                }
                //一点也没动的情况
                else if (complte == 0)
                {
                    //第一个颜色条充满
                    this.b1.Width = this.Width;
                    //设置为红色
                    b1.Background = this.Resources["RedColor"] as Brush;
                    //图标为未开始
                    this.trainImg.Source = new BitmapImage(new Uri(Environment.CurrentDirectory.Replace("bin\\Debug", "Image\\") + "trc.png", UriKind.Relative));
                }
                if (complte == 1 || complte == 0)
                {
                    this.b1.CornerRadius = new CornerRadius(15);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DataControl_Loaded", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion
    }
}
