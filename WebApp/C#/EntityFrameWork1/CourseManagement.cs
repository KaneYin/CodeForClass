using EntityFrameWork1.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameWork1
{
    public class CourseManagement
    {
        public void ShowCourseMenu()
        {
            bool isContinue = true;
            while (isContinue)
            {
                Console.Clear();
                Console.WriteLine("1. 添加一门课程");
                Console.WriteLine("2. 显示所有课程");
                Console.WriteLine("3. 搜索课程");
                Console.WriteLine("4. 更新课程信息");
                Console.WriteLine("5. 删除课程信息");
                Console.WriteLine("6. 返回上一级");
                Console.Write("Enter your choice: ");
                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AddNewCourse();
                        break;
                    case "2":
                        ShowAllCourses();
                        break;
                    case "3":
                        SearchCourse();
                        break;
                    case "4":
                        UpdateCourseInfo();
                        break;
                    case "5":
                        DelCourseInfo();
                        break;
                    case "6":
                        isContinue = false;
                        break;                
                }
            }
        }


        // 有关课程的信息
        void AddNewCourse()
        {
            Console.WriteLine("请输入课程代码:");
            var str = Console.ReadLine();
            using (var context = new _2114010313DbContext())
            {
                var checkCourse = context.Courses.Find(str);
                if (!str.StartsWith('C'))
                {
                    Console.WriteLine("课程号不合法,请重新输入");
                    printMessage();
                    return;
                }
                if (checkCourse != null && checkCourse.CourseName != "")
                {
                    Console.WriteLine($"课程号为: {str} , 已经被 {checkCourse.CourseName}占用, 请重试");
                    printMessage();
                    return;
                }
                else
                {
                    Console.WriteLine("请输入课程名称:");
                    var courseName = Console.ReadLine();
                    Console.WriteLine("请输入课程学分:");
                    int credit = int.Parse(Console.ReadLine());
                    var course = new Course();

                    course.CourseName = courseName;
                    course.CourseCredit = credit;
                    course.CourseId = str;

                    context.Courses.Add(course);
                    context.SaveChanges();
                }
                printMessage();
            }
        }

        void ShowAllCourses()
        {
            using (var context = new _2114010313DbContext())
            {
                var courses = context.Courses.ToList();
                foreach (var course in courses)
                {
                    Console.WriteLine($"课程号: {course.CourseId}, 课程名称: {course.CourseName},课程学分: {course.CourseCredit}");
                }
                printMessage();
            }
        }

        void SearchCourse()
        {
            Console.WriteLine("请输入要查找的课程号:");
            var str = Console.ReadLine();
            using (var context = new _2114010313DbContext())
            {
                var checkCourse = context.Courses.Find(str);
                if (checkCourse == null)
                {
                    Console.WriteLine($"不存在课程号为:{str}的课程,请重试");
                    printMessage();
                    return;
                }
                else
                {
                    Console.WriteLine($"课程号为:{str} 的课程名称为:{checkCourse.CourseName} ,课程学分为:{checkCourse.CourseCredit}");
                    printMessage();
                }
            }
        }

        void UpdateCourseInfo()
        {
            Console.WriteLine("请输入想要修改的课程的课程号:");
            var str = Console.ReadLine();
            using (var context = new _2114010313DbContext())
            {
                //tell StudentID is valid or not
                var checkCourse = context.Courses.Find(str);

                //判断逻辑
                if (checkCourse == null)
                {
                    Console.WriteLine($"不存在学号为:{str}的,请重试");
                    printMessage();
                }
                else
                {
                    Console.WriteLine($"该课程的原课程名为:{checkCourse.CourseName},原课程学分为:{checkCourse.CourseCredit}");
                    Console.WriteLine("请输入新的课程姓名:");
                    var courseName = Console.ReadLine();
                    Console.WriteLine("请输入新的课程学分:");
                    int credit = int.Parse(Console.ReadLine());

                    checkCourse.CourseName = courseName;
                    checkCourse.CourseCredit = credit;
                    context.SaveChanges();
                    Console.WriteLine("课程信息修改成功,该课程的新信息如下:");
                    Console.WriteLine($"课程号为:{str} 的课程名修改为:{checkCourse.CourseName} ,课程学分修改为:{checkCourse.CourseCredit}");
                    printMessage();
                }
            }
        }

        void DelCourseInfo()
        {
            Console.WriteLine("请输入想要删除的课程号:");
            var str = Console.ReadLine();
            using (var context = new _2114010313DbContext())
            {
                var checkCourse = context.Courses.Find(str);

                //判断逻辑
                if (checkCourse == null)
                {
                    Console.WriteLine($"不存在课程号为：{str} 的课程,请重试");
                    printMessage();
                }
                else
                {
                    context.Courses.Remove(checkCourse);
                    context.SaveChanges();
                    Console.WriteLine("该课程信息已经成功删除...");
                    printMessage();
                }
            }
        }



        //输出提示信息
        void printMessage()
        {
            Console.WriteLine("successfully, press any key to continue...");
            Console.ReadKey();
        }
    }
}
