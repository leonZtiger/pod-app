using pod_app.DataLayer;
using pod_app.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class MongoDbServiceAsyncTests
    {
        private readonly IPodcastRepositoryAsync service;

        public MongoDbServiceAsyncTests()
        {
           
            service = new PodcastRepositoryInMemoryAsync();
        }

        [Fact]
        public async Task TestGetAllFeedsAsync()
        {
            
            var before = await service.GetAllFeedsAsync();
            int base_count = before.Count;

           
            var feed1 = new Podcast
            {
                Category = "Tech",
                Episodes = new List<Episode>()
            };

            var feed2 = new Podcast
            {
                Category = "News",
                Episodes = new List<Episode>()
            };

            await service.PushFeedAsync(feed1);
            await service.PushFeedAsync(feed2);

           
            var data = await service.GetAllFeedsAsync();

            
            Assert.NotNull(data);
            Assert.Equal(base_count + 2, data.Count);
            Assert.Contains(data, f => f.Category == "Tech");
            Assert.Contains(data, f => f.Category == "News");

            
            await service.DeleteFeedAsync(feed1);
            await service.DeleteFeedAsync(feed2);
        }

        [Fact]
        public async Task TestGetFeedAsync()
        {
            
            var feed = new Podcast
            {
                Category = "Music",
                Episodes = new List<Episode>()
            };

            await service.PushFeedAsync(feed);
            Assert.False(string.IsNullOrWhiteSpace(feed.Id));

        
            var loaded = await service.GetFeedAsync(feed.Id);

            
            Assert.NotNull(loaded);
            Assert.Equal(feed.Id, loaded!.Id);
            Assert.Equal("Music", loaded.Category);

            // Remove data
            await service.DeleteFeedAsync(feed);
            var afterDelete = await service.GetFeedAsync(feed.Id);
            Assert.Null(afterDelete);
        }

        [Fact]
        public async Task TestPushFeedAsync()
        {
         
            var feed = new Podcast
            {
                Category = "Comedy",
                Episodes = new List<Episode>()
            };

            await service.PushFeedAsync(feed);

           
            var fromDb = await service.GetFeedAsync(feed.Id);
            Assert.NotNull(fromDb);
            Assert.Equal("Comedy", fromDb!.Category);

            
            await service.DeleteFeedAsync(feed);
        }

        [Fact]
        public async Task TestDeleteFeedAsync()
        {
            
            var feed = new Podcast
            {
                Category = "DeleteMe",
                Episodes = new List<Episode>()
            };

            await service.PushFeedAsync(feed);

            var exists = await service.GetFeedAsync(feed.Id);
            Assert.NotNull(exists);

            
            await service.DeleteFeedAsync(feed);

            
            var deleted = await service.GetFeedAsync(feed.Id);
            Assert.Null(deleted);
        }
    }
}

