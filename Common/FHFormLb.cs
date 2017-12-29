using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Media;

namespace MhczTBG.Common
{
    public class FHFormLb
    {
        #region 变量

        string _ID = string.Empty;
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        string _线别 = string.Empty;
        public string 线别
        {
            get { return _线别; }
            set { _线别 = value; }
        }
    
        string _设备厂家 = string.Empty;
        public string 设备厂家
        {
            get { return _设备厂家; }
            set { _设备厂家 = value; }
        }
       

        string _障碍设备名称 = string.Empty;
        public string 障碍设备名称
        {
            get { return _障碍设备名称; }
            set { _障碍设备名称 = value; }
        }

        string _工区 = string.Empty;
        public string 工区
        {
            get { return _工区; }
            set { _工区 = value; }
        }

        string _责任单位 = string.Empty;
        public string 责任单位
        {
            get { return _责任单位; }
            set { _责任单位 = value; }
        }

        string _障碍现象 = string.Empty;
        public string 障碍现象
        {
            get { return _障碍现象; }
            set { _障碍现象 = value; }
        }

        string _影响范围 = string.Empty;
        public string 影响范围
        {
            get { return _影响范围; }
            set { _影响范围 = value; }
        }

        string _原因分析 = string.Empty;
        public string 原因分析
        {
            get { return _原因分析; }
            set { _原因分析 = value; }
        }


        string _故障地点 = string.Empty;
        public string 故障地点
        {
            get { return _故障地点; }
            set { _故障地点 = value; }
        }

        string _是否报局 = string.Empty;
        public string 是否报局
        {
            get { return _是否报局; }
            set { _是否报局 = value; }
        }

        string _段定 = string.Empty;
        public string 段定
        {
            get { return _段定; }
            set { _段定 = value; }
        }

        string _局定 = string.Empty;
        public string 局定
        {
            get { return _局定; }
            set { _局定 = value; }
        }

        string _数据类型 = string.Empty;
        public string 数据类型
        {
            get { return _数据类型; }
            set { _数据类型 = value; }
        }

        string _定性 = string.Empty;
        public string 定性
        {
            get { return _定性; }
            set { _定性 = value; }
        }

        string _定责 = string.Empty;
        public string 定责
        {
            get { return _定责; }
            set { _定责 = value; }
        }

        string _处理单位 = string.Empty;
        public string 处理单位
        {
            get { return _处理单位; }
            set { _处理单位 = value; }
        }

        string _受理日期时间 = string.Empty;

        public string 受理日期时间
        {
            get { return _受理日期时间; }
            set { _受理日期时间 = value; }
        }

        string _故障发生日期时间 = string.Empty;
        public string 故障发生日期时间
        {
            get { return _故障发生日期时间; }
            set { _故障发生日期时间 = value; }
        }

        string _延时 = string.Empty;
        public string 延时
        {
            get { return _延时; }
            set { _延时 = value; }
        }

        string _经济损失 = string.Empty;
        public string 经济损失
        {
            get { return _经济损失; }
            set { _经济损失 = value; }
        }

        string _是否有附件 = string.Empty;
        public string 是否有附件
        {
            get { return _是否有附件; }
            set { _是否有附件 = value; }
        }

        string _考核意见 = string.Empty;
        public string 考核意见
        {
            get { return _考核意见; }
            set { _考核意见 = value; }
        }

        string _附件 = string.Empty;
        //uri
        public string 附件
        {
            get { return _附件; }
            set { _附件 = value; }
        }
     
        string _影响客车次 = string.Empty;
        public string 影响客车次
        {
            get { return _影响客车次; }
            set { _影响客车次 = value; }
        }

        string _影响客车数 = string.Empty;
        public string 影响客车数
        {
            get { return _影响客车数; }
            set { _影响客车数 = value; }
        }

        string _影响货车次 = string.Empty;
        public string 影响货车次
        {
            get { return _影响货车次; }
            set { _影响货车次 = value; }
        }

        string _影响货车数 = string.Empty;
        public string 影响货车数
        {
            get { return _影响货车数; }
            set { _影响货车数 = value; }
        }

        string _故障处理经过 = string.Empty;
        public string 故障处理经过
        {
            get { return _故障处理经过; }
            set { _故障处理经过 = value; }
        }

        string _设备类型一级 = string.Empty;
        public string 设备类型一级
        {
            get { return _设备类型一级; }
            set { _设备类型一级 = value; }
        }

        string _设备类型二级 = string.Empty;
        public string 设备类型二级
        {
            get { return _设备类型二级; }
            set { _设备类型二级 = value; }
        }

        string _故障恢复日期时间 = string.Empty;
        public string 故障恢复日期时间
        { 
            get { return _故障恢复日期时间; }
            set { _故障恢复日期时间 = value; }
        }

        Brush _责任单位背景 = new SolidColorBrush(Colors.Transparent);
        public Brush 责任单位背景
        {
            get { return _责任单位背景; }
            set { _责任单位背景 = value; }
        }

        #endregion

        #region 构造函数

        public FHFormLb()
        {

        }

        public FHFormLb(Dictionary<string, object> dicForm)
        {
             try
            {
            if (dicForm.Keys.Contains("ID"))
            {
                if (dicForm["ID"] != null)
                    this._ID = Convert.ToString(dicForm["ID"]);
                else this._ID = string.Empty;
            }
            if (dicForm.Keys.Contains("Line"))
            {
                if (dicForm["Line"] != null)
                    this._线别 = Convert.ToString(dicForm["Line"]);
                else this._线别 = string.Empty;
            }
            if (dicForm.Keys.Contains("GuZhangSheBeiMingCheng"))
            {
                if (dicForm["GuZhangSheBeiMingCheng"] != null)
                    this._障碍设备名称 = Convert.ToString(dicForm["GuZhangSheBeiMingCheng"]);
                else this._障碍设备名称 = string.Empty;

            }         
            if (dicForm.Keys.Contains("GongQu"))
            {
                if (dicForm["GongQu"] != null)
                    this._工区 = Convert.ToString(dicForm["GongQu"]);
                else this._工区 = string.Empty;
            }
            if (dicForm.Keys.Contains("SheBeiChangJia"))
            {
                if (dicForm["SheBeiChangJia"] != null)
                    this._设备厂家 = Convert.ToString(dicForm["SheBeiChangJia"]);
                else this._设备厂家 = string.Empty;
            }
            if (dicForm.Keys.Contains("CheJianMingCheng"))
            {
                if (dicForm["CheJianMingCheng"] != null)
                    this._责任单位 = Convert.ToString(dicForm["CheJianMingCheng"]);
                else this._责任单位 = string.Empty;
            }
            if (dicForm.Keys.Contains("ZhangAiXianXiang"))
            {
                if (dicForm["ZhangAiXianXiang"] != null)
                    this._障碍现象 = Convert.ToString(dicForm["ZhangAiXianXiang"]);
                else this._障碍现象 = string.Empty;
            }
            if (dicForm.Keys.Contains("YingXiangFanWei"))
            {
                if (dicForm["YingXiangFanWei"] != null)
                    this._影响范围 = Convert.ToString(dicForm["YingXiangFanWei"]);
                else this._影响范围 = string.Empty;
            }
            if (dicForm.Keys.Contains("YuanYingFenXi"))
            {
                if (dicForm["YuanYingFenXi"] != null)
                    this._原因分析 = Convert.ToString(dicForm["YuanYingFenXi"]);
                else this._原因分析 = string.Empty;
            }
            if (dicForm.Keys.Contains("KaoHeYiJian"))
            {
                if (dicForm["KaoHeYiJian"] != null)
                    this._考核意见 = Convert.ToString(dicForm["KaoHeYiJian"]);
                else this._考核意见 = string.Empty;
            }
            if (dicForm.Keys.Contains("YanShi"))
            {
                if (dicForm["YanShi"] != null)
                    this._延时 = Convert.ToString(dicForm["YanShi"]);
                else this._延时 = string.Empty;
            }
            if (dicForm.Keys.Contains("YingXiangKeCheCi"))
            {
                if (dicForm["YingXiangKeCheCi"] != null)
                    this._影响客车次 = Convert.ToString(dicForm["YingXiangKeCheCi"]);
                else this._影响客车次 = string.Empty;
            }
            if (dicForm.Keys.Contains("YingXiangKeCheShu"))
            {
                if (dicForm["YingXiangKeCheShu"] != null)
                    this._影响客车数 = Convert.ToString(dicForm["YingXiangKeCheShu"]);
                else this._影响客车数 = string.Empty;
            }
            if (dicForm.Keys.Contains("YingXiangHuoCheCi"))
            {
                if (dicForm["YingXiangHuoCheCi"] != null)
                    this._影响货车次 = Convert.ToString(dicForm["YingXiangHuoCheCi"]);
                else this._影响货车次 = string.Empty;
            }
            if (dicForm.Keys.Contains("YingXiangHuoCheShu"))
            {
                if (dicForm["YingXiangHuoCheShu"] != null)
                    this._影响货车数 = Convert.ToString(dicForm["YingXiangHuoCheShu"]);
                else this._影响货车数 = string.Empty;
            }
            if (dicForm.Keys.Contains("GuZhangChuLiJingGuo"))
            {
                if (dicForm["GuZhangChuLiJingGuo"] != null)
                    this._故障处理经过 = Convert.ToString(dicForm["GuZhangChuLiJingGuo"]);
                else this._故障处理经过 = string.Empty;
            }

            if (dicForm.Keys.Contains("DiDian"))
            {
                if (dicForm["DiDian"] != null)
                    this._故障地点 = Convert.ToString(dicForm["DiDian"]);
                else this._故障地点 = string.Empty;
            }
            if (dicForm.Keys.Contains("JingJiSunShi"))
            {
                if (dicForm["JingJiSunShi"] != null)
                    this._经济损失 = Convert.ToString(dicForm["JingJiSunShi"]);
                else this._经济损失 = string.Empty;
            }
            if (dicForm.Keys.Contains("ShiFouBaoJu"))
            {
                if (dicForm["ShiFouBaoJu"] != null)
                    this._是否报局 = Convert.ToString(dicForm["ShiFouBaoJu"]);
                else this._是否报局 = string.Empty;
            }
            if (dicForm.Keys.Contains("DuanDing"))
            {
                if (dicForm["DuanDing"] != null)
                    this._段定 = Convert.ToString(dicForm["DuanDing"]);
                else this._段定 = string.Empty;
            }
            if (dicForm.Keys.Contains("JuDing"))
            {
                if (dicForm["JuDing"] != null)
                    this._局定 = Convert.ToString(dicForm["JuDing"]);
                else this._局定 = string.Empty;
            }
            if (dicForm.Keys.Contains("SheBeiTypeYiJi"))
            {
                if (dicForm["SheBeiTypeYiJi"] != null)
                    this._设备类型一级 = Convert.ToString(dicForm["SheBeiTypeYiJi"]);
                else this._设备类型一级 = string.Empty;
            }
            if (dicForm.Keys.Contains("SheBeiTypeErJi"))
            {
                if (dicForm["SheBeiTypeErJi"] != null)
                    this._设备类型二级 = Convert.ToString(dicForm["SheBeiTypeErJi"]);
                else this._设备类型二级 = string.Empty;
            }
            if (dicForm.Keys.Contains("EndData"))
            {
                if (dicForm["EndData"] != null)
                    this._故障恢复日期时间 = Convert.ToString(dicForm["EndData"]);
                else this._故障恢复日期时间 = string.Empty;
            }
            if (dicForm.Keys.Contains("ShuJuLeiXing"))
            {
                if (dicForm["ShuJuLeiXing"] != null)
                    this._数据类型 = Convert.ToString(dicForm["ShuJuLeiXing"]);
                else this._数据类型 = string.Empty;
            }
            if (dicForm.Keys.Contains("DingXing"))
            {
                if (dicForm["DingXing"] != null)
                    this._定性 = Convert.ToString(dicForm["DingXing"]);
                else this._定性 = string.Empty;
            }
            if (dicForm.Keys.Contains("DingZe"))
            {
                if (dicForm["DingZe"] != null)
                    this._定责 = Convert.ToString(dicForm["DingZe"]);
                else this._定责 = string.Empty;
            }
            if (dicForm.Keys.Contains("chuliDanWei"))
            {
                if (dicForm["chuliDanWei"] != null)
                    this._处理单位 = Convert.ToString(dicForm["chuliDanWei"]);
                else this._处理单位 = string.Empty;
            }
            if (dicForm.Keys.Contains("ShouLiTime"))
            {
                if (dicForm["ShouLiTime"] != null)
                    this._受理日期时间 = Convert.ToString(dicForm["ShouLiTime"]);
                else this._受理日期时间 = string.Empty;
            }
            if (dicForm.Keys.Contains("startData"))
            {
                if (dicForm["startData"] != null)
                    this._故障发生日期时间 = Convert.ToString(dicForm["startData"]);
                else this._故障发生日期时间 = string.Empty;
            }
            if (dicForm.Keys.Contains("Attachments"))
            {
                if (dicForm["Attachments"] != null)
                    this._是否有附件 = Convert.ToString(dicForm["Attachments"]);
                else this._是否有附件 = string.Empty;
            }
  }
            catch (Exception ex)
            {
                MethodLb.CreateLog(this.GetType().FullName, "FHFormLb", ex.ToString(),dicForm);              
            }
        }

        #endregion     
    }
}
