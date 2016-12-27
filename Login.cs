using System;
using System.IO;
using System.Linq;
using SQLite.Net;
using SQLite.Net.Platform.XamarinAndroid;
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
using CScore.BCL;


namespace UOTCS_android
{
    [Activity(Label = "Login", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/Theme.Student")]
    public class Login : AppCompatActivity
    {
        private Button login;
        private FrameLayout mlayout;
        private EditText userIDEditText;
        private EditText passwordEditText;

        private Status LoginStatus; 
       
        // user ID and password
        private int userID;
        private String password;
        protected override void OnCreate(Bundle savedInstanceState)
        {
           // setTheme(R.style.MyAppTheme);
            base.OnCreate(savedInstanceState);

            // domain 
            CScore.SAL.AuthenticatorS.domain = "http://192.168.1.4/CStestAPIs";
            SetContentView(Resource.Layout.Login);
            // Create your application here
            userIDEditText = FindViewById<EditText>(Resource.Id.txtUsername);
            passwordEditText = FindViewById<EditText>(Resource.Id.txtPassword);

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

        async void login_click(object sender,EventArgs e)
        {
        

            LoginStatus = await CScore.BCL.User.login(userID, password);
           

          //  this.showMessage(LoginStatus.message);
            Intent intent = new Intent(this, typeof(Profile));
            if (LoginStatus.status)
            {
                StatusWithObject<String> aut = await CScore.SAL.AuthenticatorS.authenticate();
                if (aut.status.status)
                    showMessage(aut.status.message);
                else
                    showMessage(aut.status.message);
                string x = CScore.BCL.User.use_type;
                this.buildDB(x);
               // this.StartActivity(intent);
            }
          
        }

        private async void buildDB(String userType)
        {
            // DBname 
            string dbname = "BCLV88.db";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string path = Path.Combine(documentsPath, dbname);
            await CScore.DataLayer.DBuilder.InitializeAsync(path, new SQLitePlatformAndroid(), userType);
        }
        

        private void showMessage(String message)
        {
            Android.Support.V7.App.AlertDialog.Builder alert =
           new Android.Support.V7.App.AlertDialog.Builder(this);
            alert.SetTitle("Login Status");
            alert.SetMessage(message);
            //alert.SetPositiveButton("OK", (senderAlert, args) => {
            //    Toast.MakeText(this, "", ToastLength.Short).Show();
            //});

            Dialog x = alert.Create();
            x.Show();
        }

    } 
}