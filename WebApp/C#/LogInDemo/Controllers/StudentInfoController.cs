using LogInDemo.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;

namespace LogInDemo.Controllers
{
	public class StudentInfoController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult ShowStudentInfo()
		{
			using(var db = new _2114010313DbContext())
			{
				List<Student> student = db.Students.ToList();
				return View(student);
			}
		}
		public IActionResult StudentInfoEdit(String id)
		{
			if(string.IsNullOrWhiteSpace(id))
			{
				return View();
			}
			using(var db = new _2114010313DbContext())
			{
				var student = db.Students.Find(id);
				return View(student);
			}
		}
		public IActionResult SaveStudentInfo(Student student)
		{
			using(var db = new _2114010313DbContext())
			{
				var model = db.Students.Find(student.StudentId);
				if (model == null)
				{
					model = new Student();
					model.StudentId = student.StudentId;
					model.StudentName = student.StudentName;
					model.StudentClass = student.StudentClass;
					db.Students.Add(model);
				}
				else
				{
					model.StudentName = student.StudentName;
					model.StudentClass = student.StudentClass;
				}
				db.SaveChanges();
			}

			return RedirectToAction("ShowStudentInfo", "StudentInfo");
		}
		public IActionResult DeleteStudentInfo(Student student)
		{
			using(var db = new _2114010313DbContext())
			{
				//删除课程需要连带所有已选课程一起删除
				var model = db.Students.Find(student.StudentId);
				var allCourses = db.Courses.ToList();
				//遍历所有课程,只要能找到要删除的学生有选课记录,就删除
				foreach(var course in allCourses)
				{
					var selectedCourses = db.Stucourse1s.Find(student.StudentId, course.CourseId);
					// 该变量不为空,说明学生选了这门课
					if(selectedCourses != null)
					{
						db.Stucourse1s.Remove(selectedCourses);
					}
				}
				if(model == null)
				{
                    
                    db.SaveChanges();
                    return RedirectToAction("ShowStudentInfo", "StudentInfo");
				}
				else
				{
                    db.Students.Remove(model);
                }
                
                db.SaveChanges();
            }
			return RedirectToAction("ShowStudentInfo", "StudentInfo");
		}
		public IActionResult StudentInfoDelete(string  Id) 
		{
			if(string.IsNullOrEmpty(Id))
			{
				return View();
			}
			using(var db = new _2114010313DbContext())
			{
				var student = db.Students.Find(Id);
				return View(student);
			}
		}

		public IActionResult AddStudentInfo(Student student)
		{
			return View(student);
		}

		public IActionResult StudentInfoAdd(Student student)
		{
			using(var db = new _2114010313DbContext())
			{
				if(db.Students.Find(student.StudentId) != null)
				{
					TempData["StudentInfoAddFailed"] = "该学生已经存在,请重试";
					return RedirectToAction("StudentInfoAdd", "StudentInfo");
				}
				else
				{
					db.Students.Add(student);
					db.SaveChanges();
					return RedirectToAction("ShowStudentInfo", "StudentInfo");
				}
			}
		}
	}
	
}

