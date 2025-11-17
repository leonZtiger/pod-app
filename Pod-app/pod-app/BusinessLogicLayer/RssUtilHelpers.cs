using System.Net;
using System.Net.Http;
using System.Xml;
using System.Xml.Linq;
using pod_app.Models;

namespace pod_app.BusinessLogicLayer
{
    public static class RssUtilHelpers
    {

        public static async Task<string> GetRssXMLFile(string rss_url)
        {
            // Send get request to the rss url
            HttpResponseMessage response = await (new HttpClient()).GetAsync(rss_url);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Could not get rss document from server.");
            }
            return await response.Content.ReadAsStringAsync();
        }

        public static PodFlow GetPodFeedFromXML(string XML_str)
        {
            XDocument xmlReader = XDocument.Parse(XML_str);
            PodFlow podFeed = new PodFlow();
            podFeed.Pods = new();
            // Read all items
            if (xmlReader.Root != null)
                // Create a podcast model per item and push to podFeed
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

            return podFeed;
        }


        public static PodModel MapStrings(string? title, string? desc, string? image, string? release, string? duration, string? url, string? category)
        {
            PodModel pod = new PodModel()
            {
                Title = title ?? "Unknown",
                Description = desc ?? "No description",
                ImageUrl = image ?? "",
                ReleaseDate = release != null ? DateTime.Parse(release) : DateTime.MinValue,
                Duration = duration ?? "Unknown",
                Category = category ?? "Unknown",
                URL = url ?? "",
                IsSaved = false
            };
            return pod;
        }
    }
}