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
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using Microsoft.SharePoint.Client;
using MhczTBG.Common;


namespace MhczTBG.Controls.Book
{
    public class UcBook : Grid
    {
        #region 变量

        string title;
        /// <summary>
        /// 书本的标题
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        string date;
        /// <summary>
        /// 书本的发布日期
        /// </summary>
        public string Date
        {
            get { return date; }
            set { date = value; }
        }

        bool visbl;
        /// <summary>
        /// 显示状态
        /// </summary>
        public bool Visbl
        {
            get { return visbl; }
            set
            {
                if (value)
                {
                    path2.Opacity = 0.5;
                }
                else
                {
                    path2.Opacity = 0.9;
                }
                visbl = value;
            }
        }

        Folder _folder;
        /// <summary>
        /// 该容器所包含的文档库
        /// </summary>
        public Folder Folder
        {
            get { return _folder; }
            set { _folder = value; }
        }

        Microsoft.SharePoint.Client.File _file;
        /// <summary>
        /// 该容器所包含的文档
        /// </summary>
        public Microsoft.SharePoint.Client.File File
        {
            get { return _file; }
            set { _file = value; }
        }


        Microsoft.SharePoint.Client.List _list;
        /// <summary>
        /// 该容器所包含的列表
        /// </summary>
        public Microsoft.SharePoint.Client.List List
        {
            get { return _list; }
            set { _list = value; }
        }


        /// <summary>
        /// 基本命名空间
        /// </summary>
        const string xmlns = "xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'";
        /// <summary>
        /// 路径
        /// </summary>
        Path path2 = null;
        /// <summary>
        /// StackPanel面板
        /// </summary>
        StackPanel panelTitle = null;

        #endregion

        #region 构造函数

        /// <summary>
        /// 书本的构造函数(文件夹构造者)
        /// </summary>
        /// <param name="title">书本的标题</param>     
        /// <param name="imageUri">书的背景图</param>
        public UcBook(Folder forder, string imageName)
            : this()
        {
            try
            {
                //生成对象时添加属性值
                this.Title = forder.Name;
                this.Folder = forder;

                BitmapImage imageSource = CommonMethod.GetImageSource(imageName);
                this.TomBook_Loaded(title, imageSource);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "UcBook", ex.ToString(), forder, imageName);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 书本的构造函数(文件构造者)
        /// </summary>
        /// <param name="title">书本的标题</param>             
        /// <param name="imageUri">书的背景图</param>
        public UcBook(Microsoft.SharePoint.Client.File filer, string imageName)
            : this()
        {
            try
            {
                //生成对象时添加属性值
                this.Title = filer.Name;
                //this.ImageUri = imageUri;
                this.File = filer;

                BitmapImage imageSource = CommonMethod.GetImageSource(imageName);
                this.TomBook_Loaded(title, imageSource);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "UcBook", ex.ToString(), filer, imageName);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 书本的构造函数(列表构造者)
        /// </summary>
        /// <param name="title">书本的标题</param>             
        /// <param name="imageUri">书的背景图</param>
        public UcBook(Microsoft.SharePoint.Client.List list, string imageName)
            : this()
        {
            try
            {
                //生成对象时添加属性值
                this.Title = list.Title;
                //this.ImageUri = imageUri;
                this.List = list;
                BitmapImage imageSource = CommonMethod.GetImageSource(imageName);
                this.TomBook_Loaded(title, imageSource);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "UcBook", ex.ToString(), list, imageName);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 书本的构造函数(普通元素构造者)
        /// </summary>
        /// <param name="title">书本的标题</param>             
        /// <param name="imageUri">书的背景图</param>
        public UcBook(string title, string imageName)
            : this()
        {
            try
            {
                //生成对象时添加属性值
                this.Title = title;
                //this.ImageUri = imageUri;

                BitmapImage imageSource = new BitmapImage(new Uri(Environment.CurrentDirectory.Replace("bin\\Debug", "Image\\") + imageName, UriKind.RelativeOrAbsolute));
                this.TomBook_Loaded(title, imageSource);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "UcBook", ex.ToString(), title, imageName);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 书本的构造函数(普通元素构造者【供内部使用】)
        /// </summary>
        /// <param name="title">书本的标题</param>             
        /// <param name="imageUri">书的背景图</param>
        public UcBook(string title, ImageSource imageSource)
            : this()
        {
            try
            {
                //生成对象时添加属性值
                this.Title = title;
                this.TomBook_Loaded(title, imageSource);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "UcBook", ex.ToString(), title, imageSource);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 书本的构造函数（无参数）
        /// </summary>
        public UcBook()
        {
            try
            {
                //书本在书架上的位置以及宽度与高度
                this.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
                this.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                this.Cursor = Cursors.Hand;

                this.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(80) });
                this.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(60) });
                this.RowDefinitions.Add(new RowDefinition());
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "UcBook", ex.ToString());
            }
            finally
            {
            }
        }


        #endregion

        #region UI事件区域

        /// <summary>
        /// 加载书本时发生的事件（比如说赋予给父容器）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="title">书本的标题</param>
        /// <param name="data">书本的发布日期</param>
        /// <param name="note">书本的备注</param>
        /// <param name="imageUri">书本的背景图</param>
        void TomBook_Loaded(string title, ImageSource imageSource)
        {
            try
            {
                this.Width = 100;
                this.Height = 150;
                this.Margin = new Thickness(0, 0, 0, -8);
                //白色画刷
                SolidColorBrush FontBrush = new SolidColorBrush(Colors.White);
                //居中布局
                HorizontalAlignment horCenter = System.Windows.HorizontalAlignment.Center;

                this.Background = new ImageBrush() { ImageSource = imageSource };

                //束带
                path2 = GetPath("M98,176 L96,178.66667 L95.333336,183.33243 L95.333336,236.66194 L389.29663,235.995 L390.6301,178.66644 L387.96338,175.33339 L384.63046,176.66693 L379.96439,178.00015 L102.66608,179.33337 z");
                Grid.SetRow(path2, 1);
                this.Children.Add(path2);

                //书本名称与发布日期组合
                panelTitle = new StackPanel() { Visibility = System.Windows.Visibility.Visible };
                TextBlock txtTitle = new TextBlock() { TextWrapping = System.Windows.TextWrapping.Wrap, HorizontalAlignment = horCenter, Text = title, Margin = new Thickness(0, 3, 0, 0), Foreground = FontBrush, FontSize = 14, };
                panelTitle.Children.Add(txtTitle);
                if (Folder != null)
                {
                    TextBlock txtDate = new TextBlock() { HorizontalAlignment = horCenter, Text = Folder.ItemCount.ToString(), Margin = new Thickness(0, 3, 0, 0), Foreground = FontBrush, FontSize = 14, };
                    panelTitle.Children.Add(txtDate);
                }

                Grid.SetRow(panelTitle, 1);
                this.Children.Add(panelTitle);

                //当鼠标进入设置为显示（文本，束带）
                this.MouseEnter += (object sender1, MouseEventArgs e1) =>
                    {
                        Visbl = true;
                    };
                //当鼠标进入设置为隐藏（文本，束带）
                this.MouseLeave += (object sender1, MouseEventArgs e1) =>
                {
                    Visbl = false;
                };
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TomBook_Loaded", ex.ToString(), title, imageSource);
            }
            finally
            {
            }
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 快速获取一个Path类实例
        /// </summary>
        /// <param name="pathData">指定Data属性</param>
        Path GetPath(string pathData)
        {
            Path path = null;
            try
            {
                string path1 = @"<Path Fill ='Red' Margin='-1.5,0,-2,0' Stretch='Fill' Opacity='0.9' " + xmlns + "  Data = '" + pathData + "'  />";
                path = (Path)XamlReader.Load(UcBookShell.GetReader(path1));
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "GetPath", ex.ToString(), pathData);
            }
            finally
            {
            }
            return path;
        }

        #endregion
    }
}
