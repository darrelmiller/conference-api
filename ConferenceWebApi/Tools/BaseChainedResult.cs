using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace ConferenceWebApi.Tools
{
    public abstract class BaseChainedResult : IHttpActionResult
    {
        private readonly IHttpActionResult _NextActionResult;

        protected BaseChainedResult(IHttpActionResult actionResult)
        {
            _NextActionResult = actionResult;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            if (_NextActionResult == null)
            {
                var response = new HttpResponseMessage();
                ApplyAction(response);
                return Task.FromResult(response);
            }
            else
            {

                return _NextActionResult.ExecuteAsync(cancellationToken)
                    .ContinueWith(t =>
                    {
                        var response = t.Result;
                        ApplyAction(response);
                        return response;
                    }, cancellationToken);
            }
        }

        public abstract void ApplyAction(HttpResponseMessage response);
    
    }
}