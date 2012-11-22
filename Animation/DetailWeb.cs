using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading;

namespace Animation
{
    public partial class DetailWeb : Form
    {
        public DetailWeb(String url, String title)
        {
            InitializeComponent();
            webBrowser1.DocumentText = @"<h1>正在加载。。。</h1>";
            this.Text = title;
            updateHtml(url);
        }

        private void updateHtml(string url)
        {
            Thread t = new Thread(() =>
             {
                 String html = getDetailHtml(url);
                 this.BeginInvoke(new MethodInvoker(() =>
                 {
                     webBrowser1.DocumentText = html;
                 }));
             });
            t.IsBackground = true;
            t.Start();
        }

        private String getDetailHtml(String url)
        {
            String html = Utils.NetUtils.getHtml(url);
            Match m = Regex.Match(html, @"(<div class=""topic-nfo box ui-corner-all"">[\s\S]*?)<div id=""play-asia"" class=""box ui-corner-all"">");
            return @"<link href=""http://share.dmhy.org/min/g=css&v=10"" rel=""stylesheet"" type=""text/css"" />" + @"<div class=""topic-main"">" + m.Groups[1].ToString() + "</div>";
        }        
    }
}
