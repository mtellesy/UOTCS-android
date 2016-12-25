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
using Android.Views.InputMethods;

// CScore 


namespace UOTCS_android
{
    [Activity(Label = "Login", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/Theme.DesignDemo")]
    public class Login : AppCompatActivity
    {
        private Button login;
        private FrameLayout mlayout;
        protected override void OnCreate(Bundle savedInstanceState)
        {
           // setTheme(R.style.MyAppTheme);
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Login);
            // Create your application here

            login = FindViewById<Button>(Resource.Id.btnLogin);
            login.Click += login_click;

            mlayout = FindViewById<FrameLayout>(Resource.Id.frame);
            mlayout.Click += mlayout_Click;
        }

        private void mlayout_Click(object sender, EventArgs e)
        {
            InputMethodManager inputManager = (InputMethodManager)this.GetSystemService(Activity.InputMethodService);
            inputManager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken,HideSoftInputFlags.None);
        }

        void login_click(object sender,EventArgs e)
        {

            Intent intent = new Intent(this, typeof(MainActivity));
            this.StartActivity(intent);
        }
    } 
}