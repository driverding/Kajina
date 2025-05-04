using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Storage;


namespace Kajina
{
    public partial class WritePageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged = delegate { };

        public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged();
            }
        }

        private string symbol;
        public string Symbol
        {
            get { return symbol; }
            set
            {
                symbol = value;
                OnPropertyChanged();
            }
        }

        private bool symbolVisibility;
        public bool SymbolVisibility
        {
            get { return symbolVisibility; }
            set
            {
                symbolVisibility = value;
                OnPropertyChanged();
            }
        }

        private string[] choices;
        public string[] Choices
        {
            get { return choices; }
            set
            {
                choices = value;
                OnPropertyChanged();
            }
        }
    }

    public sealed partial class WritePage : Page
    {
        private enum State { Questioned, Graded };

        private WritePageViewModel viewModel { get; set; }
        private State state = State.Graded;
        private string answer = "";
        private readonly bool extraEnabled;

        private Mode mode;

        public WritePage()
        {
            this.InitializeComponent();

            this.viewModel = new WritePageViewModel
            {
                Title = "Get Ready",
                Symbol = "Accept",
                SymbolVisibility = false,
                Choices = ["1", "2", "3", "4", "5", "6"],
            };

            var localSettings = ApplicationData.Current.LocalSettings;
            this.extraEnabled = (bool)localSettings.Values["ExtraEnabled"];
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.mode = (Mode)e.Parameter;
        }

        private void Grid_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (this.state == State.Questioned) return;
            this.Question();
            this.state = State.Questioned;
        }

        private void Question()
        {
            var questions = Data.PickRandom(this.mode, this.extraEnabled, 6);
            string[] temp = new string[6];
            
            for (int i = 0; i < 6; i += 1)
            {
                temp[i] = questions[i].Item1;
            }
            viewModel.Choices = temp;

            Random random = new();
            int index = random.Next(6);

            this.answer = questions[index].Item1;
            viewModel.Title = questions[index].Item2.ToUpper();

            viewModel.SymbolVisibility = false;

            foreach (var child in ChoicesGrid.Children)
            {
                (child as Button).Background = (Brush)Application.Current.Resources["ControlFillColorDefaultBrush"]; // new SolidColorBrush(Colors.Transparent);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.state == State.Graded)
            {
                this.Question();
                this.state = State.Questioned;
                return;
            }

            Button button = sender as Button;

            string text = (button.Content as TextBlock).Text;

            if (text != this.answer)
            {
                viewModel.Symbol = "Cancel";
                button.Background = (Brush)Application.Current.Resources["SystemFillColorCriticalBackgroundBrush"];
                
                foreach (var child in ChoicesGrid.Children)
                {
                    Button correctButton = child as Button;
                    if ((correctButton.Content as TextBlock).Text == this.answer)
                    {
                        correctButton.Background = (Brush)Application.Current.Resources["SystemFillColorSuccessBackgroundBrush"];
                    }
                }
            }
            else
            {
                viewModel.Symbol = "Accept";
                button.Background = (Brush)Application.Current.Resources["SystemFillColorSuccessBackgroundBrush"];
            }

            viewModel.SymbolVisibility = true;

            this.state = State.Graded;
        }
    }
}
