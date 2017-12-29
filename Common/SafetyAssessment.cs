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
using System.ComponentModel;

namespace MhczTBG.Common
{
    public class SafetyAssessment 
    {
        
        string _time;
        /// <summary>
        /// 日期时间
        /// </summary>
        public string Time
        {
            get { return _time; }
            set { _time = value; }
        }

        string _cause;
        /// <summary>
        /// 原因分析
        /// </summary>
        public string Cause 
        {
            get { return _cause; }
            set { _cause = value; } 
        }

        string _trainEffect;
        /// <summary>
        /// 影响列车
        /// </summary>
        public string TrainEffect
        {
            get { return _trainEffect; }
            set { _trainEffect = value; }
        }  

        string _delayTime;
        /// <summary>
        /// 延迟时间
        /// </summary>
        public string DelayTime
        {
            get { return _delayTime; }
            set { _delayTime = value; }
        }    
       
        string _section;
        /// <summary>
        /// 段定
        /// </summary>
        public string Section
        {
            get { return _section; }
            set { _section = value; }
        }
     
        string _bureau;
        /// <summary>
        /// 局定
        /// </summary>
        public string Bureau
        {
            get { return _bureau; }
            set { _bureau = value; }
        }

        string _check;
        /// <summary>
        /// 考核
        /// </summary>
        public string Check
        {
            get { return _check; }
            set { _check = value; }
        }

        /// <summary>
        /// 创建一条安全故障信息
        /// </summary>
        /// <param name="time">日期时间</param>
        /// <param name="cause">原因分析</param>
        /// <param name="trainEffect">列车影响</param>
        /// <param name="delayTime">延迟时间</param>
        /// <param name="section">段定</param>
        /// <param name="bureau">局定</param>
        /// <param name="check">考核</param>
        public SafetyAssessment(string time,string cause,string trainEffect,string delayTime,string section,string bureau,string check)
        {
            this._time = time;
            this._cause = cause;
            this._trainEffect = trainEffect;
            this._delayTime = delayTime;
            this._section = section;
            this._bureau = bureau;
            this._check = check;
        }     
    }
}
