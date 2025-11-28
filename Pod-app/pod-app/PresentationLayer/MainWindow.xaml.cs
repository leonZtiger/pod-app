using pod_app.BusinessLogicLayer;
using pod_app.DataLayer;
using pod_app.PresentationLayer.Pages;
using pod_app.PresentationLayer.Views;
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
            InitDbManager();
            this.DataContext = this;

        }

        public void InitDbManager()
        {
            var dialog = new ConnectionDialog();
            dialog.ShowDialog();   // <-- Här skapas podcastManager

            // Skapa sidorna EFTER att podcastManager finns
            homePage = new HomePage(mainFrame, podcastManager!);
            savedPage = new SavedPage(mainFrame, podcastManager!);

            mainFrame.Navigate(homePage);

            dialog.Close();
        }
    }
}
