
import java.util.ArrayList;
import java.util.Scanner;

//添加,删除,修改,退出
public class StudentManagement {

    private boolean flag = true;

    public StudentManagement() {
    }

    public boolean getFlag(){
        return flag;
    }

    //打印菜单功能
    public void printMenu(){
        System.out.println("----欢迎来到学生管理系统----");
        System.out.println("1.添加学生");
        System.out.println("2.删除学生");
        System.out.println("3.修改学生");
        System.out.println("4.查询学生");
        System.out.println("5.退出");
        System.out.println("请输入您的选择:");
    }



    public int inputNum(){
        Scanner sc = new Scanner(System.in);
        return sc.nextInt();
    }

    //键盘录入每一个学生的信息并添加,需要满足ID唯一
    public ArrayList<Student> addStu(ArrayList<Student> stuArr ){

        while (true) {
            Student tmpStu = new Student();
        System.out.println("请输入要添加学生的唯一id");

        Scanner scanner = new Scanner(System.in);
        String tmpId = scanner.next();
        System.out.println("请输入要添加学生的name");
        String tmpName = scanner.next();
        System.out.println("请输入要添加学生的addr");
        String tmpAddr = scanner.next();
        System.out.println("请输入要添加学生的age");
        int tmpAge = scanner.nextInt();

        tmpStu.setId(tmpId);
        tmpStu.setName(tmpName);
        tmpStu.setAddr(tmpAddr);
        tmpStu.setAge(tmpAge);

        if (lookUp(stuArr,tmpId)) {
            stuArr.add(tmpStu);
            System.out.println("该名学生信息添加成功,是否继续添加(yes/no)?");
            Scanner sc = new Scanner(System.in);
            String str = sc.next();
            if(str.equals("yes")){
                continue;
            } else if (str.equals("no")) {
                break;
            }
        } else {
            System.out.println("该ID已经存在请重试!");
        }
        }


        
            //查找是否存在相同的ID

        return stuArr;
    }

    //键盘录入要删除学生的ID,若存在则删除,不存在则报错
    public ArrayList<Student> delStu(ArrayList<Student> stuArr){
        flag = true;
        System.out.println("请输入要删除学生的id");
        Scanner scanner = new Scanner(System.in);
        String tmpId = scanner.next();
        if(!lookUp(stuArr,tmpId)){
            int index = 0;
            for (int i = 0; i < stuArr.size(); i++) {
                if(stuArr.get(i).getId().equals(tmpId)){
                    index = i;
                    break;
                }
            }
            stuArr.remove(index);
        }
        else{
            System.out.println("这名学生不存在!");
            flag = false;
        }
        return stuArr;
    }

    public ArrayList<Student> changeStu(ArrayList<Student> stuArr){
        System.out.println("请输入要修改学生的id");
        Scanner scanner = new Scanner(System.in);
        String tmpId = scanner.next();

        if(!lookUp(stuArr,tmpId)){
            int index = 0;
            for (int i = 0; i < stuArr.size(); i++) {
                if(stuArr.get(i).getId().equals(tmpId)){
                    index = i;
                    break;
                }
            }
            System.out.println("请输入新的姓名:");
            String tmpName = scanner.next();
            System.out.println("请输入新的地址:");
            String tmpAddr = scanner.next();
            System.out.println("请输入新的年龄:");
            int tmpAge = scanner.nextInt();

            Student tmpStu = new Student();
            tmpStu.setId(tmpId);
            tmpStu.setName(tmpName);
            tmpStu.setAddr(tmpAddr);
            tmpStu.setAge(tmpAge);

            stuArr.remove(index);
            stuArr.add(tmpStu);

        }
        else{
            System.out.println("该学生不存在!");
            flag = false;
        }
        return stuArr;
    }

    //打印学生信息
    //如果没有,提示添加后再查询;如果有,则输出全部学生信息
    public void searchStu(ArrayList<Student> stuArr){
        if(stuArr.size() == 0){
            System.out.println("当前不存在学生信息,请添加后再查询");
        }else{
            for (int i = 0; i < stuArr.size(); i++) {
                System.out.println(stuArr.get(i).getId() +" "+ stuArr.get(i).getName() +" "+ stuArr.get(i).getAge() +" "+ stuArr.get(i).getAddr());
            }     
        }
    }

    //根据ID查找学生是否存在于学生列表中,学号没有重复返回true
    //TODO 解决bug
    public boolean lookUp(ArrayList<Student> stuArr,String id){
        boolean flag = true;
        if(stuArr.size() == 0){
            return true;
        }
        for (int i = 0; i < stuArr.size(); i++) {
            if(stuArr.get(i).getId().equals(id)){
                flag = false;
                break;
            }
        }
        return flag;
    }
}
