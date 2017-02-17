using System;
using System.Collections.Generic;
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

using CScore.BCL;
using Refractored.Controls;

namespace UOTCS_android
{
    [Activity(Label = "Schedule",Icon = "@drawable/icon", Theme = "@style/Theme.Student")]
    public class Schedule : AppCompatActivity
    {
        private SupportToolbar toolBar;
        private SupportActionBar actionbar;
        private DrawerLayout drawerLayout;
        private NavigationView navigationView;
        private View view;
        private CircleImageView profileImage;
        UOTCS_android.Fragments.ScheduleFragment myFragment;
        StatusWithObject<List<CScore.BCL.Course>> returndValue;
        List<CScore.BCL.Course> userCourses;

        protected override async void OnCreate(Bundle bundle)
        {
            //dont forget to update the current term
            // CScore.BCL.Semester.current_term = 3;

            //   RequestWindowFeature(Window.FEATURE_NO_TITLE);
            // Set our view from the "main" layout resource
            base.OnCreate(bundle);
            this.Title = CScore.FixdStrings.Schedule.ScheduleLable();
            Values.changeTheme(this);
            SetContentView(Resource.Layout.Schedule);
            findViews();
            initiateFragments();
            SetSupportActionBar(toolBar);
            setUpActionBar(actionbar);
            setUpNavigationView(navigationView);

            
            handleEvents();
        }



        private async void findViews()
        {
            toolBar = FindViewById<SupportToolbar>(Resource.Id.toolBar);
            SetSupportActionBar(toolBar);
            actionbar = SupportActionBar;
            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            view = navigationView.GetHeaderView(0);
            profileImage = view.FindViewById<CircleImageView>(Resource.Id.nav_profile);
            myFragment = new UOTCS_android.Fragments.ScheduleFragment();
            userCourses = new List<CScore.BCL.Course>();
            returndValue = await CScore.BCL.Course.getUserCoursesSchedule();
            if (returndValue.status.status)
            {
                userCourses = returndValue.statusObject;
                if (userCourses != null)
                    foreach (CScore.BCL.Course c in userCourses)
                    {
                        if (c != null)
                            myFragment.setCourseDayAndTime(c);
                    }

            }
        }


        private void initiateFragments()
        {
            var tran = SupportFragmentManager.BeginTransaction();
            tran.Add(Resource.Id.ScheduleFrame, myFragment, "newFragment");
            tran.Commit();
        }
        private void setUpActionBar(SupportActionBar actionBar)
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
            navigationView.SetCheckedItem(Resource.Id.nav_schedule);
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
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    drawerLayout.OpenDrawer((int)GravityFlags.Start);
                    return true;


                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public  int getCurrentActvity()
        {
            return Resource.Id.nav_schedule;
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
                Values.handleSwitchActivities(this, e.MenuItem.ItemId, navigationView);
            }

        }
        private void ProfileImage_Click(object sender, EventArgs e)
        {
            drawerLayout.CloseDrawers();
            Values.startProfile(this);
            Finish();
        }
    }
}