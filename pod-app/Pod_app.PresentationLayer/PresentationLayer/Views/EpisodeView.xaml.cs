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
        public bool IsExpanded { get; set; }
        
        public EpisodeView()
        {
            InitializeComponent();
        }

        // Event for toggle the size of the view
        private void EpisodeView_ToggleEpisodeExpanded(object sender, MouseButtonEventArgs e)
        {
            ToggleExpanded();
        }

        private void ToggleExpanded()
        {
            IsExpanded = !IsExpanded;

            // Expands the EpisodeView so the entire title and description shows
            if (IsExpanded)
            {
                RootBorder.Height = Double.NaN; 
                DescriptionText.TextTrimming = TextTrimming.None;
                DescriptionText.MaxHeight = double.PositiveInfinity; // No height limit
            }
            // Shorinks the EpisodeView 
            else
            {
                RootBorder.Height = 120; 
                DescriptionText.MaxHeight = 40;
            }
        }

        /// <summary>
        /// Expands the whole usercontrol, to alow all content to be seen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
