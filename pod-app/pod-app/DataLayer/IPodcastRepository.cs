using pod_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pod_app.DataLayer
{
    public interface IPodcastRepository
    {
        /// <summary>
        /// Returns all stored podcast feeds.
        /// </summary>
        /// <returns>List of all podcast feeds.</returns>
        /// <exception cref="TimeoutException">If the storage layer cannot be queried.</exception>
        List<PodFlow> GetAllFeeds();

        /// <summary>
        /// Returns the feed that contains the given Id.
        /// </summary>
        /// <param name="id">The object id of the feed.</param>
        /// <returns>The feed, or null if not found.</returns>
        /// <exception cref="TimeoutException">If the storage layer fails.</exception>
        PodFlow? GetFeed(string id);

        /// <summary>
        /// Returns the podcast list of the given feed.
        /// </summary>
        /// <param name="feed">The the feed.</param>
        /// <returns>List of all podcasts.</returns>
        /// <exception cref="TimeoutException">If the storage layer fails.</exception>
        List<PodModel> GetPodcasts(PodFlow feed);
        /// <summary>
        /// Creates a new feed and stores it.
        /// </summary>
        /// <exception cref="ArgumentNullException">If feed is null.</exception>
        /// <exception cref="InvalidOperationException">If a feed with the same Id already exists.</exception>
        /// <exception cref="TimeoutException">If writing to storage fails.</exception>
        void PushFeed (PodFlow feed);

        /// <summary>
        /// Deletes the specified feed.
        /// </summary>
        /// <exception cref="ArgumentNullException">If feed is null.</exception>
        /// <exception cref="KeyNotFoundException">If the feed does not exist.</exception>
        /// <exception cref="TimeoutException">If the storage layer fails.</exception>
        void DeleteFeed(PodFlow feed);

        /// <summary>
        /// Saves a podcast episode.
        /// </summary>
        /// <exception cref="ArgumentNullException">If pod is null.</exception>
        /// <exception cref="KeyNotFoundException">If the feed does not exist.</exception>
        /// <exception cref="InvalidOperationException">If an episode with the same Id already exists.</exception>
        /// <exception cref="TimeoutException">If writing to storage fails.</exception>
        void PushPod(PodModel pod);

        /// <summary>
        /// Deletes a podcast episode.
        /// </summary>
        /// <exception cref="ArgumentNullException">If pod is null.</exception>
        /// <exception cref="KeyNotFoundException">If the episode does not exist.</exception>
        /// <exception cref="TimeoutException">If the storage layer fails.</exception>
        void DeletePod(PodModel pod);

        /// <summary>
        /// Returns the podcast with matching id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The found podcast object with matching id.</returns>
        /// <exception cref="TimeoutException">If the storage layer fails.</exception>
        PodModel? GetPodcast(string id);

        /// <summary>
        /// Updates all the fields of a podcast
        /// </summary>
        /// <exception cref="TimeoutException">If the storage layer fails.</exception>
        /// <exception cref="KeyNotFoundException">If the episode does not exist.</exception>
        /// <param name="model"></param>
        void UpdateModel(PodModel model);
    }
}
