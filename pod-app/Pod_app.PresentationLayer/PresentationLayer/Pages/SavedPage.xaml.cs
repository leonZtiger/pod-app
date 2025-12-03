using pod_app.BusinessLogicLayer;
using pod_app.Models;
using pod_app.PresentationLayer.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace pod_app.PresentationLayer.Pages
{
    public partial class SavedPage : Page
    {
        private readonly PodcastManagerAsync _manager;
        private readonly Frame parentFrame;

        public List<Episode> EpisodesList { get; set; } = new();
        private Podcast selectedPodcast;
        private int itemsToShow = 10;

        public SavedPage(Frame parentFrame, PodcastManagerAsync manager)
        {
            InitializeComponent();
            this._manager = manager;
            this.parentFrame = parentFrame;

            this.DataContext = this;

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

        public async 
        Task
RefreshSavedFeeds()
        {
            var feeds = await _manager.GetAllFeedsAsync();
            PodListControl.ItemsSource = feeds;
        }

        private void OnFeedClicked(object sender, MouseButtonEventArgs e)
        {
            var clicked = (sender as FrameworkElement)?.DataContext as Podcast;
            if (clicked == null) return;

            if (clicked == selectedPodcast && clicked.IsExpanded)
            {
                clicked.IsExpanded = false;
                selectedPodcast = null;
                PodListControl.Items.Refresh();
                return;
            }

            foreach (var p in PodListControl.ItemsSource as IEnumerable<Podcast>)
                p.IsExpanded = false;

            clicked.IsExpanded = true;
            selectedPodcast = clicked;
            itemsToShow = 10;

            UpdateEpisodeList();

            PodListControl.Items.Refresh();
        }

        private void UpdateEpisodeList()
        {
            if (selectedPodcast == null) return;

            EpisodesList = selectedPodcast.Episodes
                                          .Take(itemsToShow)
                                          .ToList();

            this.DataContext = null;
            this.DataContext = this;
        }

        private void LoadMore_Click(object sender, RoutedEventArgs e)
        {
            if (selectedPodcast == null) return;

            itemsToShow += 10;
            UpdateEpisodeList();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            parentFrame.Navigate(MainWindow.homePage);
        }

        private async void FilterItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not MenuItem item)
                return;

            string? category = item.Header?.ToString();
            if (string.IsNullOrWhiteSpace(category))
                return;

            var allFeeds = await _manager.GetAllFeedsAsync();

            if (category == "Alla")
            {
                PodListControl.ItemsSource = allFeeds;
            }
            else
            {
                PodListControl.ItemsSource = allFeeds
                    .Where(p => string.Equals(p.Category?.Trim(), category.Trim(), StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
        }


        private async void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn)
                return;

            var menu = btn.ContextMenu;
            if (menu == null)
                return;

            menu.Items.Clear();


            var categories = await _manager.GetAllCategoriesAsync();


            var allItem = new MenuItem { Header = "Alla" };
            allItem.Click += FilterItem_Click;
            menu.Items.Add(allItem);


            foreach (var cat in categories)
            {
                var item = new MenuItem { Header = cat };
                item.Click += FilterItem_Click;
                menu.Items.Add(item);
            }

            menu.PlacementTarget = btn;
            menu.IsOpen = true;
        }


        private async void CategoryHandler_Click(object sender, RoutedEventArgs e)
        {
            
                var dlg = new CategoryManagerDialog(_manager)
                {
                    Owner = Window.GetWindow(this)
                };

                dlg.ShowDialog();
        
                await RefreshSavedFeeds();





        }
    }
}