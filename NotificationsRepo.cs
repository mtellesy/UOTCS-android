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
            Intent intent = new Intent(x, typeof(Enrollment));
            PendingIntent contentIntent = PendingIntent.GetActivity(x, 0, intent, PendingIntentFlags.UpdateCurrent);

            Android.Support.V4.App.NotificationCompat.Builder builder =
            new Android.Support.V4.App.NotificationCompat.Builder(x)
            .SetAutoCancel(true)
            .SetContentTitle(CScore.FixdStrings.Enrollment.EnrollmentStatusNotification())
            .SetSmallIcon(Resource.Drawable.ic_info_white_24dp)
            .SetContentText(CScore.FixdStrings.Enrollment.EnrollmentIsAvailable())
            .SetContentIntent(contentIntent);
           
            
            notificationManager = (NotificationManager)x.GetSystemService(Context.NotificationService);

            notificationManager.Notify(EnrollmentStatusNotificationID, builder.Build());

        }

        public static void majorNotification(Context x)
        {
            Intent intent = new Intent(x, typeof(Major));
            
            PendingIntent contentIntent = PendingIntent.GetActivity(x,0,intent, PendingIntentFlags.UpdateCurrent);
            Android.Support.V4.App.NotificationCompat.Builder builder =
            new Android.Support.V4.App.NotificationCompat.Builder(x)
            .SetAutoCancel(true)
            .SetContentTitle(CScore.FixdStrings.Major.MajorStatus())
            .SetSmallIcon(Resource.Drawable.ic_info_white_24dp)
            .SetContentText(CScore.FixdStrings.Major.MajormentIsAvailable())
            .SetContentIntent(contentIntent);

            notificationManager = (NotificationManager)x.GetSystemService(Context.NotificationService);

            notificationManager.Notify(MajoringStatusNotificationID, builder.Build());

        }


    }
}