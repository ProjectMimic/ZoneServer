using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Text;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;

namespace ZoneServer
{

    public class ContentLoader
    {
        V8ScriptEngine engine = null;
        FileSystemWatcher watcher = null;

        static readonly string ContentDirectory = Path.Combine(Directory.GetCurrentDirectory(), "content");
        private List<string> Directories = new List<string>();

        private List<string> ChangedFiles = new List<string>();

        public ContentLoader()
        {
            engine = new V8ScriptEngine(V8ScriptEngineFlags.EnableDebugging);
            engine.AddHostType(typeof(Job));
            engine.AddHostType(typeof(Ecosystem));
            engine.AddHostType(typeof(Archetypes));
            engine.AddHostType(typeof(ContentLoader));

            Directories.Add(ContentDirectory);

#if DEVELOPER
            watcher = new FileSystemWatcher(ContentDirectory);
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Filter = "*.js";
            watcher.IncludeSubdirectories = true;
            watcher.Changed += new FileSystemEventHandler(OneFileChanged);
            watcher.Created += new FileSystemEventHandler(OneFileChanged);
            watcher.Deleted += new FileSystemEventHandler(OneFileChanged);
            watcher.Renamed += new RenamedEventHandler(OnFileRenamed);
            watcher.EnableRaisingEvents = true;
#endif
        }

        ~ContentLoader()
        {
            engine.Dispose();
            if (watcher != null)
            {
                watcher.Dispose();
            }
        }

        public void Update()
        {
            foreach (string changedFile in ChangedFiles)
            {
                if (changedFile.EndsWith("archetypes.js"))
                {
                    // Reload all archetype files
                    foreach (string directory in Directories)
                    {
                        LoadFiles(directory, "archetypes.js");
                    }
                }
            }
            ChangedFiles.Clear();
        }

        public void Load()
        {
            LoadFiles(ContentDirectory, "archetypes.js");
            LoadFiles(Path.Combine(ContentDirectory, "effects"), "*.js");
            LoadFiles(Path.Combine(ContentDirectory, "quests"), "*.js");
            LoadFiles(Path.Combine(ContentDirectory, "spells"), "*.js");
            //jobs
            //items
            //equipment
            //actions
            //abilities
        }

        public void LoadZone(string zone)
        {
            string zoneDirectory = Path.Combine(ContentDirectory, "zones", zone);
            Directories.Add(zoneDirectory);
            LoadFiles(zoneDirectory, "archetypes.js");
            LoadFiles(zoneDirectory, "entities.js");
            LoadFiles(Path.Combine(zoneDirectory, "entities"), "*.js");
            LoadFiles(Path.Combine(zoneDirectory, "battlefields"), " *.js");
            //regions
            //helm
            //fishing
            //digging
        }

        public void LoadFiles(string directory, string searchPattern)
        {
            if (!Directory.Exists(directory))
                return;

            List<string> files = new List<string>();
            
            foreach (string subdirectory in Directory.EnumerateDirectories(directory, "*", SearchOption.AllDirectories))
            {
                foreach (string file in Directory.GetFiles(subdirectory, searchPattern, SearchOption.AllDirectories))
                {
                    files.Add(file);
                }
            }

            files.Sort();
            foreach (string file in files)
            {
                string contents = File.ReadAllText(file);
                engine.Execute(contents);
            }
        }

        private void OneFileChanged(object sender, FileSystemEventArgs e)
        {
            ChangedFiles.Add(e.FullPath);
        }

        private void OnFileRenamed(object sender, RenamedEventArgs e)
        {
            ChangedFiles.Add(e.FullPath);
        }
    }
}
