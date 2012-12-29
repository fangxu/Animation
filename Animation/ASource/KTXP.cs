using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Animation.Utils;
using System.Text.RegularExpressions;

namespace Animation.ASource
{
    class KTXP:IASource
    {
        const String BILL_URL = @"http://bt.ktxp.com/playbill.php";
        List<String> allWeek;
        Dictionary<Kind, String> kind2url;

        public KTXP() {
            allWeek = new List<String>();
            allWeek.AddRange(new String[] { "星期一", "星期二", "星期三", "星期四", "星期五", "星期六", "星期日" });
        }

        public Dictionary<string, Dictionary<string, string>> getBill() {
            String html = NetUtils.getHtml(BILL_URL);
            html = NetUtils.formateHtml(html);
            if (html == null) {
                return null;
            }
            String pattern = null;
            MatchCollection matches = null;
            Dictionary<string, Dictionary<string, string>> bill;
            bill = new Dictionary<String, Dictionary<String, String>>();

            matches = Regex.Matches(html, @"<dl>[\s\S]*?</dl>");
            for (int i = 0; i < matches.Count; i++) {
                Match m = matches[i];
                //<dd><a href="/search.php?keyword=%E5%AE%87%E5%AE%99%E5%85%84%E5%BC%9F" target="_blank">宇宙兄弟</a></dd>
                Dictionary<String, String> playbill = new Dictionary<String, String>();
                String p = @"keyword=([\s\S]*?)""[\s\S]*?>([\s\S]*?)</a>";
                MatchCollection mm = Regex.Matches(m.ToString(), p);
                foreach (Match mms in mm) {
                    playbill.Add(mms.Groups[2].ToString(), mms.Groups[1].ToString());
                }
                bill.Add(allWeek[i], playbill);
            }

//             foreach (String week in allWeek) {
//                 Dictionary<String, String> playbill = new Dictionary<String, String>();
//                 pattern = TextUtils.weekToEng(week) + @"array\.push[\s\S]*?,'([\s\S]*?)','([\s\S]*?)'";
//                 matches = Regex.Matches(html, pattern, RegexOptions.Compiled);
//                 foreach (Match m in matches) {
//                     if (playbill.ContainsValue(m.Groups[2].ToString())) {
//                         continue;
//                     }
//                     playbill.Add(NetUtils.stripHtml(m.Groups[1].ToString()), m.Groups[2].ToString());
//                 }
//                 bill.Add(week, playbill);
//             }
            return bill;
        }

        public void getBillAndItems(out Dictionary<string, Dictionary<string, string>> bill, out List<DMItem> items) {
            bill = getBill();
            items = getDMItem();
        }


        public string getDetailHtml(string url) {
             throw new NotImplementedException();
        }

        
        public List<DMItem> getDMItem(Kind kind, string keywords = null) {
            String url = null;
            return getDMItem(url);
        }

        //http://bt.ktxp.com/index-1.html
        private List<DMItem> getDMItem(string url=null) {
            url = url == null ? "http://bt.ktxp.com/index-1.html" : url;
            String html = NetUtils.getHtml(url);
            html = NetUtils.formateHtml(html);
            if (html == null) {
                return null;
            }

            /*
             <tr>
                        <td title="2012/12/29 21:04">今天 21:04</td>
                        <td><a href="/sort-12-1.html">新番连载</a></td>                        <td class="ltext ttitle"><a href="/down/1356786257/bb5ae2a63a56d9f326c01ef82a9f1a486723c7e5.torrent" class="quick-down cmbg"></a><a href="/html/2012/1229/283069.html" target="_blank">【極影字幕社】★ 出包王女 To Love Ru Darkness 第12集 BIG5 720p MP4</a></td>
                        <td>146.8MB</td>
                        <td class="bts-3">0</td>
                        <td class="btl-3">0</td>
                        <td class="btc-3">0</td>
                        <td><a href="/team-1-1.html" class="team-name">极影字幕</a></td>                    </tr>*/
            String dateP = @"<td title=""([\d|/]* \d\d:\d\d)"">";
            String kindP = @"[\s\S]*?\.html"">(\S*?)<";
            String torrentP = @"[\s\S]*?href=""([\S]*?)""";
            String detailP = @"[\s\S]*?href=""([\S]*?)""";
            String titleP = @"[\s\S]*?"">([\s\S]*?)</a>";
            String sizeP = @"[\s\S]*?<td>([\d|\.]*.B)</td>";
            String pubisherP = @"[\s\S]*?team-name"">([\S]*?)</a>";

            String pattern = dateP + kindP + torrentP + detailP + titleP + sizeP + pubisherP;
            List<DMItem> list = new List<DMItem>();
            DMItem item = null;
            Regex rgx = new Regex(pattern, RegexOptions.Compiled);
            MatchCollection matches = rgx.Matches(html);
            foreach (Match match in matches) {
                item = new DMItem();
                item.Date = match.Groups[1].ToString();
                item.kind = match.Groups[2].ToString();
                item.TorrentUrl = match.Groups[3].ToString();
                item.DetailUrl = match.Groups[4].ToString();
                item.Title = NetUtils.stripHtml(match.Groups[5].ToString());
                item.size = match.Groups[6].ToString();
                item.pulisher = NetUtils.stripHtml(match.Groups[7].ToString());

                list.Add(item);
            }
            return list;
        }

        public void getTorrent(string tURL, string PathName) {
            throw new NotImplementedException();
        }

        
    }
}
