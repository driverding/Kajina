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
        }

        private Window? m_window;
    }
}
