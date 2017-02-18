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
using CScore.FixdStrings;

namespace UOTCS_android
{
    [Activity(Label = "Send Message", Icon = "@drawable/icon", Theme = "@style/Theme.Student", ParentActivity = (typeof(Messages)))]
         
    public class SendMessage : AppCompatActivity
    {
        ImageButton sendButton;
        AutoCompleteTextView SendTo;
        EditText messageSubject;
        EditText messageContent;
        ArrayAdapter<String> usersNames;
        private SupportToolbar toolBar;
        private SupportActionBar actionbar;
        private DrawerLayout drawerLayout;
        private NavigationView navigationView;
        private View view;
        private CircleImageView profileImage;
        private int lecturer_id;
        private OtherUsers lecturer;
        Language e;
        //the list of users in SendTo drop down list
        List<OtherUsers> users;


        protected override  void OnCreate(Bundle savedInstanceState)
        {
            Bundle b = Intent.Extras;

            Intent intent2 = Intent;
            if (null != b)
            { //Null Checking
              //lecturer_id = intent2.GetLongExtra("lecturer_id",-1);

                lecturer_id = b.GetInt("lecturer_id");
            }
            if (lecturer_id != 0)
            {
                bindLecturerData((int)lecturer_id);
            }
            base.OnCreate(savedInstanceState);
            this.Title = CScore.FixdStrings.Messages.SendMessageLable();
            Values.changeTheme(this);
            SetContentView(Resource.Layout.sendMessage);
            this.findViews();
            SetSupportActionBar(toolBar);
            setUpActionBar(actionbar);
            setUpNavigationView(navigationView);
            this.handleEvents();

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
           if (lecturer != null)
            {
                e = LanguageSetter.getLanguage();

                switch (e)
                {
                    case (Language.AR):
                        SendTo.Text = lecturer.use_nameAR;
                        break;

                    case (Language.EN):
                    default:
                        SendTo.Text = lecturer.use_nameEN;
                        break;
                }
                SendTo.DismissDropDown();
            }
           
            messageSubject = FindViewById<EditText>(Resource.Id.title_message_announcement_fragment);

            messageContent = FindViewById<EditText>(Resource.Id.content_message_announcement_fragment);

            
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
            navigationView.SetCheckedItem(Resource.Id.nav_messages);
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
            return Resource.Id.nav_messages;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            base.OnOptionsItemSelected(item);
            if (item.ItemId == Android.Resource.Id.Home)
            {
                NavUtils.NavigateUpFromSameTask(this);
                return true;
            }

            return base.OnOptionsItemSelected(item);

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
                Values.handleSwitchActivities(this, e.MenuItem.ItemId, navigationView);
            }

        }
        private void ProfileImage_Click(object sender, EventArgs e)
        {
            drawerLayout.CloseDrawers();
            Values.startProfile(this);
            Finish();
        }

        private async void fillDropDownList()
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
                var task = Task.Run(async () =>
                {
                    Students = await CScore.BCL.OtherUsers.getLecturerStudents();
                });
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
      
       

        private async void SendButton_Click(object sender, EventArgs e)
        {
            StatusWithObject<CScore.BCL.Messages> reMessage = new StatusWithObject<CScore.BCL.Messages>();
            CScore.BCL.Messages message = new CScore.BCL.Messages();
            message.Mes_content = messageContent.Text;
            message.Mes_subject = messageSubject.Text;

            int userid = 0;
            string name = SendTo.Text;
            if (name != "" || name != "''" || name != null)
            {
                userid = getUserIDByName(SendTo.Text);
                if(userid!=0)
                {
                    message.Mes_reciever = userid;
                    bool status = await CScore.BCL.Messages.sendMessage(message);
                    if(status)
                        showMessage(CScore.FixdStrings.Messages.MessageHasSuccessfullySent(),true);
                    else
                        showMessage(CScore.FixdStrings.Messages.MessageSendFaild(),false);
                }
                else
                {
                    showMessage(CScore.FixdStrings.Users.UserDoesNotExist(), false);
                }

            }
            else
            {
                showMessage(CScore.FixdStrings.Users.PleaseTypeUsername(), false);
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

        private void bindLecturerData(int lecturer_id)
        {
            StatusWithObject<OtherUsers> values = new StatusWithObject<OtherUsers>();
            try
            {
                var task = Task.Run(async () => { values = await this.getLecturer(lecturer_id); });
                task.Wait();
            }
            catch (AggregateException ex)
            {

            }
            lecturer = values.statusObject;
        }
        private async Task<CScore.BCL.StatusWithObject<OtherUsers>> getLecturer(int lecturer_id)
        {
            CScore.BCL.StatusWithObject<OtherUsers> result = new StatusWithObject<OtherUsers>();
            try
            {
                result = await CScore.BCL.OtherUsers.getOtherUser(lecturer_id);
                var x = result;
            }
            catch (Exception ex)
            {

            }
            return result;
        }





    }
}

