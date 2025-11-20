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
    public class PodcastManager
    {

        private readonly IPodcastRepository podcastDataService;

        /// <summary>
        /// Initiates a PodcastManager object with the service usages of the passed argument.
        /// </summary>
        /// <param name="podcastDataService">The service to use</param>
        /// <exception cref="ArgumentNullException">If service was null</exception>
        public PodcastManager(IPodcastRepository podcastDataService)
        {
            // Create data layer object pass connetion string
            this.podcastDataService = podcastDataService ?? throw new ArgumentNullException(nameof(podcastDataService)); ;
        }

        /// <summary>
        /// Returns all the saved podcast feeds.
        /// </summary>
        /// <exception cref="DataAccessException">If the connection fails.</exception>
        /// <returns>The users saved podcasts mapped to feeds</returns>
        public List<PodFlow> GetAllFeeds()
        {

            var feeds = podcastDataService.GetAllFeeds();
            // Insert all the podcast 
            foreach (var i in feeds)
            {
                i.Podcasts = podcastDataService.GetPodcasts(i);
            }

            return feeds;
        }


        /// <summary>
        /// Deletes the given podcast model.
        /// </summary>
        /// <exception cref="DataAccessException">If the connection fails.</exception>
        /// <param name="pod">The podcast to delete from storage.</param>
        public void DeletePod(PodModel pod)
        {
            podcastDataService.DeletePod(pod);

            var feed = podcastDataService.GetFeed(pod.PodcastFeedId);
            // If feed is empty remove it
            if (feed != null && feed.Podcasts.Count == 0)
            {
                podcastDataService.DeleteFeed(feed);
            }
        }

        /// <summary>
        /// Saves the podcast to a matching feed.
        /// </summary>
        /// <exception cref="DataAccessException">If the connection fails.</exception>
        /// <param name="pod">The podcast to save.</param>
        public void PushPod(PodModel pod, PodFlow feed)
        {
            var feeds = podcastDataService.GetAllFeeds();

            // if no feed matches the given feed
            if (!feeds.Any(f => f.Id == feed.Id))
            {
                podcastDataService.PushFeed(feed);
            }

            podcastDataService.PushPod(pod);
        }

        /// <summary>
        /// Moves a podcast model to a new feed, removing it from the source feed.
        /// Will also remove the source feed if its empty.
        /// </summary>
        /// <exception cref="DataAccessException">If the connection fails.</exception>
        /// <param name="model">The podcast to move.</param>
        /// <param name="feed">The feed to move the podcast to.</param>
        public void MovePod(PodModel model, PodFlow feed)
        {
            var prevFeed = model.PodcastFeedId;
            model.PodcastFeedId = feed.Id;
            podcastDataService.UpdateModel(model);
            PushPod(model, feed);

            // delete on empty
            PodFlow? prev = podcastDataService.GetFeed(prevFeed);
            if(prev?.Podcasts.Count == 0)
            {
                DeletePodFeed(prev);
            }
        }

        /// <summary>
        /// Deletes an entire feed and its contents.
        /// </summary>
        /// <exception cref="DataAccessException">If the connection fails.</exception>
        /// <param name="feed">The feed to delete</param>
        public void DeletePodFeed(PodFlow feed)
        {
            // Delete all containing episodes of that feed
            feed.Podcasts.Clear();

            podcastDataService.DeleteFeed(feed);
        }
    }
}
