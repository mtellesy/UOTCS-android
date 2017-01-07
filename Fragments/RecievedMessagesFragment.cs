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

namespace UOTCS_android.Fragments
{
    public class RecievedMessagesFragment : SupportFragment
    {
        RecyclerView recyclerView;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

             recyclerView = inflater.Inflate(Resource.Layout.RecievedMessagesF, container, false) as RecyclerView;

                SetUpRecyclerView(recyclerView);
            return recyclerView;
        }

        private void SetUpRecyclerView(RecyclerView recyclerView)
        {
            String[] x = { "fuck", "me", "hard", "mother", "fucking", "bastard", "i", "hate", "my", "life" };
            List<string> y = new List<string>(x);
    
            var values = y;
 //           var values = GetRandomSubList(y, x.Length);

            recyclerView.SetLayoutManager(new LinearLayoutManager(recyclerView.Context));
            recyclerView.SetAdapter(new RecyclerViewAdapter( values));

            recyclerView.SetItemClickListener((rv, position, view) =>
            {
                //An item has been clicked
                Context context = view.Context;
                       String mes_id = "stuped";
                       Intent intent = new Intent(context, typeof(MessageDetailsActivity));
                       intent.PutExtra(mes_id, values[position]);

                       context.StartActivity(intent);

        /*        MessageDetailsFragment nextFrag = new MessageDetailsFragment();
                this.FragmentManager.BeginTransaction()
                .Replace(Resource.Id.Fragment_messageContainer, nextFrag, null)
                .AddToBackStack(null)
                .Hide(this)
                .Commit();*/
            });
        }

     
        private List<string> GetRandomSubList(List<string> items, int amount)
        {
            List<string> list = new List<string>();
            Random random = new Random();
            while (list.Count < amount)
            {
                list.Add(items[random.Next(items.Count)]);
            }

            return list;
        }
    }
}