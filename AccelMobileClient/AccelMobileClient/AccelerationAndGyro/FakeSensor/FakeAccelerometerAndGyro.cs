using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

namespace AccelerationAndGyro
{
    public class FakeAccelerometerAndGyro : IAccelerationAndGyroSensor
    {
        public event EventHandler<AccelerationAndGyroModel> NewSensorReading;

        List<AccelerationAndGyroModel> data;

        int currentReading = 0;
        Timer timer;
        

        public FakeAccelerometerAndGyro()
        {
            
            var serializer = new JsonSerializer();

            using (var dataStream = typeof(FakeAccelerometerAndGyro).GetTypeInfo().Assembly.GetManifestResourceStream("AccelerationAndGyro.FakeSensor.SampleData.txt"))
            using (var sr = new StreamReader(dataStream))
            using (var jsonTextReader = new JsonTextReader(sr))
            {
                data = serializer.Deserialize<List<AccelerationAndGyroModel>>(jsonTextReader);
            }

            timer = new System.Threading.Timer(raiseEvent, null, TimeSpan.Zero, TimeSpan.FromSeconds(0.01));            
        }

        private void raiseEvent(object state)
        {
            NewSensorReading?.Invoke(this, data[currentReading]);
            currentReading = ++currentReading % data.Count;
        }
    }
}
