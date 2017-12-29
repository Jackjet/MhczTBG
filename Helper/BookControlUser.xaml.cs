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
using System.Reflection;
using System.Text;

using Microsoft.CSharp;
using MhczTBG.Common;

namespace MhczTBG.Helper
{
    public partial class BookControlUser : UserControl
    {
        //故障信息集合
        List<SafetyAssessment> safetyAssessmentList = new List<SafetyAssessment>();
        public BookControlUser()
        {
            InitializeComponent();

            //故障实例
            SafetyAssessment assessment1 = new SafetyAssessment("3月25日,22:02分", "由于S2线沙河站动力源开关电源控制模块故障,导致48v直流电压不稳,造成633M传输设备掉电脱管,影响沙河站闭塞带电话及自动电话", "影响3趟客车", "延时1小时45分", "经研究定怀北车间行车设备故障", "局定行车设备故障外转至中兴通讯公司50%考核通信段", "因此，考核车间15分，车间正职50分：考核段领导班子3分，有线科4分，其他科室3分");
            SafetyAssessment assessment2 = new SafetyAssessment("4月1日17:15分", "由于枣强县城局对京九线K303+540铁路桥涵两侧施工时未提前通天之路我段，导致京九电气化新20芯光路被铲伤，造成京九电气化电气化58-59号（枣强站）基站光路中断，衡水一清河城中兴S385传输中断。", "影响枣强、大营镇、清河城，大葛村分区所牵引远动，", "延时54分", "经研究定衡水车间非责任二类障碍。", "", "考核车间1分，车间正职3分");

            //添加故障元素
            safetyAssessmentList.Add(assessment1);
            safetyAssessmentList.Add(assessment2);
            //遍历生成故障信息承载体
            foreach (var item in safetyAssessmentList)
            {
                AssessmentControl contro = new AssessmentControl(item);
                this.LayoutRoot.Children.Add(contro);

            }
            //添加
            for (int i = 0; i < 5; i++)
            {
                AssessmentControl contro = new AssessmentControl(assessment2);
                this.LayoutRoot.Children.Add(contro);
            }
            //导入按钮（oob模式）
            Button btnWord = new Button()
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
                Cursor = Cursors.Hand,
                VerticalAlignment = System.Windows.VerticalAlignment.Bottom,
                Width = 80,
                Height = 23,
                Content = "载入word",
                Margin = new Thickness(0, 0, 70, 20),
            };
            //添加按钮
            this.gridMain.Children.Add(btnWord);

            //导入事件
            btnWord.Click += (object sender, RoutedEventArgs e) =>
                {
                    //判断是否为oob模式
                    if (Window.GetWindow(this) != null)
                    {
                        ////创建word应用程序
                        //dynamic word = AutomationFactory.CreateObject("Word.Application");
                        ////可见
                        //word.Visible = true;
                        ////添加文档
                        //dynamic doc = word.Documents.Add();
                        ////
                        //dynamic range = doc.Range(0, 0);
                        ////遍历给word文档添加故障信息
                        //for (int i = 0; i < InformationCollection.builderCollection.Count; i++)
                        //{
                        //    range.Text += string.Format("{0} . ", i + 1);
                        //    //InformationCollection,故障信息集中
                        //    range.Text += InformationCollection.builderCollection[i];
                        //    //word换行
                        //    range.Text += "\r\n";
                        //    range.Text += "\r\n";
                        //}
                    }
                };
        }
    }
}
