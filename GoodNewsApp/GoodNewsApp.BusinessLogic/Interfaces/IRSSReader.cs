using System.Collections.Generic;
using System.ServiceModel.Syndication;

namespace GoodNewsApp.BusinessLogic.Services
{
    public interface IRSSReader
    {
        IEnumerable<SyndicationItem> GetNewsFromRSSFeed(string feedURL);
        
    }
}