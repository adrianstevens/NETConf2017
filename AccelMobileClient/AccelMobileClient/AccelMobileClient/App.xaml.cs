using AccelClient;
using AccelerationAndGyro;
using System;
using Xamarin.Forms;

namespace AccelMobileClient
{
    public partial class App : Application
    {
        public static Func<IAccelerationAndGyroSensor> GetGyroSensor { get; set; } = CommonSensorConfig.GetFakeSensor;

        public App()
        {
            InitializeComponent();

            MainPage = new DevicesMasterDetail();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}