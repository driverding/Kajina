using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Kajina
{
    public partial class ReadPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged = delegate { };

        public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string? title;
        public string? Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged();
            }
        }

        private string? subtitle;
        public string? Subtitle
        {
            get { return subtitle; }
            set
            {
                subtitle = value;
                OnPropertyChanged();
            }
        }

        private string? symbol;
        public string? Symbol
        {
            get { return symbol; }
            set
            {
                symbol = value;
                OnPropertyChanged();
            }
        }

        private bool? symbolVisibility;
        public bool? SymbolVisibility
        {
            get { return symbolVisibility; }
            set
            {
                symbolVisibility = value;
                OnPropertyChanged();
            }
        }
    }
}
