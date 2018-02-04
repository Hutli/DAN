using System.Windows;
using System.Speech.Synthesis;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Speech.Recognition;
using System;

namespace DAN
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DAN dan = new DAN();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            dan.Speak(SpeechInputTextBox.Text);
        }
    }

    class DAN
    {
        private SpeechSynthesizer mouth = new SpeechSynthesizer();
        private SpeechRecognizer ears = new SpeechRecognizer();

        public DAN()
        {
            ears.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(speechRecognized);

            mouth.SetOutputToDefaultAudioDevice();
        }

        public List<string> GetVoices()
        {
            ReadOnlyCollection<InstalledVoice> InstalledVoices = mouth.GetInstalledVoices();
            
            List<string> InstalledVoicesNames = new List<string>();

            foreach (InstalledVoice voice in InstalledVoices)
            {
                InstalledVoicesNames.Add(voice.VoiceInfo.Name);
            }

            return InstalledVoicesNames;
        }

        public void Speak(string text)
        {
            mouth.Speak(text);
        }

        private void speechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            MessageBox.Show(e.Result.Text);
        }
    }

    class DANWeather
    {
        // Enter your API key here.
        // Get an API key by making a free account at:
        //      http://home.openweathermap.org/users/sign_in
        private const string API_KEY = "9749874xw2kfiq9029j092m0j9kfj07e";

        // Query URLs. Replace @LOC@ with the location.
        private const string CurrentUrl =
            "http://api.openweathermap.org/data/2.5/weather?" +
            "q=@LOC@&mode=xml&units=imperial&APPID=" + API_KEY;
        private const string ForecastUrl =
            "http://api.openweathermap.org/data/2.5/forecast?" +
            "q=@LOC@&mode=xml&units=imperial&APPID=" + API_KEY;

        // Get current conditions.
        private void btnConditions_Click(object sender, EventArgs e)
        {
            // Compose the query URL.
            string url = CurrentUrl.Replace("@LOC@", txtLocation.Text);
            txtXml.Text = GetFormattedXml(url);
        }

        // Get a forecast.
        private void btnForecast_Click(object sender, EventArgs e)
        {
            // Compose the query URL.
            string url = ForecastUrl.Replace("@LOC@", txtLocation.Text);
            txtXml.Text = GetFormattedXml(url);
        }

        // Return the XML result of the URL.
        private string GetFormattedXml(string url)
        {
            // Create a web client.
            using (WebClient client = new WebClient())
            {
                // Get the response string from the URL.
                string xml = client.DownloadString(url);

                // Load the response into an XML document.
                XmlDocument xml_document = new XmlDocument();
                xml_document.LoadXml(xml);

                // Format the XML.
                using (StringWriter string_writer = new StringWriter())
                {
                    XmlTextWriter xml_text_writer =
                        new XmlTextWriter(string_writer);
                    xml_text_writer.Formatting = Formatting.Indented;
                    xml_document.WriteTo(xml_text_writer);

                    // Return the result.
                    return string_writer.ToString();
                }
            }
        }
    }
}
