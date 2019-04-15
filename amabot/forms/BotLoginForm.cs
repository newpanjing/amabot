using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace amabot.forms
{
    public partial class BotLoginForm : Form
    {
        public BotLoginForm()
        {
            InitializeComponent();
        }

        private void BotLoginForm_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            new MainForm().Show();
            this.Visible = false;
        }
    }
}
