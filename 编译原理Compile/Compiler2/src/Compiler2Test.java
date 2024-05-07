public class Compiler2Test {
    public static void main(String[] args) {
        Compiler2 compiler2 = new Compiler2();
        compiler2.initAnalysisTable();
        compiler2.setInput();
        compiler2.analysis();
    }
}