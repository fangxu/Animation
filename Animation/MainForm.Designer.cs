namespace Animation
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItemDetail = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemTitleCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemtorrentCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemDownload = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.listView1 = new System.Windows.Forms.ListView();
            this.listView_date = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listView_title = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listView_size = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listView_kind = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listView_team = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuStripWeek = new System.Windows.Forms.MenuStrip();
            this.ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBox3 = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.menuStripName = new System.Windows.Forms.MenuStrip();
            this.contextMenuStrip1.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.menuStripWeek.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemDetail,
            this.ToolStripMenuItemCopy,
            this.ToolStripMenuItemDownload,
            this.ToolStripMenuItemOpen});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(101, 92);
            // 
            // ToolStripMenuItemDetail
            // 
            this.ToolStripMenuItemDetail.Name = "ToolStripMenuItemDetail";
            this.ToolStripMenuItemDetail.Size = new System.Drawing.Size(100, 22);
            this.ToolStripMenuItemDetail.Text = "介绍";
            this.ToolStripMenuItemDetail.Click += new System.EventHandler(this.ToolStripMenuItemDetail_Click);
            // 
            // ToolStripMenuItemCopy
            // 
            this.ToolStripMenuItemCopy.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemTitleCopy,
            this.ToolStripMenuItemtorrentCopy});
            this.ToolStripMenuItemCopy.Name = "ToolStripMenuItemCopy";
            this.ToolStripMenuItemCopy.Size = new System.Drawing.Size(100, 22);
            this.ToolStripMenuItemCopy.Text = "复制";
            // 
            // ToolStripMenuItemTitleCopy
            // 
            this.ToolStripMenuItemTitleCopy.Name = "ToolStripMenuItemTitleCopy";
            this.ToolStripMenuItemTitleCopy.Size = new System.Drawing.Size(100, 22);
            this.ToolStripMenuItemTitleCopy.Text = "标题";
            this.ToolStripMenuItemTitleCopy.Click += new System.EventHandler(this.ToolStripMenuItemTitleCopy_Click);
            // 
            // ToolStripMenuItemtorrentCopy
            // 
            this.ToolStripMenuItemtorrentCopy.Name = "ToolStripMenuItemtorrentCopy";
            this.ToolStripMenuItemtorrentCopy.Size = new System.Drawing.Size(100, 22);
            this.ToolStripMenuItemtorrentCopy.Text = "链接";
            this.ToolStripMenuItemtorrentCopy.Click += new System.EventHandler(this.ToolStripMenuItemtorrentCopy_Click);
            // 
            // ToolStripMenuItemDownload
            // 
            this.ToolStripMenuItemDownload.Name = "ToolStripMenuItemDownload";
            this.ToolStripMenuItemDownload.Size = new System.Drawing.Size(100, 22);
            this.ToolStripMenuItemDownload.Text = "下载";
            this.ToolStripMenuItemDownload.Click += new System.EventHandler(this.ToolStripMenuItemDownload_Click);
            // 
            // ToolStripMenuItemOpen
            // 
            this.ToolStripMenuItemOpen.Name = "ToolStripMenuItemOpen";
            this.ToolStripMenuItemOpen.Size = new System.Drawing.Size(100, 22);
            this.ToolStripMenuItemOpen.Text = "打开";
            this.ToolStripMenuItemOpen.Click += new System.EventHandler(this.ToolStripMenuItemOpen_Click);
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.listView1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(909, 432);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            this.toolStripContainer1.Size = new System.Drawing.Size(909, 506);
            this.toolStripContainer1.TabIndex = 7;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.menuStripWeek);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip2);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.menuStripName);
            // 
            // listView1
            // 
            this.listView1.BackColor = System.Drawing.SystemColors.Window;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.listView_date,
            this.listView_title,
            this.listView_size,
            this.listView_kind,
            this.listView_team});
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.ShowItemToolTips = true;
            this.listView1.Size = new System.Drawing.Size(909, 432);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // listView_date
            // 
            this.listView_date.Text = "date";
            // 
            // listView_title
            // 
            this.listView_title.Text = "title";
            // 
            // listView_size
            // 
            this.listView_size.Text = "size";
            // 
            // listView_kind
            // 
            this.listView_kind.Text = "kind";
            // 
            // listView_team
            // 
            this.listView_team.Text = "team";
            // 
            // menuStripWeek
            // 
            this.menuStripWeek.AutoSize = false;
            this.menuStripWeek.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStripWeek.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem1,
            this.ToolStripMenuItem2,
            this.ToolStripMenuItem3,
            this.ToolStripMenuItem4,
            this.ToolStripMenuItem5,
            this.ToolStripMenuItem6,
            this.ToolStripMenuItem7});
            this.menuStripWeek.Location = new System.Drawing.Point(6, 0);
            this.menuStripWeek.Name = "menuStripWeek";
            this.menuStripWeek.Size = new System.Drawing.Size(438, 25);
            this.menuStripWeek.Stretch = false;
            this.menuStripWeek.TabIndex = 2;
            this.menuStripWeek.Text = "menuStrip2";
            // 
            // ToolStripMenuItem1
            // 
            this.ToolStripMenuItem1.BackColor = System.Drawing.Color.Silver;
            this.ToolStripMenuItem1.Name = "ToolStripMenuItem1";
            this.ToolStripMenuItem1.Size = new System.Drawing.Size(56, 21);
            this.ToolStripMenuItem1.Text = "星期一";
            this.ToolStripMenuItem1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ToolStripMenuItem_MouseMove);
            // 
            // ToolStripMenuItem2
            // 
            this.ToolStripMenuItem2.BackColor = System.Drawing.Color.Silver;
            this.ToolStripMenuItem2.Name = "ToolStripMenuItem2";
            this.ToolStripMenuItem2.Size = new System.Drawing.Size(56, 21);
            this.ToolStripMenuItem2.Text = "星期二";
            this.ToolStripMenuItem2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ToolStripMenuItem_MouseMove);
            // 
            // ToolStripMenuItem3
            // 
            this.ToolStripMenuItem3.BackColor = System.Drawing.Color.Silver;
            this.ToolStripMenuItem3.Name = "ToolStripMenuItem3";
            this.ToolStripMenuItem3.Size = new System.Drawing.Size(56, 21);
            this.ToolStripMenuItem3.Text = "星期三";
            this.ToolStripMenuItem3.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ToolStripMenuItem_MouseMove);
            // 
            // ToolStripMenuItem4
            // 
            this.ToolStripMenuItem4.BackColor = System.Drawing.Color.Silver;
            this.ToolStripMenuItem4.Name = "ToolStripMenuItem4";
            this.ToolStripMenuItem4.Size = new System.Drawing.Size(56, 21);
            this.ToolStripMenuItem4.Text = "星期四";
            this.ToolStripMenuItem4.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ToolStripMenuItem_MouseMove);
            // 
            // ToolStripMenuItem5
            // 
            this.ToolStripMenuItem5.BackColor = System.Drawing.Color.Silver;
            this.ToolStripMenuItem5.Name = "ToolStripMenuItem5";
            this.ToolStripMenuItem5.Size = new System.Drawing.Size(56, 21);
            this.ToolStripMenuItem5.Text = "星期五";
            this.ToolStripMenuItem5.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ToolStripMenuItem_MouseMove);
            // 
            // ToolStripMenuItem6
            // 
            this.ToolStripMenuItem6.BackColor = System.Drawing.Color.Silver;
            this.ToolStripMenuItem6.Name = "ToolStripMenuItem6";
            this.ToolStripMenuItem6.Size = new System.Drawing.Size(56, 21);
            this.ToolStripMenuItem6.Text = "星期六";
            this.ToolStripMenuItem6.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ToolStripMenuItem_MouseMove);
            // 
            // ToolStripMenuItem7
            // 
            this.ToolStripMenuItem7.BackColor = System.Drawing.Color.Silver;
            this.ToolStripMenuItem7.Name = "ToolStripMenuItem7";
            this.ToolStripMenuItem7.Size = new System.Drawing.Size(56, 21);
            this.ToolStripMenuItem7.Text = "星期日";
            this.ToolStripMenuItem7.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ToolStripMenuItem_MouseMove);
            // 
            // toolStrip2
            // 
            this.toolStrip2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.toolStrip2.AutoSize = false;
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolStripComboBox3,
            this.toolStripSeparator2,
            this.toolStripLabel2,
            this.toolStripTextBox1,
            this.toolStripButton1});
            this.toolStrip2.Location = new System.Drawing.Point(463, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(420, 25);
            this.toolStrip2.TabIndex = 3;
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(44, 22);
            this.toolStripLabel1.Text = "分类：";
            // 
            // toolStripComboBox3
            // 
            this.toolStripComboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBox3.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.toolStripComboBox3.Name = "toolStripComboBox3";
            this.toolStripComboBox3.Size = new System.Drawing.Size(121, 25);
            this.toolStripComboBox3.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBox3_SelectedIndexChanged);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(56, 17);
            this.toolStripLabel2.Text = "关键字：";
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(100, 23);
            this.toolStripTextBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolStripTextBox1_KeyPress);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.ForeColor = System.Drawing.Color.Blue;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(36, 22);
            this.toolStripButton1.Text = "搜索";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // menuStripName
            // 
            this.menuStripName.AutoSize = false;
            this.menuStripName.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStripName.Location = new System.Drawing.Point(0, 25);
            this.menuStripName.Name = "menuStripName";
            this.menuStripName.Size = new System.Drawing.Size(909, 24);
            this.menuStripName.TabIndex = 1;
            this.menuStripName.Text = "menuStrip1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(909, 506);
            this.Controls.Add(this.toolStripContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStripName;
            this.Name = "MainForm";
            this.Text = "Animation";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.contextMenuStrip1.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.menuStripWeek.ResumeLayout(false);
            this.menuStripWeek.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemDownload;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader listView_date;
        private System.Windows.Forms.ColumnHeader listView_title;
        private System.Windows.Forms.ColumnHeader listView_size;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemOpen;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemDetail;
        private System.Windows.Forms.ColumnHeader listView_kind;
        private System.Windows.Forms.ColumnHeader listView_team;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemCopy;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemtorrentCopy;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemTitleCopy;
        private System.Windows.Forms.MenuStrip menuStripWeek;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem7;
        private System.Windows.Forms.MenuStrip menuStripName;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
    }
}

