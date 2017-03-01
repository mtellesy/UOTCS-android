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
using Android.Support.Design.Widget;
using Android.Support.V7;
using Android.Support.V4.App;
using UOTCS_android.Fragments;
namespace UOTCS_android
{
    [Activity(Label = "Send Announcement", Icon = "@drawable/icon", Theme = "@style/Theme.Student", ParentActivity = (typeof(Messages)))]
    public class SendAnnouncement: MainActivity
    {
        SendMessageAnnouncementFragment sendAnnouncement;
        internal bool fabShouldBeShown;
        FloatingActionButton fab;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);

            Values.changeTheme(this);
            SetContentView(Resource.Layout.sendMessage);

            findViews();
            handleEvents();
        }



        private void findViews()
        {
            Android.Support.V7.Widget.Toolbar toolBar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolBar);
            fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.SetVisibility(ViewStates.Visible);
            fab.Visibility = ViewStates.Visible;
            SetSupportActionBar(toolBar);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            methodWhereFabIsHidden();

            //sendAnnouncement = new SendMessageAnnouncementFragment("Announcement");

            //var trans = SupportFragmentManager.BeginTransaction();
            //trans.Add(Resource.Id.send_message_fragment_container, sendAnnouncement, "sendAnnouncement");
            //trans.Commit();
        }

        internal FloatingActionButton.OnVisibilityChangedListener fabListener = new OnVisibilityChangedListenerAnonymousInnerClass();

        private class OnVisibilityChangedListenerAnonymousInnerClass : FloatingActionButton.OnVisibilityChangedListener
        {
            internal bool fabShouldBeShown;
            public OnVisibilityChangedListenerAnonymousInnerClass()
            {
            }

            public override void OnShown(FloatingActionButton fab)
            {
                base.OnShown(fab);
                if (!fabShouldBeShown)
                {
                    fab.Hide();
                }
            }
            public override void OnHidden(FloatingActionButton fab)
            {
                base.OnHidden(fab);
                if (fabShouldBeShown)
                {
                    fab.Show();
                }
            }
        }
        public  void methodWhereFabIsHidden()
        {
            fabShouldBeShown = false;
            fab.Hide(fabListener);
        }
        public  void methodWhereFabIsShown()
        {
            fabShouldBeShown = true;
            fab.Show(fabListener);
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
            base.OnOptionsItemSelected(item);
          if(item.ItemId == Android.Resource.Id.Home)
           {
                NavUtils.NavigateUpFromSameTask(this);
                return true;
            }

            return base.OnOptionsItemSelected(item);

        }

        public int getCurrentActvity()
        {
            return Resource.Id.nav_announcements;
        }



    }
}

