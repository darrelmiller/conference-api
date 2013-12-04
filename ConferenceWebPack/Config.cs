using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tavis;

namespace ConferenceWebPack
{
    public static class Config
    {
        public static void Register(LinkFactory linkFactory)
        {
            linkFactory.AddLinkType<HomeLink>();
            linkFactory.AddLinkType<DaysLink>();
            linkFactory.AddLinkType<SessionLink>();
            linkFactory.AddLinkType<SessionsLink>();
            linkFactory.AddLinkType<SpeakerLink>();
            linkFactory.AddLinkType<SpeakersLink>();
            linkFactory.AddLinkType<TopicLink>();
            linkFactory.AddLinkType<TopicsLink>();
            linkFactory.AddLinkType<ReviewLink>();
        }
    }
}
