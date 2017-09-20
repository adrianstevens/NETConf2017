using AccelClient.ViewModels;
using System;
using System.Reflection;
using Xamarin.Forms;

namespace AccelClient
{
    public partial class DevicesMasterDetail : MasterDetailPage
    {
        public DevicesMasterDetail()
        {
            InitializeComponent();
            MasterPage.ListView.ItemSelected += ListViewItemSelected;

            var landingPage = new ContentPage()
            {
                Content = new Image()
                {
                    Source = ImageSource.FromResource("AccelMobileClient.Images.IoT.png"),
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    WidthRequest = 200,
                    HeightRequest = 200,
                }
            };
            Detail = new NavigationPage(landingPage);
        }

        void ListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var deviceStatus = e.SelectedItem as DeviceStatus;

            if (deviceStatus == null)
                return;

            ContentPage page;

            if(Device.Idiom == TargetIdiom.Desktop)
                page = new DeviceDetail(new DeviceDetailViewModel(deviceStatus));
            else //Phone or Tablet
                page = new DeviceDetailMobile(new DeviceDetailViewModel(deviceStatus));

            Detail = new NavigationPage(page);
            IsPresented = false;

            MasterPage.ListView.SelectedItem = null;
        }
    }
}