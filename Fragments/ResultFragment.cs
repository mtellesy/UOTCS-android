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
    public class ResultFragment : SupportFragment
    {
        public List<ResultAndroid> results;
        string activityName;
        View view;
        ListViewAdapterResults adapter;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            if (IsAdded)
            {
                activityName= Activity.Class.SimpleName;
            }
            if (activityName=="Result")
            {
                view = inflater.Inflate(Resource.Layout.ResultF, container, false);

                ListView listView = view.FindViewById<ListView>(Resource.Id.ResultListView);
                 adapter = new ListViewAdapterResults(results, Activity);
               
                listView.Adapter = adapter;
                listView.ItemClick += ListView_ItemClick;
            }
            else if (activityName == "Transcript")
            {
                view = inflater.Inflate(Resource.Layout.transcriptF, container, false);
                NonScrollListView listView = view.FindViewById<NonScrollListView>(Resource.Id.ResultListView);
                adapter = new ListViewAdapterResults(results, Activity);
                listView.Adapter = adapter;

            }
            else if (activityName == "MyCourses")
            {
                view = inflater.Inflate(Resource.Layout.ResultF, container, false);
                ListView listView = view.FindViewById<ListView>(Resource.Id.ResultListView);
                adapter = new ListViewAdapterResults(results, Activity);
                listView.Adapter = adapter;

            }
            return view;
        }

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var item = this.adapter.results[e.Position];
            Intent intent = new Intent(Activity, typeof(ResultDetails));
            this.StartActivity(intent);
        }
    }
}