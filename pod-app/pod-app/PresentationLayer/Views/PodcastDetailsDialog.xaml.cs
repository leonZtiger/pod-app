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
using pod_app.PresentationLayer.Validation;


namespace pod_app.PresentationLayer.Views
{
    public partial class PodcastDetailsDialog : Window
    {
        public Podcast CurrentFeed { get; }

        public PodcastDetailsDialog(Podcast feed)
        {
            InitializeComponent();

            CurrentFeed = feed ?? throw new ArgumentNullException(nameof(feed));

            TitleTextBox.Text = feed.Title;     // Suggests the Title as the podcast name
            CategoryTextBox.Text = feed.Genre;  // Suggests the Genre as the podcast category

        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            // Validates the user input
            var validation = Validator.ValidatePodcastForm(
             TitleTextBox.Text,
             CategoryTextBox.Text);

            if (!validation.IsValid)
            {
                MessageBox.Show(validation.ErrorMessage);
                return;
            }

            if (MainWindow.podcastManager is null)  // Sends the user to the connectionDialog pop up if theres no connection to MongoDB
                MainWindow.InitDbManager();

            if (MainWindow.podcastManager is null)
            {
                MessageBox.Show("Ingen databasanslutning kunde skapas.");
                return;
            }

            if (MainWindow.homePage is not null)
            {
                MainWindow.homePage.UpdateConnectButtonUI();  // Changes the status for the Connect Button
            }

             CurrentFeed.Title = TitleTextBox.Text;              // sets the new name for the Podcast and its category
             CurrentFeed.Category = CategoryTextBox.Text;

            try
            {
                await MainWindow.podcastManager.PushFeedAsync(CurrentFeed); // Pushes the Podcast to the database and then closes the pop up
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

          


