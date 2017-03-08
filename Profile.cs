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
using Refractored.Controls;
using Android.Content.Res;
using Java.Util;

namespace UOTCS_android
{
    [Activity(Label = "Profile", Icon = "@drawable/icon", Theme = "@style/Theme.Student")]
    public class Profile : AppCompatActivity
    {
        //       private Button view;
        //     private Button hide;
        private Fragment mCurrentFragment;
        private Username username;
        private UserInformationFragment userInformation;
        private UserMoreInfomationFragment userMoreInformation;
        private Button status;
        private SupportToolbar toolBar;
        private SupportActionBar actionbar;
        private DrawerLayout drawerLayout;
        private NavigationView navigationView;
        private View view;
        private CircleImageView profileImage;

        protected override void OnCreate(Bundle bundle)
        {

           
            base.OnCreate(bundle);

            var task = Task.Run(async () => { await CScore.BCL.Semester.getCurrentSemester(); });
            task.Wait();


            // start the service for notifications
            //      Intent intent = new Intent(this, typeof(Services.StatusChecker));
            //    this.StartService(intent);


            // var task = Task.Run( async () => { await CScore.BCL.Semester.getCurrentSemester(); });
            //  task.Wait();
           
            //start the enrollment status checker           
            Values.changeTheme(this);
            SetContentView(Resource.Layout.Profile);

            findViews();
            SetSupportActionBar(toolBar);
            setUpActionBar(actionbar);
            setUpNavigationView(navigationView);

            initiateFragments();
            handleEvents();
        }



        private void findViews()
        {

            username = new Username();
            userInformation = new UserInformationFragment();
            userMoreInformation = new UserMoreInfomationFragment();
            status = FindViewById<Button>(Resource.Id.status_btn);
            toolBar = FindViewById<SupportToolbar>(Resource.Id.toolBar);
            SetSupportActionBar(toolBar);
            actionbar = SupportActionBar;
            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            view = navigationView.GetHeaderView(0);
            profileImage = view.FindViewById<CircleImageView>(Resource.Id.nav_profile);

        }
        private void initiateFragments()
        {

            var trans = SupportFragmentManager.BeginTransaction();
            trans.Add(Resource.Id.UsernameFragmentContainer, username, "Username");
            trans.Add(Resource.Id.UserInformationFragmentContainer, userInformation, "User_information");
            trans.Add(Resource.Id.UserInformationFragmentContainer, userMoreInformation, "User_more_information");
            trans.Hide(userMoreInformation);
            mCurrentFragment = userInformation;
            trans.Commit();
        }

        private void setUpActionBar(SupportActionBar actionBar)
        {
            actionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);
            
            actionBar.SetDisplayHomeAsUpEnabled(true);
        }
        private void setUpNavigationView(NavigationView navigationView)
        {
            navigationView.ItemIconTintList = null;
            Values.changeNavigationItems(navigationView, this);
            if (navigationView != null)
            {
                SetUpDrawerContent(navigationView);
            }
        }
        private void SetUpDrawerContent(NavigationView navigationView)
        {
            Values.handleSetUpDrawerContent(navigationView, drawerLayout);
        }

        private void handleEvents()
        {
            status.Click += status_btn_Click;
            navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;
            profileImage.Click += ProfileImage_Click;

        }
        public int getCurrentActvity()
        {
            return Resource.Id.nav_profile;
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    drawerLayout.OpenDrawer((int)GravityFlags.Start);
                    return true;


                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
        public override void OnBackPressed()
        {
            MoveTaskToBack(true);
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
            else if (mCurrentFragment == userMoreInformation)
            {
                trans.Hide(userMoreInformation);
                trans.Show(userInformation);
                mCurrentFragment = userInformation;
                trans.Commit();
                status.Text = "STATUS";

            }
        }
        private void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            drawerLayout.CloseDrawers();
            if (e.MenuItem.ItemId != getCurrentActvity())
            {
                Values.handleSwitchActivities(this, e.MenuItem.ItemId);
            }
        }
        private void ProfileImage_Click(object sender, EventArgs e)
        {
            drawerLayout.CloseDrawers();
        }
    }
}
