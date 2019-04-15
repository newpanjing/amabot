using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.IO.Compression;

namespace amabot.classs
{
    class IOUtils
    {

        /// <summary>
        /// 读取响应流
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static string getContent(HttpWebResponse response)
        {

            Encoding encode = System.Text.Encoding.GetEncoding("utf-8");

            Stream stream = response.GetResponseStream();


            if (response.ContentEncoding.ToLower().Contains("gzip"))
            {
                //gzip格式
                stream = new GZipStream(stream, CompressionMode.Decompress);
            }

            string html;

            using (System.IO.StreamReader sr = new System.IO.StreamReader(stream, Encoding.UTF8))
            {
                html = sr.ReadToEnd();
            }

            return html;
        }
    }
}
