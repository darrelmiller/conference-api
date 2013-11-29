using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ndc.Controllers
{
    public class DocsController : ApiController
    {
        [Route("docs/{resourceclass}", Name = Links.DocsByResourceClass)]
        public HttpResponseMessage Get(string resourceclass)
        {
            var doc = ""; 
            switch (resourceclass)
            {
                case "event":
                    doc = "This is an event";
                    break;
                case "events":
                    doc = "This is a list of event";
                    break;
            }

            
            return new HttpResponseMessage(){ Content =new StringContent(doc)};
        }
    }
}
