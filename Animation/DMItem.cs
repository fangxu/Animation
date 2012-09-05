using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Animation
{
    class DMItem
    {
        public DateTime data { get; set; }
        private String title;

        public String size { get; set; }
        public String team { get; set; }
        public String kind { get; set; }
        private String torrentUrl;
        private String detailUrl;
        public System.String DetailUrl
        {
            get { return detailUrl; }
            set { detailUrl = @"http://bt.ktxp.com" + value; }
        }
        public String TorrentName
        {
            get { return torrentUrl.Substring(torrentUrl.LastIndexOf('/')+1,
                torrentUrl.Length-torrentUrl.LastIndexOf('/')-1); }            
        }
        public System.String TorrentUrl
        {
            get { return torrentUrl; }
            set { torrentUrl = @"http://bt.ktxp.com" + value; }
        }
        public System.String Title
        {
            get { return title; }
            set { title = value; }
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
            return data.ToString() + " " + title + " " + size + " " + team + " " + torrentUrl;
        }

        public override Boolean Equals(System.Object obj)
        {
            return String.Equals(((DMItem)obj).title, this.title);
        }
    }
}
