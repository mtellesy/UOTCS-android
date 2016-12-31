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
                switch (dayID)
                {
                    case 1: // saturday
                        switch (timeID)
                        {
                            case 1: 
                                if(SA1.Text != "" || SA1.Text != null)
                                {
                                    SA1.Text += "\n";
                                }
                                SA1.Text += courseID;
                                break;

                            case 2:
                                if (SA2.Text != "" || SA2.Text != null)
                                {
                                    SA2.Text += "\n";
                                }
                                SA2.Text += courseID;
                                break;

                            case 3:
                                if (SA3.Text != "" || SA3.Text != null)
                                {
                                    SA3.Text += "\n";
                                }
                                SA3.Text += courseID;
                                break;

                            case 4:
                                if (SA4.Text != "" || SA4.Text != null)
                                {
                                    SA4.Text += "\n";
                                }
                                SA4.Text += courseID;
                                break;

                            case 5:
                                if (SA5.Text != "" || SA5.Text != null)
                                {
                                    SA5.Text += "\n";
                                }
                                SA5.Text += courseID;
                                break;

                            case 6:
                                if (SA6.Text != "" || SA6.Text != null)
                                {
                                    SA6.Text += "\n";
                                }
                                SA6.Text += courseID;
                                break;

                            default:
                                break;
                        }
                        break;

                    case 2: // saturday
                        switch (timeID)
                        {
                            case 1:
                                if (SU1.Text != "" || SU1.Text != null)
                                {
                    //                SU1.Text += "\n";
                                }
                                SU1.Text += courseID;
                                break;

                            case 2:
                                if (SU2.Text != "" || SU2.Text != null)
                                {
                                    SU2.Text += "\n";
                                }
                                SU2.Text += courseID;
                                break;

                            case 3:
                                if (SU3.Text != "" || SU3.Text != null)
                                {
                                    SU3.Text += "\n";
                                }
                                SU3.Text += courseID;
                                break;

                            case 4:
                                if (SU4.Text != "" || SU4.Text != null)
                                {
                                    SU4.Text += "\n";
                                }
                                SU4.Text += courseID;
                                break;

                            case 5:
                                if (SU5.Text != "" || SU5.Text != null)
                                {
                                    SU5.Text += "\n";
                                }
                                SU5.Text += courseID;
                                break;

                            case 6:
                                if (SU6.Text != "" || SU6.Text != null)
                                {
                                    SU6.Text += "\n";
                                }
                                SU6.Text += courseID;
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