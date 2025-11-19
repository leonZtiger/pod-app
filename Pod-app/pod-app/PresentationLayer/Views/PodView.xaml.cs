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
    public partial class PodView : UserControl, INotifyPropertyChanged
    {
        private PodModel podModel;
        public PodModel PodModel
        {
            get { return podModel; }
            set
            {
                if (podModel != value)
                {
                    podModel = value;
                    OnPropertyChanged(nameof(PodModel));
                }
            }
        }

        private event EventHandler LikeClicked;
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Only used to initialize for tests. For actual use, please use <code> PodView(EventHandler LikeClicked, PodModel podModel)</code>.
        /// </summary>
        public PodView()
        {
            InitializeComponent();
        }

        public PodView(EventHandler LikeClicked, PodModel podModel)
        {
            InitializeComponent();
            this.LikeClicked = LikeClicked;
            this.podModel = podModel;
            DataContext = this;
        }


        private void OnLikeClicked(object sender, RoutedEventArgs e)
        {
            PodModel.IsSaved = !PodModel.IsSaved;
            OnPropertyChanged(nameof(PodModel));

            LikeClicked?.Invoke(this, EventArgs.Empty);
        }
        protected void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private void OnOpenLinkClicked(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(podModel.URL))
            {
                return;
            }
            Process.Start(new ProcessStartInfo
            {
                FileName = podModel.URL,
                UseShellExecute = true
            });
        }
    }
}
