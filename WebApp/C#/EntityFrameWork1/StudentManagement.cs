using EntityFrameWork1.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameWork1
{
    public class StudentManagement
    {
        public void ShowStudentMenu()
        {
            bool isContinue = true;
            
            while (isContinue)
            {
                Console.Clear();
                Console.WriteLine("1. 新增一个学生");
                Console.WriteLine("2. 显示所有学生信息");
                Console.WriteLine("3. 搜索一个学生");
                Console.WriteLine("4. 更新学生信息");
                Console.WriteLine("5. 删除学生信息");
                Console.WriteLine("6. 返回上级菜单");
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
                        isContinue = false;
                        break; 
                    default:
                        break;
                }
            }
        }

        public void  AddNewStudent()
        {
            Console.WriteLine("请输入新添加的学生学号:");
            var studentId = Console.ReadLine();

            if(!studentId.StartsWith('S'))
            {
                Console.WriteLine("学号不合法,请重新输入");
                printMessage();
                return;
            }

            Console.WriteLine("请输入新添加的学生姓名:");
            var studentName = Console.ReadLine();
            Console.WriteLine("请输入学生的班级: ");
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

        public void ShowAllStudents()
        {
            using (var context = new _2114010313DbContext())
            {
                var students = context.Students.ToList();
                foreach (var student in students)
                {
                    Console.WriteLine($"学号: {student.StudentId}, 学生姓名: {student.StudentName},学生班级: {student.StudentClass}");
                }
            }
            printMessage();
        }

        public void SearchStudent()
        {
            Console.WriteLine("请输入学生学号:");
            var str = Console.ReadLine();

            if (!str.StartsWith('S'))
            {
                Console.WriteLine("学号不合法,请重新输入");
                printMessage();
                return;
            }
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
                    Console.WriteLine($"学号为:{str} 的学生姓名为:{checkStudent.StudentName} ,班级为:{checkStudent.StudentClass}");
                    printMessage();
                }

            }
        }
        public void UpdateStudentInfo()
        {
            Console.WriteLine("请输入想要修改的学生的学号:");
            var str = Console.ReadLine();
            if (!str.StartsWith('S'))
            {
                Console.WriteLine("学号不合法,请重新输入");
                printMessage();
                return;
            }
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
                    Console.WriteLine($"该学生的原信息为:姓名 {checkStudent.StudentName},班级{checkStudent.StudentClass}");
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
        public void DelStudentInfo()
        {
            Console.WriteLine("请输入想要删除的学生学号:");
            var str = Console.ReadLine();
            if (!str.StartsWith('S'))
            {
                Console.WriteLine("学号不合法,请重新输入");
                printMessage();
                return;
            }
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
        public void printMessage()
        {
            Console.WriteLine("successfully, press any key to continue...");
            Console.ReadKey();
        }
    }
}
