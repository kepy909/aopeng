using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace aopeng
{
	internal class Program
	{
		private static WebHelper web = new WebHelper();

		private static int dayOnlieTime = 0;

		private static int dayCourseClick = 0;

		private static string userName = "";

		private static string userPwd = "";

		private static void Main(string[] args)
		{
			Console.WriteLine("请输入账号后回车");
			Program.userName = Console.ReadLine();
			Console.WriteLine("请输入密码后回车");
			Program.userPwd = Console.ReadLine();
			Console.WriteLine("开始登陆");
			try
			{
				Program.Login(Program.userName, Program.userPwd);
			}
			catch (Exception ex)
			{
				Console.WriteLine("异常:" + ex.Message);
			}
			finally
			{
				Console.WriteLine("运行结束");
				Console.ReadLine();
			}
		}

		private static int CourseOnlineTime(string CourseId)
		{
			JObject value = new JObject(new object[]
			{
				new JProperty("CourseId", CourseId),
				new JProperty("timesecond", 60000)
			});
			string str = Program.web.Post(URLS.Host_CourseOnlineTime, JsonConvert.SerializeObject(value), true);
			Console.WriteLine("60秒心跳返回结果:" + str);
			return 1;
		}

		private static void Login(string userName, string userPwd)
		{
			string text = "6e13ee0d5-075a-42e7-8aa6-7r3d25222223d";
			string text2 = "100008";
			JObject jObject = new JObject();
			jObject.Add(new JProperty("deviceId", text));
			jObject.Add(new JProperty("username", userName));
			jObject.Add(new JProperty("password", userPwd));
			jObject.Add(new JProperty("agreementId", text2));
			string json = Program.web.Post(URLS.Host_loginByPassword, JsonConvert.SerializeObject(jObject), false);
			JObject jObject2 = JObject.Parse(json);
			bool flag = int.Parse(jObject2["code"].ToString()) != 300;
			if (flag)
			{
				Console.WriteLine(jObject2["msg"].ToString());
			}
			else
			{
				JArray jArray = JArray.Parse(jObject2["userInfor"].ToString());
				for (int i = 0; i < jArray.Count; i++)
				{
					string text3 = jArray[i]["StudentCode"].ToString();
					string data = JsonConvert.SerializeObject(new JObject
					{
						{
							"deviceId",
							text
						},
						{
							"ucUserId",
							jObject2["ucUserId"].ToString()
						},
						{
							"agreementId",
							text2
						},
						{
							"wxInfor",
							""
						},
						new JProperty("userInfo", new JArray(new JObject(new object[]
						{
							new JProperty("StudentCode", text3),
							new JProperty("UniversityCode", jArray[i]["UniversityCode"].ToString()),
							new JProperty("UniversityName", jArray[i]["UniversityName"].ToString()),
							new JProperty("BatchCode", jArray[i]["BatchCode"].ToString()),
							new JProperty("BatchName", jArray[i]["BatchName"].ToString()),
							new JProperty("LevelName", jArray[i]["LevelName"].ToString()),
							new JProperty("SpecialityName", jArray[i]["SpecialityName"].ToString()),
							new JProperty("checked", true)
						})))
					});
					json = Program.web.Post(URLS.Host_ensureCreat, data, false);
					jObject = JObject.Parse(json);
					string text4 = jObject["ucUserId"].ToString();
					string token = jObject["token"].ToString();
					Program.web.Token = token;
					Program.GetClassCount(text3);
				}
			}
		}

		private static void GetClassCount(string StudentCode)
		{
			string text = Program.web.Get(URLS.Host_studyCourses, true);
			JArray jArray = JArray.Parse(JObject.Parse(text)["list"].ToString());
			for (int i = 0; i < jArray.Count; i++)
			{
				string text2 = jArray[i]["CourseId"].ToString();
				string json = Program.web.Get(string.Format(URLS.Host_studyUserXapi, text2), false);
				JObject jObject = JObject.Parse(json);
				string content = jObject["userId"].ToString();
				string value = jObject["signature"].ToString();
				string value2 = jObject["signatureNonce"].ToString();
				string value3 = jObject["appkey"].ToString();
				string value4 = jObject["platform"].ToString();
				string content2 = jObject["terminalinfo"].ToString().Replace("\\s+", "+");
				string content3 = jObject["electiveCourseId"].ToString();
				string s = jObject["timestamp"].ToString();
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("signature", value);
				dictionary.Add("signatureNonce", value2);
				dictionary.Add("appkey", value3);
				dictionary.Add("platform", value4);
				dictionary.Add("timestamp", DateTime.Parse(s).ToString("yyyy-MM-ddTHH:mm:ssZ"));
				string str = jArray[i]["CourseName"].ToString();
				string json2 = Program.web.Get(string.Format(URLS.Host_studyCourseDetail, text2), false);
				JObject jObject2 = JObject.Parse(json2);
				string content4 = jObject2["orgId"].ToString();
				string content5 = jObject2["StudentCourseId"].ToString();
				string str2 = jObject2["schoolName"].ToString();
				string str3 = jObject2["videoFirstImg"].ToString();
				bool flag = !JsonConvert.SerializeObject(jObject2).Contains("coursewareId");
				if (flag)
				{
					string text3 = jObject2["name"].ToString();
				}
				else
				{
					string text4 = jObject2["tree"]["coursewareId"].ToString();
					JArray jArray2 = JArray.Parse(jObject2["tree"]["children"].ToString());
					for (int j = 0; j < jArray2.Count; j++)
					{
						Program.dayCourseClick += Program.CourseClick(text2, text4);
						string text5 = jArray2[j]["name"].ToString();
						JArray jArray3 = JArray.Parse(jArray2[j]["children"].ToString());
						for (int k = 0; k < jArray3.Count; k++)
						{
							string text6 = jArray3[k]["name"].ToString();
							Console.WriteLine("正在学习:" + text6);
							JArray jArray4 = JArray.Parse(jArray3[k]["children"].ToString());
							for (int l = 0; l < jArray4.Count; l++)
							{
								string text7 = jArray4[l]["id"].ToString();
								string text8 = "60";
								double num = 0.0;
								bool flag2 = jArray4[l].ToString().Contains("duration");
								if (flag2)
								{
                                    try
                                    {
										text8 = jArray4[l]["duration"].ToString();
										bool flag3 = jArray4[l].ToString().Contains("breakPoint");
										if (flag3)
										{
											double.TryParse(jArray4[l]["breakPoint"].ToString(), out num);
										}
									}
                                    catch (Exception)
                                    {

										text8 = jArray4[l]["children"][0]["duration"].ToString();
										bool flag3 = jArray4[l]["children"][0].ToString().Contains("breakPoint");
										if (flag3)
										{
											double.TryParse(jArray4[l]["children"][0]["breakPoint"].ToString(), out num);
										}
									}
								}
								text8 = ((text8.Length > 0) ? (float.Parse(text8) / 1000f).ToString("f2") : "0");
								double num2 = double.Parse(text8);
								bool flag4 = num + 20.0 >= num2;
								if (!flag4)
								{
									Console.WriteLine("总时长:" + text8.ToString() + " 已学:" + num.ToString());
									while (num2 > num)
									{
										num += 60.0;
										Program.dayOnlieTime += Program.CourseOnlineTime(text2);
										bool flag5 = Program.dayOnlieTime >= 150;
										if (flag5)
										{
											Console.WriteLine("今日已完成在线积分");
											bool flag6 = Program.dayCourseClick >= 20;
											if (flag6)
											{
												return;
											}
										}
										string str4 = Program.web.Post(URLS.Host_videoProgress, string.Concat(new string[]
										{
											"type=2&data=",
											Convert.ToInt32(num).ToString(),
											"&resourceID=",
											text7,
											"&courseID=",
											text2
										}), false);
										Console.WriteLine("videoProgress Request:" + str4);
										string str5 = Program.web.Post(URLS.Hsot_learningDurations, "durations=60", false);
										Console.WriteLine("LearningDurations Request:" + str5);
										int num3 = int.Parse(jArray4[l]["type"].ToString());
										num3 = 2;
										int num4 = 1;
										JObject value5 = new JObject(new object[]
										{
											new JProperty("orgId", content4),
											new JProperty("StudentCourseId", content5)
										});
										string text9 = JsonConvert.SerializeObject(value5);
										string data = string.Format("courseId={0}&courseName={1}&firstImg={2}&schoolName={3}&coursewareId={4}&resourceId={5}&resourceName={6}&duration={7}&courseType={8}&resourceType={9}&extend={10}", new object[]
										{
											text2,
											str.toUrl(),
											str3.toUrl(),
											str2.toUrl(),
											text4,
											text7,
											text6.toUrl(),
											text8,
											num3,
											num4,
											text9
										});
										text = Program.web.Post(URLS.Host_PlayHistory, data, false);
										Console.WriteLine("PlayHistory Requset:" + text);
										string str6 = JsonConvert.SerializeObject(new JObject
										{
											new JProperty("actor", new JObject(new object[]
											{
												new JProperty("extensions", new JObject(new object[]
												{
													new JProperty("studentStatusId", StudentCode),
													new JProperty("organizationId", content4)
												})),
												new JProperty("actorId", content),
												new JProperty("actorName", ""),
												new JProperty("actorType", 1)
											})),
											new JProperty("objectInfo", new JObject(new object[]
											{
												new JProperty("objId", text7),
												new JProperty("objType", "video"),
												new JProperty("objName", text6),
												new JProperty("extensions", "")
											})),
											new JProperty("verbInfo", new JObject(new object[]
											{
												new JProperty("verbType", "set"),
												new JProperty("extensions", new JObject(new JProperty("verbCategory", "pause")))
											})),
											new JProperty("context", new JObject(new object[]
											{
												new JProperty("ip", "192.168.0.1"),
												new JProperty("terminalInfo", content2),
												new JProperty("terminalType", 2),
												new JProperty("extensions", new JObject(new object[]
												{
													new JProperty("coursewareId", text4),
													new JProperty("resourcesId", text7),
													new JProperty("courseId", text2),
													new JProperty("electiveCourseId", content3),
													new JProperty("time", Convert.ToInt32(num))
												}))
											})),
											new JProperty("result", ""),
											new JProperty("actionTimestamp", Program.GetTimeStamp(false))
										});
										string str7 = Program.web.Post_end(URLS.Host_data_collection, "jsonData=" + str6, dictionary);
										Console.WriteLine("data-collection Requset:" + str7);
									}
								}
							}
						}
					}
				}
			}
		}

		private static int CourseClick(string Courseid, string CourseWareID)
		{
			JObject value = new JObject(new object[]
			{
				new JProperty("Courseid", Courseid),
				new JProperty("CourseWareID", CourseWareID)
			});
			string str = Program.web.Post(URLS.Host_courseClick, JsonConvert.SerializeObject(value), true);
			Console.WriteLine("课件点击:" + str);
			bool flag = Program.dayCourseClick >= 20;
			if (flag)
			{
				Console.WriteLine("今日课件点击积分已完成");
			}
			return 1;
		}

		private static string GetTimeStamp(bool bflag)
		{
			TimeSpan timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
			string result = string.Empty;
			if (bflag)
			{
				result = Convert.ToInt64(timeSpan.TotalSeconds).ToString();
			}
			else
			{
				result = Convert.ToInt64(timeSpan.TotalMilliseconds).ToString();
			}
			return result;
		}

		private static void Data_Acollection()
		{
		}
	}
}
