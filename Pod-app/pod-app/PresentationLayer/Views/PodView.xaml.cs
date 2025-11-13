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
        private Models.PodModel podcast;
        public PodView()
        {
            podcast = new Models.PodModel();
            podcast.ImageUrl = "https://hips.hearstapps.com/clv.h-cdn.co/assets/16/18/gettyimages-586890581.jpg?crop=0.668xw:1.00xh;0.219xw,0";
           
            this.DataContext = podcast;
            InitializeComponent();
        }
    }
}
