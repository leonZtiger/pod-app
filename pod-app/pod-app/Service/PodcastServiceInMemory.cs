using pod_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pod_app.Service
{
    public class PodcastServiceInMemory : IPodcastDataService
    {
        private readonly List<Podcast> feedCollection = new();
        private readonly List<Episode> podcastCollection = new();

        public void PushFeed(Podcast feed)
        {
            if (feed == null)
                throw new ArgumentNullException(nameof(feed));

            // Generate ID if missing
            if (string.IsNullOrWhiteSpace(feed.Id))
                feed.Id = Guid.NewGuid().ToString();

            feed.Episodes ??= new List<Episode>();

            feedCollection.Add(feed);
        }

        public void DeleteFeed(Podcast feed)
        {
            if (feed == null)
                throw new ArgumentNullException(nameof(feed));

            // Remove all episodes belonging to the feed
            podcastCollection.RemoveAll(p => p.PodcastFeedId == feed.Id);

            // Remove the feed
            feedCollection.RemoveAll(f => f.Id == feed.Id);
        }

        public List<Podcast> GetAllFeeds()
        {
            foreach (var feed in feedCollection)
            {
                feed.Episodes ??= new List<Episode>();
                feed.Episodes = podcastCollection
                    .Where(p => p.PodcastFeedId == feed.Id)
                    .ToList();
            }


            return feedCollection.ToList();
        }

        public Podcast? GetFeed(string id)
        {
            var feed = feedCollection.FirstOrDefault(f => f.Id == id);
            if (feed == null)
                return null;

            feed.Episodes ??= new List<Episode>();

            feed.Episodes = podcastCollection
                .Where(p => p.PodcastFeedId == feed.Id)
                .ToList();

            return feed;
        }

        public List<Episode> GetPodcasts(Podcast feed)
        {  
            var p = podcastCollection
                .Where(p => p.PodcastFeedId == feed.Id)
                .ToList();

            return feed.Episodes;
        }
    }
}
