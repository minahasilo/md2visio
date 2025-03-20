using md2visio.mermaid.cmn;

namespace md2visio.struc.figure
{
    internal abstract class Figure : Container
    {
        string title = string.Empty;
        Config config = new Config(new MmdFrontMatter(), new MmdJsonObj());

        public string Title
        {
            get => title;
            set => title = value.Trim();
        }
        public MmdFrontMatter FrontMatter { get => config.FrontMatter; }
        public MmdJsonObj Directive { get => config.Directive; }
        public Config Config { get => config; }

        public Figure()
        {
        }        

        public abstract void ToVisio(string path);
    }

    class EmptyFigure : Figure
    {
        public static EmptyFigure Instance { get; } = Empty.Get<EmptyFigure>();
        public EmptyFigure() { }

        public override void ToVisio(string path)
        {
            throw new NotImplementedException();
        }
    }
}
