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
using Android.Support.V7.App;
using Android.Support.V4.App;

namespace UOTCS_android
{
    [Activity(Label = "AnnouncementsLecturer")]
    public class AnnouncementsLecturer : AppCompatActivity
    {
        private SendMessageAnnouncementFragment sendAnnouncement;
        private FloatingActionButton fab;
        private Android.Support.V7.Widget.Toolbar toolBar;
        private Android.Support.V7.App.ActionBar actionbar;
        private DrawerLayout drawerLayout;
        private NavigationView navigationView;
        private TabLayout tabs;
        private ViewPager viewPager;
        private TabAdapter adapter;
        private RecievedAnnouncementsFragment recievedAnnouncement;
        private SentAnnouncementsFragment sentAnnouncement;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Values.changeTheme(this);
            SetContentView(Resource.Layout.AnnouncementsLecturer);
            findViews();
            SetSupportActionBar(toolBar);
            setUpActionBar(actionbar);
            setUpNavigationView(navigationView);
            SetUpViewPager(viewPager);
            handleEvents();

        }



        private void findViews()
        {
            toolBar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolBarWT);
            SetSupportActionBar(toolBar);
            actionbar = SupportActionBar;
            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layoutWT);
            navigationView = FindViewById<NavigationView>(Resource.Id.nav_viewWT);
            sendAnnouncement = new SendMessageAnnouncementFragment("Announcement");
            tabs = FindViewById<TabLayout>(Resource.Id.tabsWT);
            viewPager = FindViewById<ViewPager>(Resource.Id.viewpagerWT);
            adapter = new TabAdapter(SupportFragmentManager);
            recievedAnnouncement = new RecievedAnnouncementsFragment();
            sentAnnouncement = new SentAnnouncementsFragment();
            fab = FindViewById<FloatingActionButton>(Resource.Id.fabWT);
            fab.Show();
        }



        private void handleEvents()
        {
            navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;
            fab.Click += Fab_Click;
        }

        private void SetUpDrawerContent(NavigationView navigationView)
        {
            Values.handleSetUpDrawerContent(navigationView, drawerLayout);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    drawerLayout.OpenDrawer((int)GravityFlags.Left);
                    return true;


                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public int getCurrentActvity()
        {
            return Resource.Id.nav_announcements;
        }


        private void setUpActionBar(Android.Support.V7.App.ActionBar actionBar)
        {
            actionBar.SetHomeAsUpIndicator(Resource.Drawable.menu);
            actionBar.SetDisplayHomeAsUpEnabled(true);
        }
        private void setUpNavigationView(NavigationView navigationView)
        {
            navigationView.ItemIconTintList = null;
            Values.changeNavigationItems(navigationView, this);
            if (navigationView != null)
            {
                SetUpDrawerContent(navigationView);
            }
        }

        private void SetUpViewPager(ViewPager viewPager)
        {

            adapter.AddFragment(recievedAnnouncement, "Rescieved");
            adapter.AddFragment(sentAnnouncement, "Sent");

            viewPager.Adapter = adapter;
        }

        public override void OnBackPressed()
        {
            MoveTaskToBack(true);
        }


        private void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            if (e.MenuItem.ItemId != getCurrentActvity())
            {
                Values.handleSwitchActivities(this, e.MenuItem.ItemId);
            }
            drawerLayout.CloseDrawers();

        }
        private void Fab_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(SendAnnouncement));
            this.StartActivity(intent);
        }

    }
}




