using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pod_app.Models;

namespace pod_app.BusinessLogicLayer
{
    internal class UserPodFeedLogic
    {

        private DataLayer datalayer;

        /// <summary>
        /// Initaites a UserPodFeedLogic object with a connection to the Mongo DB instance.
        /// </summary>
        /// <exception cref="Exception">Throws a exception if the connection to the instance failed.</exception>
        /// <param name="DbConnection_str">The Mongo DB connection string.</param>
        public UserPodFeedLogic(string DbConnection_str)
        {
            // Create data layer object pass connetion string
            this.datalayer = new(DbConnection_str);

            ThrowOnBadConnection();
        }

        /// <summary>
        /// Checks if the connection to the instance exist. Throws if no connection.
        /// </summary>
        /// <exception cref="Exception">The exception to be thrown when the connection fails.</exception>
        private void ThrowOnBadConnection()
        {
            if (datalayer == null || !datalayer.hasConnection())
                throw new Exception("Connection to Mongo DB instance failed.");
        }

        /// <summary>
        /// Returns all the saved podcast in a list group into correspoind feeds per category.
        /// </summary>
        /// <exception cref="Exception">The exception to be thrown when the connection fails.</exception>
        /// <returns>The users saved podcasts mapped to categories</returns>
        public List<PodFlow> GetPodFeeds()
        {
            ThrowOnBadConnection();

            List<PodFlow> podFlows = datalayer.GetAllFeeds();

            return podFlows ?? new();
        }

        /// <summary>
        /// Deletes the given podcast model.
        /// </summary>
        /// <exception cref="Exception">The exception to be thrown when the connection fails.</exception>
        /// <param name="pod">The podcast to delete from storage.</param>
        public void DeletePod(PodModel pod)
        {
            ThrowOnBadConnection();
            datalayer.DeletePod(pod.Id);
        }

        /// <summary>
        /// Saves the podcast to a matching genre.
        /// </summary>
        /// <exception cref="Exception">The exception to be thrown when the connection fails.</exception>
        /// <param name="pod">The podcast to save.</param>
        public void PushPod(PodModel pod)
        {
            ThrowOnBadConnection();

            datalayer.PushPod(pod.Id, pod.Category);
        }

        /// <summary>
        /// Deletes intaire feed and it contents.
        /// </summary>
        /// <param name="feed">The feed to delete</param>
        public void DeletePodFeed(PodFlow feed)
        {
            ThrowOnBadConnection();

        }
        /// <summary>
        /// Adds a podcast to the passed feed.
        /// </summary>
        /// <exception cref="Exception">The exception to be thrown when the connection fails.</exception>
        /// <param name="pod">The podcast to add.</param>
        /// <param name="feed">The feed to add this podcast to.</param>
        public void PushPodToFeed(PodModel pod, PodFlow feed)
        {
            ThrowOnBadConnection();

        }


    }
}
