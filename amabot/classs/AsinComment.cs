using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.IO.Compression;
using NSoup;
using NSoup.Nodes;
using System.Windows.Forms;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System;
using System.Threading;

namespace amabot.classs
{
    class AsinComment
    {

        public delegate void Progress(object sender,string text);

        public event Progress progress;

        public delegate void OnComment(Comment comment);

        public event OnComment onComment;

        /// <summary>
        /// 休眠
        /// </summary>
        public int sleep
        {

            set;
            get;
        }
        
        /// <summary>
        /// 启动任务
        /// </summary>
        /// <param name="asins"></param>
        public void start(string[] asins) { 
            foreach(string asin in asins){
                getComment(asin);
            }
        }

        /// <summary>
        /// 获取评论,返回有邮箱的
        /// </summary>
        /// <param name="asin"></param>
        /// <returns></returns>
        public void getComment(string asin) {
            progress(this, "准备解析ASIN=" + asin);
            List<Comment> comments=new List<Comment>();
            
            //获取评论页数
            string url = "https://www.amazon.com/product-reviews/"+asin;

            string cookie= CookieHelper.getCookie();

             HttpWebResponse response= HttpWebResponseUtility.CreateGetHttpResponse(url,10000,cookie);

             Encoding encode = System.Text.Encoding.GetEncoding("utf-8");

             Stream stream = response.GetResponseStream();


             if (response.ContentEncoding.ToLower().Contains("gzip"))
             {
                 //gzip格式
                 stream = new GZipStream(stream, CompressionMode.Decompress);
             }


            //如果速度太快，亚马逊返回503，就暂停程序5-10分钟
            
             string html;

             using (System.IO.StreamReader sr = new System.IO.StreamReader(stream, Encoding.UTF8))
             {
                 html = sr.ReadToEnd();
             }

             Document doc= NSoupClient.Parse(html);
             Element element= doc.Select(".a-pagination").First;

             int page = 0;

             if (element != null) {
                 element = doc.Select(".a-last").First;
                 element= element.PreviousElementSibling;
                 string text= element.Text().Replace(" ", "");

                 page= int.Parse(text);
             }
             progress(this, "解析成功ASIN=" + asin+",评论页数="+page);

             for (int i = 1; i <= page; i++)
             {

                 try
                 {
                     // 获取其他页
                     getCommentUsers(asin, i);
                 }
                 catch (Exception e) {
                     progress(this, "读取第" + i + "页评论失败，"+e.Message);
                 }
             }
            
        }

        private void getCommentUsers(string asin,int page){
            
            progress(this, "ASIN=" + asin + ",获取第" + page + "页");
            string url = "https://www.amazon.com/ss/customer-reviews/ajax/reviews/get/ref=cm_cr_arp_d_paging_btm_"+page;

            IDictionary<string,string> parameters=new Dictionary<string,string>();

            parameters.Add("sortBy", "");
            parameters.Add("reviewerType", "");
            parameters.Add("formatType", "");
            parameters.Add("filterByStar", "");
            parameters.Add("pageNumber", page.ToString());
            parameters.Add("filterByKeyword", "");
            parameters.Add("shouldAppend", "undefined");
            parameters.Add("deviceType", "desktop");
            parameters.Add("reftag", "cm_cr_getr_d_paging_btm_"+page);
            parameters.Add("pageSize", "10");
            parameters.Add("asin", asin);
            parameters.Add("scope", "reviewsAjax1");

             HttpWebResponse response= HttpWebResponseUtility.CreatePostHttpResponse(url, parameters,10000,Encoding.UTF8,CookieHelper.getCookie());
             string content= IOUtils.getContent(response);

             string []array=Regex.Split(content, "&&&", RegexOptions.IgnoreCase);

            foreach(string item in array){
                if (item.IndexOf("#cm_cr-review_list") != -1) { 

                    //开始时间
                    //结束时间

                    //休眠=(休眠时间-(结束时间-开始时间))>0

                    //解析json
                    JArray jarray=(JArray) JsonConvert.DeserializeObject(item);

                    if (jarray[0].ToString().IndexOf("append") == -1) {
                        continue;
                    }

                    string commentInfo = jarray[2].ToString();

                    Document doc= NSoupClient.Parse(commentInfo);
                    Element element= doc.Select(".a-icon-star").First;

                    //评分
                    int start = 0;

                    if (element != null) {
                    
                    for(int i=1;i<=5;i++){
                        if (element.HasClass("a-star-" + i)) {
                            start = i;
                            break;
                        }
                    }
                    }
                    //昵称
                    element = doc.Select("a[data-hook=review-author]").First;
                    if (element == null) {
                        continue;
                    }
                    string nickName =element.Text();
                    

                    string profile ="https://www.amazon.com"+ element.Attr("href");

                    //获取个人资料
                    Dictionary<string, string> profileInfo = null;

                    try
                    {
                        profileInfo = getProfile(profile);
                    }
                    catch (Exception e) {
                        progress(this, "读取个人主页报错 profile="+progress+" error ="+e.Message);
                    }

                    if (profileInfo == null) {
                        continue;
                    }

                    string email = profileInfo["email"];
                    if (email != null && !"".Equals(email)) {
                        //MessageBox.Show(email);
                    }
                    progress(this, "ASIN=" + asin + ",昵称=" + nickName + ",评分=" + start + ",邮箱=" + email);
                    
                    //
                    Comment comment=new Comment();
                    comment.Star=start;
                    comment.Email=email;
                    comment.Rank=0;
                    comment.NickName=nickName;
                    comment.Profile = profile;


                    long beginTime = GetUnixTime(DateTime.Now);
                    string rank= profileInfo["rank"];
                    if (rank != null) { 
                        rank=rank.Replace(",","");
                        if (!rank.Equals(""))
                        {
                            comment.Rank = int.Parse(rank);
                        }
                    }
                    this.onComment(comment);

                    //结束时间-开始时间<=sleep
                    long endTime=GetUnixTime(DateTime.Now);

                    long value=endTime-beginTime;
                    if(value<=sleep){
                        Thread.Sleep((int)(sleep-value));
                    }
                }
            }
            
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <param name="nowTime">当前时间</param>
        /// <returns>时间戳(type:long)</returns>
        static long GetUnixTime(DateTime nowTime)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            return (long)Math.Round((nowTime - startTime).TotalMilliseconds, MidpointRounding.AwayFromZero);
        }


        /// <summary>
        /// 获取个人资料，邮箱、排名
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        public Dictionary<string, string> getProfile(string profile)
        {

            Dictionary<string, string> rs = new Dictionary<string, string>();

            string cookieString = CookieHelper.getCookie();



            HttpWebResponse response = HttpWebResponseUtility.CreateGetHttpResponse(profile, 10000, cookieString);
            string html= IOUtils.getContent(response);

            rs.Add("email", JsonMatch.GetJSONValue(html, "publicEmail"));
            rs.Add("rank", JsonMatch.GetJSONValue(html, "rank"));
            //MessageBox.Show("ok");

            return rs;
        }


    }
}
