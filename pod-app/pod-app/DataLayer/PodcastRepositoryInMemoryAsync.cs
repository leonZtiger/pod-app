using pod_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pod_app.DataLayer
{
    public class PodcastRepositoryInMemoryAsync : IPodcastRepositoryAsync
    {
        private readonly List<Podcast> feedCollection = new();
        private readonly List<Episode> podcastCollection = new();



        public Task<List<Podcast>> GetAllFeedsAsync()
        {

            foreach (var feed in feedCollection)
            {
                feed.Episodes = podcastCollection
                    .Where(p => p.PodcastFeedId == feed.Id)
                    .ToList();
            }


            return Task.FromResult(feedCollection.ToList());
        }

        public Task<Podcast?> GetFeedAsync(string id)
        {
            var feed = feedCollection.FirstOrDefault(f => f.Id == id);
            if (feed == null)
                return Task.FromResult<Podcast?>(null);

            feed.Episodes = podcastCollection
                .Where(p => p.PodcastFeedId == feed.Id)
                .ToList();

            return Task.FromResult<Podcast?>(feed);
        }

        public Task<List<Episode>> GetPodcastsAsync(Podcast feed)
        {
            var pods = podcastCollection
                .Where(p => p.PodcastFeedId == feed.Id)
                .ToList();

            return Task.FromResult(pods);
        }

        public Task PushFeedAsync(Podcast feed)
        {
            if (feed == null)
                throw new ArgumentNullException(nameof(feed));

            if (string.IsNullOrWhiteSpace(feed.Id))
                feed.Id = Guid.NewGuid().ToString();

            feed.Episodes ??= new List<Episode>();


            if (!feedCollection.Any(f => f.Id == feed.Id))
            {
                feedCollection.Add(feed);
            }

            return Task.CompletedTask;
        }

        public Task UpdateFeedAsync(Podcast feed)
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

        public Task DeleteFeedAsync(Podcast feed)
        {
            if (feed == null)
                throw new ArgumentNullException(nameof(feed));


            podcastCollection.RemoveAll(p => p.PodcastFeedId == feed.Id);

            feedCollection.RemoveAll(f => f.Id == feed.Id);

            return Task.CompletedTask;
        }

    }
}
