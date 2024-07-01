using LogInDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LogInDemo.Controllers
{
	public class StudentController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		//返回的应该是登录学生已经选过的课程,而不是全部的课程
		public IActionResult ShowSelectedCourse()
		{
			ViewData["UserName"] = HttpContext.Session.GetString("UserName");
			//获取传入的学生编号
			string userid = ViewData["Username"]?.ToString();

			using (var db = new _2114010313DbContext())
			{
				var student = db.Students.Find(userid);

				//返回所有包含该学生编号的选课信息,即是该学生已经选的课程
				var selectedCourses = db.Stucourse1s
									  .Where(sc=>sc.StudentId.Contains(userid))
									  .ToList();

				return View(selectedCourses);
			}
		}

		public IActionResult StudentWelcome()
		{
			ViewData["UserName"] = HttpContext.Session.GetString("UserName");
			return View();
        }

		//展示某个学生所有可以选择的课程
		public IActionResult ShowAvailableCourse()
		{
			using (var db = new _2114010313DbContext())
			{
				var courses = db.Courses.ToList();
				return View(courses);
			}
		}
		public IActionResult SelectCourse(Course course)
		{
			String stuId = HttpContext.Session.GetString("UserName");

			using(var db = new _2114010313DbContext())
			{
				//如果该学生已经选过该课程,返回到查看所有可选课程的页面中
				if(db.Stucourse1s.Find(stuId,course.CourseId) != null)
				{
					TempData["SelectCourseFailed"] = "你已经选过该课程了,不可重复选课";
					return RedirectToAction("ShowAvailableCourse","Student");
				}
				else
				{
					Stucourse1 stucourse = new Stucourse1();
					stucourse.StudentId = stuId;
					stucourse.CourseCredit = course.CourseCredit;
					stucourse.CourseName = course.CourseName;
					stucourse.CourseId = course.CourseId;
					db.Stucourse1s.Add(stucourse);
					db.SaveChanges();
				}
				return RedirectToAction("ShowSelectedCourse", "Student");
			}
		}
		public IActionResult UnSelectCourse(Course course)
		{
			String stuId = HttpContext.Session.GetString("UserName");

			using(var db = new _2114010313DbContext())
			{
				//如果该学生已经选过该课程,才允许学生退课
				if (db.Stucourse1s.Find(stuId,course.CourseId) == null)
				{
					TempData["UnselectCourseFailed"] = "你没有选过这门课程,不能退课";
					return RedirectToAction("ShowSelectedCourse", "Student");
				}
				else
				{
					Stucourse1 stucourse = db.Stucourse1s.Find(stuId,course.CourseId);
					db.Stucourse1s.Remove(stucourse);
					db.SaveChanges();
					return RedirectToAction("ShowSelectedCourse", "Student");
				}
			}
		}
	}
}
