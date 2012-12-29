using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using Animation.Utils;

namespace Animation
{
    public partial class MainForm : Form
    {
        //列表项目
        private List<DMItem> itemList = null;
        //程序目录
        private String EXEPath = null;
        //星期-新番-网址
        private Dictionary<String, Dictionary<String, String>> bill = null;
        //当前选择的星期
        private String currentWeek = null;
        //当前选择的新番
        private String currentXinFan = null;
        //所有星期
        List<String> allWeek = null;
        //资源类别
        Dictionary<String, String> kinds = null;
        public MainForm()
        {
            EXEPath = System.Environment.CurrentDirectory;
            if (!Directory.Exists(EXEPath + "/torrent"))
            {
                Directory.CreateDirectory(EXEPath + "/torrent");
            }
            InitializeComponent();
            //窗口非使能
            EnabledUI(false);
            //资源类别初始化
            kinds = new Dictionary<String, String>();
            //所有动画全集漫画音乐RAW
            kinds.Add("所有", "");
            kinds.Add("动画", "sort_id:2");
            kinds.Add("全集", "sort_id:31");
            kinds.Add("漫画", "sort_id:3");
            kinds.Add("音乐", "sort_id:4");
            kinds.Add("RAW", "sort_id:7");
            //所有星期初始化
            allWeek = new List<String>();
            allWeek.AddRange(new String[] { "星期一", "星期二", "星期三", "星期四", "星期五", "星期六", "星期日" });
            //今天的星期
            DayOfWeek dw = DateTime.Now.DayOfWeek;
            String w = dw.ToString().Substring(0, 3).ToLower();
            currentWeek = TextUtils.engToWeek(w);
            menuStripWeek.Items[allWeek.IndexOf(currentWeek)].ForeColor = Color.OrangeRed;
            //增加ListView行距
            ImageList imageList1 = new ImageList();
            imageList1.ImageSize = new Size(1, 25);
            listView1.SmallImageList = imageList1;
            //初始化
            Thread t = new Thread(initIndex);
            t.IsBackground = true;
            t.Start();
        }

        /************************************************************************/
        /* 首页初始化
         * http://share.dmhy.org/
         * 提取新番和最新item
        /************************************************************************/
        private void initIndex()
        {
            String html = NetUtils.getHtml(@"http://share.dmhy.org/");
            html = NetUtils.formateHtml(html);
            if (html == null)
            {
                return;
            }
            String pattern = null;
            MatchCollection matches = null;
            bill = new Dictionary<String, Dictionary<String, String>>();
            foreach (String week in allWeek)
            {
                Dictionary<String, String> playbill = new Dictionary<String, String>();
                pattern = TextUtils.weekToEng(week) + @"array\.push[\s\S]*?,'([\s\S]*?)','([\s\S]*?)'";
                matches = Regex.Matches(html, pattern, RegexOptions.Compiled);
                foreach (Match m in matches)
                {
                    if (playbill.ContainsValue(m.Groups[2].ToString()))
                    {
                        continue;
                    }
                    playbill.Add(NetUtils.stripHtml(m.Groups[1].ToString()), m.Groups[2].ToString());
                }
                bill.Add(week, playbill);
            }
            //获取项目列表
            String dateP = @"<span style=""display: none;"">([\d|/]* \d\d:\d\d)<";
            String kindP = @"[\s\S]*?<font color=\S*?>(\S*?)<";
            String titleP = @"[\s\S]*?<td class=""title"">([\s\S]*?)</td>";
            String torrentP = @"[\s\S]*?href=""([\S]*?)""";
            String sizeP = @"[\s\S]*?align=""center"">([\d|\.]*.B)</td>";
            String pubisherP = @"[\s\S]*?user_id/\d*?"">([\S]*?)</a>";
            pattern = dateP + kindP + titleP + torrentP + sizeP + pubisherP;
            itemList = new List<DMItem>();
            DMItem item = null;
            Regex rgx = new Regex(pattern, RegexOptions.Compiled);
            Regex rgx2 = new Regex(@"href=""([\s\S]*?)""");
            Regex rgx1 = new Regex(@"_blank"" >([\s\S]*)");
            matches = rgx.Matches(html);
            foreach (Match match in matches)
            {
                item = new DMItem();
                item.Date = match.Groups[1].ToString();
                item.kind = match.Groups[2].ToString();
                item.TorrentUrl = match.Groups[4].ToString();
                //item.Title = NetUtils.striphtml(match.Groups[3].ToString());
                item.size = match.Groups[5].ToString();
                item.pulisher = NetUtils.stripHtml(match.Groups[6].ToString());
                String[] s = Regex.Split(match.Groups[3].ToString(), @"</a>");
                if (s.Length == 2)
                {
                    String s1 = s[0];
                    item.Title = NetUtils.stripHtml(rgx1.Match(s1).Groups[1].ToString());
                    item.Team = null;
                    item.DetailUrl = rgx2.Match(s1).Groups[1].ToString();
                }
                else
                {
                    String s1 = s[0];
                    item.Team = s1.Substring(s1.LastIndexOf('>') + 1, s1.Length - s1.LastIndexOf('>') - 1);
                    item.TeamID = s1.Substring(s1.LastIndexOf('/') + 1, s1.LastIndexOf('"') - s1.LastIndexOf('/') - 1);
                    s1 = s[1];
                    item.Title = NetUtils.stripHtml(rgx1.Match(s1).Groups[1].ToString());
                    item.DetailUrl = rgx2.Match(s1).Groups[1].ToString();
                }
                itemList.Add(item);
            }
            this.Invoke(new MethodInvoker(() =>
            {
                //第二菜单加入详细新番名称
                ToolStripMenuItem MenuItem;
                menuStripName.Items.Clear();
                int length = 0;
                foreach (KeyValuePair<String, String> k in bill[currentWeek])
                {
                    MenuItem = new ToolStripMenuItem();
                    MenuItem.Size = new System.Drawing.Size(k.Key.Length * 20, 21);
                    MenuItem.Text = k.Key;
                    MenuItem.Click += new EventHandler(XinFanName_MouseDown);
                    menuStripName.Items.Add(MenuItem);
                    length += k.Key.Length;
                }
                menuStripName.MinimumSize = new System.Drawing.Size(0, (length / 60) * 23);
                menuStripWeek.Items[allWeek.IndexOf(currentWeek)].BackColor = Color.Blue;
                //类别下拉框
                toolStripComboBox2.SelectedIndex = 0;
                //更新ListView
                UpdateListView();
            }));
        }
        /************************************************************************/
        /* 根据itemList的数据，更新ListView。                                   */
        /************************************************************************/
        private void UpdateListView()
        {
            if (itemList == null)
            {
                return;
            }
            ListViewItem lv;
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    listView1.Items.Clear();
                    listView1.BeginUpdate();
                }));
            }
            else
            {
                listView1.Items.Clear();
                listView1.BeginUpdate();
            }
            foreach (DMItem item in itemList)
            {
                lv = new ListViewItem(new string[] { item.Date,
                item.Title, item.size ,item.kind,item.Team});
                if (listView1.Items.Count % 2 == 0)
                {
                    lv.BackColor = Color.FromArgb(0xccddff);
                }
                if (item.Date.StartsWith("今天"))
                {
                    lv.ForeColor = Color.Blue;
                }
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        listView1.Items.Add(lv);
                    }));
                }
                else
                {
                    listView1.Items.Add(lv);
                }
            }
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    listView1.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                    listView1.Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                    listView1.Columns[4].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                    listView1.EndUpdate();
                    int width = 0;
                    for (int i = 0; i < listView1.Columns.Count; i++)
                    {
                        width += listView1.Columns[i].Width;
                    }
                    this.Width = width + 40;                    
                }));
            }
            else
            {
                listView1.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                listView1.Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                listView1.Columns[4].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                listView1.EndUpdate();
                int width = 0;
                for (int i = 0; i < listView1.Columns.Count; i++)
                {
                    width += listView1.Columns[i].Width;
                }
                this.Width = width + 40;                
            }
            EnabledUI(true);

        }
        /************************************************************************/
        /* 根据url获取新番List。                                                */
        /************************************************************************/
        private List<DMItem> getDMItemList(String url)
        {
            EnabledUI(false);
            String html = NetUtils.getHtml(url);
            html = NetUtils.formateHtml(html);
            if (html == null)
            {
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
            foreach (Match match in matches)
            {
                item = new DMItem();
                item.Date = match.Groups[1].ToString();
                item.kind = match.Groups[2].ToString();
                item.TorrentUrl = match.Groups[4].ToString();
                item.Title = NetUtils.stripHtml(match.Groups[3].ToString());
                item.size = match.Groups[5].ToString();
                item.pulisher = NetUtils.stripHtml(match.Groups[6].ToString());
                String[] s = Regex.Split(match.Groups[3].ToString(), @"</a>");
                if (s.Length == 2)
                {
                    String s1 = s[0];
                    item.Title = NetUtils.stripHtml(rgx1.Match(s1).Groups[1].ToString());
                    item.Team = null;
                    item.DetailUrl = rgx2.Match(s1).Groups[1].ToString();
                }
                else
                {
                    String s1 = s[0];
                    item.Team = s1.Substring(s1.LastIndexOf('>') + 1, s1.Length - s1.LastIndexOf('>') - 1);
                    item.TeamID = s1.Substring(s1.LastIndexOf('/') + 1, s1.LastIndexOf('"') - s1.LastIndexOf('/') - 1);
                    s1 = s[1];
                    item.Title = NetUtils.stripHtml(rgx1.Match(s1).Groups[1].ToString());
                    item.DetailUrl = rgx2.Match(s1).Groups[1].ToString();
                }
                list.Add(item);
            }

            return list;
        }
        /************************************************************************/
        /* 获取item并更新ListView，多线程。                                     */
        /************************************************************************/
        private void GetUpdate(String url)
        {
            Thread t = new Thread(() =>
            {
                itemList = getDMItemList(url);
                UpdateListView();
            });
            t.IsBackground = true;
            t.Start();
        }
        /************************************************************************/
        /* 窗口使能                                                             */
        /************************************************************************/
        private void EnabledUI(bool b)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    this.menuStripName.Enabled = b;
                    this.menuStripWeek.Enabled = b;
                    this.listView1.Enabled = b;
                    this.UseWaitCursor = !b;
                    this.Refresh();
                }));
            }
            else
            {
                this.menuStripName.Enabled = b;
                this.menuStripWeek.Enabled = b;
                this.listView1.Enabled = b;
                this.UseWaitCursor = !b;
                this.Refresh();
            }            
        }
        /************************************************************************/
        /* 右键选择下载                                                         */
        /************************************************************************/
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
                NetUtils.getFile(selected.TorrentUrl, sfd.FileName);
                MessageBox.Show(this, sfd.FileName + "\n保存完成！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }        
        /************************************************************************/
        /* 双击鼠标，调用右键打开函数。                                         */
        /************************************************************************/
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            ToolStripMenuItemOpen_Click(null, null);
        }
        /************************************************************************/
        /* 右键选择打开                                                         */
        /************************************************************************/
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
                NetUtils.getFile(selected.TorrentUrl, EXEPath + "/torrent/" + selected.TorrentName);
                System.Diagnostics.Process.Start(EXEPath + "/torrent/" + selected.TorrentName);
            }
        }
        /************************************************************************/
        /* 鼠标右键，搜索当前选中的新番组+当前新番或搜索词                      */
        /************************************************************************/
        private void ToolStripMenuItemTeam_Click(object sender, EventArgs e)
        {
            if (currentXinFan == null)
            {
                MessageBox.Show("缺少关键字。");
                return;
            }
            ListViewItem item = listView1.SelectedItems[0];
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
            if (selected.TeamID == null)
            {
                MessageBox.Show("缺少字幕组id。");
                return;
            }
            String url = @"http://share.dmhy.org/topics/list?keyword="
                + currentXinFan + "+team_id:" + selected.TeamID;
            GetUpdate(url);
        }

        /************************************************************************/
        /* 右键选择详细，打开新窗口，显示新番具体内容。                         */
        /************************************************************************/
        private void MouseMenuDetail_Click(object sender, EventArgs e)
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
                new DetailWeb(selected.DetailUrl, selected.Title).Show();
            }
        }
        /************************************************************************/
        /* 右键-复制-标题                                                       */
        /************************************************************************/
        private void MouseMenuTitleCopy_Click(object sender, EventArgs e)
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
        /************************************************************************/
        /* 右键-复制-种子，复制种子下载页面地址。                               */
        /************************************************************************/
        private void MouseMenuTorrentCopy_Click(object sender, EventArgs e)
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

        /************************************************************************/
        /* 鼠标在星期菜单移动，第二菜单更新新番                                 */
        /************************************************************************/
        private void WeekMenu_MouseMove(object sender, MouseEventArgs e)
        {
            if (bill == null)
            {
                return;
            }
            String week = sender.ToString();
            if (week.Equals(currentWeek))
            {
                return;
            }
            ToolStripMenuItem MenuItem;
            menuStripName.Items.Clear();
            int length = 0;
            foreach (KeyValuePair<String, String> k in bill[week])
            {
                MenuItem = new ToolStripMenuItem();
                MenuItem.Size = new System.Drawing.Size(k.Key.Length * 20, 21);
                MenuItem.Text = k.Key;
                MenuItem.Click += new EventHandler(XinFanName_MouseDown);
                menuStripName.Items.Add(MenuItem);
                length += k.Key.Length;
            }
            menuStripName.MinimumSize = new System.Drawing.Size(0, (length / 60) * 23);


            menuStripWeek.Items[allWeek.IndexOf(week)].BackColor = Color.Blue;
            if (currentWeek != null)
            {
                menuStripWeek.Items[allWeek.IndexOf(currentWeek)].BackColor = Color.Silver;
            }
            currentWeek = week;

        }

        /************************************************************************/
        /* 鼠标点击新番，获取新番List，更新ListView                             */
        /************************************************************************/
        private void XinFanName_MouseDown(object sender, EventArgs e)
        {
            currentXinFan = bill[currentWeek][sender.ToString()];
            String url = @"http://share.dmhy.org/topics/list?keyword=" + currentXinFan;
            GetUpdate(url);
        }

        /************************************************************************/
        /* 点击搜索按钮，根据combox的kind搜索，获取List并更新                   */
        /************************************************************************/
        private void searchButton_Click(object sender, EventArgs e)
        {
            currentXinFan = kinds[toolStripComboBox2.SelectedItem.ToString()]
                + "+" + toolStripTextBox1.Text;
            String url = @"http://share.dmhy.org/topics/list?keyword=" + currentXinFan;
            GetUpdate(url);
        }
        /************************************************************************/
        /* 在搜索框点击键盘的enter键，调用搜索按钮的响应。                      */
        /************************************************************************/
        private void searchTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                searchButton_Click(null, null);
            }
        }
        /************************************************************************/
        /* 关闭主窗口时删除torrent文件夹。                                      */
        /************************************************************************/
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Directory.Delete(EXEPath + "/torrent", true);
        }
    }
}
