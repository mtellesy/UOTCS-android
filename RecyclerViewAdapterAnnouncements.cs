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
using Android.Support.V7.Widget;
using Android.Content.Res;
using Android.Util;

using Refractored.Controls;
using Android.Graphics;
using System.Threading.Tasks;
using CScore.BCL;

namespace UOTCS_android
{
    class RecyclerViewAdapterAnnouncements : RecyclerView.Adapter
    {

        public List<CScore.BCL.Announcements> mValues;

        public RecyclerViewAdapterAnnouncements(List<CScore.BCL.Announcements> items)
        {
            mValues = items;
            
        }

        public override int ItemCount
        {
            get
            {
                return mValues.Count;
            }
        }
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).
                        Inflate(Resource.Menu.Card_row_announcements, parent, false);
            AnnouncementViewHolder vh = new AnnouncementViewHolder(itemView);
            return vh;
        }

        public override void
            OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            AnnouncementViewHolder vh = holder as AnnouncementViewHolder;

            int x = position;
            //    vh.Image.SetImageResource(mPhotoAlbum[position].PhotoID);
            vh.content.Text = mValues[position].Ano_content;
            var Sender = new StatusWithObject<OtherUsers>();
            var task = Task.Run( async () => {

                Sender = await CScore.BCL.OtherUsers.getOtherUser(mValues[position].Ano_sender);
            });
            task.Wait();
            if (Sender.statusObject != null)
                vh.sender.Text = Sender.statusObject.use_nameEN;
            vh.time.Text = mValues[position].Ano_time;
        }

    }
    public class AnnouncementViewHolder : RecyclerView.ViewHolder
    {


        public TextView sender;
        public TextView time;
        public TextView content;

        public AnnouncementViewHolder(View view) : base(view)
        {

            View x = view;

            sender = view.FindViewById<TextView>(Resource.Id.announcement_sender_card);
            time = view.FindViewById<TextView>(Resource.Id.announcement_time_card);
            content = view.FindViewById<TextView>(Resource.Id.announcement_content_card);

        }

        public override string ToString()
        {
            return base.ToString() + " '" + content.Text;
        }
    }
}
