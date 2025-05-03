using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Storage;

namespace Kajina
{
    public partial class SettingsPageViewModel : INotifyPropertyChanged
    {
        private readonly ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public event PropertyChangedEventHandler? PropertyChanged = delegate { };

        public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool ExtraEnabled
        {
            get => (bool)localSettings.Values["ExtraEnabled"];
            set
            {
                localSettings.Values["ExtraEnabled"] = value;
                OnPropertyChanged();
            }
        }
    }
}
