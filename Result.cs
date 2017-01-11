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
using UOTCS_android.Fragments;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;

namespace UOTCS_android
{
    [Activity(Label = "Result")]
    public class Result : MainActivity
    {

        public ResultFragment result;
        public UserMoreInfomationFragment userMoreInformation;
        Button status;
        protected override void OnCreate(Bundle bundle)
        {
            DrawerLayout mdrawerLayout;


            //       var task = Task.Run(async () => { await CScore.BCL.Semester.getCurrentSemester(); });
            //     task.Wait();


            // start the service for notifications
            //      Intent intent = new Intent(this, typeof(Services.StatusChecker));
            //    this.StartService(intent);


            // var task = Task.Run( async () => { await CScore.BCL.Semester.getCurrentSemester(); });
            //  task.Wait();

            base.OnCreate(bundle);


            // Set our view from the "main" layout resource
            Values.changeTheme(this);

            SetContentView(Resource.Layout.Results);



           
            findViews();
            handleEvents();
        }



        private void findViews()
        {
            base.findViews();


            // initiating fragments
            result = new ResultFragment();
            userMoreInformation = new UserMoreInfomationFragment();
            var trans = SupportFragmentManager.BeginTransaction();

            trans.Add(Resource.Id.ResultFragmentContainerResult, result, "result");
            trans.Add(Resource.Id.UserInformationFragmentContainerResult, userMoreInformation, "User_more_information");
     
            trans.Commit();

            
        }

        
        private void handleEvents()
        {

        }


        private void SetUpDrawerContent(NavigationView navigationView)
        {
            base.SetUpDrawerContent(navigationView);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            bool x = base.OnOptionsItemSelected(item);
            return x;
        }

        public int getCurrentActvity()
        {
            return Resource.Id.nav_announcements;
        }

        public override void OnBackPressed()
        {
            MoveTaskToBack(true);
        }

    }
}