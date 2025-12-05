using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pod_app.PresentationLayer
{
    /// <summary>
    /// Class used to create strings that are update dynamicly in XAML.
    /// </summary>
    public class ObservableString : INotifyPropertyChanged
    {
        private string? value;

        public string? Value
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

        /// <summary>
        /// Contructs from initial value.
        /// </summary>
        /// <param name="initial"></param>
        public ObservableString(string? initial = null)
        {
            value = initial;
        }
        
        public override string? ToString() => Value;
    }
}
