using Android.App;
using Android.Widget;
using Android.OS;
using TheRightDrawerStuff.XF;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;

namespace TheRightDrawerStuff
{
    [Activity(Label = "TheRightDrawerStuff", MainLauncher = true, Icon = "@mipmap/icon", Theme="@style/Theme.AppCompat")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Forms.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.Main);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            base.SetActionBar(toolbar);
                
            var menu = FindViewById<Android.Support.Design.Widget.NavigationView>(Resource.Id.navigationView);
            menu.NavigationItemSelected += OnMenuItemSelected;

            Navigate((new HistoricalPage()).CreateFragment(this));
        }   

        void OnMenuItemSelected(object sender, Android.Support.Design.Widget.NavigationView.NavigationItemSelectedEventArgs e)
        {
            switch (e.MenuItem.ItemId)
            {
                case Resource.Id.realtimeMenuItem: Navigate((new RealTimePage()).CreateFragment(this) ); break;
                case Resource.Id.historicalMenuItem: Navigate((new HistoricalPage()).CreateFragment(this)); break;
            }

            e.MenuItem.SetChecked(true);

            var drawerLayout = FindViewById<Android.Support.V4.Widget.DrawerLayout>(Resource.Id.drawerLayout);
            drawerLayout.CloseDrawer(Android.Support.V4.View.GravityCompat.End);
        }

        void Navigate(Fragment fragment)
        {
            var transaction = base.FragmentManager.BeginTransaction();
            transaction.Replace(Resource.Id.contentFrame, fragment);
            transaction.Commit();
        }
    }
}