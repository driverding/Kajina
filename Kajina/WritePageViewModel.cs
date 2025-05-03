
using System.ComponentModel;
using System.Runtime.CompilerServices;

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
}
