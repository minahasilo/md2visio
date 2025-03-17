using md2visio.mermaid._cmn;
using md2visio.mermaid.pie;
using md2visio.struc.figure;

namespace md2visio.struc.pie
{
    internal class PieBuilder : FigureBuilder
    {
        Pie pie = new Pie();
        public PieBuilder(SttIterator iter) : base(iter)
        {
        }

        public override void Build(string outputFile)
        {
            while (iter.HasNext())
            {
                SynState cur = iter.Next();
                if (cur is SttMermaidStart) { }
                if (cur is SttMermaidClose)
                {
                    pie.ToVisio(outputFile);
                    break;
                }
                if (cur is PieSttKeyword) { BuildKeyword(); }
                if (cur is PieSttTuple) { BuildDataItem(); }
                if (cur is SttComment) { pie.InitDirectiveFromComment(cur.Fragment); }
                if (cur is SttFrontMatter) { }
            }
        }

        void BuildKeyword()
        {
            string kw = iter.Current.Fragment;
            SynState next = iter.PeekNext();

            if (kw == "pie") {
                if(next is PieSttKeywordParam)
                {
                    var dict = ((PieSttKeywordParam) iter.Next()).ParsePieParam();
                    pie.ShowData = dict.ContainsKey("showData");
                    if(dict.ContainsKey("title")) pie.Title = dict["title"];
                }
            }
            else if (kw == "title")
            {
                if (next is not PieSttKeywordParam) throw new SynException("expected title", iter);
                pie.Title = iter.Next().Fragment;
            }
        }

        void BuildDataItem()
        {
            PieSttTuple tuple = (PieSttTuple) iter.Current;
            PieDataItem dataItem = pie.RetrieveNode<PieDataItem>(tuple.CompoList.Get(1));

            double data;
            if (!double.TryParse(tuple.CompoList.Get(2), out data) || 
                data <= 0) 
                throw new SynException("data must be a positive number", iter);
            dataItem.Data = data;

            pie.AddInnerNode(dataItem);
        }
    }
}
