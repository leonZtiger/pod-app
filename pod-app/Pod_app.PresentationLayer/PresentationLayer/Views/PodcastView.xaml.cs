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
using System.Windows.Navigation;
using System.Windows.Shapes;
using pod_app.Models;

namespace pod_app.PresentationLayer.Views
{
    /// <summary>
    /// Interaction logic for PodcastView.xaml
    /// </summary>
    public partial class PodcastView : UserControl
    {
        public event EventHandler DeleteClicked;
        public event EventHandler EditClicked;
        public event EventHandler CategoryClicked;

        public PodcastView()
        {
            InitializeComponent();
        }


        private async void OnDeletePodcast_Click(object sender, RoutedEventArgs e)
        {
            if (DeleteClicked is not null)
                DeleteClicked(this, EventArgs.Empty);

        }
        private async void OnEditCategory_Click(object sender, RoutedEventArgs e)
        {
            if (CategoryClicked is not null)
                CategoryClicked(this, EventArgs.Empty);

        }

        private async void OnRenamePodcast_Click(object sender, RoutedEventArgs e)
        {
            if (EditClicked is not null)
                EditClicked(this, EventArgs.Empty);

        }

    }
}