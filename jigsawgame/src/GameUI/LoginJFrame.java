package GameUI;

import javax.swing.*;

public class LoginJFrame extends JFrame {

    //在创建登录界面的时候,同时给这个界面设置一些信息
    //宽高,是否可见等
    public LoginJFrame(){
        this.setSize(488,430);
        this.setTitle("用户登录");
        this.setLocationRelativeTo(null);
        this.setVisible(true);
    }
}
