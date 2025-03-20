using System.Text;

namespace md2visio.mermaid.cmn
{
    internal abstract class ValueAccessor
    {
        abstract public T? GetValue<T>(string keyPath) where T : class;
        abstract public void SetValue(string key, object value);

        virtual public string? GetString(string keyPath)
        {
            return GetValue<string>(keyPath);
        }

        virtual public(bool success, int r) GetInt(string keyPath)
        {
            string? val = GetValue<string>(keyPath);
            int r = 0;
            bool success = int.TryParse(val, out r);
            return (success, r);
        }

        virtual public(bool success, double r) GetDouble(string keyPath)
        {
            string? val = GetValue<string>(keyPath);
            double r = 0;
            bool success = double.TryParse(val, out r);
            return (success, r);
        }

        protected void AppendKey(StringBuilder path, string key)
        {
            if (path.Length > 0) path.Append(".");
            path.Append(key);
        }

        protected void UnappendKey(StringBuilder path)
        {
            int dotIndex = path.Length - 1;
            for (; dotIndex >= 0; --dotIndex)
            {
                if (path[dotIndex] == '.') break;                
            }
            if(dotIndex >= 0) path.Remove(dotIndex, path.Length - dotIndex);
            else path.Clear();
        }
    }
}
