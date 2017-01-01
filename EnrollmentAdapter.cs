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
using CScore.BCL;
using Android.Provider;

namespace UOTCS_android
{
    public class EnrollmentAdapter : BaseAdapter
    {
        List<CourseItem> CoursesItemsList;
        Activity _activity;

        public EnrollmentAdapter(Activity activity,List<CScore.BCL.Course> courses)
        {
            _activity = activity;
            FillContacts(courses);
        }

        void FillContacts(List<CScore.BCL.Course> courses)
        {

            int id = 0;
            CoursesItemsList = new List<CourseItem>();
            foreach(CScore.BCL.Course course in courses)
            {
                CourseItem x = new CourseItem();
                x.CourseID = course.Cou_id;
                x.Groups = new List<String>();
                int i = 0;
                foreach(CScore.BCL.Schedule sch in course.Schedule)
                {
                    x.Groups.Add(sch.Gro_NameEN);
                }
                x.Id++;
                CoursesItemsList.Add(x);
            }
            
           
        }

        class CourseItem
        {
            public long Id { get; set; }
            public String CourseID { get; set; }
            public List<string> Groups { get; set; }
            
           
        }

        public override int Count
        {
            get { return CoursesItemsList.Count; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            // could wrap a Contact in a Java.Lang.Object
            // to return it here if needed
            return null;
        }

        public override long GetItemId(int position)
        {
            return CoursesItemsList[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? _activity.LayoutInflater.Inflate(
                Resource.Layout.EnrollmentItemView, parent, false);
            var CourseCode = view.FindViewById<TextView>(Resource.Id.enrollCourseText);
            var GroupSpinner = view.FindViewById<Spinner>(Resource.Id.enrollGroupsList);
            var EnrollButton = view.FindViewById<Button>(Resource.Id.enrollButton);
            CourseCode.Text = CoursesItemsList[position].CourseID;

            var adapter = new ArrayAdapter<String>(_activity, Android.Resource.Layout.SimpleSpinnerItem, CoursesItemsList[position].Groups);
            GroupSpinner.Adapter = adapter;
            GroupSpinner.SetSelection(0);

           
            return view;
        }
    }
}