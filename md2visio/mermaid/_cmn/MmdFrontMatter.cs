using md2visio.struc.figure;
using System.Net.Http.Headers;
using YamlDotNet.Serialization;

namespace md2visio.mermaid._cmn
{
    internal class MmdFrontMatter : ValueGetter, IEmpty
    {
        public static readonly MmdFrontMatter Empty = new MmdFrontMatter();

        object? yamlObj;

        public MmdFrontMatter() { }

        public MmdFrontMatter(string yaml)
        {
            yamlObj = new DeserializerBuilder().Build().Deserialize(yaml);
        }

        public MmdFrontMatter(object? yamlObj) { this.yamlObj = yamlObj; }

        public override T? GetValue<T>(string keyPath) where T : class
        {
            if (!keyPath.Contains("."))
            {
                if (this[keyPath] is T) return this[keyPath] as T;
            }
            else
            {
                string[] items = keyPath.Split('.');
                int count = items.Length;
                MmdFrontMatter fm = this;
                object? result = yamlObj;
                foreach (string key in items)
                {
                    --count;
                    result = fm[key];
                    if (result == null) break;

                    fm = new MmdFrontMatter(result);
                }
                if (count == 0 && result is T) return result as T;
            }
            return null;
        }

        public bool IsEmpty()
        {
            return this == Empty;
        }

        public object? this[string key]
        {
            get
            {
                if (yamlObj is IList<object>)
                {
                    int index = 0;
                    if (!int.TryParse(key, out index)) return null;
                    return (yamlObj as IList<object>)?[index];
                }
                else if (yamlObj is IDictionary<object, object>)
                {
                    IDictionary<object, object>? dict = yamlObj as IDictionary<object, object>;
                    if (dict != null && dict.ContainsKey(key)) return dict[key];
                }
                return null;
            }
        }

        public object? this[int index]
        {
            get
            {
                if (yamlObj is IList<object>)
                {
                    IList<object>? list = yamlObj as IList<object>;
                    if (list != null && index >= 0 && index < list.Count)
                        return list[index];
                }
                return null;
            }
        }
    }
}
