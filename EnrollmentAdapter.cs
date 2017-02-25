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
           //declaration of the views
            var view = convertView ?? _activity.LayoutInflater.Inflate(
                Resource.Layout.EnrollmentItemView, parent, false);
            var CourseCode = view.FindViewById<TextView>(Resource.Id.enrollCourseText);
            var GroupSpinner = view.FindViewById<Spinner>(Resource.Id.enrollGroupsList);
            var EnrollButton = view.FindViewById<Button>(Resource.Id.enrollButton);
            CourseCode.Text = CoursesItemsList[position].CourseID;
            var adapter = new ArrayAdapter<String>(_activity, Android.Resource.Layout.SimpleSpinnerItem, CoursesItemsList[position].Groups);
            GroupSpinner.Adapter = adapter;
            GroupSpinner.SetSelection(0);
            //end of it

            // current course Status
            ActiveButtons CourseStatus = activeButtons.Where(i => i.courseCode.Equals(CoursesItemsList[position].CourseID)).First();



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

            // case Enrollment
            if (!dropOnly)
            {
                EnrollButton.Click += (sender, e) =>
                {
                   

                    //get the current credit to display it to the user
                    Enrollment.total_credit.Text = CScore.BCL.Enrollment.getCreditSum().ToString();

                    //fetch the current course 
                    Course c = _courses.Where(i => i.Cou_id.Equals(CoursesItemsList[position].CourseID)).First();
                    //fetch it's group ID the eques the selected group in the (group spinner GUI)
                    List<CScore.BCL.Schedule> s = c.Schedule.Where(i => i.Gro_NameEN.Equals(GroupSpinner.SelectedItem.ToString())).ToList();
                    c.TemGro_id = s.First().Gro_id;

                    //first see the course current status (active or not active)
                    //** active means selected and added to the list of enrolled courses
                    //case active means unactivate it (Delete it), else
                    //case not active means actiave it (add it)

                    if (CourseStatus.status)//case active and want to drop
                    {
                        //see if the course was already active in the server

                        if (startedEnrolled)// case it was already enrolled in the server
                        {
                            //check if it was the same group
                            if (c.TemGro_id == CourseStatus.groupID)//case the same group
                            {
                                //see if it is in the drop list durring this process or not.
                                if (inDropList)// case it is in the drop list
                                {
                                    //the course was already enrolled (same group) and it has been added to 
                                    // the drop list
                                    // since we want to drop it any way we should keep it the 
                                    // the way it is 
                                    // we don't need this setion its just for testing
                                    // give feed back and change the style of the buton

                                    // case 0
                                    // give feed back and change the style of the buton
                                    this.showMessage(CScore.FixdStrings.Enrollment.disenrollmentSucceededMessage());
                                    Enrollment.total_credit.Text = CScore.BCL.Enrollment.getCreditSum().ToString();
                                    //change it later 
                                    EnrollButton.SetTextColor(Android.Graphics.Color.Black);
                                    GroupSpinner.Enabled = true;
                                    CourseStatus.status = false;

                                }
                                else// case it isn't in drop list
                                {
                                    //the course is already in server and we want to drop it
                                    // we need just to add it to drop list
                                    CScore.BCL.Enrollment.addToDropList(c);
                                    // there is no need to remove it from Course list 
                                    // becasue it wouding be there 
                                    // change the flag
                                    inDropList = true;

                                    // case 1
                                    // give feed back and change the style of the buton
                                    this.showMessage(CScore.FixdStrings.Enrollment.disenrollmentSucceededMessage());
                                    Enrollment.total_credit.Text = CScore.BCL.Enrollment.getCreditSum().ToString();
                                    //change it later 
                                    EnrollButton.SetTextColor(Android.Graphics.Color.Black);
                                    GroupSpinner.Enabled = true;
                                    CourseStatus.status = false;
                                }

                            }
                            else//case not the same group
                            {
                                //if you want to delete an already enrolled 
                                //course but it's hasn't the same group
                                // it's wrong to ask if it was in dropList 
                                //because indropList conected to course with same group id
                                // the best way is to look for in droplist and find ourselvs
                                // first declare newInDropList
                                // to know if the course with this id is in droplist or not
                                bool newInDropList = false;
                                var count = CScore.BCL.Enrollment.dropedCourses.Where(i => i.Cou_id.Equals(c.Cou_id)).ToList()
                                 .Where(i => i.TemGro_id.Equals(c.TemGro_id));
                               // if (count > 0) newInDropList = true;

                                if (newInDropList)//case it was in the drop list
                                {
                                    //case it was in the drop list 
                                    //remove it from both 
                                    //droplist and enrolledlist
                                    //just remove the course from drop list
                                    //without removing its reserved time 
                                    //that job is for removeFromEnrollList
                                    CScore.BCL.Enrollment.removeFromDropList_Enrolled(c);
                                    //remove it from enrolled list
                                    CScore.BCL.Enrollment.removeFromCourseList(c);
                                    //update the flag 
                                    inDropList = false;

                                    // case 2
                                    // give feed back and change the style of the buton
                                    this.showMessage(CScore.FixdStrings.Enrollment.disenrollmentSucceededMessage());
                                    Enrollment.total_credit.Text = CScore.BCL.Enrollment.getCreditSum().ToString();
                                    //change it later 
                                    EnrollButton.SetTextColor(Android.Graphics.Color.Black);
                                    GroupSpinner.Enabled = true;
                                    CourseStatus.status = false;
                                }
                                else//case it wasn't in the droplist
                                {
                                    //delete an already enrolled courses 
                                    //that hasn't the same group
                                    //and it isn't in drop list

                                    //just delete it from enrolled courses list 
                                    CScore.BCL.Enrollment.removeFromCourseList(c);

                                    // case 3
                                    // give feed back and change the style of the buton
                                    this.showMessage(CScore.FixdStrings.Enrollment.disenrollmentSucceededMessage());
                                    Enrollment.total_credit.Text = CScore.BCL.Enrollment.getCreditSum().ToString();
                                    //change it later 
                                    EnrollButton.SetTextColor(Android.Graphics.Color.Black);
                                    GroupSpinner.Enabled = true;
                                    CourseStatus.status = false;
                                }

                            }

                        }
                        else// case it wasn't already enrolled in the server
                        {
                            if (inDropList)// case it is in the drop list
                            {
                                //case we want to drop a course that wasn't already enrolled
                                //and it was in the drop list 

                                // since it was already in the drop list we have no job to do just keep it
                                // ######### IDEA ################
                                // idea we could add a new function the checks the enroll list 
                                // if it was contains this course and remove it from it

                                // case 2
                                // give feed back and change the style of the buton
                                this.showMessage(CScore.FixdStrings.Enrollment.disenrollmentSucceededMessage());
                                Enrollment.total_credit.Text = CScore.BCL.Enrollment.getCreditSum().ToString();
                                //change it later 
                                EnrollButton.SetTextColor(Android.Graphics.Color.Black);
                                GroupSpinner.Enabled = true;
                                CourseStatus.status = false;
                            }
                            else// case it isn't in drop list
                            {
                                // case we want to drop course that wasn't already enrolled 
                                //in the server and it wasn't already in drop list
                                // just remove it from the enrolled course list
                                // ############ IDEA ##################
                                // idea we could add a new function the checks the enroll list 
                                // if it was contains this course and remove it from it

                                CScore.BCL.Enrollment.removeFromCourseList(c);
                                // case 3
                                // give feed back and change the style of the buton
                                this.showMessage(CScore.FixdStrings.Enrollment.disenrollmentSucceededMessage());
                                Enrollment.total_credit.Text = CScore.BCL.Enrollment.getCreditSum().ToString();
                                //change it later 
                                EnrollButton.SetTextColor(Android.Graphics.Color.Black);
                                GroupSpinner.Enabled = true;
                                CourseStatus.status = false;
                            }
                        }
                    }
                    else //case not active and want to add
                    {
                  //      c.TemGro_id = s.First().Gro_id;
                        Status status = CScore.BCL.Enrollment.isEnrollable(c);
                        if (status.status)
                        {
                            if (startedEnrolled)// case it was already enrolled in the server
                            {
                              
                                //check if it was the same group
                                if (c.TemGro_id == CourseStatus.groupID)
                                {
                                    //see if it is in the drop list durring this process or not.
                                    if (inDropList)// case it is in the drop list
                                    {
                                        //the course was already enrolled (same group) and it has been added to 
                                        // the drop list
                                        // just delete it from the drop list becasue it already enrolled in
                                        // the server
                                        CScore.BCL.Enrollment.removeFromDropList_Enrolled(c);
                                        // and now add it's time to the reserved times without adding it 
                                        // to the list of enrolled courses
                                        CScore.BCL.Enrollment.addToCourseList_Enrolled(c);
                                        //finaly since we removed it from the drop list we must
                                        // change the flag to false
                                        inDropList = false;

                                        // case 4
                                        // give feed back and change the style of the buton
                                        this.showMessage(CScore.FixdStrings.Enrollment.enrollmentSucceededMessage());
                                        Enrollment.total_credit.Text = CScore.BCL.Enrollment.getCreditSum().ToString();
                                        //change it later 
                                        EnrollButton.SetTextColor(Android.Graphics.Color.Red);
                                        GroupSpinner.Enabled = false;
                                        CourseStatus.status = true;

                                    }
                                    else// case it isn't in drop list
                                    {
                                        //the course was already enrolled, but it was wasn't in the drop list
                                        //the could should not need this part because 
                                        //the moment user remove an already enrolled course it should be in course list
                                        // and droplist also
                                        // but this is just for test and error handling 

                                        // add the course to the reserved times without adding it 
                                        // to the list of enrolled courses
                                        CScore.BCL.Enrollment.addToCourseList_Enrolled(c);

                                        // case 5
                                        // give feed back and change the style of the buton
                                        this.showMessage(CScore.FixdStrings.Enrollment.enrollmentSucceededMessage());
                                        Enrollment.total_credit.Text = CScore.BCL.Enrollment.getCreditSum().ToString();
                                        //change it later 
                                        EnrollButton.SetTextColor(Android.Graphics.Color.Red);
                                        GroupSpinner.Enabled = false;
                                        CourseStatus.status = true;
                                    }

                                }
                                //case it wasn't the same group
                                else
                                {
                                    //course is already enrolled in server but it's not the same group 
                                    //all cases must end with us adding the course to enrolled List
                                    if (inDropList)
                                    {
                                        //case the course was in the drop list
                                        // and the course is existed but it's not the same group
                                        //so no need to remove it from the drop list because 
                                        // it's no not the same group
                                        // just add it 
                                        //to enroll list
                                        //remove it without adding the reserved time
                                       
                                        // and now add it's time to the reserved times and add it 
                                        // to the list of enrolled courses
                                        CScore.BCL.Enrollment.addToCourseList(c);
                                        
                                        //finaly since we removed it from the drop list we must
                                        // change the flag to false
                                    //    inDropList = false;

                                        // case 6
                                        // give feed back and change the style of the buton
                                        this.showMessage(CScore.FixdStrings.Enrollment.enrollmentSucceededMessage());
                                        Enrollment.total_credit.Text = CScore.BCL.Enrollment.getCreditSum().ToString();
                                        //change it later 
                                        EnrollButton.SetTextColor(Android.Graphics.Color.Red);
                                        GroupSpinner.Enabled = false;
                                        CourseStatus.status = true;

                                    }
                                    else
                                    {
                                        //case the course wasn't in the drop list
                                        // and the course is existed but it's not the same group
                                        //just add it to the courses List
                                        CScore.BCL.Enrollment.addToCourseList(c);
                                        // case 7
                                        // give feed back and change the style of the buton
                                        this.showMessage(CScore.FixdStrings.Enrollment.enrollmentSucceededMessage());
                                        Enrollment.total_credit.Text = CScore.BCL.Enrollment.getCreditSum().ToString();
                                        //change it later 
                                        EnrollButton.SetTextColor(Android.Graphics.Color.Red);
                                        GroupSpinner.Enabled = false;
                                        CourseStatus.status = true;
                                    }
                                }

                            }
                            else// case it wasn't already enrolled in the server
                            {
                                if (inDropList)// case it is in the drop list
                                {
                                    //in case the course wasn't already enrolled in the server and 
                                    //it was in the droping list and we want to add it to 
                                    // enrolled course
                                    // first remove it from the dropList
                                    // since we are gonna remove it from drop list
                                    // and then add it to enrolledCourses
                                    // we need to use removeFromDropList_Enrolled();
                                    //becasue it adds the reseved time and we will leave 
                                    //this job to addToEnrolledList();
                                    CScore.BCL.Enrollment.removeFromDropList_Enrolled(c);
                                    //add now to enrolledList
                                    CScore.BCL.Enrollment.addToCourseList(c);
                                    //it's no longer in the droping list
                                    inDropList = false;
                                    // case 8
                                    // give feed back and change the style of the buton
                                    this.showMessage(CScore.FixdStrings.Enrollment.enrollmentSucceededMessage());
                                    Enrollment.total_credit.Text = CScore.BCL.Enrollment.getCreditSum().ToString();
                                    //change it later 
                                    EnrollButton.SetTextColor(Android.Graphics.Color.Red);
                                    GroupSpinner.Enabled = false;
                                    CourseStatus.status = true;
                                }
                                else// case it isn't in drop list
                                {
                                    //this section conatins the most frequentnly used code
                                    //in case the course wasn't already enrolled in the server 
                                    //and it wasn't  in droplist
                                    CScore.BCL.Enrollment.addToCourseList(c);

                                    // case 9
                                    // give feed back and change the style of the buton
                                    this.showMessage(CScore.FixdStrings.Enrollment.enrollmentSucceededMessage());
                                    Enrollment.total_credit.Text = CScore.BCL.Enrollment.getCreditSum().ToString();
                                    //change it later 
                                    EnrollButton.SetTextColor(Android.Graphics.Color.Red);
                                    GroupSpinner.Enabled = false;
                                    CourseStatus.status = true;


                                }
                            }
                        }
                        else // case not enrollable
                        {
                            this.showMessage(status.message);
                        }
                    }
                   
                };
                }
                    // case Disenrollment
                    else
                    {

                    }

            //if (!dropOnly)
            //{
            //    //  event handler  
            //    EnrollButton.Click += (sender, e) =>
            //    {
            //        Enrollment.total_credit.Text = CScore.BCL.Enrollment.getCreditSum().ToString() ;
            //        Course c = _courses.Where(i => i.Cou_id.Equals(CoursesItemsList[position].CourseID)).First();

            //        List<CScore.BCL.Schedule> s = c.Schedule.Where(i => i.Gro_NameEN.Equals(GroupSpinner.SelectedItem.ToString())).ToList();
            //        c.TemGro_id = s.First().Gro_id;

            //        // if the course is not currently enrolled
            //        if (!CourseStatus.status)//status means it's an activated button 
            //        {
            //            // ask about the course if user can enroll or not 
            //            Status status = CScore.BCL.Enrollment.isEnrollable(c);

            //            // if okay
            //            if (status.status)
            //            {
            //                // if already enrolled in the server with 
            //                if(startedEnrolled && c.TemGro_id == CourseStatus.groupID)
            //                {
            //                    //JUST ADD THE RESERVED TIME
            //                    CScore.BCL.Enrollment.addToCourseList_Enrolled(c);

            //                }
            //                else
            //                    CScore.BCL.Enrollment.addToCourseList(c);

            //                if (inDropList && CourseStatus.groupID == c.TemGro_id)
            //                {
            //                    CScore.BCL.Enrollment.removeFromDropList_Enrolled(c);
            //                    inDropList = false;
            //                }
            //                this.showMessage("Good");
            //                int index = activeButtons.IndexOf(activeButtons.Where(i => i.courseCode.Equals(c.Cou_id)).First());
            //                activeButtons[index].status = true;

            //                // later change the style
            //                EnrollButton.SetTextColor(Android.Graphics.Color.Red);
            //                GroupSpinner.Enabled = false;
            //                Enrollment.total_credit.Text = CScore.BCL.Enrollment.getCreditSum().ToString();

            //            }
            //            else
            //            {
            //                this.showMessage(status.message);
                           
                           
            //            }


            //        }
            //        else
            //        {
            //            if(!inEnrollList && startedEnrolled)
            //            {
                           
            //                if (startedEnrolled && c.TemGro_id == CourseStatus.groupID)
            //                {
            //                    CScore.BCL.Enrollment.removeFromCourseList_Enrolled(c);
            //                    //CScore.BCL.Enrollment.addToDropList_Enrolled(c);
            //                    //inDropList = true;
            //                }
            //                else
            //                    CScore.BCL.Enrollment.removeFromCourseList(c);
                          
            //                if (inDropList && CourseStatus.groupID == c.TemGro_id)
            //                {
            //                    CScore.BCL.Enrollment.removeFromDropList_Enrolled(c);
            //                    inDropList = false;
            //                }

            //                Enrollment.total_credit.Text = CScore.BCL.Enrollment.getCreditSum().ToString();
            //            }
            //            else if(!inEnrollList && !startedEnrolled)
            //            { CScore.BCL.Enrollment.removeFromCourseList(c);
            //                if (inDropList && CourseStatus.groupID == c.TemGro_id)
            //                {
            //                    CScore.BCL.Enrollment.removeFromDropList_Enrolled(c);
            //                    inDropList = false;
            //                }

            //                Enrollment.total_credit.Text = CScore.BCL.Enrollment.getCreditSum().ToString();
            //            }
            //            else 
            //            {
            //                CScore.BCL.Enrollment.removeFromCourseList_Enrolled(c);
            //                if(!inDropList || (CourseStatus.groupID == c.TemGro_id && startedEnrolled))
            //                {
            //                    CScore.BCL.Enrollment.addToDropList_Enrolled(c);
            //                    inDropList = true;
            //                }
                          
            //                Enrollment.total_credit.Text = CScore.BCL.Enrollment.getCreditSum().ToString();

            //            }

                        
            //             inEnrollList = false;
            //            int index = activeButtons.IndexOf(activeButtons.Where(i => i.courseCode.Equals(c.Cou_id)).First());
            //            activeButtons[index].status = false;
            //            GroupSpinner.Enabled = true;
            //            EnrollButton.SetTextColor(Android.Graphics.Color.Black);
            //            Enrollment.total_credit.Text = CScore.BCL.Enrollment.getCreditSum().ToString();


            //        }

            //    };

            //}
            //else
            //{
            //    EnrollButton.Click += (sender, e) =>
            //    {
            //        Enrollment.total_credit.Text = CScore.BCL.Enrollment.getCreditSum().ToString();
            //        Course c = _courses.Where(i => i.Cou_id.Equals(CoursesItemsList[position].CourseID)).First();

            //        List<CScore.BCL.Schedule> s = c.Schedule.Where(i => i.Gro_NameEN.Equals(GroupSpinner.SelectedItem.ToString())).ToList();
            //        c.TemGro_id = s.First().Gro_id;

            //        if (CourseStatus.status)
            //        {

            //            CScore.BCL.Enrollment.addToDropList(c);
            //            ;
            //            this.showMessage("Good");
            //            int index = activeButtons.IndexOf(activeButtons.Where(i => i.courseCode.Equals(c.Cou_id)).First());
            //            activeButtons[index].status = false;
            //            Enrollment.total_credit.Text = CScore.BCL.Enrollment.getCreditSum().ToString();

            //            // later change the style
            //            EnrollButton.SetTextColor(Android.Graphics.Color.Black);
            //        }
            //        else
            //        {
            //            CScore.BCL.Enrollment.removeFromDropList(c);
            //            EnrollButton.SetTextColor(Android.Graphics.Color.Red);
            //            int index = activeButtons.IndexOf(activeButtons.Where(i => i.courseCode.Equals(c.Cou_id)).First());
            //            activeButtons[index].status = true;
            //            Enrollment.total_credit.Text = CScore.BCL.Enrollment.getCreditSum().ToString();

            //            this.showMessage("");
            //        }
            //    };
            //}
 

           
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