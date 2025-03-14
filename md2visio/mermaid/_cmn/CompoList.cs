using System.Text;

namespace md2visio.mermaid._cmn
{
    internal class CompoList
    {
        public static CompoList Empty = new CompoList();
        string entire = "entire";
        List<CompoValue> values = new List<CompoValue>();
        Dictionary<string, CompoValue> parts = new Dictionary<string, CompoValue>();

        public string Entire { get { return values[0].Value; } set { values[0].Value = value; } }

        public CompoList()
        {
            Add(entire, string.Empty);
        }

        public void Add(string key, string value)
        {
            if (parts.ContainsKey(key)) parts[key].Value = value;
            else
            {
                CompoValue pv = new CompoValue(key, value);
                values.Add(pv);
                parts.Add(key, pv);
            }
        }

        public void Add(string value)
        {
            Add($"{values.Count}", value);
        }

        public string Get(string key)
        {
            return parts[key].Value;
        }

        public string Get(int index)
        {
            return values[index].Value;
        }

        public List<CompoValue> Values()
        {
            return values;
        }

        public string MakeWhole()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in values)
            {
                if (item.Key == entire) continue;
                sb.Append(item.Value).Append(' ');
            }
            return sb.ToString();
        }
    }
}
