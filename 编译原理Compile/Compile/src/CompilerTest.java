public class CompilerTest {
    public static void main(String[] args) {
        Compiler c = new Compiler();

        String str = new String("void main ( ) { int sum1 = 1 ; int sum2 = 2 ; if ( a >= b ) a = a ++ ; }");
        String[] strArr = c.strToArr(str);
        c.analysis(strArr);

    }
}