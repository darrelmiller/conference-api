using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using Tavis;
using Tavis.IANA;

namespace ConferenceWebApi.Tools
{
    public class DescribedByHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            var response = await  base.SendAsync(request, cancellationToken);

            
            var routeData = request.GetRouteData();

            if (routeData == null && routeData.Route.DataTokens != null && routeData.Route.DataTokens.ContainsKey("actions"))
            {
                var actions = (HttpActionDescriptor[]) routeData.Route.DataTokens["actions"];
                if (actions.Length > 0)
                {
                    var controller = actions[0].ControllerDescriptor.ControllerName;

                    response.Headers.AddLinkHeader(
                        request.ResolveLink<DescribedByLink>(Links.DocsByResourceClass, new { resourceclass = controller }));
                }
            }
            return response;
        }
    }
}