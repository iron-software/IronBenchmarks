using System;
using System.Collections.Generic;

namespace Benchmarks.Runner
{
    public abstract class GenericPlayList
    {
        public abstract Dictionary<string, Dictionary<string, TimeSpan>> RunPlayList(string resultFolder);
    }
}