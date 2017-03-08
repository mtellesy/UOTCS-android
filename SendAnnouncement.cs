using System;
using System.Collections.Generic;

using Android.App;

using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.Design.Widget;
using Android.Support.V7;
using Android.Support.V4.App;
using UOTCS_android.Fragments;
using System.Threading.Tasks;
using System.Threading;
using Android.Support.V7.App;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using SupportActionBar = Android.Support.V7.App.ActionBar;

using Android.Support.V4.Widget;
using SupportFragment = Android.Support.V4.App.Fragment;
using SupportFragmentManager = Android.Support.V4.App.FragmentManager;

using CScore.BCL;
using static Android.Views.View;
using Refractored.Controls;
using Android.Content;

namespace UOTCS_android
{
    [Activity(Label = "Send Announcement", Icon = "@drawable/icon", Theme = "@style/Theme.Student", ParentActivity = (typeof(Messages)))]
    public class SendAnnouncement: AppCompatActivity
    {
        ImageButton sendButton;
        AutoCompleteTextView SendTo;
        EditText messageSubject;
        EditText messageContent;
        ArrayAdapter<String> userCourses;
        private SupportToolbar toolBar;
        private SupportActionBar actionbar;
        private DrawerLayout drawerLayout;
        private NavigationView navigationView;
        private View view;
        private CircleImageView profileImage;

        //the list of users in SendTo drop down list
        List<Course> courses;

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            this.Title = CScore.FixdStrings.Announcements.SendAnnouncementLable();
            Values.changeTheme(this);
            SetContentView(Resource.Layout.sendMessage);
            this.findViews();
            this.handleEvents();

           
            SetSupportActionBar(toolBar);
            setUpActionBar(actionbar);
            setUpNavigationView(navigationView);
           
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


            sendButton = FindViewById<ImageButton>(Resource.Id.sendMessageButton);
            sendButton.Visibility = ViewStates.Visible;

            SendTo = FindViewById<AutoCompleteTextView>(Resource.Id.send_to_message_announcement_fragment);

            messageSubject = FindViewById<EditText>(Resource.Id.title_message_announcement_fragment);

            messageContent = FindViewById<EditText>(Resource.Id.content_message_announcement_fragment);


            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            fillDropDownList();
        }

        private void setUpActionBar(SupportActionBar actionBar)
        {
            actionBar.SetHomeButtonEnabled(true);
            actionBar.SetDisplayHomeAsUpEnabled(true);
        }
        private void setUpNavigationView(NavigationView navigationView)
        {
            Values.changeNavigationItems(navigationView, this);
            if (navigationView != null)
            {
                SetUpDrawerContent(navigationView);
            }
            navigationView.SetCheckedItem(Resource.Id.nav_announcements);
        }
        private void SetUpDrawerContent(NavigationView navigationView)
        {
            Values.handleSetUpDrawerContent(navigationView, drawerLayout);
        }

        private void handleEvents()
        {
            navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;
            profileImage.Click += ProfileImage_Click;
            sendButton.Click += SendButton_Click;

        }
        public int getCurrentActvity()
        {
            return Resource.Id.nav_announcements;
        }
       
        public override void OnBackPressed()
        {
            MoveTaskToBack(true);
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
        private void fillDropDownList()
        {
            courses = new List<Course>();
                List<String> coursesCodes = new List<string>();
                StatusWithObject<List<Course>> Courses = new StatusWithObject<List<Course>>();
                var task = Task.Run(async () => { Courses = await CScore.BCL.Course.getUserCoursesSchedule(); });
                task.Wait();
                if (Courses.status.status && Courses.statusObject != null)
                    foreach (var cou in Courses.statusObject)
                    {
                        if (cou != null && !isExited(cou.Cou_id))
                        {
                            coursesCodes.Add(cou.Cou_id);
                            courses.Add(cou);
                        }
                    }
                userCourses = new ArrayAdapter<String>(this, Resource.Layout.dropDownList_style, coursesCodes);
                SendTo.Adapter = userCourses;
                SendTo.Touch += SendTo_Touch;
                userCourses.NotifyDataSetChanged();

            
        }



    

        private async void SendButton_Click(object sender, EventArgs e)
        {
            StatusWithObject <CScore.BCL.Announcements> ReAnnouncement = new StatusWithObject<CScore.BCL.Announcements>();
            CScore.BCL.Announcements newAnnouncement = new CScore.BCL.Announcements();
            newAnnouncement.Ano_content = messageContent.Text;
            newAnnouncement.Ter_id = CScore.BCL.Semester.current_term; //messageSubject.Text;

            string name = SendTo.Text;

            if (name != "")
            {
               
                if (isExited(SendTo.Text))
                {
                    newAnnouncement.Cou_id = SendTo.Text;
                    ReAnnouncement = await CScore.BCL.Announcements.sendAnnouncement(newAnnouncement);
                    if (ReAnnouncement.status.status)
                        showMessage(CScore.FixdStrings.Announcements.AnnouncementHasSuccessfullySent(),true);
                    else
                        showMessage(CScore.FixdStrings.Announcements.AnnouncementSendFaild(),false);
                }
                else
                {
                    showMessage(CScore.FixdStrings.Courses.CourseDoesNotExist(),false);
                }

            }
            else
            {
                showMessage(CScore.FixdStrings.Courses.PleaseTypeCourseCode(),false);
            }

        }

        private void SendTo_Touch(object sender, TouchEventArgs e)
        {
            SendTo.ShowDropDown();
        }

        private void showMessage(String message,bool status)
        {
            Android.Support.V7.App.AlertDialog.Builder alert =
           new Android.Support.V7.App.AlertDialog.Builder(this);
            alert.SetTitle(CScore.FixdStrings.Messages.MessageStatus());
            alert.SetMessage(message);
            alert.SetNeutralButton(CScore.FixdStrings.Buttons.DONE(), (senderAlert, args) => {
                if (status)
                    this.Finish();
            });

            Dialog x = alert.Create();
            x.SetCancelable(false);
            x.Show();
        }

        private bool isExited(String id)
        {
            foreach (var u in courses)
            {
                if (u.Cou_id == id)
                    return true;
            }
            return false;
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

       


    }
}

