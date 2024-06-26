import java.util.Scanner;
import java.util.Stack;

/**
 * 对于只含有+、*运算的算术表达式，编写相应的语法分析程序
 * 上下文无关文法表示为:
 * E -> TE'
 * T -> FT'
 * F -> (E)|id
 * E' -> +TE'|ε
 * T' -> *FT'|ε
 */
public class Compiler2 {
    //输入字符指针
    private int id = 0;
    public boolean flag = true;
    /**
     * 终结符字符串
     */
    char[] terminal = new char[]{'i','+','*','(',')','$'};

    /**
     * 非终结符字符串数组
     * E' => G      T' => H
     */
    char[] nonterminal = new char[]{'E','G','T','H','F'};
    private int row = nonterminal.length;
    private int colomn = terminal.length;
    String[][] analysisTable = new String[row + 1][colomn + 1];

    private Stack<Character> stack = new Stack<Character>();

    private String input = new String();

    /**
     * 给analysisTable初始化,防止在出现错误时因为引用了未初始化的变量而抛出异常
     * 根据分析表内容,给analysisTable赋初值
     * E' => G      T' => H
     */
    public void initAnalysisTable(){

        for (int i = 0; i < analysisTable.length; i++) {
            for (int j = 0; j < analysisTable[i].length; j++) {
                analysisTable[i][j] = "";
            }
        }
        analysisTable[1][1] = "E->TG";
        analysisTable[1][4] = "E->TG";
        analysisTable[2][2] = "G->+TG";
        analysisTable[2][5] = "G->ε";
        analysisTable[2][6] = "G->ε";
        analysisTable[3][1] = "T->FH";
        analysisTable[3][4] = "T->FH";
        analysisTable[4][2] = "H->ε";
        analysisTable[4][3] = "H->*FH";
        analysisTable[4][5] = "H->ε";
        analysisTable[4][6] = "H->ε";
        analysisTable[5][1] = "F->i";
        analysisTable[5][4] = "F->(E)";
    }
    public void analysis(){
        System.out.println("stack\tinput\t");
        stack.push('$');
        stack.push('E');
        //查看当前栈顶元素,用变量character接收
        Character character = stack.peek();

        while (character != '$'){
            character = stack.peek();
            String str1 = new String(nonterminal);
            int i = str1.indexOf(character) + 1;
            String str2 = new String(terminal);
            int j = str2.indexOf(input.charAt(id)) + 1;

            System.out.print(stack.toString()+"\t\t");
            //只输出待分析的字符串
            for (int index = id; index < input.length(); index++) {
                System.out.print(input.charAt(index));
            }
            System.out.print("\t\t");

            if(character == input.charAt(id)) {
                stack.pop();
                id++;
                System.out.println("匹配");
            } else if (i <= 0 || j <= 0) {
                //如果当前M[X,a]空白,说明M[X,a]是一个报错条目
                System.out.println("错误,忽略当前字符");
                //抛弃一个输入记号,直到能够匹配为止
                id++;
                flag = false;
            } else if (isTerminal(character)) {
                //如果当前是终结符,但是没有匹配,表示语法分析错误
                System.out.println("错误,忽略当前字符");
                id++;
                flag = false;
            } else if (analysisTable[i][j].length() == 0) {
                //如果当前M[X,a]空白,说明M[X,a]是一个报错条目
                System.out.println("错误,忽略当前字符");
                //抛弃一个输入记号,直到能够匹配为止
                id++;
                flag = false;
            } else if(analysisTable[i][j].length() != 0){
                System.out.println(analysisTable[i][j]);
                stack.pop();
                char[] temp = analysisTable[i][j].substring(3).toCharArray();
                //把字符数组内容逆序入栈
                for (int k = temp.length - 1; k >= 0; k--) {
                    //如果不是 -> ε 的产生式,那么将产生式的字符加入到分析栈中
                    if(!isEmpty(temp[k])){
                        stack.push(temp[k]);
                    }
                }
            }
        }
        if(character == '$' && flag){
            System.out.println("接受,语法分析结束");
        }
    }
    /**
     * 判断某个字符是不是终结符
     */
    public boolean isTerminal(Character character){
        for (int i = 0; i < terminal.length; i++) {
            if(character == terminal[i]) {
                return true;
            }
        }
        return false;
    }
    public void setInput(){
        System.out.println("请输入待分析的字符串:");
        Scanner sc = new Scanner(System.in);
        input = sc.next();
    }
    public boolean isEmpty(Character character){
        return character == 'ε';
    }
}
