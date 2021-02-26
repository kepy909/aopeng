using Newtonsoft.Json.Linq;
using System;
using System.Windows.Forms;

namespace aopeng
{
	public class PCProgram
	{
		private Timer timer = new Timer();

		private WebHelper web = new WebHelper();

		private string bust = PCProgram.GetTimeStamp();

		private string nowCourseID = "";

		public string Pkid = "";

		public string Remark = "";

		private static bool isUpdate = false;

		public PCProgram(string _pkid, string _reamrk, bool _isUpdate)
		{
			this.Pkid = _pkid;
			this.Remark = _reamrk;
			PCProgram.isUpdate = _isUpdate;
		}

		private void SaveXapi(object obj, EventArgs e)
		{
			bool flag = string.IsNullOrEmpty(this.web.Tcookie) || string.IsNullOrEmpty(this.nowCourseID);
			if (!flag)
			{
				this.web.PC_Post(string.Format(PC_URLS.Host_SaveXapi, this.nowCourseID), "url=/StudentCenter/MyCourse/MyCourseDetail?CourseID=" + this.nowCourseID + "&amp;CourseIndex=0&timestep=120", "");
			}
		}

		public bool Login(string userName, string userPwd)
		{
			string data = string.Concat(new string[]
			{
				"loginName=",
				userName,
				"&passWord=",
				userPwd,
				"&validateNum=&black_box=e3Y6ICIyLjUuMCIsIG9zOiAid2ViIiwgczogMTk5LCBlOiAianMgbm90IGRvd25sb2FkIn0="
			});
			HttpResult httpResult = this.web.PC_Post(PC_URLS.Host_UnitLogin + "?bust=" + this.bust, data, "");
			bool flag = !httpResult.Html.Contains("status");
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				JObject jObject = JObject.Parse(httpResult.Html);
				bool flag2 = int.Parse(jObject["status"].ToString()) != 0;
				if (flag2)
				{
					Console.WriteLine(jObject["message"].ToString());
					result = false;
				}
				else
				{
					HttpResult httpResult2 = this.web.PC_GET(string.Format(PC_URLS.Host_GetPointRuleDetails, this.bust), httpResult.Cookie);
					JArray jArray = JArray.Parse(JObject.Parse(httpResult2.Html)["data"].ToString());
					for (int i = 0; i < jArray.Count; i++)
					{
						string text = jArray[i]["PointItemName"].ToString();
						string text2 = jArray[i]["RuleExplain"].ToString();
						int num = int.Parse(jArray[i]["PointsNum"].ToString());
					}
					result = true;
				}
			}
			return result;
		}

		private void GetCourseWareJsonByRs(string courseid)
		{
			this.timer.Start();
			string html = this.web.PC_GET(string.Format(PC_URLS.Host_GetCourseWareJsonByRs, this.bust, courseid), "").Html;
			JObject jObject = JObject.Parse(html);
			JArray jArray = JArray.Parse(jObject["data"]["payload"]["children"].ToString());
			for (int i = 0; i < jArray.Count; i++)
			{
				JArray jArray2 = JArray.Parse(jArray[i]["children"].ToString());
				for (int j = 0; j < jArray2.Count; j++)
				{
					JArray jArray3 = JArray.Parse(jArray2[j]["children"].ToString());
					for (int k = 0; k < jArray3.Count; k++)
					{
						string dataid = jArray3[k]["id"].ToString();
						string dataname = jArray3[k]["name"].ToString();
						this.SaveLog(courseid, dataid, dataname);
					}
				}
			}
		}

		private void SaveLog(string courseid, string dataid, string dataname)
		{
			string data = string.Concat(new string[]
			{
				"courseid=",
				courseid,
				"&dataid=",
				dataid,
				"&datatype=0&dataname=",
				dataname.toUrl()
			});
			string html = this.web.PC_Post(string.Format(PC_URLS.Host_SaveLog, this.bust), data, "").Html;
		}

		private decimal GetProcess(string courseId)
		{
			string html = this.web.PC_GET(string.Format(PC_URLS.Host_GetCourseDetail, this.bust, courseId), "").Html;
			JObject jObject = JObject.Parse(html);
			return decimal.Parse(jObject["data"]["Progerss"].ToString());
		}

		public static string GetTimeStamp()
		{
			DateTime now = DateTime.Now;
			return PCProgram.ConvertDateTimeToInt(now).ToString();
		}

		private static long ConvertDateTimeToInt(DateTime time)
		{
			DateTime dateTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
			return (time.Ticks - dateTime.Ticks) / 10000L;
		}
	}
}
