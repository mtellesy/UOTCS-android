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
using UOTCS_android.Fragments;
using Android.Graphics;
using Android.Support.V4.App;

namespace UOTCS_android
{
    [Activity(Label = "Messages",Icon = "@drawable/icon", Theme = "@style/Theme.Student")]
    public class Messages : MainActivity
    {
        SupportActionBar ab;
        Color tabsBackground;
        FloatingActionButton fab;
        protected override void OnCreate(Bundle bundle)
        {
            DrawerLayout mdrawerLayout;
       
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            Values.changeTheme(this);
            SetContentView(Resource.Layout.Messages);




            findViews();
            handleEvents();
        }



        private  void findViews()
        {
            //   base.findViews();
            SupportToolbar toolBar = FindViewById<SupportToolbar>(Resource.Id.toolBarWT);
            SetSupportActionBar(toolBar);

             ab = SupportActionBar;
            //

            ab.SetHomeAsUpIndicator(Resource.Drawable.menu);
  //          SupportActionBar.SetHomeButtonEnabled(true);

            ab.SetDisplayHomeAsUpEnabled(true);



            mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layoutWT);

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_viewWT);
            navigationView.ItemIconTintList = null;

            if (navigationView != null)
            {
                SetUpDrawerContent(navigationView);
            }

            navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;

            TabLayout tabs = FindViewById<TabLayout>(Resource.Id.tabsWT);
            ViewPager viewPager = FindViewById<ViewPager>(Resource.Id.viewpagerWT);

            SetUpViewPager(viewPager);
           
           
            Values.Use_Color = new Color();
            tabsBackground = Values.Use_Color;
            tabs.SetBackgroundColor(tabsBackground);
            tabs.SetupWithViewPager(viewPager);
            fab = FindViewById<FloatingActionButton>(Resource.Id.fabWT);
            fab.Show();
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

        private void SetUpViewPager(ViewPager viewPager)
        {
            TabAdapter adapter = new TabAdapter(SupportFragmentManager);
            adapter.AddFragment(new RecievedMessagesFragment(), "Rescieved");
            adapter.AddFragment(new SentMessagesFragment(), "Sent");

            viewPager.Adapter = adapter;
        }

        private void handleEvents()
        {
            fab.Click += Fab_Click;
        }

        private void Fab_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(SendMessage));
            this.StartActivity(intent);
        }

        private  void SetUpDrawerContent(NavigationView navigationView)
        {
            base.SetUpDrawerContent(navigationView);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            bool x =base.OnOptionsItemSelected(item);
          

            return x;
        }

        public  int getCurrentActvity()
        {
            return Resource.Id.nav_messages;
        }


    }
}