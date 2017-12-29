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
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Markup;
using System.Xml;
using System.IO;
using MhczTBG.Common;

namespace MhczTBG.Controls.CustomButton
{
    /// <summary>
    /// 自定义图标按钮类
    /// </summary>
    public class MyButton : Button
    {
        #region 变量

        private String tittle;
        /// <summary>
        /// 按钮的标题
        /// </summary>
        public String Tittle
        {
            get { return tittle; }
            set { tittle = value; }
        }

        #endregion

        #region 构造函数

        /// <param name="uri1">触发前的图片</param>
        /// <param name="uri2">触发后的图片</param>
        /// <param name="height">高</param>
        /// <param name="text">文本内容</param>
        /// <param name="fontSize">字体大小</param>
        public MyButton()
        {
            try
            {
                //鼠标形态
                this.Cursor = Cursors.Hand;
                //字符串优化类
                StringBuilder builder = new StringBuilder();
                //序列化
                builder.Append("<ControlTemplate TargetType='{x:Type Button}' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>");

                builder.Append("<Grid >");

                builder.Append("<Grid.RowDefinitions>");
                builder.Append("<RowDefinition />");
                builder.Append("<RowDefinition Height='25'/>");
                builder.Append("</Grid.RowDefinitions>");

                builder.Append("<Border BorderThickness = '1' BorderBrush='silver' Background ='{TemplateBinding Background}' />");

                builder.Append("<TextBlock FontSize ='{TemplateBinding FontSize}'  Grid.Row='1' HorizontalAlignment='Center' Foreground='{TemplateBinding Foreground}' VerticalAlignment='Center' Text='{TemplateBinding Content}'/>");

                builder.Append("</Grid>");

                builder.Append("</ControlTemplate>");
                //通过序列化创建模板
                ControlTemplate template = (ControlTemplate)XamlReader.Load(GetReader(builder.ToString()));
                //给button设置模板
                this.Template = template;
                //绑定事件
                this.MouseEnter += new MouseEventHandler(MyButton_MouseEnter);
                this.MouseLeave += new MouseEventHandler(MyButton_MouseLeave);
                //为换图片做准备,供关联的事件所使用
                //uriArray = new string[] { uri1, uri2 };
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "MyButton", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion

        #region UI事件区域

        /// <summary>
        /// 鼠标离开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MyButton_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                //可视树获取子元素
                var con = VisualTreeHelper.GetChild(sender as Button, 0);
                //获取子元素包含的子子元素的数量
                var child = VisualTreeHelper.GetChild(con, 0);
                if (child is Ellipse)
                {
                    Ellipse elli = child as Ellipse;
                    //换图
                    //elli.Fill = new ImageBrush() { ImageSource = new BitmapImage(new Uri(uriArray[0], UriKind.RelativeOrAbsolute)) };
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "MyButton_MouseLeave", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 鼠标进入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MyButton_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                //可视树获取子元素
                var con = VisualTreeHelper.GetChild(sender as Button, 0);
                //获取子元素包含的子子元素的数量
                var child = VisualTreeHelper.GetChild(con, 0);
                if (child is Ellipse)
                {
                    Ellipse elli = child as Ellipse;
                    //换图
                    //elli.Fill = new ImageBrush() { ImageSource = new BitmapImage(new Uri(uriArray[1], UriKind.RelativeOrAbsolute)) };
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "MyButton_MouseEnter", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }

        #endregion

        #region 辅助方法
        /// <summary>
        /// 读取xml
        /// </summary>
        /// <param name="xml">xml字符串</param>
        /// <returns>xmlreader</returns>
        public static XmlReader GetReader(string xml)
        {
            XmlTextReader xmlreader = null;
            try
            {
                StringReader strreader = new StringReader(xml);
                xmlreader = new XmlTextReader(strreader);

            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(typeof(MyButton).ToString(), "GetReader", ex.ToString(), xml);
            }
            finally
            {
            }
            return xmlreader;
        }

        #endregion
    }
}
