using System.Net;
using System.Net.Http;
using System.Xml;
using System.Xml.Linq;
using pod_app.Models;

namespace pod_app.BusinessLogicLayer
{
    internal static class RssUtilHelpers
    {


        public static async Task<PodFlow> GetPodFeedTask(string rss_url)
        {
            // Send get request to the rss url
            HttpResponseMessage response = await (new HttpClient()).GetAsync(rss_url);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Could not get rss document from server.");
            }

            // Parse the xml code received 
            string responseBody = await response.Content.ReadAsStringAsync();

            XDocument xmlReader = XDocument.Parse(responseBody);
            PodFlow podFeed = new PodFlow();

            // Read all items
            if (xmlReader.Root != null)
                // Create a podcast model per item
                foreach (var item in xmlReader.Root.Elements("item"))
                {
                    string? title = item.Element("title")?.Value;
                    string? desc = item.Element("description")?.Value;
                    string? image = item.Element("image")?.Value;
                    string? release = item.Element("pubDate")?.Value;
                    string? duration = item.Element("duration")?.Value;
                    string? url = item.Element("link")?.Value;
                    string? category = item.Element("category")?.Value;

                    podFeed.Pods.Add(MapStrings(title, desc, image, release, duration, url, category));
                }



            // Create feed of all episodes



            return podFeed;
        }

        private static PodModel MapStrings(string? title, string? desc, string? image, string? release, string? duration, string? url, string? category)
        {
            PodModel pod = new PodModel(
            title ?? "Unknown",
             = desc ?? "No description";
             = image ?? "";
             = release != null ? DateTime.Parse(release) : DateTime.MinValue;
             = duration ?? "Unknown";
              category ?? "Unknown";
             url ?? "";
             false
);
            return pod;
        }
    }
}