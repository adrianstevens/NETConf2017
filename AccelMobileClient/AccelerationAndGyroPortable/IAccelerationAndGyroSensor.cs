using System;

namespace AccelerationAndGyro
{
    public interface IAccelerationAndGyroSensor
    {
        event EventHandler<AccelerationAndGyroModel> NewSensorReading;
    }
}