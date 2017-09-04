using IoTDevicesShared.Models;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace IoTDevicesShared
{
    public class DeviceToCloud
    {
        int messageId = 1;
        DeviceClient deviceClient;

        string deviceId;
        string deviceKey;
        string iotHubUri;

        public DeviceToCloud(string deviceId, string deviceKey, string iotHubUri)
        {
            this.deviceId = deviceId;
            this.deviceKey = deviceKey;
            this.iotHubUri = iotHubUri;

            deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey), TransportType.Http1);
        }

        SensorData GetRandomSensorData()
        {
            var rand = new Random();

            var sensorValues = new SensorData()
            {
                AccelerationX = rand.NextDouble(),
                AccelerationY = rand.NextDouble(),
                AccelerationZ = rand.NextDouble(),
                GyroX = rand.NextDouble(),
                GyroY = rand.NextDouble(),
                GyroZ = rand.NextDouble(),
            };

            return sensorValues;
        }

        public Task<string> SendFakeDeviceToCloudSensorDataAsync()
        {
            return SendDeviceToCloudSensorDataAsync(GetRandomSensorData());
        }

        public async Task<string> SendDeviceToCloudSensorDataAsync(SensorData sensorData)
        {


            var telemetryDataPoint = new
            {
                messageId = messageId++,
                deviceId = deviceId,
                accelerationX = sensorData.AccelerationX,
                accelerationY = sensorData.AccelerationY,
                accelerationZ = sensorData.AccelerationZ,
                gyroX = sensorData.GyroX,
                gyroY = sensorData.GyroY,
                gyroZ = sensorData.GyroZ,
            };

            var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
            var message = new Message(Encoding.ASCII.GetBytes(messageString));

            await deviceClient.SendEventAsync(message);

            return messageString;
        }

        async void ReceiveCloudToDeviceMessagesAsync()
        {
            Debug.WriteLine("\nReceiving cloud to device messages from service");

            while (true)
            {
                var receivedMessage = await deviceClient.ReceiveAsync();

                if (receivedMessage == null)
                    continue;

                Debug.WriteLine("Received message: {0}", Encoding.ASCII.GetString(receivedMessage.GetBytes()));

                await deviceClient.CompleteAsync(receivedMessage);
            }
        }
    }
}