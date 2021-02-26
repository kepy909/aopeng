using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Windows.Forms;



namespace aopeng
{
  public  class PCProgram
    {
        Timer timer = new Timer();
        WebHelper web = new WebHelper();
        string bust = GetTimeStamp();
        string nowCourseID = "";
        public string Pkid = "";
        public string Remark = "";
        static bool isUpdate = false;
        public PCProgram(string _pkid,string _reamrk,bool _isUpdate)
        {
            Pkid = _pkid;
            Remark = _reamrk;
            isUpdate = _isUpdate;
        }

        void SaveXapi(object obj, EventArgs e)
        {
            if (string.IsNullOrEmpty(web.Tcookie) || string.IsNullOrEmpty(nowCourseID))
                return;
            web.PC_Post(string.Format(PC_URLS.Host_SaveXapi, nowCourseID), "url=/StudentCenter/MyCourse/MyCourseDetail?CourseID="+nowCourseID+"&amp;CourseIndex=0&timestep=120");
        }
        public  bool Login(string userName, string userPwd)
        {
            string _data = $"loginName={userName}&passWord={userPwd}&validateNum=&black_box=e3Y6ICIyLjUuMCIsIG9zOiAid2ViIiwgczogMTk5LCBlOiAianMgbm90IGRvd25sb2FkIn0=";
            HttpResult http = web.PC_Post(PC_URLS.Host_UnitLogin+ "?bust="+bust, _data);
            if (!http.Html.Contains("status"))
            {
                return false;
            }
            JObject jObject = JObject.Parse(http.Html);
            if (int.Parse(jObject["status"].ToString())!=0)
            {
                Console.WriteLine(jObject["message"].ToString());
                return false;
            }
            HttpResult httpPonit = web.PC_GET(string.Format(PC_URLS.Host_GetPointRuleDetails, bust),http.Cookie);
            JArray jScoreList = JArray.Parse(JObject.Parse(httpPonit.Html)["data"].ToString());
            for (int i = 0; i < jScoreList.Count; i++)
            {
                string PointItemName = jScoreList[i]["PointItemName"].ToString();
                string RuleExplain= jScoreList[i]["RuleExplain"].ToString();
                int PointsNum = int.Parse(jScoreList[i]["PointsNum"].ToString());
            }

            return true;
        }

        void GetCourseWareJsonByRs(string courseid)
        {
            timer.Start();
            string Requset = web.PC_GET(string.Format(PC_URLS.Host_GetCourseWareJsonByRs, bust, courseid)).Html;
            JObject jObject = JObject.Parse(Requset);
            JArray jArray = JArray.Parse(jObject["data"]["payload"]["children"].ToString());
            for (int a = 0; a < jArray.Count; a++)
            {
                JArray jchildren = JArray.Parse(jArray[a]["children"].ToString());
                for (int b = 0; b < jchildren.Count; b++)
                {
                    JArray jchildrenchildren = JArray.Parse(jchildren[b]["children"].ToString());
                    for (int c = 0; c < jchildrenchildren.Count; c++)
                    {

                        string dataid = jchildrenchildren[c]["id"].ToString();//022b85a1-2dad-4328-9807-babf326db972
                        string dataname= jchildrenchildren[c]["name"].ToString();//第001讲unit 1 Listen and Talk
                        SaveLog(courseid, dataid, dataname);

                    }


                }


            }

        }

        void SaveLog(string courseid,string dataid,string dataname)
        {
            string _data = $"courseid={courseid}&dataid={dataid}&datatype=0&dataname={dataname.toUrl()}";
            string Request = web.PC_Post(string.Format(PC_URLS.Host_SaveLog, bust), _data).Html;
        
        }
        /// <summary>
        /// 获取课程进度
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        private decimal GetProcess(string courseId)
        {
            string Request = web.PC_GET(string.Format(PC_URLS.Host_GetCourseDetail, bust, courseId)).Html;
            JObject jObject = JObject.Parse(Request);
            return decimal.Parse(jObject["data"]["Progerss"].ToString());
        }
        public static string GetTimeStamp()
        {
            System.DateTime time = System.DateTime.Now;
            long ts = ConvertDateTimeToInt(time);
            return ts.ToString();
        }
        private static long ConvertDateTimeToInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;  
            return t;
        }


    }
}
