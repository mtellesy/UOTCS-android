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

using Android.Support.V7.Widget;
using UOTCS_android.Helpers;
using CScore.BCL;
using System.Threading.Tasks;
using Android.Graphics;
using Android.Support.V4.App;
using CScore.FixdStrings;

namespace UOTCS_android.Fragments
{
    public class PersonalProfileFragment : SupportFragment
    {
        private FragmentActivity myContext;
        private TextView email;
        private TextView address;
        private TextView usernamAR;
        private TextView usernameEN;
        private TextView phone;
        private TextView nationality;
        private int user_id;
        public OtherUsers lecturer;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override void OnAttach(Activity activity)
        {
            base.OnAttach(activity);
            myContext = (FragmentActivity)activity;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            
            View view = inflater.Inflate(Resource.Layout.personalProfile, container, false);
            address = view.FindViewById<TextView>(Resource.Id.address_textview);
            email = view.FindViewById<TextView>(Resource.Id.email_textview);
            phone = view.FindViewById<TextView>(Resource.Id.phone_textview);
            nationality = view.FindViewById<TextView>(Resource.Id.nationality_textview);
            usernamAR = view.FindViewById<TextView>(Resource.Id.usernameF_nameInAR_lable);
            usernameEN = view.FindViewById<TextView>(Resource.Id.usernameF_nameInEN_lable);
            bingData();
            Language e = LanguageSetter.getLanguage();
            switch (e)
            {
                case (Language.AR):
                    nationality.Text = "ليبيا";
                    break;

                case (Language.EN):
                default:
                    nationality.Text = "Libya";
                    break;
            }
            return view;
        }

        private void bingData()
        {
            if (lecturer ==null)
            {
                usernamAR.Text = CScore.BCL.User.use_nameAR;
                usernameEN.Text = CScore.BCL.User.use_nameEN;
                address.Text = "tripoli,Libya";
                phone.Text = CScore.BCL.User.use_phone.ToString();
                email.Text = CScore.BCL.User.use_email;
            }
            else 
            {
                usernamAR.Text = lecturer.use_nameAR;
                usernameEN.Text = lecturer.use_nameEN;
                address.Text = "tripoli,Libya";
                phone.Text = lecturer.use_phone.ToString();
                email.Text = lecturer.use_email;
            }
         }
    }
}