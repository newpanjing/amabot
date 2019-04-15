using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace amabot.classs
{
    class CookieHelper
    {

        /// <summary>
        /// 写cookie
        /// </summary>
        /// <param name="cookie"></param>
        public static void Write(String cookie) {
            string root= Application.StartupPath.ToString();//

            FileStream fs = new FileStream(root+"\\cookie.inf", FileMode.Create);
            //获得字节数组
            byte[] data = System.Text.Encoding.Default.GetBytes(cookie);
            //开始写入
            fs.Write(data, 0, data.Length);
            //清空缓冲区、关闭流
            fs.Flush();
            fs.Close();


            //MessageBox.Show(root);
        }

        /// <summary>
        /// 读取cookie
        /// </summary>
        /// <returns></returns>
        public static string getCookie() {

            string root = Application.StartupPath.ToString();

            StreamReader sr = new StreamReader(root+"\\cookie.inf", Encoding.Default);
            StringBuilder sb = new StringBuilder();

            String line;
            while ((line = sr.ReadLine()) != null)
            {
                sb.Append(line);
                //Console.WriteLine(line.ToString());
            }

            return sb.ToString();
        }


    }
}
