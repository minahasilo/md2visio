using Microsoft.Office.Interop.Visio;
using System.Text;

namespace md2visio.mermaid._cmn
{
    internal class MmdJsonObj
    {
        public static MmdJsonObj Empty = new MmdJsonObj();

        Dictionary<string, string> data = new Dictionary<string, string>();

        public MmdJsonObj() { }

        public string this[string key]
        {
            get { return data[key]; }
            set
            {
                data[key] = value;
            }
        }

        public bool HasKey(string key)
        {
            return data.ContainsKey(key);
        }

        public MmdJsonObj(string text)
        {
            Build(text);
        }

        public void Join(MmdJsonObj json)
        {
            foreach (string key in json.data.Keys)
                data[key] = json.data[key];
        }

        void Build(string text)
        {
            StringBuilder key = new StringBuilder();
            StringBuilder value = new StringBuilder();
            bool withInQuote = false;
            bool withInSQuote = false;
            bool appendKey = true;
            foreach (char c in text)
            {
                if (c == '"') withInQuote = !withInQuote;
                else if (c == '\'') withInSQuote = !withInSQuote;

                if (withInQuote || withInSQuote)
                {
                    Append(key, value, c, appendKey);
                    continue;
                }

                if (c == ':')
                {
                    appendKey = !appendKey;
                    continue;
                }
                else if (c == ',')
                {
                    appendKey = !appendKey;
                    Add(key, value);
                    continue;
                }
                else if (c == '{') continue;
                else if (c == '}')
                {
                    TryAdd(key, value);
                    continue;
                }

                Append(key, value, c, appendKey);
            }
            TryAdd(key, value);
        }

        void TryAdd(StringBuilder key, StringBuilder value)
        {
            string k = key.ToString().Trim();
            string v = value.ToString().Trim();
            if (k.Length > 0 && v.Length > 0)
            {
                data[k] = v;
            }
            key.Clear();
            value.Clear();
        }

        void Add(StringBuilder key, StringBuilder value)
        {
            string k = key.ToString().Trim();
            string v = value.ToString().Trim();
            Assert("JSON key can't be empty", !string.IsNullOrEmpty(k));
            Assert("JSON value can't be empty", !string.IsNullOrEmpty(v));
            data[k] = v;
            key.Clear();
            value.Clear();
        }

        void Append(StringBuilder key, StringBuilder value, char c, bool appendKey)
        {
            if (appendKey) key.Append(c);
            else value.Append(c);
        }

        void Assert(string message, bool assert)
        {
            if (!assert) throw new ArgumentException(message);
        }
    }
}
