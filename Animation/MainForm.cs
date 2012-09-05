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
        public MainForm()
        {
            EXEPath = System.Environment.CurrentDirectory;
            if (!Directory.Exists(EXEPath + "/torrent"))
            {
                Directory.CreateDirectory(EXEPath + "/torrent");
            }
            initKinds();
            InitializeComponent();
            int d = (int)DateTime.Now.DayOfWeek;
            toolStripComboBox1.SelectedIndex = d - 1;
            toolStripComboBox3.SelectedIndex = 0;
            updateXinfan();
            //updateNew();
        }

        private void initKinds()
        {
            kinds = new Dictionary<String, String>();
            kinds.Add("all", @"http://bt.ktxp.com/sort-1-1.html");
            kinds.Add("新番连载", @"http://bt.ktxp.com/sort-12-1.html");
            kinds.Add("完整动画", @"http://bt.ktxp.com/sort-28-1.html");
            kinds.Add("剧场版", @"http://bt.ktxp.com/sort-39-1.html");
            kinds.Add("DVDRIP", @"http://bt.ktxp.com/sort-14-1.html");
            kinds.Add("BDRIP", @"http://bt.ktxp.com/sort-50-1.html");
        }

        private void updateXinfan()
        {
            xinFan = parserXinFan(getHtml(@"http://bt.ktxp.com/playbill.php"),
                toolStripComboBox1.Items[toolStripComboBox1.SelectedIndex].ToString());
            toolStripComboBox2.Items.Clear();
            foreach (KeyValuePair<String, String> item in xinFan)
            {
                toolStripComboBox2.Items.Add(item.Key);
            }
            //toolStripComboBox2.SelectedIndex = 0;
        }

        private Dictionary<String, String> parserXinFan(String html, String week)
        {
            Dictionary<String, String> playbill = new Dictionary<String, String>();
            Match match = Regex.Match(html, week + @"[\s\S]*?</dt>([\s\S]*?)</dl>");
            String weekBill = match.Groups[1].ToString();
            String pattern = @"<dd><a href=""([\s\S]*?)"" target=""_blank"">([\s\S]*?)</a>";
            MatchCollection matches = Regex.Matches(weekBill, pattern);
            foreach (Match m in matches)
            {
                playbill.Add(m.Groups[2].ToString(), m.Groups[1].ToString());
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
                lv = new ListViewItem(new string[] { item.data.Month+"/"+item.data.Day+" "+item.data.ToShortTimeString(),
                item.Title, item.size ,item.kind,item.team});
                if (listView1.Items.Count%2==0)
                {
                    lv.BackColor = Color.FromArgb(0xccddff);
                }
                listView1.Items.Add(lv);
            }
            listView1.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            listView1.Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            listView1.Columns[4].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private List<DMItem> parserHtml(String html)
        {//@"<tr class="""">[\s\S]*?(\d{4}/\d{2}/\d{2}\s\d{2}:\d{2})[\s\S]*?target=""_blank"" >\s*?([\S]*?)</a>[\s\S]*?href=""(\S*?)""[\s\S]*?align=""center"">(\S*?)</td>[\s\S]*?</tr>"
            //<td title="2012/09/04 20:55">今天 20:55</td>   <td>297.1MB</td>
            //<a href="/down/1346745933/15907be1c8ddf43eee555a94f50e8e49c9645df8.torrent" class="quick-down cmbg">
            String dateP = @"<td title=""([\d|/]* \d\d:\d\d)""";
            String kindP = @"[\s\S]*?l"">(\S*?)</a>";
            String torrentP = @"[\s\S]*?<a href=""([\S]*?)"" class=""quick-down cmbg"">";
            String detailP = @"[\s\S]*?<a href=""([\s\S]*?)""";
            String titleP = @" target=""_blank"">([\s\S]*?)</a>";
            String sizeP = @"[\s\S]*?<td>([\d|\.]*.B)</td>";
            String teamP = @"[\s\S]*?[l|e]"">([\s\S]*?)</a>[\s\S]*?</tr>";
            String pattern = dateP + torrentP + detailP + titleP + sizeP + teamP;
            if (kind == "all")
            {
                pattern = dateP + kindP + torrentP + detailP + titleP + sizeP + teamP;
            }
            else
            {
                pattern = dateP + torrentP + detailP + titleP + sizeP + teamP;
            }
            List<DMItem> list = new List<DMItem>();
            DMItem item = null;
            Regex rgx = new Regex(pattern);
            MatchCollection matches = rgx.Matches(html);
            foreach (Match match in matches)
            {
                item = new DMItem();
                if (kind == "all")
                {
                    item.data = DateTime.Parse(match.Groups[1].ToString());
                    item.kind = match.Groups[2].ToString();
                    item.TorrentUrl = match.Groups[3].ToString();
                    item.DetailUrl = match.Groups[4].ToString();
                    item.Title = striphtml(match.Groups[5].ToString());
                    item.size = match.Groups[6].ToString();
                    item.team = match.Groups[7].ToString();
                }
                else
                {
                    item.data = DateTime.Parse(match.Groups[1].ToString());
                    item.kind = kind;
                    item.TorrentUrl = match.Groups[2].ToString();
                    item.DetailUrl = match.Groups[3].ToString();
                    item.Title = striphtml(match.Groups[4].ToString());
                    item.size = match.Groups[5].ToString();
                    item.team = match.Groups[6].ToString();
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
            request.Timeout = 30 * 1000;
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
            request.Timeout = 30 * 1000;
            request.Method = "GET";
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
                Console.WriteLine(selected);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //string url = @"http://bt.ktxp.com/sort-1-1.html";
            //if (toolStripTextBox1.Text != "")
            //{
            String url = @"http://bt.ktxp.com/search.php?keyword=" + toolStripTextBox1.Text;
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
            String url = @"http://bt.ktxp.com" + xinFan[toolStripComboBox2.Items[toolStripComboBox2.SelectedIndex].ToString()];
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
                Console.WriteLine(selected);
            }
        }

        private String getDetailHtml(String url)
        {
            String html = getHtml(url);
            Match m = Regex.Match(html, @"<div class=""intro container-style"">[\s\S]*?</div>");
            return @"<link type=""text/css"" rel=""stylesheet"" href=""http://static.ktxp.com/bt/style/global.min.css"" />" + m.Value;
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
                Console.WriteLine(selected);
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
                Clipboard.SetText(selected.TorrentUrl);
            }
        }

        
    }
}
