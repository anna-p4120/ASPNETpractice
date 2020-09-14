using System;
using System.Collections.Generic;
using System.Text;

namespace GoodNewsApp.DataAccess.Entities
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}
