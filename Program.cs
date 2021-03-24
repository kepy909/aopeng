using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.Threading;
using System.Data;

namespace aopeng
{
    class Program
    {
        static WebHelper web = new WebHelper();
        static int dayOnlieTime = 0;//5分钟算1分，一天最多30分，也就是150分钟，也就是150次Post数据包
        static int dayCourseClick = 0;//1次课件点击2分，一天对多10次点击

        static string userName = "";
        static string userPwd = "";

        static void Main(string[] args)
        {


            Console.WriteLine("请输入账号后回车");
            userName = Console.ReadLine();//账号
            Console.WriteLine("请输入密码后回车");
            userPwd = Console.ReadLine();//密码
            Console.WriteLine("开始登陆");
            try
            {
                Login(userName, userPwd);
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

        /// <summary>
        /// 课程在线时长
        /// </summary>
        /// <param name="CourseId"></param>
        static int CourseOnlineTime(string CourseId)
        {
            JObject jObject = new JObject(
                new JProperty("CourseId", CourseId),
                      new JProperty("timesecond", 60000)
                );
            string Requset = web.Post(URLS.Host_CourseOnlineTime, JsonConvert.SerializeObject(jObject), true); ;
            Console.WriteLine("60秒心跳返回结果:" + Requset);
            return 1;
        }
        static void Login(string userName, string userPwd)
        {
            string deviceId = "6e13ee0d5-075a-42e7-8aa6-7r3d25222223d";
            string agreementId = "100008";

            JObject jObject = new JObject();
            jObject.Add(new JProperty("deviceId", deviceId));
            jObject.Add(new JProperty("username", userName));
            jObject.Add(new JProperty("password", userPwd));
            jObject.Add(new JProperty("agreementId", agreementId));
            string Requset = web.Post(URLS.Host_loginByPassword, JsonConvert.SerializeObject(jObject));
            JObject jLogin = JObject.Parse(Requset);
            if (int.Parse(jLogin["code"].ToString()) != 300)
            {
                Console.WriteLine(jLogin["msg"].ToString());
            }
            else
            {
                JArray juserInfor = JArray.Parse(jLogin["userInfor"].ToString());
                for (int a = 0; a < juserInfor.Count; a++)
                {

                    string StudentCode = juserInfor[a]["StudentCode"].ToString();
                    JObject jBody = new JObject();
                    jBody.Add("deviceId", deviceId);
                    jBody.Add("ucUserId", jLogin["ucUserId"].ToString());
                    jBody.Add("agreementId", agreementId);
                    jBody.Add("wxInfor", "");
                    jBody.Add(
                        new JProperty("userInfo", new JArray(new JObject(
                            new JProperty("StudentCode", StudentCode),
                             new JProperty("UniversityCode", juserInfor[a]["UniversityCode"].ToString()),
                             new JProperty("UniversityName", juserInfor[a]["UniversityName"].ToString()),
                               new JProperty("BatchCode", juserInfor[a]["BatchCode"].ToString()),
                                new JProperty("BatchName", juserInfor[a]["BatchName"].ToString()),
                                 new JProperty("LevelName", juserInfor[a]["LevelName"].ToString()),
                                  new JProperty("SpecialityName", juserInfor[a]["SpecialityName"].ToString()),
                                   new JProperty("checked", true)
                            ))));
                    string Info = JsonConvert.SerializeObject(jBody);
                    Requset = web.Post(URLS.Host_ensureCreat, Info);
                    jObject = JObject.Parse(Requset);
                    string ucUserId = jObject["ucUserId"].ToString();
                    string token = jObject["token"].ToString();
                    web.Token = token;
                    GetClassCount(StudentCode);
                }
            }


        }


        static void GetClassCount(string StudentCode)
        {
            string Requset = web.Get(URLS.Host_studyCourses, true);
            JArray jArray = JArray.Parse(JObject.Parse(Requset)["list"].ToString());
            for (int a = 0; a < jArray.Count; a++)
            {
                string CourseId = jArray[a]["CourseId"].ToString();//43604

                string studyUserXapi = web.Get(string.Format(URLS.Host_studyUserXapi, CourseId));
                JObject jstudyUserXapi = JObject.Parse(studyUserXapi);
                string userId = jstudyUserXapi["userId"].ToString();
                string signature = jstudyUserXapi["signature"].ToString();
                string signatureNonce = jstudyUserXapi["signatureNonce"].ToString();
                string appkey = jstudyUserXapi["appkey"].ToString();
                string platform = jstudyUserXapi["platform"].ToString();
                string terminalinfo = jstudyUserXapi["terminalinfo"].ToString().Replace(@"\s+", "+");
                string electiveCourseId = jstudyUserXapi["electiveCourseId"].ToString();
                string timestamp = jstudyUserXapi["timestamp"].ToString();
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("signature", signature);
                dic.Add("signatureNonce", signatureNonce);
                dic.Add("appkey", appkey);
                dic.Add("platform", platform);
                //T17:11:41Z
                dic.Add("timestamp", DateTime.Parse(timestamp).ToString("yyyy-MM-ddTHH:mm:ssZ"));



                string CourseName = jArray[a]["CourseName"].ToString();
                string studyCourseDetail = web.Get(string.Format(URLS.Host_studyCourseDetail, CourseId));
                JObject jojstudyCourseDetail = JObject.Parse(studyCourseDetail);
                string orgId = jojstudyCourseDetail["orgId"].ToString();//10145
                string StudentCourseId = jojstudyCourseDetail["StudentCourseId"].ToString();//73504611
                string schoolName = jojstudyCourseDetail["schoolName"].ToString();//东北大学
                string videoFirstImg = jojstudyCourseDetail["videoFirstImg"].ToString();//http://file.open.com.cn/Lms/CourseImg/28.jpg

                if (!JsonConvert.SerializeObject(jojstudyCourseDetail).Contains("coursewareId"))
                {
                    string _name = jojstudyCourseDetail["name"].ToString();//得用播放器才能播放的课程
                    continue;
                }
                string coursewareId = jojstudyCourseDetail["tree"]["coursewareId"].ToString();//bb9e375b-f777-4382-9d9a-3a22ccf9436b
                JArray jstudyCourseDetail = JArray.Parse(jojstudyCourseDetail["tree"]["children"].ToString());
                for (int b = 0; b < jstudyCourseDetail.Count; b++)
                {

                    //课件点击积分
                    dayCourseClick += CourseClick(CourseId, coursewareId);
                    string tname = jstudyCourseDetail[b]["name"].ToString();//大学英语(三)
                    JArray jAchildren = JArray.Parse(jstudyCourseDetail[b]["children"].ToString());
                    for (int c = 0; c < jAchildren.Count; c++)
                    {
                        string name = jAchildren[c]["name"].ToString();//第001讲unit 1 Listen and Talk
                        Console.WriteLine("正在学习:" + name);
                        JArray jAchildrenchildren = JArray.Parse(jAchildren[c]["children"].ToString());
                        for (int d = 0; d < jAchildrenchildren.Count; d++)
                        {
                            if (jAchildrenchildren[d]["children"].ToString() != "[]")
                            {
                                JArray jAchildrenchildrenchildren = JArray.Parse(jAchildrenchildren[d]["children"].ToString());
                                for (int e = 0; e < jAchildrenchildrenchildren.Count; e++)
                                {
                                    string namee = jAchildrenchildrenchildren[e]["name"].ToString();//第001讲unit 1 Listen and Talk
                                    Console.WriteLine("正在学习:" + namee);
                                    JArray jAchildrenchildrenchildrenchildren = JArray.Parse(jAchildrenchildrenchildren[e]["children"].ToString());
                                    for (int f = 0; f < jAchildrenchildrenchildrenchildren.Count; f++)
                                    {
                                        string resourceId = jAchildrenchildrenchildrenchildren[f]["id"].ToString();

                                        string duration = "60";
                                        double breakPoint = 0;
                                        if (jAchildrenchildrenchildrenchildren[f].ToString().Contains("duration"))
                                            duration = jAchildrenchildrenchildrenchildren[f]["duration"].ToString();
                                        if (jAchildrenchildrenchildrenchildren[f].ToString().Contains("breakPoint"))
                                            double.TryParse(jAchildrenchildrenchildrenchildren[f]["breakPoint"].ToString(), out breakPoint);

                                        duration = duration.Length > 0 ? (float.Parse(duration) / 1000).ToString("f2") : "0";
                                        double dduration = double.Parse(duration);

                                        if (breakPoint + 20 >= dduration) continue;
                                        Console.WriteLine("总时长:" + duration.ToString() + " 已学:" + breakPoint.ToString());
                                        while (dduration > breakPoint)
                                        {

                                            breakPoint += 60;
                                            //课程在线时长
                                            dayOnlieTime += CourseOnlineTime(CourseId);
                                            if (dayOnlieTime >= 150)
                                            {
                                                Console.WriteLine("今日已完成在线积分");
                                                if (dayCourseClick >= 20)
                                                {
                                                    return;
                                                }
                                            }


                                            //课程进度
                                            string videoProgress = web.Post(URLS.Host_videoProgress, "type=2&data=" + Convert.ToInt32(breakPoint) + "&resourceID=" + resourceId + "&courseID=" + CourseId);
                                            Console.WriteLine("videoProgress Request:" + videoProgress);
                                            //学习时长
                                            string LearningDurations = web.Post(URLS.Hsot_learningDurations, "durations=60");
                                            Console.WriteLine("LearningDurations Request:" + LearningDurations);
                                            int coursetype = int.Parse(jAchildrenchildrenchildrenchildren[f]["type"].ToString());
                                            //播放记录
                                            coursetype = 2;
                                            int resourceType = 1;
                                            JObject jExteend = new JObject(
                                                new JProperty("orgId", orgId),
                                                new JProperty("StudentCourseId", StudentCourseId));
                                            string extend = JsonConvert.SerializeObject(jExteend);
                                            string postDate = $"courseId={CourseId}&courseName={CourseName.toUrl()}&firstImg={videoFirstImg.toUrl()}&schoolName={schoolName.toUrl()}&coursewareId={coursewareId}&resourceId={resourceId}&resourceName={name.toUrl()}&duration={duration}&courseType={coursetype}&resourceType={resourceType}&extend={extend}";
                                            Requset = web.Post(URLS.Host_PlayHistory, postDate);
                                            Console.WriteLine("PlayHistory Requset:" + Requset);

                                            //关键数据包，发送当前学习进度
                                            JObject jdata = new JObject();
                                            jdata.Add(new JProperty("actor", new JObject(new JProperty("extensions", new JObject(new JProperty("studentStatusId", StudentCode), new JProperty("organizationId", orgId))), new JProperty("actorId", userId), new JProperty("actorName", ""), new JProperty("actorType", 1))));
                                            jdata.Add(new JProperty("objectInfo", new JObject(new JProperty("objId", resourceId), new JProperty("objType", "video"), new JProperty("objName", name), new JProperty("extensions", ""))));
                                            jdata.Add(new JProperty("verbInfo", new JObject(new JProperty("verbType", "set"), new JProperty("extensions", new JObject(new JProperty("verbCategory", "pause"))))));
                                            jdata.Add(new JProperty("context", new JObject(new JProperty("ip", "192.168.0.1"), new JProperty("terminalInfo", terminalinfo), new JProperty("terminalType", 2),
                                                new JProperty("extensions", new JObject(new JProperty("coursewareId", coursewareId), new JProperty("resourcesId", resourceId), new JProperty("courseId", CourseId),
                                                new JProperty("electiveCourseId", electiveCourseId), new JProperty("time", Convert.ToInt32(breakPoint)))))));
                                            jdata.Add(new JProperty("result", ""));
                                            jdata.Add(new JProperty("actionTimestamp", GetTimeStamp(false)));

                                            string jsonData = JsonConvert.SerializeObject(jdata);
                                            string data_collection =
                                                web.Post_end(URLS.Host_data_collection, "jsonData=" + jsonData, dic);
                                            Console.WriteLine("data-collection Requset:" + data_collection);

                                        }


                                    }
                                }
                            }
                            else
                            {
                                string resourceId = jAchildrenchildren[d]["id"].ToString();

                                string duration = "60";
                                double breakPoint = 0;
                                if (jAchildrenchildren[d].ToString().Contains("duration"))
                                    duration = jAchildrenchildren[d]["duration"].ToString();
                                if (jAchildrenchildren[d].ToString().Contains("breakPoint"))
                                    double.TryParse(jAchildrenchildren[d]["breakPoint"].ToString(), out breakPoint);

                                duration = duration.Length > 0 ? (float.Parse(duration) / 1000).ToString("f2") : "0";
                                double dduration = double.Parse(duration);

                                if (breakPoint + 20 >= dduration) continue;
                                Console.WriteLine("总时长:" + duration.ToString() + " 已学:" + breakPoint.ToString());
                                while (dduration > breakPoint)
                                {

                                    breakPoint += 60;
                                    //课程在线时长
                                    dayOnlieTime += CourseOnlineTime(CourseId);
                                    if (dayOnlieTime >= 150)
                                    {
                                        Console.WriteLine("今日已完成在线积分");
                                        if (dayCourseClick >= 20)
                                        {
                                            return;
                                        }
                                    }


                                    //课程进度
                                    string videoProgress = web.Post(URLS.Host_videoProgress, "type=2&data=" + Convert.ToInt32(breakPoint) + "&resourceID=" + resourceId + "&courseID=" + CourseId);
                                    Console.WriteLine("videoProgress Request:" + videoProgress);
                                    //学习时长
                                    string LearningDurations = web.Post(URLS.Hsot_learningDurations, "durations=60");
                                    Console.WriteLine("LearningDurations Request:" + LearningDurations);
                                    int coursetype = int.Parse(jAchildrenchildren[d]["type"].ToString());
                                    //播放记录
                                    coursetype = 2;
                                    int resourceType = 1;
                                    JObject jExteend = new JObject(
                                        new JProperty("orgId", orgId),
                                        new JProperty("StudentCourseId", StudentCourseId));
                                    string extend = JsonConvert.SerializeObject(jExteend);
                                    string postDate = $"courseId={CourseId}&courseName={CourseName.toUrl()}&firstImg={videoFirstImg.toUrl()}&schoolName={schoolName.toUrl()}&coursewareId={coursewareId}&resourceId={resourceId}&resourceName={name.toUrl()}&duration={duration}&courseType={coursetype}&resourceType={resourceType}&extend={extend}";
                                    Requset = web.Post(URLS.Host_PlayHistory, postDate);
                                    Console.WriteLine("PlayHistory Requset:" + Requset);

                                    //关键数据包，发送当前学习进度
                                    JObject jdata = new JObject();
                                    jdata.Add(new JProperty("actor", new JObject(new JProperty("extensions", new JObject(new JProperty("studentStatusId", StudentCode), new JProperty("organizationId", orgId))), new JProperty("actorId", userId), new JProperty("actorName", ""), new JProperty("actorType", 1))));
                                    jdata.Add(new JProperty("objectInfo", new JObject(new JProperty("objId", resourceId), new JProperty("objType", "video"), new JProperty("objName", name), new JProperty("extensions", ""))));
                                    jdata.Add(new JProperty("verbInfo", new JObject(new JProperty("verbType", "set"), new JProperty("extensions", new JObject(new JProperty("verbCategory", "pause"))))));
                                    jdata.Add(new JProperty("context", new JObject(new JProperty("ip", "192.168.0.1"), new JProperty("terminalInfo", terminalinfo), new JProperty("terminalType", 2),
                                        new JProperty("extensions", new JObject(new JProperty("coursewareId", coursewareId), new JProperty("resourcesId", resourceId), new JProperty("courseId", CourseId),
                                        new JProperty("electiveCourseId", electiveCourseId), new JProperty("time", Convert.ToInt32(breakPoint)))))));
                                    jdata.Add(new JProperty("result", ""));
                                    jdata.Add(new JProperty("actionTimestamp", GetTimeStamp(false)));

                                    string jsonData = JsonConvert.SerializeObject(jdata);
                                    string data_collection =
                                        web.Post_end(URLS.Host_data_collection, "jsonData=" + jsonData, dic);
                                    Console.WriteLine("data-collection Requset:" + data_collection);
                                }
                            }
                        }
                    }
                }

            }

        }
        /// <summary>
        /// 学习新视频前的点击动作
        /// </summary>
        /// <param name="Courseid"></param>
        /// <param name="CourseWareID"></param>
        static int CourseClick(string Courseid, string CourseWareID)
        {
            JObject jObject = new JObject(
                new JProperty("Courseid", Courseid),
                new JProperty("CourseWareID", CourseWareID)
                );
            string Requset = web.Post(URLS.Host_courseClick, JsonConvert.SerializeObject(jObject), true);
            Console.WriteLine("课件点击:" + Requset);

            if (dayCourseClick >= 20)
                Console.WriteLine("今日课件点击积分已完成");
            return 1;
        }

        static string GetTimeStamp(bool bflag)
        {
            //真返回10位
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string ret = string.Empty;
            if (bflag)
                ret = Convert.ToInt64(ts.TotalSeconds).ToString();
            else
                ret = Convert.ToInt64(ts.TotalMilliseconds).ToString();
            return ret;
        }
        static void Data_Acollection()
        {


        }
    }
}
