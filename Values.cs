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
using Android.Graphics;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Support.V4.Widget;

namespace UOTCS_android
{
    public static class Values
    {
        private static int use_typeID;

        public static int Use_typeID
        {
            set
            {
                use_typeID = value;
            }
            get
            {
                return use_typeID;
            }
        }
        private static Color use_color;
        public static Color Use_Color
        {
            set
            {

                if (Values.use_typeID > 1)
                {
                    use_color = new Color(0, 150, 136);
                }
                else
                {
                    use_color = new Color(63, 81, 181);
                }

            }
            get
            {
                return use_color;
            }
        }
        private static Color use_color_accent;
        public static Color Use_Color_accent
        {
            set
            {

                if (Values.use_typeID > 1)
                {
                    use_color_accent = new Color(255, 110, 64);
                }
                else
                {
                    use_color_accent = new Color(255, 82, 82);
                }

            }
            get
            {
                return use_color_accent;
            }
        }


        public static void changeTheme(Context myActivity)
        {
            if (Values.Use_typeID > 1)
            {
                myActivity.SetTheme(Resource.Style.Theme_Lecturer);
            }
            else if (Values.use_typeID == 1)
            {
                myActivity.SetTheme(Resource.Style.Theme_Student);
            }
        }
        public static void startAnnouncement(Context myActivity)
        {
            Intent intent;
            if (Values.Use_typeID > 1)
            {
                intent = new Intent(myActivity, typeof(AnnouncementsLecturer));
                myActivity.StartActivity(intent);
            }
            else
            {
                intent = new Intent(myActivity, typeof(Announcements));
                myActivity.StartActivity(intent);
            }

        }
        public static void switchActivitiesStudent(Context myActivity, int itemId)
        {
            Intent intent;
            switch (itemId)
            {
                case Resource.Id.nav_announcements:
                    intent = new Intent(myActivity, typeof(Announcements));
                    myActivity.StartActivity(intent);
                    break;
                case Resource.Id.nav_messages:
                    intent = new Intent(myActivity, typeof(Messages));
                    myActivity.StartActivity(intent); break;
                case Resource.Id.nav_myCourses:
                    intent = new Intent(myActivity, typeof(MyCourses));
                    myActivity.StartActivity(intent); break;
                case Resource.Id.nav_schedule:
                    intent = new Intent(myActivity, typeof(Schedule));
                    myActivity.StartActivity(intent); break;
                case Resource.Id.nav_timetable:
                    intent = new Intent(myActivity, typeof(Timetable));
                    myActivity.StartActivity(intent); break;
                case Resource.Id.nav_enrollment:
                    intent = new Intent(myActivity, typeof(Enrollment));
                    myActivity.StartActivity(intent); break;
                case Resource.Id.nav_major:
                    intent = new Intent(myActivity, typeof(Major));
                    myActivity.StartActivity(intent); break;
                case Resource.Id.nav_result:
                    intent = new Intent(myActivity, typeof(Result));
                    myActivity.StartActivity(intent); break;
            }
        }
        public static void switchActivitiesLecturer(Context myActivity, int itemId)
        {

            Intent intent;
            switch (itemId)
            {
                case Resource.Id.nav_announcements:
                    intent = new Intent(myActivity, typeof(AnnouncementsLecturer));
                    myActivity.StartActivity(intent);
                    break;
                case Resource.Id.nav_messages:
                    intent = new Intent(myActivity, typeof(Messages));
                    myActivity.StartActivity(intent); break;
                case Resource.Id.nav_myCourses:
                    intent = new Intent(myActivity, typeof(MyCourses));
                    myActivity.StartActivity(intent); break;
                case Resource.Id.nav_schedule:
                    intent = new Intent(myActivity, typeof(Schedule));
                    myActivity.StartActivity(intent); break;
                case Resource.Id.nav_timetable:
                    intent = new Intent(myActivity, typeof(Timetable));
                    myActivity.StartActivity(intent); break;
            }
        }
        public static void changeNavigationItems(NavigationView nav_view, Context myActivity)
        {
            nav_view.Menu.Clear();
            View hView = nav_view.GetHeaderView(0);
            ImageView navHeaderBackgrpound = (ImageView)hView.FindViewById(Resource.Id.nav_header_image);
            ImageView navProfile = (ImageView)hView.FindViewById(Resource.Id.nav_profile);
            Values.Use_Color = new Color();

            if (Values.Use_typeID > 1)
            {
                nav_view.InflateMenu(Resource.Menu.drawer_view_lecturer);
                navHeaderBackgrpound.SetImageDrawable(myActivity.Resources.GetDrawable(Resource.Drawable.nav_header_lecturer));
                var drawable = Android.Ui.TextDrawable.TextDrawable.TextDrwableBuilder.BeginConfig().UseFont(Typeface.Default).FontSize(50).ToUpperCase().Height(60).Width(60).TextColor(Values.Use_Color)
                .EndConfig().BuildRound("x", new Color(250, 250, 250));
                navProfile.SetImageDrawable(drawable);
            }
            else
            {
                nav_view.InflateMenu(Resource.Menu.drawer_view);
                navHeaderBackgrpound.SetImageDrawable(myActivity.Resources.GetDrawable(Resource.Drawable.nav_header_student));
                var drawable = Android.Ui.TextDrawable.TextDrawable.TextDrwableBuilder.BeginConfig().UseFont(Typeface.Default).FontSize(40).ToUpperCase().Height(60).Width(60).TextColor(Values.Use_Color)
               .EndConfig().BuildRound("x", new Color(250, 250, 250));
                navProfile.SetImageDrawable(drawable);
            }
        }
        public static void handleOnBackPressed(Context myActivity)
        {
            Intent intent = new Intent(myActivity, typeof(Profile));
            myActivity.StartActivity(intent);
        }
        public static void handleSwitchActivities(Context myActivity, int itemId)
        {
            if (Values.Use_typeID > 1)
            {
                Values.switchActivitiesLecturer(myActivity, itemId);
            }
            else if (Values.use_typeID == 1)
            {
                Values.switchActivitiesStudent(myActivity, itemId);
            }
        }
        public static void handleSetUpDrawerContent(NavigationView navigationView, DrawerLayout drawerLayout)
        {
            navigationView.NavigationItemSelected += (object sender, NavigationView.NavigationItemSelectedEventArgs e) =>
            {
                e.MenuItem.SetChecked(true);
                drawerLayout.CloseDrawers();
            };
        }
        public static List<TimetableAndMidmarkAndroid> timetableMaker(CScore.BCL.Semester semester)
        {
            List<TimetableAndMidmarkAndroid> result = new List<TimetableAndMidmarkAndroid>();
            TimetableAndMidmarkAndroid temp;
            temp = new TimetableAndMidmarkAndroid("Term name", semester.Ter_nameAR);
            result.Add(temp);
            temp = new TimetableAndMidmarkAndroid("Term name EN", semester.Ter_nameEN);
            result.Add(temp);
            temp = new TimetableAndMidmarkAndroid("year", semester.Year);
            result.Add(temp);
            temp = new TimetableAndMidmarkAndroid("Term start", semester.Ter_start);
            result.Add(temp);
            temp = new TimetableAndMidmarkAndroid("Term enrollment", semester.Ter_enrollment);
            result.Add(temp);
            temp = new TimetableAndMidmarkAndroid("Drop Courses", semester.Ter_dropCourses);
            result.Add(temp); 
            foreach(CScore.BCL.Exams x in semester.Exam)
            {
                temp = new TimetableAndMidmarkAndroid(x.ExamTypeEN, x.StartDate);
                result.Add(temp);
            }
            temp = new TimetableAndMidmarkAndroid("last study date", semester.Ter_lastStudyDate);
            result.Add(temp);
            return result;
        }

    }
}