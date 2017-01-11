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

namespace UOTCS_android
{
    public static  class Values
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
                
                if (Values.use_typeID>1)
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
                    Values.startAnnouncement(myActivity);
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

        }

    }
}