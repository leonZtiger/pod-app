using pod_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pod_app.DataLayer
{

    // Interface for the repository
    // Makes sure the repository implements these methods
    public interface IPodcastRepositoryAsync
    {
        Task<List<Podcast>> GetAllFeedsAsync();

        Task<Podcast?> GetFeedAsync(string id);
       
        Task PushFeedAsync(Podcast feed);

        Task UpdateFeedAsync(Podcast feed);

        Task DeleteFeedAsync(Podcast feed);  

    }
}
