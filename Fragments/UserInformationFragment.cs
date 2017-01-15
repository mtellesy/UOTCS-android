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
        
            return view;
        }
        
    }
}