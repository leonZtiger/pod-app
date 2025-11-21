using pod_app.BusinessLogicLayer;
using pod_app.DataLayer;
using pod_app.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class PodcastManagerAsyncTests
    {
        private readonly PodcastRepositoryInMemoryAsync repo;
        private readonly PodcastManagerAsync manager;

        public PodcastManagerAsyncTests()
        {
            
            repo = new PodcastRepositoryInMemoryAsync();
            manager = new PodcastManagerAsync(repo);
        }

        [Fact]
        public async Task GetAllFeedsAsync_ReturnsAllFeeds()
        {
            
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

            await repo.PushFeedAsync(feed1);
            await repo.PushFeedAsync(feed2);

         
            var feeds = await manager.GetAllFeedsAsync();

            
            Assert.NotNull(feeds);
            Assert.Equal(2, feeds.Count);
            Assert.Contains(feeds, f => f.Category == "Tech");
            Assert.Contains(feeds, f => f.Category == "News");
        }

        [Fact]
        public async Task PushFeedAsync_AddsNewFeed_WhenItDoesNotExist()
        {
          
            var startFeeds = await manager.GetAllFeedsAsync();
            int baseCount = startFeeds.Count;

            var feed = new PodFlow
            {
                Id = Guid.NewGuid().ToString(),
                Category = "NewCategory",
                Podcasts = new List<PodModel>()
            };

           
            await manager.PushFeedAsync(feed);

           
            var feeds = await manager.GetAllFeedsAsync();
            Assert.Equal(baseCount + 1, feeds.Count);
            Assert.Contains(feeds, f => f.Id == feed.Id && f.Category == "NewCategory");
        }

        [Fact]
        public async Task PushFeedAsync_DoesNotDuplicateExistingFeed()
        {
            
            var feedId = Guid.NewGuid().ToString();

            var feed = new PodFlow
            {
                Id = feedId,
                Category = "DuplicateTest",
                Podcasts = new List<PodModel>()
            };

            await manager.PushFeedAsync(feed);

           
            await manager.PushFeedAsync(feed);

            
            var feeds = await manager.GetAllFeedsAsync();
            var matches = feeds.FindAll(f => f.Id == feedId);

            Assert.Single(matches);
        }

        [Fact]
        public async Task DeleteFeedAsync_RemovesFeed()
        {
           
            var feed = new PodFlow
            {
                Id = Guid.NewGuid().ToString(),
                Category = "DeleteCategory",
                Podcasts = new List<PodModel>()
            };

            await manager.PushFeedAsync(feed);
            var before = await manager.GetAllFeedsAsync();
            Assert.Contains(before, f => f.Id == feed.Id);

         
            await manager.DeleteFeedAsync(feed);

            
            var after = await manager.GetAllFeedsAsync();
            Assert.DoesNotContain(after, f => f.Id == feed.Id);
        }
    }
}

