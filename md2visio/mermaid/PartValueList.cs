using System.Text;

namespace md2visio.mermaid
{
    internal class PartValueList
    {
        public static PartValueList Empty = new PartValueList();
        string whole = "whole";
        List<PartValue> values = new List<PartValue>();
        Dictionary<string, PartValue> parts = new Dictionary<string, PartValue>();

        public string Whole { get { return values[0].Value; } set { values[0].Value = value; } }

        public PartValueList() {
            Add(whole, string.Empty);  
        }

        public void Add(string key, string value) {
            if(parts.ContainsKey(key)) parts[key].Value = value;
            else
            {
                PartValue pv = new PartValue(key, value);
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

        public List<PartValue> Values()
        {
            return values;
        }

        public string MakeWhole()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in values)
            {
                if (item.Key == whole) continue;
                sb.Append(item.Value).Append(' ');
            }
            return sb.ToString();
        }
    }
}
