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
				//ʹ��TempData��¼��¼���û���,��ʾ�ڵ�¼��Ľ�����
				TempData["UserNamemessage"] = loginUser.username;
                //��ת��Course��������ShowAllCourse��������
                return RedirectToAction("ShowAllCourse","Course");
            }
            //��������username��S��ͷ,����������ѧ�����ڵ�½,��ת��ѧ����ص�ҳ��
            else if(loginUser.username.StartsWith("S") || loginUser.username.StartsWith("s"))
            {
				HttpContext.Session.SetString("UserName", loginUser.username);
				TempData["message"] = loginUser.username;
                //ֱ���ض���ѧ���������µ���ʾ��ѧ��������ѡ�γ���.
                string studentId = loginUser.username;
                //ʹ��pwd�����û���¼ʱ���������
                string pwd = loginUser.pwd;
                using(var db = new _2114010313DbContext())
                {
                    var student = db.Students.Find(studentId);
                    if(student == null)
					{
						TempData["UserNameInvalid"] = "�û�������,����������";
						return RedirectToAction("ShowLogin", "Home");
                    }

                    //�ж�ѧ���û��������Ƿ���ȷ
                    else if(pwd != student.InitPassword)
                    {
                        TempData["PasswordInvalid"] = "�û��������,����������";
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
                TempData["Message"] = "��Ч���û��������������";
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
