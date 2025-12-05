using pod_app.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
    public partial class EpisodeView : UserControl
    {
     
        /// <summary>
        /// Only used to initialize for tests. For actual use, please use <code> PodView(EventHandler LikeClicked, PodModel podModel)</code>.
        /// </summary>
        public EpisodeView()
        {
            InitializeComponent();
        }


        private void OnOpenLinkClicked(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty((this.DataContext as Episode).URL))
            {
                return;
            }

            Process.Start(new ProcessStartInfo
            {
                FileName = (DataContext as Episode).URL,
                UseShellExecute = true
            });
        }
    }
}
