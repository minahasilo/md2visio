using md2visio.mermaid._cmn;
using Microsoft.Office.Interop.Visio;

namespace md2visio.mermaid.pie
{
    internal class PieSttKeywordParam : SttKeywordParam
    {
        public override SynState NextState()
        {
            return Save(ExpectedGroups["param"].Value).Forward<PieSttChar>();
        }

        public Dictionary<string, string> ParsePieParam()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string[] items = Fragment.Split(" ");
            for (int i=0; i<items.Length; ++i)
            {
                string item = items[i];
                if(string.IsNullOrEmpty(item)) continue;

                if(item == "showData") dict.Add(item, item);
                else if(item == "title") i = SetTitle(dict, items, i + 1);
            }
            return dict;
        }

        int SetTitle(Dictionary<string, string> dict, string[] items, int index)
        {
            while (index < items.Length)
            {
                string item = items[index++];
                if (!string.IsNullOrEmpty(item))
                {
                    dict.Add("title", item);
                    break;
                } 
            }

            return index;
        }

    }
}
