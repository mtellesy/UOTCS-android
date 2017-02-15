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
        private int user_id;
        private Language e;
        private Button transcript;
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
            View view = inflater.Inflate(Resource.Layout.academicProfile, container, false);
            department = view.FindViewById<TextView>(Resource.Id.department_textview);
            currentsemester = view.FindViewById<TextView>(Resource.Id.current_semester_textview);
            unitscompleted = view.FindViewById<TextView>(Resource.Id.units_completed_textview);
            warnings = view.FindViewById<TextView>(Resource.Id.warnings_textview);
            gpa = view.FindViewById<TextView>(Resource.Id.gpa_textview);
            transcript = view.FindViewById<Button>(Resource.Id.transcript_btn);
            bingData();
            e = LanguageSetter.getLanguage();
            transcript.Click += Transcript_Click;
            return view;
        }

        private void Transcript_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(Activity, typeof(Transcript));
            this.StartActivity(intent);
        }

        private void bingData()
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
    }
}