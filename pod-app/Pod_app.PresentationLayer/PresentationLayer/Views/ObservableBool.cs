using System.ComponentModel;

namespace pod_app.PresentationLayer.Pages
{
    /// <summary>
    /// Class used to create booleans that are update dynamicly in XAML.
    /// </summary>
    public class ObservableBool : INotifyPropertyChanged
    {

        private bool value;

        public bool Value
        {
            get => value;
            set
            {
                // Skip unnecessary re-renders
                if (this.value != value)
                {
                    this.value = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
            
        public ObservableBool(bool initial)
        {
            value = initial;
        }
    }
}