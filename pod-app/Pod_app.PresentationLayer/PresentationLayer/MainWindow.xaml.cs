using pod_app.BusinessLogicLayer;
using pod_app.DataLayer;
using pod_app.PresentationLayer.Pages;
using pod_app.PresentationLayer.Views;
using System;
using System.Windows;

namespace pod_app
{
    public partial class MainWindow : Window
    {
        private HomePage? homePage = null;
        private SavedPage? savedPage = null;
        public static PodcastManagerAsync? podcastManager;

        public MainWindow()
        {
            InitializeComponent();

            // Load connection string
            string con_str = Properties.Settings.Default.ConnectionString; 

            // If on exist, try to connect to database
            if (!string.IsNullOrWhiteSpace(con_str)) 
            {
                try
                {
                    podcastManager = new PodcastManagerAsync(new MongoDbRepositoryAsync(con_str));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Föregående sessionens Connection-string misslyckades. Vänligen återanslut.\n\n" +
                        ex.Message);

                    podcastManager = null; 
                }
            }

            // Else if failed, prompt user
            if (podcastManager is null)
            {
                InitDbManager();
            }

            
            homePage = new HomePage(this);
            savedPage = new SavedPage(this);

            this.DataContext = this;
            mainFrame.Navigate(homePage);
        }

        /// <summary>
        /// Initiates the database manager by prompting the user for a connection-string.
        /// </summary>
        public static void InitDbManager()
        {
            var dialog = new ConnectionDialog();
            dialog.ShowDialog();
            dialog.Close();
            
        }

        public void GoToSaved()
        {
            mainFrame.Navigate(savedPage);
        }

        public void GoToHome()
        {
            mainFrame.Navigate(homePage);
        }
    }
}
