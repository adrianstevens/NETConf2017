using System;
using System.Collections.Generic;
using System.Text;

namespace IoTDevicesShared.Models
{
    public class SensorData
    {
        public double AccelerationX { get; set; }
        public double AccelerationY { get; set; }
        public double AccelerationZ { get; set; }

        public double GyroX { get; set; }
        public double GyroY { get; set; }
        public double GyroZ { get; set; }
    }
}
