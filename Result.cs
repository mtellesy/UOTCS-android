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
using UOTCS_android.Fragments;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Refractored.Controls;

namespace UOTCS_android
{
    [Activity(Label = "Result")]
    public class Result : AppCompatActivity
    {

        private ResultFragment result;
        private UserMoreInfomationFragment userMoreInformation;
        private Android.Support.V7.Widget.Toolbar toolBar;
        private Android.Support.V7.App.ActionBar actionbar;
        private DrawerLayout drawerLayout;
        private NavigationView navigationView;
        private View view;
        private CircleImageView profileImage;

        protected override void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);

            Values.changeTheme(this);
            SetContentView(Resource.Layout.Results);

            findViews();
            SetSupportActionBar(toolBar);
            setUpActionBar(actionbar);
            setUpNavigationView(navigationView);

            initiateFragments();
            handleEvents();

        }



        private void findViews()
        {

            result = new ResultFragment();
            userMoreInformation = new UserMoreInfomationFragment();
            toolBar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolBar);
            SetSupportActionBar(toolBar);
            actionbar = SupportActionBar;
            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            view = navigationView.GetHeaderView(0);
            profileImage = view.FindViewById<CircleImageView>(Resource.Id.nav_profile);

        }

        private void initiateFragments()
        {
            var trans = SupportFragmentManager.BeginTransaction();
            trans.Add(Resource.Id.ResultFragmentContainerResult, result, "result");
            trans.Add(Resource.Id.UserInformationFragmentContainerResult, userMoreInformation, "User_more_information");
            trans.Commit();

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
            navigationView.SetCheckedItem(Resource.Id.nav_result);
        }
        private void SetUpDrawerContent(NavigationView navigationView)
        {
            Values.handleSetUpDrawerContent(navigationView, drawerLayout);
        }

        private void handleEvents()
        {
            navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;
            profileImage.Click += ProfileImage_Click;

        }
        public int getCurrentActvity()
        {
            return Resource.Id.nav_result;
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