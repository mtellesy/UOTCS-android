using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using SupportFragment = Android.Support.V4.App.Fragment;
using CScore.BCL;
using System.Threading.Tasks;
using System.Threading;
using static Android.Views.View;

namespace UOTCS_android.Fragments
{
    public class SendMessageAnnouncementFragment : SupportFragment
    {
        public int i = 0;

        public View view;
        public  AutoCompleteTextView SendTo;
        ArrayAdapter<String> adapter;
        private string type;
        public SendMessageAnnouncementFragment(string type)
        {
            this.type = type;
        }
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
           view = inflater.Inflate(Resource.Layout.SendMessageAnnouncementF, container, false);
            SendTo = view.FindViewById<AutoCompleteTextView>(Resource.Id.send_to_message_announcement_fragment);

            setHintContent(view);
             setTextViewMessages(Android.App.Application.Context);
                

            return view;

            //   task.Wait();

        }

        private  void setHintContent(View view)
        {
            EditText content = view.FindViewById<EditText>(Resource.Id.content_message_announcement_fragment);
            if (type == "Announcement")
            {
                content.SetHint(Resources.GetIdentifier("compose_announcement_hint","string", Activity.PackageName)); 
            }else if (type == "Message")
            {
                content.SetHint(Resources.GetIdentifier("compose_message_hint","string",Activity.PackageName));
               
            }
        }

   

        public void setTextViewMessages(Context context)
        {

            //case the User was a Student
            if (CScore.BCL.User.use_type == "S")
            {
                List<String> lecturersNames = new List<string>();
                StatusWithObject<List<Course>> Courses = new StatusWithObject<List<Course>>();
                var task = Task.Run(async () => {Courses = await CScore.BCL.Course.getUserCoursesSchedule(); });
                task.Wait();
                if (Courses.status.status && Courses.statusObject != null)
                    foreach (var cou in Courses.statusObject)
                    {

                       // lecturersNames.Add(cou.Schedule[0].Tea_id.ToString());
                        lecturersNames.Add("ShityLife");

                        
                    }

                var myAdapter = new ArrayAdapter<String>(context, Android.Resource.Layout.SimpleDropDownItem1Line, lecturersNames);

                
                SendTo.Adapter = myAdapter;
                //SendTo.ShowDropDown();
                myAdapter.NotifyDataSetChanged();
                i = 1;

                 }
    //case he was a Lecturer
    //else if (CScore.BCL.User.use_type == "L")
    //{
    //    List<String> studentNames = new List<string>();
    //    StatusWithObject<List<OtherUsers>> Students = await CScore.BCL.OtherUsers.getLecturerStudents();
    //    if (Students.status.status && Students.statusObject != null)
    //        foreach (var stu in Students.statusObject)
    //        {
    //            studentNames.Add(stu.use_nameEN);
    //        }
    //    adapter = new ArrayAdapter<String>(Android.App.Application.Context, Android.Resource.Layout.SimpleDropDownItem1Line, studentNames);
    //    SendTo.Adapter = adapter;

    //}

}

        public void test()
        {
            SendTo.ShowDropDown(); 
        }
    }
}