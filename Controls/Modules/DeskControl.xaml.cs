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
using System.Windows.Media.Animation;
using System.Diagnostics;
using System.Windows.Threading;
using MhczTBG.Controls.CustomWindow;

namespace MhczTBG.Controls.Modules
{
    /// <summary>
    /// Interaction logic for DeskControl.xaml
    /// </summary>
    public partial class DeskControl : UserControl
    {
        #region 变量
        
        static Point startPoint = new Point();      //开始坐标
        static Point endPoint = new Point();        //结束坐标
        FrameworkElement currentElement = null;     //要拖动的控件
        static bool isSecect = false;               //是否选中
        static int page = 0;
        static int oldpage = 0;
        double screen_x = SystemParameters.WorkArea.Width, tovalue = 0,fromvalue=0;
        Storyboard changepage_sb = new Storyboard();

        Border bordMain_1;
        /// <summary>
        /// 第一个画面承载的容器
        /// </summary>
        public Border BordMain_1
        {
            get { return bordMain_1; }
            set
            {
                if (value != null)
                {
                    this.borMain1.Child = value;
                    bordMain_1 = value;
                }
            }
        }

        Border bordMain_2;
        /// <summary>
        /// 第2个画面承载的容器
        /// </summary>
        public Border BordMain_2
        {
            get { return bordMain_2; }
            set
            {
                if (value != null)
                {
                    this.borMain2.Child = value;
                    bordMain_2 = value;
                }
            }
        }

        Border bordMain_3;
        /// <summary>
        /// 第一个画面承载的容器
        /// </summary>
        public Border BordMain_3
        {
            get { return bordMain_3; }
            set
            {
                if (value != null)
                {
                    this.bordMain_3.Child = value;
                    bordMain_3 = value;
                }
            }
        }

        Border bordMain_4;
        /// <summary>
        /// 第一个画面承载的容器
        /// </summary>
        public Border BordMain_4
        {
            get { return bordMain_4; }
            set
            {
                if (value != null)
                {
                    this.bordMain_4.Child = value;
                    bordMain_4 = value;
                }
            }
        }

        Border bordMain_5;
        /// <summary>
        /// 第一个画面承载的容器
        /// </summary>
        public Border BordMain_5
        {
            get { return bordMain_5; }
            set
            {
                if (value != null)
                {
                    this.bordMain_5.Child = value;
                    bordMain_5 = value;
                }
            }
        }


        #endregion

        #region 添加子项



        #endregion

        public DeskControl()
        {
            InitializeComponent();

            #region 前后台元素核对

            //if (BordMain_1 == null) BordMain_1 = borMain1;
            //if (BordMain_2 == null) BordMain_2 = borMain2;
            //if (BordMain_3 == null) BordMain_3 = borMain3;
            //if (BordMain_4 == null) BordMain_4 = borMain4;
            //if (BordMain_5 == null) BordMain_5 = borMain5;

            #endregion
           
            this.desk_sp.MouseLeftButtonDown += new MouseButtonEventHandler(desk_sp_MouseLeftButtonDown);
            this.desk_sp.MouseMove += new MouseEventHandler(desk_sp_MouseMove);
            this.desk_sp.MouseLeftButtonUp += new MouseButtonEventHandler(desk_sp_MouseLeftButtonUp);

            this.pagelist.SelectionChanged += new SelectionChangedEventHandler(pagelist_SelectionChanged);
                    
            changepage_sb.Completed += new EventHandler(changepage_sb_Completed);

            desk_sp.Margin = new Thickness(0);
            currentElement = desk_sp;
            this.pagelist.SelectedIndex = 0;
        }

      
        void icon7_Click(object sender, RoutedEventArgs e)
        {
            ChildWindow childwindow = new ChildWindow();
            childwindow.Show();
        }

    

        //点击页面切换按钮
        void pagelist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isSecect == false)
            {
                page = (sender as ListBox).SelectedIndex;
                if (page > oldpage)
                {
                    for (int i = oldpage + 1; i < page; i++)
                    {
                        (this.FindName("page" + (i + 1).ToString()) as Grid).Visibility = Visibility.Collapsed;
                    }
                }
                else if (page < oldpage)
                {
                    for (int i = oldpage - 1; i > page; i--)
                    {
                        (this.FindName("page" + (i + 1).ToString()) as Grid).Visibility = Visibility.Collapsed;
                        tovalue = screen_x + tovalue;
                    }
                    SetMargin();
                    fromvalue = tovalue;
                }
                ChangePage1(oldpage, page);
                oldpage = page;
            }
        }

        #region [重新设置，定位margin]
        public void SetMargin()
        {
            Storyboard sb = new Storyboard();
            ThicknessAnimation myAnimation = new ThicknessAnimation();
            myAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.01));
            myAnimation.To = new Thickness(tovalue, 0, 0, 0); ;
            Storyboard.SetTarget(myAnimation, desk_sp);
            Storyboard.SetTargetProperty(myAnimation, new PropertyPath(MarginProperty));
            sb.Children.Add(myAnimation);
            sb.Begin();
        }
        #endregion

        #region [滑屏鼠标事件]
        void desk_sp_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isSecect)
            {
                currentElement.ReleaseMouseCapture();//移除鼠标捕获
            }
            startPoint = e.GetPosition(null);//获取当前坐标 

            if (startPoint.X - endPoint.X > 50)
            {
                if (page != 0 && (startPoint.X - endPoint.X > 80))
                {
                    page--;
                }
                tovalue = -screen_x * page;
            }
            else if (startPoint.X - endPoint.X <= 50)
            {
                if (page != 4 && (startPoint.X - endPoint.X < -80))
                {
                    page++;
                }
                tovalue = -screen_x * page;
            }
            pagelist.SelectedIndex = page;
            oldpage = page;
            isSecect = false;
            ChangePage(tovalue);
        }
        void desk_sp_MouseMove(object sender, MouseEventArgs e)
        {
            if (isSecect)
            {
                Point pt = e.GetPosition(null);             //获取当前坐标
                double pos = (pt.X - startPoint.X) + tovalue;
                Storyboard sb = new Storyboard();
                ThicknessAnimation myAnimation = new ThicknessAnimation();
                myAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.01));
                myAnimation.From = new Thickness(pos, 0, 0, 0);
                myAnimation.To = new Thickness(pos, 0, 0, 0);
                Storyboard.SetTarget(myAnimation, desk_sp);
                Storyboard.SetTargetProperty(myAnimation, new PropertyPath(MarginProperty));
                sb.Children.Add(myAnimation);
                sb.Begin();

            }
        }
        void desk_sp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            currentElement = sender as FrameworkElement;
            currentElement.CaptureMouse();//鼠标捕获
            isSecect = true;//选中
            startPoint = e.GetPosition(null);//获取当前坐标 
            endPoint = e.GetPosition(null);
        }
        #endregion

        #region [滑屏动画]
        public void ChangePage(double tovalue)
        {
            Storyboard sb = new Storyboard();
            ThicknessAnimation myAnimation = new ThicknessAnimation();
            myAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            myAnimation.From = currentElement.Margin;
            myAnimation.To = new Thickness(tovalue, 0, 0, 0);
            Storyboard.SetTarget(myAnimation, desk_sp);
            Storyboard.SetTargetProperty(myAnimation, new PropertyPath(MarginProperty));
            sb.Children.Add(myAnimation);
            sb.Begin();
        }

        #endregion
        #region [点击页面切换动画]
        public void ChangePage1(int oldpage, int newpage)
        {
            ThicknessAnimation myAnimation = new ThicknessAnimation();
            myAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
            myAnimation.From = currentElement.Margin;

            if (newpage > oldpage)
            {
                tovalue = -screen_x + tovalue;
            }
            else if (newpage < oldpage)
            {
                tovalue = screen_x + tovalue;
                myAnimation.From = new Thickness(fromvalue, 0, 0, 0);
            }
            myAnimation.To = new Thickness(tovalue, 0, 0, 0);
            Storyboard.SetTarget(myAnimation, desk_sp);
            Storyboard.SetTargetProperty(myAnimation, new PropertyPath(MarginProperty));
            changepage_sb.Children.Add(myAnimation);
            changepage_sb.Begin();
        }

        void changepage_sb_Completed(object sender, EventArgs e)
        {
            this.page1.Visibility = Visibility.Visible;
            this.page2.Visibility = Visibility.Visible;
            this.page3.Visibility = Visibility.Visible;
            this.page4.Visibility = Visibility.Visible;
            this.page5.Visibility = Visibility.Visible;
            tovalue = -screen_x * page;
            SetMargin();
        }
        #endregion
    }
}
