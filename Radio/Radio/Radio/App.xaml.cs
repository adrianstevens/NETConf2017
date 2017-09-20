using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilationAttribute(XamlCompilationOptions.Compile)]

namespace Radio
{
    public partial class App : Application
    {
        public static GameManager GameMan = new GameManager();

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new ContentPage());

            DependencyService.Register<NavigationService>();
        }

        protected override void OnStart()
        {
            MainPage = new NavigationPage(new HomePage());
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}