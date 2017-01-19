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
using Android.Support.V7.App;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using SupportActionBar = Android.Support.V7.App.ActionBar;

using Android.Support.V4.Widget;
using SupportFragment = Android.Support.V4.App.Fragment;
using SupportFragmentManager = Android.Support.V4.App.FragmentManager;

using CScore.BCL;
using static Android.Views.View;
using Refractored.Controls;
using Android.Content;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using UOTCS_android.Fragments;
using Java.Lang;

namespace UOTCS_android
{
    [Activity(Label = "Transcript", ParentActivity = (typeof(Profile)))]
    public class Transcript : AppCompatActivity
    {
        private ResultFragment result;

        private SupportToolbar toolBar;
        private SupportActionBar actionbar;
        private DrawerLayout drawerLayout;
        private NavigationView navigationView;
        private View view;
        private CircleImageView profileImage;
        private List<ResultAndroid> resultshere;

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            Values.changeTheme(this);
            SetContentView(Resource.Layout.transcript);
            this.findViews();

            findViews();
            SetSupportActionBar(toolBar);
            setUpActionBar(actionbar);
            setUpNavigationView(navigationView);
            bindData();
            initiateFragments();
            this.handleEvents();
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
            resultshere = new List<ResultAndroid>();
            result = new ResultFragment();
            ScrollView scrollView = FindViewById<ScrollView>(Resource.Id.layout);
            scrollView.SmoothScrollingEnabled = true;
            //    scrollView.FullScroll(FocusSearchDirection.Up);
        }
        private void bindData()
        {
            List<CScore.BCL.AllResult> x = new List<AllResult>();
            AllResult temp = new AllResult();
            ResultAndroid temp2;
            for (int i = 0; i < 10; i++)
            {
                temp = new AllResult();
                temp.Cou_nameEN = "course name" + i;
                temp.Cou_id = "ITGS10" + i;
                temp.Res_total = i * 10;
                x.Add(temp);
            }
            foreach (AllResult y in x)
            {
                temp2 = new ResultAndroid(y);
                resultshere.Add(temp2);
            }

        }
        private void initiateFragments()
        {
            var trans = SupportFragmentManager.BeginTransaction();
            result.results = resultshere;
            trans.Add(Resource.Id.ResultFragmentContainerResult, result, "result");

            //   trans.Add(Resource.Id.UserInformationFragmentContainerResult, userMoreInformation, "User_more_information");
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
            navigationView.SetCheckedItem(Resource.Id.nav_profile);
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
            return Resource.Id.nav_profile;
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




    }
}