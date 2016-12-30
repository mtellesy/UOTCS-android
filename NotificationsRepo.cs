using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using SupportActionBar = Android.Support.V7.App.ActionBar;
using Android.Support.V4.App;
using TaskStackBuilder = Android.Support.V4.App.TaskStackBuilder;

using Android.Support.V4.Widget;
using SupportFragment = Android.Support.V4.App.Fragment;
using SupportFragmentManager = Android.Support.V4.App.FragmentManager;

using Android.Support.V7.App;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using UOTCS_android.Fragments;

namespace UOTCS_android
{
   class NotificationsRepo 
    {
        static NotificationManager notificationManager;
        public static bool isEnrollmentNotified = false;
        public static bool isMajorNotified = false;
        //start from 1000 and step is +1000
        static readonly int EnrollmentStatusNotificationID = 1000;
        static readonly int MajoringStatusNotificationID = 2000;

        public static void enrollmentNotification(Context x)
        {
            
            Android.Support.V4.App.NotificationCompat.Builder builder =
            new Android.Support.V4.App.NotificationCompat.Builder(x)
            .SetAutoCancel(true)
            .SetContentTitle("Enrollment Status")
            .SetSmallIcon(Resource.Drawable.Icon)
            .SetContentText("Enrollment is Available");

            notificationManager = (NotificationManager)x.GetSystemService(Context.NotificationService);

            notificationManager.Notify(EnrollmentStatusNotificationID, builder.Build());

        }

        public static void majorNotification(Context x)
        {

            Android.Support.V4.App.NotificationCompat.Builder builder =
            new Android.Support.V4.App.NotificationCompat.Builder(x)
            .SetAutoCancel(true)
            .SetContentTitle("Major Status")
            .SetSmallIcon(Resource.Drawable.Icon)
            .SetContentText("Major is Available");

            notificationManager = (NotificationManager)x.GetSystemService(Context.NotificationService);

            notificationManager.Notify(MajoringStatusNotificationID, builder.Build());

        }


    }
}