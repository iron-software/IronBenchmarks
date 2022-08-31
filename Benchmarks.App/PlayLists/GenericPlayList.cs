using Benchmarks.Configuration;

namespace Benchmarks.App.PlayLists
{
    internal abstract class GenericPlayList
    {
        public abstract Dictionary<string, Dictionary<string, TimeSpan>> RunPlayList(IAppConfig appConfig);
    }
}
