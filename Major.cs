using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using System.Linq;
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

namespace UOTCS_android
{
    [Activity(Label = "Major",Icon = "@drawable/icon", Theme = "@style/Theme.Student",ParentActivity = typeof(Profile))]
    public class Major : AppCompatActivity
    {
        private SupportToolbar toolBar;
        private SupportActionBar actionbar;
        private DrawerLayout drawerLayout;
        private NavigationView navigationView;
        private View view;
        private CircleImageView profileImage;

        public static TextView total_credit;
        
        protected override void OnPause()
        {
            base.OnPause();
            this.Finish();
        }

        protected async override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Values.changeTheme(this);
                this.Title = CScore.FixdStrings.Major.MajorTitle();
            SetContentView(Resource.Layout.Major);
            var MajorButton = FindViewById<FloatingActionButton>(Resource.Id.fab);
            MajorButton.SetImageDrawable( Resources.GetDrawable( Resource.Drawable.ic_save));
            MajorButton.Visibility = ViewStates.Visible;
            try
            { 
            if (Int32.Parse(CScore.BCL.User.dep_id) <= 1)
                if (await CScore.BCL.Major.isMajorEnabled())
                {
                    CScore.BCL.StatusWithObject<List<CScore.BCL.Department>> Deparments =
                    await CScore.BCL.Major.getAvailableDepartments();
                    var majorAdapter = new MajorAdapter(this, Deparments.statusObject, false);
                    var contactsListView = FindViewById<ListView>(Resource.Id.myMajorListView);
                    contactsListView.Adapter = majorAdapter;

                    MajorButton.Click += async (sender, e) => {
                        if (majorAdapter.FinalDepID == -1 || majorAdapter.FinalDepID == 0)
                        {
                            //the user didn't choose any department
                            showMessage(CScore.FixdStrings.Major.PleaseChooseAdepartmentFirst());
                        }
                        else
                        {
                            var myDep = Deparments.statusObject.Where(i => i.Dep_id.Equals(majorAdapter.FinalDepID)).First();
                            CScore.BCL.StatusWithObject<String> result;
                            if (myDep != null)
                                result = await CScore.BCL.Major.major(myDep);
                            showMajorSucceededMessage();
                        }

                    };
                }
                else
                {
                    showMajorNoAvailable();
                }
            else showMajorNoAvailable();

            }
             catch(Exception ex) {   showMessage(CScore.SAL.FixedResponses.getResponse(0)); }

            findViews();
            SetSupportActionBar(toolBar);
            setUpActionBar(actionbar);
            setUpNavigationView(navigationView);
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
            navigationView.SetCheckedItem(Resource.Id.nav_major);
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
            return Resource.Id.nav_major;
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
                Values.handleSwitchActivities(this, e.MenuItem.ItemId, navigationView);
            }
        }
        private void ProfileImage_Click(object sender, EventArgs e)
        {
            drawerLayout.CloseDrawers();
            Intent intent = new Intent(this, typeof(Profile));
            this.StartActivity(intent);
            Finish();
        }
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        private void showMessage(String message)
        {
            Android.Support.V7.App.AlertDialog.Builder alert =
           new Android.Support.V7.App.AlertDialog.Builder(this);
            alert.SetTitle(CScore.FixdStrings.Major.MajorStatus());
            alert.SetMessage(message);
            //alert.SetPositiveButton("OK", (senderAlert, args) => {
            //    Toast.MakeText(this, "", ToastLength.Short).Show();
            //});

            Dialog x = alert.Create();
            x.Show();
        }

        private void showMajorNoAvailable()
        {
            Android.Support.V7.App.AlertDialog.Builder alert =
            new Android.Support.V7.App.AlertDialog.Builder(this);
            alert.SetTitle(CScore.FixdStrings.Major.MajorStatus());
            alert.SetMessage(CScore.FixdStrings.Major.SorryMajorisNotAvailable());
            alert.SetPositiveButton(CScore.FixdStrings.Buttons.DONE(), (senderAlert, args) => {
                Intent intent = new Intent(this,typeof(Profile));
                StartActivity(intent);
                this.Finish();
            });

            Dialog x = alert.Create();
            x.Show();
        }

        private void showMajorSucceededMessage()
        {
            Android.Support.V7.App.AlertDialog.Builder alert =
            new Android.Support.V7.App.AlertDialog.Builder(this);
            alert.SetTitle(CScore.FixdStrings.Major.MajorStatus());
            alert.SetMessage(CScore.FixdStrings.Major.YouHaveBeenSuccessfullyMajored());
            alert.SetNeutralButton(CScore.FixdStrings.Buttons.DONE(), (senderAlert, args) => {
                Intent intent = new Intent(this, typeof(Profile));
                StartActivity(intent);
                this.Finish();
            });

            Dialog x = alert.Create();
            x.SetCancelable(false);
            x.Show();
        }
    }
}