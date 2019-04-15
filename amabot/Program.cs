using System;
using System.Windows.Forms;
using amabot.forms;
using amabot.classs;
using System.Collections.Generic;

namespace amabot
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //try
            //{
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new EmailForm());

            //}
            //catch (Exception ex) {
            //    MessageBox.Show(ex.StackTrace);
            //}
           //string html= FileHelper.readFile("profile.txt");
           //MessageBox.Show(EmailMatch.GetMail(html));

           
        }
    }
}
