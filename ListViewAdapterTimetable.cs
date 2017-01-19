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
using Android.Graphics;

namespace UOTCS_android
{
   public class ListViewAdapterTimetable : BaseAdapter<TimetableAndMidmarkAndroid>
    {
        public List<TimetableAndMidmarkAndroid> results;
        Activity context;
        public ListViewAdapterTimetable(List<TimetableAndMidmarkAndroid> results,Activity context):base()
        {
            this.results = results;
            this.context = context;
        }
        public override int Count
        {
            get { return results.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }
        public override TimetableAndMidmarkAndroid this[int position]
        {
            get { return results[position]; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? context.LayoutInflater.Inflate(
                Resource.Menu.List_row_timetable, parent, false);
            var rowName = view.FindViewById<TextView>(Resource.Id.list_row_timetable_name);
            var rowValue = view.FindViewById<TextView>(Resource.Id.list_row_timetable_result);
            rowName.Text = results[position].Name;
            rowValue.Text = results[position].Value;
            
            return view;
        }
    }
}