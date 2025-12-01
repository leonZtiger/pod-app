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

            this.podcastRepo = podcastRepo ?? throw new ArgumentNullException(nameof(podcastRepo));
        }

        public async Task<List<Podcast>> GetAllFeedsAsync()
        {
            var feeds = await podcastRepo.GetAllFeedsAsync();

            foreach (var i in feeds)
            {
                if (string.IsNullOrWhiteSpace(i.Url))
                {
                    i.Episodes = new();
                    continue;
                }

                string xmlFile = await RssUtilHelpers.GetRssXMLFile(i.Url);

                if (string.IsNullOrEmpty(xmlFile))
                {
                    i.Episodes = new();
                    continue;
                }

                var p = RssUtilHelpers.GetPodFeedFromXML(xmlFile, i.Url);

                // Copy loaded fields to podcast object.
                i.Episodes = p.Episodes;
                i.About = p.About;
                i.ImageUrl = p.ImageUrl;

                // Only gets the title from the rss if the user hasnt put in its own title
                if (string.IsNullOrWhiteSpace(i.Title))
                {
                    i.Title = p.Title;
                }

                // Only gets the Genre as category from the rss if the user hasnt put in its own Category
                if (string.IsNullOrWhiteSpace(i.Category))
                {
                    i.Category = p.Category;
                }
            }

            return feeds;
        }



        // Pushes the podcast into the collection
        public async Task PushFeedAsync(Podcast feed)
        {
            var feeds = await podcastRepo.GetAllFeedsAsync();

            if (!feeds.Any(f => f.Id == feed.Id))
            {
                await podcastRepo.PushFeedAsync(feed);
            }

        }


        // Removes the podcast from the collection
        public async Task DeleteFeedAsync(Podcast feed)
        {

            feed.Episodes.Clear();


            await podcastRepo.DeleteFeedAsync(feed);
        }

        public async Task UpdateFeedAsync(Podcast feed)
        {
            await podcastRepo.UpdateFeedAsync(feed);
        }

        // Gets all the different categories in the collection
        public async Task<List<string>> GetAllCategoriesAsync()
        {
            var podcasts = await podcastRepo.GetAllFeedsAsync();

            return podcasts
            .Select(p => p.Category?.Trim())
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Select(c => c!)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(c => c, StringComparer.OrdinalIgnoreCase)  // does not differentiate between upper and lowercase
            .ToList();
        }


        // Adds a category to a Podcast
        public async Task AddCategoryToPodcastAsync(string podId, string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                throw new ArgumentException("Category name can’t be empty.", nameof(categoryName));

            var podcast = await podcastRepo.GetFeedAsync(podId);
            if (podcast == null)
                throw new KeyNotFoundException("Podcast not found.");

            podcast.Category = categoryName.Trim();
            await podcastRepo.UpdateFeedAsync(podcast);
        }
        
        public async Task UpdateFeedAsync(Podcast feed)
        {
            await podcastRepo.UpdateFeedAsync(feed);
        }

        // Changes the name of a category and makes sure all podcasts gets the new name
        public async Task RenameCategoryAsync(string oldCategory, string newCategory)
        {
            if (string.IsNullOrWhiteSpace(oldCategory))
                throw new ArgumentException("Old category cannot be empty.");

            if (string.IsNullOrWhiteSpace(newCategory))
                throw new ArgumentException("New category cannot be empty.");

            if (oldCategory.Trim().Equals(newCategory.Trim(), StringComparison.OrdinalIgnoreCase))
                return;

            var podcasts = await podcastRepo.GetAllFeedsAsync();

            var toUpdate = podcasts
                .Where(p => string.Equals(p.Category?.Trim(), oldCategory.Trim(), StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!toUpdate.Any())
                return;

            foreach (var podcast in toUpdate)
            {
                podcast.Category = newCategory.Trim();
                await podcastRepo.UpdateFeedAsync(podcast);
            }
        }

        // Removes a category from the collection
        public async Task DeleteCategoryAsync(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                throw new ArgumentException("Category name cannot be empty.");

            var podcasts = await podcastRepo.GetAllFeedsAsync();

            var toUpdate = podcasts
                .Where(p => string.Equals(p.Category?.Trim(), categoryName.Trim(), StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!toUpdate.Any())
                return;

            foreach (var podcast in toUpdate)
            {
                podcast.Category = null;
                await podcastRepo.UpdateFeedAsync(podcast);
            }
        }
    }
}

