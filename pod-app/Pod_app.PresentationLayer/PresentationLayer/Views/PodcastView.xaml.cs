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
        public PodcastView()
        {
            InitializeComponent();
        }


        private async void OnDeletePodcast_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as FrameworkElement)?.DataContext is not Podcast pod)
                return;

            var confirm = MessageBox.Show(
                $"Vill du ta bort '{pod.Title}'?",
                "Bekräfta borttagning",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirm != MessageBoxResult.Yes)
                return;

            await MainWindow.podcastManager?.DeleteFeedAsync(pod);
            // RefreshSavedFeeds();
        }
        private async void OnEditCategory_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as FrameworkElement)?.DataContext is not Podcast pod)
                return;

            var categories = await MainWindow.podcastManager.GetAllCategoriesAsync();

            var dlg = new EditPodcastCategoryDialog(pod, categories)
            {
                Owner = Window.GetWindow(this)
            };

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                string newCategory = dlg.SelectedCategory;
                await MainWindow.podcastManager.AddCategoryToPodcastAsync(pod.Id, newCategory);
                pod.Category = newCategory;
            }
        

        }

        private async void OnRenamePodcast_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as FrameworkElement)?.DataContext is not Podcast pod)
                return;

            string newName = Microsoft.VisualBasic.Interaction.InputBox(
                "Nytt namn:", "Byt namn", pod.Title);

            if (string.IsNullOrWhiteSpace(newName))
                return;

            pod.Title = newName.Trim();

            // Spara i databasen
            await MainWindow.podcastManager?.UpdateFeedAsync(pod);

            // Ladda om listan så UI uppdateras
            //   RefreshSavedFeeds();
        }

    }
}