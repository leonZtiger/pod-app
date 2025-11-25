using pod_app.BusinessLogicLayer;
using pod_app.PresentationLayer.Pages;
using pod_app.Service;
using System.Windows;

namespace pod_app
{
    public partial class MainWindow : Window
    {
        private PodcastManager _manager;

        public static HomePage? homePage = null;
        public static SavedPage? savedPage = null;

        public MainWindow()
        {
            InitializeComponent();

            // 1. skapa manager
            _manager = new PodcastManager(new PodcastServiceInMemory());

            // 2. skapa pages med rätt argument
            homePage = new HomePage(mainFrame);
            savedPage = new SavedPage(_manager, mainFrame);

            // 3. navigera
            mainFrame.Navigate(homePage);
        }
    }
}
