namespace TvMazeApi.Proxy.Utils.Interfaces
{
    public interface IProxySettingsProvider
    {
        string ApiUrl { get; }
        int MaxAttemptNumberForTooManyRequestsHttpError { get; }
        int DelayForTooManyRequestHttpError { get; }
    }
}
