using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using CollectionJson;


namespace ConferenceWebApi.Tools
{
    public class CollectionJsonContent : HttpContent
    {
        private readonly ReadDocument _readDocument;
        private readonly JsonSerializer _serializer;

        public CollectionJsonContent(Collection collection)
        {
           
            _serializer = JsonSerializer.Create(new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Formatting = Formatting.Indented,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

            collection.Version = "1.0";
            _readDocument = new ReadDocument(collection);
            
            Headers.ContentType = new MediaTypeHeaderValue("application/vnd.collection+json");
        }


        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            using (var writer = new JsonTextWriter(new StreamWriter(stream)) { CloseOutput = false })
            {
                _serializer.Serialize(writer, _readDocument);
                writer.Flush();
            }
            return Task.FromResult(0);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = -1;
            return false;
        }
    }

    [DataContract]
    public class ReadDocument : IReadDocument
    {
        public ReadDocument(Collection collection)
        {
            Collection = collection;
        }
        public ReadDocument()
        {
            Collection = new Collection();
        }

        [DataMember(Name = "collection")]
        public Collection Collection { get; private set; }
    }
}