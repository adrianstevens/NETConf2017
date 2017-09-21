using AccelClient.ViewModels;
using Xamarin.Forms;
using SkiaSharp;
using Microcharts;
using System.Collections.Generic;
using AccelMobileClient.Pages;

namespace AccelClient
{
    public partial class DeviceDetail : ContentPage
    {
        public DeviceDetail() { }

        public DeviceDetail(DeviceDetailViewModel viewModel)
        {
            viewModel.DataLoaded += () => AddChartData();

            InitializeComponent();

            BindingContext = viewModel;

            imageDevice.Source = ImageSource.FromResource($"AccelMobileClient.Images.{viewModel.TypeOfDevice}.png");

            realTime.Clicked += RealTime_Clicked;
        }

        private void RealTime_Clicked(object sender, System.EventArgs e)
        {
              Navigation.PushModalAsync (new RealTime(), true);
        }

        void AddChartData()
        {
            var data = ((DeviceDetailViewModel)this.BindingContext)?.Data;

            int count = 0;
            var entries = new List<Microcharts.Entry>();
            foreach (var d in data)
            {
                var entry = new Microcharts.Entry((float)d.Acceleration)
                {
                    Color = SKColor.Parse("#266489") //0078D7
                };

                entries.Add(entry);
                if (count++ > 20)
                    break;
            }

            chartView.Chart = new LineChart() { Entries = entries };
        }
    }
}