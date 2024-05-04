package GameUI;

import javax.swing.*;
import javax.swing.border.BevelBorder;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.KeyEvent;
import java.awt.event.KeyListener;
import java.util.Random;

public class GameJFrame extends JFrame implements KeyListener, ActionListener {

    private int[][] data = new int[4][4];

    //定义一个获胜数组
    private int[][] win = new int[][]{
            {1,2,3,4},{5,6,7,8},{9,10,11,12},{13,14,15,0}
    };
    int[] tmpArr = {0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15};

    // x,y 表示空白图片在整个图片数组中的位置
    // 记录该位置是为了方便将空白图片与上下左右四个位置的图片进行交换
    private int x;
    private int y;

    private int photoNum = 1;
    private String photoPath = "Attachment\\image\\animal\\animal" + photoNum + "\\";

    //定义一个变量,统计游戏步数
    private int step;

    /**
     * 因为接口实现的方法中需要比较对象,所以菜单选项创建的内容都声明到类中
     */
    JMenuItem replayItem = new JMenuItem("重新游戏");
    JMenuItem reLoginItem = new JMenuItem("重新登陆");
    JMenuItem closeItem = new JMenuItem("关闭游戏");
    JMenuItem accountItem = new JMenuItem("公众号");
    JMenuItem girlPhItem = new JMenuItem("美女");
    JMenuItem animalPhItem = new JMenuItem("动物");
    JMenuItem sportsPhItem = new JMenuItem("运动");

    public GameJFrame() {
        /* 初始化界面 */
        initJFrame();

        /* 初始化 */
        initJMenuBar();

        /* 初始化数据(就是打乱图片的显示) */
        initData();

        /* 初始化图片方法(根据打乱以后的结果加载图片) */
        initImage(photoPath);

        //设置页面可见的语句最好放最后面,为了界面所有组件的可见
        this.setVisible(true);
    }

    private void initData() {
        Random r = new Random();
        for (int i = 0; i < tmpArr.length; i++) {
            //生成一个随机索引
            int index = r.nextInt(tmpArr.length);
            int tmp = tmpArr[i];
            tmpArr[i] = tmpArr[index];
            tmpArr[index] = tmp;
        }
        //同时还要将x,y的数据进行返回,也就是空白块的内容
        for (int i = 0; i < tmpArr.length; i++) {
            if(tmpArr[i] == 0){
                x = i / 4;
                y = i % 4;
            }
            data[i / 4][i % 4] = tmpArr[i];
        }
    }

    /**
     * 初始化游戏界面
     */
    private void initJFrame() {
        //设置界面基础信息
        this.setSize(603, 680);

        this.setTitle("拼图游戏单机版");
        //设置游戏页面始终保持置顶
        this.setAlwaysOnTop(true);
        //设置页面居中
        this.setLocationRelativeTo(null);
        //设置游戏的关闭模式 -> 点击页面的 × 直接停止程序
        this.setDefaultCloseOperation(3);
        //给页面添加按键监听器,监听按键事件
        this.addKeyListener(this);
    }

    /**
     * 初始化菜单栏
     */
    private void initJMenuBar() {
        //给游戏主界面添加菜单栏
        JMenuBar jMenuBar = new JMenuBar();

        JMenu functionJMenu = new JMenu("功能");
        JMenu aboutJMenu = new JMenu("关于我们");
        //实现二级菜单,就是要将二级菜单作为JMenu对象嵌套到JMenu中
        JMenu changePh = new JMenu("更换图片");

        //给菜单选项添加键盘点击事件
        replayItem.addActionListener(this);
        reLoginItem.addActionListener(this);
        closeItem.addActionListener(this);
        accountItem.addActionListener(this);
        girlPhItem.addActionListener(this);
        animalPhItem.addActionListener(this);
        sportsPhItem.addActionListener(this);

        //将菜单选项添加到菜单中
        changePh.add(girlPhItem);
        changePh.add(sportsPhItem);
        changePh.add(animalPhItem);
        functionJMenu.add(changePh);

        functionJMenu.add(replayItem);
        functionJMenu.add(reLoginItem);
        functionJMenu.add(closeItem);
        aboutJMenu.add(accountItem);

        //将菜单添加到菜单bar中
        jMenuBar.add(functionJMenu);
        jMenuBar.add(aboutJMenu);

        this.setJMenuBar(jMenuBar);
    }

    /**
     * 初始化图片方法
     * 添加数据的时候,按照二维数组中的内容添加数据
     */
    private void initImage(String photoPath) {

        this.getContentPane().removeAll();

        if(victory()){
            JLabel winJLabel = new JLabel(new ImageIcon("Attachment\\image\\win.png"));
            winJLabel.setBounds(203,283,197,73);
            this.add(winJLabel);
            this.getContentPane().repaint();
        }

        //首先需要取消窗体默认的居中方式,只有取消了,才会按照x,y轴的方式进行添加
        this.setLayout(null);

        JLabel stepCount = new JLabel("step:" + step);
        stepCount.setBounds(50,30,100,20);
        this.add(stepCount);

        //通过循环,将所有的图片都加载到页面当中去
        for (int i = 0; i < data.length; i++) {
            for (int j = 0; j < data[i].length; j++) {
                //表示随机一个位置的图片添加进来
                int number = data[i][j];

                //优化,使用相对路径. 注意:相对路径是相对项目位置而言的
                JLabel jLabel = new JLabel(new ImageIcon(photoPath + number + ".jpg"));
                // 83 & 134 两个值是为了让游戏主界面位于整体界面中间偏下方的位置处
                jLabel.setBounds(105 * j + 83, 105 * i + 134, 105, 105);

                //给每个图片对象添加边框效果
                //BevelBorder 是一种斜面边框, 0 表示凸起来的效果
                jLabel.setBorder(new BevelBorder(0));

                this.add(jLabel);
            }
        }
        //将背景图片添加到窗体中
        ImageIcon bg = new ImageIcon("Attachment\\image\\background.png");
        JLabel background = new JLabel(bg);
        //setBounds 是要给背景图片这个Jlabel对象设置大小以及位置
        background.setBounds(40,40,508,560);
        this.add(background);
        this.getContentPane().repaint();
    }

    public boolean victory(){
        for (int i = 0; i < data.length; i++) {
            for (int j = 0; j < data[i].length; j++) {
                if(data[i][j] != win[i][j]) {
                    return false;
                }
            }
        }
        return true;
    }

    /**
     * KeyTyped 方法因为有bug,不能监听到某些按键,所以一般不使用该方法
     */
    @Override
    public void keyTyped(KeyEvent e) {}

    /**
     * 按下不松的时候,会调用这个方法
     * @param e the event to be processed
     */
    @Override
    public void keyPressed(KeyEvent e) {

        int code = e.getKeyCode();
        if(code == 65){
            this.getContentPane().removeAll();
            JLabel jLabel = new JLabel(new ImageIcon(photoPath + "all.jpg"));
            jLabel.setBounds(83,134,420,420);
            this.add(jLabel);
            //将背景图片添加到窗体中
            ImageIcon bg = new ImageIcon("Attachment\\image\\background.png");
            JLabel background = new JLabel(bg);
            //setBounds 是要给背景图片这个Jlabel对象设置大小以及位置
            background.setBounds(40,40,508,560);
            this.add(background);
            this.getContentPane().repaint();
        }
    }

    /**
     * 根据键盘事件,实现交换图片的逻辑
     * @param e the event to be processed
     */
    @Override
    public void keyReleased(KeyEvent e) {
        if(victory()){
            return;
        }

        int code = e.getKeyCode();

        // 左:37 上:38 右:39 下:40
        if(code == 37){
            System.out.println("按下左键");
            if(y == 3){
                // 判断当前是否到达了页面的最右边,避免出现异常
                return;
            }else{
                //实际上是把空白图片右边的往左移动
                data[x][y] = data[x][y+1];
                data[x][y+1] = 0; //原图片位置设置为空白
                y++; //更新空白图片的位置
                step++;
                initImage(photoPath);
            }
        } else if (code == 38) {
            System.out.println("按下上键");
            if(x==3){
                return;
            }else{
                //实际上是把空白图片下边的往左上移动
                data[x][y] = data[x+1][y];
                data[x+1][y] = 0; //原图片位置设置为空白
                x++; //更新空白图片的位置
                step++;
                initImage(photoPath);
            }
        } else if (code == 40) {
            System.out.println("按下下键");
            if(x == 0){
                return;
            }else {
                //实际上是把空白图片上边的往下移动
                data[x][y] = data[x-1][y];
                data[x-1][y] = 0; //原图片位置设置为空白
                x--; //更新空白图片的位置
                step++;
                initImage(photoPath);
            }
        } else if (code == 39) {
            System.out.println("按下右键");
            if(y == 0){
                return;
            }else{
                data[x][y] = data[x][y-1];
                data[x][y-1] = 0;
                y--;
                step++;
                initImage(photoPath);
            }
        } else if(code == 65){
            initImage(photoPath);
        } else if(code == 87){
            data = new int[][] {
                {1,2,3,4},
                {5,6,7,8},
                {9,10,11,12},
                {13,14,15,0}
            };
            initImage(photoPath);
        }else{
            return;
        }
    }

    @Override
    public void actionPerformed(ActionEvent e) {
        Object obj = e.getSource();
        if(obj == replayItem){
            System.out.println("重新游戏");
            //步数计数器重新赋值
            step = 0;
            //重新打乱数据
            initData();
            //重新加载图片
            initImage(photoPath);
        }else if(obj == reLoginItem){
            System.out.println("重新登录");
            //隐藏当前游戏界面
            this.setVisible(false);
            //重新创建一个登录界面的对象
            new LoginJFrame();

        } else if (obj == closeItem) {
            System.out.println("关闭游戏");
            System.exit(0);
        } else if (obj == accountItem) {
            System.out.println("公众号");

            // 创建一个显示公众号图片的弹窗
            JDialog jDialog = new JDialog();
            JLabel jLabel = new JLabel(new ImageIcon("Attachment\\image\\about.png"));
            jLabel.setBounds(0,0,258,258);
            jDialog.getContentPane().add(jLabel);
            jDialog.setSize(300,300);
            jDialog.setTitle("关于我们");
            // 设置弹窗居中显示
            jDialog.setLocationRelativeTo(null);
            // 设置弹窗置顶显示
            jDialog.setAlwaysOnTop(true);
            // 设置弹窗不关闭则不能点击下面弹窗的内容
            jDialog.setModal(true);
            // 设置弹窗可见
            jDialog.setVisible(true);
        } else if (obj == girlPhItem) {
            System.out.println("更换图片");
            //点击更换图片的时候,随机从后台文件中找到一个目录
            //更新photoPath这个表示图片路径的字符串

            //生成一个从 1-13 (两边都包括) 的随机数
            Random r = new Random();
            photoNum = r.nextInt(13) + 1;
            photoPath = "Attachment\\image\\girl\\girl" + photoNum + "\\";
            initImage(photoPath);

        } else if (obj == animalPhItem) {
            System.out.println("更换图片");
            Random r = new Random();
            photoNum = r.nextInt(8) + 1;
            photoPath = "Attachment\\image\\animal\\animal" + photoNum + "\\";
            initImage(photoPath);

        } else if (obj == sportsPhItem) {
            System.out.println("更换图片");
            Random r = new Random();
            photoNum = r.nextInt(10) + 1;
            photoPath = "Attachment\\image\\sport\\sport" + photoNum + "\\";
            initImage(photoPath);
        }
    }
}
