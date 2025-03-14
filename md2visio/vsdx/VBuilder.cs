using md2visio.main;
using md2visio.struc.figure;
using Microsoft.Win32;
using Visio = Microsoft.Office.Interop.Visio;

namespace md2visio.vsdx
{
    internal abstract class VBuilder
    {
        protected Figure figure;

        public static Visio.Application VisioApp = new Visio.Application();
        protected Visio.Document visioDoc;
        protected Visio.Page visioPage;

        public VBuilder(Figure figure) {
            this.figure = figure;            

            VisioApp.Visible = AppConfig.Instance.Visible;
            visioDoc = VisioApp.Documents.Add(""); // 添加一个新文档
            visioPage = visioDoc.Pages[1]; // 获取活动页面
        }

        public abstract void Build(string outputFile); 

        public void SaveAndClose(string outputFile)
        {
            visioPage.ResizeToFitContents();

            AppConfig config = AppConfig.Instance;
            bool overwrite = true;
            if (!config.Quiet && File.Exists(outputFile))
            {
                Console.WriteLine($"Output file '{outputFile}' exists, input Y to overwrite: ");
                overwrite = (Console.ReadLine()?.ToLower() == "y");
            }

            if (overwrite) visioDoc.SaveAsEx(outputFile, 0);
            else visioDoc.Saved = true;

            visioDoc.Close();
        }

#pragma warning disable CA1416, CS8604
        public static string? GetVisioContentDirectory()
        {
            // 尝试的Office版本范围，从最新版到更早的版本
            int[] officeVersions = Enumerable.Range(11, 16).ToArray(); // 支持从Office 2003 (11.0) 到 Office 2019/2021 (16.0)

            foreach (int version in officeVersions)
            {
                string subKey = $@"Software\Microsoft\Office\{version}.0\Visio\InstallRoot";
                using (RegistryKey? key = Registry.LocalMachine.OpenSubKey(subKey))
                {
                    object? value = key?.GetValue("Path");
                    if (value != null)
                    {
                        string contentDir = Path.Combine(value.ToString(), "Visio Content");
                        if (Directory.Exists(contentDir))
                        {
                            return contentDir;
                        }
                    }                    
                }
            }

            return null;
        }
    }
}
