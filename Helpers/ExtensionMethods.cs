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

namespace UOTCS_android.Helpers
{
   public static  class  ExtensionMethods
    {
        public static void SetItemClickListener(this RecyclerView rv, Action<RecyclerView, int, View> action)
        {
            rv.AddOnChildAttachStateChangeListener(new AttachStateChangeListener(rv, action));
        }
    }

    public class AttachStateChangeListener : Java.Lang.Object, RecyclerView.IOnChildAttachStateChangeListener
    {
        private RecyclerView mRecyclerview;
        private Action<RecyclerView, int, View> mAction;

        public AttachStateChangeListener(RecyclerView rv, Action<RecyclerView, int, View> action) : base()
        {
            mRecyclerview = rv;
            mAction = action;
        }

        public void OnChildViewAttachedToWindow(View view)
        {
            view.Click += View_Click;
        }

        public void OnChildViewDetachedFromWindow(View view)
        {
            view.Click -= View_Click;
        }

        private void View_Click(object sender, EventArgs e)
        {
            RecyclerView.ViewHolder holder = mRecyclerview.GetChildViewHolder(((View)sender));
            mAction.Invoke(mRecyclerview, holder.AdapterPosition, ((View)sender));
        }
    }
}
