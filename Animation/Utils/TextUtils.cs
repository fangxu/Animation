using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Animation.Utils
{
    class TextUtils
    {
        public static String weekToEng(String week)
        {
            String eng = null;
            switch (week)
            {
                case "星期一":
                    eng = "mon";
                    break;
                case "星期二":
                    eng = "tue";
                    break;
                case "星期三":
                    eng = "wed";
                    break;
                case "星期四":
                    eng = "thu";
                    break;
                case "星期五":
                    eng = "fri";
                    break;
                case "星期六":
                    eng = "sat";
                    break;
                case "星期日":
                    eng = "sun";
                    break;
            }
            return eng;
        }

        //星期，英文转中文
        public static String engToWeek(String eng)
        {
            String week = null;
            switch (eng)
            {
                case "mon":
                    week = "星期一";
                    break;
                case "tue":
                    week = "星期二";
                    break;
                case "wed":
                    week = "星期三";
                    break;
                case "thu":
                    week = "星期四";
                    break;
                case "fri":
                    week = "星期五";
                    break;
                case "sat":
                    week = "星期六";
                    break;
                case "sun":
                    week = "星期日";
                    break;
            }
            return week;
        }
    }
}
