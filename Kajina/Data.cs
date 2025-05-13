
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;

namespace Kajina
{
    public enum Mode { Hiragana, Katakana, Kanji };

    public static class Data
    {
        private static readonly List<(string, string)> hiragana =
        [
            ( "あ", "a" ), ( "い", "i" ), ( "う", "u" ), ( "え", "e" ), ( "お", "o" ),
            ( "か", "ka" ), ( "き", "ki" ), ( "く", "ku" ), ( "け", "ke" ), ( "こ", "ko" ),
            ( "が", "ga" ), ( "ぎ", "gi" ), ( "ぐ", "gu" ), ( "げ", "ge" ), ( "ご", "go" ),
            ( "さ", "sa" ), ( "し", "shi" ), ( "す", "su" ), ( "せ", "se" ), ( "そ", "so" ),
            ( "ざ", "za" ), ( "じ", "ji" ), ( "ず", "zu" ), ( "ぜ", "ze" ), ( "ぞ", "zo" ),
            ( "た", "ta" ), ( "ち", "chi" ), ( "つ", "tsu" ), ( "て", "te" ), ( "と","to" ),
            ( "だ", "da" ), ( "ぢ", "ji" ), ( "づ", "zu" ), ( "で", "de" ), ( "ど","do" ),
            ( "な", "na" ), ( "に", "ni" ), ( "ぬ", "nu" ), ( "ね", "ne" ), ( "の","no" ),
            ( "は", "ha" ), ( "ひ", "hi" ), ( "ふ", "fu" ), ( "へ", "he" ), ( "ほ","ho" ),
            ( "ば", "ba" ), ( "び", "bi" ), ( "ぶ", "bu" ), ( "べ", "be" ), ( "ぼ","bo" ),
            ( "ぱ", "pa" ), ( "ぴ", "pi" ), ( "ぷ", "pu" ), ( "ぺ", "pe" ), ( "ぽ","po" ),
            ( "ま", "ma" ), ( "み", "mi" ), ( "む", "mu" ), ( "め", "me" ), ( "も","mo" ),
            ( "や", "ya" ), ( "ゆ", "yu" ), ( "よ","yo" ),
            ( "ら", "ra" ), ( "り", "ri" ), ( "る", "ru" ), ( "れ", "re" ), ( "ろ","ro" ),
            ( "わ", "wa" ), ( "を","wo"  ),
            ( "ん", "n" )
        ];

        private static readonly List<(string, string)> hiraganaExtra =
        [
            ( "ゐ", "wi" ), ( "ゑ", "we" ), ( "ゔ", "vu" ),
        ];

        private static readonly List<(string, string)> katakana =
        [
            ( "ア", "a" ), ( "イ", "i" ), ( "ウ", "u" ), ( "エ", "e" ), ( "オ", "o" ),
            ( "カ", "ka" ), ( "キ", "ki" ), ( "ク", "ku" ), ( "ケ", "ke" ), ( "コ", "ko" ),
            ( "ガ", "ga" ), ( "ギ", "gi" ), ( "グ", "gu" ), ( "ゲ", "ge" ), ( "ゴ", "go" ),
            ( "サ", "sa" ), ( "シ", "shi" ), ( "ス", "su" ), ( "セ", "se" ), ( "ソ", "so" ),
            ( "ザ", "za" ), ( "ジ", "ji" ), ( "ズ", "zu" ), ( "ゼ", "ze" ), ( "ゾ", "zo" ),
            ( "タ", "ta" ), ( "チ", "chi" ), ( "ツ", "tsu" ), ( "テ", "te" ), ( "ト", "to" ),
            ( "ダ", "da" ), ( "ヂ", "ji" ), ( "ヅ", "zu" ), ( "デ", "de" ), ( "ド", "do" ),
            ( "ナ", "na" ), ( "ニ", "ni" ), ( "ヌ", "nu" ), ( "ネ", "ne" ), ( "ノ", "no" ),
            ( "ハ", "ha" ), ( "ヒ", "hi" ), ( "フ", "fu" ), ( "ヘ", "he" ), ( "ホ", "ho" ),
            ( "バ", "ba" ), ( "ビ", "bi" ), ( "ブ", "bu" ), ( "ベ", "be" ), ( "ボ", "bo" ),
            ( "パ", "pa" ), ( "ピ", "pi" ), ( "プ", "pu" ), ( "ペ", "pe" ), ( "ポ", "po" ),
            ( "マ", "ma" ), ( "ミ", "mi" ), ( "ム", "mu" ), ( "メ", "me" ), ( "モ", "mo" ),
            ( "ヤ", "ya" ), ( "ユ", "yu" ), ( "ヨ", "yo" ),
            ( "ラ", "ra" ), ( "リ", "ri" ), ( "ル", "ru" ), ( "レ", "re" ), ( "ロ", "ro" ),
            ( "ワ", "wa" ), ( "ヲ", "wo" ),
            ( "ン", "n" ),
        ];

        private static readonly List<(string, string)> katakanaExtra =
        [
            ( "ヰ", "wi" ), ( "ヱ", "we" ), ( "ヴ", "vu" ),
        ];

        public static readonly string[] builtinKanjiLists =
        {
            "n4-kanji", "n5-kanji"
        };


        public static List<(string, string)> kanji = [];
        public static List<(string, string, string)> kanjiWithKana = [];


        public static Dictionary<string, Queue<bool>> accuracy;
        public static int logLength;

        private const int maxLogLength = 100;

        static Data()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            logLength = (int)localSettings.Values["LogLength"];
        }

        public static void AppendAccuracy(string key, bool result)
        {
            if (!accuracy.ContainsKey(key))
            {
                accuracy.Add(key, new Queue<bool>(Enumerable.Repeat(false, maxLogLength)));
            }
            else
            {
                accuracy[key].Dequeue();
                accuracy[key].Enqueue(result);
            }

            WriteConfig(accuracy);
        }

        private async static void WriteConfig(Dictionary<string, Queue<bool>> accuracy)
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile configFile = await localFolder.CreateFileAsync("log.json", CreationCollisionOption.OpenIfExists);

            await FileIO.WriteTextAsync(configFile, JsonSerializer.Serialize(accuracy));
        }

        public async static Task ReadConfig()
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile? configFile = (StorageFile?) await localFolder.TryGetItemAsync("log.json");
            if (configFile == null)
            {
                accuracy = new Dictionary<string, Queue<bool>>();
            }
            else
            {
                string content = await FileIO.ReadTextAsync(configFile);
                accuracy = JsonSerializer.Deserialize<Dictionary<string, Queue<bool>>>(content);
            }
        }

        public static float GetAccuracy(string key)
        {
            if (!accuracy.ContainsKey(key))
                return 0.0f;
            else
            {
                int correctCount = accuracy[key].Skip(maxLogLength - logLength).Count(item => item == true);
                return (float)correctCount / (float)logLength;
            }
        }

        public static async void InitKanji()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            int group = (int)localSettings.Values["KanjiGroup"];

            string filename = (string)localSettings.Values["KanjiWordList"] + ".csv";
            int wordPerGroup = (int)localSettings.Values["KanjiWordPerGroup"];

            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///Assets/{filename}"));
            var lines = await FileIO.ReadLinesAsync(file);

            kanji.Clear();
            kanjiWithKana.Clear();

            var chosenLines = lines.Skip((group - 1) * wordPerGroup + 1).Take(wordPerGroup);

            foreach (var line in chosenLines)
            {
                var word = line.Split(',');
                kanji.Add((word[0], word[2]));
                kanjiWithKana.Add((word[0], word[1], word[2]));
            }
        }

        public static async void RefreshKanji(int group)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            string filename = (string)localSettings.Values["KanjiWordList"] + ".csv";
            int wordPerGroup = (int)localSettings.Values["KanjiWordPerGroup"];

            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///Assets/{filename}"));
            var lines = await FileIO.ReadLinesAsync(file);

            kanji.Clear();
            kanjiWithKana.Clear();

            var chosenLines = lines.Skip((group - 1) * wordPerGroup + 1).Take(wordPerGroup);

            foreach (var line in chosenLines)
            {
                var word = line.Split(',');
                kanji.Add((word[0], word[2]));
                kanjiWithKana.Add((word[0], word[1], word[2]));
            }
        }

        public static List<(string, string)> PickRandom(Mode mode, bool extraEnable, int quantity)
        {
            List<List<(string, string)>> candidates = [];
            List<(string, string)> result = [];

            if (mode == Mode.Hiragana)
            {
                candidates.Add(hiragana);
                if (extraEnable) candidates.Add(hiraganaExtra);
            } 
            else if (mode == Mode.Katakana)
            {
                candidates.Add(katakana);
                if (extraEnable) candidates.Add(katakanaExtra);
            } 
            else
            {
                candidates.Add(kanji);
            }

            int count = 0;
            foreach (var candidate in candidates)
            {
                count += candidate.Count;
            }

            Random random = new();

            while (result.Count < quantity)
            {
                int index = random.Next(count);

                foreach (var candidate in candidates)
                {
                    if (index < candidate.Count)
                    {
                        if (result.Contains(candidate[index])) continue;
                        result.Add(candidate[index]);
                        break;
                    }
                    index -= candidate.Count;
                }
            }

            return result;
        }
    }
}
