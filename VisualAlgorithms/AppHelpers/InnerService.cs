using System.Net.Http;

namespace VisualAlgorithms.AppHelpers
{
    public class InnerService
    {
        public InnerService(HttpClient client)
        {
            Client = client;
        }

        public HttpClient Client { get; set; }
    }
}
