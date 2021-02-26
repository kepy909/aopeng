using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aopeng
{
    public class URLS
    {
        public static string Host_loginByPassword = "http://api.open.com.cn/bff-tongxue/api/v1/loginByPassword";
        public static string Host_ensureCreat = "http://api.open.com.cn/bff-tongxue/api/v1/ensureCreat";
        public static string Host_studyCourses = "http://api.open.com.cn/bff-tongxue/api/v1/studyCourse";
        public static string Host_studyCourseDetail = "http://api.open.com.cn/bff-tongxue/api/v1/studyCourseDetail?id={0}&sourceId=&type=1";
        public static string Host_playHistory = "http://api.open.com.cn/bff-tongxue/api/v1/playHistory";
        public static string Host_courseClick = "http://api.open.com.cn/bff-tongxue/api/v1/courseClick";
        public static string Host_PlayHistory = "http://api.open.com.cn/bff-tongxue/api/v1/playHistory";

        public static string Host_CourseOnlineTime = "http://api.open.com.cn/bff-tongxue/api/v1/courseOnlineTime";
        public static string Hsot_learningDurations = "http://api.open.com.cn/bff-tongxue/api/v1/learningDurations";
        public static string Host_videoProgress = "http://api.open.com.cn/bff-tongxue/api/v1/videoProgress";
        public static string Host_studyUserXapi = "http://api.open.com.cn/bff-tongxue/api/v1/studyUserXapi?courseId={0}&xapiType=0";
        public static string Host_data_collection = "https://xapi.open.com.cn/api/v1/students/data-collection";
    }
    public class PC_URLS
    {
        public static string Host_SaveXapi = "http://learn.open.com.cn/Common/Statistics/SaveXapi?url=/StudentCenter/MyCourse/MyCourseDetail?CourseID={0}&amp;CourseIndex=0&timestep=120";
        public static string Host_UnitLogin = "http://learn.open.com.cn/Account/UnitLogin";
        public static string Host_PC = "http://learn.open.com.cn";
        public static string Host_GetMyCourse = "http://learn.open.com.cn/StudentCenter/MyCourse/GetMyCourse?bust={0}&StatusCode=1";
        public static string Host_GetCourseDetail = "http://learn.open.com.cn/StudentCenter/User/GetCourseDetail?bust={0}&courseId={1}&statusCode=1";
        public static string Host_GetCourseWareJsonByRs = "http://learn.open.com.cn/StudentCenter/MyCourse/GetCourseWareJsonByRs?bust={0}&courseid={1}";
        public static string Host_SaveLog = "http://learn.open.com.cn/StudentCenter/CourseWare/SaveLog?bust={0}";
        public static string Host_GetPointRuleDetails = "http://learn.open.com.cn/StudentCenter/Points/GetPointRuleDetails?bust={0}";

    }
}
