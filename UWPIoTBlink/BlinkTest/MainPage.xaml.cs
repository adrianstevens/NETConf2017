using System;
using Windows.Devices.Gpio;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace BlinkTest
{
    public sealed partial class MainPage : Page
    {
        const int LED_PIN = 5;
        const int BUTTON_PIN = 6;

        GpioPin pinLed;
        GpioPin pinButton;
        GpioPinValue valueLed;
        DispatcherTimer timer;
        SolidColorBrush onBrush = new SolidColorBrush(Windows.UI.Colors.Blue);
        SolidColorBrush offBrush = new SolidColorBrush(Windows.UI.Colors.LightGray);

        int count;

        public MainPage()
        {
            this.InitializeComponent();

            btnClick.Click += BtnClickClick;

            InitGPIO();
        }

        void InitGPIO()
        {
            var gpio = GpioController.GetDefault();

            // Show an error if there is no GPIO controller
            if (gpio == null)
            {
                GpioStatus.Text = "There is no GPIO controller on this device.";
                return;
            }

            //Init LED
            pinLed = gpio.OpenPin(LED_PIN);
            pinLed.Write(valueLed = GpioPinValue.High);
            pinLed.SetDriveMode(GpioPinDriveMode.Output);

            //Init Button
            pinButton = gpio.OpenPin(BUTTON_PIN);

            // Check if input pull-up resistors are supported
            if (pinButton.IsDriveModeSupported(GpioPinDriveMode.InputPullUp))
                pinButton.SetDriveMode(GpioPinDriveMode.InputPullUp);
            else
                pinButton.SetDriveMode(GpioPinDriveMode.Input);

            // Set a debounce timeout to filter out switch bounce noise from a button press
            pinButton.DebounceTimeout = TimeSpan.FromMilliseconds(25);

            pinButton.ValueChanged += PinButtonValueChanged;

            GpioStatus.Text = "GPIO pin initialized correctly.";
        }

        void PinButtonValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            // need to invoke UI updates on the UI thread because this event
            // handler gets invoked on a separate thread.
            var task = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {

                if (args.Edge == GpioPinEdge.FallingEdge)
                {
                    ToggleLED();
                }
            });
        }

        void InitTimer ()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += TimerTick;

            if (pinLed != null)
                timer.Start();
        }

        void BtnClickClick(object sender, RoutedEventArgs e)
        {
            textMessage.Text = $"Button clicked {++count} times";

            ToggleLED();
        }

        void ToggleLED()
        {
            valueLed = (valueLed == GpioPinValue.High) ? GpioPinValue.Low : GpioPinValue.High;
            pinLed.Write(valueLed);
            LED.Fill = (valueLed == GpioPinValue.High) ? offBrush : onBrush;
        }

        private void TimerTick(object sender, object e)
        {
            ToggleLED();
        }
    }
}