using System;
using Xamarin.Forms;

namespace XFDraw.Views
{
    public class ColorButton : BoxView
    {
        public event EventHandler ColorButtonTapped;

        public ColorButton()
        {
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => ColorButtonTapped?.Invoke(this, EventArgs.Empty);
            
            GestureRecognizers.Add(tapGestureRecognizer);
        }
    }
}