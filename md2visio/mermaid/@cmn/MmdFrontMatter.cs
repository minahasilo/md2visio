using System.Text;
using YamlDotNet.Serialization;

namespace md2visio.mermaid.cmn
{
    internal class MmdFrontMatter : ValueAccessor
    {
        object? yamlObj;

        public MmdFrontMatter() { }

        public MmdFrontMatter(string yaml)
        {
            Load(yaml);
        }

        public MmdFrontMatter(object? yamlObj) 
        { 
            this.yamlObj = yamlObj; 
        }

        public MmdFrontMatter Load(string yaml)
        {
            yamlObj = new DeserializerBuilder().Build().Deserialize(yaml);
            return this;
        }

        public override T? GetValue<T>(string keyPath) where T : class
        {
            string[] items = keyPath.Split('.');
            int count = items.Length;
            MmdFrontMatter fm = this;
            object? result = yamlObj;
            foreach (string key in items)
            {
                --count;
                result = fm.GetItem(key);
                if (result == null) break;

                fm = new MmdFrontMatter(result);
            }
            if (count == 0 && result is T) return result as T;
            
            return null;
        }

        public override void SetValue(string keyPath, object value)
        {
            string[] keyList = keyPath.Split('.');
            MmdFrontMatter fm = this;
            for(int i=0; i<keyList.Length; ++i)
            {
                string key = keyList[i];
                if(i == keyList.Length - 1) fm[key] = value;
                else
                {
                    object? result = fm.GetItem(key);
                    if (result == null)
                    {
                        MmdFrontMatter newFM = new("{}");
                        if (newFM.yamlObj == null) break;

                        fm.SetItem(key, newFM.yamlObj);
                        fm = newFM;
                    }
                    else fm = new MmdFrontMatter(result);
                }                
            }
        }

        object? GetItem(string key)
        {
            if (yamlObj is IList<object>)
            {
                if (!int.TryParse(key, out int index)) return null;

                return GetItem(index);
            }
            else if (yamlObj is IDictionary<object, object>)
            {
                if (yamlObj is IDictionary<object, object> dict && dict.ContainsKey(key)) return dict[key];
            }
            return null;
        }

        public object? this[string key]
        {
            get => GetItem(key);
            set => SetItem(key, value);
        }

        public object? this[int index]
        {
            get => GetItem(index);
            set => SetItem(index, value);
        }

        void SetItem(string key, object? obj)
        {
            if (yamlObj is IList<object>)
            {
                if (int.TryParse(key, out int index))
                {
                    SetItem(index, obj);
                    return;
                }
            }
            else if(yamlObj is IDictionary<object, object>)
            {
                if (yamlObj is IDictionary<object, object> dict) dict[key] = obj ?? string.Empty;
            }
        }

        object? GetItem(int index)
        {
            if (yamlObj is not IList<object> list) return null;
            if (index < 0 || index >= list.Count) return null;
                
            return list[index]; 
        }

        void SetItem(int index, object? obj)
        {
            if (yamlObj is not IList<object> arr) return;

            int num2add = index - arr.Count + 1;
            for (int i = 0; i < num2add; i++)
                arr.Insert(arr.Count, string.Empty);
            
            arr[index] = obj ?? string.Empty;
        }

        public MmdFrontMatter Join(MmdJsonObj directive)
        {
            MmdJsonObj? init = directive.GetValue<MmdJsonObj>("init");
            if (init == null) return this;

            MmdJsonObj config = new("config: {}");
            config.SetValue("config", init);

            return Join(config, new StringBuilder());
        }

        MmdFrontMatter Join(MmdJsonObj json, StringBuilder path)
        {
            if (json == null) return this;

            foreach (string key in json.Data.Keys)
            {
                AppendKey(path, key);

                object? val = json[key];
                if (val is MmdJsonObj) Join((MmdJsonObj)val, new StringBuilder(path.ToString()));
                else SetValue(path.ToString(), val ?? string.Empty);

                UnappendKey(path);
            }

            return this;
        }

        public MmdFrontMatter Join(MmdFrontMatter another)
        {
            if (another.yamlObj == null) return this;

            return Join(another.yamlObj, new StringBuilder());
        }

        MmdFrontMatter Join(object yaml, StringBuilder path)
        {
            if (yaml is not IDictionary<object, object> dict) return this;

            foreach (object key in dict.Keys)
            {
                AppendKey(path, key.ToString() ?? string.Empty);
                if (dict == null) break;

                object? val = dict[key];
                if (val is IDictionary<object, object>) Join(val, new StringBuilder(path.ToString()));
                else SetValue(path.ToString(), val);      
                
                UnappendKey(path);
            }

            return this;
        }

        public override string ToString()
        {
            if(yamlObj == null) return string.Empty;

            return ToString(yamlObj);
        }

        string ToString(object obj)
        {
            if (obj is IList<object>)
            {
                StringBuilder sb = new("[");
                IList<object> list = (IList<object>)obj;
                foreach(object item in list)
                {
                    sb.Append(ToString(item))
                        .Append(item == list.Last() ? string.Empty : ", ");
                }
                return $"{sb}]";
            }
            else if(obj is IDictionary<object, object>)
            {
                StringBuilder sb = new("{");
                IDictionary<object, object> dict = (IDictionary<object, object>)obj;
                List<object> keys = [.. dict.Keys];
                foreach (object key in keys)
                {                    
                    sb.Append(ToString(key)).Append(": ")
                        .Append(ToString(dict[key]))
                        .Append(key == keys.Last() ? string.Empty : ", ");
                }
                return $"{sb}}}";
            }

            return $"{Quote(obj)}{obj}{Quote(obj)}";
        }

        string Quote(object obj)
        {
            if (obj is IList<object> || obj is IDictionary<object, object>) return string.Empty;
            if (obj is string) return "'";
            return string.Empty;
        }

    }
}
