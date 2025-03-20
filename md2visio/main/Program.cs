// See https://aka.ms/new-console-template for more information

using md2visio.main;
using md2visio.mermaid.cmn;
using md2visio.struc.figure;

AppConfig config = AppConfig.Instance;
if (!config.ParseArgs(args)) return;
if (config.Test) { new AppTest().Test(); return; }

try
{
    SynContext synContext = new(config.InputFile);
    SttMermaidStart.Run(synContext);
    if(config.Debug) Console.Write(synContext.ToString());

    FigureBuilderFactory factory = new(synContext.NewSttIterator());
    factory.Build(config.OutputPath);
    factory.Quit();
}
catch(Exception ex)
{
    if (config.Debug) throw;
    else Console.Error.WriteLine(ex.Message);
}
