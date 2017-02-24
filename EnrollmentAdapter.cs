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
    class ActiveButtons
    {
       public String courseCode { get; set; }
        public int groupID { get; set; }
       public bool status { get; set; }
    }
    public class EnrollmentAdapter : BaseAdapter
    {
        List<CourseItem> CoursesItemsList;
        List<CScore.BCL.Course> _courses;
       static bool dropOnly { get; set; }

        /// <summary>
        /// Is used to keep information about wither the button is active or not
        /// </summary>
        List<ActiveButtons> activeButtons;// { get; set; }
        Activity _activity;
     //   int current_posstion = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="courses"></param>
        /// <param name="dropOnly">If the process is a disEnrollment process set this to true</param>
        public EnrollmentAdapter(Activity activity,List<CScore.BCL.Course> courses,bool newDropOnly)
        {
            activeButtons =  new List<ActiveButtons>();
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
           
            var view = convertView ?? _activity.LayoutInflater.Inflate(
                Resource.Layout.EnrollmentItemView, parent, false);
            var CourseCode = view.FindViewById<TextView>(Resource.Id.enrollCourseText);
            var GroupSpinner = view.FindViewById<Spinner>(Resource.Id.enrollGroupsList);
            var EnrollButton = view.FindViewById<Button>(Resource.Id.enrollButton);
            CourseCode.Text = CoursesItemsList[position].CourseID;
            

            var adapter = new ArrayAdapter<String>(_activity, Android.Resource.Layout.SimpleSpinnerItem, CoursesItemsList[position].Groups);
            GroupSpinner.Adapter = adapter;
            GroupSpinner.SetSelection(0);
           ActiveButtons buttonStatus = activeButtons.Where(i => i.courseCode.Equals(CoursesItemsList[position].CourseID)).First();
            var ButtonColor = EnrollButton.CurrentTextColor;

            // to it's in the droping list 
            bool inDropList = false;
            // is used to know if the course was already enrolled before the start of the process
            bool alreadyEnrolled = false;
            int alreadyEnrolledGroupID;
            bool startedEnrolled = false;
            if (buttonStatus.status)
            {
                //later change the style
               
                EnrollButton.SetTextColor(Android.Graphics.Color.Red);
               // GroupSpinner.get
                GroupSpinner.Enabled = false;
                GroupSpinner.SetSelection(buttonStatus.groupID-1);
                alreadyEnrolled = true;
                startedEnrolled = true;
                alreadyEnrolledGroupID = 0;
            }

            if (!dropOnly)
            {
                //  event handler  
                EnrollButton.Click += (sender, e) =>
                {
                    Enrollment.total_credit.Text = CScore.BCL.Enrollment.getCreditSum().ToString() ;
                    Course c = _courses.Where(i => i.Cou_id.Equals(CoursesItemsList[position].CourseID)).First();

                    List<CScore.BCL.Schedule> s = c.Schedule.Where(i => i.Gro_NameEN.Equals(GroupSpinner.SelectedItem.ToString())).ToList();
                    c.TemGro_id = s.First().Gro_id;

                    if (!buttonStatus.status)
                    {

                        Status status = CScore.BCL.Enrollment.isEnrollable(c);

          
                        if (status.status)
                        {

                            CScore.BCL.Enrollment.addToCourseList(c);
                            if (inDropList && buttonStatus.groupID == c.TemGro_id)
                            {
                                CScore.BCL.Enrollment.removeFromDropList_Enrolled(c);
                                inDropList = false;
                            }
                            this.showMessage("Good");
                            int index = activeButtons.IndexOf(activeButtons.Where(i => i.courseCode.Equals(c.Cou_id)).First());
                            activeButtons[index].status = true;

                            // later change the style
                            EnrollButton.SetTextColor(Android.Graphics.Color.Red);
                            GroupSpinner.Enabled = false;
                            Enrollment.total_credit.Text = CScore.BCL.Enrollment.getCreditSum().ToString();

                        }
                        else
                        {
                            this.showMessage(status.message);
                           
                           
                        }


                    }
                    else
                    {
                        if(!alreadyEnrolled)
                        {
                           
                            CScore.BCL.Enrollment.removeFromCourseList(c);
                            if(inDropList && buttonStatus.groupID == c.TemGro_id)
                            {
                                CScore.BCL.Enrollment.removeFromDropList_Enrolled(c);
                                inDropList = false;
                            }

                            Enrollment.total_credit.Text = CScore.BCL.Enrollment.getCreditSum().ToString();
                        }
                        else
                        {
                            CScore.BCL.Enrollment.removeFromCourseList_Enrolled(c);
                            if(!inDropList || (buttonStatus.groupID == c.TemGro_id && startedEnrolled))
                            {
                                CScore.BCL.Enrollment.addToDropList_Enrolled(c);
                                inDropList = true;
                            }
                          
                            Enrollment.total_credit.Text = CScore.BCL.Enrollment.getCreditSum().ToString();

                        }

                        
                         alreadyEnrolled = false;
                        int index = activeButtons.IndexOf(activeButtons.Where(i => i.courseCode.Equals(c.Cou_id)).First());
                        activeButtons[index].status = false;
                        GroupSpinner.Enabled = true;
                        EnrollButton.SetTextColor(Android.Graphics.Color.Black);
                        Enrollment.total_credit.Text = CScore.BCL.Enrollment.getCreditSum().ToString();


                    }

                };

            }
            else
            {
                EnrollButton.Click += (sender, e) =>
                {
                    Enrollment.total_credit.Text = CScore.BCL.Enrollment.getCreditSum().ToString();
                    Course c = _courses.Where(i => i.Cou_id.Equals(CoursesItemsList[position].CourseID)).First();

                    List<CScore.BCL.Schedule> s = c.Schedule.Where(i => i.Gro_NameEN.Equals(GroupSpinner.SelectedItem.ToString())).ToList();
                    c.TemGro_id = s.First().Gro_id;

                    if (buttonStatus.status)
                    {

                        CScore.BCL.Enrollment.addToDropList(c);
                        ;
                        this.showMessage("Good");
                        int index = activeButtons.IndexOf(activeButtons.Where(i => i.courseCode.Equals(c.Cou_id)).First());
                        activeButtons[index].status = false;
                        Enrollment.total_credit.Text = CScore.BCL.Enrollment.getCreditSum().ToString();

                        // later change the style
                        EnrollButton.SetTextColor(Android.Graphics.Color.Black);
                    }
                    else
                    {
                        CScore.BCL.Enrollment.removeFromDropList(c);
                        EnrollButton.SetTextColor(Android.Graphics.Color.Red);
                        int index = activeButtons.IndexOf(activeButtons.Where(i => i.courseCode.Equals(c.Cou_id)).First());
                        activeButtons[index].status = true;
                        Enrollment.total_credit.Text = CScore.BCL.Enrollment.getCreditSum().ToString();

                        this.showMessage("");
                    }
                };
            }
 

           
            return view;
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