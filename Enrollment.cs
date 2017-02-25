using System;
using System.Collections.Generic;
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
    [Activity(Label = "Enrollment",Icon = "@drawable/icon", Theme = "@style/Theme.Student")]
    public class Enrollment : MainActivity
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

            
            SetContentView(Resource.Layout.Enrollment);
            var enrollButton = FindViewById<FloatingActionButton>(Resource.Id.fab);
            enrollButton.Visibility = ViewStates.Visible;
            try
            {
                if (await CScore.BCL.Enrollment.isEnrollmentEnabled())
                {
                    CScore.BCL.StatusWithObject<List<CScore.BCL.Course>> Courses =
                    await CScore.BCL.Enrollment.getEnrollableCourses();
                    var availableCredit = FindViewById<TextView>(Resource.Id.enrollmentAllowedCredits);
                    total_credit = FindViewById<TextView>(Resource.Id.enrollmentCurrentTotalCredits);
                    availableCredit.Text = CScore.BCL.Enrollment.creditMax.ToString();
                    total_credit.Text = CScore.BCL.Enrollment.getCreditSum().ToString();
                    var enrollmentAdapter = new EnrollmentAdapter(this, Courses.statusObject, false);
                    var contactsListView = FindViewById<ListView>(Resource.Id.myEnrollmentListView);
                    contactsListView.Adapter = enrollmentAdapter;

                    enrollButton.Click += (sender, e) => { this.showMessage(CScore.BCL.Enrollment.enrolledCourses.Count.ToString()
                         + CScore.BCL.Enrollment.dropedCourses.Count.ToString());
                       var x = CScore.BCL.Enrollment.getReservedTime();
                        x.ToList(); ;
                    };


                }
                else if (await CScore.BCL.Enrollment.isDisEnrollmentEnabled())
                {
                    CScore.BCL.StatusWithObject<List<CScore.BCL.Course>> Courses =
                    await CScore.BCL.Course.getUserCoursesSchedule();

                    // since the courses are repeated we need to remove the repeated courses
                    List<CScore.BCL.Course> disCourses = new List<CScore.BCL.Course>();
                    // courses with credit
                    var CoursesCredit = await CScore.BCL.Course.getStudentCourses();

                    var c = Courses.statusObject.Select(i => i.Cou_id).Distinct();

                    foreach (String courseID in c.ToList())
                    {
                        CScore.BCL.Course CourseWithInfo = new CScore.BCL.Course();
                        CourseWithInfo =
                        Courses.statusObject.Where(i => i.Cou_id.Equals(courseID)).First();
                        CourseWithInfo.Cou_credits =
                        CoursesCredit.statusObject.Where(i => i.Cou_id.Equals(courseID)).First().Cou_credits;

                        disCourses.Add(CourseWithInfo);
                    }

                    Courses.statusObject = disCourses;
                    var titleForAvailableCredits = FindViewById<TextView>(Resource.Id.enrollmentAllowedCreditsTitle);
                    titleForAvailableCredits.Text = "Available Credits to drop:";
                    var availableCredit = FindViewById<TextView>(Resource.Id.enrollmentAllowedCredits);
                    total_credit = FindViewById<TextView>(Resource.Id.enrollmentCurrentTotalCredits);
                    total_credit.Text = CScore.BCL.Enrollment.getCreditSum().ToString();
                    availableCredit.Text = CScore.BCL.Enrollment.creditMin.ToString();
                   

                    var enrollmentAdapter = new EnrollmentAdapter(this, Courses.statusObject, true);

                    var contactsListView = FindViewById<ListView>(Resource.Id.myEnrollmentListView);
                    contactsListView.Adapter = enrollmentAdapter;

                }
                else
                {
                    showMessage("Sorry Enrollment is not Enabled");
                    // Intent intent = new Intent(this, typeof(Profile));
                    // this.StartActivity(intent);
                }


                ////add Schedule Fragment
                //UOTCS_android.Fragments.ScheduleFragment myFragment = new UOTCS_android.Fragments.ScheduleFragment();
                //var tran = SupportFragmentManager.BeginTransaction();
                //tran.Add(Resource.Id.ScheduleFrame, myFragment, "newFragment");
                //tran.Commit();
            }
            catch { showMessage(CScore.SAL.FixedResponses.getResponse(0)); }



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
            alert.SetTitle("Login Status");
            alert.SetMessage(message);
            //alert.SetPositiveButton("OK", (senderAlert, args) => {
            //    Toast.MakeText(this, "", ToastLength.Short).Show();
            //});

            Dialog x = alert.Create();
            x.Show();
        }

    }
}