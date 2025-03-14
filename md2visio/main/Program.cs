// See https://aka.ms/new-console-template for more information

using md2visio.struc.figure;
using md2visio.mermaid._cmn;
using md2visio.main;

AppConfig config = AppConfig.Instance;
if (!config.ParseArgs(args)) return;

try
{
    //string inputFile = @"C:\Users\Megre\Documents\GitHub\md2visio\md2visio\test\对应位置的载荷.md";
    //string inputFile = @"C:\Users\Megre\Documents\GitHub\md2visio\md2visio\test\journey.md";
    //string outputPath = @"C:\Users\Megre\Desktop\";

    SynContext synContext = new SynContext(config.InputFile);
    //new SynStateMachine(synContext).Start();
    SttMermaidStart.Run(synContext);
    if(config.Debug) 
        Console.Write(synContext.ToString());

    new FigureBuilderFactory(synContext.NewSttIterator()).Build(config.OutputPath);
}
catch(Exception ex)
{
    if (config.Debug) throw;
    else Console.Error.WriteLine(ex.Message);
}
