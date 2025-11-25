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
        public async void  TestGetAllFeeds()
        {
            // Test data
            var feed1 = new Podcast
            {
                Id = Guid.NewGuid().ToString(),
                Category = "Tech",
                Episodes = new List<Episode>()
            };
            var feed2 = new Podcast
            {
                Id = Guid.NewGuid().ToString(),
                Category = "News",
                Episodes = new List<Episode>()
            };

            service.PushFeed(feed1);
            service.PushFeed(feed2);

            // Act
            var feeds = await manager.GetAllFeedsAsync();

            // Assert
            Assert.NotNull(feeds);
            Assert.Equal(2, feeds.Count);
            Assert.Contains(feeds, f => f.Category == "Tech");
            Assert.Contains(feeds, f => f.Category == "News");
        }
       

        [Fact]
        public async void TestDeletePodFeed_RemovesFeedAndAllItsEpisodes()
        {
            // Arrange
            var feedId = Guid.NewGuid().ToString();
            var feed = new Podcast
            {
                Id = feedId,
                Category = "DeleteFeed",
                Episodes = new List<Episode>()
            };

            service.PushFeed(feed);

            // Sanity check
            Assert.Single(await manager.GetAllFeedsAsync());

            // Act
            manager.DeletePodcastFeed(feed);

            Assert.Empty(await manager.GetAllFeedsAsync());
        }
    }
}
