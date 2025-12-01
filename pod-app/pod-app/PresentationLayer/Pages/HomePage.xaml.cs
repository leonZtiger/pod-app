using pod_app.BusinessLogicLayer;
using pod_app.Models;
using pod_app.PresentationLayer.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace pod_app.PresentationLayer.Pages
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        private Frame parentFrame;
        public ObservableString PodcastImageUrl { get; set; }
        public ObservableString PodcastTitle { get; set; }
        public ObservableString PodcastCategory { get; set; }
        public ObservableString PodcastDescription { get; set; }

        private readonly int ItemsPerPage = 7;

        private Podcast? currentPodcastFeed;
        public ObservableCollection<Episode> ResultsList { get; set; }
        private bool isLoadingMore = false;
        private bool IsSearching { get; set; } = false;
        public HomePage()
        {
            InitializeComponent();
            ResultsList = new();
            PodcastImageUrl = new();
            PodcastTitle = new();
            PodcastCategory = new();
            PodcastDescription = new();
            this.DataContext = this;
        }

        public HomePage(Frame parentFrame) : this()
        {
            this.parentFrame = parentFrame;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            parentFrame.Navigate(MainWindow.savedPage);
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.podcastManager is null)
                MainWindow.InitDbManager();
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate text input first
            string? query = SearchBox.Text;
            if (!RssUtilHelpers.IsvalidXmlUrl(query))
            {
                // TODO: Prompt user with bad query messsage
                MessageBox.Show("Search failed");
                return;
            }

            // Clear previus results
            ResultsList.Clear();
            PodcastImageUrl.Value = "";
            PodcastTitle.Value = "";
            PodcastDescription.Value = "";
            PodcastCategory.Value = "";
            currentPodcastFeed = null;
            IsSearching = true;
            try
            {
                // Start search
                var xmlStr = await RssUtilHelpers.GetRssXMLFile(query);
                // Get podcast
                var feed = await Task.Run(() => RssUtilHelpers.GetPodFeedFromXML(xmlStr,query));

                PodcastImageUrl.Value = feed.ImageUrl;
                PodcastTitle.Value = feed.Category;
                currentPodcastFeed = feed;
                PodcastDescription.Value = feed.About;
                PodcastCategory.Value = feed.Genre;
                LoadNextPage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            IsSearching = false;
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ScrollContainer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var sv = sender as ScrollViewer;

            // only react when scrolling down
            if (e.VerticalChange <= 0) return;

            if (isLoadingMore) return;

            // if 150px near bottom preload next page
            if (sv.VerticalOffset + sv.ViewportHeight >= sv.ExtentHeight - 150)
            {
                isLoadingMore = true;

                LoadNextPage();

                isLoadingMore = false;
            }
        }

        private void LoadNextPage()
        {
            if (currentPodcastFeed is null || currentPodcastFeed.Episodes is null || currentPodcastFeed.Episodes.Count == 0) return;

            int alreadyLoaded = ResultsList.Count;
            int toTake = Math.Min(ItemsPerPage, currentPodcastFeed.Episodes.Count - alreadyLoaded);

            for (int i = 0; i < toTake; i++)
            {
                ResultsList.Add(currentPodcastFeed.Episodes[alreadyLoaded + i]);
            }
        }

        private void OnPodcastLike_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.podcastManager is null)
                MainWindow.InitDbManager();

            if (currentPodcastFeed is not null && MainWindow.podcastManager is not null)
            {
                MainWindow.podcastManager.PushFeedAsync(currentPodcastFeed);
            }

         
        private void ShowLikeToast()
        {
           
            if (currentPodcastFeed is not null)
            {
                ShowSaveToast(); // Toast animation from HomePage.XAML
            }
        }

        // Event handler to trigger the toast
        private void OnLikeToastButton_Click(object sender, RoutedEventArgs e)
        {
            ShowLikeToast();
        }

        
    


    }
}
}


