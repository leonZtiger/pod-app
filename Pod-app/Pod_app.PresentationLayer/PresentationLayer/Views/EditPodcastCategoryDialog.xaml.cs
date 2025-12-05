using pod_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace pod_app.PresentationLayer.Views
{
    public partial class EditPodcastCategoryDialog : Window
    {
        private readonly Podcast _podcast;
        private readonly IEnumerable<string> _categories;

        public string? SelectedCategory => CategoryComboBox.SelectedItem as string;

        public EditPodcastCategoryDialog(Podcast podcast, IEnumerable<string> categories)
        {
            InitializeComponent();

            if (podcast == null)
                throw new ArgumentNullException(nameof(podcast));

            if (categories == null)
                throw new ArgumentNullException(nameof(categories));

            _podcast = podcast;
            _categories = categories;

            CategoryComboBox.ItemsSource = _categories.ToList();
            CategoryComboBox.SelectedItem = _podcast.Category;    
        }

        /// <summary>
        /// Saves the new category if any.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedCategory == null)
            {
                MessageBox.Show("Du måste välja en kategori.");
                return;
            }

            DialogResult = true;
            Close();
        }

        /// <summary>
        /// Cancels the dialog & terminates the dialog.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
