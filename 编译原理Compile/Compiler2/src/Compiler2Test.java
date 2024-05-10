public class Compiler2Test {
    public static void main(String[] args) {
        Compiler2 compiler2 = new Compiler2();
        compiler2.initAnalysisTable();
        compiler2.setInput();
        compiler2.analysis();
        if(!compiler2.flag){
            System.out.println("输入串中存在语法错误,已对错误字符忽略处理");
            System.exit(1);
        }
    }
}