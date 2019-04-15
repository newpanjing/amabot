using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace amabot.classs
{
    class FileHelper
    {

        public static bool Exists(string fileName) {

            string root = Application.StartupPath.ToString();
            var file = root + "\\" + fileName;
            return File.Exists(file);
        }

        public static void Write(string fileName,string content) {

            string root = Application.StartupPath.ToString();

            FileStream fs = new FileStream(root + "\\"+fileName, FileMode.Create);
            //获得字节数组
            byte[] data = System.Text.Encoding.Default.GetBytes(content);
            //开始写入
            fs.Write(data, 0, data.Length);
            //清空缓冲区、关闭流
            fs.Flush();
            fs.Close();
        }

        public static string readFile(string fileName) {
            string root = Application.StartupPath.ToString();

            StreamReader sr = new StreamReader(root + "\\"+fileName, Encoding.Default);
            StringBuilder sb = new StringBuilder();

            String line;
            while ((line = sr.ReadLine()) != null)
            {
                sb.Append(line);
            }

            return sb.ToString();
        }
    }
}
