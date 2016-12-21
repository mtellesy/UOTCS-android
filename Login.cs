using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using SupportActionBar = Android.Support.V7.App.ActionBar;

using Android.Support.V4.Widget;
using SupportFragment = Android.Support.V4.App.Fragment;
using SupportFragmentManager = Android.Support.V4.App.FragmentManager;

using Android.Support.V7.App;
using Android.Support.Design.Widget;
using Android.Support.V4.View;


namespace UOTCS_android
{
    [Activity(Label = "Login", Icon = "@drawable/icon", Theme = "@style/Theme.DesignDemo")]
    public class Login : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
           // setTheme(R.style.MyAppTheme);
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Login);
            // Create your application here
        }
    }
}