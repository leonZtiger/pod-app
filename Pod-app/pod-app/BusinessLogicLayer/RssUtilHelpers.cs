using pod_app.Models;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Channels;
using System.Xml;
using System.Xml.Linq;

namespace pod_app.BusinessLogicLayer
{
    public static class RssUtilHelpers
    {
        /// <summary>
        /// Asynchrounosly fetches XML file from given URL.
        /// </summary>
        /// <remarks>This method does not check if the return body is an actual XML Doc.</remarks>
        /// <param name="rss_url">The URL of the document to fetch.</param>
        /// <returns>The string representation of the document.</returns>
        /// <exception cref="HttpRequestException">Throwed if the Get requested resulted not a 200 "OK" message.</exception>
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
        /// <summary>
        /// Returns the podcast feed based on the xml string.
        /// Expects all the item tags in the xml to be a valid podcast episode.
        /// </summary>
        /// <param name="XML_str">The complete XML document in raw format.</param>
        /// <returns>The parsed Xml items to a single podcast feed.</returns>
        public static Podcast GetPodFeedFromXML(string XML_str)
        {
            XDocument xmlReader = XDocument.Parse(XML_str);

            Podcast podFeed = new Podcast();
            podFeed.Episodes = new();
            // Read all items
            if (xmlReader.Root != null)
            {
                // Used for itunes custom xml tags
                XNamespace itunes = "http://www.itunes.com/dtds/podcast-1.0.dtd";

                var channel = xmlReader.Root?.Element("channel");
                // Get show title
                podFeed.Category = channel?.Element(itunes + "category")?.Attribute("text")?.Value ?? "No category yet";
                podFeed.About = channel?.Element("description")?.Value ?? "No description found.";
                podFeed.Title = channel?.Element("title")?.Value ?? "No title yet.";

                // Get show image
                podFeed.ImageUrl = channel?
                                    .Element("image")?
                                    .Element("url")?
                                    .Value ?? "PresentationLayer\\Views\\Assets\\NoImage.png";


                // Create a podcast model per item and push to podFeed
                foreach (var item in xmlReader.Descendants("item"))
                {
                    string? title = item.Element("title")?.Value;
                    string? desc = item.Element("description")?.Value;
                    string? image = item.Element(itunes + "image")?.Attribute("href")?.Value ?? "PresentationLayer\\Views\\Assets\\NoImage.png";
                    string? release = item.Element("pubDate")?.Value;
                    string? duration = item.Element(itunes + "duration")?.Value;
                    string? url = item.Element("link")?.Value;
                    string? category = item.Element("category")?.Value;
                    string? episode = item.Element(itunes + "episode")?.Value;

                    podFeed.Episodes.Add(MapStrings(title, desc, image, release, duration, url, category, episode));
                }
            }

            return podFeed;
        }

        /// <summary>
        /// Returns a podcast model object with all its required fields sets.
        /// Handles null-cases by settting defualt values.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="desc"></param>
        /// <param name="image"></param>
        /// <param name="release"></param>
        /// <param name="duration"></param>
        /// <param name="url"></param>
        /// <param name="category"></param>
        /// <returns>The created podcast model object</returns>
        public static Episode MapStrings(string? title, string? desc, string? image, string? release, string? duration, string? url, string? category, string? episode)
        {
            // Parse duration from seconds as string to string of hour:minute
            if (int.TryParse(duration, out var duration_int))
            {
                if (duration_int < TimeSpan.MaxValue.TotalSeconds)
                {
                    TimeSpan time = TimeSpan.FromSeconds(duration_int);
                    duration = time.ToString(@"hh\:mm");
                }
            }

            bool validEpisode = int.TryParse(episode, out var episodeId);

            Episode pod = new Episode()
            {
                Title = title ?? "Unknown",
                Description = desc ?? "No description",
                ImageUrl = image ?? "",
                ReleaseDate = release != null ? DateTime.Parse(release) : DateTime.MinValue,
                Duration = duration ?? "Unknown",
                Category = category ?? "Unknown",
                URL = url ?? "",
                EpisodeNum = validEpisode ? episodeId : 1
            };
            return pod;
        }

        /// <summary>
        /// Generates a id based on unique parameters with the MD5 algorithm.
        /// Url is always unique and is preffered to not be null for faster hash.
        /// </summary>
        /// <param name="title">Title of the podcast</param>
        /// <param name="desc">Description of the podcast</param>
        /// <param name="image">Image of the podcast</param>
        /// <param name="release">Release of the podcast</param>
        /// <param name="duration">Duration of the podcast</param>
        /// <param name="url">Url of the podcast</param>
        /// <param name="category">Category of the podcast</param>
        /// <returns>The hash generate by the MD5 algorithm</returns>
        public static string GeneratePodId(string? title, string? desc, string? image, string? release, string? duration, string? url, string? category)
        {
            string hash_src;
            if (url is null)
            {
                // Fields that are most likly to be unqiue
                hash_src = (title ?? "Unknown") +
               (desc ?? "No description") +
               (release != null ? DateTime.Parse(release) : DateTime.MinValue).ToString() +
               (duration ?? "Unknown");

            }
            // Url is always per podcast which in it self is unique
            else
            {
                hash_src = url;
            }
            // TODO: MD5 returns a 128 bit hash, but we cast it for now since c# dont support long longs.
            // This highers the risk of collisions but for now is fixed to 64 bits.
            return string.Format("{0:X}", BitConverter.ToUInt64(MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(hash_src))));
        }

        /// <summary>
        /// Check whether the format of the URL matches that of a valid xml file.
        /// </summary>
        /// <param name="url">The URL to validate.</param>
        /// <returns>True if valid url, false otherwise.</returns>
        public static bool IsvalidXmlUrl(string? url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return false;

            // Try to parse as an absolute URI
            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
                return false;

            // Only allow
            if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
                return false;

            // Check that the path ends with .xml
            /*  var ext = Path.GetExtension(uri.AbsolutePath);
              if (!ext.Equals(".xml", StringComparison.OrdinalIgnoreCase))
                  return false;
            */
            return true;
        }
    }
}