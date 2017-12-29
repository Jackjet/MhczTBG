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
using System.Collections.Generic;
using System.Reflection;

namespace MhczTBG.Helper
{
    public class CustomTestInformation : INotifyPropertyChanged
    {
        public List<CustomTestInformation> GetList()
        {
            customerUnitInformation();
            return CustomerUnitList;
        }

        public List<CustomTestInformation> CustomerUnitList = new List<CustomTestInformation>();
        /// 往集合中添加一个元素
        /// <summary>
        /// 往集合中添加一个元素
        /// </summary>
        /// <typeparam name="T">未知类型</typeparam>
        /// <param name="list">list集合</param>
        /// <param name="args">元素的属性值</param>
        internal void DataGridRowAdd<T>(List<T> list, params object[] args)
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


        ///给客户单位(CustomerUnit)添加数据
        /// <summary>
        /// 添加数据
        /// </summary>
        void customerUnitInformation()
        {
            //添加一条信息
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "1", "周杰伦-龙卷风", "Image/7204055403m614341.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "2", "周杰伦-断了的弦", "Image/7204055403m614341.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "3", "周杰伦-威廉古堡", "Image/7204055403m614341.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "4", "周杰伦-七里香", "Image/7204055403m614341.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "5", "周杰伦-完美主义", "Image/7204055403m614341.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "6", "周杰伦-爷爷泡的茶", "Image/7204055403m614341.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "7", "周杰伦-爱你没差", "Image/142957zRbuc.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "8", "周杰伦-半兽人", "Image/142957zRbuc.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "9", "周杰伦-爱在西元前", "Image/21034324-1_o.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "10", "周杰伦-东风破", "Image/OOOPIC_jiangjingrong_20090330c1b174614c888cee.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "11", "周杰伦-半岛铁盒", "Image/2561296_082755038979_2.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "12", "周杰伦-简单爱", "Image/2561296_082755038979_2.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "13", "周杰伦-斗牛", "Image/2561296_082755038979_2.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "14", "周杰伦-龙卷风", "Image/7204055403m614341.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "15", "周杰伦-断了的弦", "Image/7204055403m614341.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "16", "周杰伦-威廉古堡", "Image/7204055403m614341.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "17", "周杰伦-七里香", "Image/7204055403m614341.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "18", "周杰伦-完美主义", "Image/7204055403m614341.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "19", "周杰伦-爷爷泡的茶", "Image/7204055403m614341.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "20", "周杰伦-爱你没差", "Image/142957zRbuc.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "21", "周杰伦-半兽人", "Image/142957zRbuc.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "22", "周杰伦-爱在西元前", "Image/21034324-1_o.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "23", "周杰伦-东风破", "Image/OOOPIC_jiangjingrong_20090330c1b174614c888cee.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "24", "周杰伦-半岛铁盒", "Image/2561296_082755038979_2.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "25", "周杰伦-简单爱", "Image/2561296_082755038979_2.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "26", "周杰伦-斗牛", "Image/2561296_082755038979_2.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "27", "周杰伦-龙卷风", "Image/7204055403m614341.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "28", "周杰伦-断了的弦", "Image/7204055403m614341.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "29", "周杰伦-威廉古堡", "Image/7204055403m614341.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "30", "周杰伦-七里香", "Image/7204055403m614341.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "31", "周杰伦-完美主义", "Image/7204055403m614341.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "32", "周杰伦-爷爷泡的茶", "Image/7204055403m614341.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "33", "周杰伦-爱你没差", "Image/142957zRbuc.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "34", "周杰伦-半兽人", "Image/142957zRbuc.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "35", "周杰伦-爱在西元前", "Image/21034324-1_o.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "36", "周杰伦-东风破", "Image/OOOPIC_jiangjingrong_20090330c1b174614c888cee.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "37", "周杰伦-半岛铁盒", "Image/2561296_082755038979_2.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "38", "周杰伦-简单爱", "Image/2561296_082755038979_2.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "39", "周杰伦-斗牛", "Image/2561296_082755038979_2.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "40", "周杰伦-龙卷风", "Image/7204055403m614341.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "41", "周杰伦-断了的弦", "Image/7204055403m614341.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "42", "周杰伦-威廉古堡", "Image/7204055403m614341.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "43", "周杰伦-七里香", "Image/7204055403m614341.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "44", "周杰伦-完美主义", "Image/7204055403m614341.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "45", "周杰伦-爷爷泡的茶", "Image/7204055403m614341.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "46", "周杰伦-爱你没差", "Image/142957zRbuc.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "47", "周杰伦-半兽人", "Image/142957zRbuc.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "48", "周杰伦-爱在西元前", "Image/21034324-1_o.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "49", "周杰伦-东风破", "Image/OOOPIC_jiangjingrong_20090330c1b174614c888cee.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "50", "周杰伦-半岛铁盒", "Image/2561296_082755038979_2.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "51", "周杰伦-简单爱", "Image/2561296_082755038979_2.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "52", "周杰伦-斗牛", "Image/2561296_082755038979_2.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "53", "周杰伦-龙卷风", "Image/7204055403m614341.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "54", "周杰伦-断了的弦", "Image/7204055403m614341.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "周杰伦", "周杰伦-威廉古堡", "Image/7204055403m614341.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "郑源", "周杰伦-七里香", "Image/7204055403m614341.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "周杰伦", "周杰伦-完美主义", "Image/7204055403m614341.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "郑源", "周杰伦-爷爷泡的茶", "Image/7204055403m614341.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "周杰伦", "周杰伦-爱你没差", "Image/142957zRbuc.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "周杰伦", "周杰伦-半兽人", "Image/142957zRbuc.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "郑源", "周杰伦-爱在西元前", "Image/21034324-1_o.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "周杰伦", "周杰伦-东风破", "Image/OOOPIC_jiangjingrong_20090330c1b174614c888cee.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "郑源", "周杰伦-半岛铁盒", "Image/2561296_082755038979_2.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "周杰伦", "周杰伦-简单爱", "Image/2561296_082755038979_2.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "周杰伦", "周杰伦-斗牛", "Image/2561296_082755038979_2.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "周杰伦", "周杰伦-龙卷风", "Image/7204055403m614341.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "郑源", "周杰伦-断了的弦", "Image/7204055403m614341.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "周杰伦", "周杰伦-威廉古堡", "Image/7204055403m614341.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "郑源", "周杰伦-七里香", "Image/7204055403m614341.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "周杰伦", "周杰伦-完美主义", "Image/7204055403m614341.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "郑源", "周杰伦-爷爷泡的茶", "Image/7204055403m614341.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "周杰伦", "周杰伦-爱你没差", "Image/142957zRbuc.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "周杰伦", "周杰伦-半兽人", "Image/142957zRbuc.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "郑源", "周杰伦-爱在西元前", "Image/21034324-1_o.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "周杰伦", "周杰伦-东风破", "Image/OOOPIC_jiangjingrong_20090330c1b174614c888cee.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "郑源", "周杰伦-半岛铁盒", "Image/2561296_082755038979_2.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "周杰伦", "周杰伦-简单爱", "Image/2561296_082755038979_2.jpg", "04:40", "6.42Mb");
            this.DataGridRowAdd<CustomTestInformation>(CustomerUnitList, "周杰伦", "周杰伦-斗牛", "Image/2561296_082755038979_2.jpg", "04:40", "6.42Mb");
        }
        string singer = string.Empty;
        public string Singer
        {
            get
            {
                return singer;
            }
            set
            {
                if (this.singer == value) return;
                this.singer = value;
                Notify("Name");
            }
        }

        string musicName = string.Empty;
        public string MusicName
        {
            get
            {
                return musicName;
            }
            set
            {
                if (this.musicName == value) return;
                this.musicName = value;
                Notify("MusicName");
            }
        }

        string mv = string.Empty;
        public string Mv
        {
            get
            {
                return mv;
            }
            set
            {
                if (this.mv == value) return;
                this.mv = value;
                Notify("Mv");
            }
        }

        string musicTime = string.Empty;
        public string MusicTime
        {
            get
            {
                return musicTime;
            }
            set
            {
                if (this.musicTime == value) return;
                this.musicTime = value;
                Notify("MusicTime");
            }
        }

        string musicSize = string.Empty;
        public string MusicSize
        {
            get
            {
                return musicSize;
            }
            set
            {
                if (this.musicSize == value) return;
                this.musicSize = value;
                Notify("MusicSize");
            }
        }

        string popular = string.Empty;
        public string Popular
        {
            get
            {
                return popular;
            }
            set
            {
                if (this.popular == value) return;
                this.popular = value;
                Notify("Popular");
            }
        }

        string operate = string.Empty;
        public string Operate
        {
            get
            {
                return operate;
            }
            set
            {
                if (this.operate == value) return;
                this.operate = value;
                Notify("Operate");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify(string propName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
