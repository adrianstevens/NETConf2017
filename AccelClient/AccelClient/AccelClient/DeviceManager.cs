using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AccelClient
{
    public class DeviceManager
    {
        const string Url = "http://accelwebappaspnet.azurewebsites.net/";

        public DeviceManager()
        {

        }

        public async Task<IEnumerable<DeviceStatus>> GetDevices ()
        {
            var client = new HttpClient();

            var result = await client.GetStringAsync(Url + "/api/Devices");

            return JsonConvert.DeserializeObject<IEnumerable<DeviceStatus>>(result);
        }

        public async Task<IEnumerable<DeviceData>> GetData (string device)
        {
            var client = new HttpClient();

            var result = await client.GetStringAsync(Url + $"/api/Accel/{device}");

            return JsonConvert.DeserializeObject<IEnumerable<DeviceData>>(result);
        }
    }
}