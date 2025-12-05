using Microsoft.VisualStudio.TestPlatform.Utilities;
using pod_app.BusinessLogicLayer;
using pod_app.DataLayer;
using pod_app.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests
{


    using Xunit;
    using Xunit.Abstractions;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class PodcastManagerAsyncTests
    {
        private readonly ITestOutputHelper output;
        private readonly PodcastRepositoryInMemoryAsync repo;
        private readonly PodcastManagerAsync manager;

        public PodcastManagerAsyncTests(ITestOutputHelper output)
        {
            this.output = output;

            repo = new PodcastRepositoryInMemoryAsync();
            manager = new PodcastManagerAsync(repo);

            repo.Seed(
                new Podcast { Id = "1", Category = "Tech" },
                new Podcast { Id = "2", Category = "News" },
                new Podcast { Id = "3", Category = "Tech" },
                new Podcast { Id = "4", Category = "  Sport  " },
                new Podcast { Id = "5", Category = "" }
            );
        }

        [Fact]
        public async Task GetAllCategoriesAsync_ReturnsDistinctSortedList()
        {
            // Act
            var categories = await manager.GetAllCategoriesAsync();

            // Debug-utskrift
            
            foreach (var c in categories)
                output.WriteLine($"- {c}");

            // Assert
            Assert.NotNull(categories);
            Assert.Equal(3, categories.Count);
            Assert.Equal(new List<string> { "News", "Sport", "Tech" }, categories);
        }

        [Fact]
        public async Task AddCategoryToPodcastAsync_UpdatesCategory_WhenPodcastExists()
        {
            // Arrange
            repo.Seed(
                new Podcast { Id = "10", Category = "OldCategory" }
            );

            // Act
            await manager.AddCategoryToPodcastAsync("10", "  Skräck  ");

            var updated = await repo.GetFeedAsync("10");

            // Debug-utskrift
            output.WriteLine($"Podcast ID: {updated?.Id}");
            output.WriteLine($"Kategori efter uppdatering: '{updated?.Category}'");

            // Assert
            Assert.NotNull(updated);
            Assert.Equal("Skräck", updated!.Category);

        }

        [Fact]
        public async Task RenameCategoryAsync_UpdatesAllMatchingCategories()
        {
            // Arrange seed
            repo.Seed(
                new Podcast { Id = "1", Category = "Tech" },
                new Podcast { Id = "2", Category = "News" },
                new Podcast { Id = "3", Category = "Tech" }
            );

            // Act
            await manager.RenameCategoryAsync("Tech", "Technology");

            // Assert + utskrift
            var p1 = await repo.GetFeedAsync("1");
            var p2 = await repo.GetFeedAsync("2");
            var p3 = await repo.GetFeedAsync("3");

            
            output.WriteLine($"Podcast 1 kategori: {p1?.Category}");
            output.WriteLine($"Podcast 2 kategori: {p2?.Category}");
            output.WriteLine($"Podcast 3 kategori: {p3?.Category}");

            Assert.Equal("Technology", p1!.Category);
            Assert.Equal("News", p2!.Category);              
            Assert.Equal("Technology", p3!.Category);
        }
        [Fact]
        public async Task DeleteCategoryAsync_RemovesCategoryFromAllMatchingPodcasts()
        {
            // Arrange
            repo.Seed(
                new Podcast { Id = "1", Category = "Tech" },
                new Podcast { Id = "2", Category = "News" },
                new Podcast { Id = "3", Category = "Tech" }
            );

            // Act
            await manager.DeleteCategoryAsync("Tech");

            var p1 = await repo.GetFeedAsync("1");
            var p2 = await repo.GetFeedAsync("2");
            var p3 = await repo.GetFeedAsync("3");

            // Debug-output
            
            output.WriteLine($"Podcast 1 kategori: {(p1?.Category == null ? "null" : p1.Category)}");
            output.WriteLine($"Podcast 2 kategori: {(p2?.Category == null ? "null" : p2.Category)}");
            output.WriteLine($"Podcast 3 kategori: {(p3?.Category == null ? "null" : p3.Category)}");

            Assert.Null(p1!.Category);
            Assert.Equal("News", p2!.Category);
            Assert.Null(p3!.Category);

        }


    }
}

