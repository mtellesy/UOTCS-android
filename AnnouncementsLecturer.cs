using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V4.View;
using UOTCS_android.Fragments;

namespace UOTCS_android
{
    [Activity(Label = "AnnouncementsLecturer")]
    public class AnnouncementsLecturer : MainActivity
    {
        Android.Support.V7.App.ActionBar ab;
        Color tabsBackground;
        FloatingActionButton fab;
        DrawerLayout mdrawerLayout;

        protected override void OnCreate(Bundle bundle)
        {


            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            Values.changeTheme(this);
            SetContentView(Resource.Layout.AnnouncementsLecturer);




            findViews();
            handleEvents();

        }



        private void findViews()
        {
            //   base.findViews();
            Android.Support.V7.Widget.Toolbar toolBar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolBarWT);
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
            adapter.AddFragment(new RecievedAnnouncementsFragment(), "Rescieved");
            adapter.AddFragment(new SentAnnouncementsFragment(), "Sent");

            viewPager.Adapter = adapter;
        }

        private void handleEvents()
        {
            fab.Click += Fab_Click;


        }

        private void Fab_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(SendAnnouncement));
            this.StartActivity(intent);
        }

        private void SetUpDrawerContent(NavigationView navigationView)
        {
            base.SetUpDrawerContent(navigationView);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            bool x = base.OnOptionsItemSelected(item);


            return x;
        }

        public int getCurrentActvity()
        {
            return Resource.Id.nav_messages;
        }


    }
}