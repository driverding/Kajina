using Microsoft.UI.Xaml.Controls;
using Windows.Storage;

namespace Kajina
{
    public sealed partial class LearnKatakanaPage : Page
    {
        public bool ExtraVisibility { get; }

        public LearnKatakanaPage()
        {
            this.InitializeComponent();

            var localSettings = ApplicationData.Current.LocalSettings;
            ExtraVisibility = (bool)localSettings.Values["ExtraEnabled"];
        }
    }
}
