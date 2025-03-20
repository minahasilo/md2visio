using md2visio.main;
using Microsoft.Win32;
using Visio = Microsoft.Office.Interop.Visio;

namespace md2visio.vsdx.@base
{
    internal abstract class VBuilder
    {
        public static Visio.Application VisioApp = new();
        protected Visio.Document visioDoc;
        protected Visio.Page visioPage;
        public VBuilder()
        {
            VisioApp.Visible = AppConfig.Instance.Visible;
            visioDoc = VisioApp.Documents.Add(""); // 添加一个新文档
            visioPage = visioDoc.Pages[1]; // 获取活动页面
        }

        public void SaveAndClose(string outputFile)
        {
            visioPage.ResizeToFitContents();

            AppConfig config = AppConfig.Instance;
            bool overwrite = true;
            if (!config.Quiet && File.Exists(outputFile))
            {
                Console.WriteLine($"Output file '{outputFile}' exists, input Y to overwrite: ");
                overwrite = Console.ReadLine()?.ToLower() == "y";
            }

            if (overwrite) visioDoc.SaveAsEx(outputFile, 0);
            else visioDoc.Saved = true;

            visioDoc.Close();
        }


        public static string? GetVisioContentDirectory()
        {
            // 支持从Office 2003 (11.0) 到 Office 2019/2021 (16.0)
            int[] officeVersions = Enumerable.Range(11, 16).ToArray();

            foreach (int version in officeVersions)
            {
                string subKey = $@"Software\Microsoft\Office\{version}.0\Visio\InstallRoot";
#pragma warning disable CA1416, CS8604
                using RegistryKey? key = Registry.LocalMachine.OpenSubKey(subKey);
                object? value = key?.GetValue("Path");
                if (value != null)
                {
                    string contentDir = Path.Combine(value.ToString(), "Visio Content");
#pragma warning restore CA1416, CS8604
                    if (Directory.Exists(contentDir))
                    {
                        return contentDir;
                    }
                }
            }

            return null;
        }
    }
}
