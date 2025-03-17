using md2visio.struc.figure;
using System.Text;

namespace md2visio.mermaid._cmn
{
    internal class MmdJsonArray: ValueGetter, IEmpty
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
                    AddString(item);
                    continue;
                }
                else if (c == '{')
                {
                    MmdJsonObj obj = new MmdJsonObj(textBuilder, index);
                    AddJsonObj(obj);
                    index = obj.Index;
                    continue;
                }
                else if (c == '[')
                {
                    MmdJsonArray arr = new MmdJsonArray(textBuilder, index);
                    AddJsonObj(arr);
                    index = arr.Index;
                    continue;
                }
                else if (c == ']')
                {
                    AddString(item);
                    return;
                }
                else if (c == ' ') continue;

                item.Append(c);
            }
            AddString(item); // not closed by ']'
        }

        public object? this[string key]
        {
            get
            {
                int v = 0; 
                if(!int.TryParse(key, out v)) return null;
                return this[v];
            }
        }

        public object? this[int index]
        {
            get { return index >=0 && index < list.Count ? list[index] : null; }
        }

        public bool IsEmpty()
        {
            return this == Empty;
        }

        void AddString(StringBuilder item)
        {
            string v = TrimSpaceAndQuote(item);
            if (v.Length > 0) list.Add(v);
            item.Clear();
        }

        void AddJsonObj(object value)
        {
            list.Add(value);
        }

        string TrimSpaceAndQuote(StringBuilder stringBuilder)
        {
            char[] trims = new char[] { '"', '\''};
            return stringBuilder.ToString().Trim().TrimStart(trims).TrimEnd(trims);
        }

        public override T? GetValue<T>(string keyPath) where T : class
        {
            if (this[keyPath] is T) return this[keyPath] as T;
            return null;
        }
    }
}
