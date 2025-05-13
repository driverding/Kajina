
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Windows.ApplicationModel;
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

        public ObservableCollection<int> LogLengthItemsSource = [10, 20, 30, 50, 100];

        public ObservableCollection<string> KanjiWordListItemsSource = [.. Data.builtinKanjiLists];

        public ObservableCollection<int> KanjiWordPerGroupItemsSource = [ 10, 20, 30, 50 ];


        private ObservableCollection<int> kanjiGroupItemsSource = [ ];

        public ObservableCollection<int> KanjiGroupItemsSource
        {
            get => kanjiGroupItemsSource;
            set
            {
                kanjiGroupItemsSource = value;
                OnPropertyChanged();
            }
        }
    }
    public sealed partial class SettingsPage : Page
    {
        private readonly ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public SettingsPageViewModel viewModel { get; set; } = new SettingsPageViewModel();

        private bool pageLoaded = false;

        public SettingsPage()
        {
            this.InitializeComponent();

            var version = Package.Current.Id.Version;
            string versionString = $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            VersionTextBlock.Text = versionString;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await RefreshGroupNumber();
            KanjiGroupComboBox.SelectedIndex = (int)this.localSettings.Values["KanjiGroup"];
            pageLoaded = true;
        }

        private void LogLengthComboBox_Loading(FrameworkElement sender, object args)
        {
            (sender as ComboBox).SelectedItem = (int)this.localSettings.Values["LogLength"];
        }

        private void LogLengthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).SelectedItem == null) return;
            this.localSettings.Values["LogLength"] = (sender as ComboBox).SelectedItem;
        }

        private void KanjiWordListComboBox_Loading(FrameworkElement sender, object args)
        {
            (sender as ComboBox).SelectedItem = (string)this.localSettings.Values["KanjiWordList"];
        }

        private void KanjiWordListComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!pageLoaded) return;
            this.localSettings.Values["KanjiWordList"] = (sender as ComboBox).SelectedItem;
            RefreshGroupNumber();
        }

        private void KanjiWordPerGroupComboBox_Loading(FrameworkElement sender, object args)
        {
            (sender as ComboBox).SelectedItem = this.localSettings.Values["KanjiWordPerGroup"];
        }

        private void KanjiWordPerGroupComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!pageLoaded) return;
            this.localSettings.Values["KanjiWordPerGroup"] = (sender as ComboBox).SelectedItem;
            RefreshGroupNumber();
        }

        private void KanjiGroupComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).SelectedItem == null) return;

            int group = (int)(sender as ComboBox).SelectedItem;
            this.localSettings.Values["KanjiGroup"] = group;
            Data.RefreshKanji(group);
        }

        public async Task RefreshGroupNumber()
        {
            int? originalSelection = (int?)KanjiGroupComboBox.SelectedItem;

            string filename = (string)this.localSettings.Values["KanjiWordList"] + ".csv";
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///Assets/{filename}"));
            var lines = await FileIO.ReadLinesAsync(file);

            int wordPerGroup = (int)this.localSettings.Values["KanjiWordPerGroup"];
            double groupCountDouble = (lines.Count - 2) / wordPerGroup;
            int groupCount = (int)Math.Ceiling(groupCountDouble);

            viewModel.KanjiGroupItemsSource.Clear();

            for (int i = 1; i <= groupCount; i += 1)
            {
                viewModel.KanjiGroupItemsSource.Add(i);
            }

            if (originalSelection != null)
            {
                KanjiGroupComboBox.SelectedItem = 
                    viewModel.KanjiGroupItemsSource.Contains((int)originalSelection) ? originalSelection : 1;
            }
        }
    }
}
