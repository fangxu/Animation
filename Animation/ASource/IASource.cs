using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Animation.ASource
{
    public interface IASource
    {
        Dictionary<String, Dictionary<String, String>> getBill();
        // List<DMItem> getDMItem(String url=null);
        void getBillAndItems(out Dictionary<String, Dictionary<String, String>> bill, out List<DMItem> items);
        List<DMItem> getDMItem(Kind kind, String keywords = null);
        void getTorrent(String tURL, String PathName);
        String getDetailHtml(String url);
        String getComment(String url);
    }
}
