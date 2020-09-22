using GoodNewsApp.BusinessLogic.Services.NewsServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoodNewsApp.BusinessLogic.Interfaces
{
   public interface INewsService
    {
        Task AddAsync(NewsDTO newsDTO);
        Task<NewsDTO> GetByIdAsync(Guid id, CancellationToken token);
        Task<List<NewsDTO>> GetAllAsync(CancellationToken token);
        Task UpdateAsync(NewsDTO newsDTO);
        Task DeleteAsync(Guid id); 
        
      
    }
}
