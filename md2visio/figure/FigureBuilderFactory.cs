using md2visio.graph;
using md2visio.journey;
using md2visio.mermaid.cmn;
using System.Reflection;

namespace md2visio.figure
{
    internal class FigureBuilderFactory
    {
        string outputFile;

        Dictionary<string, Type> builderDict = new Dictionary<string, Type>()
        {
            { "graph", typeof(GBuilder) }, { "flowchart", typeof(GBuilder) },
            { "journey", typeof(JoBuilder) },
        };

        SttIterator iter;
        int count = 1;
        public FigureBuilderFactory(SttIterator iter) 
        {
            this.iter = iter;
            outputFile = iter.Context.InputFile;
        }

        public void Build()
        {
            while (iter.HasNext())
            {
                List<SynState> list = iter.Context.StateList;
                for (int pos = iter.Pos + 1; pos < list.Count; ++pos)
                {
                    string word = list[pos].Fragment;
                    if (SttFigureType.IsFigure(word)) DoBuild(word);
                }
            }
        }

        public void Build(string outputFile)
        {
            if(!File.Exists(outputFile) && !Directory.Exists(outputFile)) 
                throw new FileNotFoundException($"output path not exists: '{outputFile}'");

            this.outputFile = outputFile;            
            Build();
        }

        void DoBuild(string figure)
        {
            if(!builderDict.ContainsKey(figure)) throw new NotImplementedException($"{figure} build not implemented");

            Type type = builderDict[figure];
            object? obj = Activator.CreateInstance(type, iter);
            MethodInfo? method = type.GetMethod("Build", BindingFlags.Public | BindingFlags.Instance, null,
                new Type[] {typeof(string)}, null);

            method?.Invoke(obj, new object[] { Output() });
        }

        string Output()
        {
            string? name = string.Empty, dir = string.Empty;                
            if(File.Exists(outputFile)) // file
            {
                name =Path.GetFileNameWithoutExtension(outputFile);
                dir = Path.GetDirectoryName(outputFile);
            }
            else // directory
            {
                name = Path.GetFileNameWithoutExtension(iter.Context.InputFile);
                dir = outputFile;
            }

            return $"{dir}{name}{count++}.vsdx";
        }
    }
}
