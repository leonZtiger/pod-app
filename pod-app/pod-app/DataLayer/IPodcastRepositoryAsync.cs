using pod_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pod_app.DataLayer
{
    public interface IPodcastRepositoryAsync
    {
        Task<List<PodFlow>> GetAllFeedsAsync();

        Task<PodFlow?> GetFeedAsync(string id);
        
        Task<List<PodModel>> GetPodcastsAsync(PodFlow feed);

        Task PushFeedAsync(PodFlow feed);

        Task UpdateFeedAsync(PodFlow feed);

        Task DeleteFeedAsync(PodFlow feed);

        

    }
}
