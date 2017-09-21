using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AccelerationAndGyro
{
    
    public class AccelerationAndGyroModel
    {
        public float SamplePeriod { get; set; }

        public float AccelerationX { get; set; }
        public float AccelerationY { get; set; }
        public float AccelerationZ { get; set; }
        public float GyroX { get; set; }
        public float GyroY { get; set; }
        public float GyroZ { get; set; }
    }
    
}
