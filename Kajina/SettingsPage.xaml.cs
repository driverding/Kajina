
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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

        public ObservableCollection<string> KanjiWordListItemsSource = [.. Data.builtinWordLists];


        public ObservableCollection<int> KanjiWordPerGroupItemsSource = [ 10, 20, 30, 50 ];

        private ObservableCollection<int> kanjiGroupItemsSource = [ 1 ];

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

        public SettingsPage()
        {
            this.InitializeComponent();

            var version = Package.Current.Id.Version;
            string versionString = $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            VersionTextBlock.Text = versionString;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshGroupNumber();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            Data.LoadKanji();
        }

        private void KanjiWordListComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            (sender as ComboBox).SelectedItem = (string)this.localSettings.Values["KanjiWordList"];
        }

        private void KanjiWordListComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.localSettings.Values["KanjiWordList"] = (sender as ComboBox).SelectedItem;
            RefreshGroupNumber();
            KanjiGroupComboBox.SelectedItem = 1;
        }

        private void KanjiWordPerGroupComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            (sender as ComboBox).SelectedItem = this.localSettings.Values["KanjiWordPerGroup"];
        }

        private void KanjiWordPerGroupComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.localSettings.Values["KanjiWordPerGroup"] = (sender as ComboBox).SelectedItem;
            RefreshGroupNumber();
            KanjiGroupComboBox.SelectedItem = 1;
        }

        private void KanjiGroupComboBox_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            (sender as ComboBox).SelectedItem = this.localSettings.Values["KanjiGroup"];
        }

        private void KanjiGroupComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.localSettings.Values["KanjiGroup"] = (sender as ComboBox).SelectedItem;
        }

        public async void RefreshGroupNumber()
        {
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

            KanjiGroupComboBox.SelectedItem = 1;
        }
    }
}
