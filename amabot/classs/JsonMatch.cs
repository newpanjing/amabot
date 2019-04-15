using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace amabot.classs
{
    class JsonMatch
    {
        /// <summary>
        /// 邮箱格式验证
        /// </summary>
        /// <returns></returns>
        public static string GetJSONValue(string str,string key)
        {
            string result = null;
            Regex regex = new Regex("\""+key+"\":\"(.*?)\"");
            Match match = regex.Match(str);
            if (match.Success)
            {
                result = match.Groups[1].Value;
            }
          

            return result;
        }
    }
}
