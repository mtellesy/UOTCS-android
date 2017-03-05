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
using CScore;
using System.Threading.Tasks;
using CScore.BCL;

namespace UOTCS_android
{
    class RecyclerViewAdapter:RecyclerView.Adapter
    {

        public List<CScore.BCL.Messages> mValues;
       static public List<String> names;

        public RecyclerViewAdapter( List<CScore.BCL.Messages> items)
        {

            mValues = items;
            names = new List<string>();
            int x = 1 + 1;
        }

        public override int ItemCount
        {
            get
            {
                return mValues.Count;
            }
        }
        public override RecyclerView.ViewHolder   OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).
                        Inflate(Resource.Menu.List_row_messages, parent, false);
            SimpleViewHolder vh = new SimpleViewHolder(itemView);
            return vh;
        }

        public override void
            OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            SimpleViewHolder vh = holder as SimpleViewHolder;
            String username = null;
            int x = position;
            //     vh.Image.SetImageResource(mPhotoAlbum[position].PhotoID);
            vh.messageTitle.Text = mValues[position].Mes_subject;
            // get sender name 
            var messageSender =new  StatusWithObject<OtherUsers>();
            var task = Task.Run(
                async () => { messageSender= await CScore.BCL.OtherUsers.getOtherUser(mValues[position].Mes_sender); }
                );
            task.Wait();

          
            if (messageSender.statusObject!= null)
              username =  messageSender.statusObject.use_nameEN;

            if (String.IsNullOrEmpty(username))
                username = "User";
            Random rnd = new Random();
            Color color = new Color(Color.Argb(255, rnd.Next(256), rnd.Next(256), rnd.Next(256)));
            String firstChar;
            if (String.IsNullOrEmpty(username))//mValues[position].Mes_subject))
                firstChar = "M";
            else
                firstChar = username[0].ToString();// mValues[position].Mes_subject[0].ToString();

            var drawable = Android.Ui.TextDrawable.TextDrawable.TextDrwableBuilder.BeginConfig().UseFont(Typeface.Default).FontSize(50).ToUpperCase().Height(60).Width(60)
                .EndConfig().BuildRound(firstChar.ToString(), color);

            vh.messageSender.Text = username;
            names.Add(username);
            vh.messageImage.SetImageDrawable(drawable);

            vh.messageContent.Text = mValues[position].Mes_content;
        }

    }
    public class SimpleViewHolder : RecyclerView.ViewHolder
    {
       
        public  CircleImageView messageImage;
        public  TextView messageTitle;
        public TextView messageContent;
        public TextView messageSender;

        public SimpleViewHolder(View view) : base(view)
        {

            View x = view;
            messageImage = view.FindViewById<CircleImageView>(Resource.Id.profile_pic_row);
            messageTitle = view.FindViewById<TextView>(Resource.Id.message_title_row);
            messageContent = view.FindViewById<TextView>(Resource.Id.message_conent_row);
            messageSender = view.FindViewById<TextView>(Resource.Id.user_name_row);

        }

        public override string ToString()
        {
            return base.ToString() + " '" + messageTitle.Text;
        }
    }
}
