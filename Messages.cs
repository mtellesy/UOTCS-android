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
    [Activity(Label = "Messages",Icon = "@drawable/icon", Theme = "@style/Theme.Student")]
    public class Messages : MainActivity
    {

        protected override void OnCreate(Bundle bundle)
        {
            DrawerLayout mdrawerLayout;
       
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            if (Values.Use_typeID > 0)
            {
                SetTheme(Resource.Style.Theme_Lecturer);
            }

            SetContentView(Resource.Layout.Messages);




            findViews();
            handleEvents();
        }



        private void findViews()
        {
          base.findViews();
           
            TabLayout tabs = FindViewById<TabLayout>(Resource.Id.tabs);
            ViewPager viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);

            SetUpViewPager(viewPager);

            tabs.SetupWithViewPager(viewPager);
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