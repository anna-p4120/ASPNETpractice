using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsApp.Models.Entities
{
    public class Comment
    {
        public Guid Id { get; set; }
       public string Content { get; set; } 
        public DateTime PublicationDate { get; set; }
        public Guid UsersId { get; set; }
 
        public Guid NewsId { get; set; }



    }


}
