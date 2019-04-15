using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using amabot.classs;

namespace amabot
{
    public partial class AmazonLogin : Form
    {
        public AmazonLogin()
        {
            InitializeComponent();
        }

        //对错误进行处理  
        void Window_Error(object sender, HtmlElementErrorEventArgs e)
        {
            // 自己的处理代码  
            e.Handled = true;
        } 

        private void AmazonLogin_Load(object sender, EventArgs e)
        {
            this.webBrowser1.ScriptErrorsSuppressed = true;
            
 
            this.webBrowser1.Navigate("https://www.amazon.com/ap/signin?_encoding=UTF8&openid.assoc_handle=usflex&openid.claimed_id=http%3A%2F%2Fspecs.openid.net%2Fauth%2F2.0%2Fidentifier_select&openid.identity=http%3A%2F%2Fspecs.openid.net%2Fauth%2F2.0%2Fidentifier_select&openid.mode=checkid_setup&openid.ns=http%3A%2F%2Fspecs.openid.net%2Fauth%2F2.0&openid.ns.pape=http%3A%2F%2Fspecs.openid.net%2Fextensions%2Fpape%2F1.0&openid.pape.max_auth_age=0&openid.return_to=https%3A%2F%2Fwww.amazon.com%2F%3Fref_%3Dnav_custrec_signin");
            this.webBrowser1.Navigated += new WebBrowserNavigatedEventHandler(webBrowser1_Navigated);
        }

        void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            //捕获控件的错误  
            this.webBrowser1.Document.Window.Error += new HtmlElementErrorEventHandler(Window_Error);  

            string cookie= this.webBrowser1.Document.Cookie;
            
            //MessageBox.Show(cookie);
            //https://www.amazon.com/?ref_=nav_custrec_signin
            var url = this.webBrowser1.Url.ToString();
            if (url.IndexOf("https://www.amazon.com/?ref_=nav_custrec_signin") == 0)
            {
                //MessageBox.Show(cookie);
                //登陆成功，写cookie
                CookieHelper.Write(cookie);

                //关闭当前
                this.Dispose();
            }
            

        }
    }
}
