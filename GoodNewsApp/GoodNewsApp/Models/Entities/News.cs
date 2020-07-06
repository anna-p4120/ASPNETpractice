using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsApp.Models.Entities
{
    public class News
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }
        public string SourseURL { get; set; }

        public DateTime PublicationDate { get; set; }

        public IEnumerable<Comment> Comment { get; set; }




    }
}
