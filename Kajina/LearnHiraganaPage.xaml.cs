using Microsoft.UI.Xaml.Controls;
using Windows.Storage;

namespace Kajina
{
    public sealed partial class LearnHiraganaPage : Page
    {
        public bool ExtraVisibility { get; }

        public LearnHiraganaPage()
        {
            this.InitializeComponent();

            var localSettings = ApplicationData.Current.LocalSettings;
            ExtraVisibility = (bool)localSettings.Values["ExtraEnabled"];
        }
    }
}
