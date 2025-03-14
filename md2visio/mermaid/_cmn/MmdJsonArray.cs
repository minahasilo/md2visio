using md2visio.struc.figure;
using System.Text;

namespace md2visio.mermaid._cmn
{
    internal class MmdJsonArray: IEmpty
    {
        public static readonly MmdJsonArray Empty = new MmdJsonArray();
        List<object> list = new List<object>();
        StringBuilder textBuilder = new StringBuilder();
        int index = 0;

        public int Index { get { return index; } }

        public MmdJsonArray() { }

        public MmdJsonArray(StringBuilder textBuilder, int index)
        {
            this.textBuilder = textBuilder;
            this.index = index;

            Build();
        }

        void Build()
        {
            StringBuilder item = new StringBuilder();
            bool withInQuote = false;
            bool withInSQuote = false;
            for (; index < textBuilder.Length; ++index)
            {
                char c = textBuilder[index];
                if (c == '"') withInQuote = !withInQuote;
                else if (c == '\'') withInSQuote = !withInSQuote;

                if (withInQuote || withInSQuote)
                {
                    item.Append(c);
                    continue;
                }

                if (c == ',')
                {
                    Add(item);
                    continue;
                }
                else if (c == '{')
                {
                    MmdJsonObj obj = new MmdJsonObj(textBuilder, index);
                    Add(obj);
                    index = obj.Index;
                    continue;
                }
                else if (c == '[')
                {
                    MmdJsonArray arr = new MmdJsonArray(textBuilder, index);
                    Add(arr);
                    index = arr.Index;
                    continue;
                }
                else if (c == ']')
                {
                    Add(item);
                    break;
                }
                else if (c == ' ') continue;

                item.Append(c);
            }
            Add(item); // if not closed by ']'
        }

        public bool IsEmpty()
        {
            return this == Empty;
        }

        void Add(object value)
        {
            if (value is StringBuilder)
            { 
                StringBuilder stringBuilder = (StringBuilder) value;
                string v = TrimItem(stringBuilder);
                if (v.Length > 0) list.Add(value); 
                stringBuilder.Clear();
            }
            else list.Add(value);
        }

        string TrimItem(StringBuilder stringBuilder)
        {
            char[] trims = new char[] { '"', '\''};
            return stringBuilder.ToString().Trim().TrimStart(trims).TrimEnd(trims);
        }
    }
}
