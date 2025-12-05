using pod_app.BusinessLogicLayer;
using pod_app.Models;
using pod_app.PresentationLayer.Validation;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace pod_app.PresentationLayer.Views
{
    public partial class CategoryManagerDialog : Window
    {
        private readonly PodcastManagerAsync _manager;
        private readonly ObservableCollection<string> _categories = new();
        private readonly ObservableCollection<Podcast> _podcasts = new();

        public CategoryManagerDialog(PodcastManagerAsync manager)
        {
            InitializeComponent();
            _manager = manager ?? throw new ArgumentNullException(nameof(manager));  

            CategoryList.ItemsSource = _categories;
           

            Loaded += CategoryManagerDialog_Loaded;                           // event when the categories load
            CategoryList.SelectionChanged += CategoryList_SelectionChanged;   // event when the user selects an cateogory
        }

        // Loads the categories and podcasts
        private async void CategoryManagerDialog_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadCategoriesAsync();
            await LoadPodcastsAsync();
        }

        private async Task LoadCategoriesAsync()
        {
            _categories.Clear();
            var cats = await _manager.GetAllCategoriesAsync();
            foreach (var c in cats)
                _categories.Add(c);
        }

        private async Task LoadPodcastsAsync()
        {
            _podcasts.Clear();
            var feeds = await _manager.GetAllFeedsAsync();
            foreach (var p in feeds)
                _podcasts.Add(p);
        }

        // Updates the textbox with the chosen category  
        private void CategoryList_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (CategoryList.SelectedItem is string selected)
                CategoryNameTextBox.Text = selected;
        }

        // Method for adding a new category and selecting a podcast to use it
        private async void AddCategoryToPodcast_Click(object sender, RoutedEventArgs e)
        {
            // Makes sure the user has podcasts saved
            try
            {
                var feeds = await _manager.GetAllFeedsAsync();
                if (feeds == null || feeds.Count == 0)
                {
                    MessageBox.Show("Det finns inga poddar att kategorisera.");
                    return;
                }

                // sends the user to the add category dialog/pop-up
                var dialog = new AddCategoryDialog(feeds);
                dialog.Owner = this;

                // If the user cancels there is no changes 
                var result = dialog.ShowDialog();
                if (result != true || dialog.SelectedPodcast == null)
                    return;

                // Formal validation of the name
                var validation = Validator.ValidateCategoryName(dialog.CategoryName);
                if (!validation.IsValid)
                {
                    MessageBox.Show(validation.ErrorMessage);
                    return;
                }

                var trimmed = dialog.CategoryName.Trim();

                // adds the category to the chosen podcast and updates the UI
                await _manager.AddCategoryToPodcastAsync(dialog.SelectedPodcast.Id, trimmed);
                await LoadCategoriesAsync();

                MessageBox.Show("Kategorin har lagts till på podden.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kunde inte lägga till kategori: " + ex.Message);
            }
        }

        // Method for changing a category name and updating the name for the podcasts using it
        private async void RenameCategory_Click(object sender, RoutedEventArgs e)
        {
            var oldCategory = CategoryList.SelectedItem as string;
            var newCategory = CategoryNameTextBox.Text;

            // Formal validation
            var validation = Validator.ValidateRenameCategory(oldCategory, newCategory);
            if (!validation.IsValid)
            {
                MessageBox.Show(validation.ErrorMessage);
                return;
            }

            try
            {
                await _manager.RenameCategoryAsync(oldCategory!, newCategory!.Trim());  // RenameCategoryAsync from podcastManager
                await LoadCategoriesAsync();
                CategoryList.SelectedItem = newCategory.Trim();

                MessageBox.Show("Kategorin har bytt namn.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kunde inte byta namn på kategori: " + ex.Message);
            }
        }

        // Method for deleting a category
        private async void DeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            var category = CategoryList.SelectedItem as string;

            // Formal validation
            var validation = Validator.ValidateDeleteCategory(category);
            if (!validation.IsValid)
            {
                MessageBox.Show(validation.ErrorMessage);
                return;
            }

            // check if the user really wants to delete the category
            var result = MessageBox.Show(
                "Vill du ta bort kategorin från alla poddar?",
                "Bekräfta borttagning",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                await _manager.DeleteCategoryAsync(category!);  // DeleteCategoryAsync from podcastManager
                await LoadCategoriesAsync();
                CategoryNameTextBox.Clear();

                MessageBox.Show("Kategorin har tagits bort.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kunde inte ta bort kategori: " + ex.Message);
            }
        }


        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        
    }
}
