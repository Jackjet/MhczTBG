using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Win32;
using Microsoft.SharePoint.Client;
using System.Reflection;
using System.Threading;
using System.Management;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Controls;
using Microsoft.Office.Interop.Excel;
using MhczTBG.Controls;
using MhczTBG.Controls.DataGridOperate;
using MhczTBG.Controls.Print;
using MhczTBG.Controls.Files;
using System.Net;
using System.Diagnostics;
using System.Windows.Input;
using MhczTBG.Controls.CustomWindow;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.ComponentModel;

namespace MhczTBG.Common
{
    public static class CommonMethod
    {
        #region 获取顶级的父容器

        /// <summary>
        /// 获取顶级的父容器
        /// </summary>
        /// <param name="count">指定遍历的层数</param>
        /// <param name="element">指定子元素</param>
        /// <returns>返回顶级容器</returns>
        public static FrameworkElement GetRootParent(int count, FrameworkElement element)
        {
            //定义一个变量,每次遍历存储更高级的父元素
            FrameworkElement rootParent = element;
            try
            {
                //一层一层遍历
                for (int i = 0; i < count; i++)
                {
                    //如果达到最顶层，则跳出
                    if (rootParent.Parent == null) break;
                    rootParent = rootParent.Parent as FrameworkElement;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(typeof(FrameworkElement).ToString(), "GetRootParent", ex.ToString(), count, element);
            }
            finally
            {
            }
            //返回顶层元素
            return rootParent;
        }

        #endregion

        #region 获取进程列表中的所有进程的参数

        /// <summary>
        /// 获取所有进程的参数
        /// </summary>
        /// <returns>返回进程参数的列表</returns>
        public static List<string> GetProcess()
        {
            List<string> list = new List<string>();
            try
            {
                SelectQuery selectQuery = new SelectQuery("select * from Win32_Process");
                object cmdLine = string.Empty;
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(selectQuery))
                {
                    foreach (ManagementObject process in searcher.Get())
                    {
                        cmdLine = process.Properties["CommandLine"].Value;
                        list.Add(cmdLine.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(typeof(FrameworkElement).ToString(), "GetProcess", ex.ToString());
            }
            finally
            {
            }
            return list;
        }

        #endregion

        #region 获取时间差（时间差显示年、月、日、时、分、秒）

        /// <summary>
        /// 获取时间差
        /// </summary>
        ///  /// <param name="dStart"></param>
        /// <param name="dEnd"></param>
        /// <returns>返回具体时间差</returns>
        public static string TimeDistance(DateTime dStart, DateTime dEnd)
        {
            string timeDsiance = string.Empty;
            try
            {
                TimeSpan sp = dStart - dEnd;
                timeDsiance = string.Format("{0}年,{1}月,{2}天,{3}时,{4}分,{5}秒", (dStart.Year - dEnd.Year), (dStart.Month - dEnd.Month), sp.Days, sp.Hours, sp.Minutes, sp.Seconds);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(typeof(FrameworkElement).ToString(), "TimeDistance", ex.ToString(), dStart, dEnd);
            }
            finally
            {
            }
            return timeDsiance;
        }

        /// <summary>
        /// 获取时间差
        /// </summary>
        /// <param name="strStart"></param>
        /// <param name="strEnd"></param>
        ///  /// <returns>返回具体时间差</returns>
        public static string TimeDistance(string strStart, string strEnd)
        {
            string timeDsiance = string.Empty;
            try
            {
                DateTime d1 = Convert.ToDateTime(strStart);
                DateTime d2 = Convert.ToDateTime(strEnd);
                TimeSpan sp = d1 - d2;
                timeDsiance = string.Format("{0}年,{1}月,{2}天,{3}时,{4}分,{5}秒", (d1.Year - d2.Year), (d1.Month - d2.Month), sp.Days, sp.Hours, sp.Minutes, sp.Seconds);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(typeof(FrameworkElement).ToString(), "TimeDistance", ex.ToString(), strStart, strEnd);
            }
            finally
            {
            }
            return timeDsiance;
        }

        #endregion

        #region 通过对话框获取一个文件，并将其转为字节数组

        /// <summary>
        /// 将流转为字节数组
        /// </summary>
        /// <returns>返回字节数组</returns>
        public static byte[] OpengDialogGetStream()
        {
            //声明一个数组
            byte[] byteArrayGet = null;
            try
            {
                //打开一个对话框
                OpenFileDialog dialog = new OpenFileDialog();
                if (dialog.ShowDialog() == true)
                {
                    //将文件流写入指定的字节数组里去
                    using (Stream fs = dialog.OpenFile())
                    {
                        byteArrayGet = new byte[fs.Length];
                        fs.Read(byteArrayGet, 0, byteArrayGet.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(typeof(FrameworkElement).ToString(), "OpengDialogGetStream", ex.ToString());
            }
            finally
            {
            }
            //返回文件的字节数组
            return byteArrayGet;
        }

        #endregion

        #region 往一个集合里按照属性的排列顺序进行添加一系列的参数，将生成一个新的Item

        /// 往集合中添加一个元素
        /// <summary>
        /// 往集合中添加一个元素
        /// </summary>
        /// <typeparam name="T">未知类型</typeparam>
        /// <param name="list">list集合</param>
        /// <param name="args">元素的属性值</param>
        public static void DataGridRowAdd<T>(List<T> list, params object[] args)
        {
            try
            {
                //获取指定类型
                Type type = typeof(T);
                //通过程序集加载来创建实例
                T obj = (T)Assembly.Load(type.Assembly.FullName).CreateInstance(type.FullName);
                //获取属性
                PropertyInfo[] propertyInfoes = obj.GetType().GetProperties();
                //遍历赋值
                for (int i = 0; i < args.Length; i++)
                {
                    propertyInfoes[i].SetValue(obj, args[i], null);

                }
                //添加到集合中去
                list.Add(obj);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(typeof(FrameworkElement).ToString(), "DataGridRowAdd", ex.ToString(), list, args);
            }
            finally
            {
            }
        }

        #endregion

        #region CAML语法

        //And 并且
        //BeginsWith 以某字符串开始的
        //Contains 包含某字符串
        //Eq 等于
        //FieldRef 一个字段的引用 (在GroupBy 中使用)
        //Geq 大于等于
        //GroupBy 分组
        //Gt 大于
        //IsNotNull 非空
        //IsNull 空
        //Leq 小于等于
        //Lt 小于
        //Neq 不等于
        //Now 当前时间
        //Or 或
        //OrderBy 排序
        //Today 今天的日期
        //TodayIso 今天的日期（ISO格式）
        //Where Where子句

        public static string GetCamlString(string startData, string endData, Dictionary<string, string> msgs)
        {
            string strCamlResult = null;
            try
            {
                string strAnd = string.Empty;

                List<string> list = new List<string>();

                string endResult = string.Empty;

                if (msgs.Count > 0)
                {
                    foreach (var item in msgs)
                    {
                        strAnd += "<And>";
                        list.Add("</And><Eq><FieldRef Name='" + item.Key + "'/><Value Type='Text'>" + item.Value + "</Value></Eq>");
                    }
                }

                list.ForEach(Item => endResult += Item);
                strCamlResult = "<View><Query><Where><And>" + strAnd + "<Geq><FieldRef Name ='startData'/><Value Type = 'DateTime'>" + startData + "</Value></Geq><Leq><FieldRef Name ='startData'/><Value Type = 'DateTime'>" + endData + "</Value></Leq>" + endResult + "</And></Where></Query></View>";
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(typeof(FrameworkElement).ToString(), "GetCamlString", ex.ToString(), startData, endData, msgs);
            }
            finally
            {
            }
            return strCamlResult;
        }

        #endregion

        #region 小小知识点

        //三元运算符的运用
        //dicSource["CheJianTongZhiShiJian"] = DateTime.TryParse(dicSource["CheJianTongZhiShiJian"], out DT) ? DateTime.Parse(dicSource["CheJianTongZhiShiJian"]).ToLocalTime().ToString() : "";
        //三目运算符<表达式1>?<表达式2>:<表达式3>; 
        //属于关系运算符，常用于关系比较，主要用于比较关系的状态只有两种的情况（大于 和 不大于，真 和 假）先求表达式1的值, 如果为真, 则执行表达式2，并返回表达式2的结果 ;
        //如果表达式1的值为假, 则执行表达式3 ，并返回表达式3的结果比如以下表达式：a>0? a++:（a = 1）当a>0为真 时，执行a++，整个表达式的值等于表达式a++的值，当a>0为假 时，执行a=1，整个表达式的值等于表达式a=1的值。
        #endregion

        #region 快速获取相对路径

        public static string GetCurrentRoot(string fileName)
        {
            string result = null;
            try
            {
                result = Environment.CurrentDirectory.Replace("bin\\Debug", fileName);

                #region 其他方案

                //result = System.Windows.Application.Current.StartupUri.AbsolutePath;

                //result = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

                //result = System.AppDomain.CurrentDomain.BaseDirectory;

                //result = System.IO.Directory.GetCurrentDirectory();
                //result = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

                //result = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);

                #endregion
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(typeof(FrameworkElement).ToString(), "GetCurrentRoot", ex.ToString(), fileName);
            }
            finally
            {
            }
            return result;
        }

        #endregion

        #region 快速获取调用程序集的图片

        internal static BitmapImage GeTheSource(string imageUri)
        {
            BitmapImage imageSource = null;
            try
            {
                imageSource = new BitmapImage(new Uri(GetCurrentRoot(imageUri), UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(typeof(FrameworkElement).ToString(), "GeTheSource", ex.ToString(), imageUri);
            }
            finally
            {
            }
            return imageSource;
        }

        #endregion

        #region 读取property里设置的资源

        /// <summary>
        /// 指定property里的图片名称,得到其画刷
        /// </summary>
        /// <param name="symbolName"></param>
        /// <returns></returns>
        public static ImageBrush GetCurrentImagebrush(string ImageName)
        {
            ImageBrush imageBrush = new ImageBrush();
            try
            {
                imageBrush.ImageSource = new BitmapImage(new Uri(GetCurrentRoot(ImageName), UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(typeof(FrameworkElement).ToString(), "GetCurrentImagebrush", ex.ToString(), ImageName);
            }
            finally
            {
            }
            return imageBrush;
        }

        /// <summary>
        /// 指定property里的图片名称,得到其画刷
        /// </summary>
        /// <param name="symbolName"></param>
        /// <returns></returns>
        public static ImageBrush GetImagebrush(string ImageName)
        {
            ImageBrush imageBrush = new ImageBrush();
            try
            {
                System.Resources.ResourceManager rm = MhczTBG.Properties.Resources.ResourceManager;
                System.Drawing.Bitmap b = (System.Drawing.Bitmap)rm.GetObject(ImageName);

                imageBrush.ImageSource = GetImageSource(b);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(typeof(FrameworkElement).ToString(), "GetImagebrush", ex.ToString(), ImageName);
            }
            finally
            {
            }
            return imageBrush;
        }

        /// <summary>
        /// 指定property里的图片名称,得到其画刷
        /// </summary>
        /// <param name="symbolName"></param>
        /// <returns></returns>
        public static ImageBrush GetImagebrush(Bitmap bitmap)
        {
            ImageBrush imageBrush = new ImageBrush();
            try
            {
                imageBrush.ImageSource = GetImageSource(bitmap);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(typeof(FrameworkElement).ToString(), "GetImagebrush", ex.ToString(), bitmap);
            }
            finally
            {
            }
            return imageBrush;
        }


        /// <summary>
        /// 指定bitmap资源返回图片资源
        /// </summary>
        /// <param name="bitmap">Propertys里的资源（嵌入）</param>
        /// <returns>返回图片资源</returns>
        public static BitmapSource GetImageSource(Bitmap bitmap)
        {
            BitmapImage result = new BitmapImage();
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    //注意：转换的图片的原始格式ImageFormat设为BMP、JPG、PNG等
                    bitmap.Save(stream, ImageFormat.Png);
                    stream.Position = 0;

                    result.BeginInit();
                    result.CacheOption = BitmapCacheOption.OnLoad;
                    result.StreamSource = stream;
                    result.EndInit();
                    result.Freeze();
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(typeof(FrameworkElement).ToString(), "GetImageSource", ex.ToString(), bitmap);
            }
            finally
            {
            }
            return result;
        }

        /// <summary>
        /// 指定图片名称获取图片资源
        /// </summary>
        /// <param name="imageName">图片名称</param>
        /// <returns>返回图片资源</returns>
        public static BitmapImage GetImageSource(string imageName)
        {
            BitmapImage result = new BitmapImage();
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    System.Resources.ResourceManager rm = MhczTBG.Properties.Resources.ResourceManager;
                    System.Drawing.Bitmap bitmap = (System.Drawing.Bitmap)rm.GetObject(imageName);

                    //注意：转换的图片的原始格式ImageFormat设为BMP、JPG、PNG等
                    bitmap.Save(stream, ImageFormat.Png);
                    stream.Position = 0;

                    result.BeginInit();
                    result.CacheOption = BitmapCacheOption.OnLoad;
                    result.StreamSource = stream;
                    result.EndInit();
                    result.Freeze();
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(typeof(FrameworkElement).ToString(), "GetImageSource", ex.ToString(), imageName);
            }
            finally
            {
            }
            return result;
        }

        #endregion

        #region 将Datagrid导出到Excel

        /// <summary>
        /// 将Datagrid导出到Excel
        /// </summary>
        /// <param name="tittle">工作簿的名称</param>
        /// <param name="datagrid">要导出的列表</param>
        public static void Excel_LeadingOut(DataGrid datagrid, Type type)
        {
            try
            {
                int rowDistance = 1;

                int columnDistance = 1;

                //创建excel应用程序
                Microsoft.Office.Interop.Excel.Application excelother = new Microsoft.Office.Interop.Excel.Application();
                //创建一个excel工作簿
                Microsoft.Office.Interop.Excel._Worksheet ws = new Microsoft.Office.Interop.Excel.WorksheetClass();
                //启用
                excelother.Application.Workbooks.Add(true);

                #region 设置页面标题

                ws = (Microsoft.Office.Interop.Excel.Worksheet)excelother.ActiveSheet;
                Microsoft.Office.Interop.Excel.Range rangTittle = null;


                #endregion

                #region 生成数据

                //生成数据
                PropertyInfo[] propertyInes = type.GetProperties();
                for (int x = 0; x < datagrid.Columns.Count; x++)
                {
                    for (int y = 0; y < datagrid.Items.Count; y++)
                    {
                        var strText = propertyInes[x].GetValue(datagrid.Items[y], null);
                        if (strText == null) continue;
                        excelother.Cells[y + rowDistance + 1, x + columnDistance] = strText.ToString();
                    }
                }

                #endregion

                //生成大标题
                for (int i = 0; i < datagrid.Columns.Count; i++)
                {
                    rangTittle = ws.get_Range(ws.Cells[rowDistance, i + columnDistance], ws.Cells[rowDistance, i + columnDistance]);
                    excelother.Cells[rowDistance, i + columnDistance] = datagrid.Columns[i].Header.ToString();
                    rangTittle.RowHeight = 25;
                    rangTittle.ColumnWidth = 13;
                    rangTittle.Font.Size = 17;
                    rangTittle.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                    rangTittle.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                }

                excelother.Visible = true;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(typeof(FrameworkElement).ToString(), "Excel_LeadingOut", ex.ToString(), datagrid, type);
            }
            finally
            {
            }
        }

        #endregion

        #region 加密解密方法

        //加密过程
        public static string tobase(string password)
        {
            string Base64Str = string.Empty;
            try
            {
                byte[] myByte;
                Encoding myEncoding = Encoding.GetEncoding("utf-8");
                myByte = myEncoding.GetBytes(password);
                Base64Str = Convert.ToBase64String(myByte);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(typeof(FrameworkElement).ToString(), "tobase", ex.ToString(), password);
            }
            finally
            {
            }
            return Base64Str;
        }

        //解密过程
        public static string frombase(string password)
        {
            string factString = string.Empty;
            try
            {
                byte[] myByte;
                Encoding myEncoding = Encoding.GetEncoding("utf-8");
                myByte = Convert.FromBase64String(password);
                factString = myEncoding.GetString(myByte);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(typeof(FrameworkElement).ToString(), "frombase", ex.ToString(), password);
            }
            finally
            {
            }
            return factString;
        }

        #endregion

        #region 获取子元素

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj)

           where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }
                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        public static void FindChildByType(DependencyObject relate, Type type, ref FrameworkElement resElement)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(relate); i++)
            {
                var el = VisualTreeHelper.GetChild(relate, i) as FrameworkElement;
                if (el.GetType() == type)
                {
                    resElement = el;
                    return;
                }
                else
                {
                    FindChildByType(el, type, ref resElement);
                }
            }
        }


        #endregion

        #region 打印

        public static void FHDataGrid_btn_打印(string strPrintTittle, DataPager dataPager, DataGrid dataGrid, List<FHFormLb> listFormResult)
        {
            try
            {
                //生成自定义打印控件
                PrintDocument print = new PrintDocument();

                //打印列表集合
                List<PrintDataGrid> listDataGrids = new List<PrintDataGrid>();

                //每张页面所能承载的数量
                int intCount = 45;

                //获取打印的总页数
                int intPage = (int)Math.Ceiling((double)dataPager.list.Count / intCount);

                int j = 0;


                //循环打印每一页
                for (int i = 0; i < intPage; i++)
                {
                    //创建打印列表
                    PrintDataGrid datagrid = new PrintDataGrid();

                    //生成标题
                    datagrid.TitleInit(dataGrid.Columns);

                    //循环添加数据
                    for (; j < intCount * (i + 1); j++)
                    {
                        if (j < dataPager.list.Count)
                        {

                            //if (j == 0) continue;
                            datagrid.ItemsAdd(listFormResult[j]);

                        }
                        else break;
                    }

                    listDataGrids.Add(datagrid);
                }

                //循环添加每一个打印页
                foreach (var item in listDataGrids)
                {
                    print.Items_Add(strPrintTittle + DateTime.Now.ToShortDateString(), item.datagrid);
                }
                //打印窗体显示
                print.Show();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(typeof(CommonMethod).FullName, "FHDataGrid_btn_打印", ex.ToString(), strPrintTittle, dataPager, dataGrid, listFormResult);
            }
        }

        public static void FHDataGrid_btn_打印(PrintDocument print, string strPrintTittle, DataPager dataPager, DataGrid dataGrid, List<FHFormLb> listFormResult)
        {

            //打印列表集合
            List<PrintDataGrid> listDataGrids = new List<PrintDataGrid>();

            //每张页面所能承载的数量
            int intCount = 45;

            //获取打印的总页数
            int intPage = (int)Math.Ceiling((double)dataPager.list.Count / intCount);

            int j = 0;


            //循环打印每一页
            for (int i = 0; i < intPage; i++)
            {
                //创建打印列表
                PrintDataGrid datagrid = new PrintDataGrid();

                //生成标题
                datagrid.TitleInit(dataGrid.Columns);

                //循环添加数据
                for (; j < intCount * (i + 1); j++)
                {
                    if (j < dataPager.list.Count)
                    {

                        //if (j == 0) continue;
                        datagrid.ItemsAdd(listFormResult[j]);

                    }
                    else break;
                }

                listDataGrids.Add(datagrid);
            }

            //循环添加每一个打印页
            foreach (var item in listDataGrids)
            {
                print.Items_Add(strPrintTittle + DateTime.Now.ToShortDateString(), item.datagrid);
            }
            //打印窗体显示
            print.Show();

        }

        #endregion

        #region 导入Excel

        //excel应用程序
        static Microsoft.Office.Interop.Excel.Application _excelother = null;

        public static void FHDataGrid_btn_导入Excel(string pageTittle, DataGrid dataGrid, List<FHFormLb> listFormResult,ref Microsoft.Office.Interop.Excel.Application application)
        {
            try
            {
                int rowDistance = 1;

                if (_excelother == null)
                    //创建excel应用程序
                    _excelother = new Microsoft.Office.Interop.Excel.Application();

                application = _excelother;

                //创建一个excel工作簿
                Microsoft.Office.Interop.Excel._Worksheet ws = new Microsoft.Office.Interop.Excel.WorksheetClass();

                _excelother.WorkbookBeforeClose += new AppEvents_WorkbookBeforeCloseEventHandler(excelother_WorkbookBeforeClose);
                //启用
                _excelother.Application.Workbooks.Add(true);

                #region 生成数据

                int count = 0;

                List<string> tittleList = new List<string>();

                bool isWait = true;

                dataGrid.Dispatcher.BeginInvoke(new System.Action(() =>
                    {
                        count = dataGrid.Columns.Count;

                        foreach (var item in dataGrid.Columns)
                        {
                            tittleList.Add(item.Header.ToString());
                        }
                        isWait = false;
                    }));
                while (isWait)


                    //生成小标题
                    for (int i = 1; i < count; i++)
                    {
                        _excelother.Cells[1 + rowDistance, i] = tittleList[i];

                    }

                ////生成数据
                PropertyInfo[] propertyInes = typeof(FHFormLb).GetProperties();

                for (int x = 1; x < count; x++)
                {
                    for (int y = 1; y < listFormResult.Count; y++)
                    {
                        string columnName = tittleList[x];

                        string columnValue = string.Empty;

                        foreach (var property in propertyInes)
                        {

                            if (property.Name == columnName)
                            {
                                if (property.Name == "附件")
                                {
                                    columnName = listFormResult[y].是否有附件;
                                    break;
                                }
                                else
                                {
                                    columnValue = property.GetValue(listFormResult[y - 1], null).ToString();
                                    break;
                                }
                            }
                        }
                        _excelother.Cells[y + 1 + rowDistance, x] = columnValue;
                    }
                }

                #endregion

                #region 设置页面标题

                ws = (Microsoft.Office.Interop.Excel._Worksheet)_excelother.ActiveSheet;
                Microsoft.Office.Interop.Excel.Range rangTittle = null;
                rangTittle = ws.get_Range(ws.Cells[1, 1], ws.Cells[1, count - 1]);
                rangTittle.MergeCells = true;
                _excelother.Cells[1, 1] = pageTittle;
                rangTittle.ColumnWidth = 26;
                rangTittle.Font.Size = 17;
                rangTittle.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;

                rangTittle.Borders.LineStyle = XlLineStyle.xlContinuous;
                #endregion

                #region 单元格属性设置

                //设置标题属性
                for (int i = 1; i < count; i++)
                {
                    //设置标题单元格属性                
                    Microsoft.Office.Interop.Excel.Range r = ws.get_Range(ws.Cells[1 + rowDistance, i], ws.Cells[1 + rowDistance, i]);
                    r.Borders.LineStyle = XlLineStyle.xlContinuous;
                    r.Font.Size = 13;
                    r.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;

                    string columnHeader = tittleList[i];
                    if (columnHeader.Contains("日期时间") || columnHeader.Contains("影响范围") || columnHeader.Contains("单位") || columnHeader.Contains("障碍设备") || columnHeader.Contains("线别"))
                    {
                        r.ColumnWidth = 20;
                    }
                }

                //数据属性
                for (int x = 1; x < count; x++)
                {
                    for (int y = 1; y < listFormResult.Count; y++)
                    {
                        //设置数据单元格属性                   
                        Microsoft.Office.Interop.Excel.Range r = ws.get_Range(ws.Cells[y + 1 + rowDistance, x], ws.Cells[y + 1 + rowDistance, x]);
                        r.Borders.LineStyle = XlLineStyle.xlContinuous;
                        r.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                        r.RowHeight = 26;
                    }
                }
                _excelother.Visible = true;

                #endregion
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(typeof(CommonMethod).FullName, "FHDataGrid_btn_导入Excel", ex.ToString());
            }
        }

        /// <summary>
        /// excel关闭窗体时结束进程
        /// </summary>
        /// <param name="Wb"></param>
        /// <param name="Cancel"></param>
        static void excelother_WorkbookBeforeClose(Microsoft.Office.Interop.Excel.Workbook Wb, ref bool Cancel)
        {
            try
            {
                Wb.Application.WorkbookBeforeClose -= excelother_WorkbookBeforeClose;

                var workBooksCount = Wb.Application.Workbooks.Count;
                if (workBooksCount == 1)
                {
                    CommonMethod.Kill(Wb.Application);
                    _excelother = null;
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(typeof(CommonMethod).FullName, "excelother_WorkbookBeforeClose", ex.ToString(), Wb, Cancel);
            }
        }

        #endregion

        #region 点击附件图片查看附件（xaml绑定事件）

        public static void WatchImage(int ID)
        {
            try
            {
                ////获取相应ID
                //int ID = Convert.ToInt32((sender as FrameworkElement).Tag);
                //通过ID获取当前记录的附件
                string AttachmentNames = DataOperation.GetItemByID(Proxy.ListName, ID);
                //分割
                string[] files = AttachmentNames.Split(new char[] { ';' });

                string[] realFiles = new string[files.Count() - 1];

                for (int i = 0; i < realFiles.Count(); i++)
                {
                    realFiles[i] = files[i].Split(new char[] { ',' })[0];
                }
                //附件承载容器
                System.Windows.Controls.ListBox uploadList = new System.Windows.Controls.ListBox();
                //显示附件窗体
                TbgWindow window = new TbgWindow()
                {
                    Width = 350,
                    Height = 400,
                    WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen
                };
                window.Title = "附件列表";
                window.BorMain.Child = uploadList;
                window.WindowBorderBrushSetting("linColor1", 0.8);
                //通过循环来添加子项
                for (int i = 0; i < realFiles.Count(); i++)
                {
                    if (!string.IsNullOrEmpty(realFiles[i])) //不为空执行
                    {
                        uploadList.Items.Add(GetHyperLinkText(ID, realFiles[i]));
                    }
                }
                ////关闭时重新注册
                //window.Closed += (object sender1, EventArgs e1) =>
                //{
                //    (sender as FrameworkElement).MouseLeftButtonDown += new MouseButtonEventHandler(imgexit2_MouseLeftButtonUp);
                //};
                //附件窗体显示
                window.Topmost = true;
                window.Show();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(typeof(CommonMethod).FullName, "WatchImage", ex.ToString(), ID);
            }
        }

        #endregion

        #region 生成超链接下载文本（通过浏览器查看，也可以直接下载）

       public static ListBoxItem GetHyperLinkText(int id, string fileName)
        {
            //下载链接子项
            ListBoxItem item = new ListBoxItem();
            try
            {
                //下载链接承载容器
                StackPanel Filepanel = new StackPanel();
                Filepanel.Orientation = Orientation.Horizontal;

                //下载按钮
                System.Windows.Controls.Button imagebutton = new System.Windows.Controls.Button();
                imagebutton.Content = "下载";
                imagebutton.Width = 30;
                imagebutton.HorizontalAlignment = HorizontalAlignment.Center;
                imagebutton.VerticalAlignment = VerticalAlignment.Center;
                //点击进行下载
                imagebutton.Click += new RoutedEventHandler(imagebutton_Click);
                imagebutton.Height = Convert.ToDouble(20);
                if (fileName.Contains(Proxy.SelectedServiceUri + Proxy.strSiteUrl))
                    imagebutton.Tag = fileName;
                else
                    imagebutton.Tag = Proxy.SelectedServiceUri + Proxy.strSiteUrl + Convert.ToString(id) + "/" + fileName;
                imagebutton.Style = MhczTBG.StyleResource.TongXinStyle.Instacnce.Resources["hybutton"] as System.Windows.Style;

                //所要下载的文件名称
                System.Windows.Controls.Button ly02 = new System.Windows.Controls.Button();
                ly02.Foreground = new SolidColorBrush(Colors.Blue);
                ly02.Content = fileName;
                if (fileName.Contains(Proxy.SelectedServiceUri + Proxy.strSiteUrl))
                    ly02.Tag = fileName;
                else
                    ly02.Tag = new Uri(Proxy.SelectedServiceUri + Proxy.strSiteUrl + Convert.ToString(id) + "/" + fileName);
                //点击进行预览
                ly02.Click += new RoutedEventHandler(ly02_Click);
                ly02.Style = MhczTBG.StyleResource.TongXinStyle.Instacnce.Resources["hybutton"] as System.Windows.Style;
                Filepanel.Children.Add(ly02);
                Filepanel.Children.Add(imagebutton);


                item.Content = Filepanel;
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(typeof(CommonMethod).FullName, "GetHyperLinkText", ex.ToString(), id, fileName);
            }
            return item;
        }

      public  static void ly02_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //获取附件的url
                string url = ((System.Windows.Controls.Button)sender).Tag.ToString();
                WatchIMage(url);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(typeof(CommonMethod).FullName, "ly02_Click", ex.ToString());
            }
        }

        static void imagebutton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //获取附件的url
                string fileUrl = ((System.Windows.Controls.Button)sender).Tag.ToString();
                FileDown(fileUrl);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(typeof(CommonMethod).FullName, "imagebutton_Click", ex.ToString());
            }
        }


        #region 通过浏览器来显示附件
        /// <summary>
        /// 点击查看图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       public static void WatchIMage(string url)
        {
            try
            {
                //如果不存在这个文件夹，则生成目录
                if (!Directory.Exists(Proxy.StrWatchUrl))
                {

                    Directory.CreateDirectory(Proxy.StrWatchUrl);
                }
                //通过url获取文件名称
                string fileName = url.Substring(url.LastIndexOf("/") + 1);

                //如果不存在该文件，则下载
                if (!System.IO.File.Exists(Proxy.StrWatchUrl + "\\" + fileName))
                {
                    WebClient client = new WebClient();
                    client.Credentials = new NetworkCredential(Proxy.UserName, Proxy.Password, Proxy.Domain);
                    client.DownloadFile(url, Proxy.StrWatchUrl + "\\" + fileName);
                }
                //通过进程打开该文件
                Process.Start(Proxy.StrWatchUrl + "\\" + fileName);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(typeof(CommonMethod).FullName, "WatchIMage", ex.ToString(), url);
            }
        }
        #endregion

        #region 附件下载

        public static void FileDown(string fileUrl)
        {
            try
            {
                //打开下载框
                FileDown dcWindown = new FileDown(fileUrl, Proxy.UserName, Proxy.Password, Proxy.Domain);
                //如果对话框取消,则关闭控件显示
                if (dcWindown.needDownload == true)
                {
                    dcWindown.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(typeof(CommonMethod).FullName, "FileDown", ex.ToString(), fileUrl);
            }
        }
        #endregion

        #endregion

        #region 将DataTable转为List
        //将转换好的list返回
        static List<FHFormLb> formList = new List<FHFormLb>();

        public static List<FHFormLb> DataTableToList(System.Data.DataTable datable)
        {
            //使用前先删除
            formList.Clear();
            try
            {
                //字典集合，先将datable转为字典集合，然后再将字典集合转为list
                Dictionary<string, object> dicList = new Dictionary<string, object>();

                //遍历创建表单对象，根据datable的数量
                for (int j = 0; j < datable.Rows.Count; j++)
                {
                    dicList.Clear();
                    //创建一个表单
                    FHFormLb form = new FHFormLb();
                    //通过databale列的数量来决定表单的字段的数量
                    for (int i = 0; i < datable.Columns.Count; i++)
                    {
                        //获取字段值
                        var d = datable.Rows[j][i];
                        //不为空则给表单对象赋值
                        if (d != DBNull.Value)
                        {
                            dicList.Add(datable.Columns[i].ToString(), d.ToString());
                        }
                    }
                    //添加一个表单
                    formList.Add(new FHFormLb(dicList));
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(typeof(CommonMethod).FullName, "DataTableToList", ex.ToString(), datable);
            }
            return formList;
        }

        #endregion

        #region 存储列表,获取列表

        static string fileRoot = @"C://鸣赫持志";

        static string fileAddress = @"C://鸣赫持志//列表模块.txt";

        public static bool SaveListControl(DataGridUserEntity userControl)
        {
            bool result = false;
            try
            {

                BindingList<DataGridUserEntity> listDataGridUserEntity = GetListControl();

                if (listDataGridUserEntity == null)
                    listDataGridUserEntity = new BindingList<DataGridUserEntity>();

                int ID = listDataGridUserEntity.Count;

                userControl.ID = ID;

                listDataGridUserEntity.Add(userControl);

                result = SaveListList(listDataGridUserEntity);
            }

            catch (Exception)
            {

            }
            return result;
        }

        public static bool SaveListList(BindingList<DataGridUserEntity> listDataGridUserEntity)
        {
            bool result = false;
            try
            {
                if (!Directory.Exists(fileRoot))
                {
                    Directory.CreateDirectory(fileRoot);
                }

                BinaryFormatter formatter = new BinaryFormatter();

                using (Stream stream = new FileStream(fileAddress, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                {
                    FileInfo fileni = new FileInfo(fileAddress);

                    formatter.Serialize(stream, listDataGridUserEntity);
                    result = true;

                    stream.Flush();
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(typeof(CommonMethod).FullName, "SaveListList", ex.ToString(), listDataGridUserEntity);
            }
            finally
            {
            }
            return result;
        }

        public static BindingList<DataGridUserEntity> GetListControl()
        {
            BindingList<DataGridUserEntity> listDataGridUserEntity = new BindingList<DataGridUserEntity>();
            try
            {
                if (!Directory.Exists(fileRoot)) return null;

                if (!System.IO.File.Exists(fileAddress)) return null;

                BinaryFormatter formatter = new BinaryFormatter();

                using (Stream destream = new FileStream(fileAddress, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    var dd = formatter.Deserialize(destream);
                    listDataGridUserEntity = dd as BindingList<DataGridUserEntity>;

                    destream.Flush();
                }
            }
            catch (Exception)
            {

            }
            return listDataGridUserEntity;

        }

        #endregion

        #region 刷新

        public static void ReFresh()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        #endregion

        #region 通过Application关闭Excel进程

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);
        public static void Kill(Microsoft.Office.Interop.Excel.Application excel)
        {
            try
            {
                IntPtr t = new IntPtr(excel.Hwnd);   //得到这个句柄，具体作用是得到这块内存入口 

                int k = 0;
                GetWindowThreadProcessId(t, out k);   //得到本进程唯一标志k
                System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById(k);   //得到对进程k的引用
                p.Kill();     //关闭进程k

            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(typeof(CommonMethod).FullName, "Kill", ex.ToString(), excel);
            }
        }

        #endregion
    }
}
