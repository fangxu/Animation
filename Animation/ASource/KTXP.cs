using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Animation.Utils;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace Animation.ASource
{
    class KTXP : IASource
    {
        const String BILL_URL = @"http://bt.ktxp.com/playbill.php";
        const String HOME_URL = @"http://bt.ktxp.com";
        List<String> allWeek;
        Dictionary<Kind, String> kind2url;

        public KTXP() {
            allWeek = new List<String>();
            allWeek.AddRange(new String[] { "星期一", "星期二", "星期三", "星期四", "星期五", "星期六", "星期日" });

            kind2url = new Dictionary<Kind, String>();
            kind2url.Add(Kind.ALL, "");
            kind2url.Add(Kind.ANIMATION, "&sort_id=1");
            kind2url.Add(Kind.COLLECTION, "&sort_id=28");
            kind2url.Add(Kind.COMIC, "&sort_id=3");
            kind2url.Add(Kind.MUSIC, "&sort_id=4");
            kind2url.Add(Kind.RAW, "&sort_id=15");
        }

        public Dictionary<string, Dictionary<string, string>> getBill() {
            Console.WriteLine("getBill begin");
            Console.WriteLine("getHtml begin");
            String html = NetUtils.getHtml(BILL_URL);
            html = NetUtils.formateHtml(html);
            Console.WriteLine("getHtml done");
            if (html == null) {
                return null;
            }

            //MatchCollection matches = null;
            Dictionary<string, Dictionary<string, string>> bill;
            bill = new Dictionary<String, Dictionary<String, String>>();
            Dictionary<String, String> playbill;
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            //HtmlNodeCollection trlist = doc.DocumentNode.SelectNodes(".//tr");
            //HtmlNode tbody = doc.DocumentNode.SelectSingleNode(".//tbody");
            HtmlNodeCollection dls = doc.DocumentNode.SelectNodes(".//dl");
            for (int i = 0; i < dls.Count; i++) {
                HtmlNodeCollection aS = dls[i].SelectNodes(".//a");
                playbill = new Dictionary<String, String>();
                foreach (HtmlNode a in aS) {
                    String keyword=a.Attributes[0].Value;
                    
                    playbill.Add(a.InnerText, keyword.Split('=')[1]);
                }
                bill.Add(allWeek[i], playbill);
            }
            //             matches = Regex.Matches(html, @"<dl>[\s\S]*?</dl>", RegexOptions.Compiled);
            //             for (int i = 0; i < matches.Count; i++) {
            //                 Match m = matches[i];
            //                 //<dd><a href="/search.php?keyword=%E5%AE%87%E5%AE%99%E5%85%84%E5%BC%9F" target="_blank">宇宙兄弟</a></dd>
            //                 Dictionary<String, String> playbill = new Dictionary<String, String>();
            //                 String p = @"keyword=([\s\S]*?)""[\s\S]*?>([\s\S]*?)</a>";
            //                 MatchCollection mm = Regex.Matches(m.ToString(), p, RegexOptions.Compiled);
            //                 foreach (Match mms in mm) {
            //                     playbill.Add(mms.Groups[2].ToString(), mms.Groups[1].ToString());
            //                 }
            //                 bill.Add(allWeek[i], playbill);
            //             }
            Console.WriteLine("getBill done");
            return bill;
        }

        public void getBillAndItems(out Dictionary<string, Dictionary<string, string>> bill, out List<DMItem> items) {
            bill = getBill();
            items = getDMItem();
        }


        public string getDetailHtml(string url) {
            String html = Utils.NetUtils.getHtml(url);
            String patten = @"(<div class=""intro container-style"">[\s\S]*?)<div class=""r money"">";
            Match m = Regex.Match(html, patten);
            return @"<link type=""text/css"" rel=""stylesheet"" href=""http://static.ktxp.com/bt/style/global.min.css"" />"
            + m.Groups[1].ToString();
        }


        public List<DMItem> getDMItem(Kind kind, string keywords = null) {
            String url = keywords == null ? null : @"http://bt.ktxp.com/search.php?keyword="
                + keywords + kind2url[kind] + "&field=title";
            return getDMItem(url);
        }

        //http://bt.ktxp.com/index-1.html
        //http://bt.ktxp.com/search.php?keyword=%E6%84%BF%E6%AD%A4%E5%88%BB%E6%B0%B8%E6%81%92&sort=28
        private List<DMItem> getDMItem(string url = null) {
            Console.WriteLine("getDMItem begin");
            url = url == null ? "http://bt.ktxp.com/index-1.html" : url;
            Console.WriteLine("getHtml begin");
            String html = NetUtils.getHtml(url);
            html = NetUtils.formateHtml(html);
            if (html == null) {
                return null;
            }
            Console.WriteLine("getHtml done");
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            //HtmlNodeCollection trlist = doc.DocumentNode.SelectNodes(".//tr");
            HtmlNode tbody = doc.DocumentNode.SelectSingleNode(".//tbody");
            List<DMItem> list = new List<DMItem>();
            DMItem item = null;
            foreach (HtmlNode tr in tbody.SelectNodes("./tr")) {
                HtmlNodeCollection tds = tr.SelectNodes("./td");

                item = new DMItem();
                item.Date = tds[0].Attributes[0].Value;
                item.kind = tds[1].FirstChild.InnerText;
                item.TorrentUrl = HOME_URL + tds[2].ChildNodes[0].Attributes[0].Value;
                item.DetailUrl = HOME_URL + tds[2].ChildNodes[1].Attributes[0].Value;
                item.Title = NetUtils.stripHtml(tds[2].ChildNodes[1].InnerText);
                item.size = tds[3].InnerText;
                item.pulisher = NetUtils.stripHtml(tds[7].FirstChild.InnerText);

                list.Add(item);
            }

            //             String dateP = @"<td title=""([\d|/]* \d\d:\d\d)"">";
            //             String kindP = @"[\s\S]*?\.html"">(\S*?)<";
            //             String torrentP = @"[\s\S]*?href=""([\S]*?)""";
            //             String detailP = @"[\s\S]*?href=""([\S]*?)""";
            //             String titleP = @"[\s\S]*?"">([\s\S]*?)</a>";
            //             String sizeP = @"[\s\S]*?<td>([\d|\.]*.B)</td>";
            //             String pubisherP = @"[\s\S]*?team-name"">([\S]*?)</a>";
            // 
            //             String pattern = dateP + kindP + torrentP + detailP + titleP + sizeP + pubisherP;
            //             List<DMItem> list = new List<DMItem>();
            //             DMItem item = null;
            //             Regex rgx = new Regex(pattern, RegexOptions.Compiled);
            //             MatchCollection matches = rgx.Matches(html);
            //             
            //             foreach (Match match in matches) {
            //                 item = new DMItem();
            //                 item.Date = match.Groups[1].ToString();
            //                 item.kind = match.Groups[2].ToString();
            //                 item.TorrentUrl = HOME_URL + match.Groups[3].ToString();
            //                 item.DetailUrl = HOME_URL + match.Groups[4].ToString();
            //                 item.Title = NetUtils.stripHtml(match.Groups[5].ToString());
            //                 item.size = match.Groups[6].ToString();
            //                 item.pulisher = NetUtils.stripHtml(match.Groups[7].ToString());
            // 
            //                 list.Add(item);
            //             }
            Console.WriteLine("getDMItem done");
            return list;
        }

        public void getTorrent(string tURL, string PathName) {
            NetUtils.getFile(tURL, PathName);
        }


        public string getComment(string url) {
            return null;
        }
    }
}
