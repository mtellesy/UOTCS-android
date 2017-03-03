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

using CScore.BCL;
using static Android.Views.View;

namespace UOTCS_android
{
    [Activity(Label = "Send Announcement", Icon = "@drawable/icon", Theme = "@style/Theme.Student", ParentActivity = (typeof(Messages)))]
    public class SendAnnouncement: MainActivity
    {
        ImageButton sendButton;
        AutoCompleteTextView SendTo;
        EditText messageSubject;
        EditText messageContent;
        ArrayAdapter<String> userCourses;

        //the list of users in SendTo drop down list
        List<Course> courses;

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            Values.changeTheme(this);
            SetContentView(Resource.Layout.sendMessage);
            this.findViews();
            this.handleEvents();
        }



        private void findViews()
        {
            Android.Support.V7.Widget.Toolbar toolBar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolBar);



            sendButton = FindViewById<ImageButton>(Resource.Id.sendMessageButton);
            sendButton.Visibility = ViewStates.Visible;

            SendTo = FindViewById<AutoCompleteTextView>(Resource.Id.send_to_message_announcement_fragment);

            messageSubject = FindViewById<EditText>(Resource.Id.title_message_announcement_fragment);

            messageContent = FindViewById<EditText>(Resource.Id.content_message_announcement_fragment);


            SetSupportActionBar(toolBar);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            fillDropDownList();
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



        private void handleEvents()
        {
            sendButton.Click += SendButton_Click;
        }


        private async void SendButton_Click(object sender, EventArgs e)
        {
            StatusWithObject <CScore.BCL.Announcements> ReAnnouncement = new StatusWithObject<CScore.BCL.Announcements>();
            CScore.BCL.Announcements newAnnouncement = new CScore.BCL.Announcements();
            newAnnouncement.Ano_content = messageContent.Text;
            newAnnouncement.Ter_id = CScore.BCL.Semester.current_term; //messageSubject.Text;

         
            if (SendTo.Text != null || SendTo.Text != "" || SendTo.Text == " ")
            {
               
                if (isExited(SendTo.Text))
                {
                    newAnnouncement.Cou_id = SendTo.Text;
                    ReAnnouncement = await CScore.BCL.Announcements.sendAnnouncement(newAnnouncement);
                    if (ReAnnouncement.status.status)
                        showMessage(CScore.FixdStrings.Announcements.AnnouncementHasSuccessfullySent());
                    else
                        showMessage(CScore.FixdStrings.Announcements.AnnouncementSendFaild());
                }
                else
                {
                    showMessage(CScore.FixdStrings.Courses.CourseDoesNotExist());
                }

            }
            else
            {
                showMessage(CScore.FixdStrings.Courses.PleaseTypeCourseCode());
            }

        }

        private void SendTo_Touch(object sender, TouchEventArgs e)
        {
            SendTo.ShowDropDown();
        }

        private void showMessage(String message)
        {
            Android.Support.V7.App.AlertDialog.Builder alert =
           new Android.Support.V7.App.AlertDialog.Builder(this);
            alert.SetTitle(CScore.FixdStrings.Messages.MessageStatus());
            alert.SetMessage(message);
            alert.SetNeutralButton(CScore.FixdStrings.Buttons.DONE(), (senderAlert, args) => {

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

