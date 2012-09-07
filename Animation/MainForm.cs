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
        private Dictionary<String, String> xinFan = null;
        private Dictionary<String, String> kinds = null;
        private String kind = null;
        private String EXEPath = null;
        private String xinFanString = null;

        public MainForm()
        {
            EXEPath = System.Environment.CurrentDirectory;
            if (!Directory.Exists(EXEPath + "/torrent"))
            {
                Directory.CreateDirectory(EXEPath + "/torrent");
            }
            InitializeComponent();
            initKinds();
            xinFanString = getXinFanString();            
            int d = (int)DateTime.Now.DayOfWeek;
            toolStripComboBox1.SelectedIndex = d - 1;
            toolStripComboBox3.SelectedIndex = 0;
            ImageList imageList1 = new ImageList();
            imageList1.ImageSize = new Size(1, 25);
            listView1.SmallImageList = imageList1; 
            //listView1.
            //toolStripComboBox3.DroppedDown = true;
            //updateXinfan();
            //updateNew();
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
            foreach (KeyValuePair<String,String> x in kinds)
            {
                toolStripComboBox3.Items.Add(x.Key);
            }
        }

        private void updateXinfan()
        {
            xinFan = parserXinFan(toolStripComboBox1.Items[toolStripComboBox1.SelectedIndex].ToString());
            toolStripComboBox2.Items.Clear();
            foreach (KeyValuePair<String, String> item in xinFan)
            {
                toolStripComboBox2.Items.Add(item.Key);
            }
            //toolStripComboBox2.SelectedIndex = 0;
        }

        private Dictionary<String, String> parserXinFan(String week)
        {
            Dictionary<String, String> playbill = new Dictionary<String, String>();
            String xingqi = Regex.Match(xinFanString, @"//" + week + @"[\s\S]*?//星期").Value;
            String pattern = @"push[\s\S]*?,'([\s\S]*?)','([\s\S]*?)'";
            MatchCollection matches = Regex.Matches(xingqi, pattern);
            
            foreach (Match m in matches)
            {
                if (playbill.ContainsValue(m.Groups[2].ToString()))
               {
                   break;
               }
                playbill.Add(m.Groups[1].ToString(), m.Groups[2].ToString());
                
            }
            return playbill;
        }

        private void updateSearch()
        {
            String url = @"http://bt.ktxp.com/search.php?keyword=" + toolStripTextBox1.Text;
            itemList = parserHtml(getHtml(url));
            //this.Text = itemList.Count.ToString();
            updateList();
        }

        private void updateNew()
        {
            string url = @"http://bt.ktxp.com/sort-1-1.html";
            itemList = parserHtml(getHtml(url));
            //this.Text = itemList.Count.ToString();
            updateList();
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
        {//@"<tr class="""">[\s\S]*?(\d{4}/\d{2}/\d{2}\s\d{2}:\d{2})[\s\S]*?target=""_blank"" >\s*?([\S]*?)</a>[\s\S]*?href=""(\S*?)""[\s\S]*?align=""center"">(\S*?)</td>[\s\S]*?</tr>"
            //<td title="2012/09/04 20:55">今天 20:55</td>   <td>297.1MB</td>
            //<a href="/down/1346745933/15907be1c8ddf43eee555a94f50e8e49c9645df8.torrent" class="quick-down cmbg">
            String dateP = @"<span style=""display: none;"">([\d|/]* \d\d:\d\d)<";
            String kindP = @"[\s\S]*?<font color=\S*?>(\S*?)<";
            String titleP = @"[\s\S]*?<td class=""title"">([\s\S]*?)</td>";
            String torrentP = @"[\s\S]*?href=""([\S]*?)""";
            //String detailP = @"[\s\S]*?<a href=""([\s\S]*?)""";

            String sizeP = @"[\s\S]*?align=""center"">([\d|\.]*.B)</td>";
            //String teamP = @"[\s\S]*?[l|e]"">([\s\S]*?)</a>[\s\S]*?</tr>";
            String pattern = dateP + kindP + titleP+torrentP+sizeP;
            /*if (kind == "all")
            {
                pattern = dateP + kindP + torrentP + detailP + titleP + sizeP + teamP;
            }
            else
            {
                pattern = dateP + torrentP + detailP + titleP + sizeP + teamP;
            }*/
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
                //item.DetailUrl = match.Groups[4].ToString();
                item.Title = striphtml(match.Groups[3].ToString());
                item.size = match.Groups[5].ToString();
                //item.team = match.Groups[7].ToString();
                //String teamTitle = match.Groups[3].ToString();
                
                String[] s=Regex.Split(match.Groups[3].ToString(), @"</a>");
                if (s.Length==2)
                {
                    String s1=s[0];
                    item.Title = striphtml(rgx1.Match(s1).Groups[1].ToString());
                    item.Team = null;
                    //Match result = rgx2.Match(s1);
                    item.DetailUrl = rgx2.Match(s1).Groups[1].ToString();
                } 
                else
                {
                    String s1 = s[0];
                    item.Team = s1.Substring(s1.LastIndexOf('>')+1, s1.Length - s1.LastIndexOf('>')-1);
                    s1 = s[1];
                    item.Title = striphtml(rgx1.Match(s1).Groups[1].ToString());
                    //Match result = rgx2.Match(s1);
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
            byte[] buf = new byte[65535];
            int len = 0;
            StringBuilder sb = new StringBuilder();
            while ((len = rs.Read(buf, 0, 65535)) != 0)
            {
                sb.Append(Encoding.UTF8.GetString(buf, 0, len));
            }
            rs.Close();
            return sb.ToString();
        }
        //http://share.dmhy.org/topics/down/date/1346742945/hash_id/9958cfadc4a9b922670ca82c1a7233699baee610
        private void getFile(String fileUrl, String path)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(fileUrl);
            CookieContainer cc = new CookieContainer();
            cc.Add(new Cookie("rsspass", "adf651b88f87892db88b62cd60", null, "share.dmhy.org"));
            cc.Add(new Cookie("uid", "120145", null, "share.dmhy.org"));
            //cc.Add(new Cookie("rsspass", "adf651b88f87892db88b62cd60", null, "share.dmhy.org"));
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
                //Console.WriteLine(selected);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //string url = @"http://bt.ktxp.com/sort-1-1.html";
            if (toolStripTextBox1.Text == "")
            {
                return;
            }
            String url = @"http://share.dmhy.org/topics/list?keyword=" + toolStripTextBox1.Text;
            // }
            kind = "all";
            itemList = parserHtml(getHtml(url));
            //this.Text = itemList.Count.ToString();
            updateList();
            //getFile(url, "h.html");
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateXinfan();
        }

        private void toolStripComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {//http://bt.ktxp.com/search.php?keyword=%E6%B8%B8%E6%88%8F%E7%8E%8B
            String url = @"http://share.dmhy.org/topics/list?keyword=" + xinFan[toolStripComboBox2.Items[toolStripComboBox2.SelectedIndex].ToString()];
            kind = "all";
            itemList = parserHtml(getHtml(url));
            //this.Text = itemList.Count.ToString();
            updateList();
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
                //File.Delete(selected.TorrentName);
                //Console.WriteLine(selected);
            }
        }

        private String getDetailHtml(String url)
        {
            String html = getHtml(url);
            Match m = Regex.Match(html, @"(<div class=""topic-nfo box ui-corner-all"">[\s\S]*?)<div id=""play-asia"" class=""box ui-corner-all"">");
            return @"<link href=""http://share.dmhy.org/min/g=css&v=10"" rel=""stylesheet"" type=""text/css"" />" + @"<div class=""topic-main"">"+m.Groups[1].ToString()+"</div>";
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
                //Console.WriteLine(selected);
            }
        }

        private void toolStripComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            kind = toolStripComboBox3.Items[toolStripComboBox3.SelectedIndex].ToString();
            String url = kinds[kind];
            itemList = parserHtml(getHtml(url));
            updateList();
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

        private void toolStripTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                toolStripButton1_Click(null, null);
            }
        }
    }
}
