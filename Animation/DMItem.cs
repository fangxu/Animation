﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

namespace Animation
{
    public class DMItem
    {
        private DateTime date;
        private String title;
        public String size { get; set; }
        public String pulisher { get; set; }
        private String team;
        private String teamID;
        public String kind { get; set; }
        private String torrentUrl;
        private String detailUrl;

        public System.String TeamID {
            get { return teamID; }
            set { teamID = value; }
        }
        public String Date {
            get {
                String dateS = null;
                if (date.Date == DateTime.Today) {
                    dateS = "今天 " + date.ToShortTimeString();
                } else if (date.Date == DateTime.Today.AddDays(-1)) {
                    dateS = "昨天 " + date.ToShortTimeString();
                } else if (date.Year == DateTime.Today.Year) {
                    dateS = date.Month + "/" + date.Day + " " + date.ToShortTimeString();
                } else {
                    dateS = date.ToShortDateString() + " " + date.ToShortTimeString();
                }
                return dateS;
            }
            set {
                date = DateTime.Parse(value);
            }
        }

        public String Team {
            get {
                if (team == null) {
                    return pulisher;
                } else {
                    return team;
                }
            }
            set { team = value; }
        }

        public System.String DetailUrl {
            get { return detailUrl; }
            set { detailUrl =  value; }
        }
        public String TorrentName {
            get {
                return torrentUrl.Substring(torrentUrl.LastIndexOf('/') + 1,
                    torrentUrl.Length - torrentUrl.LastIndexOf('/') - 1) + ".torrent";
            }
        }
        public System.String TorrentUrl {
            get { return torrentUrl; }
            set { torrentUrl =  value; }
        }
        public System.String Title {
            get {
                return title;
            }
            set {
                title = new Regex(@"\r|\n|\t| ").Replace(value, "");
            }
        }

        public DMItem(String title) {
            this.title = title;
        }

        public DMItem() {

        }

        public override Int32 GetHashCode() {
            return StringComparer.CurrentCulture.GetHashCode(this.title);
        }

        public override System.String ToString() {
            return date.ToString() + " " + title + " " + size + " " + team + " " + torrentUrl;
        }

        public override Boolean Equals(System.Object obj) {
            return String.Equals(((DMItem)obj).title, this.title);
        }
    }
}
