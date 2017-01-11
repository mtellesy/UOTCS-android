using System;
using System.Threading.Tasks;
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
using UOTCS_android.Fragments;
using Android.Graphics;
using Fragment = Android.Support.V4.App.Fragment;
namespace UOTCS_android
{
    [Activity(Label = "Profile",Icon = "@drawable/icon", Theme = "@style/Theme.Student")]
    public class Profile : MainActivity
    {
        //       private Button view;
        //     private Button hide;
        public Fragment mCurrentFragment;
        private Username username;
        public UserInformationFragment userInformation;
        public UserMoreInfomationFragment userMoreInformation;
        Button status;
        protected override void OnCreate(Bundle bundle)
        {
            DrawerLayout mdrawerLayout;


     //       var task = Task.Run(async () => { await CScore.BCL.Semester.getCurrentSemester(); });
       //     task.Wait();


            // start the service for notifications
      //      Intent intent = new Intent(this, typeof(Services.StatusChecker));
        //    this.StartService(intent);


            // var task = Task.Run( async () => { await CScore.BCL.Semester.getCurrentSemester(); });
            //  task.Wait();

            base.OnCreate(bundle);


            // Set our view from the "main" layout resource
            if (Values.Use_typeID > 1)
            {
                SetTheme(Resource.Style.Theme_Lecturer);
            }
            else
            {
                //start the enrollment status checker
            }

            SetContentView(Resource.Layout.Profile);



        /*    if (Values.Use_typeID > 1)
            {
                butt.SetBackgroundColor(Color.ParseColor("#1abc9c"));

            }
            else
            {
                butt.SetBackgroundColor(Color.ParseColor("#ef717a"));

            }*/
            findViews();
            handleEvents();
        }



        private void findViews()
        {
            base.findViews();


            // initiating fragments
            username = new Username();
             userInformation = new UserInformationFragment();

             userMoreInformation = new UserMoreInfomationFragment();
            var trans =SupportFragmentManager.BeginTransaction();
         //   var trans2 = SupportFragmentManager.BeginTransaction();
            trans.Add(Resource.Id.UsernameFragmentContainer, username, "Username");
            trans.Add(Resource.Id.UserInformationFragmentContainer, userInformation, "User_information");
            trans.Add(Resource.Id.UserInformationFragmentContainer, userMoreInformation, "User_more_information");
            trans.Hide(userMoreInformation);
            mCurrentFragment = userInformation;
          //ce.Animation.FadeIn, Resource.Animation.FadeOut,Resource.Animation.FadeIn, Resource.Animation.FadeOut);
            trans.Commit();

        status = FindViewById<Button>(Resource.Id.status_btn);
           status.Click += status_btn_Click;
        }

        private void status_btn_Click(object sender, EventArgs e)
        {
            var trans = SupportFragmentManager.BeginTransaction();
            trans.SetCustomAnimations(Resource.Animation.FadeIn, Resource.Animation.FadeOut, Resource.Animation.FadeIn, Resource.Animation.FadeOut);
            if (mCurrentFragment == userInformation)
            {
                trans.Hide(userInformation);
                trans.Show(userMoreInformation);
                mCurrentFragment = userMoreInformation;
                trans.Commit();
                status.Text = "BACK";
            }
            else if(mCurrentFragment == userMoreInformation)
            {
                trans.Hide(userMoreInformation);
                trans.Show(userInformation);
                mCurrentFragment = userInformation;
                trans.Commit();
                status.Text = "STATUS";

            }
        }

        private void handleEvents()
        {

        }

        private void hide_Click(object sender, EventArgs e)
        {
            var trans = SupportFragmentManager.BeginTransaction();
            trans.Hide(userMoreInformation);
            trans.Show(userInformation);
            trans.Commit();
        }

        private void view_Click(object sender, EventArgs e)
        {
            var trans = SupportFragmentManager.BeginTransaction();
            trans.Hide(userInformation);
            trans.Show(userMoreInformation);
            trans.Commit();
        }


        private  void SetUpDrawerContent(NavigationView navigationView)
        {
            base.SetUpDrawerContent(navigationView);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            bool x =base.OnOptionsItemSelected(item);
            return x;
        }

        public int getCurrentActvity()
        {
            return Resource.Id.nav_announcements;
        }

        public override void OnBackPressed()
        {
            MoveTaskToBack(true);
        }


        public  void switchFragments()
        {
            if(mCurrentFragment== userInformation)
            {

            }
        }
    }
}
