using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Results;
using ConferenceWebApi.Tools;
using SharpYaml.Serialization;
using System.IO;
using System.Text;
using SharpYaml;

namespace ConferenceWebApi.Controllers
{

    [Route("")]
    public class HomeController : ApiController
    {
        
        public IHttpActionResult Get(string format = "yaml")
        {
            var stream = this.GetType().Assembly.GetManifestResourceStream("ConferenceWebApi.OpenApi2.yaml");
            HttpContent responseContent;

            if (format=="json" || this.Request.Headers.Accept.Contains(new MediaTypeWithQualityHeaderValue("application/json")))
            {
                var serializer = new Serializer(new SerializerSettings() { EmitJsonComptible = true });
                var yamlObject = serializer.Deserialize(stream);

                var buffer = new StringBuilder();
                var jserializer = new Newtonsoft.Json.JsonSerializer();
                jserializer.Serialize(new StringWriter(buffer), yamlObject);
                responseContent = new StringContent(buffer.ToString());
                responseContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            } else
            {
                responseContent = new StreamContent(stream);
                responseContent.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
            }

            
            return new OkResult(Request)
                .WithContent(responseContent);
        }
    }


  }
