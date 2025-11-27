using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using MongoDB.Driver;
using pod_app.DataLayer;
using pod_app.Models;

namespace pod_app.PresentationLayer.Views
{
    public partial class PodcastDetailsDialog : Window
    {
        public Podcast CurrentFeed { get; }

        public PodcastDetailsDialog(Podcast feed)
        {
            InitializeComponent();

            CurrentFeed = feed ?? throw new ArgumentNullException(nameof(feed));

            TitleTextBox.Text = feed.Title;
            CategoryTextBox.Text = feed.Genre;  // Suggests the Genre as the podcast category

        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
           
            if (MainWindow.podcastManager is null)
                MainWindow.InitDbManager();

            if (MainWindow.podcastManager is null)
            {
                MessageBox.Show("Ingen databasanslutning kunde skapas.");
                return;
            }
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text))
            {
                MessageBox.Show("Du måste ange ett namn.");
                return;
            }

              CurrentFeed.Title = TitleTextBox.Text;
              CurrentFeed.Category = CategoryTextBox.Text;

            try
            {
                await MainWindow.podcastManager.PushFeedAsync(CurrentFeed);
                Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Kunde inte spara podden.");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {            
            Close();
        }

    }
}

          


