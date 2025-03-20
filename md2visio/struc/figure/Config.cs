using md2visio.mermaid.cmn;

namespace md2visio.struc.figure
{
    internal class Config(MmdFrontMatter frontMatter, MmdJsonObj directive)
    {
        MmdFrontMatter frontMatter = frontMatter;
        MmdJsonObj directive = directive;

        public MmdFrontMatter FrontMatter { get => frontMatter; set => frontMatter = value; }
        public MmdJsonObj Directive { get => directive; set => directive = value; }

        public MmdJsonObj LoadDirective(string directive)
        {
            try
            {
                return Directive.Load(directive);
            }
            catch (ArgumentException)
            {
                return Empty.Get<MmdJsonObj>();
            }
        }

        public MmdJsonObj LoadDirectiveFromComment(string comment)
        {
            if (!comment.TrimEnd(' ').EndsWith("%%")) return Empty.Get<MmdJsonObj>();

            char[] trims = ['%', '\t', '\n', '\r', '\f', ' '];
            string setting = comment.TrimStart(trims).TrimEnd(trims);
            return LoadDirective(setting);
        }
        public MmdFrontMatter LoadFrontMatter(string frontMatter)
        {
            return FrontMatter.Load(frontMatter);
        }

        public (bool success, double d) GetDouble(string keyPath)
        {
            (bool success, double d) = directive.GetDouble(keyPath);
            if (success) return (success, d);
            return frontMatter.GetDouble(keyPath);
        }
        public (bool success, int i) GetInt(string keyPath)
        {
            (bool success, int i) = directive.GetInt(keyPath);
            if (success) return (success, i);
            return frontMatter.GetInt(keyPath);
        }

        public string? GetString(string keyPath)
        {
            string? s = directive.GetString(keyPath);
            if (s != null) return s;
            return frontMatter.GetString(keyPath);
        }
    }
}
