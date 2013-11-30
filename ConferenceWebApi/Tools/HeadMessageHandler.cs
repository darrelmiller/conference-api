using System.Net.Http;
using System.Threading.Tasks;

namespace ConferenceWebApi.Tools
{
    public class HeadMessageHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            bool isHead = request.Method == HttpMethod.Head;
            if (isHead)
            {
                request.Method = HttpMethod.Get;
            }

            var response = await  base.SendAsync(request, cancellationToken);

            if (isHead)
            {
                request.Method = HttpMethod.Head;
                response.Content = null;
            }
            return response;
        }
    }
}