using pod_app.BusinessLogicLayer;
using pod_app.Models;
using Xunit.Abstractions;

namespace UnitTests;

public class RssUtilHelperTest
{

    private readonly ITestOutputHelper _output;

    public RssUtilHelperTest(ITestOutputHelper output)
    {
        _output = output;
    }


    [Fact]
    public async void TestXmlResponseFromRss()
    {
        var xml_tsk = RssUtilHelpers.GetRssXMLFile("https://feeds.megaphone.fm/GLT1412515089");

        try
        {
            string xml = await xml_tsk;

            Assert.NotNull(xml);
        }
        catch (Exception ex)
        {
            Assert.Fail();
        }

    }
    [Fact]
    public void TestPodFeedFromXML()
    {

        const string xml = @"
<root>
  <item>
    <title>Episode 1</title>
    <description>First ep desc</description>
    <image>https://example.com/img1.jpg</image>
    <pubDate>2025-06-01T10:30:00</pubDate>
    <duration>1h 00m</duration>
    <link>https://example.com/ep1</link>
    <category>Tech</category>
  </item>
  <item>
    <title>Episode 2</title>
    <description>Second ep desc</description>
    <image>https://example.com/img2.jpg</image>
    <pubDate>2025-06-02T11:00:00</pubDate>
    <duration>2h 15m</duration>
    <link>https://example.com/ep2</link>
    <category>News</category>
  </item>
</root>";

        // Act
        Podcast result = RssUtilHelpers.GetPodFeedFromXML(xml);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Episodes);
        Assert.Equal(2, result.Episodes.Count);

        var first = result.Episodes[0];
        Assert.Equal("Episode 1", first.Title);
        Assert.Equal("First ep desc", first.Description);
        Assert.Equal("https://example.com/img1.jpg", first.ImageUrl);
        Assert.Equal(DateTime.Parse("2025-06-01T10:30:00"), first.ReleaseDate);
        Assert.Equal("1h 00m", first.Duration);
        Assert.Equal("Tech", first.Category);
        Assert.Equal("https://example.com/ep1", first.URL);
        

        var second = result.Episodes[1];
        Assert.Equal("Episode 2", second.Title);
        Assert.Equal("Second ep desc", second.Description);
        Assert.Equal("https://example.com/img2.jpg", second.ImageUrl);
        Assert.Equal(DateTime.Parse("2025-06-02T11:00:00"), second.ReleaseDate);
        Assert.Equal("2h 15m", second.Duration);
        Assert.Equal("News", second.Category);
        Assert.Equal("https://example.com/ep2", second.URL);
    }

    [Fact]
    public void GetPodFeedFromXML_UsesDefaults_WhenTagsMissing()
    {
        // item with only title
        const string xml = @"
<root>
  <item>
    <title>Only title</title>
  </item>
</root>";

        // Act
        Podcast result = RssUtilHelpers.GetPodFeedFromXML(xml);

        // Assert
        Assert.Single(result.Episodes);
        var pod = result.Episodes[0];

        Assert.Equal("Only title", pod.Title);
        Assert.Equal("No description", pod.Description);
        Assert.Equal("", pod.ImageUrl);
        Assert.Equal(DateTime.MinValue, pod.ReleaseDate);
        Assert.Equal("Unknown", pod.Duration);
        Assert.Equal("Unknown", pod.Category);
        Assert.Equal("", pod.URL);
        
    }

    [Fact]
    public void MapStrings_UsesDefaults_WhenValuesAreNull()
    {
        // Act
        var result = RssUtilHelpers.MapStrings(
            title: null,
            desc: null,
            image: null,
            release: null,
            duration: null,
            url: null,
            category: null,
            episode: null
        );

        // Assert
        Assert.Equal("Unknown", result.Title);
        Assert.Equal("No description", result.Description);
        Assert.Equal("", result.ImageUrl);
        Assert.Equal(DateTime.MinValue, result.ReleaseDate);
        Assert.Equal("Unknown", result.Duration);
        Assert.Equal("Unknown", result.Category);
        Assert.Equal("", result.URL);
       
    }

    [Fact]
    public void MapStrings_ParsesReleaseDate_WhenProvided()
    {

        var releaseString = "2025-06-01T10:30:00";

        // Act
        var result = RssUtilHelpers.MapStrings(
            title: "Test",
            desc: "Desc",
            image: "img.png",
            release: releaseString,
            duration: "1h 30m",
            url: "https://example.com",
            category: "Tech",
            episode: "1"
        );

        // Assert
        Assert.Equal("Test", result.Title);
        Assert.Equal("Desc", result.Description);
        Assert.Equal("img.png", result.ImageUrl);
        Assert.Equal(DateTime.Parse(releaseString), result.ReleaseDate);
        Assert.Equal("1h 30m", result.Duration);
        Assert.Equal("Tech", result.Category);
        Assert.Equal("https://example.com", result.URL);
    }
}
