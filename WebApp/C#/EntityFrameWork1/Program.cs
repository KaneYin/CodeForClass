// See https://aka.ms/new-console-template for more information



using EntityFrameWork1.DatabaseModels;


var isContinue = true;
while (isContinue)
{
    Console.Clear();
    Console.WriteLine("1. Add a student");
    Console.WriteLine("2. Show all students");
    Console.WriteLine("3. Search a student");
    Console.WriteLine("4. Update student information");
    Console.WriteLine("5. Delete student information");
    Console.WriteLine("-------------------------------");
    Console.WriteLine("6. Select a Course");
    Console.WriteLine("7. Show all selected courses");
    Console.WriteLine("-------------------------------");
    Console.WriteLine("8. Add a course");
    Console.WriteLine("9. Show all courses");
    Console.WriteLine("10. Search a course");
    Console.WriteLine("11. Update Course information");
    Console.WriteLine("12. Delete Course information");

    Console.WriteLine("13. Exit");
    Console.Write("Enter your choice: ");
    var choice = Console.ReadLine();
    switch (choice)
    {
        case "1":
            AddNewStudent();
            break;
        case "2":
            ShowAllStudents();
            break;
        case "3":
            SearchStudent();
            break;
        case "4":
            UpdateStudentInfo();
            break;
        case "5":
            DelStudentInfo();
            break;
        case "6":
            SelectCourse();
            break;
        case "7":
            ShowSelectedCourses();
            break;
        case "8":
            AddNewCourse();
            break;
        case "9":
            ShowAllCourses();
            break;
        case "10":
            SearchCourse();
            break;
        case "11":
            UpdateCourseInfo();
            break;
        case "12":
            DelCourseInfo();
            break;
        case "13":
            isContinue = false;
            break;
        default:
            Console.WriteLine("Invalid choice");
            break;
    }
}

void AddNewStudent()
{
    Console.WriteLine("Adding new student id:");
    var studentId = Console.ReadLine();
    Console.WriteLine("Enter student name: ");
    var studentName = Console.ReadLine();
    Console.WriteLine("Enter Student class: ");
    var studentClass = Console.ReadLine();
    Console.WriteLine("输入学生密码:"); 
    var studentPassword = Console.ReadLine();
    using (var context = new _2114010313DbContext())
    {
        var checkStu = context.Students.Find(studentId);
        if (checkStu != null && checkStu.StudentName != "")
        {
            Console.WriteLine($"学号：{studentId},已经被 {checkStu.StudentName} 占用");
            printMessage();
            return;
        }
        var student = new Student();
        student.StudentName = studentName;
        student.StudentId = studentId;
        student.StudentClass = studentClass;
        student.InitPassword = studentPassword;
        context.Students.Add(student);
        context.SaveChanges();
    }
    printMessage();
}

void ShowAllStudents()
{
    using (var context = new _2114010313DbContext())
    {
        var students = context.Students.ToList();
        foreach (var student in students)
        {
            Console.WriteLine($"Id: {student.StudentId}, Name: {student.StudentName},Class: {student.StudentClass}");
        }
    }
    printMessage();
}

void SearchStudent()
{
    Console.WriteLine("Please input the Student ID:");
    var str =  Console.ReadLine();
    
    using (var context = new _2114010313DbContext())
    {
        //tell StudentID is valid or not
        var checkStudent = context.Students.Find(str);

        //判断逻辑
        if(checkStudent == null)
        {
            Console.WriteLine($"不存在学号为:{str}的学生,请重试");
            printMessage();
        }
        else
        {
            Console.WriteLine($"学号为:{str} 的学生姓名为:{checkStudent.StudentName} ,班级为:{checkStudent.StudentClass}");
            printMessage();
        }
        
    }
}
void UpdateStudentInfo()
{
    Console.WriteLine("请输入想要修改的学生的学号:");
    var str = Console.ReadLine();
    using (var context = new _2114010313DbContext())
    {
        //tell StudentID is valid or not
        var checkStudent = context.Students.Find(str);

        //判断逻辑
        if (checkStudent == null)
        {
            Console.WriteLine($"不存在学号为:{str}的学生,请重试");
            printMessage();
        }
        else
        {
            Console.WriteLine("请输入新的学生姓名:");
            var studentName = Console.ReadLine();
            Console.WriteLine("请输入新的学生班级:");
            var studentClass = Console.ReadLine();
            Console.WriteLine("请输入新的学生密码:");
            var studentPassword = Console.ReadLine();

            checkStudent.StudentName = studentName;
            checkStudent.StudentClass = studentClass;
            checkStudent.InitPassword = studentPassword;
            context.SaveChanges();
            Console.WriteLine("学生信息修改成功,该学生的新信息如下:");
            Console.WriteLine($"学号为:{str} 的学生姓名修改为:{checkStudent.StudentName} ,班级修改为:{checkStudent.StudentClass}");
            printMessage();
        }
    }
}

// 删除学生信息逻辑
void DelStudentInfo()
{
    Console.WriteLine("请输入想要删除的学生学号:");
    var str = Console.ReadLine();
    using (var context = new _2114010313DbContext())
    {
        //tell StudentID is valid or not
        var checkStudent = context.Students.Find(str);

        //判断逻辑
        if (checkStudent == null)
        {
            Console.WriteLine($"不存在学号为:{str}的学生,请重试");
            printMessage();
        }
        else
        {
            context.Students.Remove(checkStudent);
            context.SaveChanges();
            Console.WriteLine("该名学生的信息已经成功删除...");
            printMessage();
        }
    }
}

void SelectCourse()
{
    Console.WriteLine("请输入学生的学号:");
    var str = Console.ReadLine();
    using(var context = new _2114010313DbContext())
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
            if( password == checkStudent.InitPassword)
            {
                Console.WriteLine("密码正确,请输入学生选课的课程号:");
                var CourseID = Console.ReadLine();
                var checkCourse = context.Courses.Find(CourseID);
                if(checkCourse == null) 
                {
                    Console.WriteLine($"不存在课程号为:{CourseID} 的课程, 请重试");
                    printMessage();
                    return;
                }
                else
                {
                    var stuCourse = new Stucourse();
                    stuCourse.StudentId = str;
                    stuCourse.CourseId = CourseID;
                    context.Stucourses.Add(stuCourse);
                    context.SaveChanges();
                    Console.WriteLine("为该名学生选课成功...");
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

// 有关课程的信息
void AddNewCourse()
{
    Console.WriteLine("请输入课程代码:");
    var str = Console.ReadLine();
    using(var context = new _2114010313DbContext()) 
    {
        var checkCourse = context.Courses.Find(str);

        if(checkCourse != null && checkCourse.CourseName != "") 
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
    using(var context = new _2114010313DbContext())
    {
        var courses = context.Courses.ToList();
        foreach (var course in courses)
        {
            Console.WriteLine($"Id: {course.CourseId}, Name: {course.CourseName},Credit: {course.CourseCredit}");
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

void ShowSelectedCourses()
{
    Console.WriteLine("请输入想要查看选课信息的学生学号:");
    var str = Console.ReadLine();
    using(var  context = new _2114010313DbContext())
    {
        var checkStudent = context.Students.Find(str);
        if (checkStudent == null)
        {
            Console.WriteLine($"不存在学号为:{str}的学生,请重试");
            printMessage();
            return;
        }
        var stuCourses = context.Stucourses.Find(str);
        Console.WriteLine($"学号为{stuCourses.StudentId}的学生,已经选的课程号有:{stuCourses.CourseId}");
        printMessage();
    }
}

//输出提示信息
void  printMessage()
{
    Console.WriteLine("successfully, press any key to continue...");
    Console.ReadKey();
}