using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiContrib.CollectionJson;

namespace ndc.Tools
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
