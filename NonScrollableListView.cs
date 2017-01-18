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
using Android.Util;

namespace UOTCS_android
{
    public class NonScrollListView : ListView
    {

        public NonScrollListView(Context context) : base(context)
        {
        }
        public NonScrollListView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }
        public NonScrollListView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
        }
        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            int heightMeasureSpec_custom = MeasureSpec.MakeMeasureSpec(int.MaxValue >> 2, MeasureSpecMode.AtMost);
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec_custom);
            ViewGroup.LayoutParams @params = LayoutParameters;
            @params.Height = MeasuredHeight;
        }
    }
}