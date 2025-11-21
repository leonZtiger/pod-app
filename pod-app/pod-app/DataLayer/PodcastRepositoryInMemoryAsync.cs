using pod_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pod_app.DataLayer
{
    public class PodcastRepositoryInMemoryAsync : IPodcastRepositoryAsync
    {
        private readonly List<PodFlow> feedCollection = new();
        private readonly List<PodModel> podcastCollection = new();

        

        public Task<List<PodFlow>> GetAllFeedsAsync()
        {
           
            foreach (var feed in feedCollection)
            {
                feed.Podcasts = podcastCollection
                    .Where(p => p.PodcastFeedId == feed.Id)
                    .ToList();
            }

            
            return Task.FromResult(feedCollection.ToList());
        }

        public Task<PodFlow?> GetFeedAsync(string id)
        {
            var feed = feedCollection.FirstOrDefault(f => f.Id == id);
            if (feed == null)
                return Task.FromResult<PodFlow?>(null);

            feed.Podcasts = podcastCollection
                .Where(p => p.PodcastFeedId == feed.Id)
                .ToList();

            return Task.FromResult<PodFlow?>(feed);
        }

        public Task<List<PodModel>> GetPodcastsAsync(PodFlow feed)
        {
            var pods = podcastCollection
                .Where(p => p.PodcastFeedId == feed.Id)
                .ToList();

            return Task.FromResult(pods);
        }

        public Task PushFeedAsync(PodFlow feed)
        {
            if (feed == null)
                throw new ArgumentNullException(nameof(feed));

            if (string.IsNullOrWhiteSpace(feed.Id))
                feed.Id = Guid.NewGuid().ToString();

            feed.Podcasts ??= new List<PodModel>();

            
            if (!feedCollection.Any(f => f.Id == feed.Id))
            {
                feedCollection.Add(feed);
            }

            return Task.CompletedTask;
        }

        public Task UpdateFeedAsync(PodFlow feed)
        {
            if (feed == null)
                throw new ArgumentNullException(nameof(feed));

            int index = feedCollection.FindIndex(f => f.Id == feed.Id);

            if (index == -1)
            {
                throw new KeyNotFoundException("Could not find feed with specified id.");
            }

         
            feedCollection[index] = feed;

            return Task.CompletedTask;
        }

        public Task DeleteFeedAsync(PodFlow feed)
        {
            if (feed == null)
                throw new ArgumentNullException(nameof(feed));

            
            podcastCollection.RemoveAll(p => p.PodcastFeedId == feed.Id);

            feedCollection.RemoveAll(f => f.Id == feed.Id);

            return Task.CompletedTask;
        }

        
        public Task PushPodAsync(PodModel pod)
        {
            if (pod == null)
                throw new ArgumentNullException(nameof(pod));
            if (string.IsNullOrWhiteSpace(pod.PodcastFeedId))
                throw new InvalidOperationException("PodcastFeedId must be set.");

            if (string.IsNullOrWhiteSpace(pod.Id))
                pod.Id = Guid.NewGuid().ToString();

            podcastCollection.Add(pod);

            var feed = feedCollection.FirstOrDefault(f => f.Id == pod.PodcastFeedId);
            if (feed != null)
            {
                feed.Podcasts ??= new List<PodModel>();
                feed.Podcasts.Add(pod);
            }

            return Task.CompletedTask;
        }
    }
}
