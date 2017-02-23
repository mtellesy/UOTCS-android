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

namespace UOTCS_android
{
    [Activity(Label = "Schedule",Icon = "@drawable/icon", Theme = "@style/Theme.Student")]
    public class Schedule : MainActivity
    {

        protected override async void OnCreate(Bundle bundle)
        {
            DrawerLayout mdrawerLayout;
            base.OnCreate(bundle);

            //dont forget to update the current term
           // CScore.BCL.Semester.current_term = 3;
            
         //   RequestWindowFeature(Window.FEATURE_NO_TITLE);
            // Set our view from the "main" layout resource
            if (use_typeID > 0)
            {
                SetTheme(Resource.Style.Theme_Lecturer);
            }

            
           
            SetContentView(Resource.Layout.Schedule);

            //add Schedule Fragment
            UOTCS_android.Fragments.ScheduleFragment myFragment = new UOTCS_android.Fragments.ScheduleFragment();
            var tran = SupportFragmentManager.BeginTransaction();
            tran.Add(Resource.Id.ScheduleFrame, myFragment, "newFragment");
            tran.Commit();

            List<CScore.BCL.Course> userCourses = new List<CScore.BCL.Course>();
         
            StatusWithObject<List<CScore.BCL.Course>> returndValue = await CScore.BCL.Course.getUserCoursesSchedule();
            if(returndValue.status.status)
            {
                userCourses = returndValue.statusObject;
                if(userCourses != null)
                foreach(CScore.BCL.Course c in userCourses)
                {
                    if(c != null)
                    myFragment.setCourseDayAndTime(c);
                }
                
            }
            


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