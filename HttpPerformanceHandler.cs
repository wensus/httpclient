using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Wensus
{
    public class HttpPerformanceHandler : DelegatingHandler
    {
        private const string webErrorAction = "weberror";
        private const string webPerformanceAction = "webperf";
        private readonly Stopwatch timer;

        public HttpPerformanceHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
            timer = new Stopwatch();
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            timer.Start();

            var requestUrl = request.RequestUri;

            var response = await base.SendAsync(request, cancellationToken);

            timer.Stop();

            if (!response.IsSuccessStatusCode)
            {
                var errorMsg = string.Format("{0}||{1}", requestUrl,
                    ((int)response.StatusCode).ToString());

                Client.Mark(webErrorAction, errorMsg);

                return response;
            }

            var data = await response.Content.ReadAsByteArrayAsync();
            var bytesReceived = data.Length;

            var elapsedTime = timer.ElapsedMilliseconds;
            var dataTransferred = bytesReceived;

            var msg = string.Format("{0}||{1}||{2}", requestUrl, elapsedTime, dataTransferred);

            Client.Mark(webPerformanceAction, msg);

            return response;
        }
    }

}
