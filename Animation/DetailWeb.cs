using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading;
using Animation.ASource;

namespace Animation
{
    public partial class DetailWeb : Form
    {
        private IASource source;
        Thread t;

        public DetailWeb(String url, String title, IASource source = null) {
            InitializeComponent();
            webBrowser1.DocumentText = @"<h1>正在加载。。。</h1>";
            this.Text = title;
            this.source = source;
            updateHtml(url);
        }

        private void updateHtml(string url) {
            t = new Thread(() =>
             {
                 String html = source.getDetailHtml(url);
                 if (this.InvokeRequired) {
                     this.BeginInvoke(new MethodInvoker(() =>
                         {
                             webBrowser1.DocumentText = html;
                         }));
                 } else {
                     webBrowser1.DocumentText = html;
                 }

             });
            t.IsBackground = true;
            t.Start();
        }

        private void DetailWeb_FormClosed(object sender, FormClosedEventArgs e) {
            if (t.IsAlive) {
                t.Abort();
            }
        }
    }
}
