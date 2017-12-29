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

namespace MhczTBG.Controls.Modules
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Desk : Window
    {

        Border bordMain;
        /// <summary>
        /// 承载的容器
        /// </summary>
        public Border BordMain
        {
            get { return this.desk; }
            set
            {
                if (value != null)
                {
                    this.desk.Child = value;
                    bordMain = value;
                }
            }
        }
        //string str = "应用桌面";
        enum Appstate
        {
            window, desk
        }
        Appstate appstate = Appstate.desk;

        double window_width = SystemParameters.WorkArea.Width;

        public Desk()
        {

            InitializeComponent();

            #region 前后台元素核对

            //if (BordMain == null) BordMain = desk;
       

            #endregion

            this.MaxHeight = SystemParameters.WorkArea.Height;
            this.desk_x.X = window_width;
            this.window_btn.Click += new RoutedEventHandler(window_btn_Click);
            this.desk_btn.Click += new RoutedEventHandler(desk_btn_Click);

            main.Visibility = Visibility.Collapsed;           
            //this.deskcontrol.window.MouseLeftButtonDown += new MouseButtonEventHandler(window_MouseLeftButtonDown);     //切换到window
            ShowDesk();
            this.main.Visibility = Visibility.Visible;
        }

        void window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (appstate != Appstate.window)
            {
                ShowWindow();
                appstate = Appstate.window;

                this.window_btn.IsChecked = true;
                this.desk_btn.IsChecked = false;
                this.window_btn.SetValue(Panel.ZIndexProperty, 1);
                this.desk_btn.SetValue(Panel.ZIndexProperty, 0);
            }
        }

        void close_btn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        void cancel_btn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        void login_btn_Click(object sender, RoutedEventArgs e)
        {
            //this.main.Visibility = Visibility.Visible;
            //this.login.Visibility = Visibility.Collapsed;
            ShowDesk();
        }

        void desk_btn_Click(object sender, RoutedEventArgs e)
        {
            if (appstate != Appstate.desk)
            {
                ShowDesk();
                appstate = Appstate.desk;

                this.window_btn.SetValue(Panel.ZIndexProperty, 0);
                this.desk_btn.SetValue(Panel.ZIndexProperty, 1);
            }
        }

        void window_btn_Click(object sender, RoutedEventArgs e)
        {
            if (appstate != Appstate.window)
            {
                ShowWindow();
                appstate = Appstate.window;

                this.window_btn.SetValue(Panel.ZIndexProperty, 1);
                this.desk_btn.SetValue(Panel.ZIndexProperty, 0);
            }
        }

        public void ShowWindow()    //显示电脑桌面
        {
            Move_StoryBoard(window_width);
        }

        public void ShowDesk()      //显示统一桌面
        {
            Move_StoryBoard(0);
        }
        public void Move_StoryBoard(double width)
        {
            Storyboard sb = new Storyboard();
            DoubleAnimationUsingKeyFrames da = new DoubleAnimationUsingKeyFrames();
            EasingDoubleKeyFrame ed1 = new EasingDoubleKeyFrame();
            ed1.EasingFunction = new CircleEase() { EasingMode = EasingMode.EaseOut };
            da.KeyFrames.Add(ed1);
            da.KeyFrames.Add(new LinearDoubleKeyFrame(width, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.3))));
            Storyboard.SetTarget(da, desk);
            Storyboard.SetTargetProperty(da, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
            sb.Children.Add(da);
            sb.Begin();
        }
    }
}
