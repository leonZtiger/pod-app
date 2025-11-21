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
using MongoDB.Driver;
using pod_app.Service;

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

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            // Try to create new manager
            try
            {
                MainWindow.podcastManager = new(new PodcastServiceMongoDb(Input));
                // Test if connection is alive
                MainWindow.podcastManager.GetAllFeedsAsync();
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
                else
                {
                    ErrorMsg.Value = "Ett fel inträffade";
                }
            }
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.podcastManager = new(new PodcastServiceInMemory());
            Close();
        }
    }
}
