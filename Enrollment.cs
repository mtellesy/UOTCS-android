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
    [Activity(Label = "Enrollment", Icon = "@drawable/icon", Theme = "@style/Theme.Student")]
    public class Enrollment : AppCompatActivity
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
            SetContentView(Resource.Layout.Enrollment);

            

            var enrollButton = FindViewById<FloatingActionButton>(Resource.Id.fab);
            enrollButton.Visibility = ViewStates.Visible;
            try
            {
                if (await CScore.BCL.Enrollment.isEnrollmentEnabled())
                {

                        var allowedCreditTitle = FindViewById<TextView>(Resource.Id.enrollmentAllowedCreditsTitle);
                        allowedCreditTitle.Text = CScore.FixdStrings.Enrollment.AvaialabeCreditToEnroll();
                        var totalTitle = FindViewById<TextView>(Resource.Id.enrollmentTotalCreditsTitle);
                        totalTitle.Text = CScore.FixdStrings.Enrollment.TotalCredits();
                        CScore.BCL.StatusWithObject<List<CScore.BCL.Course>> Courses =
                        await CScore.BCL.Enrollment.getEnrollableCourses();
                        var availableCredit = FindViewById<TextView>(Resource.Id.enrollmentAllowedCredits);
      
                        total_credit = FindViewById<TextView>(Resource.Id.enrollmentCurrentTotalCredits);
                        availableCredit.Text = CScore.BCL.Enrollment.creditMax.ToString();
                        total_credit.Text = CScore.BCL.Enrollment.getCreditSum().ToString();
                        var enrollmentAdapter = new EnrollmentAdapter(this, Courses.statusObject, false);
                        var contactsListView = FindViewById<ListView>(Resource.Id.myEnrollmentListView);
                        contactsListView.Adapter = enrollmentAdapter;

                    enrollButton.Click += async (sender, e) => {

                        // Droped Coures: message
                        String DropMessage = CScore.FixdStrings.Enrollment.dropedCourses();
                        foreach (var drop in CScore.BCL.Enrollment.dropedCourses)
                        {
                            var dropResults = await CScore.BCL.Enrollment.dropCourse(drop, drop.TemGro_id);
                            DropMessage += drop.Cou_id + " " + CScore.FixdStrings.General.Status() + ": " + dropResults.status.status.ToString() +
                            "\n";
                        }


                        var results = await CScore.BCL.Enrollment
                           .enrollCourse(CScore.BCL.Enrollment.enrolledCourses, "true");
                        String Message = "";
                        if (results.status.status)
                        {
                            Message += CScore.FixdStrings.Enrollment.enrollmentSucceededMessage();
                            var Cres = (List<CScore.BCL.Course>)results.statusObject;
                            foreach (var c in Cres)
                            {
                                Message += CScore.FixdStrings.Courses.CourseCode() + ": " + c.Cou_id + " "+CScore.FixdStrings.General.Status() +" " +
                                c.Flag + "\n";
                            }
                        }

                        Message += " " + DropMessage;
                        if (CScore.BCL.Enrollment.dropedCourses.Count > 0 || CScore.BCL.Enrollment.enrolledCourses.Count > 0)

                            this.showEnrollmentDoneMessage(CScore.FixdStrings.Enrollment.enrollmentStatus(), Message);
                        else
                            this.showEnrollmentDoneMessage(CScore.FixdStrings.Enrollment.enrollmentStatus(), CScore.FixdStrings.Enrollment.nothingIsChangedMessage());

                    };


                }
                else if (await CScore.BCL.Enrollment.isDisEnrollmentEnabled())
                {

                    var allowedCreditTitle = FindViewById<TextView>(Resource.Id.enrollmentAllowedCreditsTitle);
                    allowedCreditTitle.Text = CScore.FixdStrings.Enrollment.AvaialabeCreditToEnroll();
                    var totalTitle = FindViewById<TextView>(Resource.Id.enrollmentTotalCreditsTitle);
                    totalTitle.Text = CScore.FixdStrings.Enrollment.TotalCredits();

                    CScore.BCL.StatusWithObject<List<CScore.BCL.Course>> Courses =
                    await CScore.BCL.Course.getUserCoursesSchedule();
                    if (Courses.statusObject != null)
                    {
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
                        titleForAvailableCredits.Text = CScore.FixdStrings.Enrollment.AvaialabeCreditTodrop();
                        var availableCredit = FindViewById<TextView>(Resource.Id.enrollmentAllowedCredits);
                        total_credit = FindViewById<TextView>(Resource.Id.enrollmentCurrentTotalCredits);
                        total_credit.Text = CScore.BCL.Enrollment.getCreditSum().ToString();
                        availableCredit.Text = CScore.BCL.Enrollment.creditMin.ToString();


                        var enrollmentAdapter = new EnrollmentAdapter(this, Courses.statusObject, true);

                        var contactsListView = FindViewById<ListView>(Resource.Id.myEnrollmentListView);
                        contactsListView.Adapter = enrollmentAdapter;

                        enrollButton.Click += async (sender, e) =>
                        {
                            if (CScore.BCL.Enrollment.dropedCourses.Count > 0)
                            {
                                String DropMessage = CScore.FixdStrings.Enrollment.dropedCourses();
                                foreach (var drop in CScore.BCL.Enrollment.dropedCourses)
                                {
                                    var dropResults = await CScore.BCL.Enrollment.dropCourse(drop, drop.TemGro_id);
                                    DropMessage += drop.Cou_id + " " + CScore.FixdStrings.General.Status() + ": " + dropResults.status.status.ToString() +
                                    "\n";
                                }
                                this.showEnrollmentDoneMessage(CScore.FixdStrings.Enrollment.disenrollmentStatus(), DropMessage);
                            }
                            else
                                this.showEnrollmentDoneMessage(CScore.FixdStrings.Enrollment.disenrollmentStatus(), CScore.FixdStrings.Enrollment.nothingIsChangedMessage());
                        };
                    }
                    else { showEnrollmentDoneMessage(CScore.FixdStrings.Enrollment.disenrollmentStatus(), CScore.FixdStrings.Enrollment.noCoursesToDrop()); } 

                    }
                else
                {
                    showEnrollmentNoAvailable();
                    // Intent intent = new Intent(this, typeof(Profile));
                    // this.StartActivity(intent);
                }


                ////add Schedule Fragment
                //UOTCS_android.Fragments.ScheduleFragment myFragment = new UOTCS_android.Fragments.ScheduleFragment();
                //var tran = SupportFragmentManager.BeginTransaction();
                //tran.Add(Resource.Id.ScheduleFrame, myFragment, "newFragment");
                //tran.Commit();
            }
            catch (Exception ex) { this.showMessage(CScore.FixdStrings.General.Error(), CScore.SAL.FixedResponses.getResponse(0)); }

            findViews();
            SetSupportActionBar(toolBar);
            setUpActionBar(actionbar);
            setUpNavigationView(navigationView);
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

        }
        private void setUpActionBar(SupportActionBar actionBar)
        {
            actionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);
            actionBar.SetDisplayHomeAsUpEnabled(true);
        }
        private void handleEvents()
        {
            navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;
            profileImage.Click += ProfileImage_Click;

        }
        private void setUpNavigationView(NavigationView navigationView)
        {
            Values.changeNavigationItems(navigationView, this);
            if (navigationView != null)
            {
                SetUpDrawerContent(navigationView);
            }
            navigationView.SetCheckedItem(Resource.Id.nav_enrollment);
        }
        private void SetUpDrawerContent(NavigationView navigationView)
        {
            Values.handleSetUpDrawerContent(navigationView, drawerLayout);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    drawerLayout.OpenDrawer((int)GravityFlags.Left);
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
        public int getCurrentActvity()
        {
            return Resource.Id.nav_enrollment;
        }

        public override void OnBackPressed()// needs modifications
        {
            MoveTaskToBack(true);
        }
        private void initiateFragments()
        { 
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




        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        private void showMessage(String title, String message)
        {
            Android.Support.V7.App.AlertDialog.Builder alert =
           new Android.Support.V7.App.AlertDialog.Builder(this);
            alert.SetTitle(title);
            alert.SetMessage(message);
            //alert.SetPositiveButton("OK", (senderAlert, args) => {
            //    Toast.MakeText(this, "", ToastLength.Short).Show();
            //});

            Dialog x = alert.Create();
            x.Show();
        }

        private void showEnrollmentDoneMessage(String title, String message)
        {
            Android.Support.V7.App.AlertDialog.Builder alert =
          new Android.Support.V7.App.AlertDialog.Builder(this);
            alert.SetTitle(title);
            alert.SetMessage(message);
            alert.SetNeutralButton(CScore.FixdStrings.Buttons.DONE(), (senderAlert, args) => {
                Intent intent = new Intent(this, typeof(Profile));
                this.StartActivity(intent);
            });

            Dialog x = alert.Create();
            x.SetCancelable(false);
            x.Show();
        }
        private void showEnrollmentNoAvailable()
        {
            Android.Support.V7.App.AlertDialog.Builder alert =
           new Android.Support.V7.App.AlertDialog.Builder(this);
            alert.SetTitle(CScore.FixdStrings.Enrollment.enrollmentNotAvailable());
            alert.SetMessage(CScore.FixdStrings.Enrollment.enrollmentNotAvailable());
            alert.SetNeutralButton(CScore.FixdStrings.Buttons.DONE(), (senderAlert, args) => {
                Intent intent = new Intent(this, typeof(Profile));
                this.StartActivity(intent);
            });

            Dialog x = alert.Create();
            x.SetCancelable(false);
            x.Show();
        }
        //++++++++++++++++++++++++++++++++++++++++++++++++++++=++++++++++++++++++++
    }
}
