using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Markup;
using System.IO;
using System.Xml;
using System.Windows.Media;
using PP = System.Windows.Shapes;
using System.Windows;
using MhczTBG.Common;

namespace MhczTBG.Controls.Book
{
    public class UcButton3 : Button
    {

        /// <summary>
        /// 按钮的颜色
        /// </summary>
        string tomColor;

        /// <summary>
        /// 按钮的颜色
        /// </summary>
        public string TomColor
        {
            get { return tomColor; }
            set
            {
                if (value != tomColor)
                {

                    tomColor = value;
                }

            }
        }

        /// <summary>
        /// 按钮的文本
        /// </summary>
        string tomText;

        /// <summary>
        /// 按钮的文本
        /// </summary>
        public string TomText
        {
            get { return tomText; }
            set
            {
                if (value != tomText)
                {

                    tomText = value;
                }
            }
        }

        /// <summary>
        /// 字体大小
        /// </summary>
        int tomTextFont;

        /// <summary>
        /// 字体大小
        /// </summary>
        public int TomTextFont
        {
            get { return tomTextFont; }
            set
            {
                if (value != tomTextFont)
                {

                    tomTextFont = value;
                }
            }
        }

        /// <summary>
        /// 字体颜色
        /// </summary>
        Brush tomTextForeground;

        /// <summary>
        /// 字体颜色
        /// </summary>
        public Brush TomTextForeground
        {
            get { return tomTextForeground; }
            set
            {
                if (value != tomTextForeground)
                {

                    tomTextForeground = value;
                }
            }
        }



        /// <summary>
        /// xmlns 命名空间
        /// </summary>
        public const string xmlns = "xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'";
        /// <summary>
        /// xmlns:sdk 命名空间
        /// </summary>
        public const string xmlnsSdk = "xmlns ='http://schemas.microsoft.com/winfx/2006/xaml/presentation/sdk'";

        /// <summary>
        /// 创建按钮的构造函数
        /// </summary>
        public UcButton3()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("<ControlTemplate " + xmlns + ">");
                builder.Append("<Grid " + xmlns + ">");
                builder.Append("<Grid.RowDefinitions>");
                builder.Append("<RowDefinition/>");
                builder.Append("<RowDefinition/>");
                builder.Append("</Grid.RowDefinitions>");

                builder.Append("<Path " + xmlns + " Grid.RowSpan='2' Data='M266.5,386.5 L275.5,384.75 280,383 288.25,379.5 292.75,376.75 297,373.75 301.75,369.25 307,363.5 310.25,359.75 312.5,356 315,351.5 316.75,347.25 318.5,342 320,334.75 321,329.25 321.33333,323.16667 320.83367,316.16629 320.50034,312.33258 320.167,307.83289 318.66699,303.66655 317.50032,299.83285 315.66697,296.33318 313.83363,293.16686 310.33361,286.99986&#xd;&#xa;307.33359,282.33317 303.00023,277.8335 299.50021,274.50051 296.16685,272.1672 292.66683,269.33354 288.83347,267.16723 284.33345,265.33357 281.50009,263.66726 278.50008,262.50027 274.83339,261.33361 270.00002,260.16729 266.16666,259.33363 263.49962,259.33363 260.66627,259.16729 257.66587,259.0003 252.83208,259.0003 249.83235,259.0003 247.66599,259.33364 244.49893,259.5003 240.99887,260.33364 238.83216,261.0003 234.49911,262.16696 230.99905,263.83363&#xd;&#xa;229.33236,264.91696 227.16601,266.25029 223.41563,268.41695 220.83192,270.50028 217.66555,272.91694 213.41514,276.5836 211.49809,278.41693 207.74802,282.08358 205.66464,284.58358 204.0601,287.00024 202.87207,289.12523 201.55953,291.87523 200.06,294.43772 199.06048,296.81271 198.12246,298.50021 197.49745,300.12521 196.37243,302.3752 195.74741,304.5002 194.74738,308.43769 194.24736,310.06268 193.74735,313.06268 193.05983,316.81267 192.81032,319.25016&#xd;&#xa; C192.81032,319.25016 192.68582,321.12516 192.68582,321.43766 192.68582,321.75015 192.74782,324.31265 192.74782,324.31265 192.74782,324.31265 192.81032,325.81265 192.81032,326.06264 192.81032,326.31264 193.18533,330.18763 193.18533,330.18763 193.18533,330.18763 193.56034,332.93763 193.62284,333.12513 193.68534,333.31263 194.18535,336.56262 194.18535,336.56262 L195.18537,339.62511 195.68538,341.25011 196.6229,344.2501 198.06043,347.75009 198.87294,349.25009 200.24797,351.87508 201.5605,354.50008 203.18553,357.25007 204.87306,359.37506 207.0606,362.43756 209.12314,364.68755 210.93568,366.68755 212.1857,368.18754 213.62323,369.87504 215.37327,371.43754 216.68579,372.62503 218.68583,374.06253 220.06086,375.00003 221.37338,375.93753 223.43592,377.25002 225.31096,378.50002 226.99849,379.56252 229.06103,380.62501&#xd;&#xa;231.93609,382.00001 233.87363,382.81251 236.56118,383.81251 238.24871,384.37501 240.56126,384.8125 243.06131,385.3125 244.68634,385.5625 246.49888,385.8125 249.31143,386.3125 251.93648,386.4375 253.62402,386.4375 256.31157,386.5625 257.8741,386.7495 259.12412,386.68701 260.81166,386.81201 C260.81166,386.81201 261.68667,386.81151 261.87418,386.81201 262.06168,386.81251 263.43671,386.75001 263.43671,386.75001 L265.13987,386.68751 266.35864,386.56251 266.42114,386.51564 264.37423,371.43755 261.99918,371.62505 259.74914,371.81255 257.06158,371.87505 254.93654,371.87505 251.87398,371.68755 249.24893,371.37505 246.24887,370.87505 242.93631,369.93755 239.81125,369.00005 236.18617,367.56256 234.12363,366.37506 231.74859,364.75006 229.49854,363.43757 227.87351,362.31257 225.31096,360.31257 222.99842,357.93758 220.68587,355.37508 218.31082,352.56259 216.24828,349.9376&#xd;&#xa;214.81076,348.1876 212.83155,344.50011 211.66486,341.83345 210.3315,338.16679 209.16481,334.50013 208.16479,330.66681 207.49811,326.25015 207.37311,322.12516 207.74812,316.75017 207.99812,313.75018 208.87314,309.37519 209.87316,306.8752 210.87318,304.5002 212.31071,302.06271 212.87322,300.62521 215.99828,295.12522 218.99834,291.25023 220.62337,289.12524 223.87343,285.87524 226.62349,283.87525 228.49852,282.25025 231.49858,280.25026 235.49866,278.25026&#xd;&#xa;239.49874,276.50026 242.24879,275.37527 245.74886,274.00027 249.24893,273.50027 252.49899,273.12527 255.12405,273.00027 257.7491,273.00027 260.62415,273.12527 262.49919,273.12527 265.49925,273.37527 267.74929,273.62527 269.87433,274.25027 272.37438,275.00027 274.62443,276.00027 276.62447,276.87526 280.06203,278.25026 282.31208,279.56276 284.12461,280.75026 285.93715,282.06275 287.24968,283.06275 288.87471,284.18775 289.62472,284.81275 290.87475,286.00024&#xd;&#xa;292.24977,287.43774 293.4998,288.93774 294.49982,289.93773 296.18735,292.12523 C296.18735,292.12523 297.24987,293.25023 297.37487,293.43773 297.49988,293.62523 298.24989,294.81272 298.24989,294.81272 L299.37491,296.68772 300.68744,298.75021 301.68746,301.25021 302.74998,303.6252 303.625,306.1252 304.37501,308.37519 305.43753,311.93768 306.06254,314.37518 C306.06254,314.37518 306.31255,317.12517 306.31255,317.31267 306.31255,317.50017 306.43755,319.62517 306.43755,320.00017 306.43755,320.37516 306.81256,323.62516 306.81256,323.62516 L306.81256,325.75015 306.68756,328.25015 306.18755,331.06264 305.81254,333.31263 305.31253,335.68763 304.75002,337.43763 303.625,340.56262 302.37497,343.37511 301.56246,345.50011 300.56244,347.3751 299.68742,348.7501 298.37489,350.3751 297.06237,352.31259 295.87484,354.00009 294.68732,355.18758 293.4998,356.68758 291.87477,358.18758 290.06223,359.93757 288.93721,360.93757 287.74969,361.81257 286.24966,362.62507 284.43712,363.87506 282.43708,365.06256&#xd;&#xa;280.68705,366.00006 278.3745,367.00006 276.93697,367.75006 275.43694,368.56255 273.1244,369.43755 269.93684,370.25005 267.81179,370.93755 266.43677,371.31255 265.32737,371.39067 264.79611,371.43755 264.49923,371.45317'  Stretch='Fill' >");
                builder.Append("<Path.Fill>");
                builder.Append("<LinearGradientBrush " + xmlns + " >");
                builder.Append("<GradientStop Color='Beige' Offset='0.3'/>");
                builder.Append("<GradientStop Color='black' Offset='1'/>");
                builder.Append("</LinearGradientBrush>");
                builder.Append("</Path.Fill>");
                builder.Append("<Path.Effect>");
                builder.Append("<DropShadowEffect " + xmlns + " Direction='320' BlurRadius='16' Color='Black'/>");
                builder.Append("</Path.Effect>");
                builder.Append("</Path>");

                builder.Append("<Ellipse Name ='elli' " + xmlns + " Fill='Purple' Grid.RowSpan='2' Margin='10'></Ellipse>");

                builder.Append("<Path Name ='pathLeft' " + xmlns + "  Margin='20,20,50,15' Fill='White' Grid.Row='1' Data='M321,373.33333 L324.66667,381.33368 327.33274,386.00088 328.33302,389.00101 328.33302,391.00109 323.00001,391.66779 318.33332,390.33409 314.33376,388.33404 311.00095,386.66733 308.33442,384.0006 306.33488,382.00055 z'    Stretch='Fill'>");
                builder.Append("<Path.Effect>");
                builder.Append("<BlurEffect " + xmlns + " Radius='11'/>");
                builder.Append("</Path.Effect>");
                builder.Append("</Path>");

                builder.Append("<Path Name ='pathRight' Margin='50,0,15,15' " + xmlns + " Fill='White' Grid.Row='1' Data='M328.66667,373 L330.99966,378.33333 333.99967,383.66735 337.66634,389.33388 340.33259,389.66758 342.99956,389.00055 347.33288,387.33383 352.66662,384.33372 356.99962,381.66696 361.99963,377.00014 365.99963,372.33331 368.99963,367.00023 371.99963,360.66682 373.33297,353.66629 373.33297,347.66574 366.99993,353.33232 362.9999,357.99919 355.99985,362.66606 347.33312,365.66619 339.6664,367.99963 333.66636,370.99976 z' HorizontalAlignment='Right'   Stretch='Fill' >");
                builder.Append("<Path.Effect>");
                builder.Append("<BlurEffect " + xmlns + " Radius='10'/>");
                builder.Append("</Path.Effect>");
                builder.Append("</Path>");

                builder.Append("<Ellipse Name = 'ellTop' " + xmlns + " Fill='white' Height='10'   HorizontalAlignment='Center' Width='25' Margin='0,0,20,0'>");
                builder.Append("<Ellipse.RenderTransform>");
                builder.Append("<RotateTransform Angle='-17.509'/>");
                builder.Append("</Ellipse.RenderTransform>");
                builder.Append("<Ellipse.Effect>");
                builder.Append("<BlurEffect " + xmlns + " Radius='10'/>");
                builder.Append("</Ellipse.Effect>");
                builder.Append("</Ellipse>");

                builder.Append("<TextBlock " + xmlns + " Name = 'txtBlock' Text='水晶按钮'  HorizontalAlignment='Center' VerticalAlignment='Center' Grid.RowSpan='2'>");
                builder.Append("</TextBlock>");
                builder.Append("</Grid>");
                builder.Append("</ControlTemplate>");
                //按钮的模板
                ControlTemplate template = (ControlTemplate)XamlReader.Load(UcBookShell.GetReader(builder.ToString()));
                //设置
                this.Template = template;

                //通过大小更改事件激发的时候，xaml方式后台加载的元素已经加载完毕，可以通过VisualTreeHelper来获取相应的元素了
                this.SizeChanged += new SizeChangedEventHandler(TomButton3_SizeChanged);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "UcButton3", ex.ToString());
            }
            finally
            {
            }

        }

        /// <summary>
        /// 调整大小，并将属性赋予按钮（先走构造函数，再走属性，然后走这个时间关联的方法）
        /// </summary>
        void TomButton3_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                //获取主要的圆
                PP.Ellipse elli = GetChildObject<PP.Ellipse>(this, "elli");

                //左边的反光体
                PP.Path pathLeft = GetChildObject<PP.Path>(this, "pathLeft");
                //上边的反光体
                PP.Ellipse ellTop = GetChildObject<PP.Ellipse>(this, "ellTop");
                //右边的反光体
                PP.Path pathRight = GetChildObject<PP.Path>(this, "pathRight");
                //按钮显示的文本
                TextBlock txtBlcok = GetChildObject<TextBlock>(this, "txtBlock");
                //承载体设置为最合适的尺寸
                elli.Margin = new Thickness(this.ActualWidth * 0.1, this.ActualHeight * 0.1, this.ActualWidth * 0.1, this.ActualHeight * 0.1);
                //反光体设置为最合适的尺寸
                ellTop.Margin = new Thickness(this.ActualWidth * 0.1, this.ActualHeight * 0.15, this.ActualWidth * 0.15, this.ActualHeight * 0.1);
                pathLeft.Margin = new Thickness(20, this.ActualHeight * 0.15, this.ActualWidth * 0.6, this.ActualHeight * 0.13);
                pathRight.Margin = new Thickness(this.ActualWidth * 0.5, 0, this.ActualWidth * 0.15, this.ActualHeight * 0.15);

                //属性不为空，更改默认的文本字体
                if (this.ActualWidth != 0)
                {
                    txtBlcok.FontSize = this.ActualWidth * 0.14;
                }

                //属性不为空，更改默认的承载体颜色
                if (tomColor != null)
                {
                    elli.Fill = GetSolicolorBrush(tomColor);
                }
                //属性不为空，更改默认的文本内容
                if (tomText != null)
                {
                    txtBlcok.Text = tomText;
                }
                //属性不为空，更改默认的字体大小
                if (tomTextFont != 0)
                {
                    txtBlcok.FontSize = tomTextFont;
                }
                //属性不为空，更改默认的字体颜色
                if (tomTextForeground != null)
                {
                    txtBlcok.Foreground = tomTextForeground;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TomButton3_SizeChanged", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 通过code名查找
        /// </summary>
        /// <typeparam name="T">未知类型</typeparam>
        /// <param name="obj">指定父类</param>
        /// <param name="name">指定code名（name）</param>
        /// <returns>返回所查询到的元素</returns>
        public T GetChildObject<T>(DependencyObject obj, string name) where T : FrameworkElement
        {
            //返回默认值
            DependencyObject child = null;
            //返回默认值
            T grandChild = null;

            //遍历验证获取指定实例
            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                //获取元素
                child = VisualTreeHelper.GetChild(obj, i);
                if (child != null)
                {
                    var e = child.GetValue(FrameworkElement.NameProperty);
                    if (e.ToString() == name)
                    {
                        return (T)child;
                    }
                }
                //验证
                if (child is T)
                {
                    var t = child as T;
                }
                //非指定类型，查找其子元素
                else
                {
                    //获取元素
                    grandChild = GetChildObject<T>(child, name);
                    //验证
                    if (grandChild != null)
                        return grandChild;
                }
            }
            return null;
        }

        //普通画刷
        #region SolidColorBrush
        /// <summary>
        /// 快速获取一个SolidColorBrush
        /// </summary>
        /// <param name="bbcdd">可以是（#bb9900）</param>
        /// <returns>返回实例</returns>
        public SolidColorBrush GetSolicolorBrush(string bbcdd)
        {
            SolidColorBrush s = null;
            try
            {
                s = (SolidColorBrush)XamlReader.Load(UcBookShell.GetReader("<SolidColorBrush xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' Color ='" + bbcdd + "'/>"));
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "GetSolicolorBrush", ex.ToString(), bbcdd);
            }
            finally
            {
            }
            return s;
        }
        #endregion
    }

}
