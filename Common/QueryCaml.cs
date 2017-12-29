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

namespace MhczTBG.Common
{
    public class QueryCaml
    {
        #region 构造函数
        public QueryCaml() { }

        public QueryCaml(string filedName1, string type1, string values1, string rule1)
        {
            FiledName = filedName1;
            Type = type1;
            if (filedName1.Contains("startData") || filedName1.Contains("EndData"))
            {
                Values = values1.Replace('/', '-').Replace('.', '-').Replace('年', '-').Replace('月', '-').Replace("日", "");

            }
            else
                values = values1;

            Rule = rule1;
        }
        #endregion

        #region 实体类
        string filedName;
        /// <summary>
        /// 字段名称
        /// </summary>
        public string FiledName
        {
            get { return filedName; }
            set { filedName = value; }
        }


        string type;
        /// <summary>
        /// 查询字段的类型
        /// </summary>
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        string values;
        /// <summary>
        /// 查询字段的值
        /// </summary>
        public string Values
        {
            get { return values; }
            set { values = value; }
        }

        string rule;
        /// <summary>
        /// 查询字段的规则
        /// </summary>
        public string Rule
        {
            get { return rule; }
            set { rule = value; }
        }
        #endregion

        #region 类型转换
        //通过字典目录得到list  可以通过_替换解决具体的字段名
        public List<QueryCaml> dirToListCamll(Dictionary<string, string> camldir)
        {
            //FiledName = filedName1;
            //Type = type1;
            //Values = values1;
            //Rule = rule1;
            List<QueryCaml> dirList = new List<QueryCaml>();
            foreach (var item in camldir)
            {
                dirList.Add(new QueryCaml(item.Key.ToString().Replace("EndData", "startData")
                    .Replace("YanShi_", "YanShi")
                    .Replace("_JingJiSunShi", "JingJiSunShi")
                    .Replace("_ZeRenDanWeiChengDanSunShiFeiYong", "ZeRenDanWeiChengDanSunShiFeiYong"),
                    item.Value.Split(new char[] { '#' })[1],
                    item.Value.Split(new char[] { '#' })[0].Remove(item.Value.Split(new char[] { '#' })[0].LastIndexOf(",")),
                    item.Value.Split(new char[] { '#' })[2]));
            }
            return dirList;


        }
        #endregion

        public string GetQuery(List<QueryCaml> chaxuncaml)
        {
            string query = "";
            string 条件and = "And";
            string 条件or = "Or";
            string 组合true = " Group='True' ";

            #region  有一个条件



            if (chaxuncaml.Count == 1)
            {


                foreach (QueryCaml item in chaxuncaml)
                {
                    if (item.Values.Split(new char[] { ',' }).Length == 1)
                    {
                        query += "<" + item.Rule + "><FieldRef Name='" + item.FiledName + "'  /><Value Type='" + item.Type + "'>" + item.Values.Split(new char[] { ',' })[0] + "</Value></" + item.Rule + ">";
                    }
                    else if (item.Values.Split(new char[] { ',' }).Length == 2)
                    {
                        string querytwo = "";
                        for (int i = 0; i < item.Values.Split(new char[] { ',' }).Length; i++)
                        {
                            querytwo += "<" + item.Rule + "><FieldRef Name='" + item.FiledName + "'  /><Value Type='" + item.Type + "'>" + item.Values.Split(new char[] { ',' })[i] + "</Value></" + item.Rule + ">";
                        }
                        query = "<" + 条件and + ">" + querytwo + "</" + 条件and + ">";
                    }
                    else if (item.Values.Split(new char[] { ',' }).Length > 2)
                    {
                        //前两个id的链接
                        string querythree = "";
                        string newmessage2 = "";
                        for (int i = 0; i < 2; i++)
                        {
                            newmessage2 += "<" + item.Rule + "><FieldRef Name='" + item.FiledName + "'  /><Value Type='" + item.Type + "'>" + item.Values.Split(new char[] { ',' })[i] + "</Value></" + item.Rule + ">";
                        }
                        querythree = "<" + 条件and + ">" + newmessage2 + "</" + 条件and + ">";

                        string query条件 = "";
                        string newmessage3 = "";
                        for (int j = 2; j < item.Values.Split(new char[] { ',' }).Length; j++)
                        {
                            query条件 += "<" + 条件and + ">";
                            newmessage3 += "<" + item.Rule + "><FieldRef Name='" + item.FiledName + "'  /><Value Type='" + item.Type + "'>" + item.Values.Split(new char[] { ',' })[j] + "</Value></" + item.Rule + "></" + 条件and + ">";

                        }
                        query = query条件 + querythree + newmessage3;
                    }
                }
            }
            #endregion

            #region 有两个条件


            else if (chaxuncaml.Count == 2)
            {
                foreach (QueryCaml item in chaxuncaml)
                {

                    //一个value的时候
                    if (item.Values.Split(new char[] { ',' }).Length == 1)
                    {
                        query += "<" + item.Rule + "><FieldRef Name='" + item.FiledName + "'  /><Value Type='" + item.Type + "'>" + item.Values.Split(new char[] { ',' })[0] + "</Value></" + item.Rule + ">";
                    }
                    //两个value值的时候
                    else if (item.Values.Split(new char[] { ',' }).Length == 2)
                    {
                        string querytwo = "";
                        for (int i = 0; i < item.Values.Split(new char[] { ',' }).Length; i++)
                        {
                            querytwo += "<" + item.Rule + "><FieldRef Name='" + item.FiledName + "'  /><Value Type='" + item.Type + "'>" + item.Values.Split(new char[] { ',' })[i] + "</Value></" + item.Rule + ">";
                        }
                        query += "<" + 条件or + 组合true + ">" + querytwo + "</" + 条件or + ">";

                    }
                    //三个value值的时候
                    else if (item.Values.Split(new char[] { ',' }).Length > 2)
                    {
                        string querythree = "";
                        string newmessage2 = "";
                        for (int i = 0; i < 2; i++)
                        {
                            newmessage2 += "<" + item.Rule + "><FieldRef Name='" + item.FiledName + "'  /><Value Type='" + item.Type + "'>" + item.Values.Split(new char[] { ',' })[i] + "</Value></" + item.Rule + ">";
                        }
                        querythree = "<" + 条件or + ">" + newmessage2 + "</" + 条件or + ">";

                        string query条件 = "";
                        string newmessage3 = "";
                        for (int j = 2; j < item.Values.Split(new char[] { ',' }).Length; j++)
                        {
                            if (j == 2)
                            {
                                query条件 += "<" + 条件or + 组合true + ">";
                            }
                            else
                            {
                                query条件 += "<" + 条件or + ">";
                            }

                            newmessage3 += "<" + item.Rule + "><FieldRef Name='" + item.FiledName + "'  /><Value Type='" + item.Type + "'>" + item.Values.Split(new char[] { ',' })[j] + "</Value></" + item.Rule + "></" + 条件or + ">";

                        }
                        query += query条件 + querythree + newmessage3;
                    }
                }
                query = "<" + 条件and + ">" + query + "</" + 条件and + ">";

            }
            #endregion

            #region 两个以上


            else if (chaxuncaml.Count > 2)
            {
                for (int i = 0; i < 2; i++)
                {
                    query = 条件分解(chaxuncaml, query, 条件or, 组合true, i);

                }
                query = "<" + 条件and + ">" + query + "</" + 条件and + ">";

                for (int i = 2; i < chaxuncaml.Count; i++)
                {
                    query = 条件分解(chaxuncaml, query, 条件or, 组合true, i);
                    query = "<" + 条件and + ">" + query + "</" + 条件and + ">";
                }

            }
            #endregion

            //return "<Where>" + query + "</Where>";
            return query;
        }
        /// <summary>
        /// 执行排序
        /// </summary>
        /// <param name="chaxuncaml"></param>
        /// <param name="descField"></param>
        /// <returns></returns>
        public string GetQuery(List<QueryCaml> chaxuncaml, string descField)
        {
            string query = "";
            string 条件and = "And";
            string 条件or = "Or";
            string 组合true = " Group='True' ";

            #region  有一个条件



            if (chaxuncaml.Count == 1)
            {


                foreach (QueryCaml item in chaxuncaml)
                {
                    if (item.Values.Split(new char[] { ',' }).Length == 1)
                    {
                        query += "<" + item.Rule + "><FieldRef Name='" + item.FiledName + "'  /><Value Type='" + item.Type + "'>" + item.Values.Split(new char[] { ',' })[0] + "</Value></" + item.Rule + ">";
                    }
                    else if (item.Values.Split(new char[] { ',' }).Length == 2)
                    {
                        string querytwo = "";
                        for (int i = 0; i < item.Values.Split(new char[] { ',' }).Length; i++)
                        {
                            querytwo += "<" + item.Rule + "><FieldRef Name='" + item.FiledName + "'  /><Value Type='" + item.Type + "'>" + item.Values.Split(new char[] { ',' })[i] + "</Value></" + item.Rule + ">";
                        }
                        query = "<" + 条件and + ">" + querytwo + "</" + 条件and + ">";
                    }
                    else if (item.Values.Split(new char[] { ',' }).Length > 2)
                    {
                        //前两个id的链接
                        string querythree = "";
                        string newmessage2 = "";
                        for (int i = 0; i < 2; i++)
                        {
                            newmessage2 += "<" + item.Rule + "><FieldRef Name='" + item.FiledName + "'  /><Value Type='" + item.Type + "'>" + item.Values.Split(new char[] { ',' })[i] + "</Value></" + item.Rule + ">";
                        }
                        querythree = "<" + 条件and + ">" + newmessage2 + "</" + 条件and + ">";

                        string query条件 = "";
                        string newmessage3 = "";
                        for (int j = 2; j < item.Values.Split(new char[] { ',' }).Length; j++)
                        {
                            query条件 += "<" + 条件and + ">";
                            newmessage3 += "<" + item.Rule + "><FieldRef Name='" + item.FiledName + "'  /><Value Type='" + item.Type + "'>" + item.Values.Split(new char[] { ',' })[j] + "</Value></" + item.Rule + "></" + 条件and + ">";

                        }
                        query = query条件 + querythree + newmessage3;
                    }
                }
            }
            #endregion

            #region 有两个条件


            else if (chaxuncaml.Count == 2)
            {
                foreach (QueryCaml item in chaxuncaml)
                {

                    //一个value的时候
                    if (item.Values.Split(new char[] { ',' }).Length == 1)
                    {
                        query += "<" + item.Rule + "><FieldRef Name='" + item.FiledName + "'  /><Value Type='" + item.Type + "'>" + item.Values.Split(new char[] { ',' })[0] + "</Value></" + item.Rule + ">";
                    }
                    //两个value值的时候
                    else if (item.Values.Split(new char[] { ',' }).Length == 2)
                    {
                        string querytwo = "";
                        for (int i = 0; i < item.Values.Split(new char[] { ',' }).Length; i++)
                        {
                            querytwo += "<" + item.Rule + "><FieldRef Name='" + item.FiledName + "'  /><Value Type='" + item.Type + "'>" + item.Values.Split(new char[] { ',' })[i] + "</Value></" + item.Rule + ">";
                        }
                        query += "<" + 条件or + 组合true + ">" + querytwo + "</" + 条件or + ">";

                    }
                    //三个value值的时候
                    else if (item.Values.Split(new char[] { ',' }).Length > 2)
                    {
                        string querythree = "";
                        string newmessage2 = "";
                        for (int i = 0; i < 2; i++)
                        {
                            newmessage2 += "<" + item.Rule + "><FieldRef Name='" + item.FiledName + "'  /><Value Type='" + item.Type + "'>" + item.Values.Split(new char[] { ',' })[i] + "</Value></" + item.Rule + ">";
                        }
                        querythree = "<" + 条件or + ">" + newmessage2 + "</" + 条件or + ">";

                        string query条件 = "";
                        string newmessage3 = "";
                        for (int j = 2; j < item.Values.Split(new char[] { ',' }).Length; j++)
                        {
                            if (j == 2)
                            {
                                query条件 += "<" + 条件or + 组合true + ">";
                            }
                            else
                            {
                                query条件 += "<" + 条件or + ">";
                            }

                            newmessage3 += "<" + item.Rule + "><FieldRef Name='" + item.FiledName + "'  /><Value Type='" + item.Type + "'>" + item.Values.Split(new char[] { ',' })[j] + "</Value></" + item.Rule + "></" + 条件or + ">";

                        }
                        query += query条件 + querythree + newmessage3;
                    }
                }
                query = "<" + 条件and + ">" + query + "</" + 条件and + ">";

            }
            #endregion

            #region 两个以上


            else if (chaxuncaml.Count > 2)
            {
                for (int i = 0; i < 2; i++)
                {
                    query = 条件分解(chaxuncaml, query, 条件or, 组合true, i);

                }
                query = "<" + 条件and + ">" + query + "</" + 条件and + ">";

                for (int i = 2; i < chaxuncaml.Count; i++)
                {
                    query = 条件分解(chaxuncaml, query, 条件or, 组合true, i);
                    query = "<" + 条件and + ">" + query + "</" + 条件and + ">";
                }

            }
            #endregion
            return "<Where>" + query + "</Where>"
                     + "<OrderBy>"
                     + "<FieldRef Name='startData' Ascending='FALSE'/>"
                     + "</OrderBy>";


        }

        private static string 条件分解(List<QueryCaml> chaxuncaml, string query, string 条件or, string 组合true, int i)
        {
            if (chaxuncaml[i].Values.Split(new char[] { ',' }).Length == 1)
            {
                query += "<" + chaxuncaml[i].Rule + "><FieldRef Name='" + chaxuncaml[i].FiledName + "'  /><Value Type='" + chaxuncaml[i].Type + "'>" + chaxuncaml[i].Values.Split(new char[] { ',' })[0] + "</Value></" + chaxuncaml[i].Rule + ">";
            }
            //两个value值的时候
            else if (chaxuncaml[i].Values.Split(new char[] { ',' }).Length == 2)
            {
                string querytwo = "";
                for (int j = 0; j < chaxuncaml[i].Values.Split(new char[] { ',' }).Length; j++)
                {
                    querytwo += "<" + chaxuncaml[i].Rule + "><FieldRef Name='" + chaxuncaml[i].FiledName + "'  /><Value Type='" + chaxuncaml[i].Type + "'>" + chaxuncaml[i].Values.Split(new char[] { ',' })[j] + "</Value></" + chaxuncaml[i].Rule + ">";
                }
                query += "<" + 条件or + 组合true + ">" + querytwo + "</" + 条件or + ">";
            }

            //三个value值的时候
            else if (chaxuncaml[i].Values.Split(new char[] { ',' }).Length > 2)
            {
                string querythree = "";
                string newmessage2 = "";
                for (int k = 0; k < 2; k++)
                {
                    newmessage2 += "<" + chaxuncaml[i].Rule + "><FieldRef Name='" + chaxuncaml[i].FiledName + "'  /><Value Type='" + chaxuncaml[i].Type + "'>" + chaxuncaml[i].Values.Split(new char[] { ',' })[k] + "</Value></" + chaxuncaml[i].Rule + ">";
                }
                querythree = "<" + 条件or + ">" + newmessage2 + "</" + 条件or + ">";

                string query条件 = "";
                string newmessage3 = "";
                for (int m = 2; m < chaxuncaml[i].Values.Split(new char[] { ',' }).Length; m++)
                {
                    if (m == 2)
                    {
                        query条件 += "<" + 条件or + 组合true + ">";
                    }
                    else
                    {
                        query条件 += "<" + 条件or + ">";
                    }

                    newmessage3 += "<" + chaxuncaml[i].Rule + "><FieldRef Name='" + chaxuncaml[i].FiledName + "'  /><Value Type='" + chaxuncaml[i].Type + "'>" + chaxuncaml[i].Values.Split(new char[] { ',' })[m] + "</Value></" + chaxuncaml[i].Rule + "></" + 条件or + ">";

                }
                query += query条件 + querythree + newmessage3;
            }
            return query;
        }

    }
}
