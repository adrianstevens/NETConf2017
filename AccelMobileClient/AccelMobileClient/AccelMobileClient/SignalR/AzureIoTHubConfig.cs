using System;
using System.Collections.Generic;
using System.Text;

namespace AccelMobileClient
{
    static class AzureIoTHubConfig
    {
        public const string DeviceId = "JasonsRazPi3";

        public const string DeviceKey = "";

        public const string IotHubUri = "Accel.azure-devices.net";
    }
    static class AzureSignalRConfig
    {
        public const string EndPoint = "http://accelwebappaspnet.azurewebsites.net";

        public const string HubName = "AccelHub";
    }
}
