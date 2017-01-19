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


using SupportFragment = Android.Support.V4.App.Fragment;

using Android.Support.V7.Widget;
using UOTCS_android.Helpers;
using CScore.BCL;
using System.Threading.Tasks;
namespace UOTCS_android.Fragments
{
    public class TimetableAndMidmarkFragment : SupportFragment
    {
        public List<TimetableAndMidmarkAndroid> data;
        string activityName;
        View view;
        ListViewAdapterTimetable adapter;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.TimetableAndMidExamF, container, false);
            NonScrollListView listView = view.FindViewById<NonScrollListView>(Resource.Id.timetableAndMidExamView);

            adapter = new ListViewAdapterTimetable(data, Activity);
            listView.Adapter = adapter;
            if (IsAdded)
            {
                activityName = Activity.Class.SimpleName;
            }
            if (activityName == "ResultDetails")
            {

        
            }
            else if (activityName == "Transcript")
            {


            }
            return view;
        }
    }
}