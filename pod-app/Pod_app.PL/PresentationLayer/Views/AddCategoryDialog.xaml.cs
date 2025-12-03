using pod_app.Models;
using pod_app.PresentationLayer.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace pod_app.PresentationLayer.Views
{
    public partial class AddCategoryDialog : Window
    {
        private readonly IEnumerable<Podcast> _podcasts;
        public Podcast? SelectedPodcast => PodcastComboBox.SelectedItem as Podcast;
        public string CategoryName => CategoryNameTextBox.Text;

        // Constructor that gets all the saved podcasts
        public AddCategoryDialog(IEnumerable<Podcast> podcasts)
        {
            InitializeComponent();


            if (podcasts == null)
                throw new ArgumentNullException(nameof(podcasts));  // Controls that the user has atleast one saved podcast

            _podcasts = podcasts;
            PodcastComboBox.ItemsSource = _podcasts.ToList();  // Makes the saved podcast show up in the combobox
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var validation = Validator.ValidateAddCategory(CategoryName, SelectedPodcast);
            if (!validation.IsValid)
            {
                MessageBox.Show(validation.ErrorMessage);  // Validates the user input
                return;
            }

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }

}


