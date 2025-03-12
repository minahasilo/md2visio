using md2visio.figure;
using md2visio.vsdx;

namespace md2visio.journey
{
    internal class Journey: Figure
    {
        string title = string.Empty;       

        public string Title { 
            get => title; 
            set => title = value.Trim(); 
        }
        public Journey() { }

        public HashSet<string> JoinerSet()
        {
            HashSet<string> set = new HashSet<string>();
            foreach (INode section in innerNodes.Values) 
            {
                foreach(string joiner in ((JoSection)section).JoinerSet())
                    set.Add(joiner);
            }
            return set;
        }

        public override void ToVisio(string path)
        {
            new VBuilderJo(this).Build(path);
        }
    }
}
