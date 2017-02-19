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
using Refractored.Controls;
using Android.Support.V4.Content;

namespace UOTCS_android
{
    [Activity(Label = "AnnouncementsLecturer")]
    public class AnnouncementsLecturer : AppCompatActivity
    {

        private FloatingActionButton fab;
        private SupportToolbar toolBar;
        private SupportActionBar actionbar;
        private DrawerLayout drawerLayout;
        private NavigationView navigationView;
        private TabLayout tabs;
        private ViewPager viewPager;
        private TabAdapter adapter;
        private RecievedAnnouncementsFragment recievedAnnouncement;
        private SentAnnouncementsFragment sentAnnouncement;
        private View view;
        private CircleImageView profileImage;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            this.Title = CScore.FixdStrings.Announcements.AnnouncementsLable();
            Values.changeTheme(this);
            SetContentView(Resource.Layout.AnnouncementsLecturer);
            findViews();
            SetSupportActionBar(toolBar);
            setUpActionBar(actionbar);
            setUpNavigationView(navigationView);
            SetUpViewPager(viewPager);
            tabs.SetupWithViewPager(viewPager);
            handleEvents();

        }



        private void findViews()
        {
            toolBar = FindViewById<SupportToolbar>(Resource.Id.toolBarWT);
            SetSupportActionBar(toolBar);
            actionbar = SupportActionBar;
            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layoutWT);
            navigationView = FindViewById<NavigationView>(Resource.Id.nav_viewWT);
            tabs = FindViewById<TabLayout>(Resource.Id.tabsWT);
            viewPager = FindViewById<ViewPager>(Resource.Id.viewpagerWT);
            adapter = new TabAdapter(SupportFragmentManager);
            recievedAnnouncement = new RecievedAnnouncementsFragment();
            sentAnnouncement = new SentAnnouncementsFragment();
            fab = FindViewById<FloatingActionButton>(Resource.Id.fabWT);
            fab.SetImageDrawable(ContextCompat.GetDrawable(this, Resource.Drawable.ic_create));
            fab.Show();
            view = navigationView.GetHeaderView(0);
            profileImage = view.FindViewById<CircleImageView>(Resource.Id.nav_profile);

        }



        private void handleEvents()
        {
            navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;
            fab.Click += Fab_Click;
            profileImage.Click += ProfileImage_Click;

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
                    drawerLayout.OpenDrawer((int)GravityFlags.Start);
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
            actionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);
            actionBar.SetDisplayHomeAsUpEnabled(true);
        }
        private void setUpNavigationView(NavigationView navigationView)
        {
            Values.changeNavigationItems(navigationView, this);
            if (navigationView != null)
            {
                SetUpDrawerContent(navigationView);
            }
            navigationView.SetCheckedItem(Resource.Id.nav_announcements);
        }

        private void SetUpViewPager(ViewPager viewPager)
        {
             adapter.AddFragment(recievedAnnouncement, CScore.FixdStrings.General.recieved());
            adapter.AddFragment(sentAnnouncement, CScore.FixdStrings.General.sent());
            viewPager.Adapter = adapter;
        }

        public override void OnBackPressed()
        {
            Values.startProfile(this);
            Finish();
        }


        private void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            drawerLayout.CloseDrawers();
            if (e.MenuItem.ItemId != getCurrentActvity())
            {
                Values.handleSwitchActivities(this, e.MenuItem.ItemId, navigationView);
                Finish();
            }
    

        }
        private void Fab_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(SendAnnouncement));
            this.StartActivity(intent);
        }
        private void ProfileImage_Click(object sender, EventArgs e)
        {
            drawerLayout.CloseDrawers();
            Values.startProfile(this);
            Finish();
        }

    }
}




