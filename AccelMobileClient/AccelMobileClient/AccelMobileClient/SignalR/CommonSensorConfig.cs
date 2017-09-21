using AccelerationAndGyro;

namespace AccelMobileClient
{
    public class CommonSensorConfig
    {
        public static IAccelerationAndGyroSensor GetVirtualSensor()
        {
            IAccelerationAndGyroSensor sensor = new VirtualAccelerationAndGyroSensorFromAzure(AzureSignalRConfig.EndPoint, AzureSignalRConfig.HubName, AzureIoTHubConfig.DeviceId);
            
            return sensor;
        }

        public static IAccelerationAndGyroSensor GetFakeSensor()
        {
            IAccelerationAndGyroSensor sensor = new FakeAccelerometerAndGyro();
            
            return sensor;
        }
        

    }
}
