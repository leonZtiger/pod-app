using MongoDB.Bson;
using MongoDB.Driver;
using pod_app.BusinessLogicLayer;
using pod_app.DataLayer;
using pod_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class PodcastManagerTest
    {

        private PodcastManager manager;
        private PodcastServiceInMemory service;
        
        public PodcastManagerTest()
        {
            service = new();
            manager = new(service);
        }

        [Fact]
        public void TestGetAllFeeds()
        {
            // Test data
            var feed1 = new PodFlow
            {
                Id = Guid.NewGuid().ToString(),
                Category = "Tech",
                Podcasts = new List<PodModel>()
            };
            var feed2 = new PodFlow
            {
                Id = Guid.NewGuid().ToString(),
                Category = "News",
                Podcasts = new List<PodModel>()
            };

            service.PushFeed(feed1);
            service.PushFeed(feed2);

            // Act
            var feeds = manager.GetAllFeeds();

            // Assert
            Assert.NotNull(feeds);
            Assert.Equal(2, feeds.Count);
            Assert.Contains(feeds, f => f.Category == "Tech");
            Assert.Contains(feeds, f => f.Category == "News");
        }

        [Fact]
        public void TestPushPod_CreatesFeedIfMissingAndAddsEpisode()
        {
            int startCount = manager.GetAllFeeds().Count;

            var feedId = Guid.NewGuid().ToString();

            var feed = new PodFlow
            {
                Id = feedId,
                Category = "PushPodCategory",
                Podcasts = new List<PodModel>()
            };

            var pod = new PodModel
            {
                PodcastFeedId = feedId,
                Title = "PushedEpisode",
                Description = "desc",
                ImageUrl = "http://example.com/img.png",
                ReleaseDate = DateTime.UtcNow,
                Duration = "30:00",
                URL = "http://example.com/ep",
                Category = "PushPodCategory",
                IsSaved = true
            };

            // Act
            manager.PushPod(pod, feed);

            // Assert
            var feeds = manager.GetAllFeeds();
            Assert.Equal(feeds.Count, startCount +1);
            var storedFeed = feeds.Find(f => f.Id == feed.Id);
            Assert.Equal(feedId, storedFeed.Id);
            Assert.Equal("PushPodCategory", storedFeed.Category);
            Assert.Single(storedFeed.Podcasts);
            Assert.Equal("PushedEpisode", storedFeed.Podcasts[0].Title);

            Assert.False(string.IsNullOrWhiteSpace(pod.Id));
            var storedPod = service.GetPodcast(pod.Id);
            Assert.NotNull(storedPod);
            Assert.Equal(feedId, storedPod!.PodcastFeedId);
        }

        [Fact]
        public void TestDeletePod_RemovesEpisodeAndDeletesEmptyFeed()
        {
            var feedId = Guid.NewGuid().ToString();

            var feed = new PodFlow
            {
                Id = feedId,
                Category = "DeletePodCategory",
                Podcasts = new List<PodModel>()
            };
            service.PushFeed(feed);

            var pod = new PodModel
            {
                PodcastFeedId = feedId,
                Title = "EpisodeToDelete",
                Description = "desc",
                ImageUrl = "http://example.com/img.png",
                ReleaseDate = DateTime.UtcNow,
                Duration = "20:00",
                URL = "http://example.com/ep",
                Category = "DeletePodCategory",
                IsSaved = false
            };
            service.PushPod(pod);

            Assert.Single(manager.GetAllFeeds());
            Assert.NotNull(service.GetPodcast(pod.Id));

            // Act
            manager.DeletePod(pod);

            // episode removed
            Assert.Null(service.GetPodcast(pod.Id));

            // Feed should be deleted because it became empty
            var feeds = manager.GetAllFeeds();
            Assert.Empty(feeds);
        }

        [Fact]
        public void TestMovePod_MovesEpisodeToNewFeedAndDeletesSourceIfEmpty()
        {
            // Arrange
            var srcId = Guid.NewGuid().ToString();
            var dstId = Guid.NewGuid().ToString();

            var srcFeed = new PodFlow
            {
                Id = srcId,
                Category = "Source",
                Podcasts = new List<PodModel>()
            };
            var dstFeed = new PodFlow
            {
                Id = dstId,
                Category = "Destination",
                Podcasts = new List<PodModel>()
            };

            service.PushFeed(srcFeed);
            service.PushFeed(dstFeed);

            var pod = new PodModel
            {
                PodcastFeedId = srcId,
                Title = "MoveMe",
                Description = "desc",
                ImageUrl = "http://example.com/img.png",
                ReleaseDate = DateTime.UtcNow,
                Duration = "15:00",
                URL = "http://example.com/ep",
                Category = "Source",
                IsSaved = true
            };
            service.PushPod(pod);

            Assert.Single(service.GetAllFeeds().Where(f => f.Id == srcId));
            Assert.Single(service.GetFeed(srcId)!.Podcasts);
            Assert.Empty(service.GetFeed(dstId)!.Podcasts);

            // Act
            manager.MovePod(pod, dstFeed);

            var allFeeds = manager.GetAllFeeds();    
            // Ensure source is deleted since it its empty
            Assert.Null(allFeeds.Find(f => f.Id == srcId));
            // Ensure destination has the moved podcast
            Assert.NotNull(allFeeds.Find(f => f.Id == dstId));
            Assert.NotNull(allFeeds.Find(f => f.Id == dstId)?.Podcasts.Find(p => p.Id == pod.Id));

        }

        [Fact]
        public void TestDeletePodFeed_RemovesFeedAndAllItsEpisodes()
        {
            // Arrange
            var feedId = Guid.NewGuid().ToString();

            var episode1 = new PodModel
            {
                PodcastFeedId = feedId,
                Title = "Ep1",
                Description = "First",
                ImageUrl = "http://example.com/1.png",
                ReleaseDate = DateTime.UtcNow,
                Duration = "10:00",
                URL = "http://example.com/1",
                Category = "DeleteFeed",
                IsSaved = false
            };

            var episode2 = new PodModel
            {
                PodcastFeedId = feedId,
                Title = "Ep2",
                Description = "Second",
                ImageUrl = "http://example.com/2.png",
                ReleaseDate = DateTime.UtcNow,
                Duration = "20:00",
                URL = "http://example.com/2",
                Category = "DeleteFeed",
                IsSaved = true
            };

            var feed = new PodFlow
            {
                Id = feedId,
                Category = "DeleteFeed",
                Podcasts = new List<PodModel>()
            };

            service.PushFeed(feed);
            service.PushPod(episode1);
            service.PushPod(episode2);

            // Sanity check
            Assert.Single(manager.GetAllFeeds());
            Assert.Equal(2, service.GetFeed(feedId)!.Podcasts.Count);

            // Act
            manager.DeletePodFeed(feed);

            Assert.Empty(manager.GetAllFeeds());

            Assert.Null(service.GetPodcast(episode1.Id));
            Assert.Null(service.GetPodcast(episode2.Id));
        }
    }
}
