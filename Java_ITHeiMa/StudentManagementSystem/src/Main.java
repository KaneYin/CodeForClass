//学生管理系统项目

import java.util.ArrayList;

public class Main {
    public static void main(String[] args) {
        StudentManagement test = new StudentManagement();
        ArrayList<Student> stuArr = new ArrayList<>();

        while (true){
            test.printMenu();


            int choice = test.inputNum();
            switch (choice){
                case 1:stuArr = test.addStu(stuArr);continue;
                case 2:stuArr = test.delStu(stuArr);continue;
                case 3:stuArr = test.changeStu(stuArr);continue;
                case 4:test.searchStu(stuArr);continue;
                case 5:break;
            }
            break;
        }
        return;
    }
}