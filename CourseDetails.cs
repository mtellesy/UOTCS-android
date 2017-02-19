using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SupportFragment = Android.Support.V4.App.Fragment;

using Android.Support.V7.Widget;
using UOTCS_android.Helpers;
using CScore.BCL;
using System.Threading.Tasks;
using Android.Graphics;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using SupportActionBar = Android.Support.V7.App.ActionBar;

using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using Refractored.Controls;
using Android.Support.V4.App;
using Android.Support.V7.App;
using UOTCS_android.Fragments;

namespace UOTCS_android
{
    [Activity(Label = "CourseDetails", ParentActivity = (typeof(MyCourses)))]
    public class CourseDetails : AppCompatActivity
    {
        private SupportToolbar toolBar;
        private SupportActionBar actionbar;
        private DrawerLayout drawerLayout;
        private NavigationView navigationView;
        private View view;
        private CircleImageView profileImage;
        //    private CourseNameFragment courseNameFragment;
        private AcademicProfileFragment coursedetails;
        private Button sendMessage;
        private string course_id;
        private Course course;

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            Intent intents = Intent;
            if (null != intents)
            { //Null Checking
                course_id = intents.GetStringExtra("course_id");
                if (course_id != null)
                {
                    this.Title = course_id;
                }
                else
                {
                    if (course != null)
                    {
                        this.Title = course.Cou_id;
                    }
                }
            }
            Values.changeTheme(this);
            SetContentView(Resource.Layout.CourseDetails);
            this.findViews();
            bindData();
            initiateFragments();
            SetSupportActionBar(toolBar);
            setUpActionBar(actionbar);
            setUpNavigationView(navigationView);
            this.handleEvents();
       //     course_id = savedInstanceState.GetString("id");
          
    
        }




        private void findViews()
        {
            toolBar = FindViewById<SupportToolbar>(Resource.Id.toolBar);
            SetSupportActionBar(toolBar);
            actionbar = SupportActionBar;
            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            view = navigationView.GetHeaderView(0);
            profileImage = view.FindViewById<CircleImageView>(Resource.Id.nav_profile);
     //       sendMessage = FindViewById<Button>(Resource.Id.send_message_btn);
            //        courseNameFragment = new CourseNameFragment();
            coursedetails = new AcademicProfileFragment();
            
            course = new Course();
        }
        private void initiateFragments()
        {
            coursedetails.course = course;
            var trans = SupportFragmentManager.BeginTransaction();
            trans.Add(Resource.Id.MyCoursesDetailsFragmentContainer, coursedetails, "course_)details");
            trans.Commit();

        }
        private void setUpActionBar(SupportActionBar actionBar)
        {
            actionBar.SetHomeButtonEnabled(true);
            actionBar.SetDisplayHomeAsUpEnabled(true);
        }
        private void setUpNavigationView(NavigationView navigationView)
        {
            Values.changeNavigationItems(navigationView, this);
            if (navigationView != null)
            {
                SetUpDrawerContent(navigationView);
            }
            navigationView.SetCheckedItem(Resource.Id.nav_myCourses);
        }
        private void SetUpDrawerContent(NavigationView navigationView)
        {
            Values.handleSetUpDrawerContent(navigationView, drawerLayout);
        }
        private void handleEvents()
        {
            navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;
            profileImage.Click += ProfileImage_Click;
    //        sendMessage.Click += SendMessage_Click;
        }

        private void SendMessage_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(SendMessage));
            Bundle b = new Bundle();
            b.PutInt("id",course.Tea_id);
            this.StartActivity(intent);
        }

        public int getCurrentActvity()
        {
            return Resource.Id.nav_myCourses;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            base.OnOptionsItemSelected(item);
            if (item.ItemId == Android.Resource.Id.Home)
            {
                NavUtils.NavigateUpFromSameTask(this);
                return true;
            }

            return base.OnOptionsItemSelected(item);

        }
        public override void OnBackPressed()
        {
            NavUtils.NavigateUpFromSameTask(this);

        }
        private void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            drawerLayout.CloseDrawers();
            if (e.MenuItem.ItemId != getCurrentActvity())
            {
                Values.handleSwitchActivities(this, e.MenuItem.ItemId, navigationView);
                Finish();
            }

        }
        private void ProfileImage_Click(object sender, EventArgs e)
        {
            drawerLayout.CloseDrawers();
            Values.startProfile(this);
            Finish();
        }

        private void bindData()
        {
            StatusWithObject<List<Course>> values = new StatusWithObject<List<Course>>();
            try
            {
                var task = Task.Run(async () => { values = await this.getCourse(); });
                task.Wait();
            }
            catch (AggregateException ex)
            {

            }
            course = values.statusObject.First();
        }
        private async  Task<CScore.BCL.StatusWithObject<List<CScore.BCL.Course>>> getCourse()
        {
            CScore.BCL.StatusWithObject<List<Course>> result = new StatusWithObject<List<Course>>();
            try
            {
                result = await CScore.BCL.Course.getCourses(course_id);
            }
            catch(Exception ex)
            {
               
            }
            return result;
        }

    }
}

