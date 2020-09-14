using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GoodNewsApp.BusinessLogic.Services
{
    //"http://s13.ru/rss"
    public class RSSReader: IRSSReader
    {
        public IEnumerable<SyndicationItem> GetNewsFromRSSFeed(string feedURL)
        {
            List<SyndicationItem> newsFromFeed = new List<SyndicationItem>();

            try
            {
                using (XmlReader reader = XmlReader.Create(feedURL)) //,new XmlReaderSettings() { Async = true }
                {
                    SyndicationFeed feed = SyndicationFeed.Load(reader);
                    foreach(var item in feed.Items)
                    {
                        item.SourceFeed = feed;
                    }

                    newsFromFeed.AddRange(feed.Items);
                }
                return newsFromFeed;
            }
            catch
            {
                throw;
            }
        }
    }
}
