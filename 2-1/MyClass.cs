using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//使用面向对象的思想，模拟现实世界中的银行、账号、ATM等对象，其中类里面应该包含字段、属性、方法、索引器等，并注意使用修饰符；//okk
//使用继承，继承账号（Account类）得到一个子类（如信用账号），增加字段（如信用额度）、属性、方法，覆盖（Override）一些方法；//okk
//使用事件和委托，实现一个事件BigMoneyFetched（取走大笔金额），即ATM机操作时如果用户取款数大于10000元，则可以激活该事件。//okk
//事件的参数也是一个对象（可以定义类BigMoneyArgs），含有账号及当时的取款数。在程序中注册该事件，使之能在界面中显示告警信息。//okk
//程序中使用自定义异常，比如定义一个异常类BadCashException，表示有坏的钞票，在程序中适当的地方（比如使用Random模拟坏钞率为30%左右）抛出自定义的异常类，在ATM调用时捕获该异常//okk
namespace _2_1
{
    class Bank//银行
    {
        string name;//银行名字
        string location;//银行位置
        private List<Account> accounts = new List<Account>(1000);//账号列表
        public ATM[] atms = new ATM[100];//ATM机列表
        public Bank(string name,string location)
        {
            this.name = name;
            this.location = location;
        }
        public Bank()
        {
        }
        public Account this[int idx]//索引器
        {
            get
            {
                if (idx >= 0 && idx < accounts.Count)
                    return accounts[idx];
                else
                    return null;
            }
        }
        public List<Account> GetList()
        {
            return accounts;
        }
        public string Name
        {
            get
            {
                return name;
            }
        }
        public string Location
        {
            get
            {
                return location;
            }
        }
    }

    class Account//账号
    {
        public Account(String id, String password, int money)//构造函数
        {
            this.id = id;
            this.password = password;
            this.money = money;
        }
        public Account()
        {

        }
        private String id;//id
        private String password;//密码
        private int money;//存款
        public String ID//id
        {
            get
            {
                return id;
            }
        }
        public String Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
            }
        }
        public int Money//存款
        {
            get
            {
                return money;
            }
            set
            {
                money = value;
            }
        }
        public void Addmoney(int n)
        {
            money += n;

        }//存钱
        public void Takemoney(int n)
        {
            money -= n;
        }//取钱
        public virtual bool hasmoneyornot()
        {
            return (money > 0);
        }//有没有钱
    }

    class ATM//ATM
    {
        int id;//编号
        int havemoney;//拥有的资金
        string name;//所属银行名字
        string location;//位置
        public int Havemoney
        {
            get
            {
                return havemoney;
            }
            set
            {
                havemoney = value;
            }
        }
        public ATM()
        {

        }
        public ATM(int id, string name, string location,int havemoney)
        {
           this.id=id;
           this.location = location ;
           this.name = name ;
            this.havemoney = havemoney;
        }
        public void takemoney(Account nowaccount,int money)
        {
            nowaccount.Takemoney(money);
            Random rd = new Random();
            int i = rd.Next(0, 10);
            if (i > 7)
                throw new BadCashException();
        }
        public void savemoney(Account nowaccount, int money)
        {
            nowaccount.Addmoney(money);
        }
    }

    class CreditAccout : Account//信用账号
    {
        private int credit;//信用额度
        public int Credit//额度
        {
            get
            {
                return credit;
            }
        }
        public CreditAccout(String id, String password, int money, int credit) : base(id, password, money)
        {
            this.credit = credit;
        }//构造方法
        public void setcredit(int n)
        {
            credit = n;
        }//设置额度
        public override bool hasmoneyornot()
        {
            return (Money + credit > 0);
        }//有没有钱
    }

    public class BigMoneyArgs:EventArgs//大笔金额事件
    {
        public String id;//账号
        public int moneytaken;//当时的取款数
        public BigMoneyArgs(String Id,int Moneytaken)
        {
            this.id = Id;
            this.moneytaken = Moneytaken;
        }
            
    }

    public class BadCashException : Exception//异常类
    {
        public override string Message
        {
            get { return "坏钞了！"; }
        }
    }
}

