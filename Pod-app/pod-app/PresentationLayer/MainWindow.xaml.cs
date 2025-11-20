using pod_app.BusinessLogicLayer;
using pod_app.PresentationLayer.Pages;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace pod_app
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static HomePage? homePage = null;
        public static SavedPage? savedPage = null;
        public static PodcastManager? podcastManager;
        public MainWindow()
        {
            InitializeComponent();

            homePage = new HomePage(mainFrame);
            savedPage = new SavedPage(mainFrame);

            mainFrame.Navigate(homePage);
        }
    }
}