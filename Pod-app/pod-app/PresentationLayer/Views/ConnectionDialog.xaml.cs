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


namespace pod_app.PresentationLayer.Views
{
    /// <summary>
    /// Interaction logic for ConnectionDialog.xaml
    /// </summary>
    public partial class ConnectionDialog : Window
    {
        public ObservableString ErrorMsg { get; set; }

        private string input = "";
        public string Input { get { return input; } set { input = value; ErrorMsg.Value = ""; } }

        public ConnectionDialog()
        {
            ErrorMsg = new();
            InitializeComponent();
            this.DataContext = this;
        }

        private async void Connect_Click(object sender, RoutedEventArgs e)
        {
            // Try to create new manager
            try
            {
                MainWindow.podcastManager = new(new MongoDbRepositoryAsync(Input));
                await MainWindow.podcastManager.GetAllFeedsAsync();
                // Store new connection string
                Properties.Settings.Default.ConnectionString = Input;
                Properties.Settings.Default.Save();

                Close();
            }
            catch (Exception ex)
            {
                if (ex is MongoAuthenticationException)
                {
                    ErrorMsg.Value = "Fel inlogg";
                }
                else if (ex is MongoExecutionTimeoutException)
                {
                    ErrorMsg.Value = "Hittade ej Databasen";
                }
                else if (ex is MongoConfigurationException)
                {
                    ErrorMsg.Value = "Felaktig sträng";
                }
                // Failed to get feed, ignored can fail. Means that the user was authenticated still.
                else if (ex is XmlException)
                {
                    Close();
                }
                else
                {
                    ErrorMsg.Value = "Ett fel inträffade";
                }
            }
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.podcastManager = new(new PodcastRepositoryInMemoryAsync());
            Close();
        }
    }
}
