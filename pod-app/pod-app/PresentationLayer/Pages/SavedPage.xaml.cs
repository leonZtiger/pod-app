using pod_app.BusinessLogicLayer;
using pod_app.Models;
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
            _ = BuildCategoryMenuAsync();
        
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

        public async void RefreshSavedFeeds()
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

        //  DELETE PODCAST

        private async void OnDeletePodcast_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as FrameworkElement)?.DataContext is not Podcast pod)
                return;

            var confirm = MessageBox.Show(
                $"Vill du ta bort '{pod.Title}'?",
                "Bekräfta borttagning",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirm != MessageBoxResult.Yes)
                return;

            await _manager.DeleteFeedAsync(pod);
            RefreshSavedFeeds();
        }





        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            parentFrame.Navigate(MainWindow.homePage);
        }

        private async void FilterItem_Click(object sender, RoutedEventArgs e)
        {
            var item = sender as MenuItem;
            if (item == null) return;

            string category = item.Header.ToString();
            var allFeeds = await _manager.GetAllFeedsAsync();

            if (category == "Alla")
                PodListControl.ItemsSource = allFeeds;
            else
                PodListControl.ItemsSource = allFeeds
                    .Where(f => f.Category == category)
                    .ToList();
        }



        private async Task BuildCategoryMenuAsync()
        {
            filterMenu.Items.Clear();

            // Always include "Alla"
            var allItem = new MenuItem { Header = "Alla" };
            allItem.Click += FilterItem_Click;
            filterMenu.Items.Add(allItem);

            var feeds = await _manager.GetAllFeedsAsync();
            var categories = feeds
                .Where(f => !string.IsNullOrWhiteSpace(f.Category))
                .Select(f => f.Category.Trim())
                .Distinct()
                .OrderBy(c => c);

            foreach (var cat in categories)
            {
                var item = new MenuItem { Header = cat };
                item.Click += FilterItem_Click;
                filterMenu.Items.Add(item);
            }
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            btn.ContextMenu.PlacementTarget = btn;
            btn.ContextMenu.IsOpen = true;
        }

        private async void OnEditCategory_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as FrameworkElement)?.DataContext is not Podcast pod) return;

            string newCat = Microsoft.VisualBasic.Interaction.InputBox(
                "Ny kategori:", "Byt kategori", pod.Category);

            if (string.IsNullOrWhiteSpace(newCat))
                return;

            await _manager.AddCategoryToPodcastAsync(pod.Id, newCat);
            RefreshSavedFeeds();


        }

        private async void OnRenamePodcast_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as FrameworkElement)?.DataContext is not Podcast pod)
                return;

            string newName = Microsoft.VisualBasic.Interaction.InputBox(
                "Nytt namn:", "Byt namn", pod.Title);

            if (string.IsNullOrWhiteSpace(newName))
                return;

            pod.Title = newName.Trim();

            // Spara i databasen
            await _manager.UpdateFeedAsync(pod);

            // Ladda om listan så UI uppdateras
            RefreshSavedFeeds();
        }



    }
}