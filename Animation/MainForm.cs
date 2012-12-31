using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using Animation.Utils;
using Animation.ASource;

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
        //当前来源
        ResourcesKind currentRK;

        /*        Dictionary<String, String> kinds = null;*/

        IASource source;

        public MainForm() {
            EXEPath = System.Environment.CurrentDirectory;
            if (!Directory.Exists(EXEPath + "/torrent")) {
                Directory.CreateDirectory(EXEPath + "/torrent");
            }
            InitializeComponent();
            //窗口非使能
            EnabledUI(false);

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
            //来源初始化
            toolStripComboBoxSources.SelectedIndex = 0;
            currentRK = ResourcesKind.DMHY;
            source = Factory.create(currentRK);
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
        private void initIndex() {

            source.getBillAndItems(out bill, out itemList);
            this.Invoke(new MethodInvoker(() =>
            {
                //第二菜单加入详细新番名称
                ToolStripMenuItem MenuItem;
                menuStripName.Items.Clear();
                int length = 0;
                foreach (KeyValuePair<String, String> k in bill[currentWeek]) {
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
        private void UpdateListView() {
            if (itemList == null) {
                return;
            }
            ListViewItem lv;
            if (this.InvokeRequired) {
                this.Invoke(new MethodInvoker(() =>
                {
                    listView1.Items.Clear();
                    listView1.BeginUpdate();
                }));
            } else {
                listView1.Items.Clear();
                listView1.BeginUpdate();
            }
            foreach (DMItem item in itemList) {
                lv = new ListViewItem(new string[] { item.Date,
                item.Title, item.size ,item.kind,item.Team});
                if (listView1.Items.Count % 2 == 0) {
                    lv.BackColor = Color.FromArgb(0xccddff);
                }
                if (item.Date.StartsWith("今天")) {
                    lv.ForeColor = Color.Blue;
                }
                if (this.InvokeRequired) {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        listView1.Items.Add(lv);
                    }));
                } else {
                    listView1.Items.Add(lv);
                }
            }
            if (this.InvokeRequired) {
                this.Invoke(new MethodInvoker(() =>
                {
                    listView1.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                    listView1.Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                    listView1.Columns[4].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                    listView1.EndUpdate();
                    int width = 0;
                    for (int i = 0; i < listView1.Columns.Count; i++) {
                        width += listView1.Columns[i].Width;
                    }
                    this.Width = width + 40;
                }));
            } else {
                listView1.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                listView1.Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                listView1.Columns[4].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                listView1.EndUpdate();
                int width = 0;
                for (int i = 0; i < listView1.Columns.Count; i++) {
                    width += listView1.Columns[i].Width;
                }
                this.Width = width + 40;
            }
            EnabledUI(true);

        }

        private void GetUpdate(Kind kind, string keywords) {
            EnabledUI(false);
            Thread t = new Thread(() =>
            {
                itemList = source.getDMItem(kind, keywords);
                UpdateListView();
            });
            t.IsBackground = true;
            t.Start();
        }
        /************************************************************************/
        /* 窗口使能                                                             */
        /************************************************************************/
        private void EnabledUI(bool b) {
            if (this.InvokeRequired) {
                this.Invoke(new MethodInvoker(() =>
                {
                    this.menuStripName.Enabled = b;
                    this.menuStripWeek.Enabled = b;
                    this.listView1.Enabled = b;
                    this.UseWaitCursor = !b;
                    this.Refresh();
                }));
            } else {
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
        private void ToolStripMenuItemDownload_Click(object sender, EventArgs e) {
            foreach (ListViewItem item in listView1.SelectedItems) {
                String s = item.SubItems[1].Text;
                DMItem selected = itemList.Find(it =>
                {
                    if (it.Title.Equals(s)) {
                        return true;
                    } else {
                        return false;
                    }
                });
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "BT 文件(*.torrent)|*.torrent";
                sfd.RestoreDirectory = true;
                sfd.FileName = selected.TorrentName;
                if (sfd.ShowDialog() != DialogResult.OK) {
                    return;
                }
                source.getTorrent(selected.TorrentUrl, sfd.FileName);
                MessageBox.Show(this, sfd.FileName + "\n保存完成！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        /************************************************************************/
        /* 双击鼠标，调用右键打开函数。                                         */
        /************************************************************************/
        private void listView1_DoubleClick(object sender, EventArgs e) {
            ToolStripMenuItemOpen_Click(null, null);
        }
        /************************************************************************/
        /* 右键选择打开                                                         */
        /************************************************************************/
        private void ToolStripMenuItemOpen_Click(object sender, EventArgs e) {
            foreach (ListViewItem item in listView1.SelectedItems) {
                String s = item.SubItems[1].Text;
                DMItem selected = itemList.Find(it =>
                {
                    if (it.Title.Equals(s)) {
                        return true;
                    } else {
                        return false;
                    }
                });
                source.getTorrent(selected.TorrentUrl, EXEPath + "/torrent/" + selected.TorrentName);
                System.Diagnostics.Process.Start(EXEPath + "/torrent/" + selected.TorrentName);
            }
        }
        /************************************************************************/
        /* 鼠标右键，搜索当前选中的新番组+当前新番或搜索词                      */
        /************************************************************************/
        private void ToolStripMenuItemTeam_Click(object sender, EventArgs e) {
            if (currentRK==ResourcesKind.KTXP)
            {
                MessageBox.Show("KTXP资源不支持该功能。");
                return;
            } 

            if (currentXinFan == null) {
                MessageBox.Show("缺少关键字。");
                return;
            }
            ListViewItem item = listView1.SelectedItems[0];
            String s = item.SubItems[1].Text;
            DMItem selected = itemList.Find(it =>
            {
                if (it.Title.Equals(s)) {
                    return true;
                } else {
                    return false;
                }
            });
            if (selected.TeamID == null) {
                MessageBox.Show("缺少字幕组id。");
                return;
            }
            //             String url = @"http://share.dmhy.org/topics/list?keyword="
            //                 + currentXinFan + "+team_id:" + selected.TeamID;
            GetUpdate(Kind.ALL, currentXinFan + "+team_id:" + selected.TeamID);
        }

        /************************************************************************/
        /* 右键选择详细，打开新窗口，显示新番具体内容。                         */
        /************************************************************************/
        private void MouseMenuDetail_Click(object sender, EventArgs e) {
            foreach (ListViewItem item in listView1.SelectedItems) {
                String s = item.SubItems[1].Text;
                DMItem selected = itemList.Find(it =>
                {
                    if (it.Title.Equals(s)) {
                        return true;
                    } else {
                        return false;
                    }
                });
                new DetailWeb(selected.DetailUrl, selected.Title, source).Show();
            }
        }
        /************************************************************************/
        /* 右键-复制-标题                                                       */
        /************************************************************************/
        private void MouseMenuTitleCopy_Click(object sender, EventArgs e) {
            foreach (ListViewItem item in listView1.SelectedItems) {
                String s = item.SubItems[1].Text;
                DMItem selected = itemList.Find(it =>
                {
                    if (it.Title.Equals(s)) {
                        return true;
                    } else {
                        return false;
                    }
                });
                Clipboard.SetText(selected.Title);
            }
        }
        /************************************************************************/
        /* 右键-复制-种子，复制种子下载页面地址。                               */
        /************************************************************************/
        private void MouseMenuTorrentCopy_Click(object sender, EventArgs e) {
            foreach (ListViewItem item in listView1.SelectedItems) {
                String s = item.SubItems[1].Text;
                DMItem selected = itemList.Find(it =>
                {
                    if (it.Title.Equals(s)) {
                        return true;
                    } else {
                        return false;
                    }
                });
                Clipboard.SetText(selected.DetailUrl);
            }
        }

        /************************************************************************/
        /* 鼠标在星期菜单移动，第二菜单更新新番                                 */
        /************************************************************************/
        private void WeekMenu_MouseMove(object sender, MouseEventArgs e) {
            if (bill == null) {
                return;
            }
            String week = sender.ToString();
            if (week.Equals(currentWeek)) {
                return;
            }
            ToolStripMenuItem MenuItem;
            menuStripName.Items.Clear();
            int length = 0;
            foreach (KeyValuePair<String, String> k in bill[week]) {
                MenuItem = new ToolStripMenuItem();
                MenuItem.Size = new System.Drawing.Size(k.Key.Length * 20, 21);
                MenuItem.Text = k.Key;
                MenuItem.Click += new EventHandler(XinFanName_MouseDown);
                menuStripName.Items.Add(MenuItem);
                length += k.Key.Length;
            }
            menuStripName.MinimumSize = new System.Drawing.Size(0, (length / 60) * 23);


            menuStripWeek.Items[allWeek.IndexOf(week)].BackColor = Color.Blue;
            if (currentWeek != null) {
                menuStripWeek.Items[allWeek.IndexOf(currentWeek)].BackColor = Color.Silver;
            }
            currentWeek = week;

        }

        /************************************************************************/
        /* 鼠标点击新番，获取新番List，更新ListView                             */
        /************************************************************************/
        private void XinFanName_MouseDown(object sender, EventArgs e) {
            currentXinFan = bill[currentWeek][sender.ToString()];
            GetUpdate(Kind.ALL, currentXinFan);
        }


        /************************************************************************/
        /* 点击搜索按钮，根据combox的kind搜索，获取List并更新                   */
        /************************************************************************/
        private void searchButton_Click(object sender, EventArgs e) {
            GetUpdate((Kind)toolStripComboBox2.SelectedIndex, toolStripTextBox1.Text);
        }
        /************************************************************************/
        /* 在搜索框点击键盘的enter键，调用搜索按钮的响应。                      */
        /************************************************************************/
        private void searchTextBox_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)Keys.Enter) {
                searchButton_Click(null, null);
            }
        }
        /************************************************************************/
        /* 关闭主窗口时删除torrent文件夹。                                      */
        /************************************************************************/
        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            Directory.Delete(EXEPath + "/torrent", true);
        }
        /************************************************************************/
        /* 选择来源                                                             */
        /************************************************************************/
        private void toolStripComboBoxSources_SelectedIndexChanged(object sender, EventArgs e) {
            int index = ((ToolStripComboBox)sender).SelectedIndex;
            if ((int)currentRK == index) {
                return;
            }
            currentRK = (ResourcesKind)index;
            source = Factory.create(currentRK);
            EnabledUI(false);
            Thread t = new Thread(initIndex);
            t.IsBackground = true;
            t.Start();
        }
    }
}
