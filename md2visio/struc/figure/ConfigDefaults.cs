using md2visio.mermaid.cmn;

namespace md2visio.struc.figure
{
    internal class ConfigDefaults: IConfig
    {
        static readonly string defaultDir = "./default";
        static readonly string themeDir = $"{defaultDir}/theme";        
        static readonly Dictionary<string, MmdFrontMatter> defConfig = new(); // ./default/
        static readonly Dictionary<string, MmdFrontMatter> themeVars = new(); // ./default/theme/
        static readonly string commonCfg = "default";

        public Figure Figure { get; set; } = EmptyFigure.Instance;
        public string Theme { get; set; } = "default";
        public bool DarkMode { get; set; } = false;

        public ConfigDefaults() { 
            LoadCommonConfig();
        }

        public bool GetDouble(string keyPath, out double d)
        {
            if (LoadThemeVars(Theme, DarkMode).GetDouble(keyPath, out d))
                return true;
            if (LoadDefaultConfig(Figure).GetDouble(keyPath, out d))
                return true;
            return LoadCommonConfig().GetDouble(keyPath, out d);
        }
        public bool GetInt(string keyPath, out int i)
        {
            if (LoadThemeVars(Theme, DarkMode).GetInt(keyPath, out i))
                return true;

            return LoadCommonConfig().GetInt(keyPath, out i);
        }
        public bool GetString(string keyPath, out string s)
        {
            if (LoadThemeVars(Theme, DarkMode).GetString(keyPath, out s))
                return true;

            return LoadCommonConfig().GetString(keyPath, out s);
        }

        MmdFrontMatter LoadThemeVars(string theme2load, bool darkMode=false)
        {
            DarkMode = darkMode;
            Theme = theme2load.ToLower();
            Theme = string.Format("{0}{1}", Theme, Theme == "base" && darkMode ? "-darkMode" : "");
            if (themeVars.TryGetValue(Theme, out MmdFrontMatter? fm)) 
                return fm ?? Empty.Get<MmdFrontMatter>();
           
            fm = MmdFrontMatter.FromFile($"{themeDir}/{Theme}.yaml");
            themeVars.Add(Theme, fm);

            return fm;
        }

        MmdFrontMatter LoadDefaultConfig(Figure figure)
        {
            if (figure.IsEmpty()) return Empty.Get<MmdFrontMatter>();

            Figure = figure;
            string fName = TypeMap.ConfigMap[figure.GetType().Name];
            if (defConfig.TryGetValue(fName, out MmdFrontMatter? fm))
                return fm ?? Empty.Get<MmdFrontMatter>();

            fm = MmdFrontMatter.FromFile($"{defaultDir}/{fName}.yaml");
            defConfig.Add(fName, fm);

            return fm;
        }

        MmdFrontMatter LoadCommonConfig()
        {
            if (defConfig.TryGetValue(commonCfg, out MmdFrontMatter? fm))
                return fm;

            fm = MmdFrontMatter.FromFile($"{defaultDir}/{commonCfg}.yaml");
            defConfig.Add(commonCfg, fm);
            return fm;
        }

        
    }
}
