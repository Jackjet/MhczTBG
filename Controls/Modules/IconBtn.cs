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
using System.Windows.Media.Imaging;

namespace MhczTBG.Controls.Modules
{
     class IconBtn : RadioButton
    {
        /// <summary>
        /// 按钮图片
        /// </summary>
        public BitmapImage BtnImage
        {
            get { return (BitmapImage)GetValue(BtnImageProperty); }
            set { SetValue(BtnImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BtnImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BtnImageProperty =
            DependencyProperty.Register("BtnImage", typeof(BitmapImage), typeof(IconBtn), null);

        //public static DependencyProperty BtnImageProperty;//依赖属性

        //public BitmapImage BtnImage
        //{
        //    get { return (BitmapImage)GetValue(BtnImageProperty); }
        //    set { SetValue(BtnImageProperty, value); }
        //}

        //static IconBtn()
        //{
        //    BtnImageProperty = DependencyProperty.Register(
        //        "BtnImage", typeof(BitmapImage), typeof(IconBtn),
        //        new FrameworkPropertyMetadata(""));
        //}


    }
}
