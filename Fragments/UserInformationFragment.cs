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
using Android.Support.V4.App;
using CScore.FixdStrings;

namespace UOTCS_android.Fragments
{
    public class UserInformationFragment : Android.Support.V4.App.Fragment
    {

        private FragmentActivity myContext;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }
        public override void OnAttach(Activity activity)
        {
            base.OnAttach(activity);
            myContext = (FragmentActivity)activity;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            //
            View view = inflater.Inflate(Resource.Layout.UserInfomationsF, container, false);
            TextView EmailLable = view.FindViewById<TextView>(Resource.Id.emailTV);
            EmailLable.Text = CScore.FixdStrings.Profile.UserEmail();
            TextView Email = view.FindViewById<TextView>(Resource.Id.emailTVB);
            Email.Text = CScore.BCL.User.use_email;
            TextView PhoneLable = view.FindViewById<TextView>(Resource.Id.phoneTV);
            PhoneLable.Text = CScore.FixdStrings.Profile.UserPhone();
            TextView Phone = view.FindViewById<TextView>(Resource.Id.phoneTVB);
            Phone.Text = CScore.BCL.User.use_phone.ToString();
            TextView DepartmentLable = view.FindViewById<TextView>(Resource.Id.departmentTV);
            DepartmentLable.Text = CScore.FixdStrings.Profile.UserDepartment();
            TextView Department = view.FindViewById<TextView>(Resource.Id.departmentTVB);
            Language e = LanguageSetter.getLanguage();
            switch (e)
            {
                case (Language.AR):  Department.Text = CScore.BCL.User.dep_nameAR;
                    break;

                case (Language.EN):
                default: Department.Text = CScore.BCL.User.dep_nameEN;
                    break;
            }
           
            return view;
        }
        
    }
}