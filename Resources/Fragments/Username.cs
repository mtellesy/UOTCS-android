using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using CircleImageView = Refractored.Controls.CircleImageView;

namespace UOTCS_android.Fragments
{
    public class Username : Android.Support.V4.App.Fragment
    {
        TextView userNameAR;
        TextView userNameEN;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.usernameF, container, false);
            Android.Graphics.Drawables.Drawable x;
            if (Values.Use_typeID > 1)
            {
                //int id = Resources.GetIdentifier("lecturer_pic", "drawable", PackageName);
               x  = Resources.GetDrawable(Resource.Drawable.lecturer_pic);
                
            }
            else
            {
                x = Resources.GetDrawable(Resource.Drawable.student_pic);
            }
            CircleImageView imageView = view.FindViewById<CircleImageView>(Resource.Id.profile_image2);
            imageView.SetImageDrawable(x);
            userNameAR = view.FindViewById<TextView>(Resource.Id.usernameF_nameInAR_lable);
            userNameAR.Text = CScore.BCL.User.use_nameAR;
            userNameEN = view.FindViewById<TextView>(Resource.Id.usernameF_nameInEN_lable);
            userNameEN.Text = CScore.BCL.User.use_nameEN;
            return view ;
        }
    }
}