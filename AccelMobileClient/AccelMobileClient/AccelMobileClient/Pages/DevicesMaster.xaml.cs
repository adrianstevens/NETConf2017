using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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