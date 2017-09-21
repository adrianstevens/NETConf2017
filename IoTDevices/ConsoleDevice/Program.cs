using IoTDevicesShared;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ConsoleDevice
{
    class Program
    {
        static string deviceId = "ConsoleDevice";
        static string deviceKey = "ySMBr4sd/9JPZ0N3i8LEojCL+CGRMJSk/4OfGjSd76E=";
        static string hostName = "Accel.azure-devices.net";

        static void Main(string[] args)
        {
            SendMessages();
            Console.ReadLine();
        }

        static async void SendMessages()
        {
            var deviceToCloud = new DeviceToCloud(deviceId, deviceKey, hostName);

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
