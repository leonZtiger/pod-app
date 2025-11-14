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

namespace pod_app.PresentationLayer.Pages
{
    /// <summary>
    /// Interaction logic for SavedPage.xaml
    /// </summary>
    public partial class SavedPage : Page
    {
        public SavedPage()
        {
            InitializeComponent();
        }

            private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            btnFilter.ContextMenu.IsOpen = true;
        }

        // När man väljer en kategori i menyn
        private void FilterItem_Click(object sender, RoutedEventArgs e)
        {
            // Filtreringslogik

        }
          
        

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            // Gå tillbaka till HomePage

        }
    }
}
