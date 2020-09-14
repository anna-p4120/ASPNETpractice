using Hangfire;
using Hangfire.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GoodNewsApp.BusinessLogic.Services
{
    public class NewsFromFeedJob


    {
        CancellationToken token;
        //https://www.onliner.by/feed
        //https://news.tut.by/rss/all.rss
        public void CreateAndStoreNewsFromFeed()
        {
            //RecurringJob.AddOrUpdate(() => CreateDtoNewsFromFeed(), Cron.Daily());

            List<NewsDTO> newsDTO = new NewsFromFeedCreator().CreateDtoNewsFromFeed();

            BackgroundJob.Enqueue<NewsRangeLoader>(x => x.
            UpdateRangeByUrlAsync(newsDTO, token));
            //foreach (var n in newsDTO)
            //{
            //    await _newsService.AddAsync(n);
            //}

        }
    }
}
