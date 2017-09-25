using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilationAttribute(XamlCompilationOptions.Compile)]

namespace eXam
{
    public partial class App : Application
    {
        public static GameManager GameMan = new GameManager();

        public App()
        {
            InitializeComponent();
		
			MainPage = new NavigationPage(new HomePage());

            DependencyService.Register<NavigationService>();
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