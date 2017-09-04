using Accel;
using IoTDevicesShared;
using IoTDevicesShared.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace UWPIoTDevice
{
    public sealed partial class MainPage : Page
    {
        static string deviceId = "{TODO}";
        static string deviceKey = "{TODO}";
        static string iotHubUri = "{TODO}";

        SensorData sensorData = new SensorData();

        public MainPage()
        {
            this.InitializeComponent();

            StartMPU();

            SendMessages();
        }

    
        async void StartMPU()
        {
            var mpu = new MPU6050();

            mpu.SensorInterruptEvent += MpuSensorInterruptEvent;

            await mpu.Init();
        }

        void MpuSensorInterruptEvent(object sender, MpuSensorEventArgs e)
        {
            sensorData = e.Values[0];

            Debug.WriteLine(e.ToString());
        }

        async void SendMessages()
        {
            var deviceToCloud = new DeviceToCloud(deviceId, deviceKey, iotHubUri);

            while (true)
            {
                string msg;

                msg = await deviceToCloud.SendFakeDeviceToCloudSensorDataAsync();

                AddMessageToList(msg);

                Debug.WriteLine("{0} > Sending message: {1}", DateTime.Now, msg);

                await Task.Delay(3000);
            }
        }

        void AddMessageToList(string msg)
        {

        }
    }
}
