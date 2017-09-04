using Android.App;
using Android.Widget;
using Android.OS;
using System;
using IoTDevicesShared;
using System.Threading.Tasks;

namespace AndroidThingsDevice
{
    [Activity(Label = "AndroidThingsDevice", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        static string deviceId = "{TODO}";
        static string deviceKey = "{TODO}";
        static string iotHubUri = "{TODO}";

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SendMessages();
        }

        async void SendMessages()
        {
            var deviceToCloud = new DeviceToCloud(deviceId, deviceKey, iotHubUri);

            while (true)
            {
                string msg;
                msg = await deviceToCloud.SendFakeDeviceToCloudSensorDataAsync();

                System.Diagnostics.Debug.WriteLine("{0} > Sending message: {1}", DateTime.Now, msg);

                await Task.Delay(3000);
            }
        }
    }
}

