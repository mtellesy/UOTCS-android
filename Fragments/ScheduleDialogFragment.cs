using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

using Android.Support.V4.Widget;
using SupportFragment = Android.Support.V4.App.Fragment;
using SupportFragmentManager = Android.Support.V4.App.FragmentManager;


using Android.Support.V7.App;
using Android.Support.Design.Widget;
using Android.Support.V4.View;

using CScore.BCL;
using Refractored.Controls;
using Android.Support.V4.App;

namespace UOTCS_android.Fragments
{
    public class ScheduleDialogFragment :Android.Support.V4.App.DialogFragment
    {
      
        SupportFragmentManager supportFragmentManager;
        FrameLayout myFrame;
        private FragmentActivity myContext;
        UOTCS_android.Fragments.ScheduleFragment scheduleFragment;
        public List<CScore.BCL.Course> courses;
        public Activity activity;
        View view;


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
           
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
             view = inflater.Inflate(Resource.Layout.ScheduleDialogFragment, container, false);
            //this.Dialog.SetTitle("dd");

            getActivity(activity);
            initiateFragments(courses);
            return view;
        }

        public void onAttach(Activity activity)
        {
            myContext = (FragmentActivity)activity;
            base.OnAttach(activity);
          

        }
        public void getActivity(Activity activity)
        {
            myContext = (FragmentActivity)activity;

        }
        public void initiateFragments(List<CScore.BCL.Course> courses)
        {
            var fm = myContext.SupportFragmentManager;
        
            var tran = fm.BeginTransaction();
            scheduleFragment = new ScheduleFragment();
            var k = scheduleFragment;
            foreach (var c in courses)
                scheduleFragment.setCourseDayAndTime(c);
            tran.Add(Resource.Id.fragmentLayout, scheduleFragment, "newFragment");
            tran.Commit();
        }
        public  void setScheduleTimeAndDate(List<CScore.BCL.Course> courses)
        {
            foreach (var c in courses)
                scheduleFragment.setCourseDayAndTime(c);
          
        }
        public void ShowDialog(List<CScore.BCL.Course> courses, SupportFragmentManager scheduleMangage )
        {
            this.initiateFragments(courses);
            this.Show(scheduleMangage, "Schedule");
        }


    }
}