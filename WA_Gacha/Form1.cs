using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WA_Gacha
{
    public partial class Form1 : Form
    {
        public const string Pattern = @"^[0-9]*$";
        public static decimal Total = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormViewModel model = BindModel();

            decimal money;
            GetCoin();
            decimal.TryParse(GetCoin(), out money);
            model.Money = money;
            Total += model.Money;
            GenerateStatus(Total);
        }

        private void GenerateStatus(decimal money)
        {
            if (money > 0 && money < 50)
            {
                button2.Enabled = true;
                textBox2.Text = string.Format(@"State:InsufficientAmountState 有投幣,但金額還不足夠" + "\r\n" + "Money:{0}", money);
                return;
            }
            if (money >= 50)
            {
                button2.Enabled = true;
                button3.Enabled = true;

                textBox2.Text = string.Format(@"State:ReadyToShip 投入足夠金額,等待客人指令就出貨" + "\r\n" + "Money:{0}", money);
                return;
            }

            textBox2.Text = string.Format(@"State:StandbyState 機器沒在工作,等待下一個客人操作" + "\r\n" + "Money:{0}", money);
        }

        private string GetCoin()
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                return 0.ToString();

            }
            if (textBox1.Text.Length <= 0)
            {
                return 0.ToString(); //_model.Money = 0;
            }
            if (Regex.IsMatch(textBox1.Text, Pattern) == false)
            {
                return 0.ToString();
            }

            if (int.Parse(textBox1.Text) <= 0)
            {
                return 0.ToString();
            }
            return textBox1.Text;
        }

        private FormViewModel BindModel()
        {
            var model = new FormViewModel();
            model.Coin = textBox1.Text;
            model.State = textBox2.Text;
            //model.Money +=  model.Coin;
            return model;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            Total -= Total;
            GenerateReturnStatus(Total);
        }

        private void GenerateReturnStatus(decimal money)
        {
            MessageBox.Show(@"已完成退幣作業");
            button2.Enabled = false;
            button3.Enabled = false;

            textBox2.Text = string.Format(@"State:StandbyState 機器沒在工作,等待下一個客人操作 " + "\r\n" + "Money:{0}", money);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Total = Total - 50;
            GenerateShippedStatus(Total);
        }

        private void GenerateShippedStatus(decimal money)
        {
            var model = BindModel();
            model.Money = money;
           // model.Money = money - 50;
            if (model.Money >= 50)
            {
                MessageBox.Show(@"已出貨");
                button2.Enabled = true;
                button3.Enabled = true;
                textBox2.Text = string.Format(@"State:ReadyToShip 投入足夠金額,等待客人指令就出貨" + "\r\n" + "Money:{0}", model.Money);
                return;
            
            }else if (model.Money > 0 && model.Money < 50)
            {
                button2.Enabled = true;
                button3.Enabled = false;
                textBox2.Text = string.Format(@"State:InsufficientAmountState 有投幣,但金額還不足夠" + "\r\n" + "Money:{0}", model.Money);
            }
            else
            {
                button2.Enabled = false;
                button3.Enabled = false;

                textBox2.Text = string.Format(@"State:ReadyToShip 投入足夠金額,等待客人指令就出貨" + "\r\n" + "Money:{0}",
        model.Money);
            }
        }
    }

    public class FormViewModel
    {
        public string Coin { get; set; }
        public string State { get; set; }
        public decimal Money { get; set; }
    }
}
