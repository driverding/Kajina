
using Microsoft.UI.Xaml.Controls;
using Windows.ApplicationModel;

namespace Kajina
{
    public sealed partial class SettingsPage : Page
    {
        public SettingsPageViewModel ViewModel { get; set; } = new SettingsPageViewModel();

        public SettingsPage()
        {
            this.InitializeComponent();

            var version = Package.Current.Id.Version;
            string versionString = $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            VersionTextBlock.Text = versionString;
        }
    }
}
