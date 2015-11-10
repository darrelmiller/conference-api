using CollectionJson;

namespace ConferenceWebApi.Tools
{
    public static class LinkExtensions
    {
        public static Link ToCJLink(this Tavis.Link link)
        {
            var cjlink = new Link {Href = link.Target, Rel = link.Relation};
            return cjlink;
        } 
    }
}
