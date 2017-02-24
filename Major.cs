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


namespace UOTCS_android
{
    [Activity(Label = "Major",Icon = "@drawable/icon", Theme = "@style/Theme.Student",ParentActivity = typeof(Profile))]
    public class Major : MainActivity
    {
        public static TextView total_credit;
        
        protected override void OnPause()
        {
            base.OnPause();
            this.Finish();
           
        }

        protected async override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            
            SetContentView(Resource.Layout.Major);
            var MajorButton = FindViewById<FloatingActionButton>(Resource.Id.fab);
            MajorButton.Visibility = ViewStates.Visible;
            try
            {
                if (await CScore.BCL.Major.isMajorEnabled())
                {
                    CScore.BCL.StatusWithObject<List<CScore.BCL.Department>> Deparments =
                    await CScore.BCL.Major.getAvailableDepartments();
                    var majorAdapter = new MajorAdapter(this, Deparments.statusObject, false);
                    var contactsListView = FindViewById<ListView>(Resource.Id.myMajorListView);
                    contactsListView.Adapter = majorAdapter;

                    MajorButton.Click += async (sender, e) => {
                    
                    };


                }
                else
                {
                    showMajorNoAvailable();
                }

            }
            catch(Exception ex) {   showMessage(CScore.SAL.FixedResponses.getResponse(0) + ex.Message); }



            findViews();
            handleEvents();
        }



        private void findViews()
        {
            base.findViews();
        }

        private void handleEvents()
        {

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
        public  int getCurrentActvity()
        {
            return Resource.Id.nav_timetable;
        }

        private void showMessage(String message)
        {
            Android.Support.V7.App.AlertDialog.Builder alert =
           new Android.Support.V7.App.AlertDialog.Builder(this);
            alert.SetTitle("Major Status");
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
            alert.SetTitle("Major Status");
            alert.SetMessage("Sorry Major is Not Available");
            alert.SetPositiveButton("OK", (senderAlert, args) => {
                Intent intent = new Intent(this,typeof(Profile));
                StartActivity(intent);
                this.Finish();
            });

            Dialog x = alert.Create();
            x.Show();
        }
    }
}