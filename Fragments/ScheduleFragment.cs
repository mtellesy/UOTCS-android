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
using CScore.BCL;

namespace UOTCS_android.Fragments
{
    public class ScheduleFragment : Android.Support.V4.App.Fragment
    {

        TextView SA1;
        TextView SA2;
        TextView SA3;
        TextView SA4;
        TextView SA5;
        TextView SA6;

        TextView SU1;
        TextView SU2;
        TextView SU3;
        TextView SU4;
        TextView SU5;
        TextView SU6;

        TextView MO1;
        TextView MO2;
        TextView MO3;
        TextView MO4;
        TextView MO5;
        TextView MO6;

        TextView TU1;
        TextView TU2;
        TextView TU3;
        TextView TU4;
        TextView TU5;
        TextView TU6;

        TextView WE1;
        TextView WE2;
        TextView WE3;
        TextView WE4;
        TextView WE5;
        TextView WE6;

        TextView TH1;
        TextView TH2;
        TextView TH3;
        TextView TH4;
        TextView TH5;
        TextView TH6;

        TextView DH;
        TextView SATDAY;
        TextView SUNDAY;
        TextView MONDAY;
        TextView TUEDAY;
        TextView WEDDAY;
        TextView THUDAY;





        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //set all the views in the table

            // Create your fragment here
        }

        

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
           
            View view = inflater.Inflate(Resource.Layout.ScheduleFragment, container, false);
            view =  this.getViews(view);
            return view;

           // return base.OnCreateView(inflater, container, savedInstanceState);
        }


        public void setCourseDayAndTime(CScore.BCL.Course courseWithSchedule)
        {
            String courseID = courseWithSchedule.Cou_id;
            foreach(CScore.BCL.Schedule sch in courseWithSchedule.Schedule)
            {
                int dayID = sch.DayID;
                int timeID = sch.ClassTimeID;
                courseID += String.Format("\n({0})\n", sch.Gro_NameEN);
                switch (dayID)
                {
 // ---------------------------> saturday <--------------------------------
                    case 1: // saturday
                        switch (timeID)
                        {
                            case 1:

                                SA1.Text += courseID;

                                break;

                            case 2:

                                SA2.Text += courseID;

                                break;

                            case 3:

                                SA3.Text += courseID;

                                break;

                            case 4:

                                SA4.Text += courseID;

                                break;

                            case 5:

                                SA5.Text += courseID;

                                break;

                            case 6:

                                SA6.Text += courseID;

                                break;

                            default:
                                break;
                        }
                        break;
   // ---------------------------> sunday <--------------------------------
                    case 2: // sunday
                        switch (timeID)
                        {
                            case 1:

                                SU1.Text += courseID;

                                break;

                            case 2:

                                SU2.Text += courseID;

                                break;

                            case 3:

                                SU3.Text += courseID;

                                break;

                            case 4:
   
                                SU4.Text += courseID;

                                break;

                            case 5:

                                SU5.Text += courseID;

                                break;

                            case 6:

                                SU6.Text += courseID;

                                break;

                            default:
                                break;
                        }
                        break;
 // ---------------------------> monday <--------------------------------
                    case 3: // monday
                        switch (timeID)
                        {
                            case 1:

                                MO1.Text += courseID;

                                break;

                            case 2:

                                MO2.Text += courseID;

                                break;

                            case 3:

                                MO3.Text += courseID;

                                break;

                            case 4:

                                MO4.Text += courseID;

                                break;

                            case 5:

                                MO5.Text += courseID;

                                break;

                            case 6:

                                MO6.Text += courseID;

                                break;

                            default:
                                break;
                        }
                        break;
// ---------------------------> tuesday <--------------------------------
                    case 4: // tuesday
                        switch (timeID)
                        {
                            case 1:

                                TU1.Text += courseID;

                                break;

                            case 2:

                                TU2.Text += courseID;

                                break;

                            case 3:

                                TU3.Text += courseID;

                                break;

                            case 4:

                                TU4.Text += courseID;

                                break;

                            case 5:

                                TU5.Text += courseID;

                                break;

                            case 6:

                                TU6.Text += courseID;

                                break;

                            default:
                                break;
                        }

                        break;
// ---------------------------> wednesday <--------------------------------
                    case 5: // wednesday
                        switch (timeID)
                        {
                            case 1:

                                WE1.Text += courseID;

                                break;

                            case 2:

                                WE2.Text += courseID;

                                break;

                            case 3:

                                WE3.Text += courseID;

                                break;

                            case 4:

                                WE4.Text += courseID;

                                break;

                            case 5:

                                WE5.Text += courseID;

                                break;

                            case 6:

                                WE6.Text += courseID;

                                break;

                            default:
                                break;
                        }

                        break;

                    // ---------------------------> thursday <--------------------------------
                    case 6: // thursday
                        switch (timeID)
                        {
                            case 1:

                                TH1.Text += courseID;

                                break;

                            case 2:

                                TH2.Text += courseID;

                                break;

                            case 3:

                                TH3.Text += courseID;

                                break;

                            case 4:

                                TH4.Text += courseID;

                                break;

                            case 5:

                                TH5.Text += courseID;

                                break;

                            case 6:

                                TH6.Text += courseID;

                                break;

                            default:
                                break;
                        }

                        break;
                    default:
                        break;
                }
            }
           
        }

        //set the views
        View getViews(View view)
        {
            // Change the view based on system language
            SATDAY = view.FindViewById<TextView>(Resource.Id.Sat_day);
            SATDAY.Text = CScore.FixdStrings.Days.SAT();
            SUNDAY = view.FindViewById<TextView>(Resource.Id.Sun_day);
            SUNDAY.Text = CScore.FixdStrings.Days.SUN();
            MONDAY = view.FindViewById<TextView>(Resource.Id.Mon_day);
            MONDAY.Text = CScore.FixdStrings.Days.MON();
            TUEDAY = view.FindViewById<TextView>(Resource.Id.Tue_day);
            TUEDAY.Text = CScore.FixdStrings.Days.TUE();
            WEDDAY = view.FindViewById<TextView>(Resource.Id.Wed_day);
            WEDDAY.Text = CScore.FixdStrings.Days.WED();
            THUDAY = view.FindViewById<TextView>(Resource.Id.Thu_day);
            THUDAY.Text = CScore.FixdStrings.Days.THU();
            DH = view.FindViewById<TextView>(Resource.Id.DandH);
            DH.Text = CScore.FixdStrings.Days.DandH();
            // saturday
            SA1 = view.FindViewById<TextView>(Resource.Id.SA1);
             SA2 = view.FindViewById<TextView>(Resource.Id.SA2);
             SA3 = view.FindViewById<TextView>(Resource.Id.SA3);
             SA4 = view.FindViewById<TextView>(Resource.Id.SA4);
             SA5 = view.FindViewById<TextView>(Resource.Id.SA5);
             SA6 = view.FindViewById<TextView>(Resource.Id.SA6);
            //sunday
             SU1 = view.FindViewById<TextView>(Resource.Id.SU1);
             SU2 = view.FindViewById<TextView>(Resource.Id.SU2);
             SU3 = view.FindViewById<TextView>(Resource.Id.SU3);
             SU4 = view.FindViewById<TextView>(Resource.Id.SU4);
             SU5 = view.FindViewById<TextView>(Resource.Id.SU5);
             SU6 = view.FindViewById<TextView>(Resource.Id.SU6);
            //monday
             MO1 = view.FindViewById<TextView>(Resource.Id.MO1);
             MO2 = view.FindViewById<TextView>(Resource.Id.MO2);
             MO3 = view.FindViewById<TextView>(Resource.Id.MO3);
             MO4 = view.FindViewById<TextView>(Resource.Id.MO4);
             MO5 = view.FindViewById<TextView>(Resource.Id.MO5);
             MO6 = view.FindViewById<TextView>(Resource.Id.MO6);
            //tuesday
             TU1 = view.FindViewById<TextView>(Resource.Id.TU1);
             TU2 = view.FindViewById<TextView>(Resource.Id.TU2);
             TU3 = view.FindViewById<TextView>(Resource.Id.TU3);
             TU4 = view.FindViewById<TextView>(Resource.Id.TU4);
             TU5 = view.FindViewById<TextView>(Resource.Id.TU5);
             TU6 = view.FindViewById<TextView>(Resource.Id.TU6);
            //wedensday
             WE1 = view.FindViewById<TextView>(Resource.Id.WE1);
             WE2 = view.FindViewById<TextView>(Resource.Id.WE2);
             WE3 = view.FindViewById<TextView>(Resource.Id.WE3);
             WE4 = view.FindViewById<TextView>(Resource.Id.WE4);
             WE5 = view.FindViewById<TextView>(Resource.Id.WE5);
             WE6 = view.FindViewById<TextView>(Resource.Id.WE6);
            //thursday
             TH1 = view.FindViewById<TextView>(Resource.Id.TH1);
             TH2 = view.FindViewById<TextView>(Resource.Id.TH2);
             TH3 = view.FindViewById<TextView>(Resource.Id.TH3);
             TH4 = view.FindViewById<TextView>(Resource.Id.TH4);
             TH5 = view.FindViewById<TextView>(Resource.Id.TH5);
             TH6 = view.FindViewById<TextView>(Resource.Id.TH6);
            return view;
                
        }
       
    }
    
}