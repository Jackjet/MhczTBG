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
using System.Text;

namespace MhczTBG.Common
{
    class AssessmentControl : WrapPanel
    {
        /// <summary>
        /// 收集故障信息集合
        /// </summary>
        /// <param name="assessment"></param>
        public AssessmentControl(SafetyAssessment assessment)
        {
            try
            {
                //故障信息之间的空隙
                this.Margin = new Thickness(5, 0, 5, 15);
                //集中故障实例的属性值
                List<string> assessmentAll = new List<string>()
               {
                assessment.Time,
                assessment.Cause,
                assessment.TrainEffect,
                assessment.DelayTime,
                assessment.Section,
                assessment.Bureau,
                assessment.Check,
               };
                //生成一个信息集
                StringBuilder builder = new StringBuilder();
                foreach (var asse in assessmentAll)
                {
                    TextBlock lbl = new TextBlock();
                    lbl.FontSize = 12;
                    lbl.Text = asse;
                    lbl.TextWrapping = TextWrapping.Wrap;
                    this.Children.Add(lbl);
                    //信息集收集信息
                    builder.Append(asse);
                }
                //收集
                InformationCollection.builderCollection.Add(builder);
             }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "AssessmentControl", ex.ToString(), assessment);
            }
            finally
            {
            }
        }
    }

}
