using System;

namespace AccelClient
{
    public class DeviceData
    {
        public double Acceleration { get; set; }

        public string Device { get; set; }

        public string PartitionKey { get; set; }

        public string RowKey { get; set; }

        public DateTime Timestamp { get; set; }
    }
}