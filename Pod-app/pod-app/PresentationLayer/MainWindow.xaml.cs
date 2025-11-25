using pod_app.BusinessLogicLayer;
using pod_app.PresentationLayer.Pages;

using pod_app.PresentationLayer.Views;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace pod_app
{
    public partial class MainWindow : Window
    {
        private PodcastManager _manager;

        public static HomePage? homePage = null;
        public static SavedPage? savedPage = null;
        public static PodcastManager? podcastManager;
        public MainWindow()
        {
            InitializeComponent();

            // 1. skapa manager
            _manager = new PodcastManager(new PodcastServiceInMemory());

            // 2. skapa pages med rätt argument
            homePage = new HomePage(mainFrame);

            savedPage = new SavedPage(mainFrame);
            this.DataContext = this;
            mainFrame.Navigate(homePage);           
        }

        public static void InitDbManager()
        {

            ConnectionDialog connectionDialog = new ConnectionDialog();
            connectionDialog.ShowDialog();
            connectionDialog.Close();
        }
    }
}
