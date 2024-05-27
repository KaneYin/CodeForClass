// See https://aka.ms/new-console-template for more information

using EntityFrameWork1;
using EntityFrameWork1.DatabaseModels;

StudentManagement studentManagement = new StudentManagement();
StudentCourseManagement studentCourseManagement = new StudentCourseManagement();
CourseManagement courseManagement = new CourseManagement();

var isContinue = true;
while (isContinue)
{
    Console.Clear();
    Console.WriteLine("1. 学生信息管理系统");
    Console.WriteLine("2. 学生选课管理系统");
    Console.WriteLine("3. 课程信息管理系统");
    Console.WriteLine("4. 退出");
    Console.WriteLine("enter your choice:");
    var choice = Console.ReadLine(); 
    switch(choice)
    {
        case "1":
            studentManagement.ShowStudentMenu();
            break;
        case "2":
            studentCourseManagement.ShowStudentCourseMenu();
            break;
        case "3":
            courseManagement.ShowCourseMenu();
            break;
        case "4":
            isContinue = false;
            break;
    }
}



