using System;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using System.Windows.Forms;
using System.Diagnostics;

namespace Animation.Utils
{
    class NetUtils
    {
        public static String getHtml(String url) {
            Stopwatch sw = new Stopwatch();
            Console.WriteLine("getHtml begin");
            sw.Start();
            WebClient webClient = new WebClient();
            //byte[] reqHTML;
            String temp = null;
            webClient.Encoding = Encoding.UTF8;
            webClient.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            webClient.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:17.0) Gecko/20100101 Firefox/17.0");
            temp = webClient.DownloadString(url);
            webClient.Dispose();
            sw.Stop();
            Console.WriteLine("getHtml done：" + sw.Elapsed.ToString());
            return Strings.StrConv(temp, VbStrConv.SimplifiedChinese, 0);
        }

        public static string stripHtml(string strhtml) {
            string stroutput = strhtml;
            Regex regex = new Regex(@"<[^>]+>|</[^>]+>|amp;");
            stroutput = regex.Replace(stroutput, "");
            return stroutput;
        }

        public static String formateHtml(String html) {
            string stroutput = html;
            Regex regex = new Regex(@"\t|\n|\r");
            stroutput = regex.Replace(stroutput, "");
            return stroutput;
        }

        public static void getFile(String fileUrl, String path) {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(fileUrl);
            request.Timeout = 10 * 1000;
            request.Method = "GET";
            HttpWebResponse respond = (HttpWebResponse)request.GetResponse();

            byte[] buf = new byte[65535];
            int len = 0;
            using (Stream fs = new FileStream(path, FileMode.Create, FileAccess.Write),
                rs = respond.GetResponseStream()) {
                while ((len = rs.Read(buf, 0, 65535)) != 0) {
                    fs.Write(buf, 0, len);
                }
                fs.Close();
                rs.Close();
            }
        }
    }
}
