using pod_app.PresentationLayer.Views;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace pod_app.PresentationLayer.Pages
{
    /// <summary>
    /// Interaction logic for SavedPage.xaml
    /// </summary>
    public partial class SavedPage : Page
    {
        private Frame parentFrame;

      
        public SavedPage()
        {
            InitializeComponent();
        }

       
        public SavedPage(Frame parentFrame) : this()
        {
            this.parentFrame = parentFrame;
        }

        private void BtnFilter_Click(object sender, RoutedEventArgs e)
        {
            BtnFilter.ContextMenu.IsOpen = true;
        }

        private void FilterItem_Click(object sender, RoutedEventArgs e)
        {
            // Filtreringslogik
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            parentFrame.Navigate(MainWindow.homePage);
        }

        private void podViewControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}

