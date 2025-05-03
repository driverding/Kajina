
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI;
using Windows.Storage;

namespace Kajina
{

    public sealed partial class ReadPage : Page
    {
        private enum State { Questioned, Graded };

        public ReadPageViewModel viewModel { get; set; }
        private State state = State.Graded;
        private string answer = "";
        private readonly bool extraEnabled;

        private Mode mode;

        public ReadPage()
        {
            this.InitializeComponent();

            viewModel = new ReadPageViewModel
            {
                Title = "Get Ready",
                Subtitle = "Press Enter to Start",
                Symbol = "Accept",
                SymbolVisibility = false,
            };

            MainGrid.Background = new SolidColorBrush(Colors.Transparent);

            var localSettings = ApplicationData.Current.LocalSettings;
            this.extraEnabled = (bool)localSettings.Values["ExtraEnabled"];
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TextBox.Focus(FocusState.Programmatic);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.mode = (Mode)e.Parameter;
        }


        private void TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Progress();
            }
        }

        private void Progress()
        {
            if (state == State.Graded)
            {
                var question = Data.PickRandom(this.mode, this.extraEnabled, 1)[0];
                viewModel.Title = question.Item1;
                this.answer = question.Item2;

                viewModel.Subtitle = "";
                viewModel.SymbolVisibility = false;
                MainGrid.Background = new SolidColorBrush(Colors.Transparent);

                TextBox.Text = "";
                state = State.Questioned;
            }
            else
            {
                if (TextBox.Text == answer)
                {
                    viewModel.Symbol = "Accept";
                    MainGrid.Background = (Brush)Application.Current.Resources["SystemFillColorSuccessBackgroundBrush"];
                }
                else
                {
                    viewModel.Symbol = "Cancel";
                    MainGrid.Background = (Brush)Application.Current.Resources["SystemFillColorCriticalBackgroundBrush"];
                }

                viewModel.Subtitle = answer;
                viewModel.SymbolVisibility = true;

                state = State.Graded;
            }
        }

    }
}
