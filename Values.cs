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
using Android.Graphics;

namespace UOTCS_android
{
    public static  class Values
    {
        private static int use_typeID;

        public static int Use_typeID
        {
            set
            {
                use_typeID = value;
            }
            get
            {
                return use_typeID;
            }
        }
        private static Color use_color;
        public static Color Use_Color
        {
            set
            {
                
                if (Values.use_typeID>1)
                {
                     use_color = new Color(0, 150, 136);
                }
                else
                {
                    use_color = new Color(63, 81, 181);
                }
                
            }
            get
            {
                return use_color;
            }
        }



        public static void changeTheme(Context myActivity)
        {
            if (Values.Use_typeID > 1)
            {
               myActivity.SetTheme(Resource.Style.Theme_Lecturer);
            }
        }

    }
}