using pod_app.Models;
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

namespace pod_app.PresentationLayer.Views
{
    /// <summary>
    /// Interaction logic for PodView.xaml
    /// </summary>
    public partial class PodView : UserControl
    {
        public PodView()
        {
            InitializeComponent();
        }

        public PodModel PodModel { get { return this.DataContext as PodModel; } set { this.DataContext = value; } }

        private void OnLikeClicked(object sender, RoutedEventArgs e)
        {

        }

        private void OnOpenLinkClicked(object sender, RoutedEventArgs e)
        {

        }
    }
}
