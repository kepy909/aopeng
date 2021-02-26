using System;

namespace aopeng
{
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
