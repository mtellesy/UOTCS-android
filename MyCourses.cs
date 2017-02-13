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
using Refractored.Controls;
using System.Collections.Generic;
using UOTCS_android.Fragments;
using System.Threading.Tasks;

namespace UOTCS_android
{
    [Activity(Label = "MyCourses", Icon = "@drawable/icon", Theme = "@style/Theme.Student")]
    public class MyCourses : AppCompatActivity
    {
        private SupportToolbar toolBar;
        private SupportActionBar actionbar;
        private DrawerLayout drawerLayout;
        private NavigationView navigationView;
        private View view;
        private CircleImageView profileImage;
        private List<ResultAndroid> myCoursesList;
        private List<ResultAndroid> myCourses;
        private ResultFragment myCoursesfragment;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            this.Title = CScore.FixdStrings.Courses.MyCoursesLable();           
            Values.changeTheme(this);
            SetContentView(Resource.Layout.MyCourses);

            findViews();
            SetSupportActionBar(toolBar);
            setUpActionBar(actionbar);
            setUpNavigationView(navigationView);
            bindData();
            initiateFragments();
            handleEvents();
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
            myCoursesList = new List<ResultAndroid>();
            myCoursesfragment = new ResultFragment();
        }
        private void initiateFragments()
        {
            var trans = SupportFragmentManager.BeginTransaction();
            myCoursesfragment.results = myCoursesList;
            trans.Add(Resource.Id.MyCoursesFragmentContainer, myCoursesfragment, "result");
            trans.Commit();
        }
        private void bindData()
        {
            List<CScore.BCL.Course> x = new List<CScore.BCL.Course>();

            var coursesStatus = new CScore.BCL.StatusWithObject<List<CScore.BCL.Course>>();

            ResultAndroid temp2;

            var task = Task.Run(async () =>
            {
                coursesStatus = await CScore.BCL.Course.getUserCoursesSchedule();
            }
                );
            task.Wait();
            if (coursesStatus.statusObject != null)
                x = coursesStatus.statusObject;

            //to remove any repeated courses
            var z = new List<CScore.BCL.Course>();
            foreach (CScore.BCL.Course y in x)
            {
                int r = 0;
                if (z.Count != 0)
                {
                    foreach (var i in z)
                    {

                        if (y.Cou_id == i.Cou_id && y.Schedule[0].Gro_id == i.Schedule[0].Gro_id)
                            r++;
                    }
                    if (r == 0) z.Add(y);
                }
                else z.Add(y);
            }
            x = z;
            foreach (CScore.BCL.Course y in x)
            {
                temp2 = new ResultAndroid(y);
                myCoursesList.Add(temp2);
            }

        }

        private void setUpActionBar(SupportActionBar actionBar)
        {
            actionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);
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

        }
        public int getCurrentActvity()
        {
            return Resource.Id.nav_myCourses;
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

        private void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            drawerLayout.CloseDrawers();
            if (e.MenuItem.ItemId != getCurrentActvity())
                Values.handleSwitchActivities(this, e.MenuItem.ItemId, navigationView);

        }
        private void ProfileImage_Click(object sender, EventArgs e)
        {
            drawerLayout.CloseDrawers();
            Intent intent = new Intent(this, typeof(Profile));
            this.StartActivity(intent);
            Finish();
        }

    }
}