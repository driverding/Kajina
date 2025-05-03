using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Graphics;


namespace Kajina
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.ExtendsContentIntoTitleBar = true;
            this.SetTitleBar(AppTitleBar);
            AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;
            AppWindow.Resize(new SizeInt32(1800, 1200));
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
                ContentFrame.Navigate(typeof(SettingsPage));

            var selectedTag = (string)((NavigationViewItem)args.SelectedItem).Tag;

            if (selectedTag == "ReadHiraganaPage")
            {
                Mode arg = Mode.Hiragana;
                ContentFrame.Navigate(typeof(ReadPage), arg);
            }
            else if (selectedTag == "ReadKatakanaPage")
            {
                Mode arg = Mode.Katakana;
                ContentFrame.Navigate(typeof(ReadPage), arg);
            }
            else if (selectedTag == "ReadKanjiPage")
            {
                Mode arg = Mode.Kanji;
                ContentFrame.Navigate(typeof(ReadPage), arg);
            }
            else if (selectedTag == "WriteHiraganaPage")
            {
                Mode arg = Mode.Hiragana;
                ContentFrame.Navigate(typeof(WritePage), arg);
            }
            else if (selectedTag == "WriteKatakanaPage")
            {
                Mode arg = Mode.Katakana;
                ContentFrame.Navigate(typeof(WritePage), arg);
            }
            else if (selectedTag == "LearnHiraganaPage")
                ContentFrame.Navigate(typeof(LearnHiraganaPage));
            else if (selectedTag == "LearnKatakanaPage")
                ContentFrame.Navigate(typeof(LearnKatakanaPage));
            else if (selectedTag == "LearnKanjiPage")
                ContentFrame.Navigate(typeof(LearnKanjiPage));
        }
    }
}
