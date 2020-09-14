using HtmlAgilityPack;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsApp.BusinessLogic.Services
{
    public class NewsFromFeedCreator


    {

        public List<NewsDTO> CreateDtoNewsFromFeed()
        {
            ConcurrentBag<SyndicationItem> newsFromFeed = new ConcurrentBag<SyndicationItem>();

            try
            {


                Parallel.ForEach(
                    new List<string>{
                    "https://www.onliner.by/feed",
                    "http://s13.ru/rss",
                    "https://news.tut.by/rss/all.rss"
                    },
                    url =>
                    {
                        IEnumerable<SyndicationItem> newsSet = new RSSReader().GetNewsFromRSSFeed(url);




                        Parallel.ForEach(newsSet, s => newsFromFeed.Add(s));
                    });

            }
            catch
            {
                throw;
            }




            //HtmlDocument html = new HtmlDocument();
            //html.LoadHtml(feed.Summary.Text);
            //html.DocumentNode.InnerText

            var newsDTO = newsFromFeed.Select
                (feed =>

                new NewsDTO()
                {
                    Id = Guid.NewGuid(),
                    Title = feed.Title.Text,
                    Body = feed.Summary.Text,
                    SourseURL = feed.Id,
                    CreatedOnDate = feed.PublishDate.DateTime,
                    EditedOnDate = DateTime.Now,
                }
            ).ToList();

            foreach (var n in newsDTO)
            {
                HtmlDocument html = new HtmlDocument();
                html.LoadHtml(n.Body);
                n.Body = html.DocumentNode.InnerText;
            };





            return newsDTO;

            //foreach (var n in newsDTO)
            //{
            //    await _newsService.AddAsync(n);
            //}


            //var doc = new HtmlDocument();


            //var html = newsSet.FirstOrDefault().Summary.Text;
            //doc.LoadHtml(html);
            //var node = doc.DocumentNode.InnerText;


        }
    }
}
