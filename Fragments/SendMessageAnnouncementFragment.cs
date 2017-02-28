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

namespace UOTCS_android.Fragments
{
    public class SendMessageAnnouncementFragment : SupportFragment
    {
        View view;
        private string type;
        public SendMessageAnnouncementFragment(string type)
        {
            this.type = type;
        }
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
           view = inflater.Inflate(Resource.Layout.SendMessageAnnouncementF, container, false);
            setHintContent(view);
            return view;
        }

        private void setHintContent(View view)
        {
            EditText content = view.FindViewById<EditText>(Resource.Id.content_message_announcement_fragment);
            if (type == "Announcement")
            {
                content.SetHint(Resources.GetIdentifier("compose_announcement_hint","string", Activity.PackageName)); 
            }else if (type == "Message")
            {
                content.SetHint(Resources.GetIdentifier("compose_message_hint","string",Activity.PackageName));
            }
        }
    }
}