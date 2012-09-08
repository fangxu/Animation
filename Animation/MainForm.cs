using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;

namespace Animation
{
    public partial class MainForm : Form
    {
        private List<DMItem> itemList = null;
        private Dictionary<String, String> kinds = null;
        private Dictionary<String, Dictionary<String, String>> bill = null;
        private String kind = null;
        private String EXEPath = null;
        String currentWeek = null;

        Dictionary<String, String> weekEng = null;

        List<String> subitem = new List<String>();

        public MainForm()
        {
            bill = new Dictionary<String, Dictionary<String, String>>();
            weekEng = new Dictionary<String, String>();
            weekEng.Add("星期一", "mon");
            weekEng.Add("星期二", "tue");
            weekEng.Add("星期三", "wed");
            weekEng.Add("星期四", "thu");
            weekEng.Add("星期五", "fri");
            weekEng.Add("星期六", "sat");
            weekEng.Add("星期日", "sun");
            EXEPath = System.Environment.CurrentDirectory;
            if (!Directory.Exists(EXEPath + "/torrent"))
            {
                Directory.CreateDirectory(EXEPath + "/torrent");
            }
            InitializeComponent();
            initKinds();
            toolStripComboBox3.SelectedIndex = 0;
            ImageList imageList1 = new ImageList();
            imageList1.ImageSize = new Size(1, 25);
            listView1.SmallImageList = imageList1;
            initXinFan();
            initWeek();
        }

        private void initWeek()
        {
            DayOfWeek dw = DateTime.Now.DayOfWeek;
            Console.WriteLine(dw.ToString());
            String w = dw.ToString().Substring(0, 3).ToLower();
            String week = null;
            foreach (KeyValuePair<String, String> kv in weekEng)
            {
                if (kv.Value.Equals(w))
                {
                    week = kv.Key;
                    break;
                }
            }
            if (week == null)
            {
                return;
            }
            ToolStripMenuItem_MouseMove(week, null);
        }

        private void initXinFan()
        {
            String xinFanString = getXinFanString();
            foreach (ToolStripMenuItem menu in menuStripWeek.Items)
            {
                String week = menu.Text;
                Dictionary<String, String> playbill = new Dictionary<String, String>();
                String pattern = weekEng[week] + @"array[\s\S]*?,'([\s\S]*?)','([\s\S]*?)'";
                MatchCollection matches = Regex.Matches(xinFanString, pattern);
                foreach (Match m in matches)
                {
                    if (playbill.ContainsValue(m.Groups[2].ToString()))
                    {
                        break;
                    }
                    playbill.Add(m.Groups[1].ToString(), m.Groups[2].ToString());
                }
                bill.Add(week, playbill);
            }
        }

        private String getXinFanString()
        {
            String temp = getHtml(@"http://share.dmhy.org/cms/page/name/programme.html");
            return Regex.Match(temp, @"(//星期日[\s\S]*?)</script>").Groups[1].ToString();
        }

        private void initKinds()
        {
            kinds = new Dictionary<String, String>();
            kinds.Add("all", @"http://share.dmhy.org/");
            kinds.Add("动画", @"http://share.dmhy.org/topics/list/sort_id/2");
            kinds.Add("季度全集", @"http://share.dmhy.org/topics/list/sort_id/31");
            kinds.Add("漫画", @"http://share.dmhy.org/topics/list/sort_id/3");
            kinds.Add("音乐", @"http://share.dmhy.org/topics/list/sort_id/4");
            kinds.Add("RAW", @"http://share.dmhy.org/topics/list/sort_id/7");
            foreach (KeyValuePair<String, String> x in kinds)
            {
                toolStripComboBox3.Items.Add(x.Key);
            }
        }            

        private void updateList()
        {
            listView1.Items.Clear();
            ListViewItem lv;
            foreach (DMItem item in itemList)
            {
                lv = new ListViewItem(new string[] { item.Date,
                item.Title, item.size ,item.kind,item.Team});
                if (listView1.Items.Count % 2 == 0)
                {
                    lv.BackColor = Color.FromArgb(0xccddff);
                }
                //lv.Font=
                listView1.Items.Add(lv);
            }
            listView1.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            listView1.Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            listView1.Columns[4].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            int width = 0;
            for (int i = 0; i < listView1.Columns.Count; i++)
            {
                width += listView1.Columns[i].Width;
            }
            this.Width = width + 40;
        }

        private List<DMItem> parserHtml(String html)
        {
            String dateP = @"<span style=""display: none;"">([\d|/]* \d\d:\d\d)<";
            String kindP = @"[\s\S]*?<font color=\S*?>(\S*?)<";
            String titleP = @"[\s\S]*?<td class=""title"">([\s\S]*?)</td>";
            String torrentP = @"[\s\S]*?href=""([\S]*?)""";
            String sizeP = @"[\s\S]*?align=""center"">([\d|\.]*.B)</td>";
            String pattern = dateP + kindP + titleP + torrentP + sizeP;
            List<DMItem> list = new List<DMItem>();
            DMItem item = null;
            Regex rgx = new Regex(pattern);
            Regex rgx2 = new Regex(@"href=""([\s\S]*?)""");
            Regex rgx1 = new Regex(@"_blank"" >([\s\S]*)");
            MatchCollection matches = rgx.Matches(html);
            foreach (Match match in matches)
            {
                item = new DMItem();
                item.Date = match.Groups[1].ToString();
                item.kind = match.Groups[2].ToString();
                item.TorrentUrl = match.Groups[4].ToString();
                item.Title = striphtml(match.Groups[3].ToString());
                item.size = match.Groups[5].ToString();
                String[] s = Regex.Split(match.Groups[3].ToString(), @"</a>");
                if (s.Length == 2)
                {
                    String s1 = s[0];
                    item.Title = striphtml(rgx1.Match(s1).Groups[1].ToString());
                    item.Team = null;
                    item.DetailUrl = rgx2.Match(s1).Groups[1].ToString();
                }
                else
                {
                    String s1 = s[0];
                    item.Team = s1.Substring(s1.LastIndexOf('>') + 1, s1.Length - s1.LastIndexOf('>') - 1);
                    s1 = s[1];
                    item.Title = striphtml(rgx1.Match(s1).Groups[1].ToString());
                    item.DetailUrl = rgx2.Match(s1).Groups[1].ToString();
                }
                list.Add(item);
            }
            return list;
        }

        private string striphtml(string strhtml)
        {
            string stroutput = strhtml;
            Regex regex = new Regex(@"<[^>]+>|</[^>]+>|amp;");
            stroutput = regex.Replace(stroutput, "");
            return stroutput;
        }

        private String getHtml(String url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 10 * 1000;
            request.Method = "GET";
            request.UserAgent = "Mozilla/4.0";
            Stream rs = request.GetResponse().GetResponseStream();
            byte[] buf = new byte[1024];
            int len = 0;
            StringBuilder sb = new StringBuilder();
            while ((len = rs.Read(buf, 0, 1024)) != 0)
            {
                sb.Append(Encoding.UTF8.GetString(buf, 0, len));
            }
            rs.Close();
            return sb.ToString();
        }

        private void getFile(String fileUrl, String path)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(fileUrl);
            CookieContainer cc = new CookieContainer();
            cc.Add(new Cookie("rsspass", "adf651b88f87892db88b62cd60", null, "share.dmhy.org"));
            cc.Add(new Cookie("uid", "120145", null, "share.dmhy.org"));
            request.Timeout = 30 * 1000;
            request.Method = "GET";
            request.CookieContainer = cc;
            HttpWebResponse respond = (HttpWebResponse)request.GetResponse();

            byte[] buf = new byte[65535];
            int len = 0;
            using (Stream fs = new FileStream(path, FileMode.Create, FileAccess.Write),
                rs = respond.GetResponseStream())
            {
                while ((len = rs.Read(buf, 0, 65535)) != 0)
                {
                    fs.Write(buf, 0, len);
                }
                fs.Close();
                rs.Close();
            }
        }

        private void ToolStripMenuItemDownload_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                String s = item.SubItems[1].Text;
                DMItem selected = itemList.Find(it =>
                {
                    if (it.Title.Equals(s))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "BT 文件(*.torrent)|*.torrent";
                sfd.RestoreDirectory = true;
                sfd.FileName = selected.TorrentName;
                if (sfd.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                getFile(selected.TorrentUrl, sfd.FileName);
            }
        }
       
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            ToolStripMenuItemOpen_Click(null, null);
        }

        private void ToolStripMenuItemOpen_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                String s = item.SubItems[1].Text;
                DMItem selected = itemList.Find(it =>
                {
                    if (it.Title.Equals(s))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });
                getFile(selected.TorrentUrl, EXEPath + "/torrent/" + selected.TorrentName);
                System.Diagnostics.Process.Start(EXEPath + "/torrent/" + selected.TorrentName);
                            }
        }

        private String getDetailHtml(String url)
        {
            String html = getHtml(url);
            Match m = Regex.Match(html, @"(<div class=""topic-nfo box ui-corner-all"">[\s\S]*?)<div id=""play-asia"" class=""box ui-corner-all"">");
            return @"<link href=""http://share.dmhy.org/min/g=css&v=10"" rel=""stylesheet"" type=""text/css"" />" + @"<div class=""topic-main"">" + m.Groups[1].ToString() + "</div>";
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Directory.Delete(EXEPath + "/torrent", true);
        }

        private void ToolStripMenuItemDetail_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                String s = item.SubItems[1].Text;
                DMItem selected = itemList.Find(it =>
                {
                    if (it.Title.Equals(s))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });
                new DetailWeb(getDetailHtml(selected.DetailUrl), selected.Title).Show();               
            }
        }       

        private void ToolStripMenuItemTitleCopy_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                String s = item.SubItems[1].Text;
                DMItem selected = itemList.Find(it =>
                {
                    if (it.Title.Equals(s))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });
                Clipboard.SetText(selected.Title);
            }
        }

        private void ToolStripMenuItemtorrentCopy_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                String s = item.SubItems[1].Text;
                DMItem selected = itemList.Find(it =>
                {
                    if (it.Title.Equals(s))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });
                Clipboard.SetText(selected.DetailUrl);
            }
        }       

        private void ToolStripMenuItem_MouseMove(object sender, MouseEventArgs e)
        {
            String week = sender.ToString();
            if (week.Equals(currentWeek))
            {
                return;
            }
            ToolStripMenuItem MenuItem;
            menuStripName.Items.Clear();           
            foreach (KeyValuePair<String, String> k in bill[week])
            {
                MenuItem = new ToolStripMenuItem();
                MenuItem.Size = new System.Drawing.Size(k.Key.Length * 20, 21);
                MenuItem.Text = k.Key;
                MenuItem.Click += new EventHandler(SubItem_MouseDown);
                menuStripName.Items.Add(MenuItem);
            }
            List<String> ss = weekEng.Keys.ToList<String>();          
            menuStripWeek.Items[ss.IndexOf(week)].BackColor = Color.Blue;
            if (currentWeek != null)
            {
                menuStripWeek.Items[ss.IndexOf(currentWeek)].BackColor = Color.Silver;
            }
            currentWeek = week;
        }

        private void SubItem_MouseDown(object sender, EventArgs e)
        {
            String url = @"http://share.dmhy.org/topics/list?keyword=" + bill[currentWeek][sender.ToString()];
            kind = "all";
            itemList = parserHtml(getHtml(url));           
            updateList();
        }

        private void toolStripComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            kind = toolStripComboBox3.Items[toolStripComboBox3.SelectedIndex].ToString();
            String url = kinds[kind];
            itemList = parserHtml(getHtml(url));
            updateList();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {           
            if (toolStripTextBox1.Text == "")
            {
                return;
            }
            String url = @"http://share.dmhy.org/topics/list?keyword=" + toolStripTextBox1.Text;           
            kind = "all";
            itemList = parserHtml(getHtml(url));           
            updateList();          
        }

        private void toolStripTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                toolStripButton1_Click(null, null);
            }
        }
    }
}
