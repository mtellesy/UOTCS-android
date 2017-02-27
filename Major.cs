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