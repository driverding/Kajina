using Microsoft.UI.Xaml;
using Windows.Storage;

namespace Kajina
{
    public partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            m_window.Activate();

            InitializeLocalSettings();
        }

        private void InitializeLocalSettings()
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            if (!localSettings.Values.ContainsKey("ExtraEnabled"))
            {
                localSettings.Values["ExtraEnabled"] = false;
            }
            if (!localSettings.Values.ContainsKey("KanjiWordList"))
            {
                localSettings.Values["KanjiWordList"] = Data.builtinWordLists[0];
            }
            if (!localSettings.Values.ContainsKey("KanjiWordPerGroup"))
            {
                localSettings.Values["KanjiWordPerGroup"] = 20;
            }
            if (!localSettings.Values.ContainsKey("KanjiGroup"))
            {
                localSettings.Values["KanjiGroup"] = 1;
            }
        }

        private Window? m_window;
    }
}
