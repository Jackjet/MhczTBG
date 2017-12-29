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
using System.Windows.Media.Imaging;
using System.Windows.Markup;
using System.Xml;
using System.IO;
using System.Windows.Media.Effects;
using MhczTBG.Helper;
using MhczTBG.Common;

namespace MhczTBG.Controls.Book
{

    public class UcBookShell : Grid
    {
        #region 变量

        #region 搜索背景
      
        string daoHangImageUri = string.Empty;
        /// <summary>
        /// 导航框架的背景
        /// </summary>
        public string DaoHangImageUri
        {
            get { return daoHangImageUri; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    //设置导航框架的背景
                    if (grid1 != null)
                    {
                        grid1.Background = GetImageBrush(value);
                    }
                    daoHangImageUri = value;
                }
                else
                {
                    //设置导航框架的背景
                    if (grid1 != null)
                    {
                        grid1.Background = CommonMethod.GetImagebrush("DaoHang");
                    }
                }
            }
        }

        string searchBtnImageUri = string.Empty;
        /// <summary>
        /// 查找按钮的背景
        /// </summary>
        public string SearchBtnImageUri
        {
            get { return searchBtnImageUri; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    if (btnCom != null)
                    {
                        //设置查找按钮的背景
                        btnCom.Background = GetImageBrush(value);
                    }
                    searchBtnImageUri = value;
                }
                else
                {
                    if (btnCom != null)
                    {
                        //设置查找按钮的背景
                        btnCom.Background = CommonMethod.GetImagebrush("SearchBtn");
                    }
                }
            }
        }


        string otherImageUri = string.Empty;
        /// <summary>
        /// 其它控件的背景（搜索框、翻页按钮、显示当前页文本）
        /// </summary>
        public string OtherImageUri
        {
            get { return otherImageUri; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    if (txtcom != null && pathLeft != null && pathRight != null && txtBoxSelect != null)
                    {
                        // 其它控件的背景（导航）
                        txtcom.Background = pathLeft.Fill = pathRight.Fill = txtBoxSelect.Background = GetImageBrush(value);
                    }
                    otherImageUri = value;
                }
                else
                {
                    txtcom.Background = pathLeft.Fill = pathRight.Fill = txtBoxSelect.Background = CommonMethod.GetImagebrush("Other");
                }
            }
        }


        string bookShellImageUri = string.Empty;
        /// <summary>
        /// 书架的背景
        /// </summary>
        public string BookShellImageUri
        {
            get { return bookShellImageUri; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    bookShellImageUri = value;
                }
            }
        }


        string singleShellImageUri;
        /// <summary>
        /// 书架单行的背景（就是高度不够，显示一行时的背景）
        /// </summary>
        public string SingleShellImageUri
        {
            get { return singleShellImageUri; }
            set
            {
                if (value != singleShellImageUri)
                {
                    singleShellImageUri = value;
                }
            }
        }

        #endregion

        #region 数据变量

        /// <summary>
        /// 临时打开的书本集
        /// </summary>
        public List<UcBook> whoTheList;

        /// <summary>
        /// 书本集合
        /// </summary>
        public List<UcBook> bookList = new List<UcBook>();

        /// <summary>
        /// 行的数量
        /// </summary>
        int rowCount;

        /// <summary>
        /// 书架行的数量
        /// </summary>
        public int RowCount
        {
            get { return rowCount; }
            set
            {
                if (value != rowCount)
                {
                    rowCount = value;
                    //如果行的数量发生改变，那就重新调整书架
                    this.ReFlush(GetListStart(Convert.ToInt32(txtBoxSelect.Text)), whoTheList);
                }
            }
        }

        /// <summary>
        /// 列的数量
        /// </summary>
        int columnCount;

        /// <summary>
        /// 书架列的数量
        /// </summary>
        public int ColumnCount
        {
            get { return columnCount; }

            set
            {
                //列的数量不等于0
                if (value != columnCount && value != 0)
                {
                    //如果列的数量发生改变，那就重新调整书架
                    this.ReFlush(GetListStart(Convert.ToInt32(txtBoxSelect.Text)), whoTheList);
                    columnCount = value;
                }
                //如果书架的grid列数量与现在的列数量不一致，则调整
                if (gridMain.ColumnDefinitions.Count != value)
                {
                    //通过现在的页面来调整
                    this.ReFlush(GetListStart(Convert.ToInt32(txtBoxSelect.Text)), whoTheList);
                    columnCount = value;
                }
            }
        }

        /// <summary>
        /// 父容器所能盛放书本的数量
        /// </summary>
        int pageBookCount;

        /// <summary>
        /// 当前状态所能承载的书的数量
        /// </summary>
        public int PageBookCount
        {
            get { return pageBookCount; }
            set
            {
                if (value != pageBookCount)
                {
                    pageBookCount = value;
                    if (PageBookCount > 0)
                    {
                        //如果当前页的数量大于页的总数量，那就将当前页设置为最后一页
                        this.PageCount = (int)Math.Ceiling(this.bookList.Count / Convert.ToDouble(PageBookCount));
                    }
                    else
                    {
                        //如果当前页的数量大于页的总数量，那就将当前页设置为最后一页
                        this.PageCount = (int)Math.Ceiling(this.bookList.Count / Convert.ToDouble(1));
                    }

                }
            }
        }

        int pageCount;
        /// <summary>
        /// 页的数量
        /// </summary>
        public int PageCount
        {
            get { return pageCount; }
            set
            {
                if (value != pageCount && value > 0)
                {
                    //设置总共多少页
                    this.txtBlcokNum.Text = value.ToString();
                    //先赋值后刷新，因为刷新的方法里已经有了对页数量的更改，避免走入死循环
                    pageCount = value;
                    //重新调整
                    this.ReFlush(this.GetListStart(this.pageNow), whoTheList);
                }
            }
        }


        /// <summary>
        /// 当前浏览的页
        /// </summary>
        int pageNow = 1;

        /// <summary>
        /// 当前页
        /// </summary>
        public int PageNow
        {
            get { return pageNow; }
            set
            {
                if (value != pageNow)
                {
                    if (value == 0)
                    {
                        this.txtBoxSelect.Text = "1";
                    }
                    else
                    {
                        //当前页更改时，将重新更新当前页显示文本
                        this.txtBoxSelect.Text = value.ToString();
                    }
                    //重新调整
                    this.ReFlush(GetListStart(value), whoTheList);
                    pageNow = value;
                }
            }
        }

        //基本命名空间
        /// <summary>
        /// 基本命名空间
        /// </summary>
        string xmlns = "xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'";


        #endregion

        #region 控件变量

        /// <summary>
        /// 总页数显示（控件）
        /// </summary>
        TextBlock txtBlcokNum = null;

        /// <summary>
        /// 获取当前页（控件）
        /// </summary>
        TextBox txtBoxSelect = null;

        /// <summary>
        /// 查询（控件）
        /// </summary>
        TextBox txtcom = null;

        /// <summary>
        /// 导航（控件）
        /// </summary>
        Grid grid1 = null;

        /// <summary>
        /// 左箭头（控件）
        /// </summary>
        System.Windows.Shapes.Path pathLeft = null;

        /// <summary>
        /// 右箭头（控件）
        /// </summary>
        System.Windows.Shapes.Path pathRight = null;

        /// <summary>
        /// 书架
        /// </summary>
        public Grid gridMain = null;
        /// <summary>
        /// 查询按钮
        /// </summary>
        Button btnCom = null;

        #endregion

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造书架和导航的构造函数
        /// </summary>
        public UcBookShell()
        {
            try
            {
                //生成导航
                this.DaoHangInit();
                //生成书架
                this.BookShellInit();

                this.DaoHangImageUri = string.Empty;
                this.SearchBtnImageUri = string.Empty;
                this.OtherImageUri = string.Empty;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "UcBookShell", ex.ToString());
            }
            finally
            {
            }
        }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="strBookShellUri">书架的背景</param>
        /// <param name="strSingleShellUri">单独一行书架的背景</param>
        /// <param name="strDaoHangUri">导航框架背景</param>
        /// <param name="strSearchBtnUri">搜索按钮背景</param>
        /// <param name="strOtherUri">其它控件的背景（搜索框、翻页按钮、显示当前页文本）</param>       
        public UcBookShell(string strBookShellUri, string strSingleShellUri, string strDaoHangUri, string strSearchBtnUri, string strOtherUri)
        {
            try
            {
                BackgroundSetting(strBookShellUri, strSingleShellUri, strDaoHangUri, strSearchBtnUri, strOtherUri);
                //生成导航
                this.DaoHangInit();
                //生成书架
                this.BookShellInit();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "OKButton_Click", ex.ToString(), strBookShellUri, strSingleShellUri, strDaoHangUri, strSearchBtnUri, strOtherUri);
            }
            finally
            {
            }
        }

        #endregion

        #region 书架初始化

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="strBookShellUri">书架的背景</param>
        /// <param name="strSingleShellUri">单独一行书架的背景</param>
        /// <param name="strDaoHangUri">导航框架背景</param>
        /// <param name="strSearchBtnUri">搜索按钮背景</param>
        /// <param name="strOtherUri">其它控件的背景（搜索框、翻页按钮、显示当前页文本）</param>       
        public void BackgroundSetting(string strBookShellUri, string strSingleShellUri, string strDaoHangUri, string strSearchBtnUri, string strOtherUri)
        {
            try
            {
                this.BookShellImageUri = strBookShellUri;
                this.SingleShellImageUri = strSingleShellUri;
                this.DaoHangImageUri = strDaoHangUri;
                this.SearchBtnImageUri = strSearchBtnUri;
                this.OtherImageUri = strOtherUri;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "BackgroundSetting", ex.ToString(), strBookShellUri, strSingleShellUri, strDaoHangUri, strSearchBtnUri, strOtherUri);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 导航初始化
        /// </summary>        
        void DaoHangInit()
        {
            try
            {
                //临时的书本集合
                whoTheList = bookList;
                //设置为两行，一行为导航，第二行为书架
                this.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(40) });
                this.RowDefinitions.Add(new RowDefinition());

                #region 资源

                //水平
                Orientation orientationHor = Orientation.Horizontal;
                //米色画刷
                SolidColorBrush solidColorBrushBeige = GetSolicolorBrush("Beige");
                //透明
                SolidColorBrush solidColorBrushTransparent = GetSolicolorBrush("Transparent");
                //黑色画刷
                SolidColorBrush solidColorBrushBlack = GetSolicolorBrush("Black");
                //手状
                Cursor cursorHand = Cursors.Hand;


                #endregion
                grid1 = new Grid();
                if (!string.IsNullOrEmpty(DaoHangImageUri))
                {
                    //主要承载体
                    grid1.Background = GetImageBrush(DaoHangImageUri);
                }

                //导航分为两列，第一列放还原按钮，第二列放搜索
                grid1.ColumnDefinitions.Add(new ColumnDefinition());
                grid1.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(300) });

                //搜索的承载体
                StackPanel panel = new StackPanel()
                {
                    Orientation = orientationHor,
                    Margin = new Thickness(0, 5, 0, 5)
                };

                #region 搜索
                Label lblCOm = new Label() { Width = 150 };

                StackPanel panelCom = new StackPanel()
                {
                    Orientation = orientationHor,

                };
                if (!string.IsNullOrEmpty(OtherImageUri))
                {
                    panelCom.Background = GetImageBrush(OtherImageUri);
                }

                //搜索输入文本
                txtcom = new TextBox()
                {
                    Foreground = solidColorBrushBeige,
                    BorderBrush = solidColorBrushTransparent,
                    BorderThickness = new Thickness(0),
                    Width = 80,
                    Background = solidColorBrushTransparent,
                    FontSize = 12,
                    //SelectionBackground = GetSolicolorBrush("Blue"),
                };
                //搜索按钮
                btnCom = new Button() { Width = 20, Cursor = Cursors.Hand };
                //搜索按钮的图片
                if (!string.IsNullOrEmpty(SearchBtnImageUri))
                {
                    btnCom.Background = GetImageBrush(SearchBtnImageUri);
                }

                //btnCom.Content = btnImage; //设置查找按钮的背景


                //搜索按钮的点击事件
                btnCom.Click += (object sender, RoutedEventArgs e) =>
                {
                    //获取搜索文本的内容
                    string name = txtcom.Text.Trim();
                    //创建一个临时储存符合关键字的书本集合
                    List<UcBook> bookListLinShi = new List<UcBook>();
                    //通过关键字来给临时书本集合添加符合的书本
                    foreach (var item in bookList)
                    {
                        //通过标题来判断
                        if (item.Title.Contains(name))
                        {
                            bookListLinShi.Add(item);
                        }
                    }

                    //通过临时的书本集合的数量来获取书本的总页数
                    this.txtBlcokNum.Text = Math.Ceiling(bookListLinShi.Count / Convert.ToDouble(pageBookCount)).ToString();

                    if (PageCount < 0)
                    {
                        //如果小于0，设置为1
                        PageCount = 1;
                    }
                    if (PageCount == 0)
                    {
                        //如果等于1，则设置为1
                        PageCount = 1;
                        PageNow = 1;
                    }
                    //给全局的临时书本集合
                    whoTheList = bookListLinShi;
                    //刷新
                    this.ReFlush(0, bookListLinShi);
                };

                //组装搜索控件
                panelCom.Children.Add(txtcom);
                panelCom.Children.Add(btnCom);

                lblCOm.Content = panelCom;

                #endregion

                //添加搜索控件
                panel.Children.Add(lblCOm);

                #region 分页导航


                //左箭头的承载体，框
                Border borderLeft = new Border()
                {
                    //通过属性来给其设置
                    BorderBrush = solidColorBrushBlack,
                    Cursor = cursorHand,
                    BorderThickness = new Thickness(1),
                    Margin = new Thickness(15, 5, 15, 5),
                    Padding = new Thickness(5),
                    Background = solidColorBrushTransparent,
                };

                //左箭头
                pathLeft = GetPath("M264.75,310.75 L243.5,336 L264.75,360.25 z");
                //通过全局属性来设置背景
                if (!string.IsNullOrEmpty(OtherImageUri))
                {
                    pathLeft.Fill = GetImageBrush(OtherImageUri);
                }
                borderLeft.Child = pathLeft;

                //当进入左箭头区域时变白
                borderLeft.MouseEnter += (object sender, MouseEventArgs e) =>
                {
                    pathLeft.Fill = GetSolicolorBrush("white");
                };
                //当进入左箭头区域时还原
                borderLeft.MouseLeave += (object sender, MouseEventArgs e) =>
                {
                    if (!string.IsNullOrEmpty(OtherImageUri))
                    {

                        pathLeft.Fill = GetImageBrush(OtherImageUri);
                    }
                };

                //左箭头点击事件
                borderLeft.MouseLeftButtonDown += (object sender, MouseButtonEventArgs e) =>
                {
                    //前提是当前页不是第一页
                    if (this.pageNow > 1)
                    {
                        //当前页自减
                        this.PageNow--;
                        //设置当前页显示文本
                        txtBoxSelect.Text = this.PageNow.ToString();
                    }
                };

                //当前页显示文本
                txtBoxSelect = new TextBox()
                {
                    Foreground = solidColorBrushBeige,
                    BorderBrush = solidColorBrushTransparent,
                    BorderThickness = new Thickness(0),
                    Width = 20,
                    Margin = new Thickness(5, 5, 2, 5),
                    Text = this.pageNow.ToString(),

                };
                //总页数的承载体
                txtBlcokNum = new TextBlock()
                {
                    VerticalAlignment = System.Windows.VerticalAlignment.Center,
                    Text = pageNow.ToString(),
                    Margin = new Thickness(0, 5, 5, 5),

                };

                //右箭头的承载体
                Border borderRight = new Border()
                {
                    BorderBrush = solidColorBrushBlack,
                    Cursor = cursorHand,
                    BorderThickness = new Thickness(1),
                    Margin = new Thickness(15, 5, 15, 5),
                    Padding = new Thickness(5),
                    Background = solidColorBrushTransparent,
                };

                //右箭头的点击事件
                borderRight.MouseLeftButtonDown += (object sender, MouseButtonEventArgs e) =>
                {
                    //前提是当前页不是最后一页
                    if (this.pageNow < Convert.ToInt32(this.txtBlcokNum.Text))
                    {
                        //当前页自增
                        this.PageNow++;

                        //设置当前页的显示文本
                        txtBoxSelect.Text = this.PageNow.ToString();
                    }
                };



                //右箭头的图标
                pathRight = GetPath("M265.5,311 L265,361 L287,335.75 z");
                //右箭头的背景
                if (!string.IsNullOrEmpty(OtherImageUri))
                {
                    pathRight.Fill = GetImageBrush(OtherImageUri);
                }
                //右箭头组合
                borderRight.Child = pathRight;
                //当进入右箭头区域时变白
                borderRight.MouseEnter += (object sender, MouseEventArgs e) =>
                {
                    pathRight.Fill = GetSolicolorBrush("white");
                };
                //当进入右箭头区域时还原
                borderRight.MouseLeave += (object sender, MouseEventArgs e) =>
                {
                    if (!string.IsNullOrEmpty(OtherImageUri))
                    {
                        pathRight.Fill = GetImageBrush(OtherImageUri);
                    }
                };

                #endregion

                //添加左箭头，当前页文本，页的总数量，和右箭头
                panel.Children.Add(borderLeft);
                panel.Children.Add(txtBoxSelect);
                panel.Children.Add(txtBlcokNum);
                panel.Children.Add(borderRight);

                Grid.SetColumn(panel, 1);
                grid1.Children.Add(panel);

                //添加导航
                this.Children.Add(grid1);

                if (!string.IsNullOrEmpty(OtherImageUri))
                {
                    txtcom.Background = pathLeft.Fill = pathRight.Fill = txtBoxSelect.Background = GetImageBrush(OtherImageUri);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DaoHangInit", ex.ToString());
            }
            finally
            {
            }
        }

        /// <summary>
        /// 生成书架
        /// </summary>
        void BookShellInit()
        {
            try
            {
                //书架
                gridMain = new Grid();
                //书架背景
                if (!string.IsNullOrEmpty(BookShellImageUri))
                {
                    gridMain.Background = GetImageBrush(BookShellImageUri);
                }
                else
                {
                    gridMain.Background = CommonMethod.GetImagebrush("BookShell");
                }
                //用于调整rowCount和columnCount
                gridMain.SizeChanged += new SizeChangedEventHandler(TomBookShell_SizeChanged);

                //设置书架的行数
                this.RowCount = gridMain.RowDefinitions.Count;
                Grid.SetRow(gridMain, 1);
                //添加书架
                this.Children.Add(gridMain);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "BookShellInit", ex.ToString());
            }
            finally
            {
            }
        }

        #endregion

        #region UI事件区域

        /// <summary>
        /// 书架尺寸发生更改时的事件，调整行与列
        /// </summary>
        void TomBookShell_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                //获取当前窗体的宽度
                double width = (sender as FrameworkElement).ActualWidth;
                //获取当前窗体的高度
                double height = (sender as FrameworkElement).ActualHeight;
                //通过窗体高度来设置书架的行数 
                this.RowCount = this.getHeightNum(Convert.ToInt32(height));
                //通过窗体的宽度来设置书架的列数
                this.ColumnCount = this.GetWidthNum(Convert.ToInt32(width));

                if (pageNow > PageCount)
                {
                    PageNow = PageCount;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "TomBookShell_SizeChanged", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        #endregion

        #region 添加子项

        /// <summary>
        /// 添加一本书
        /// </summary>
        /// <param name="book">书本的实例</param>
        public void Items_Add(UcBook ucBook)
        {
            try
            {
                //给书本集合添加书本
                this.bookList.Add(ucBook);
                //书本阴影
                ucBook.Effect = new DropShadowEffect() { Direction = 320, BlurRadius = 16 };
                //刷新
                this.ReFlush();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Items_Add", ex.ToString(), ucBook);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 添加多本书籍
        /// </summary>
        /// <param name="ucBookList">书本集合</param>
        public void Items_Add(List<UcBook> ucBookList)
        {
            try
            {
                foreach (var ucBook in ucBookList)
                {
                    Items_Add(ucBook);
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Items_Add", ex.ToString(), ucBookList);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 通过索引删除某子项
        /// </summary>
        /// <param name="index">索引</param>
        public void Items_Remove(int index)
        {
            try
            {
                //索引必须必需在有效范围内才可以使用
                if (this.bookList.Count > index && index > -1)
                {
                    this.bookList.RemoveAt(index);
                }
                //刷新
                this.ReFlush();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Items_Remove", ex.ToString(), index);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 删除指定子项
        /// </summary>
        /// <param name="ucBook">书本</param>
        public void Items_Remove(UcBook ucBook)
        {
            try
            {
                //删除子项
                this.bookList.Remove(ucBook);
                //刷新
                this.ReFlush();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Items_Remove", ex.ToString(), ucBook);
            }
            finally
            {
            }
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 通过高度获设置行的数量
        /// </summary>
        /// <param name="height">窗体当前高度</param>
        int getHeightNum(int height)
        {
            //返回的行的数量
            int countReturn = 0;
            try
            {
                //通过硬性参数来设置
                if (height >= 150 && height < 400)
                {
                    countReturn = 1;
                }
                else if (height >= 400 && height < 800)
                {
                    countReturn = 2;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "getHeightNum", ex.ToString(), height);
            }
            finally
            {
            }
            return countReturn;
        }

        /// <summary>
        /// 判断行数
        /// </summary>
        /// <param name="width">父容器宽度</param>
        /// <returns>返回列的数量</returns>
        int GetWidthNum(int width)
        {
            //返回的列的数量
            int countReturn = gridMain.ColumnDefinitions.Count;
            try
            {
                List<int> intList = new List<int>();
                //获取硬性参数
                for (int i = 0; i < 20; i++)
                {
                    intList.Add(50 + 150 * i);
                }
                //通过硬性参数来设置
                for (int i = 0; i < 20; i++)
                {
                    if (width >= intList[i] && width < intList[i + 1])
                    {
                        countReturn = i + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "GetWidthNum", ex.ToString(), width);
            }
            finally
            {
            }
            return countReturn;
        }

        /// <summary>
        /// 通过指定的书本集合位置和书本集合来给书架添加书本
        /// </summary>
        /// <param name="i">书本集合的指定位置</param>
        /// <param name="bookListSelf">书本集合</param>
        public void ReFlush(int i, List<UcBook> bookListSelf)
        {
            try
            {
                //清除所有列
                gridMain.ColumnDefinitions.Clear();
                //清除书架上的书本
                gridMain.Children.Clear();
                //清除所有行
                gridMain.RowDefinitions.Clear();
                //如果为第二行
                if (this.rowCount == 2)
                {
                    //合理布局，以达到书本与书架之间的磨合
                    gridMain.RowDefinitions.Add(new RowDefinition());
                    gridMain.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(15) });
                    gridMain.RowDefinitions.Add(new RowDefinition());
                    gridMain.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(13) });
                    int k = 0;
                    //通过循环来添加书本并设置列的位置
                    for (; k < ColumnCount; i++, k++)
                    {
                        //添加一列
                        gridMain.ColumnDefinitions.Add(new ColumnDefinition());
                        //索引必须小于书本集的数量
                        if (i < bookListSelf.Count && bookList.Count > 0)
                        {
                            //设置行与列的位置并添加
                            Grid.SetRow(bookListSelf[i], 0);

                            Grid.SetColumn(bookListSelf[i], k);
                            gridMain.Children.Add(bookListSelf[i]);
                        }
                    }
                    if (!string.IsNullOrEmpty(BookShellImageUri))
                    {
                        //调整书架背景
                        gridMain.Background = GetImageBrush(BookShellImageUri);
                    }
                    else
                    {
                        gridMain.Background = CommonMethod.GetImagebrush("BookShell");
                    }
                    for (int j = 0; j < ColumnCount; j++, i++)
                    {
                        //索引必须小于书本集的数量
                        if (i < bookListSelf.Count && bookList.Count > 0)
                        {
                            //设置行与列的位置并添加
                            Grid.SetRow(bookListSelf[i], 2);
                            Grid.SetColumn(bookListSelf[i], j);
                            gridMain.Children.Add(bookListSelf[i]);
                        }
                    }
                }

                //如果书架只有一行
                if (this.rowCount == 1)
                {
                    //一行书架的背景
                    if (!string.IsNullOrEmpty(OtherImageUri))
                    {
                        gridMain.Background = GetImageBrush(SingleShellImageUri);
                    }
                    else
                    {
                        gridMain.Background = CommonMethod.GetImagebrush("BookShell2");
                    }
                    //合理布局，以达到书本与书架之间的磨合
                    gridMain.RowDefinitions.Add(new RowDefinition());
                    gridMain.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(17) });

                    int k = 0;
                    //通过循环来添加书本并设置列的位置
                    for (; k < ColumnCount; i++, k++)
                    {
                        //添加一列
                        gridMain.ColumnDefinitions.Add(new ColumnDefinition());
                        //索引必须小于书本集的数量
                        if (i <= bookListSelf.Count - 1 && bookListSelf.Count > 0)
                        {

                            //设置行与列的位置并添加
                            Grid.SetRow(bookListSelf[i], 0);
                            Grid.SetColumn(bookListSelf[i], k);
                            gridMain.Children.Add(bookListSelf[i]);
                        }
                    }
                }
                //书架当前所能承载的书本的数量
                PageBookCount = this.ColumnCount * this.RowCount;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ReFlush", ex.ToString(), i, bookListSelf);
            }
            finally
            {
            }
        }

        /// <summary>
        ///刷新（更改子项数量时发生）
        /// </summary>
        public void ReFlush()
        {
            try
            {
                this.ReFlush(GetListStart(Convert.ToInt32(txtBoxSelect.Text)), whoTheList);

                if (PageBookCount > 0)
                {
                    //如果当前页的数量大于页的总数量，那就将当前页设置为最后一页
                    this.PageCount = (int)Math.Ceiling(this.bookList.Count / Convert.ToDouble(PageBookCount));
                }
                else
                {
                    //如果当前页的数量大于页的总数量，那就将当前页设置为最后一页
                    this.PageCount = (int)Math.Ceiling(this.bookList.Count / Convert.ToDouble(1));
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "ReFlush", ex.ToString());
            }
            finally
            {
            }
        }

        /// <summary>
        /// 获取一个图片画刷
        /// </summary>
        /// <param name="uri">图片的路径</param>
        /// <returns>=返回一个图片画刷</returns>
        ImageBrush GetImageBrush(string uri)
        {
            ImageBrush imagebursh = null;
            try
            {
                string root = Environment.CurrentDirectory.Replace("bin\\Debug", "");
                imagebursh = new ImageBrush()
                {
                    ImageSource = new BitmapImage(new Uri(root + uri, UriKind.RelativeOrAbsolute))
                };
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "GetImageBrush", ex.ToString(), uri);
            }
            finally
            {
            }
            return imagebursh;
        }

        /// <summary>
        /// 快速获取一个SolidColorBrush
        /// </summary>
        /// <param name="bbcdd">可以是（#bb9900）</param>
        /// <returns>=返回实例</returns>
        SolidColorBrush GetSolicolorBrush(string bbcdd)
        {
            SolidColorBrush s = null;
            try
            {
                s = (SolidColorBrush)XamlReader.Load(GetReader("<SolidColorBrush xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' Color ='" + bbcdd + "'/>"));
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

        /// <summary>
        /// 快速获取一个path实例
        /// </summary>
        /// <param name="pathData">Data属性</param>
        System.Windows.Shapes.Path GetPath(string pathData)
        {
            System.Windows.Shapes.Path path = null;
            try
            {
                //通过xaml的方式后台动态添加形状
                string path1 = @"<Path Fill ='Red' Width ='10' Stretch='Fill' Opacity='0.5' " + xmlns + "  Data = '" + pathData + "'  />";
                path = (System.Windows.Shapes.Path)XamlReader.Load(GetReader(path1));
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

        /// <summary>
        /// 获取XmlReader
        /// </summary>
        /// <param name="xml">xml文本</param>
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
                MethodLb.CreateLog(typeof(UcBookShell).ToString(), "GetReader", ex.ToString(), xml);
            }
            finally
            {
            }
            return xmlreader;
        }

        /// <summary>
        /// 通过当前页来指定书本集合的起始位置
        /// </summary>
        /// <param name="pageNN">当前页</param>
        public int GetListStart(int pageNN)
        {
            int result = 0;
            try
            {
                //返回一个起始的位置（第一页为0）
                if (pageNN > 0)
                {
                    result = (pageNN * PageBookCount) - PageBookCount;
                }

            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "GetListStart", ex.ToString(), pageNN);
            }
            finally
            {
            }
            return result;
        }

        #endregion
    }
}
