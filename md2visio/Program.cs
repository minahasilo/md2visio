// See https://aka.ms/new-console-template for more information

using md2visio.figure;
using md2visio.mermaid.cmn;

Console.WriteLine($"args: {args}");

//string inputFile = @"C:\Users\Megre\Documents\GitHub\md2visio\md2visio\test\对应位置的载荷.md";
string inputFile = @"C:\Users\Megre\Documents\GitHub\md2visio\md2visio\test\journey.md";
string outputFile = @"C:\Users\Megre\Desktop\";

SynContext synContext = new SynContext(inputFile);
//new SynStateMachine(synContext).Start();
SttMermaidStart.Run(synContext);
Console.Write(synContext.ToString());

new FigureBuilderFactory(synContext.NewSttIterator()).Build(outputFile);