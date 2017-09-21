using System;
using System.Collections.Generic;
using System.Text;

namespace AccelerationAndGyro
{
    public interface IAccelerationAndGyroSensor
    {
        event EventHandler<AccelerationAndGyroModel> NewSensorReading;
    }
}
