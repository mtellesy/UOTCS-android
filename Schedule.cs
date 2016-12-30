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
    [Activity(Label = "Schedule",Icon = "@drawable/icon", Theme = "@style/Theme.Student")]
    public class Schedule : MainActivity
    {

        protected override void OnCreate(Bundle bundle)
        {
            DrawerLayout mdrawerLayout;
            base.OnCreate(bundle);
         //   requestWindowFeature(Window.FEATURE_NO_TITLE);
            // Set our view from the "main" layout resource
            if (use_typeID > 0)
            {
                SetTheme(Resource.Style.Theme_Lecturer);
            }

            SetContentView(Resource.Layout.Schedule);
            var tran = SupportFragmentManager.BeginTransaction();
            tran.Add(Resource.Id.ScheduleFrame,new UOTCS_android.Fragments.ScheduleFragment(),"newFragment");
            tran.Commit();


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
            return Resource.Id.nav_schedule;
        }


    }
}