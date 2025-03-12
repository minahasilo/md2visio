using md2visio.figure;
using md2visio.mermaid.cmn;
using md2visio.mermaid.journey;

namespace md2visio.journey
{
    internal class JoBuilder : FigureBuilder
    {
        Journey journey = new Journey();
        JoSection curSection = JoSection.Empty;
        public JoBuilder(SttIterator iter) : base(iter) { }

        public override void Build(string outputFile)
        {
            while (iter.HasNext())
            {
                SynState cur = iter.Next();
                if (cur is SttMermaidStart) { }
                if (cur is SttMermaidClose) { 
                    journey.ToVisio(outputFile); 
                    break; 
                }
                if (cur is JoSttKeyword) { BuildKeyword(); }
                if (cur is JoSttTriple) { BuildTask(); }
                if (cur is SttComment) { }
                if (cur is SttConfig) { }
            }
        }

        void BuildKeyword()
        {
            string kw = iter.Current.Fragment;
            SynState next = iter.PeekNext();

            if (kw == "journey") { }
            if (kw == "title") {
                if (next is not JoSttKeywordParam) throw new SynException("expected title", iter);
                journey.Title = iter.Next().Fragment;
            }
            if (kw == "section") {
                if (next is not JoSttKeywordParam) throw new SynException("expected section title", iter);

                curSection = journey.RetrieveNode<JoSection>(iter.Next().Fragment);
                journey.AddInnerNode(curSection);
            }

        }

        void BuildTask()
        {
            if(curSection.IsEmpty()) throw new SynException("expected section", iter);

            JoSttTriple triple = (JoSttTriple) iter.Current;
            string[] compo = triple.Fragment.Split(":");
            if (compo.Length != 3) throw new SynException("error task format", iter);

            JoTask task = new JoTask(compo[0].Trim(), compo[1].Trim(), compo[2].Trim());
            curSection.AddTask(task);
        }
    }
}
