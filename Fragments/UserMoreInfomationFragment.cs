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
    public class UserMoreInfomationFragment : Android.Support.V4.App.Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.UserMoreInformationF, container, false);
            //  Button butt = (Button)view.FindViewById(Resource.Id.);
            // butt.Click += butt_Click;
            TextView unitsLable = view.FindViewById<TextView>(Resource.Id.unitsTV);
            unitsLable.Text = CScore.FixdStrings.Profile.UserUnits();

            TextView units = view.FindViewById<TextView>(Resource.Id.unitsTVB);
            // unitsLable.Text = 

            TextView GPALable = view.FindViewById<TextView>(Resource.Id.GPATV);
            GPALable.Text = CScore.FixdStrings.Profile.UserGPA();
            TextView GPA = view.FindViewById<TextView>(Resource.Id.GPATVB);
            //GPA.Text =

            TextView NoticesLable = view.FindViewById<TextView>(Resource.Id.noticesTV);
            NoticesLable.Text = CScore.FixdStrings.Profile.UserNotices();
            TextView Notices = view.FindViewById<TextView>(Resource.Id.noticesTVB);
           // Notices.Text = 

            return view;
        }

        private void butt_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}