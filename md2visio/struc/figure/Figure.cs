using md2visio.mermaid._cmn;

namespace md2visio.struc.figure
{
    internal abstract class Figure : Container, IEmpty
    {
        public static readonly Figure Empty = new EmptyFigure();
        public MmdFrontMatter FrontMatter { get; set; } = MmdFrontMatter.Empty;
        public MmdJsonObj Directive { get; set; } = MmdJsonObj.Empty;

        public Figure()
        {
        }

        public bool IsEmpty()
        {
            return this == Empty;
        }

        public MmdJsonObj InitDirective(string setting)
        {                 
            try
            {                
                Directive = new MmdJsonObj(setting);
                return Directive;
            }
            catch (ArgumentException)
            {
                return MmdJsonObj.Empty;
            }
        }

        public MmdJsonObj InitDirectiveFromComment(string comment)
        {
            if (!comment.TrimEnd(' ').EndsWith("%%")) return MmdJsonObj.Empty;

            char[] trims = new char[] { '%', '\t', '\n', '\r', '\f', ' ' };
            string setting = comment.TrimStart(trims).TrimEnd(trims);
            return InitDirective(setting);
        }

        public MmdFrontMatter InitFrontMatter(string frontMatter)
        {
            return FrontMatter = new MmdFrontMatter(frontMatter);
        }

        public abstract void ToVisio(string path);
    }

    class EmptyFigure : Figure
    {
        public EmptyFigure() { }

        public override void ToVisio(string path)
        {
            throw new NotImplementedException();
        }
    }
}
