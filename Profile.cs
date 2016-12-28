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

namespace UOTCS_android
{
    [Activity(Label = "Profile",Icon = "@drawable/icon", Theme = "@style/Theme.Student")]
    public class Profile : MainActivity
    {
       
        protected override void OnCreate(Bundle bundle)
        {
            DrawerLayout mdrawerLayout;

            // start the service for notifications 
            Intent intent = new Intent(this, typeof(Services.StatusChecker));
            this.StartService(intent);


            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            if (use_typeID > 0)
            {
                SetTheme(Resource.Style.Theme_Lecturer);
            }
            else
            {
                //start the enrollment status checker
            }


            SetContentView(Resource.Layout.Profile);

            var trans = SupportFragmentManager.BeginTransaction();
            trans.Add(Resource.Id.FragmentContainer, new Username(),"Username");
            trans.Commit();

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

        public override void OnBackPressed()
        {
            MoveTaskToBack(true);
        }

    }
}