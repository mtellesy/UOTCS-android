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

namespace UOTCS_android.Fragments
{
    public class EnrollmentItemFragment : Android.Support.V4.App.Fragment
    {

        public String courseCode;
        public String courseGroup;
        Button enrollButton;
        TextView courseTextView;
        Spinner groupSpinner;
        View view;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

             view = inflater.Inflate(Resource.Layout.EnrollmentItemView, container, false);
            courseTextView = view.FindViewById<TextView>(Resource.Id.enrollCourseText);
            groupSpinner = view.FindViewById<Spinner>(Resource.Id.enrollGroupsList);
            enrollButton = view.FindViewById<Button>(Resource.Id.enrollButton);
          //  view = this.getViews(view);
            return view;
        }
        
        public void addEnrollmentItem(CScore.BCL.Course course)
        {
            courseCode = course.Cou_id;
            
            courseTextView.Text = course.Cou_id;

            List<string> groups = new List<string>();
            foreach(CScore.BCL.Schedule sch in course.Schedule)
            {
                groups.Add(sch.Gro_NameEN);
            }

            var adapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerItem, groups);
            groupSpinner.Adapter = adapter;
            groupSpinner.SetSelection(0);
        }

    }
}