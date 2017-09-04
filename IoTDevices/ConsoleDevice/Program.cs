using IoTDevicesShared;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ConsoleDevice
{
    class Program
    {
        static string deviceId = "{TODO}";
        static string deviceKey = "{TODO}";
        static string iotHubUri = "{TODO}";

        static void Main(string[] args)
        {
            SendMessages();
            Console.ReadLine();
        }

        static async void SendMessages()
        {
            var deviceToCloud = new DeviceToCloud(deviceId, deviceKey, iotHubUri);

            while (true)
            {
                string msg;
                msg = await deviceToCloud.SendFakeDeviceToCloudSensorDataAsync();

                Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, msg);

                await Task.Delay(3000);
            }
        }
    }
}
