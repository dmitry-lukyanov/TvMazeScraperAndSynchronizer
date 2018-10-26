using System.Threading.Tasks;
using TvMazeApi.Proxy.Utils.Interfaces;

namespace TvMazeApi.Proxy.Utils
{
    public class ThreadUtil : IThreadUtil
    {
        public async Task DelayAsync(int delay)
        {
            await Task.Delay(delay);
        }
    }
}
