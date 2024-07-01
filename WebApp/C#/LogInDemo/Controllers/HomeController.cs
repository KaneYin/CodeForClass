using LogInDemo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LogInDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult ShowLogin()
        {
            return View();
        }

        public IActionResult LoginUser(LoginDemo loginUser)
        {
            if(loginUser.username == "admin")
            {
                HttpContext.Session.SetString("UserName", loginUser.username);
				//使用TempData记录登录的用户名,显示在登录后的界面中
				TempData["UserNamemessage"] = loginUser.username;
                //跳转到Course控制器的ShowAllCourse方法下面
                return RedirectToAction("ShowAllCourse","Course");
            }
            //如果输入的username以S开头,表明现在是学生正在登陆,跳转到学生相关的页面
            else if(loginUser.username.StartsWith("S") || loginUser.username.StartsWith("s"))
            {
				HttpContext.Session.SetString("UserName", loginUser.username);
				TempData["message"] = loginUser.username;
                //直接重定向到学生控制器下的显示该学生所有已选课程下.
                string studentId = loginUser.username;
                //使用pwd接收用户登录时候传入的密码
                string pwd = loginUser.pwd;
                using(var db = new _2114010313DbContext())
                {
                    var student = db.Students.Find(studentId);
                    if(student == null)
					{
						TempData["UserNameInvalid"] = "用户不存在,请重新输入";
						return RedirectToAction("ShowLogin", "Home");
                    }

                    //判断学生用户的密码是否正确
                    else if(pwd != student.InitPassword)
                    {
                        TempData["PasswordInvalid"] = "用户密码错误,请重新输入";
						return RedirectToAction("ShowLogin", "Home");
                    }
                    else
                    {
						return RedirectToAction("StudentWelcome", "Student");
					}
				}
			}
            else
            {
                TempData["Message"] = "无效的用户名或者密码错误";
                return RedirectToAction("ShowLogin","Home");
            }
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
