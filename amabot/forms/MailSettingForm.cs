using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using amabot.classs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace amabot.forms
{
    public partial class MailSettingForm : Form
    {
        public MailSettingForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //测试
            var account = accountTxt.Text;
            var password = passwordTxt.Text;
            var stmp = stmpTxt.Text;
            var port = portTxt.Text;
            try
            {
                MailFactory.Send(account, password, stmp, int.Parse(port), "599194993@qq.com", "邮件设置测试", "邮件设置测试");

                var json = "{account:\""+account+"\",password:\""+password+"\",smtp:\""+stmp+"\",port:"+port+"}";
                //写入文件
                FileHelper.Write("mail.inf",json);

                this.Visible = false;
            }catch (Exception ex) {
                MessageBox.Show("邮件设置失败，请检查用户名或密码！");
            }



        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void MailSettingForm_Load(object sender, EventArgs e)
        {

            //读取
            if (FileHelper.Exists("mail.inf"))
            {
                string content = FileHelper.readFile("mail.inf");
                JObject jobject = (JObject)JsonConvert.DeserializeObject(content);
                this.accountTxt.Text = jobject["account"].ToString();
                this.passwordTxt.Text = jobject["password"].ToString();
                this.stmpTxt.Text = jobject["smtp"].ToString();
                this.portTxt.Text = jobject["port"].ToString();

            }

            this.accountTxt.LostFocus += new EventHandler(textBox1_LostFocus);
        }

        void textBox1_LostFocus(object sender, EventArgs e)
        {
            //设置stmp
            var val = this.accountTxt.Text;
            if (val.IndexOf("@") != -1) {
                var array=val.Split('@');
                if (array.Length > 1) {
                    stmpTxt.Text= "smtp."+array[1];
                }
            }
        }
    }
}
