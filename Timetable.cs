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
using UOTCS_android.Fragments;
using System.Collections.Generic;
using CScore.BCL;
using System.Threading.Tasks;

namespace UOTCS_android
{
    [Activity(Label = "Timetable", Icon = "@drawable/icon", Theme = "@style/Theme.Student")]
    public class Timetable : AppCompatActivity
    {

        private SupportToolbar toolBar;
        private SupportActionBar actionbar;
        private DrawerLayout drawerLayout;
        private NavigationView navigationView;
        private View view;
        private CircleImageView profileImage;
        private List<TimetableAndMidmarkAndroid> timetableList;
        private TimetableAndMidmarkFragment timetableFrament;
        protected override void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);
            this.Title = CScore.FixdStrings.Timetable.TimetableLable();
            Values.changeTheme(this);
            SetContentView(Resource.Layout.Timetable);

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
            timetableList = new List<TimetableAndMidmarkAndroid>();
            timetableFrament = new TimetableAndMidmarkFragment();
        }
        private void initiateFragments()
        {
            var trans = SupportFragmentManager.BeginTransaction();
            timetableFrament.data = timetableList;
            trans.Add(Resource.Id.TimetableFragmentContainer, timetableFrament, "result");
            trans.Commit();

        }
        private void bindData()
        {
            Semester temp = new Semester();
            StatusWithObject<Semester> semester = new StatusWithObject<Semester>();
            var task = Task.Run(async () =>
            {
                semester = await Semester.getCurrentSemester();
            }
                
                );
            task.Wait();
            if(semester.statusObject != null)
            {
                temp = semester.statusObject;
                timetableList = Values.timetableMaker(temp);
                
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
            navigationView.SetCheckedItem(Resource.Id.nav_timetable);
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
            return Resource.Id.nav_timetable;
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
            {
                Values.handleSwitchActivities(this, e.MenuItem.ItemId);
            }

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
