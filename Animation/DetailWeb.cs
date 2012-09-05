using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Animation
{
    public partial class DetailWeb : Form
    {
        public DetailWeb(String html,String title)
        {
            InitializeComponent();
            webBrowser1.DocumentText = html;
            this.Text = title;
        }
    }
}
