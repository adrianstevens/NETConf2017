using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TheRightDrawerStuff.XF
{
    public partial class RealTimePage : ContentPage
    {
        public RealTimePage()
        {
            InitializeComponent();

            Device.StartTimer(TimeSpan.FromSeconds(2), () =>
            {
                Animate();
                return true;
            });
        }

        private async Task Animate()
        {
            await theBox.RotateXTo(45, 750);
            await theBox.RotateXTo(-45, 750);
            await theBox.RotateTo(45, 750);
            await theBox.RotateTo(-45, 750);
        }
    }
}