namespace md2visio.mermaid._cmn
{
    internal abstract class ValueGetter
    {
        abstract public T? GetValue<T>(string keyPath) where T : class;

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
    }
}
