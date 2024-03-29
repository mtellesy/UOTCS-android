using System;
using System.IO;
using System.Linq;
using SQLite.Net;
using System.Threading.Tasks;

using System.Collections;
using System.Collections.Generic;

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
using Android.Content.Res;
using Java.Util;

namespace UOTCS_android
{
    [Activity( MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/Theme.Student")]
    public class Login : AppCompatActivity
    {
        private Button login;
        private FrameLayout mlayout;
        private EditText userIDEditText;
        private EditText passwordEditText;
        private TextInputLayout passwordWrapper;
        private TextInputLayout usernameWrapper;

        private Status LoginStatus;

        // user ID and password
        private int userID;
        private String password;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            // setTheme(R.style.MyAppTheme);
            base.OnCreate(savedInstanceState);

            // domain 
            CScore.SAL.AuthenticatorS.domain = "http://192.168.2.8/CStestAPIs";
            SetContentView(Resource.Layout.Login);
            // Create your application here
            userIDEditText = FindViewById<EditText>(Resource.Id.txtUsername);
            passwordEditText = FindViewById<EditText>(Resource.Id.txtPassword);
            usernameWrapper = FindViewById<TextInputLayout>(Resource.Id.txtInputLayoutUserName);
            passwordWrapper = FindViewById<TextInputLayout>(Resource.Id.txtInputLayoutPassword);


            login = FindViewById<Button>(Resource.Id.btnLogin);
            login.Click += login_click;

            mlayout = FindViewById<FrameLayout>(Resource.Id.frame);
            mlayout.Click += mlayout_Click;
        }

        private void mlayout_Click(object sender, EventArgs e)
        {
            InputMethodManager inputManager = (InputMethodManager)this.GetSystemService(Activity.InputMethodService);
            inputManager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.None);
        }

        async void login_click(object sender, EventArgs e)
        {

            login.Enabled = false;

            passwordWrapper.ErrorEnabled = false;
            usernameWrapper.ErrorEnabled = false;

            password = passwordWrapper.EditText.Text;
            
            if (userIDEditText.Text == "" || userIDEditText.Text == null)
            {
                userID = 0;
            }
            else 
            userID = Convert.ToInt32(userIDEditText.Text);
          
            object IntObject = userID;
            int error = 0;

            if (userID.Equals(0) || userID.ToString().Equals("") || IntObject.Equals(null))
            {
                usernameWrapper.Error = "Please Enter User ID";
                error++;
            }
            if (password.Equals("") || password.Equals(null))
            {
               passwordWrapper.Error = "Please Enter You password";
                error++;
            }
            
            if (error == 0)
            {
                LoginStatus = new Status();
                var task = Task.Run(async () => { LoginStatus = await CScore.BCL.User.login(userID, password); });
                task.Wait();
                //  this.showMessage(LoginStatus.message);
                //Intent intent = new Intent(this, typeof(ProfileStudent));
                if (LoginStatus.status)
                {

                    StatusWithObject<String> aut = new StatusWithObject<string>();
                    var task2 = Task.Run(async () => { aut = await CScore.SAL.AuthenticatorS.authenticate(); });
                    task2.Wait();
                    string x = CScore.BCL.User.use_type;
                    await this.buildDB(x);
                    this.setLanguage();

                    //   this.StartActivity(intent);
                    if (CScore.BCL.User.use_type=="S"|| CScore.BCL.User.use_type == "s")
                    {
                        Values.Use_typeID = 1;
                    }
                    else
                    {
                        Values.Use_typeID = 34;
                    }

                    Values.startProfile(this);


                    login.Enabled = true;

                }
                else
                {
                    this.showMessage("Somting went wrong!");
                }
            }

           
            login.Enabled = true;
        }

        private async Task buildDB(String userType)
        {
            // DBname 
            string dbname = "BCLV11003.db";
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

        private void setLanguage()
        {

            CScore.FixdStrings.LanguageSetter.setLanguage(CScore.FixdStrings.LanguageSetter.getLanguage());
            CScore.FixdStrings.Language current = CScore.FixdStrings.LanguageSetter.getLanguage();
            Configuration configuration = this.Resources.Configuration;
            switch (current)
            {

                case (CScore.FixdStrings.Language.EN):

                    configuration.SetLayoutDirection(Locale.English);//= LayoutDirection.Locale;
                    this.Resources.UpdateConfiguration(configuration, this.Resources.DisplayMetrics);
                    break;
                case (CScore.FixdStrings.Language.AR):
                default:

                    configuration.SetLayoutDirection(new Locale("ar"));//= LayoutDirection.Locale;
                    this.Resources.UpdateConfiguration(configuration, this.Resources.DisplayMetrics);
                    break;

            }
        }
    } 
}