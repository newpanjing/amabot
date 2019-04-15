using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using amabot.classs;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Threading;
namespace amabot.forms
{
    public partial class MainForm : Form
    {

        private CookieHelper cookie;

        SynchronizationContext m_SyncContext = null;
        public MainForm()
        {
            InitializeComponent();
            this.Size = new Size(900, 600);
            cookie = new CookieHelper();
            m_SyncContext = SynchronizationContext.Current;

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private Thread thread;
        private void button1_Click(object sender, EventArgs e)
        {

           int sleep = int.Parse(sleepTxt.Text);

            if (button1.Tag.ToString().Equals("0"))
            {
                thread = new Thread(delegate()
                {

                    var str = this.textBox1.Text;
                    string[] asins = str.Split('\n');
                    AsinComment ac = new AsinComment();
                    ac.sleep = sleep*1000;
                    ac.progress += new AsinComment.Progress(ac_progress);
                    ac.onComment += new AsinComment.OnComment(ac_onComment);
                    ac.start(asins);
                    //ac.getComment(asins);

                });
                thread.Start();
                button1.Tag = 1;
                button1.Text = "停止采集";
            }
            else {
                thread.Abort();
                button1.Text = "开始采集";
                button1.Tag = 0;
                ExcelExport.export(comments);
                comments.Clear();
                this.dataGridView1.Rows.Clear();
            }

        }

        void ac_onComment(Comment comment)
        {
            m_SyncContext.Post(addToTable,comment);
        }

        private List<Comment> comments = new List<Comment>();

        private void addToTable(object obj) {

            bool hasEmail = this.hasEmailCk.Checked;

            Comment comment = (Comment)obj;
            if (hasEmail) {
                if (comment.Email == null || "".Equals(comment.Email)) {
                    return;
                }
            }

            List<int> starts = getStars();
            if (starts.IndexOf(comment.Star)==-1) {
                return;
            }

            comments.Add(comment);
            DataGridViewRow row = new DataGridViewRow();

            //"昵称","邮箱","评价星级","排名","个人主页",

            DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();

            cell.Value = comment.NickName;
            row.Cells.Add(cell);

            cell = new DataGridViewTextBoxCell();
            cell.Value = comment.Email;
            row.Cells.Add(cell);

            cell = new DataGridViewTextBoxCell();
            cell.Value = comment.Star;
            row.Cells.Add(cell);


            cell = new DataGridViewTextBoxCell();
            cell.Value = comment.Rank;
            row.Cells.Add(cell);


            cell = new DataGridViewTextBoxCell();
            cell.Value = comment.Profile;
            
            row.Cells.Add(cell);

            this.dataGridView1.Rows.Add(row);
        }

        private void PostMessage(object message) {

            //防止内存爆满
            if (this.consoleTxt.Text.Length > 10000) {
                this.consoleTxt.Text = "";
            }
            this.consoleTxt.AppendText(message.ToString() + "\n");
        }

        void ac_progress(object sender, string text)
        {

            m_SyncContext.Post(PostMessage, text);
                
            
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            
            this.button1.Tag = 0;


            this.dataGridView1.RowStateChanged += new DataGridViewRowStateChangedEventHandler(dataGridView1_RowStateChanged);

            string[] headers = new string[]{
            "昵称","邮箱","评价星级","排名","个人主页"
            };

            int[] widths = new int[] { 
                100,200,100,100,500
            };

            DataGridViewColumn[] columns= new DataGridViewColumn[headers.Length];

            for (int i = 0; i < headers.Length; i++)
            {
                string name = headers[i];

                DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
               
                column.Width =widths[i];
                column.HeaderText = name;
                column.Name = "column"+name;
                columns[i] = column;

            }

            this.dataGridView1.Columns.AddRange(columns);
            
            //new AmazonLogin().ShowDialog(this);
        }

        void dataGridView1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            //e.Row.HeaderCell.Value = string.Format("{0}", e.Row.Index + 1);  
        }

        private List<int> getStars() {
            List<int> stars = new List<int>();
            foreach (Control c in startsGroup.Controls) { 
                CheckBox checkbox=(CheckBox)c;
                if (checkbox.Checked) {
                    stars.Add(checkbox.TabIndex);
                }
            }

            return stars;

        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            EmailForm form = new EmailForm();
            form.Show();
            form.LoadComment(comments);
        }
    }
}
