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
        public static HomePage? homePage = null;
        public static SavedPage? savedPage = null;
        public static PodcastManagerAsync? podcastManager;

        public MainWindow()
        {
            InitializeComponent();

            
            string con_str = Properties.Settings.Default.ConnectionString;  // Persistent connectionString

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

           
            if (podcastManager is null)
            {
                InitDbManager();
            }

            
            homePage = new HomePage(mainFrame, podcastManager!);
            savedPage = new SavedPage(mainFrame, podcastManager!);

            this.DataContext = this;
            mainFrame.Navigate(homePage);
        }

        public static void InitDbManager()
        {
            var dialog = new ConnectionDialog();
            dialog.ShowDialog();
            dialog.Close();
            
        }
    }
}
