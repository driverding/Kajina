using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace Kajina
{
    public sealed partial class KanaButton : UserControl
    {
        public string Kana
        {
            get { return (string)GetValue(KanaProperty); }
            set { SetValue(KanaProperty, value); }
        }

        public static readonly DependencyProperty KanaProperty =
            DependencyProperty.Register("Kana", typeof(string), typeof(KanaButton), new PropertyMetadata(string.Empty));

        public string Romaji
        {
            get { return (string)GetValue(RomajiProperty); }
            set { SetValue(RomajiProperty, value); }
        }

        public static readonly DependencyProperty RomajiProperty =
            DependencyProperty.Register("Romaji", typeof(string), typeof(KanaButton), new PropertyMetadata(string.Empty));

        public KanaButton()
        {
            this.InitializeComponent();
        }

        private double GetAccuracy(string kana)
        {
            // TODO!
            return 1.0;
        }

        private void Button_Loaded(object sender, RoutedEventArgs e)
        {
            double accuracy = GetAccuracy(this.Kana);
            var brush = accuracy switch
            {
                < 0.5 => (Brush)Application.Current.Resources["SystemFillColorCriticalBackgroundBrush"],
                _     => (Brush)Application.Current.Resources["SystemFillColorSuccessBackgroundBrush"]
            };
            var opacity = Math.Abs(accuracy - 0.5) / 0.5;
            brush.Opacity = opacity;
            this.Button.Background = brush;
        }
    }
}
