using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Bing.Translator;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ImgTex
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Translate : Page
    {

        public Translate()
        {
            this.InitializeComponent();
            Initialize();
        }
        public async void Initialize()
        {
            // Call GetLanguagesAsync to get the list of supported languages.
            var result = await this.Translator2.TranslatorApi.GetLanguagesAsync();
            this.GetLanguagesForTranslate.ItemsSource = result.Languages;
        }

        private async void GetLanguagesForTranslate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox b = sender as ComboBox;
            string lang = b.SelectedItem as string;
            // Call GetLanguageNamesAsync to get the names of the languages.
            this.LangTextBox.Text = string.Join(",", (await this.Translator2.TranslatorApi.GetLanguageNamesAsync("en", new string[] { lang })).Languages);
        }

    }
}
