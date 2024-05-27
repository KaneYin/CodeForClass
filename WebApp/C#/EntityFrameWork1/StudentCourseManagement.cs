using EntityFrameWork1.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameWork1
{
    public class StudentCourseManagement
    {
        
        public void ShowStudentCourseMenu()
        {
            bool isContinue = true;
            while (isContinue)
            {
                Console.Clear();
                Console.WriteLine("1. 给学生选课");
                Console.WriteLine("2. 显示学生所有已选课程");
                Console.WriteLine("3. 返回上级菜单");
                Console.Write("Enter your choice: ");
                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        SelectCourse();
                        break;
                    case "2":
                        ShowSelectedCourses();
                        break;
                    case "3":
                        isContinue = false;
                        break;
                    default:
                        Console.WriteLine("输入错误,请重新输入");
                        break;
                }
            }
        }

        void SelectCourse()
        {
            Console.WriteLine("请输入学生的学号:");
            var str = Console.ReadLine();
            using (var context = new _2114010313DbContext())
            {
                var checkStudent = context.Students.Find(str);
                if (checkStudent == null)
                {
                    Console.WriteLine($"不存在学号为:{str}的学生,请重试");
                    printMessage();
                }
                else
                {
                    Console.WriteLine("请输入该学生的初始密码:");
                    var password = Console.ReadLine();
                    if (password == checkStudent.InitPassword)
                    {
                        Console.WriteLine("密码正确,请输入学生选课的课程号:");
                        var CourseID = Console.ReadLine();
                        var checkCourse = context.Courses.Find(CourseID);
                        if (checkCourse == null)
                        {
                            Console.WriteLine($"不存在课程号为:{CourseID} 的课程, 请重试");
                            printMessage();
                            return;
                        }
                        else
                        {
                            var stuCourse = new Stucourse1();
                            stuCourse.StudentId = str;
                            stuCourse.CourseId = CourseID;
                            context.Stucourse1s.Add(stuCourse);
                            context.SaveChanges();
                            Console.WriteLine("为该名学生选课成功...");
                            printMessage();
                        }
                    }
                    else
                    {
                        Console.WriteLine("密码错误,请重试");
                        printMessage();
                        return;
                    }
                }
            }
        }
        void ShowSelectedCourses()
        {
            Console.WriteLine("请输入想要查看选课信息的学生学号:");
            var str = Console.ReadLine();
            using (var context = new _2114010313DbContext())
            {
                var checkStudent = context.Students.Find(str);
                if (checkStudent == null)
                {
                    Console.WriteLine($"不存在学号为:{str}的学生,请重试");
                    printMessage();
                    return;
                }

                var stuCourses = context.Stucourse1s.Where(Stucourse1 => Stucourse1.StudentId == str).ToList();
                if (stuCourses.Count == 0)
                {
                    Console.WriteLine("该学生没有选课");
                    printMessage(); return;
                }
                var Student = context.Students.Find(str);
                foreach (var stuCourse in stuCourses)
                {
                    var Course = context.Courses.Find(stuCourse.CourseId);
                    Console.WriteLine($"学号为{str}的学生{Student.StudentName}选了{stuCourse.CourseId}课程,课程名称为:{Course.CourseName}");
                }
                printMessage();
            }
        }


        //输出提示信息
        public void printMessage()
        {
            Console.WriteLine("successfully, press any key to continue...");
            Console.ReadKey();
        }

    }
}
