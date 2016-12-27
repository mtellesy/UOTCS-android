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
    [Activity(Label = "Announcements",Icon = "@drawable/icon", Theme = "@style/Theme.Student")]
    public class Announcements : MainActivity
    {

        protected override void OnCreate(Bundle bundle)
        {
            DrawerLayout mdrawerLayout;
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource

            if (use_typeID > 0)
            {
                SetTheme(Resource.Style.Theme_Lecturer);
            }
            SetContentView(Resource.Layout.Announcements);


            findViews();
            handleEvents();
        }



        private void findViews()
        {
            base.findViews();
        }

        private void handleEvents()
        {

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
            return Resource.Id.nav_announcements;
        }

            

    }
}