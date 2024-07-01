using LogInDemo.Models;
using Microsoft.AspNetCore.Mvc;

/**
 * 管理员维护学生信息
 * 
 * 
 */
namespace LogInDemo.Controllers
{
	public class CourseController : Controller
	{
		public IActionResult Index()
		{
			//返回默认参数,表示寻找与控制器相同名称的视图进行返回
			return View();
		}
		//视图通常以ViewResult的形式从操作返回,这是一种ActionResult类型.
		public IActionResult ShowAllCourse()
		{
			ViewData["UserName"] = HttpContext.Session.GetString("UserName");
			using(var db = new _2114010313DbContext())
			{
				List<Course> courses = db.Courses.ToList();
                //使用强类型数据,将 viewmodel 类型的实例传递给此操作的视图
                return View(courses);
			}
		}
		public IActionResult CourseEdit(String id)
		{
			if(string.IsNullOrEmpty(id))
			{
				return View();
			}
			//查询课程列表
			using (var db = new _2114010313DbContext())
			{
				var course = db.Courses.Find(id);
				return View(course);
			}
		}
		public IActionResult CourseDelete(String id)
		{
			if(string.IsNullOrEmpty(id))
			{
				return View();
			}
			using(var db=new _2114010313DbContext())
			{
				var course=db.Courses.Find(id);
				return View(course);
			}
		}
		public IActionResult SaveCourse(Course course)
		{
			using(var db = new _2114010313DbContext()) 
			{
				var model = db.Courses.Find(course.CourseId);
				if(model == null)
				{
					model = new Course();
					model.CourseId = course.CourseId;
					model.CourseName = course.CourseName;
					model.CourseCredit = course.CourseCredit;
					db.Courses.Add(model);
				}
				else
				{
					model.CourseName = course.CourseName;
					model.CourseCredit = course.CourseCredit;
				}
				db.SaveChanges();
			}

			return RedirectToAction("ShowAllCourse", "Course");
		}
		public IActionResult DelateCourse(Course course)
		{
			using(var db = new _2114010313DbContext())
			{
				var model = db.Courses.Find(course.CourseId);
				var student = db.Students.ToList();
				//遍历每一个学生,如果有一个学生选课都退出循环,返回提示信息
				foreach(var studentId in  student)
				{
					var CourseSelected = db.Stucourse1s.Find(studentId.StudentId,course.CourseId);
					if (CourseSelected != null)
					{
						TempData["DeleteFailedMessage"] = "有学生选择这门课,不允许删除";
						return RedirectToAction("ShowAllCourse", "Course");
					}
				}
				if (model == null)
				{
					return View();
				}
				else
				{
					db.Courses.Remove(model);
				}
				db.SaveChanges();
			}

			return RedirectToAction("ShowAllCourse", "Course");
		}

		public IActionResult ShowAvailableCourse(Course course)
		{
			using (var  db = new _2114010313DbContext())
			{
				return View(course);
			}
		}
		public IActionResult AddNewCourse(Course course)
		{
			return View(course);
		}

		// 添加课程
		public IActionResult AddCourse(Course course)
		{
			using (var db = new _2114010313DbContext())
			{
				if (db.Courses.Contains(course))
				{
					
					//这里不能使用viewdata,因为viewdata在网页重定向以后就被刷新掉了
					//使用TempData，它允许数据在一次重定向后保留。同时还要注意不要使用已经使用过的消息名,否则会造成显示错误
					TempData["FailedAddMessage"] = "该课程已经存在,不能继续添加";
					return RedirectToAction("ShowAllCourse", "Course");
				}
				else if (course.CourseId == null || course.CourseCredit == null || course.CourseName == null)
				{
					//如果添加的课程信息不完整,也会输出报错信息.
					TempData["InvalidMessage"] = "课程信息不完整,请重新添加";
					return RedirectToAction("ShowAllCourse", "Course");
				}
				else
				{
					db.Courses.Add(course);
					db.SaveChanges();
				}
			}
			return RedirectToAction("ShowAllCourse", "Course");
		}

		
	}
}
