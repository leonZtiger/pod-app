using pod_app.BusinessLogicLayer;
using pod_app.DataLayer;
using pod_app.Models;
using pod_app.PresentationLayer.Views;
using pod_app.Service;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;



namespace pod_app.PresentationLayer.Pages
{
    /// <summary>
    /// Interaction logic for SavedPage.xaml
    /// </summary>
    public partial class SavedPage : Page
    {
        private readonly PodcastManager _manager;
        private readonly Frame? parentFrame;


        public SavedPage(PodcastManager manager)

        {
            InitializeComponent();
            _manager = manager;
            LoadPodFlows();

        }


        public SavedPage( PodcastManager manager, Frame parentFrame) : this(manager)
        {
            this.parentFrame = parentFrame;
        }




        private void LoadPodFlows()
        {
            try
            {
                var feeds = _manager.GetAllFeeds();  // HÄR HÄMTAR VI FRÅN DATABASEN
                PodListControl.ItemsSource = feeds;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kunde inte hämta sparade poddar: " + ex.Message);
            }
        }






        private void OnFeedClicked(object sender, MouseButtonEventArgs e)
        {
            var feed = (sender as FrameworkElement)?.DataContext as PodFlow;
            if (feed == null) return;

            feed.IsExpanded = !feed.IsExpanded;
            PodListControl.Items.Refresh();
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

