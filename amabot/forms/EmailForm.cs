using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using amabot.classs;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Threading;

namespace amabot.forms
{
    public partial class EmailForm : Form
    {

        SynchronizationContext sc;
        public EmailForm()
        {
            InitializeComponent();
           sc= SynchronizationContext.Current;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            MailProgressForm form= new MailProgressForm();

            Thread thread = new Thread(delegate() {

                string content = FileHelper.readFile("mail.inf");
                JObject jobject = (JObject)JsonConvert.DeserializeObject(content);
                var account = jobject["account"].ToString();
                var password = jobject["password"].ToString();
                var smtp = jobject["smtp"].ToString();
                var port = jobject["port"].ToString();

                form.Total = mailList.Items.Count;

                var index = 0;
                foreach (object o in mailList.Items)
                {
                    index++;
                    form.Current = index + 1;
                    MailFactory.Send(account, password, smtp, int.Parse(port), o.ToString(), subjectTxt.Text, contentTxt.Text);
                }
                sc.Post(delegate(object obj)
                {
                    form.Visible = false;
                }, null);
                
                
            });

            thread.Start();

            form.ShowDialog(this);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new MailSettingForm().ShowDialog(this);
        }

        private void EmailForm_Load(object sender, EventArgs e)
        {
            this.Show();
            if (!FileHelper.Exists("mail.inf")) {
                new MailSettingForm().ShowDialog(this);
            }

            for (int i = 0; i < 100; i++)
            {
                this.mailList.Items.Add("599194993@qq.com");
                this.mailList.Items.Add("1140335125@qq.com");
                this.mailList.Items.Add("newpanjing@icloud.com");
            }

        }

        public void LoadComment(List<Comment> comments) {
            foreach (Comment c in comments) {
                if (c.Email == null || c.Email.Equals("")) {
                    continue;
                }
                this.mailList.Items.Add(c.Email);
            }
        }
    }
}
