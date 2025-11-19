using pod_app.BusinessLogicLayer;
using pod_app.Models;
using pod_app.PresentationLayer.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ObservableCollection<PodView> resultsList;
        public ObservableString PodcastImageUrl { get; set; }
        public ObservableString PodcastTitle { get; set; }

        private PodFlow? currentPodcastFeed;
        public ObservableCollection<PodView> ResultsList
        {
            get { return resultsList; }
            set { resultsList = value; }
        }

        public HomePage()
        {
            InitializeComponent();
            resultsList = new ObservableCollection<PodView>();
            PodcastImageUrl = new();
            PodcastTitle = new();
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
            resultsList.Clear();

            try
            {
                // Start search
                var xmlStr = await RssUtilHelpers.GetRssXMLFile(query);

                // Parse on background thread
                var feed = await Task.Run(() => RssUtilHelpers.GetPodFeedFromXML(xmlStr));

                PodcastImageUrl.Value = feed.ImageUrl;
                PodcastTitle.Value = feed.Category;

                if (feed.Podcasts.Count == 0)
                {
                    MessageBox.Show("No results");
                    return;
                }

                // Create all episode views
                foreach (var item in feed.Podcasts)
                {
                    ResultsList.Add(new PodView(PodView_LikeClicked, item));
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                // TODO: Prompt error message
            }
        }

        private void PodView_LikeClicked(object sender, EventArgs e)
        {
            var p = sender as PodView;
            
            if(p is not null && MainWindow.podcastManager is not null && currentPodcastFeed is not null)
            {
                MainWindow.podcastManager.PushPod(p.PodModel, currentPodcastFeed);
            }
            else
            {
                // TODO: Ask user for connection string
            }

        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
