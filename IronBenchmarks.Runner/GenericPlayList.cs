using System;
using System.Collections.Generic;

namespace IronBenchmarks.Core
{
    public abstract class GenericPlayList
    {
        public abstract Dictionary<string, Dictionary<string, TimeSpan>> RunPlayList(string resultFolder);
    }
}