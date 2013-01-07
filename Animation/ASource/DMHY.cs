using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Animation.Utils;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace Animation.ASource
{
    class DMHY : IASource
    {
        List<String> allWeek;
        Dictionary<Kind, String> kind2url;
        const String HOME_URL = @"http://share.dmhy.org";

        public DMHY() {
            allWeek = new List<String>();
            allWeek.AddRange(new String[] { "星期一", "星期二", "星期三", "星期四", "星期五", "星期六", "星期日" });
            kind2url = new Dictionary<Kind, String>();

            kind2url.Add(Kind.ALL, "");
            kind2url.Add(Kind.ANIMATION, "sort_id:2");
            kind2url.Add(Kind.COLLECTION, "sort_id:31");
            kind2url.Add(Kind.COMIC, "sort_id:3");
            kind2url.Add(Kind.MUSIC, "sort_id:4");
            kind2url.Add(Kind.RAW, "sort_id:7");
        }

        public Dictionary<string, Dictionary<string, string>> getBill() {
            String html = NetUtils.getHtml(HOME_URL);
            html = NetUtils.formateHtml(html);
            return getBillFromHtml(html);
        }

        public List<DMItem> getDMItem(string url = null) {
            url = url == null ? HOME_URL : url;
            String html = NetUtils.getHtml(url);
            html = NetUtils.formateHtml(html);
            return getDMItemFromHtml(html);
        }

        public List<DMItem> getDMItem(Kind kind, string keywords = null) {
            String url = @"http://share.dmhy.org/topics/list?keyword="
                + keywords + " " + kind2url[kind];
            return getDMItem(url);
        }

        private Dictionary<string, Dictionary<string, string>> getBillFromHtml(String html) {
            if (html == null) {
                return null;
            }
            String pattern = null;
            MatchCollection matches = null;
            Dictionary<string, Dictionary<string, string>> bill;
            bill = new Dictionary<String, Dictionary<String, String>>();
            foreach (String week in allWeek) {
                Dictionary<String, String> playbill = new Dictionary<String, String>();
                pattern = TextUtils.weekToEng(week) + @"array\.push[\s\S]*?,'([\s\S]*?)','([\s\S]*?)'";
                matches = Regex.Matches(html, pattern, RegexOptions.Compiled);
                foreach (Match m in matches) {
                    if (playbill.ContainsValue(m.Groups[2].ToString())) {
                        continue;
                    }
                    playbill.Add(NetUtils.stripHtml(m.Groups[1].ToString()), m.Groups[2].ToString());
                }
                bill.Add(week, playbill);
            }
            return bill;
        }

        private List<DMItem> getDMItemFromHtml(String html) {
            if (html == null) {
                return null;
            }
            String dateP = @"<span style=""display: none;"">([\d|/]* \d\d:\d\d)<";
            String kindP = @"[\s\S]*?<font color=\S*?>(\S*?)<";
            String titleP = @"[\s\S]*?<td class=""title"">([\s\S]*?)</td>";
            String torrentP = @"[\s\S]*?href=""([\S]*?)""";
            String sizeP = @"[\s\S]*?align=""center"">([\d|\.]*.B)</td>";
            String pubisherP = @"[\s\S]*?user_id/\d*?"">([\S]*?)</a>";
            String pattern = dateP + kindP + titleP + torrentP + sizeP + pubisherP;
            List<DMItem> list = new List<DMItem>();
            DMItem item = null;
            Regex rgx = new Regex(pattern);
            Regex rgx2 = new Regex(@"href=""([\s\S]*?)""");
            Regex rgx1 = new Regex(@"_blank"" >([\s\S]*)");
            MatchCollection matches = rgx.Matches(html);
            foreach (Match match in matches) {
                item = new DMItem();
                item.Date = match.Groups[1].ToString();
                item.kind = match.Groups[2].ToString();
                item.TorrentUrl = HOME_URL + match.Groups[4].ToString();
                item.Title = NetUtils.stripHtml(match.Groups[3].ToString());
                item.size = match.Groups[5].ToString();
                item.pulisher = NetUtils.stripHtml(match.Groups[6].ToString());
                String[] s = Regex.Split(match.Groups[3].ToString(), @"</a>");
                if (s.Length == 2) {
                    String s1 = s[0];
                    item.Title = NetUtils.stripHtml(rgx1.Match(s1).Groups[1].ToString());
                    item.Team = null;
                    item.DetailUrl = HOME_URL + rgx2.Match(s1).Groups[1].ToString();
                } else {
                    String s1 = s[0];
                    item.Team = s1.Substring(s1.LastIndexOf('>') + 1, s1.Length - s1.LastIndexOf('>') - 1);
                    item.TeamID = s1.Substring(s1.LastIndexOf('/') + 1, s1.LastIndexOf('"') - s1.LastIndexOf('/') - 1);
                    s1 = s[1];
                    item.Title = NetUtils.stripHtml(rgx1.Match(s1).Groups[1].ToString());
                    item.DetailUrl = HOME_URL + rgx2.Match(s1).Groups[1].ToString();
                }
                list.Add(item);
            }
            return list;
        }

        public void getTorrent(string tURL, string PathName) {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(tURL);
            CookieContainer cc = new CookieContainer();
            cc.Add(new Cookie("rsspass", "adf651b88f87892db88b62cd60", null, "share.dmhy.org"));
            cc.Add(new Cookie("uid", "120145", null, "share.dmhy.org"));
            request.Timeout = 30 * 1000;
            request.Method = "GET";
            request.CookieContainer = cc;
            HttpWebResponse respond = (HttpWebResponse)request.GetResponse();

            byte[] buf = new byte[65535];
            int len = 0;
            using (Stream fs = new FileStream(PathName, FileMode.Create, FileAccess.Write),
                rs = respond.GetResponseStream()) {
                while ((len = rs.Read(buf, 0, 65535)) != 0) {
                    fs.Write(buf, 0, len);
                }
                fs.Close();
                rs.Close();
            }
        }

        public string getDetailHtml(string url) {
            String html = Utils.NetUtils.getHtml(url);
            String patten = @"(<div class=""topic-nfo box ui-corner-all"">[\s\S]*?)<div id=""play-asia"" class=""box ui-corner-all"">";
            Match m = Regex.Match(html, patten);
            return @"<link href=""http://share.dmhy.org/min/g=css&v=10"" rel=""stylesheet"" type=""text/css"" />"
                + @"<script type=""text/javascript"" src=""http://share.dmhy.org/min/g=js&amp;v=9""></script>"
                + @"<div class=""topic-main"">" + m.Groups[1].ToString();
        }


        public void getBillAndItems(out Dictionary<string, Dictionary<string, string>> bill, out List<DMItem> items) {

            String html = NetUtils.getHtml(HOME_URL);
            html = NetUtils.formateHtml(html);
            Console.WriteLine("getBill begin");
            bill = getBillFromHtml(html);
            items = getDMItemFromHtml(html);
        }


        public string getComment(string url) {
            String id = null;
            Match match = Regex.Match(url, @"/(\d*?)_");
            id = match.Groups[1].ToString();
            //http://share.dmhy.org/comment/recent/topic_id/284923
            String commentUrl = @"http://share.dmhy.org/comment/recent/topic_id/" + id;
            String comment = NetUtils.getHtml(commentUrl);
            String pattern = @"(<div class=""comment_recent_main ui-corner-all"">[\s\S]*?</script>)";
            String commentHtml = Regex.Match(comment, pattern).Groups[1].ToString();
            commentHtml = commentHtml.Replace("src=\"/images/defaultUser.png", "src=\"http://share.dmhy.org/images/defaultUser.png");
            return @"<div id=""recent-commnet"" class=""comment box ui-corner-all"">"
                + commentHtml + "</div></div>";
        }
    }
}
