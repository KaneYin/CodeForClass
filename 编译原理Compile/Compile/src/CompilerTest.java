
import java.nio.file.Files;
import java.nio.file.Paths;

public class CompilerTest {
    public static void main(String[] args) {
        Compiler c = new Compiler();

        String inputCodePath = "C:\\Code\\Java\\HeiMa_java\\Compiler\\inputCode1.txt";
        /*
        使用文件读取,将文本内容读取后返回字符窜
         */
        try {
            String content = Files.readString(Paths.get(inputCodePath));
            String[] strArr = c.strToArr(content);
            c.analysis(strArr);
        }catch (Exception e){
            e.printStackTrace();
        }
    }
}