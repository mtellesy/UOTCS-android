using System;
using System.Threading;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;


using CScore.BCL;

namespace Services
{

    [Service]
    public class StatusChecker : Service
    {

      //  static readonly string TAG = "X:" + typeof(StatusChecker).Name;
        static readonly int TimerWait = 60000; 
        Timer timer;
        DateTime startTime;
        bool isStarted = false;
        

        public override void OnCreate()
        {
            base.OnCreate();
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
         //   Log.Debug(TAG, $"OnStartCommand called at {startTime}, flags={flags}, startid={startId}");
            if (isStarted)
            {
                TimeSpan runtime = DateTime.UtcNow.Subtract(startTime);
                //Log.Debug(TAG, $"This service was already started, it's been running for {runtime:c}.");
            }
            else
            {
                startTime = DateTime.UtcNow;
              //  Log.Debug(TAG, $"Starting the service, at {startTime}.");
                timer = new Timer(HandleTimerCallback, startTime, 0, TimerWait);
                isStarted = true;
            }
            return StartCommandResult.NotSticky;
        }

        public override IBinder OnBind(Intent intent)
        {
            // This is a started service, not a bound service, so we just return null.
            return null;
        }


        public override void OnDestroy()
        {
            timer.Dispose();
            timer = null;
            isStarted = false;

            TimeSpan runtime = DateTime.UtcNow.Subtract(startTime);
           // Log.Debug(TAG, $"Simple Service destroyed at {DateTime.UtcNow} after running for {runtime:c}.");
            base.OnDestroy();
        }
      
        async void HandleTimerCallback(object state)
        {
            TimeSpan runTime = DateTime.UtcNow.Subtract(startTime);
          if(await CScore.BCL.Enrollment.isEnrollmentEnabled()) 
            {
                // when enrollment is available get the current semester
           await CScore.BCL.Semester.getCurrentSemester();
                if(!UOTCS_android.NotificationsRepo.isEnrollmentNotified)
                {
                    UOTCS_android.NotificationsRepo.enrollmentNotification(this);
                    UOTCS_android.NotificationsRepo.isEnrollmentNotified = true;
                }
                
            }
            else
            {
                UOTCS_android.NotificationsRepo.isEnrollmentNotified = false;
            }

            if (await CScore.BCL.Major.isMajorEnabled())
            {
                if (!UOTCS_android.NotificationsRepo.isMajorNotified)
                {
                    UOTCS_android.NotificationsRepo.majorNotification(this);
                    UOTCS_android.NotificationsRepo.isMajorNotified = true;
                    UOTCS_android.Values.majorVisible = true;
                }

            }
            else
            {
                UOTCS_android.NotificationsRepo.isMajorNotified = false;
                UOTCS_android.Values.majorVisible = false;
            }
            var anoNot = await CScore.SAL.AnnouncementsS.getNotifications();
            if (anoNot.status.status && anoNot.statusObject.Count > 0)
            {
                if(anoNot.statusObject.Count > 1)
                {
                    UOTCS_android.NotificationsRepo.newAnnouncementNotification(this, anoNot.statusObject.Count);
                }
                else
                {
                    UOTCS_android.NotificationsRepo.newAnnouncementNotification(this);
                }
            }

            var messNot = await CScore.SAL.MessageS.getNotifications();
            if (messNot.status.status && messNot.statusObject.Count > 0)
            {
                if (messNot.statusObject.Count > 1)
                {
                    UOTCS_android.NotificationsRepo.newMessagesNotification(this, messNot.statusObject.Count);
                }
                else
                {
                    UOTCS_android.NotificationsRepo.newMessagesNotification(this);
                }
            }
            }


        }
    }
