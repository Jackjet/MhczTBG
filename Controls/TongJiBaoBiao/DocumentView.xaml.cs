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
using System.Threading;
using System.Windows.Xps.Packaging;
using System.IO.Packaging;
using System.IO;
using System.Windows.Xps;
using System.Windows.Markup;
using MhczTBG.Common;

namespace MhczTBG.Controls.TongJiBaoBiao
{
    /// <summary>
    /// DocumentView.xaml 的交互逻辑
    /// </summary>
    public partial class DocumentView : UserControl
    {
        #region 声明变量
        /// <summary>
        /// 打印的字典
        /// </summary>
        public Dictionary<string, object> dic = new Dictionary<string, object>();
        /// <summary>
        /// 字段字典
        /// </summary>
        public List<string> titleList = new List<string>();
        #endregion

        #region 构造函数
        public DocumentView()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "DocumentView", ex.ToString());
            }
            finally
            {
            }
        }
        #endregion

        #region 事件区域
        public void Run()
        {
            try
            {
                docdocument.Blocks.Clear();
                //标题
                Paragraph para = new Paragraph();
                para.Style = this.FindResource("Heading") as Style;
                para.Inlines.Add(titleList[0]);
                docdocument.Blocks.Add(para);
                //小标题
                Paragraph centerpara = new Paragraph();
                centerpara.Inlines.Add("一、安全情况 （" + titleList[1] + "）");
                docdocument.Blocks.Add(centerpara);
                dic.Remove("标题");
                foreach (string item in dic.Values)
                {
                    Paragraph centerpara1 = new Paragraph();
                    centerpara1.Inlines.Add(item);
                    docdocument.Blocks.Add(centerpara1);
                }

                docdocument.PagePadding = new Thickness(50);
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Run", ex.ToString());
            }
            finally
            {
            }
        }

        /// <summary>
        /// 导出word文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Office.Interop.Word.Application excelother = new Microsoft.Office.Interop.Word.Application();
                //excelother.Visible = true;
                dynamic doc = excelother.Documents.Add();

                dynamic rng = doc.Range;
                int start = doc.Characters.Count - 1; //定义文本的坐标 
                int end = doc.Characters.Count - 1;
                rng = doc.content;
                rng = doc.Range(ref start, ref end);
                rng.Text = titleList[0] + "\r\n";
                rng.font.size = 20;
                rng.font.name = "宋体"; //设置字体  
                rng.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                dynamic rng1 = doc.Range;
                int start1 = doc.Characters.Count - 1; //定义文本的坐标 
                int end1 = doc.Characters.Count - 1;
                rng1 = doc.content;
                rng1 = doc.Range(ref start1, ref end1);
                rng1.Text = "一、安全情况 （" + titleList[1] + "）" + "\r\n";
                rng1.font.size = 14;
                rng1.font.name = "宋体";

                foreach (var item in dic)
                {
                    dynamic rng2 = doc.Range;
                    int start2 = doc.Characters.Count - 1; //定义文本的坐标 
                    int end2 = doc.Characters.Count - 1;
                    rng2 = doc.content;
                    rng2 = doc.Range(ref start2, ref end2);
                    rng2.Text = item.Value + "\r\n";
                    rng2.font.size = 10;
                    rng2.font.name = "宋体";
                }
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Button_Click", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                content.Print();
            }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "Button_Click_1", ex.ToString(), sender, e);
            }
            finally
            {
            }
        }
        #endregion
    }
}
