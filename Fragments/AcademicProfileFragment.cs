using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using SupportFragment = Android.Support.V4.App.Fragment;

using Android.Support.V7.Widget;
using UOTCS_android.Helpers;
using CScore.BCL;
using System.Threading.Tasks;
using Android.Graphics;
using Android.Support.V4.App;
using CScore.FixdStrings;

namespace UOTCS_android.Fragments
{
    public class AcademicProfileFragment : SupportFragment
    {
        private FragmentActivity myContext;
        private TextView department;
        private TextView currentsemester;
        private TextView unitscompleted;
        private TextView warnings;
        private TextView gpa;
        private TextView departmentLabel;
        private TextView currentsemesterLabel;
        private TextView unitscompletedLabel;
        private TextView warningsLabel;
        private TextView gpaLabel;
        private int user_id;
        private Language e;
        private Button transcript;
        private Button profile;
        public Course course;
        string activityName;
        private OtherUsers lecturer;
        View view;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override void OnAttach(Activity activity)
        {
            base.OnAttach(activity);
            myContext = (FragmentActivity)activity;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            if (IsAdded)
            {
                activityName = Activity.Class.SimpleName;
            }

            if (activityName == "ProfileStudent")
            {
                view = inflater.Inflate(Resource.Layout.academicProfile, container, false);
            }
            else if (activityName == "CourseDetails")
            {
                 view = inflater.Inflate(Resource.Layout.CourseDetailsF, container, false);
                lecturer = new OtherUsers();
                bindLecturerData();

            }


            department = view.FindViewById<TextView>(Resource.Id.department_textview);
            currentsemester = view.FindViewById<TextView>(Resource.Id.current_semester_textview);
            unitscompleted = view.FindViewById<TextView>(Resource.Id.units_completed_textview);
            warnings = view.FindViewById<TextView>(Resource.Id.warnings_textview);
            gpa = view.FindViewById<TextView>(Resource.Id.gpa_textview);
            departmentLabel = view.FindViewById<TextView>(Resource.Id.department_label);
            currentsemesterLabel = view.FindViewById<TextView>(Resource.Id.current_semester_label);
            unitscompletedLabel = view.FindViewById<TextView>(Resource.Id.units_completed_label);
            warningsLabel = view.FindViewById<TextView>(Resource.Id.warnings_label);
            gpaLabel = view.FindViewById<TextView>(Resource.Id.gpa_label);
            profile = view.FindViewById<Button>(Resource.Id.profile_btn);
            transcript = view.FindViewById<Button>(Resource.Id.transcript_btn);
           
            bingData();
            handleLanguage();
            e = LanguageSetter.getLanguage();

            transcript.Click += Transcript_Click;
            profile.Click += Profile_Click;
            return view;
        }

        private void Profile_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(Activity, typeof(Profile));
            Bundle b = new Bundle();
            //   b.PutInt("lecturer_id", course.Tea_id);
            intent.PutExtra("lecturer_id",lecturer.use_id);
            this.StartActivity(intent);
        }

        private void handleLanguage()
        {
            if (activityName == "ProfileStudent")
            {
                departmentLabel.Text = CScore.FixdStrings.Profile.UserDepartment();
                currentsemesterLabel.Text = CScore.FixdStrings.Profile.currentSemester();

                unitscompletedLabel.Text = CScore.FixdStrings.Profile.UserUnits();
                warningsLabel.Text = CScore.FixdStrings.Profile.UserNotices();
                gpaLabel.Text = CScore.FixdStrings.Profile.UserGPA();
                transcript.Text = CScore.FixdStrings.Transcript.TranscriptLable();
                profile.Visibility=ViewStates.Gone;
            }
            else if(activityName== "CourseDetails")
            {
                departmentLabel.Text = CScore.FixdStrings.Courses.courseNameAR();
                currentsemesterLabel.Text = CScore.FixdStrings.Courses.courseNameEN();
                unitscompletedLabel.Text = CScore.FixdStrings.Courses.lecturerNameAR();
                warningsLabel.Text = CScore.FixdStrings.Courses.lecturerNameEN();
                gpaLabel.Text = CScore.FixdStrings.Courses.courseCredits();
                transcript.Text = CScore.FixdStrings.Courses.sendMessage();
                profile.Visibility = ViewStates.Visible;
            }      
   
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Transcript_Click(object sender, EventArgs e)
        {
            if (activityName == "ProfileStudent")
            {
                Intent intent = new Intent(Activity, typeof(Transcript));
                this.StartActivity(intent);
            }
            else if (activityName == "CourseDetails")
            {
                Intent intent = new Intent(Activity, typeof(SendMessage));
                Bundle b = new Bundle();
                b.PutInt("lecturer_id",lecturer.use_id);
                intent.PutExtras(b);
                this.StartActivity(intent);
            }
        }

        private void bingData()
        {
            if (activityName == "ProfileStudent")
            {
                switch (e)
                {
                    case (Language.AR):
                        department.Text = CScore.BCL.User.dep_nameAR;
                        break;

                    case (Language.EN):
                    default:
                        department.Text = CScore.BCL.User.dep_nameEN;
                        break;
                }
            }
            else if (activityName == "CourseDetails")
            {
                if (course != null)
                {
                    bindLecturerData();
                    department.Text = course.Cou_nameAR;
                    currentsemester.Text = course.Cou_nameEN;
                    if (lecturer != null)
                    {
                        unitscompleted.Text = lecturer.use_nameAR;
                        warnings.Text = lecturer.use_nameEN;
                        profile.Visibility = ViewStates.Visible;
                        transcript.Visibility = ViewStates.Visible;
                    }
                    else
                    {
                        unitscompleted.Text = CScore.FixdStrings.General.notAvailable();
                        warnings.Text = CScore.FixdStrings.General.notAvailable();
                        profile.Visibility = ViewStates.Gone;
                        transcript.Visibility = ViewStates.Gone;
                    }
                    gpa.Text = course.Cou_credits.ToString();
                }
            }
        }
              private void bindLecturerData()
        {
            StatusWithObject<OtherUsers> values = new StatusWithObject<OtherUsers>();
            try
            {
                var task = Task.Run(async () => { values = await this.getLecturer(); });
                task.Wait();
            }
            catch (AggregateException ex)
            {

            }
            lecturer = values.statusObject;
        }
        private async Task<CScore.BCL.StatusWithObject<OtherUsers>> getLecturer()
        {
            CScore.BCL.StatusWithObject<OtherUsers> result = new StatusWithObject<OtherUsers>();
            try
            {
                //  result = await CScore.BCL.OtherUsers.getOtherUser(course.Tea_id);
               var r = await CScore.BCL.Course.getUserCoursesSchedule();
                foreach(var y in r.statusObject)
                {
                    if(y.Cou_id==course.Cou_id)
                    {
                        result = await CScore.BCL.OtherUsers.getOtherUser(y.Schedule[0].Tea_id);
                        var x = result;
                    }
                }
               
            }
            catch (Exception ex)
            {

            }
            return result;
        }

    
    }
}