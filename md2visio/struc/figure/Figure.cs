using md2visio.mermaid._cmn;

namespace md2visio.struc.figure
{
    internal abstract class Figure : Container, IEmpty
    {
        public static readonly Figure Empty = new EmptyFigure();
        public MmdJsonObj Setting { get; set; } = MmdJsonObj.Empty;
        public Figure()
        {
        }

        public bool IsEmpty()
        {
            return this == Empty;
        }

        public MmdJsonObj InitSetting(string setting)
        {                 
            try
            {                
                Setting = new MmdJsonObj(setting);
                return Setting;
            }
            catch (ArgumentException)
            {
                return MmdJsonObj.Empty;
            }
        }

        public MmdJsonObj InitSettingFromComment(string comment)
        {
            if (!comment.TrimEnd(' ').EndsWith("%%")) return MmdJsonObj.Empty;

            char[] trims = new char[] { '%', '\t', '\n', '\r', '\f', ' ' };
            string setting = comment.TrimStart(trims).TrimEnd(trims);
            return InitSetting(setting);
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
