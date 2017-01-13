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
using Android.Support.Design.Widget;
using Android.Support.V7;
using Android.Support.V4.App;
using UOTCS_android.Fragments;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Refractored.Controls;

namespace UOTCS_android
{
    [Activity(Label = "Send Announcement", Icon = "@drawable/icon", Theme = "@style/Theme.Student", ParentActivity = (typeof(Messages)))]
    public class SendAnnouncement : AppCompatActivity
    {
        SendMessageAnnouncementFragment sendAnnouncement;
        internal bool fabShouldBeShown;
        FloatingActionButton fab;
        private Button status;
        private Android.Support.V7.Widget.Toolbar toolBar;
        private Android.Support.V7.App.ActionBar actionbar;
        private DrawerLayout drawerLayout;
        private NavigationView navigationView;
        private View view;
        private CircleImageView profileImage;

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);

            Values.changeTheme(this);
            SetContentView(Resource.Layout.sendMessage);

            findViews();
            SetSupportActionBar(toolBar);
            setUpActionBar(actionbar);
            setUpNavigationView(navigationView);

            initiateFragments();
            handleEvents();
        }



        private void findViews()
        {
            toolBar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolBar);
            SetSupportActionBar(toolBar);
            actionbar = SupportActionBar;
            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            sendAnnouncement = new SendMessageAnnouncementFragment("Announcement");
            view = navigationView.GetHeaderView(0);
            profileImage = view.FindViewById<CircleImageView>(Resource.Id.nav_profile);


        }

        internal FloatingActionButton.OnVisibilityChangedListener fabListener = new OnVisibilityChangedListenerAnonymousInnerClass();

        private class OnVisibilityChangedListenerAnonymousInnerClass : FloatingActionButton.OnVisibilityChangedListener
        {
            internal bool fabShouldBeShown;
            public OnVisibilityChangedListenerAnonymousInnerClass()
            {
            }

            public override void OnShown(FloatingActionButton fab)
            {
                base.OnShown(fab);
                if (!fabShouldBeShown)
                {
                    fab.Hide();
                }
            }
            public override void OnHidden(FloatingActionButton fab)
            {
                base.OnHidden(fab);
                if (fabShouldBeShown)
                {
                    fab.Show();
                }
            }
        }
        public void methodWhereFabIsHidden()
        {
            fabShouldBeShown = false;
            fab.Hide(fabListener);
        }
        public void methodWhereFabIsShown()
        {
            fabShouldBeShown = true;
            fab.Show(fabListener);
        }




        private void handleEvents()
        {
            navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;
            profileImage.Click += ProfileImage_Click;

        }

        private void SetUpDrawerContent(NavigationView navigationView)
        {
            Values.handleSetUpDrawerContent(navigationView, drawerLayout);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            base.OnOptionsItemSelected(item);
            if (item.ItemId == Android.Resource.Id.Home)
            {
                NavUtils.NavigateUpFromSameTask(this);
                return true;
            }

            return base.OnOptionsItemSelected(item);

        }
        private void initiateFragments()
        {

            var trans = SupportFragmentManager.BeginTransaction();
            trans.Add(Resource.Id.send_message_fragment_container, sendAnnouncement, "sendAnnouncement");
            trans.Commit();

        }

        public int getCurrentActvity()
        {
            return Resource.Id.nav_announcements;
        }


        private void setUpActionBar(Android.Support.V7.App.ActionBar actionBar)
        {
            actionBar.SetHomeButtonEnabled(true);
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


        public override void OnBackPressed()
        {
            MoveTaskToBack(true);
        }


        private void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            drawerLayout.CloseDrawers();
            if (e.MenuItem.ItemId != getCurrentActvity())
            {
                Values.handleSwitchActivities(this, e.MenuItem.ItemId);
            }

        }
        private void ProfileImage_Click(object sender, EventArgs e)
        {
            drawerLayout.CloseDrawers();
            Intent intent = new Intent(this, typeof(Profile));
            this.StartActivity(intent);
            Finish();
        }
    }
}



