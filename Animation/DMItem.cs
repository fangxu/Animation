using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Animation
{
    class DMItem
    {
        private DateTime date;
        public String Date
        {
            get
            {
                String dateS = null;
                if (date.Date == DateTime.Today)
                {
                    dateS = "今天 " + date.ToShortTimeString();
                }
                else if (date.Date == DateTime.Today.AddDays(-1))
                {
                    dateS = "昨天 " + date.ToShortTimeString();
                }
                else if (date.Year == DateTime.Today.Year)
                {
                    dateS = date.Month + "/" + date.Day + " " + date.ToShortTimeString();
                }
                else
                {
                    dateS = date.ToShortDateString() + " " + date.ToShortTimeString();
                }
                return dateS;
            }
            set
            {
                date = DateTime.Parse(value);
            }
        }
        private String title;

        public String size { get; set; }
        private String team;
        public String Team
        {
            get
            {
                if (team == null)
                {
                    return "个人";
                }
                else
                {
                    return team;
                }
            }
            set { team = value; }
        }
        public String kind { get; set; }
        private String torrentUrl;
        private String detailUrl;
        public System.String DetailUrl
        {
            get { return detailUrl; }
            set { detailUrl = @"http://share.dmhy.org" + value; }
        }
        public String TorrentName
        {
            get
            {
                return torrentUrl.Substring(torrentUrl.LastIndexOf('/') + 1,
                    torrentUrl.Length - torrentUrl.LastIndexOf('/') - 1) + ".torrent";
            }
        }
        public System.String TorrentUrl
        {
            get { return torrentUrl; }
            set { torrentUrl = @"http://share.dmhy.org" + value; }
        }
        public System.String Title
        {
            get { return title; }
            set
            {
                title = new Regex(@"\r|\n|\t").Replace(value, "");
            }
        }

        public DMItem(String title)
        {
            this.title = title;
        }

        public DMItem()
        {

        }

        public override Int32 GetHashCode()
        {
            return StringComparer.CurrentCulture.GetHashCode(this.title);
        }

        public override System.String ToString()
        {
            return date.ToString() + " " + title + " " + size + " " + team + " " + torrentUrl;
        }

        public override Boolean Equals(System.Object obj)
        {
            return String.Equals(((DMItem)obj).title, this.title);
        }
    }
}
