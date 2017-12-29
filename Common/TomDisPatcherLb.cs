using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;

namespace MhczTBG.Common
{
    public class TomDisPatcherLb : DispatcherTimer
    {
        #region 连续使用
        
        /// <summary>
        /// 构造指定时间的计时器
        /// </summary>
        /// <param name="intTimer"></param>
        public TomDisPatcherLb(int intTimer)
        {
            this.Interval = TimeSpan.FromSeconds(intTimer);
        }

        /// <summary>
        /// 通过这个方法来安排所要执行的顺序，控件将及时显示所有更改
        /// </summary>
        /// <param name="action"></param>
        public void TomTick(Action action)
        {           
            this.Tick += (object snede, EventArgs ee) =>
            {
                action();              
                this.Stop();
            };
            this.Start();          
        }

        #endregion

        #region 构造进行时2（单独使用）

        /// <summary>
        /// 构造关联委托的计时器
        /// </summary>
        /// <param name="action"></param>
        public TomDisPatcherLb(Action action)
        {
            this.Interval = TimeSpan.FromMilliseconds(500);            
            this.Tick += (object snede, EventArgs ee) =>
                {
                    action();
                    this.Stop();
                };
        }

        /// <summary>
        ///  构造关联委托并指定时间间隔的计时器
        /// </summary>
        /// <param name="intTimer"></param>
        /// <param name="action"></param>
        public TomDisPatcherLb(double intTimer, Action action)
        {
            this.Interval = TimeSpan.FromSeconds(intTimer);
            this.Tick += (object snede, EventArgs ee) =>
            {
                action();
                this.Stop();
            };
        }

        #endregion        
    }    
}