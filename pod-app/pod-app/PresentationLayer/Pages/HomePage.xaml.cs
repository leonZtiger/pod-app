using pod_app.BusinessLogicLayer;
using pod_app.Models;
using pod_app.PresentationLayer.Validation;
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
        private readonly PodcastManagerAsync _manager;
        private Frame parentFrame;
        public ObservableString PodcastImageUrl { get; set; }
        public ObservableString PodcastTitle { get; set; }
        public ObservableString PodcastCategory { get; set; }
        public ObservableString PodcastDescription { get; set; }

        private readonly int ItemsPerPage = 7;

        private Podcast? currentPodcastFeed;
        public ObservableCollection<Episode> ResultsList { get; set; }
        private bool isLoadingMore = false;
        public ObservableBool IsSearching { get; set; }

        public HomePage()
        {
            InitializeComponent();
            ResultsList = new();
            PodcastImageUrl = new();
            PodcastTitle = new();
            PodcastCategory = new();
            PodcastDescription = new();
            IsSearching = new(false);
            this.DataContext = this;
            UpdateConnectButtonUI();
        }

        public HomePage(Frame parentFrame, PodcastManagerAsync manager) : this()
        {
            this.parentFrame = parentFrame;
            _manager = manager;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            parentFrame.Navigate(new SavedPage(parentFrame, _manager));
        }

        // Changes the status of the connect button depending on the status of the MongoDB connection
        public void UpdateConnectButtonUI()
        {
            if (MainWindow.podcastManager is null)
            {
                ConnectButton.Content = "Anslut";
            }
            else
            {
                ConnectButton.Content = "Koppla från";
            }
        }


        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.podcastManager is null)
            {

                MainWindow.InitDbManager();            // Sends the user to the connectionDialog pop up if theres no connection to the database
            }
            else
            {

                MainWindow.podcastManager = null;


                Properties.Settings.Default.ConnectionString = string.Empty;    // Removing the connection by Emptying the connectionString
                Properties.Settings.Default.Save();
            }

            UpdateConnectButtonUI();  // Updates the status of the button
        }





        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // Validates the user input
            string? query = SearchBox.Text;
            var validation = Validator.ValidateSearchQuery(query);

            if (!validation.IsValid)
            {
                MessageBox.Show(validation.ErrorMessage);
                return;
            }

            // Clear previus results
            ResultsList.Clear();
            PodcastImageUrl.Value = "";
            PodcastTitle.Value = "";
            PodcastDescription.Value = "";
            PodcastCategory.Value = "";
            currentPodcastFeed = null;

            IsSearching.Value = true;
            try
            {
                // Start search
                var xmlStr = await RssUtilHelpers.GetRssXMLFile(query);
                // Get podcast
                var feed = await Task.Run(() => RssUtilHelpers.GetPodFeedFromXML(xmlStr, query));

                PodcastImageUrl.Value = feed.ImageUrl;
                PodcastTitle.Value = feed.Title;
                currentPodcastFeed = feed;
                PodcastDescription.Value = feed.About;
                PodcastCategory.Value = feed.Category;
                LoadNextPage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            IsSearching.Value = false;
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

        // Sends the user to the PodcastDetailsDialog to save the name and category 
        private void OnPodcastLike_Click(object sender, RoutedEventArgs e)
        {
            if (currentPodcastFeed is null)
                return;


            var dialog = new PodcastDetailsDialog(currentPodcastFeed);
            dialog.ShowDialog();

        }

    }
}
}


