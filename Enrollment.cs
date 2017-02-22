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

namespace UOTCS_android
{
    [Activity(Label = "Enrollment",Icon = "@drawable/icon", Theme = "@style/Theme.Student")]
    public class Enrollment : MainActivity
    {

        protected async override void OnCreate(Bundle bundle)
        {
            DrawerLayout mdrawerLayout;
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
         

           

            if (use_typeID > 0)
            {
                SetTheme(Resource.Style.Theme_Lecturer);
            }

            SetContentView(Resource.Layout.Enrollment);

            CScore.BCL.StatusWithObject<List<CScore.BCL.Course>> Courses =
             await CScore.BCL.Enrollment.getEnrollableCourses();
            
            var enrollmentAdapter = new EnrollmentAdapter(this,Courses.statusObject);
           // enrollmentAdapter.getExistedCourses();
            var contactsListView = FindViewById<ListView>(Resource.Id.myEnrollmentListView);
            contactsListView.Adapter = enrollmentAdapter;

            //for testing
            var list = CScore.BCL.Enrollment.enrolledCourses;
            if (list != null)
                list.ToString();

            ////add Schedule Fragment
            //UOTCS_android.Fragments.ScheduleFragment myFragment = new UOTCS_android.Fragments.ScheduleFragment();
            //var tran = SupportFragmentManager.BeginTransaction();
            //tran.Add(Resource.Id.ScheduleFrame, myFragment, "newFragment");
            //tran.Commit();

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
            return Resource.Id.nav_timetable;
        }



    }
}