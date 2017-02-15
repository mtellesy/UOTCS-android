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
   public class ListViewAdapterResults : BaseAdapter<ResultAndroid>
    {
        public List<ResultAndroid> results;
        Activity context;
        public ListViewAdapterResults(List<ResultAndroid> results, Activity context) : base()
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
        public override ResultAndroid this[int position]
        {
            get { return results[position]; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? context.LayoutInflater.Inflate(
                Resource.Menu.List_row_results, parent, false);
            var rowCode = view.FindViewById<TextView>(Resource.Id.list_row_results_code);
            var rowName = view.FindViewById<TextView>(Resource.Id.list_row_results_name);
            var rowTotal = view.FindViewById<TextView>(Resource.Id.list_row_results_result);
            rowCode.Text = results[position].Code;
            rowName.Text = results[position].Name;
            if (results[position].Group!=null)
            {
                Values.Use_Color_accent = new Color();
                rowTotal.Text = results[position].Group;
                rowCode.SetTextColor(Values.Use_Color_accent);
                LinearLayout lin = view.FindViewById<LinearLayout>(Resource.Id.list_row_results_background);
      //          lin.SetBackgroundColor(new Color(250, 250, 250));
            }
            else
            {
                rowTotal.Text = results[position].Result.ToString();
                float x = results[position].Result;
                if (results[position].Result < 50)
                {
                    rowTotal.SetTextColor(Color.Red);
                }
                else
                {
                    rowTotal.SetTextColor(Color.Black);
                }
                if (results[position].Result== -1)
                {
                    rowTotal.SetTextColor(Color.Black);
                    rowTotal.Text = CScore.FixdStrings.Results.notSet();
                }
                else
                {
                    rowTotal.Text= results[position].Result.ToString();
                }
            }
            
            
            return view;
        }
    }
}