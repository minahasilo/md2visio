using System.Text;

namespace md2visio.mermaid._cmn
{
    internal class MmdJsonObj
    {
        public static MmdJsonObj Empty = new MmdJsonObj();

        Dictionary<string, object> data = new Dictionary<string, object>(); // string -> string/MmdJsonObj/MmdJsonArray
        StringBuilder textBuilder = new StringBuilder();
        int index = 0;

        public int Index { get { return index; } }
        public MmdJsonObj() { }

        public MmdJsonObj(string text)
        {
            textBuilder.Append(text);
            Build();
        }

        public MmdJsonObj(StringBuilder textBuilder, int index)
        {
            this.textBuilder = textBuilder;
            this.index = index;
            Build();
        }

        public string? GetString(string keyPath)
        {
            return GetValue<string>(keyPath);
        }

        public (bool success, int r) GetInt(string keyPath)
        {
            string? val = GetValue<string>(keyPath);
            int r = 0;
            bool success = int.TryParse(val, out r);
            return (success, r);
        }

        public (bool success, double r) GetDouble(string keyPath)
        {
            string? val = GetValue<string>(keyPath);
            double r = 0;
            bool success = double.TryParse(val, out r);
            return (success, r);
        }

        T? GetValue<T>(string keyPath) where T : class
        {
            if (!keyPath.Contains("."))
            {
                if (data.ContainsKey(keyPath) && 
                    data[keyPath] is T) 
                    return (T)data[keyPath];
            }
            else
            {
                string[] path = keyPath.Split('.');
                int count = path.Length;
                MmdJsonObj obj = this;
                object? result = null;
                foreach (string pathItem in path)
                {
                    --count;
                    result = obj.GetValue<object>(pathItem);
                    if (result is MmdJsonObj) obj = (MmdJsonObj) result;
                    else break;
                }
                if (count == 0 && result is T) return (T) result;
            }
            return null;
        }

        public object this[string key]
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

        public void Join(MmdJsonObj json)
        {
            foreach (string key in json.data.Keys)
                data[key] = json.data[key];
        }

        void Build()
        {
            StringBuilder keyBuilder = new StringBuilder();
            StringBuilder valueBuilder = new StringBuilder();
            bool withInQuote = false;
            bool withInSQuote = false;
            bool bAppendKey = true;
            for (; index < textBuilder.Length; ++index)
            {
                char c = textBuilder[index];
                if (c == '"') withInQuote = !withInQuote;
                else if (c == '\'') withInSQuote = !withInSQuote;

                if (withInQuote || withInSQuote)
                {
                    Append(keyBuilder, valueBuilder, c, bAppendKey);
                    continue;
                }

                if (c == ' ') continue;
                else if (c == ':')
                {
                    Assert("JSON key can't be empty", !string.IsNullOrEmpty(TrimSpaceAndQuote(keyBuilder)));
                    bAppendKey = !bAppendKey;
                    continue;
                }
                else if (c == ',')
                {
                    bAppendKey = !bAppendKey;
                    AddString(keyBuilder, valueBuilder);
                    continue;
                }
                else if (c == '{')
                {
                    if (TrimSpaceAndQuote(keyBuilder).Length > 0)
                    {
                        MmdJsonObj obj = new MmdJsonObj(textBuilder, index);
                        AddJsonObj(keyBuilder, obj);
                        index = obj.Index;
                    }
                    continue;
                }
                else if (c == '}')
                {
                    AddString(keyBuilder, valueBuilder);
                    return;
                }
                else if(c == '[')
                {
                    MmdJsonArray arr = new MmdJsonArray(textBuilder, index);
                    AddJsonObj(keyBuilder, arr);
                    index = arr.Index;
                    continue;
                }
                else if(c == ']')
                {
                    AddString(keyBuilder, valueBuilder);
                    continue;
                }

                Append(keyBuilder, valueBuilder, c, bAppendKey);
            }
            AddString(keyBuilder, valueBuilder); // not closed by '}'
        }

        void AddString(StringBuilder key, StringBuilder value)
        {
            string k = TrimSpaceAndQuote(key);
            if (k.Length == 0) return;

            string v = TrimSpaceAndQuote(value);
            Assert("JSON value can't be empty", !string.IsNullOrEmpty(v));
            data[k] = v;
            key.Clear();
            value.Clear();
        }

        void AddJsonObj(StringBuilder key, object v)
        {
            string k = TrimSpaceAndQuote(key);
            Assert("JSON key can't be empty", !string.IsNullOrEmpty(k));

            data[k] = v;
            key.Clear();
        }         

        string TrimSpaceAndQuote(StringBuilder key)
        {
            char[] trims = new char[] { '"', '\''};
            return key.ToString().Trim().TrimStart(trims).TrimEnd(trims);
        }

        void Append(StringBuilder key, StringBuilder value, char c, bool appendKey)
        {
            if (appendKey) key.Append(c);
            else value.Append(c);
        }

        void Assert(string message, bool assert)
        {
            if (!assert) 
                throw new ArgumentException(message);
        }
    }
}
