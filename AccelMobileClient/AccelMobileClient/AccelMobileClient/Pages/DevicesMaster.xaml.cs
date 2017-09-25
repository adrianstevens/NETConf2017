using System.Threading.Tasks;
using Xamarin.Forms;

namespace AccelClient
{
    public partial class DevicesMaster : ContentPage
    {
        public ListView ListView;

        DeviceManager deviceManager = new DeviceManager();

        public DevicesMaster()
        {
            InitializeComponent();

            ListView = listDevices;

            UpdateDeviceList();
        }
    
        async Task UpdateDeviceList()
        {
            var devices = await deviceManager.GetDevices();

            this.BindingContext = devices;
        }
    }
}