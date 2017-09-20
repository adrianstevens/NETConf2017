using System;

namespace AccelClient
{
    public class DeviceStatus
    {
        public string Device { get; set; }

        public string PartitionKey { get; set; }

        public string RowKey { get; set; }

        public DateTime Timestamp { get; set; }
    }
}