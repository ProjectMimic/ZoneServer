using System;
using System.IO;

namespace ZoneServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Core core = new Core();
            core.Initialize();
            core.Run();
        }
    }
}
