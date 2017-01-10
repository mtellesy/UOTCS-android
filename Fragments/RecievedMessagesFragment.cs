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

namespace UOTCS_android.Fragments
{
    public  class RecievedMessagesFragment : SupportFragment
    {
        RecyclerView recyclerView;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override  View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

             recyclerView = inflater.Inflate(Resource.Layout.RecievedMessagesF, container, false) as RecyclerView;



                SetUpRecyclerView(recyclerView);
            return recyclerView;
        }

        private void SetUpRecyclerView(RecyclerView recyclerView)
        {
            String[] x = { "i ", "am", "amira", "and", "this", "should", "work", "well", "plz", ":(" };
            List<string> y = new List<string>(x);
    
            var values = y;
            //           var values = GetRandomSubList(y, x.Length);
          //  var task = Task.Run(async () => { await this.getRecievedMessages(); });
        //    task.Wait();
            recyclerView.SetLayoutManager(new LinearLayoutManager(recyclerView.Context));
            recyclerView.SetAdapter(new RecyclerViewAdapter( values));

            recyclerView.SetItemClickListener((rv, position, view) =>
            {
                //An item has been clicked
                Context context = view.Context;
                       String mes_id = "stupid";
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
        
     
        private async Task<CScore.BCL.StatusWithObject<List<CScore.BCL.Messages>>> getRecievedMessages()
        {
            CScore.BCL.StatusWithObject<List<CScore.BCL.Messages>> results = new StatusWithObject<List<CScore.BCL.Messages>>();
            results = await CScore.BCL.Messages.getMessages(10,1,null);
            if (results.status.status== true)
            {
                return results;
            }
            return results;
        }
    }
}