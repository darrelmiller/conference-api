using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Results;
using ConferenceWebApi.ServerLinks;
using ConferenceWebApi.Tools;
using ConferenceWebPack;
using Tavis.Home;
using Tavis.IANA;

namespace ConferenceWebApi.Controllers
{

   
    [Route("")]
    public class HomeController : ApiController
    {
        
        public IHttpActionResult Get()
        {
            var home = new HomeDocument();

            home.AddResource(TopicsLinkHelper.CreateLink(Request).WithHints());
            home.AddResource(DaysLinkHelper.CreateLink(Request).WithHints());
            home.AddResource(SessionsLinkHelper.CreateLink(Request).WithHints());
            home.AddResource(SpeakersLinkHelper.CreateLink(Request).WithHints());


            return new OkResult(Request)
                .WithCaching(new CacheControlHeaderValue() { MaxAge = new TimeSpan(0, 0, 60) })
                .WithContent(new HomeContent(home));
        }
    }


  }
