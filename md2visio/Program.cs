// See https://aka.ms/new-console-template for more information

using md2visio.graph;
using md2visio.mermaid;

Console.WriteLine($"args: {args}");

string inputFile = @"C:\Users\Megre\Documents\GitHub\md2visio\md2visio\test\对应位置的载荷.md";
string outputFile = @"C:\Users\Megre\Desktop\1.vsdx";

//new FigureBuilder(input).Build(output);

SynContext synContext = new SynContext(File.ReadAllLines(inputFile));
//new SynStateMachine(synContext).Start();
SttMermaidStart.Run(synContext);
Console.Write(synContext.ToString());

new GBuilder(synContext).Build(outputFile);