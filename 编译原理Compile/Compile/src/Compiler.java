public class Compiler {

    private String[] keyWords= new String[]{"void","main","int","char","if","else","for","while"};

    //方法将输入输入的字符串转换为字符数组
    public String[] strToArr(String input){
        String[] stringArray = input.split("[ \r\n]+");
        return stringArray;
    }

    //输入是字符串数组
    public void analysis(String[] strArr){

        int result;

        for (int index = 0; index < strArr.length; index++) {

            //字符串数组第一个符号是+
            if(strArr[index].charAt(0) == '+'){
                if(strArr[index].equals("++")){
                    System.out.println("(++,212)");
                }else{
                    System.out.println("(+,201)");
                }
            } else if (strArr[index].charAt(0) == '-') {
                if(strArr[index].equals("--")){
                    System.out.println("(--,213)");
                }else{
                    System.out.println("(-,202)");
                }
            } else if (strArr[index].charAt(0) == '*') {
                System.out.println("(*,203)");
                
            } else if (strArr[index].charAt(0) == '/') {

                //判断是否是一个注释块
                if(strArr[index].equals("/*")){
                    do {
                        index++;
                    } while (!strArr[index].equals("*/"));
                }else{
                    System.out.println("(/,204)");
                }
            } else if (strArr[index].charAt(0) == '=') {
                if (strArr[index].equals("==")){
                    System.out.println("(==,210)");
                }else{
                    System.out.println("(=,205)");
                }
                
            } else if (strArr[index].charAt(0) == '>') {
                if (strArr[index].equals(">=")){
                    System.out.println("(>=,207)");
                }else{
                    System.out.println("(>,206)");
                }

            } else if (strArr[index].charAt(0) == '<') {
                if (strArr[index].equals("<=")){
                    System.out.println("(<=,209)");
                } else if (strArr[index].equals("<>")) {
                    System.out.println("(<>,211)");
                }else {
                    System.out.println("(<,211)");
                }
            } else if (strArr[index].charAt(0) == '(') {
                System.out.println("((,301)");
            } else if (strArr[index].charAt(0) == ')') {
                System.out.println("(),302)");
            } else if (strArr[index].charAt(0) == '{') {
                System.out.println("({,303)");
            } else if (strArr[index].charAt(0) == '}') {
                System.out.println("(},304)");
            } else if (strArr[index].charAt(0) == ';') {
                System.out.println("(;,305)");
            } else if (isId(strArr[index])){
                /*
                满足标识符规则的同时包含Id和关键字,所以还需要判断是否是关键字
                 */

                if(!isKey(strArr[index])){
                    /*
                    不是关键字,将标识符打印出来
                     */
                    System.out.println("(" + strArr[index] + ",400)");
                }else {
                    /*
                    是关键字,将关键字打印出来
                     */
                    for (int i = 0; i < keyWords.length; i++) {
                        if (strArr[index].equals(keyWords[i])) {
                            result = 101 + i;
                            System.out.println("(" + keyWords[i] + "," + result + ")");
                            break;
                        }
                    }
                }

            }else if(strArr[index].charAt(0) >= '0' && strArr[index].charAt(0) <= '9'){
                System.out.println("(" + strArr[index] + ",500)");
            }else{
                System.out.println("error");
            }
        }
    }

    //如果字符串符合标识符命名规则,返回真
    public boolean isId(String str){
        char firstL = str.charAt(0);
        //符合标识符命名规则
        if ((firstL >= 'a' && firstL <= 'z' )||(firstL >= 'A' && firstL <= 'Z')||firstL == '_'){
            return true;
        }else{
            return false;
        }
    }

    //如果字符串是一个关键字,返回真
    public boolean isKey(String str){
        for (int i = 0; i < keyWords.length; i++) {
            if(str.equals(keyWords[i])){
                return true;
            }
        }
        return false;
    }
}