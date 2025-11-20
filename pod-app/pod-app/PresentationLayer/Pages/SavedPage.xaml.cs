using pod_app.DataLayer;
using pod_app.PresentationLayer.Views;
using pod_app.Service;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using pod_app.Models;


namespace pod_app.PresentationLayer.Pages
{
    /// <summary>
    /// Interaction logic for SavedPage.xaml
    /// </summary>
    public partial class SavedPage : Page
    {
        private  IPodcastDataService? _service;

        private Frame? parentFrame;


        public SavedPage()

        {
            InitializeComponent();
            /// _service = new PodcastServiceMongoDb("mongodb+srv://<YOUR-CONNECTION-STRING>");
            _service = null;
            LoadPodFlows();

        }


        public SavedPage(Frame parentFrame) : this()
        {
            this.parentFrame = parentFrame;
        }


        private void LoadPodFlows()
        {
            try
            {
                if (_service == null)
                {
                    PodListControl.ItemsSource = Array.Empty<PodModel>();
                    return;
                }

                List<PodFlow> feeds = _service.GetAllFeeds();
                List<PodModel> allEpisodes = new();

                foreach (var feed in feeds)
                {
                    List<PodModel> episodes = _service.GetPodcasts(feed);
                    if (episodes != null)
                        allEpisodes.AddRange(episodes);
                }

                PodListControl.ItemsSource = allEpisodes;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kunde inte hämta poddar: {ex.Message}");
            }
        }




        private void BtnFilter_Click(object sender, RoutedEventArgs e)
        {
            BtnFilter.ContextMenu.IsOpen = true;
        }

        private void FilterItem_Click(object sender, RoutedEventArgs e)
        {
            // Filtreringslogik
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            parentFrame?.Navigate(MainWindow.homePage);
        }

       
    }
}

