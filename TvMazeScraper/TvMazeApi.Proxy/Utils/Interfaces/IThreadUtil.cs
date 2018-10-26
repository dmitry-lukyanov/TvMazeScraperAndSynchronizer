using System.Threading.Tasks;

namespace TvMazeApi.Proxy.Utils.Interfaces
{
    public interface IThreadUtil
    {
        Task DelayAsync(int delay);
    }
}
