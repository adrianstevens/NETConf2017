using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using Android.Things.Pio;
using System.Timers;

namespace BlinkThings
{
    [Activity(Label = "BlinkThings")]
    [IntentFilter(new[] { Intent.ActionMain }, Categories = new[] { Intent.CategoryLauncher })]
    [IntentFilter(new[] { Intent.ActionMain }, Categories = new[] { "android.intent.category.IOT_LAUNCHER" })]
    public class MainActivity : Activity
    {
        //GPIO pin;

        const string TAG = "EDISON_THING";
        const string GPIO_PIN = "IO13";

        Gpio ledGpio;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            // SetContentView(Resource.Layout.Main);

            var peripheralMgr = new PeripheralManagerService();

            var gpios = peripheralMgr.GpioList;

            foreach (var gpio in gpios)
                Android.Util.Log.Debug(TAG, "Found Peripheral: {0}", gpio);

            ledGpio = peripheralMgr.OpenGpio(GPIO_PIN);
            ledGpio.SetDirection(Gpio.DirectionOutInitiallyHigh);

            var timer = new Timer(1000);
            timer.Elapsed += ToggleLED;
            timer.Start();

        }

        void ToggleLED(object sender, ElapsedEventArgs e)
        {
            if(ledGpio.Value == true)
                ledGpio.SetDirection(Gpio.DirectionOutInitiallyLow);
            else 
                ledGpio.SetDirection(Gpio.DirectionOutInitiallyHigh);
        }
    }
}

