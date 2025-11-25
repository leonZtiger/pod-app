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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static HomePage? homePage = null;
        public static SavedPage? savedPage = null;
        public static PodcastManagerAsync? podcastManager;
        public MainWindow()
        {
            InitializeComponent();

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