using Android.App;
using Android.Widget;
using Android.OS;
using IoTDevicesShared;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using IoTDevicesShared.Models;
using Android.Hardware;
using Android.Runtime;

namespace AndroidDevice
{
    [Activity(Label = "AndroidDevice", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, ISensorEventListener
    {
        static string deviceId = "{TODO}";
        static string deviceKey = "{TODO}";
        static string hostName = "{TODO}";

        SensorManager sensorManager;
        Sensor accelSensor;
        Sensor gyroSensor;

        List<string> sentMessages = new List<string>();

        ListView listMessages;
        ArrayAdapter adapter;

        SensorData sensorData = new SensorData();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView (Resource.Layout.Main);

            listMessages = FindViewById<ListView>(Resource.Id.listMessages);

            adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, sentMessages);

            listMessages.Adapter = adapter;

            InitSensors();

            SendMessages();
        }

        async void SendMessages()
        {
            var deviceToCloud = new DeviceToCloud(deviceId, deviceKey, hostName);

            while (true)
            {
                string msg;
                msg = await deviceToCloud.SendDeviceToCloudSensorDataAsync(sensorData);

                AddMessageToList(msg);
                
                System.Diagnostics.Debug.WriteLine("{0} > Sending message: {1}", DateTime.Now, msg);

                await Task.Delay(3000);
            }
        }

        void InitSensors ()
        {
            sensorManager = (SensorManager)GetSystemService(SensorService);

            accelSensor = sensorManager.GetDefaultSensor(SensorType.Accelerometer);

            if (accelSensor != null)
                sensorManager.RegisterListener(this, accelSensor, SensorDelay.Normal);

            gyroSensor = sensorManager.GetDefaultSensor(SensorType.Gyroscope);

            if (gyroSensor != null)
                sensorManager.RegisterListener(this, gyroSensor, SensorDelay.Normal);
        }

        void AddMessageToList (string msg)
        {
            sentMessages.Add(msg);

            adapter.Clear();
            adapter.AddAll(sentMessages);
            adapter.NotifyDataSetChanged();
        }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
            
        }

        //https://developer.android.com/guide/topics/sensors/sensors_motion.html
        public void OnSensorChanged(SensorEvent e)
        {
            if(e.Sensor == accelSensor)
            {
                sensorData.AccelerationX = e.Values[0];
                sensorData.AccelerationY = e.Values[1];
                sensorData.AccelerationZ = e.Values[2];
            }

            else if (e.Sensor == gyroSensor)
            {
                sensorData.GyroX = e.Values[0];
                sensorData.GyroY = e.Values[1];
                sensorData.GyroZ = e.Values[2];
            }
        }
    }
}