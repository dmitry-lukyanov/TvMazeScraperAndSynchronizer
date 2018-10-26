using Newtonsoft.Json;

namespace TvMazeScraper.Utils.ErrorHandling
{
    public class ErrorMessage
    {
        public int HttpCode { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
