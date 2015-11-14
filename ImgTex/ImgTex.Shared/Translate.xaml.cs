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
using TranslatorAPILibrary;
using Windows.Media.SpeechSynthesis;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ImgTex
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Translate : Page
    {

        // The object for controlling the speech synthesis engine (voice).
        SpeechSynthesizer speech;

        // The media object for controlling and playing audio.
        MediaElement mediaplayer;

        // The Translator object from our PCL that will call the Bing Translate API
        Translator tr;

        public static uint HResultPrivacyStatementDeclined = 0x80045509;
        public static string instructions = "Yuppi!.Görüntü Metni Aktarıldı!. Dilerseniz çevirmek istediğinizi Yazın veya Söyleyin. Daha sonra hedef dilinizi seçin ve çevir düğmesine tıklayınız";


        public Translate()
        {
            this.InitializeComponent();
            speech = new SpeechSynthesizer();
            mediaplayer = new MediaElement();

            lstLanguages.ItemsSource = SpeechSynthesizer.AllVoices;
            lstLanguages.SelectedValuePath = "Language";
            lstLanguages.SelectedValue = SpeechSynthesizer.DefaultVoice.Language;

            tr = new Translator();

            lblResult.Text = instructions;

            Windows.System.Display.DisplayRequest dr = new Windows.System.Display.DisplayRequest();
            dr.RequestActive();
        }

        private async void btnTalk_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of SpeechRecognizer.
            var speechRecognizer = new Windows.Media.SpeechRecognition.SpeechRecognizer();

            // Compile the dictation grammar that is loaded by default.
            await speechRecognizer.CompileConstraintsAsync();

            // Start recognition.
            try
            {
                Windows.Media.SpeechRecognition.SpeechRecognitionResult speechRecognitionResult = await speechRecognizer.RecognizeWithUIAsync();
                // If successful, display the recognition result.
                if (speechRecognitionResult.Status == Windows.Media.SpeechRecognition.SpeechRecognitionResultStatus.Success)
                {
                    txtSource.Text = speechRecognitionResult.Text;
                }
            }
            catch (Exception exception)
            {
                if ((uint)exception.HResult == HResultPrivacyStatementDeclined)
                {
                    //this.resultTextBlock.Visibility = Visibility.Visible;
                    lblResult.Text = "Özür dilerim, konuşma tanımayı kullanmak mümkün değildi. Konuşma gizlilik bildirimini kabul edilmedi.";
                }
                else
                {
                    var messageDialog = new Windows.UI.Popups.MessageDialog(exception.Message, "Exception");
                    messageDialog.ShowAsync().GetResults();
                }
            }
        }

        private void btnTranslate_Click(object sender, RoutedEventArgs e)
        {
            Translating();
        }

        private async void Translating()
        {
            VoiceInformation voice = (VoiceInformation)lstLanguages.SelectedItem;
            string language = voice.Language;

            lblResult.Text = await tr.TranslateString(txtSource.Text, language);
        }

        private void btnRead_Click(object sender, RoutedEventArgs e)
        {
            ReadText(lblResult.Text);
        }

        private async void ReadText(string mytext)
        {
            //Retrieve the first female voice
            speech.Voice = (VoiceInformation)lstLanguages.SelectedItem;
            // Generate the audio stream from plain text.
            SpeechSynthesisStream stream = await speech.SynthesizeTextToStreamAsync(mytext);

            // Send the stream to the media object.
            mediaplayer.SetSource(stream, stream.ContentType);
            mediaplayer.Play();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtSource.Text = "";
            lblResult.Text = instructions;
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            // Nothing to do yet
            Frame.Navigate(typeof(ExtractText));
            
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            // Nothing to do yet
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var okunan = (Translator)e.Parameter;
            txtSource.Text = okunan.okunanmetin;
        }

       
    }
}
