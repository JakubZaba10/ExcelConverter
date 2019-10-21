using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ExcelDBConverter
{
    public abstract class PropertyChangeModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
