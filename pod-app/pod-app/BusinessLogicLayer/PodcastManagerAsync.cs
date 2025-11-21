using MongoDB.Driver;
using pod_app.DataLayer;
using pod_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pod_app.BusinessLogicLayer
{

    public class PodcastManagerAsync
    {
        private readonly IPodcastRepositoryAsync podcastRepo;
    
        public PodcastManagerAsync(IPodcastRepositoryAsync podcastRepo)
        {
            
            this.podcastRepo = podcastRepo ?? throw new ArgumentNullException(nameof(podcastRepo)); ;
        }

        public async Task<List<PodFlow>> GetAllFeedsAsync()
        {
            var feeds = await podcastRepo.GetAllFeedsAsync();

            foreach (var feed in feeds)
            {
                feed.Podcasts = await podcastRepo.GetPodcastsAsync(feed);
            }

            return feeds;
        }

        public async Task PushFeedAsync(PodFlow feed)
        {
            var feeds = await podcastRepo.GetAllFeedsAsync();
           
            if (!feeds.Any(f => f.Id == feed.Id))
            {
                await podcastRepo.PushFeedAsync(feed);
            }
           
        }

        public async Task MoveFeedAsync(PodFlow fromFeed, PodFlow toFeed)
        {
            if (fromFeed is null) throw new ArgumentNullException(nameof(fromFeed));
            if (toFeed is null) throw new ArgumentNullException(nameof(toFeed));

            await PushFeedAsync(toFeed);

            var prev = await podcastRepo.GetFeedAsync(fromFeed.Id);

            if (prev != null)
            {
                await podcastRepo.DeleteFeedAsync(prev);
            }
        }


        public async Task DeleteFeedAsync(PodFlow feed)
        {
           
            feed.Podcasts.Clear();

          
            await podcastRepo.DeleteFeedAsync(feed);
        }

    }
}

