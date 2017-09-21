using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SayHelloXF
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            imageIcon.Source = ImageSource.FromResource("SayHelloXF.sayhello.png");

            buttonSpeak.Clicked += ButtonSpeakClicked;

            SizeChanged += MainPageSizeChanged;
        }

        private void MainPageSizeChanged(object sender, EventArgs e)
        {
            if (this.Width > this.Height)
                stackMain.Orientation = StackOrientation.Horizontal;
            else
                stackMain.Orientation = StackOrientation.Vertical;
        }

        void ButtonSpeakClicked(object sender, EventArgs e)
        {
            App.TextToSpeech?.Speak(entryMessage.Text);
        }
    }
}