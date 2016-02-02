using ConferenceWebApi.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace ConferenceWebApi.Controllers
{
    public class OpenApiController : ApiController
    {
        [Route("OpenApi")]
        public IHttpActionResult Get()
        {
            var stream = typeof(WebApiConfig).Assembly.GetManifestResourceStream(typeof(WebApiConfig), "OpenApi.json");

            var streamContent = new StreamContent(stream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return new OkResult(Request)
                .WithCaching(new CacheControlHeaderValue() { MaxAge = new TimeSpan(0, 0, 60) })
                .WithContent(streamContent);
        }
    }
}
