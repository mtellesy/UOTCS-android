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

namespace UOTCS_android
{
    class RecyclerViewAdapter:RecyclerView.Adapter
    {

        public List<string> mValues;

        public RecyclerViewAdapter( List<string> items)
        {
            mValues = items;
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

            int x = position;
       //     vh.Image.SetImageResource(mPhotoAlbum[position].PhotoID);
            vh.mTxtView.Text = mValues[position];
            Random rnd = new Random();
            Color color = new Color(Color.Argb(255, rnd.Next(256), rnd.Next(256), rnd.Next(256)));

            var drawable = Android.Ui.TextDrawable.TextDrawable.TextDrwableBuilder.BeginConfig().UseFont(Typeface.Default).FontSize(50).ToUpperCase().Height(60).Width(60)
                .EndConfig().BuildRound("x", color);


            vh.mImageView.SetImageDrawable(drawable);
        }

    }
    public class SimpleViewHolder : RecyclerView.ViewHolder
    {
       
        public  CircleImageView mImageView;
        public  TextView mTxtView;

        public SimpleViewHolder(View view) : base(view)
        {

            View x = view;
            mImageView = view.FindViewById<CircleImageView>(Resource.Id.profile_pic_row);
            mTxtView = view.FindViewById<TextView>(Resource.Id.message_title_row);
        }

        public override string ToString()
        {
            return base.ToString() + " '" + mTxtView.Text;
        }
    }
}
