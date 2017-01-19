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
using CScore.BCL;
namespace UOTCS_android
{
  public  class ResultAndroid
    {
        private String code;
        private String name;
        private float total;
        private String group;
        public String Code
        {
            set
            {
                code = value;
            }
            get
            {
                return code;
            }
        }
        public String Name
        {
            set
            {
                name = value;
            }
            get
            {
                return name;
            }
        }
        public float Result
        {
            set
            {
                total = value;
            }
            get
            {
                return total;
            }
        }
        public String Group
        {
            set
            {
                group = value;
            }
            get
            {
                return group;
            }
        }
        public ResultAndroid(CScore.BCL.Result result, string course_name)
        {
            code = result.Cou_id;
            name = course_name;
            this.total = calculateTotal(result);
        }

        public ResultAndroid(CScore.BCL.AllResult result)
        {
            code = result.Cou_id;
            name = result.Cou_nameEN;
            this.total = result.Res_total;
        }
        public ResultAndroid (CScore.BCL.Course course)
        {
            CScore.BCL.Schedule temp = new CScore.BCL.Schedule();
            code = course.Cou_id;
            name = course.Cou_nameEN;
            temp = course.Schedule.First();
            group = temp.Gro_NameEN;
        }
        private float calculateTotal(CScore.BCL.Result result)
        {
            float total = 0;
            foreach(CScore.BCL.MidMarkDistribution x in result.MidExams)
            {
                total += x.Grade;
            }
            total += result.Final;
            return total;
        }

        
    }
}