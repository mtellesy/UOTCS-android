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
   public class TimetableAndMidmarkAndroid
    {
        private String name;
        private String _value;
       
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
        public String Value
        {
            set
            {
                _value = value;
            }
            get
            {
                return _value;
            }
        }

        public TimetableAndMidmarkAndroid(CScore.BCL.MidMarkDistribution midmark)
        {
            this.name = midmark.Mid_nameEN;
            this._value = midmark.Grade.ToString();
        }

        public TimetableAndMidmarkAndroid(String name, String content)
        {
            this.name = name;
            this._value = content;
        }

        
    }
}