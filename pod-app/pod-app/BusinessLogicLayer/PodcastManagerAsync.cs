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

        public async Task<List<Podcast>> GetAllFeedsAsync()
        {
            var feeds = await podcastRepo.GetAllFeedsAsync(); 

            foreach (var i in feeds)
            {
                i.Episodes = RssUtilHelpers.GetPodFeedFromXML(
                    await RssUtilHelpers.GetRssXMLFile(i.Url)
                ).Episodes;
            }

            return feeds;
        }


        public async Task PushFeedAsync(Podcast feed)
        {
            var feeds = await podcastRepo.GetAllFeedsAsync();

            if (!feeds.Any(f => f.Id == feed.Id))
            {
                await podcastRepo.PushFeedAsync(feed);
            }

        }

        public async Task DeleteFeedAsync(Podcast feed)
        {
           
            feed.Episodes.Clear();

          
            await podcastRepo.DeleteFeedAsync(feed);
        }

    }
}

