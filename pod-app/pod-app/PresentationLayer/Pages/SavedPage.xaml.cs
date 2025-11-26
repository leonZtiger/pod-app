using pod_app.BusinessLogicLayer;
using pod_app.Models;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace pod_app.PresentationLayer.Pages
{
    public partial class SavedPage : Page
    {
        private readonly PodcastManagerAsync _manager;
        private readonly Frame? _parentFrame;

        public SavedPage(PodcastManagerAsync manager, Frame parentFrame)
        {
            InitializeComponent();
            _manager = manager;
            _parentFrame = parentFrame;

            LoadPodFlows();
        }

        private async void LoadPodFlows()
        {
            try
            {
                var feeds = await _manager.GetAllFeedsAsync();
                PodListControl.ItemsSource = feeds;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kunde inte hämta sparade poddar: " + ex.Message);
            }
        }

        private void OnFeedClicked(object sender, MouseButtonEventArgs e)
        {
            var feed = (sender as FrameworkElement)?.DataContext as Podcast;
            if (feed == null) return;

            feed.IsExpanded = !feed.IsExpanded;
            PodListControl.Items.Refresh();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            _parentFrame?.Navigate(MainWindow.homePage);
        }
    }
}
