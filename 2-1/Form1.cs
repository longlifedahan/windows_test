using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//使用面向对象的思想，模拟现实世界中的银行、账号、ATM等对象，其中类里面应该包含字段、属性、方法、索引器等，并注意使用修饰符；
//使用继承，继承账号（Account类）得到一个子类（如信用账号），增加字段（如信用额度）、属性、方法，覆盖（Override）一些方法；
//使用事件和委托，实现一个事件BigMoneyFetched（取走大笔金额），即ATM机操作时如果用户取款数大于10000元，则可以激活该事件。
//事件的参数也是一个对象（可以定义类BigMoneyArgs），含有账号及当时的取款数。在程序中注册该事件，使之能在界面中显示告警信息。
//程序中使用自定义异常，比如定义一个异常类BadCashException，表示有坏的钞票，在程序中适当的地方（比如使用Random模拟坏钞率为30%左右）抛出自定义的异常类，在ATM调用时捕获该异常。
//可在程序中适当使用接口、结构、枚举等语法成分。

namespace _2_1
{
    public partial class Form1 : Form
    {
        Bank mybank = new Bank("大司马银行","芜湖");
        int num = -1;//账号编号
        public delegate void BigMoneyEventHandler(object sender, BigMoneyArgs e);
        public event BigMoneyEventHandler Bigmoney;
        public Form1()
        {
            InitializeComponent();
            mybank.GetList().Add(new Account("1", "123456", 100000));
            mybank.GetList().Add(new Account("2", "123456", 2000));
            mybank.GetList().Add(new Account("3", "123456", 3000));
            mybank.GetList().Add(new Account("4", "123456", 4000));
            mybank.GetList().Add(new Account("5", "123456", 5000));
            mybank.atms[0] = new ATM(0,mybank.Name, mybank.Location,50000);//生成1个ATM机
            Save.Visible = false;
            Take.Visible = false;
            quit.Visible = false;
            this.Bigmoney += BigMoneyAppear;
            label1.Text += mybank.Name;
            ATMmoneychange();
        }

        private void button3_Click(object sender, EventArgs e)//登陆
        {
            string myid = id.Text;
            num = -1;
            for (int i = 0; i < mybank.GetList().Count; i++)
            {
                if (mybank[i].ID == myid)
                    num = i;
            }
            if (num == -1)
            {
                listBox1.Items.Add("未找到银行账号");
            }
            else
            {
                if (mybank.GetList()[num].Password == pass.Text)
                {
                    Save.Visible = true;
                    Take.Visible = true;
                    quit.Visible = true;
                    id.Enabled = false;
                    pass.Enabled = false;
                    button3.Visible = false;//不能再次登陆

                    money.Text = mybank.GetList()[num].Money.ToString();
                }
                else
                {
                    listBox1.Items.Add("密码错误");
                    Save.Visible =false;
                    Take.Visible =false;
                    money.Text = "";
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)//取钱
        {
            if (int.Parse(number.Text) > mybank.GetList()[num].Money)
                listBox1.Items.Add("取的钱太多了");
            else
            {
                if(int.Parse(number.Text)>mybank.atms[0].Havemoney)
                {
                    MessageBox.Show("ATM机的钱不够了！");
                }
                else
                {
                    try
                    {
                        mybank.atms[0].takemoney(mybank.GetList()[num], int.Parse(number.Text));
                        mybank.atms[0].Havemoney -= int.Parse(number.Text);
                    }
                    catch (BadCashException a)
                    {
                        ATMmoneychange();
                        mybank.atms[0].Havemoney -= int.Parse(number.Text);
                        MessageBox.Show(a.Message);
                       
                    }
                    listBox1.Items.Add("取了" + int.Parse(number.Text) + "元");
                    money.Text = mybank.GetList()[num].Money.ToString();
                    ATMmoneychange();
                    if (int.Parse(number.Text) > 10000)
                    {
                        BigMoneyArgs args = new BigMoneyArgs(mybank.GetList()[num].ID, int.Parse(number.Text));
                        Bigmoney(this, args);
                    }
                }              
            }
            ATMmoneychange();
        }

        private void button4_Click(object sender, EventArgs e)//存钱
        {
            if(int.Parse(number.Text)<0)
                listBox1.Items.Add("存的钱必须大于0");
            else
            {
                try
                {
                    mybank.atms[0].savemoney(mybank.GetList()[num], int.Parse(number.Text));
                    mybank.atms[0].Havemoney += int.Parse(number.Text);
                }
                catch
                {
                }
                listBox1.Items.Add("存了"+ int.Parse(number.Text)+"元");
                money.Text = mybank.GetList()[num].Money.ToString();
            }
            ATMmoneychange();
        }

        static void BigMoneyAppear(object sender,BigMoneyArgs e)
        {
            MessageBox.Show("提示！账户：" + e.id + "取了：" + e.moneytaken + "元！");
        }

        private void quit_Click(object sender, EventArgs e)
        {
            id.Enabled = true;
            pass.Enabled = true;
            quit.Visible = false;
            button3.Visible = true;//允许登陆
        }

        private void ATMmoneychange()
        {
            label7.Text = "ATM机金额：" + mybank.atms[0].Havemoney;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
