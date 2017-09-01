using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Accel
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            StartMPU();
        }

        async Task StartMPU()
        {
            var mpu = new MPU6050();

            mpu.SensorInterruptEvent += MpuSensorInterruptEvent;

            await mpu.Init();
        }

        private void MpuSensorInterruptEvent(object sender, MpuSensorEventArgs e)
        {
            Debug.WriteLine(e.ToString());
        }
    }
}
