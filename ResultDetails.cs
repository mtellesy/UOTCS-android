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
using System.Threading.Tasks;

namespace UOTCS_android
{
    [Activity(Label = "ResultDetails",ParentActivity = (typeof(Result)))]
    public class ResultDetails : AppCompatActivity
    {

        private SupportToolbar toolBar;
        private SupportActionBar actionbar;
        private DrawerLayout drawerLayout;
        private NavigationView navigationView;
        private View view;
        private CircleImageView profileImage;
        private List<TimetableAndMidmarkAndroid> resultDetailList;
        private TimetableAndMidmarkFragment resultDetailsFrament;
        private TextView totalLable;
        private TextView total;
        private TextView midExamNameLable;
        private TextView resultLable;
        private string course_code;
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            Values.changeTheme(this);
            SetContentView(Resource.Layout.ResultDetails);
            Intent intent = this.Intent;
            Bundle b = this.Intent.Extras;

            course_code = b.GetString("course_id");
            this.Title = course_code;
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
            total = FindViewById<TextView>(Resource.Id.total_resultDetailsLayout);
            totalLable = FindViewById<TextView>(Resource.Id.totalLable_resultDetailsLayout);
            resultLable = FindViewById<TextView>(Resource.Id.result_resultDetailsLayout);
            midExamNameLable = FindViewById<TextView>(Resource.Id.midexamName_resultDetailsLayout);
            totalLable.Text = CScore.FixdStrings.Results.Total();
            resultLable.Text = CScore.FixdStrings.Results.Result();
            midExamNameLable.Text = CScore.FixdStrings.General.Name();


            toolBar = FindViewById<SupportToolbar>(Resource.Id.toolBar);
            SetSupportActionBar(toolBar);
            actionbar = SupportActionBar;
            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            view = navigationView.GetHeaderView(0);
            profileImage = view.FindViewById<CircleImageView>(Resource.Id.nav_profile);
            resultDetailList = new List<TimetableAndMidmarkAndroid>();
            resultDetailsFrament = new TimetableAndMidmarkFragment();
        }
        private void initiateFragments()
        {
            var trans = SupportFragmentManager.BeginTransaction();
            resultDetailsFrament.data = resultDetailList;
            trans.Add(Resource.Id.ResultDetailsFragmentContainerResult, resultDetailsFrament, "result");
            trans.Commit();

        }
             private void bindData()
             {
                float totalInt = 0;
                List<CScore.BCL.MidMarkDistribution> x = new List<MidMarkDistribution>();

                TimetableAndMidmarkAndroid temp2;


                var statusOB = new StatusWithObject<List<CScore.BCL.Result>>();
            
                var task2 = Task.Run(async () =>
                {
                    statusOB = await CScore.BCL.Result.getSemesterResults();
                });
                task2.Wait();

                if (statusOB.statusObject != null)
                {
                var course = statusOB.statusObject.Where(i => i.Cou_id.Equals(course_code)).First();
                x = course.MidExams;
                }

                foreach (MidMarkDistribution y in x)
                        {
                            temp2 = new TimetableAndMidmarkAndroid(y);
                            totalInt += y.Grade;
                            resultDetailList.Add(temp2);
                        }
            total.Text = totalInt.ToString();
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
            navigationView.SetCheckedItem(Resource.Id.nav_result);
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
            return Resource.Id.nav_result;
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