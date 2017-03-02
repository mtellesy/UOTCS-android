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
using System.Threading.Tasks;
using System.Threading;
using Android.Support.V7.App;

using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using SupportActionBar = Android.Support.V7.App.ActionBar;

using Android.Support.V4.Widget;
using SupportFragment = Android.Support.V4.App.Fragment;
using SupportFragmentManager = Android.Support.V4.App.FragmentManager;

using Android.Support.V4.View;

using CScore.BCL;
using static Android.Views.View;

namespace UOTCS_android
{
    [Activity(Label = "Send Message", Icon = "@drawable/icon", Theme = "@style/Theme.Student", ParentActivity = (typeof(Messages)))]
         
    public class SendMessage : MainActivity
    {
        ImageButton sendButton;
        AutoCompleteTextView SendTo;
        EditText messageSubject;
        EditText messageContent;
        ArrayAdapter<String> usersNames;

        //the list of users in SendTo drop down list
        List<OtherUsers> users;


        protected override  void OnCreate(Bundle savedInstanceState)
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
            users = new List<OtherUsers>();
            if (CScore.BCL.User.use_type == "S")
            {
                List<String> lecturersNames = new List<string>();
                
                StatusWithObject<List<Course>> Courses = new StatusWithObject<List<Course>>();
                var task = Task.Run(async () => { Courses = await CScore.BCL.Course.getUserCoursesSchedule(); });
                task.Wait();
                if (Courses.status.status && Courses.statusObject != null)
                    foreach (var cou in Courses.statusObject)
                    {
                        if (cou.Schedule != null)
                        {
                            OtherUsers lecturer = new OtherUsers();
                            var task1 =
                            Task.Run(async () => { StatusWithObject<OtherUsers> x 
                                = await OtherUsers.getOtherUser(cou.Schedule[0].Tea_id);
                                lecturer = x.statusObject;
                            });
                            task1.Wait();
                            //make sure that the lecturer does not exist in the list
                            if(lecturer != null && !isExited(lecturer.use_id))
                            {
                                users.Add(lecturer);
                               lecturersNames.Add(lecturer.use_nameEN);
                            }
                           
                        }
                          
                    }

                usersNames = new ArrayAdapter<String>(Android.App.Application.Context, Resource.Layout.dropDownList_style, lecturersNames);


                SendTo.Adapter = usersNames;
                SendTo.Touch += SendTo_Touch;

                usersNames.NotifyDataSetChanged();


            }
            // case he was a Lecturer
            else if (CScore.BCL.User.use_type == "L")
            {
                List<String> studentNames = new List<string>();
                StatusWithObject<List<OtherUsers>> Students = new StatusWithObject<List<OtherUsers>>();
                var task = Task.Run(async () => { Students = await CScore.BCL.OtherUsers.getLecturerStudents(); });
                task.Wait();
                if (Students.status.status && Students.statusObject != null)
                    foreach (var stu in Students.statusObject)
                    {
                        if (stu != null && !isExited(stu.use_id))
                        {
                            studentNames.Add(stu.use_nameEN);
                            users.Add(stu);
                        }
                    }
                usersNames = new ArrayAdapter<String>(this, Resource.Layout.dropDownList_style, studentNames);
                SendTo.Adapter = usersNames;
                SendTo.Touch += SendTo_Touch;
                usersNames.NotifyDataSetChanged();

            }
        }
      
        private void handleEvents()
        {
            sendButton.Click += SendButton_Click;
        }

        private async void SendButton_Click(object sender, EventArgs e)
        {
            StatusWithObject<CScore.BCL.Messages> reMessage = new StatusWithObject<CScore.BCL.Messages>();
            CScore.BCL.Messages message = new CScore.BCL.Messages();
            message.Mes_content = messageContent.Text;
            message.Mes_subject = messageSubject.Text;

            int userid = 0;
            if(SendTo.Text != null || SendTo.Text != "" || SendTo.Text == " ")
            {
             userid = getUserIDByName(SendTo.Text);
                if(userid!=0)
                {
                    message.Mes_reciever = userid;
                    bool status = await CScore.BCL.Messages.sendMessage(message);
                    if(status)
                        showMessage(CScore.FixdStrings.Messages.MessageHasSuccessfullySent());
                    else
                        showMessage(CScore.FixdStrings.Messages.MessageSendFaild());
                }
                else
                {
                    showMessage(CScore.FixdStrings.Users.UserDoesNotExist());
                }

            }
            else
            {
                showMessage(CScore.FixdStrings.Users.PleaseTypeUsername());
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

        private bool isExited(int id)
        {
            foreach(var u in users)
            {
                if (u.use_id == id)
                    return true;
            }
            return false;
        }

        private int getUserIDByName(String name)
        {
            foreach (var u in users)
            {
                if (u.use_nameEN == name)
                    return u.use_id;
            }
            return 0;
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
            return Resource.Id.nav_messages;
        }


     

    }
}

