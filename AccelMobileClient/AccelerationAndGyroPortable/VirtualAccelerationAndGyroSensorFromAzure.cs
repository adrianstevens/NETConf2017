using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AccelerationAndGyro
{
    public class VirtualAccelerationAndGyroSensorFromAzure : IAccelerationAndGyroSensor
    {
        public event EventHandler<AccelerationAndGyroModel> NewSensorReading;

        readonly string azureSignalREndpoint;
        readonly string device;
        HubConnection connection;
        IHubProxy proxy;

        public VirtualAccelerationAndGyroSensorFromAzure(string AzureSignalREndpoint, string HubName, string Device)
        {
            azureSignalREndpoint = AzureSignalREndpoint;
            device = Device;
            connection = new HubConnection(AzureSignalREndpoint);
            proxy = connection.CreateHubProxy(HubName);

            ListenForData(device);
        }
        
        async Task ListenForData(string device)
        {
            proxy.On<string>("deviceData", deviceDataJson =>
            {
                NewSensorReading?.Invoke(this, createModel(deviceDataJson));
            });

            await connection.Start();
            await proxy.Invoke("ListenDevice", device);
        }

        private AccelerationAndGyroModel createModel(string s)
        {
            return JsonConvert.DeserializeObject<AccelerationAndGyroModel>(s);
            
        }
    }
}
