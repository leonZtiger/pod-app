using MongoDB.Driver;
using pod_app.Models;
using pod_app.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class MongoDbServiceTest : IDisposable
    {
        private readonly IPodcastDataService service;

        public MongoDbServiceTest()
        {
            service = new PodcastServiceInMemory();
        }

        [Fact]
        public void TestGetAllFeeds()
        {
            // Base count
            int base_count = service.GetAllFeeds().Count;

            // Insert test data
            var feed1 = new PodFlow
            {
                Category = "Tech",
                Podcasts = new List<PodModel>()
            };

            var feed2 = new PodFlow
            {
                Category = "News",
                Podcasts = new List<PodModel>()
            };

            service.PushFeed(feed1);
            service.PushFeed(feed2);

            // Act
            var data = service.GetAllFeeds();

            // Validate data
            Assert.NotNull(data);
            Assert.Equal(base_count + 2, data.Count);
            Assert.Contains(data, f => f.Category == "Tech");
            Assert.Contains(data, f => f.Category == "News");

            // Remove data
            service.DeleteFeed(feed1);
            service.DeleteFeed(feed2);
        }

        [Fact]
        public void TestGetFeed()
        {
            // Insert single feed
            var feed = new PodFlow
            {
                Category = "Music",
                Podcasts = new List<PodModel>()
            };

            service.PushFeed(feed);
            Assert.False(string.IsNullOrWhiteSpace(feed.Id)); // Mongo sets Id

            // Act
            var loaded = service.GetFeed(feed.Id);

            // Validate data
            Assert.NotNull(loaded);
            Assert.Equal(feed.Id, loaded.Id);
            Assert.Equal("Music", loaded.Category);

            // Remove data
            service.DeleteFeed(feed);
            Assert.Null(service.GetFeed(feed.Id));
        }

        [Fact]
        public void TestPushFeed()
        {
            // Insert single feed
            var feed = new PodFlow
            {
                Category = "Comedy",
                Podcasts = new List<PodModel>()
            };

            service.PushFeed(feed);

            // Validate data
            var fromDb = service.GetFeed(feed.Id);
            Assert.NotNull(fromDb);
            Assert.Equal("Comedy", fromDb.Category);

            // remove data
            service.DeleteFeed(feed);
        }

        [Fact]
        public void TestDeleteFeed()
        {
            // Insert single feed
            var feed = new PodFlow
            {
                Category = "DeleteMe",
                Podcasts = new List<PodModel>()
            };

            service.PushFeed(feed);
            Assert.NotNull(service.GetFeed(feed.Id));

            // remove data
            service.DeleteFeed(feed);

            // Ensure data is deleted
            var deleted = service.GetFeed(feed.Id);
            Assert.Null(deleted);
        }

        [Fact]
        public void TestPushPod()
        {
            // Insert single feed (for the episode to belong to)
            var feed = new PodFlow
            {
                Category = "EpisodesTest",
                Podcasts = new List<PodModel>()
            };
            service.PushFeed(feed);

            var episode = new PodModel
            {
                PodcastFeedId = feed.Id,
                Title = "Test Episode",
                Description = "Test description",
                ImageUrl = "http://example.com/image.png",
                ReleaseDate = DateTime.UtcNow.ToUniversalTime(),
                Duration = "30:00",
                URL = "http://example.com/episode",
                Category = "EpisodesTest",
                IsSaved = true
            };

            // Insert single podcast
            service.PushPod(episode);

            var retEpisode = service.GetPodcast(episode.Id);

            Assert.NotNull(retEpisode);
            Assert.Equal(retEpisode.Title, episode.Title);
            Assert.Equal(retEpisode.Description, episode.Description);
            Assert.Equal(retEpisode.Description, episode.Description);
            Assert.Equal(retEpisode.ImageUrl, episode.ImageUrl);
            Assert.Equal(retEpisode.ReleaseDate, episode.ReleaseDate);
            Assert.Equal(retEpisode.URL, episode.URL);
            Assert.Equal(retEpisode.Category, episode.Category);
            Assert.Equal(retEpisode.IsSaved, episode.IsSaved);

            // remove data
            service.DeletePod(retEpisode);
            service.DeleteFeed(feed);
        }

        [Fact]
        public void TestDeletePod()
        {
            // insert feed
            var feed = new PodFlow
            {
                Category = "DeletePod",
                Podcasts = new List<PodModel>()
            };
            service.PushFeed(feed);
            // insert podcast
            var episode = new PodModel
            {
                PodcastFeedId = feed.Id,
                Title = "ToDelete",
                Description = "desc",
                ImageUrl = "http://example.com/img.png",
                ReleaseDate = DateTime.UtcNow,
                Duration = "10:00",
                URL = "http://example.com",
                Category = "DeletePod",
                IsSaved = false
            };

            service.PushPod(episode);

            // delete one
            service.DeletePod(episode);

            // ensure its deleted
            Assert.Null(service.GetPodcast(episode.Id));

            service.DeleteFeed(feed);
        }

        public void Dispose()
        {
            // CLear database 
            var feeds = service.GetAllFeeds();

            foreach (var f in feeds)
            {
                var ps = f.Podcasts;

                foreach (var item in ps)
                {
                    service.DeletePod(item);
                }
                service.DeleteFeed(f);
            }

        }
    }
}
