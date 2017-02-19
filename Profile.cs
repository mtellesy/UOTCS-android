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
using Android.Support.V7.Widget;
using CScore.BCL;
using Android.Support.V4.App;

namespace UOTCS_android
{
    [Activity(Label = "Profile", Icon = "@drawable/icon", Theme = "@style/Theme.Student",ParentActivity = (typeof(CourseDetails)))]
    public class Profile : AppCompatActivity
    {
        //       private Button view;
        //     private Button hide;
        private Fragment mCurrentFragment;
        private Username username;
        private UserInformationFragment userInformation;
        private UserMoreInfomationFragment userMoreInformation;
        private SupportToolbar toolBar;
        private SupportActionBar actionbar;
        private DrawerLayout drawerLayout;
        private NavigationView navigationView;
        private View view;
        private CircleImageView profileImage;
        private PersonalProfileFragment personal;
        private CScore.BCL.OtherUsers lecturer;
        private long lecturer_id;
        protected override async void OnCreate(Bundle bundle)
        {
            Bundle b = Intent.Extras;
           
            base.OnCreate(bundle);
            Intent intent2 = Intent;
            if (null != intent2)
            { //Null Checking
                try
                {
                    lecturer_id = b.GetInt("lecturer_id");
                }catch
                { }


                }
            if (lecturer_id!= 0)
            {
                bindLecturerData((int)lecturer_id);
            }
            else
            {
                var task = Task.Run(async () =>
                {
                    await CScore.BCL.Semester.getCurrentSemester();
                });
                task.Wait();


                // start the service for notifications
                Intent intent = new Intent(this, typeof(Services.StatusChecker));
                this.StartService(intent);

            }
            


            // var task = Task.Run( async () => { await CScore.BCL.Semester.getCurrentSemester(); });
            //  task.Wait();
            this.Title = CScore.FixdStrings.Profile.ProfileLable();    
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
            personal = new PersonalProfileFragment();
            username = new Username();
            userInformation = new UserInformationFragment();
            userMoreInformation = new UserMoreInfomationFragment();
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
            if (lecturer!= null)
            {
                personal.lecturer = this.lecturer;
            }
            var trans = SupportFragmentManager.BeginTransaction();
            trans.Add(Resource.Id.UsernameFragmentContainer, personal, "personal");

            trans.Commit();
        }
        
        private void setUpActionBar(SupportActionBar actionBar)
        {
            if (lecturer != null)
            {
                actionBar.SetHomeButtonEnabled(true);

            }
            else
            {
                actionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);

            }


            actionBar.SetDisplayHomeAsUpEnabled(true);
            
            
        }
        private void setUpNavigationView(NavigationView navigationView)
        {
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
            navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;
           profileImage.Click += ProfileImage_Click;
        }

        private void Transcript_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(Transcript));
            this.StartActivity(intent);
     
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
                    if (lecturer != null)
                    {
                        //Intent upIntent = NavUtils.GetParentActivityIntent(this);
                        //upIntent.SetFlags(Intent.Flags);
                        //NavUtils.NavigateUpTo(this, upIntent);
                        Finish();
                    }
                    else
                    {
                        drawerLayout.OpenDrawer((int)GravityFlags.Start);
                    }
                    return true;


                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
        public override void OnBackPressed()
        {
            if (lecturer != null)
            {
                Finish();
            }
            else
            {
                MoveTaskToBack(true);
            }
        }
        
        private void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            drawerLayout.CloseDrawers();
            if (e.MenuItem.ItemId != getCurrentActvity())
            {
                Values.handleSwitchActivities(this, e.MenuItem.ItemId, navigationView);
            }
        }
        private void ProfileImage_Click(object sender, EventArgs e)
        {
            drawerLayout.CloseDrawers();
        }
        private void bindLecturerData(int lecturer_id)
        {
            StatusWithObject<OtherUsers> values = new StatusWithObject<OtherUsers>();
            try
            {
                var task = Task.Run(async () => { values = await this.getLecturer(lecturer_id); });
                task.Wait();
            }
            catch (AggregateException ex)
            {

            }
            lecturer = values.statusObject;
        }
        private async Task<CScore.BCL.StatusWithObject<OtherUsers>> getLecturer(int lecturer_id)
        {
            CScore.BCL.StatusWithObject<OtherUsers> result = new StatusWithObject<OtherUsers>();
            try
            {
                result = await CScore.BCL.OtherUsers.getOtherUser(lecturer_id);
                var x = result;
            }
            catch (Exception ex)
            {

            }
            return result;
        }

    }
}
