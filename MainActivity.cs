using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using SupportActionBar = Android.Support.V7.App.ActionBar;

using Android.Support.V4.Widget;
using SupportFragment = Android.Support.V4.App.Fragment;
using SupportFragmentManager = Android.Support.V4.App.FragmentManager;

using Android.Support.V7.App;
using Android.Support.Design.Widget;
using Android.Support.V4.View;

namespace UOTCS_android
{
    [Activity(Label = "UOTCS_android", Icon = "@drawable/icon", Theme = "@style/Theme.Student")]
    public  class MainActivity : AppCompatActivity
    {
        public DrawerLayout mDrawerLayout;
      //  public static int use_typeID = 1;// 1 is for student .. 2 for teacher
    
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
           
            if (Values.Use_typeID>1)
            {
                SetTheme(Resource.Style.Theme_Lecturer);
            }

            SetContentView(Resource.Layout.Main);



            findViews();
            handleEvents();
        }

    

        public void findViews()
        {
            SupportToolbar toolBar = FindViewById<SupportToolbar>(Resource.Id.toolBar);
            SetSupportActionBar(toolBar);

            SupportActionBar ab = SupportActionBar;
            ab.SetHomeAsUpIndicator(Resource.Drawable.menu);
            ab.SetDisplayHomeAsUpEnabled(true);

            mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.ItemIconTintList = null;

            if (navigationView != null)
            {
                SetUpDrawerContent(navigationView);
            }
           
            navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;
          

        }

        private void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            if (e.MenuItem.ItemId != getCurrentActvity())
                SwitchActivties(e.MenuItem.ItemId);

            mDrawerLayout.CloseDrawers();
        }

        private void SwitchActivties(int itemId)
        {
            switch (itemId)
            {
                case Resource.Id.nav_announcements:
                    Intent intent = new Intent(this, typeof(Announcements));
                    this.StartActivity(intent); break;

                case Resource.Id.nav_messages:
                    Intent intent2 = new Intent(this, typeof(Messages));
                    this.StartActivity(intent2); break;
                case Resource.Id.nav_myCourses:
                    Intent intent3 = new Intent(this, typeof(MyCourses));
                    this.StartActivity(intent3); break;
                case Resource.Id.nav_schedule:
                    Intent intent4 = new Intent(this, typeof(Schedule));
                    this.StartActivity(intent4); break;
                case Resource.Id.nav_timetable:
                    Intent intent5 = new Intent(this, typeof(Timetable));
                    this.StartActivity(intent5); break;
            }
        }

        public void handleEvents()
        {

        }



       
        public void SetUpDrawerContent(NavigationView navigationView)
        {
       
            navigationView.NavigationItemSelected += (object sender, NavigationView.NavigationItemSelectedEventArgs e) =>
            {
                e.MenuItem.SetChecked(true);
                mDrawerLayout.CloseDrawers();
            };
        

        }


        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    mDrawerLayout.OpenDrawer((int)GravityFlags.Left);
                    return true;

               
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public  int getCurrentActvity() {
            return 0;
        }

        public override void OnBackPressed()
        {
            Intent intent = new Intent(this, typeof(Profile));
            this.StartActivity(intent);
        }
    }
}

