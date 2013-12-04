using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


namespace ConferenceWebApi.Tools
{
    public class DynamicHalContent : HttpContent
    {
          private readonly MemoryStream _memoryStream = new MemoryStream();
          public DynamicHalContent(dynamic resource)
        {
            var serializerSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Formatting = Newtonsoft.Json.Formatting.Indented,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
          
            Headers.ContentType = new MediaTypeHeaderValue("application/hal+json");

            using (var writer = new JsonTextWriter(new StreamWriter(_memoryStream)){CloseOutput = false})
            {
              
                var serializer = JsonSerializer.Create(serializerSettings);
                serializer.Serialize(writer, resource);
                writer.Flush();
            }
            _memoryStream.Position = 0;
        }


        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            return _memoryStream.CopyToAsync(stream);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = _memoryStream.Length;
            return true;
        }
    }
}
