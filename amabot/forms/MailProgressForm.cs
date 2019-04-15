using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace amabot.forms
{
    public partial class MailProgressForm : Form
    {

        SynchronizationContext m_SyncContext = null;
        public MailProgressForm()
        {
            InitializeComponent();
            m_SyncContext = SynchronizationContext.Current;
        }

        public int Current
        {

            set;
            get;
        }

        public int Total
        {
            set;
            get;
        }


        public void ShowInfo(object obj) {
            float value = (float)Current / (float)Total;
            this.label1.Text = string.Format("{0:P}", value);
            progressBar1.Value = (int)(value * 100);

            this.label2.Text = "正在发生第" + Current + "封邮件，总共" + Total + "封";
        }

        private void ThreadHander() {

            while (true) {
                if (Total == 0) {

                    continue;
                }

                if (Current == Total) {
                    break;
                }
                
                m_SyncContext.Post(ShowInfo,null);
                Thread.Sleep(100);
            }
        }
        private void MailProgressForm_Load(object sender, EventArgs e)
        {
            Thread thread = new Thread(ThreadHander);
            thread.Start();
        }
    }
}
