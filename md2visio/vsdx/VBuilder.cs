using md2visio.figure;
using Microsoft.Win32;
using Visio = Microsoft.Office.Interop.Visio;

namespace md2visio.vsdx
{
    internal class VBuilder
    {
        protected Figure figure;

        protected Visio.Application visioApp = new Visio.Application();
        protected Visio.Document visioDoc;
        protected Visio.Page visioPage;

        public VBuilder(Figure figure) {
            this.figure = figure;            

            visioDoc = visioApp.Documents.Add(""); // 添加一个新文档
            visioPage = visioDoc.Pages[1]; // 获取活动页面
        }

        public virtual void Build(string outputFile)
        {                      
            
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
                        string contentDir = System.IO.Path.Combine(value.ToString(), "Visio Content");
                        if (System.IO.Directory.Exists(contentDir))
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
