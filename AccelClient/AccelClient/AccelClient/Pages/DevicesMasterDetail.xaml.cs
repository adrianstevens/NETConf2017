using AccelClient.ViewModels;
using System;
using Xamarin.Forms;

namespace AccelClient
{
    public partial class DevicesMasterDetail : MasterDetailPage
    {
        public DevicesMasterDetail()
        {
            InitializeComponent();
            MasterPage.ListView.ItemSelected += ListViewItemSelected;
        }

        void ListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var deviceStatus = e.SelectedItem as DeviceStatus;

            if (deviceStatus == null)
                return;

            var page = new DeviceDetail(new DeviceDetailViewModel(deviceStatus));

            Detail = new NavigationPage(page);
            IsPresented = false;

            MasterPage.ListView.SelectedItem = null;
        }
    }
}