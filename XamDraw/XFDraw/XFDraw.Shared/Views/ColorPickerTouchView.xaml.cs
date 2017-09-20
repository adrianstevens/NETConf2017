using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XFDraw.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ColorPickerTouchView : ColorPicker
	{
		public ColorPickerTouchView ()
		{
			InitializeComponent ();

            AddButtons((StackLayout)Content);
        }

        void AddButtons(StackLayout stackLayout)
        {
            int count = 6;
            int size = 50;

            for (int i = 0; i < count; i++)
            {
                double hue = i * 1.0 / 6.0;

                var button = GetColorButton(Color.FromHsla(hue, 0.9, 0.5), size);
                button.ColorButtonTapped += ButtonColorButtonTapped;
                stackLayout.Children.Add(button);
            }
        }
	}
}