using pod_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pod_app.Service
{
    public interface IPodcastDataService
    {
        /// <summary>
        /// Returns all stored podcast feeds.
        /// </summary>
        /// <returns>List of all podcast feeds.</returns>
        /// <exception cref="TimeoutException">If the storage layer cannot be queried.</exception>
        List<Podcast> GetAllFeeds();

        /// <summary>
        /// Returns the feed that contains the given Id.
        /// </summary>
        /// <param name="id">The object id of the feed.</param>
        /// <returns>The feed, or null if not found.</returns>
        /// <exception cref="TimeoutException">If the storage layer fails.</exception>
        Podcast? GetFeed(string id);

        /// <summary>
        /// Creates a new feed and stores it.
        /// </summary>
        /// <exception cref="ArgumentNullException">If feed is null.</exception>
        /// <exception cref="InvalidOperationException">If a feed with the same Id already exists.</exception>
        /// <exception cref="TimeoutException">If writing to storage fails.</exception>
        void PushFeed(Podcast feed);

        /// <summary>
        /// Deletes the specified feed.
        /// </summary>
        /// <exception cref="ArgumentNullException">If feed is null.</exception>
        /// <exception cref="KeyNotFoundException">If the feed does not exist.</exception>
        /// <exception cref="TimeoutException">If the storage layer fails.</exception>
        void DeleteFeed(Podcast feed);
    }
}
