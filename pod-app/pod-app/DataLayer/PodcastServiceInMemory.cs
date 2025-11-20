using pod_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pod_app.DataLayer
{
    public class PodcastServiceInMemory : IPodcastRepository
    {
        private readonly List<PodFlow> feedCollection = new();
        private readonly List<PodModel> podcastCollection = new();

        public void PushFeed(PodFlow feed)
        {
            if (feed == null)
                throw new ArgumentNullException(nameof(feed));

            // Generate ID if missing
            if (string.IsNullOrWhiteSpace(feed.Id))
                feed.Id = Guid.NewGuid().ToString();

            feed.Podcasts ??= new List<PodModel>();

            feedCollection.Add(feed);
        }

        public void DeleteFeed(PodFlow feed)
        {
            if (feed == null)
                throw new ArgumentNullException(nameof(feed));

            // Remove all episodes belonging to the feed
            podcastCollection.RemoveAll(p => p.PodcastFeedId == feed.Id);

            // Remove the feed
            feedCollection.RemoveAll(f => f.Id == feed.Id);
        }

        public void DeletePod(PodModel pod)
        {
            if (pod == null)
                throw new ArgumentNullException(nameof(pod));

            // Remove from global episode list
            podcastCollection.RemoveAll(p => p.Id == pod.Id);

            // Also remove from embedded feed.Podcasts lists
            for (int i = 0; i < feedCollection.Count; i++)
            {
                if (feedCollection[i].Podcasts != null)
                {
                    feedCollection[i].Podcasts.RemoveAll(p => p.Id == pod.Id);

                    if (feedCollection[i].Podcasts.Count == 0)
                        feedCollection.Remove(feedCollection[i]);
                }

            }


        }

        public List<PodFlow> GetAllFeeds()
        {
            foreach (var feed in feedCollection)
            {
                feed.Podcasts ??= new List<PodModel>();
                feed.Podcasts = podcastCollection
                    .Where(p => p.PodcastFeedId == feed.Id)
                    .ToList();
            }


            return feedCollection.ToList();
        }

        public void PushPod(PodModel pod)
        {
            if (pod == null)
                throw new ArgumentNullException(nameof(pod));
            if (string.IsNullOrWhiteSpace(pod.PodcastFeedId))
                throw new InvalidOperationException("PodcastFeedId must be set.");

            // Generate ID if missing
            if (string.IsNullOrWhiteSpace(pod.Id))
                pod.Id = Guid.NewGuid().ToString();

            podcastCollection.Add(pod);

            // Attach to feed.Podcasts if feed exists
            var feed = feedCollection.FirstOrDefault(f => f.Id == pod.PodcastFeedId);
            if (feed != null)
            {
                feed.Podcasts ??= new List<PodModel>();
                feed.Podcasts.Add(pod);
            }
        }

        public PodFlow? GetFeed(string id)
        {
            var feed = feedCollection.FirstOrDefault(f => f.Id == id);
            if (feed == null)
                return null;

            feed.Podcasts ??= new List<PodModel>();

            feed.Podcasts = podcastCollection
                .Where(p => p.PodcastFeedId == feed.Id)
                .ToList();

            return feed;
        }

        public PodModel? GetPodcast(string id)
        {
            return podcastCollection.FirstOrDefault(p => p.Id == id);
        }

        public void UpdateModel(PodModel model)
        {
            int i = podcastCollection.FindIndex(p => p.Id == model.Id);
            
            if (i != -1)
                podcastCollection[i] = model;
        }

        public List<PodModel> GetPodcasts(PodFlow feed)
        {  
            var p = podcastCollection
                .Where(p => p.PodcastFeedId == feed.Id)
                .ToList();

            return feed.Podcasts;
        }
    }
}
