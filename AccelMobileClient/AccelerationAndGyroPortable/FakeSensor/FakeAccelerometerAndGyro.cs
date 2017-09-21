using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AccelerationAndGyro
{
    public class FakeAccelerometerAndGyro : IAccelerationAndGyroSensor
    {
        public event EventHandler<AccelerationAndGyroModel> NewSensorReading;

        List<AccelerationAndGyroModel> data;

        int currentReading = 0;
        

        public FakeAccelerometerAndGyro()
        {
            
            var serializer = new JsonSerializer();

            using (var dataStream = typeof(FakeAccelerometerAndGyro).GetTypeInfo().Assembly.GetManifestResourceStream("AccelerationAndGyroPortable.FakeSensor.SampleData.txt"))
            using (var sr = new StreamReader(dataStream))
            using (var jsonTextReader = new JsonTextReader(sr))
            {
                data = serializer.Deserialize<List<AccelerationAndGyroModel>>(jsonTextReader);
            }

            raiseEvent(null);
        }

        private async void raiseEvent(object state)
        {
            while (true)
            {
                await Task.Delay(10);

                NewSensorReading?.Invoke(this, data[currentReading]);
                currentReading = ++currentReading % data.Count;
            }
        }
    }
}
