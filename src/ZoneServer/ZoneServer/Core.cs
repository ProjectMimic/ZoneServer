using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZoneServer
{
    public class Core
    {
        List<Zone> Zones = new List<Zone>();

        static readonly int UPDATE_TIME = 400; // Amount of time for each update in milliseconds

        ContentLoader ContentLoader = null;

        public void Initialize()
        {
            ContentLoader = new ContentLoader();
            ContentLoader.Load();
            // TODO: Determine which zones should be loaded
            // TODO: Load content from scripts
            // TODO: Register systems
        }

        public void Run()
        {
            Stopwatch watch = Stopwatch.StartNew();
            long timeSinceLastUpdate = 0;
            while (true)
            {
                timeSinceLastUpdate = watch.ElapsedMilliseconds;
                watch.Restart();

                Task[] tasks = new Task[Zones.Count];
                for (int i = 0; i < Zones.Count; ++i)
                {
                    Zone zone = Zones[i];
                    tasks[i] = Task.Factory.StartNew(zone.Update);
                }
                Task.WaitAll(tasks);

                // Perform any interzone related logic here
                ContentLoader.Update();

                if (watch.ElapsedMilliseconds < UPDATE_TIME)
                {
                    Thread.Sleep(UPDATE_TIME - (int)watch.ElapsedMilliseconds);
                }
            }
        }
    }
}
