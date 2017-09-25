using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace AccelerationAndGyro
{
    public class HandleAccelAndGyroPushToAzure
    {
        string deviceId;
        string deviceKey;
        string iotHubUri;

        DeviceClient deviceClient;

        public HandleAccelAndGyroPushToAzure(string deviceId, string deviceKey, string iotHubUri)
        {
            this.deviceId = deviceId;
            this.deviceKey = deviceKey;
            this.iotHubUri = iotHubUri;
            
            deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey), TransportType.Http1);
        }
        
        public Task SendDeviceToCloudSensorDataAsync(AccelerationAndGyroModel sensorData)
        {
            var telemetryDataPoint = new
            {
                messageId = Guid.NewGuid().ToString(),
                deviceId = deviceId,
                accelerationX = sensorData.AccelerationX,
                accelerationY = sensorData.AccelerationY,
                accelerationZ = sensorData.AccelerationZ,
                gyroX = sensorData.GyroX,
                gyroY = sensorData.GyroY,
                gyroZ = sensorData.GyroZ,
            };

            var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
            var message = new Message(Encoding.UTF8.GetBytes(messageString));

            return deviceClient.SendEventAsync(message);
        }
    }
}