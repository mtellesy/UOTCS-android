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

namespace UOTCS_android.Fragments
{
    public class ScheduleDialogFragment : DialogFragment
    {
        UOTCS_android.Fragments.ScheduleFragment scheduleFragment;
        SupportFragmentManager supportFragmentManager;
        

        public static ScheduleDialogFragment newInstance()
        {
            return new ScheduleDialogFragment();
        }
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
           
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.ScheduleDialogFragment, container, false);
            scheduleFragment = new UOTCS_android.Fragments.ScheduleFragment();
            initiateFragments();
            return view;
        }
      
        private void initiateFragments()
        {
            var tran = supportFragmentManager.BeginTransaction(); 
            tran.Add(Resource.Id.ScheduleFrame, scheduleFragment, "newFragment");
            tran.Commit();
        }
        public  void setScheduleTimeAndDate(List<CScore.BCL.Course> courses)
        {
            foreach (var c in courses)
                scheduleFragment.setCourseDayAndTime(c);
          
        }
        public static void showSchedule()
        {
            var newFragment = UOTCS_android.Fragments.ScheduleDialogFragment.newInstance();
            newFragment.Show(this.FragmentManager, "Fragment");
        }
       
    }
}