﻿using System;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using System.Windows.Forms;

namespace Animation.Utils
{
    class NetUtils
    {
        public static String getHtml(String url) {

            WebClient webClient = new WebClient();
            byte[] reqHTML;
            String temp = null;
            
            reqHTML = webClient.DownloadData(url);
            temp = System.Text.Encoding.UTF8.GetString(reqHTML);
            //             HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //             request.Timeout = 30 * 1000;
            //             request.Method = "GET";
            //             request.UserAgent = "Mozilla/5.0";
            //             request.KeepAlive = true;
            // 
            //             String temp = null;
            //             try {
            //                 using (Stream rs = request.GetResponse().GetResponseStream()) {
            //                     using (StreamReader sr = new StreamReader(rs, Encoding.UTF8)) {
            //                         temp = sr.ReadToEnd();
            //                     }
            //                 }
            //             } catch (System.Exception ex) {
            //                 MessageBox.Show(null, ex.ToString(), "无法采集数据，请稍后再试。");
            //             } finally {
            //             }

            return Strings.StrConv(temp, VbStrConv.SimplifiedChinese, 0); ;
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
