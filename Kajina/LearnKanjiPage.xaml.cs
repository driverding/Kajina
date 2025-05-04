using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.UI.Xaml.Controls;


namespace Kajina
{
    public class InventoryItem
    {
        public string Kanji { get; set; } = string.Empty;

        public string Kana { get; set; } = string.Empty;

        public string Romaji { get; set; } = string.Empty;
    }

    public sealed partial class LearnKanjiPage : Page
    {
        public ObservableCollection<InventoryItem> InventoryItems { get; set; }


        public LearnKanjiPage()
        {
            this.InitializeComponent();

            InventoryItems = [.. Data.kanjiWithKana.Select
                (
                    kanji => new InventoryItem() 
                    { 
                        Kanji = kanji.Item1,
                        Kana = kanji.Item2,
                        Romaji= kanji.Item3,
                    }
                )
            ];


        }
    }
}
