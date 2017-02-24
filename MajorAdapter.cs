using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CScore.BCL;
using Android.Provider;

namespace UOTCS_android
{
    class ActiveDepartments
    {
       public String courseCode { get; set; }
        public int groupID { get; set; }
       public bool status { get; set; }
    }
    public class MajorAdapter : BaseAdapter
    {
        List<CourseItem> CoursesItemsList;
        List<CScore.BCL.Course> _courses;
       static bool dropOnly { get; set; }

        /// <summary>
        /// Is used to keep information about wither the button is active or not
        /// </summary>
        List<ActiveDepartments> activeButtons;// { get; set; }
        Activity _activity;
     //   int current_posstion = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="courses"></param>
        /// <param name="dropOnly">If the process is a disEnrollment process set this to true</param>
        public MajorAdapter(Activity activity,List<CScore.BCL.Course> courses,bool newDropOnly)
        {
            activeButtons =  new List<ActiveDepartments>();
            dropOnly = newDropOnly;   
            var task1 = Task.Run(async () => { await CScore.BCL.Course.getUserCoursesSchedule(); });
            task1.Wait();
            _courses = new List<Course>();
            _courses = courses;
            _activity = activity;
            FillContacts(courses);

            var task = Task.Run(async () => { await getExistedCourses(); });
            task.Wait();
            Enrollment.total_credit.Text = CScore.BCL.Enrollment.getCreditSum().ToString();


        }

        public async Task getExistedCourses()
        {
           
            await CScore.BCL.Enrollment.StartEnrollmentAndGetCurrentCreditSum();

            StatusWithObject<List<CScore.BCL.Course>> courses = 
                await CScore.BCL.Course.getStudentCourses();
            var coursesWithSchedule = await CScore.BCL.Course.getUserCoursesSchedule();

            if (courses.status.status)
            {
                foreach(CScore.BCL.Course c in courses.statusObject)
                {
                    int count = activeButtons.Where(i => i.courseCode.Equals(c.Cou_id)).Count();
                    if(count > 0)
                    {
                        int index = activeButtons.IndexOf(activeButtons.Where(i => i.courseCode.Equals(c.Cou_id)).First());
                        activeButtons[index].status = true;
                       
                        activeButtons[index].groupID = c.TemGro_id;
                        foreach(var sch in coursesWithSchedule.statusObject)
                        {
                            if (c.Cou_id == sch.Cou_id)
                                activeButtons[index].groupID = sch.Schedule[0].Gro_id;
                        }
                       
                        
                    }
                }
            }

        }

         void FillContacts(List<CScore.BCL.Course> courses)
        {

           // int id = 0;
            CoursesItemsList = new List<CourseItem>();
            foreach(CScore.BCL.Course course in courses)
            {
                CourseItem x = new CourseItem();
                x.CourseID = course.Cou_id;
                x.Groups = new List<String>();

                var disCourseSchedule = course.Schedule.GroupBy(test => test.Gro_id)
                   .Select(grp => grp.First())
                   .ToList();
             //   int i = 0;
                foreach(CScore.BCL.Schedule sch in disCourseSchedule)
                {
                    x.Groups.Add(sch.Gro_NameEN);
                }
                x.Id++;
                CoursesItemsList.Add(x);

                // add item to activeButtons List
                ActiveButtons newButton = new ActiveButtons();
                newButton.courseCode = course.Cou_id;
                newButton.status = false;
                activeButtons.Add(newButton);
            }


        }

        class CourseItem
        {
            public long Id { get; set; }
            public String CourseID { get; set; }
            public List<string> Groups { get; set; }
            
           
        }

        public override int Count
        {
            get { return CoursesItemsList.Count; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            // could wrap a Contact in a Java.Lang.Object
            // to return it here if needed
            return null;
        }

        public override long GetItemId(int position)
        {
            return CoursesItemsList[position].Id;
        }

        /// <summary>
        /// Return the layout view and its content
        /// </summary>
        /// <param name="position">the current position of the list</param>
        /// <param name="convertView"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
           //declaration of the views
            var view = convertView ?? _activity.LayoutInflater.Inflate(
                Resource.Layout.MajorItemView, parent, false);
            var CourseCode = view.FindViewById<TextView>(Resource.Id.enrollCourseText);
            var GroupSpinner = view.FindViewById<Spinner>(Resource.Id.enrollGroupsList);
            var EnrollButton = view.FindViewById<Button>(Resource.Id.enrollButton);
            CourseCode.Text = CoursesItemsList[position].CourseID;
            var adapter = new ArrayAdapter<String>(_activity, Android.Resource.Layout.SimpleSpinnerItem, CoursesItemsList[position].Groups);
            GroupSpinner.Adapter = adapter;
            GroupSpinner.SetSelection(0);
            //end of it

            // current course Status
         

            // because the course objects in _courses are references and 
            // playing with it is going to change the whole structure 
            // we need to clone _courses
           
            var OriCourses =new Course() ;
            OriCourses = _courses[position].getACopy();
            
           

            // to know  it's in the droping list 
            bool inDropList = false;
            // is used to know if the course was already enrolled before the start of the process
            bool inEnrollList = false;

            // to know that the course and its group are already enrolled
            // and if the user remove it and then add it again with same group
            // there will be no need to put in enrollment list
            bool startedEnrolled = false;
            if (CourseStatus.status)
            {
                //later change the style
                EnrollButton.SetTextColor(Android.Graphics.Color.Red);
           
                GroupSpinner.Enabled = false;
                GroupSpinner.SetSelection(CourseStatus.groupID-1);
                inEnrollList = true;
                startedEnrolled = true;
              
            }
            

            return view;
        }

        private CScore.BCL.Course getMyCourse(int position)
        {
           Course c = _courses.Where(i => i.Cou_id.Equals(CoursesItemsList[position].CourseID)).First();
            return c;
        }
       
        private void showMessage(String message)
        {
            Android.Support.V7.App.AlertDialog.Builder alert =
           new Android.Support.V7.App.AlertDialog.Builder(_activity);
            alert.SetTitle("Login Status");
            alert.SetMessage(message);
            //alert.SetPositiveButton("OK", (senderAlert, args) => {
            //    Toast.MakeText(this, "", ToastLength.Short).Show();
            //});

            Dialog x = alert.Create();
            x.Show();
        }
    }
}