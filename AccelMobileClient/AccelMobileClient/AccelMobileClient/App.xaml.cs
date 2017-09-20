using AccelClient;
using Xamarin.Forms;

namespace AccelMobileClient
{
    public partial class App : Application
    {
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