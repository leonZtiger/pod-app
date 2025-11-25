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
        public async Task<List<Podcast>> GetAllFeedsAsync()
        {

            var feeds = podcastDataService.GetAllFeeds();
            // Insert all the episodes per podcast 
            foreach (var i in feeds)
            {
                i.Episodes = RssUtilHelpers.GetPodFeedFromXML(await RssUtilHelpers.GetRssXMLFile(i.Url)).Episodes;
            }

            return feeds;
        }


        /// <summary>
        /// Saves the podcast. Ingores duplicates
        /// </summary>
        /// <exception cref="DataAccessException">If the connection fails.</exception>
        /// <param name="pod">The podcast to save.</param>
        public void PushPodcast(Podcast feed)
        {
            podcastDataService.PushFeed(feed);
        }

        /// <summary>
        /// Deletes an entire feed.
        /// </summary>
        /// <exception cref="DataAccessException">If the connection fails.</exception>
        /// <param name="feed">The feed to delete</param>
        public void DeletePodcastFeed(Podcast feed)
        {
            // Delete all containing episodes of that feed
            feed.Episodes.Clear();

            podcastDataService.DeleteFeed(feed);
        }
    }
}
