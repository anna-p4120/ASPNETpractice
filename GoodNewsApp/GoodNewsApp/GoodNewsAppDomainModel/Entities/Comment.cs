using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsApp.GoodNewsAppDomainModel.Entities
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Body { get; set; } 
        public DateTime CreatedOnDate { get; set; }
        public Guid UserId { get; set; }
 
        public Guid NewsId { get; set; }

        public News News { get; set; }


    }


}
