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
    public partial class PodcastDetailsDialog : Window
    {
        public PodcastDetailsDialog()
        {
            InitializeComponent();

        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {

        }



        private void Cancel_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}

          


