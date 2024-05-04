import GameUI.GameJFrame;
import GameUI.LoginJFrame;
import GameUI.RegisterJFrame;

public class App {
    public static void main(String[] args) {
        //表示程序的入口地址

        //如果我们想要开启一个界面,可以直接创建谁的对象
        new GameJFrame();
        new LoginJFrame();
        new RegisterJFrame();

    }
}
