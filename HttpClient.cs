using System.Net.Http;

namespace Wensus
{
    public class HttpClient : System.Net.Http.HttpClient
    {
        public HttpClient()
            : this(new HttpPerformanceHandler(new HttpClientHandler()))
        {
        }

        public HttpClient(HttpMessageHandler handler)
            : base(new HttpPerformanceHandler(handler))
        {
        }

        public HttpClient(HttpMessageHandler handler, bool disposeHandler)
            : base(new HttpPerformanceHandler(handler), disposeHandler)
        {
        }
    }
}