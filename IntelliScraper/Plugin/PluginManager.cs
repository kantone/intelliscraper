using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace IntelliScraper.Plugin
{
    //http://code.msdn.microsoft.com/windowsdesktop/Creating-a-simple-plugin-b6174b62
    public class PluginManager
    {
        public string pluginDirectory{get;set;}
        private string[] dllFileNames{get;set;}
        private ICollection<Assembly> assemblies { get; set; }
        public List<KeyValuePair<string, Scrape.IScrapeAction>> plugins { get; set; }
        public PluginManager()
        {
            string pluginDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            this.pluginDirectory = pluginDirectory + "\\plugins";
            if (!Directory.Exists(this.pluginDirectory))
               Directory.CreateDirectory(this.pluginDirectory);

            this.Load();
        }

        private void Load()
        {               
            //Get files
            this.dllFileNames = Directory.GetFiles(this.pluginDirectory, "*.dll");

            //Load external assemblies
            this.assemblies = new List<Assembly>(dllFileNames.Length); 
            foreach (string dllFile in dllFileNames)
            {
                AssemblyName an = AssemblyName.GetAssemblyName(dllFile);
                Assembly assembly = Assembly.Load(an);
                assemblies.Add(assembly); 
            }

            //Find valid Plugins
            Type pluginType = typeof(Scrape.IScrapeAction);
            ICollection<Type> pluginTypes = new List<Type>();
            foreach (Assembly assembly in assemblies)
            {
                if (assembly != null)
                {
                    Type[] types = assembly.GetTypes();
                    foreach (Type type in types)
                    {
                        if (type.IsInterface || type.IsAbstract)
                        {
                            continue;
                        }
                        else
                        {
                            if (type.GetInterface(pluginType.FullName) != null)
                            {
                                pluginTypes.Add(type);
                            }
                        }
                    }
                }
            }

            //Load internal Plugins
            var _type = typeof(Scrape.IScrapeAction);
            var _types = AppDomain.CurrentDomain.GetAssemblies().ToList()
                .SelectMany(s => s.GetTypes())
                .Where(p => _type.IsAssignableFrom(p));
            foreach (var t in _types)
            {
                if(!t.IsInterface)
                    pluginTypes.Add(t);
            }

            //create instances from our found types using Reflections.
            this.plugins = new List<KeyValuePair<string, Scrape.IScrapeAction>>();
            foreach (Type type in pluginTypes)
            {
                if (!type.Namespace.Contains("IntelliScraper.Scrape.Action"))
                {
                    Scrape.IScrapeAction plugin = (Scrape.IScrapeAction)Activator.CreateInstance(type);
                    KeyValuePair<string, Scrape.IScrapeAction> pluginInfo = new KeyValuePair<string, Scrape.IScrapeAction>(plugin.getName().ToLower(), plugin);
                    plugins.Add(pluginInfo);
                }
            }

          
        }

        /// <summary>
        /// get Plugin By Name
        /// </summary>
        public Scrape.IScrapeAction getPluginByName(string name)
        {
            name = name.ToLower();
            foreach (KeyValuePair<string, Scrape.IScrapeAction> p in plugins)
            {
                if (p.Key == name)
                {
                    return p.Value;
                }
            }
            return null;
        }

        //Run Plugin
        public void runPlugin(string name, object inputData)
        {
            Scrape.IScrapeAction p = getPluginByName(name);
            p.Run(inputData);
        }
    }
}
