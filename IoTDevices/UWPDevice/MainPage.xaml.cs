using IoTDevicesShared;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace UWPDevice
{
    public sealed partial class MainPage : Page
    {
        static string deviceId = "{TODO}";
        static string deviceKey = "{TODO}";
        static string hostName = "{TODO}";

        public MainPage()
        {
            this.InitializeComponent();

            SendMessages();
        }

        async void SendMessages()
        {
            var deviceToCloud = new DeviceToCloud(deviceId, deviceKey, hostName);

            while (true)
            {
                string msg;
                msg = await deviceToCloud.SendFakeDeviceToCloudSensorDataAsync();

                AddMessageToList(msg);

                System.Diagnostics.Debug.WriteLine("{0} > Sending message: {1}", DateTime.Now, msg);

                await Task.Delay(3000);
            }
        }

        void AddMessageToList(string msg)
        {

        }
    }
}
