using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WebApiContrib.CollectionJson;

namespace ConferenceWebApi.Tools
{
    public class CollectionJsonContent : HttpContent
    {
        private readonly MemoryStream _memoryStream = new MemoryStream();
        public CollectionJsonContent(Collection collection)
        {
            var serializerSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Formatting = Newtonsoft.Json.Formatting.Indented,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
            collection.Version = "1.0";

            Headers.ContentType = new MediaTypeHeaderValue("application/vnd.collection+json");

            using (var writer = new JsonTextWriter(new StreamWriter(_memoryStream)){CloseOutput = false})
            {
                var readDocument = new ReadDocument {Collection = collection};
                var serializer = JsonSerializer.Create(serializerSettings);
                serializer.Serialize(writer,readDocument);
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