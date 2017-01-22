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
        int startFrom;
        int NumberOfMessages;
        int currentIndex;

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

            CScore.BCL.StatusWithObject<List<CScore.BCL.Messages>> messages =
                new StatusWithObject<List<CScore.BCL.Messages>>();

            startFrom = 0;
            NumberOfMessages = 100;
            RecyclerViewAdapter.names = new List<string>();
        var task = Task.Run(async () => { // await this.getRecievedMessages();
                messages = await CScore.BCL.Messages.getMessages(NumberOfMessages, startFrom, "R");
              
                                });
            task.Wait();
            if (!messages.status.status)
                messages.statusObject = new List<CScore.BCL.Messages>();

            currentIndex = messages.statusObject.Count();

            recyclerView.SetLayoutManager(new LinearLayoutManager(recyclerView.Context));
            recyclerView.SetAdapter(new RecyclerViewAdapter(messages.statusObject));

            recyclerView.SetItemClickListener((rv, position, view) =>
            {
                //An item has been clicked
                Context context = view.Context;
                      
                Intent intent = new Intent(context, typeof(MessageDetailsActivity));
                intent.PutExtra("sender", RecyclerViewAdapter.names[position]);
                intent.PutExtra("title", messages.statusObject[position].Mes_subject);
                intent.PutExtra("content", messages.statusObject[position].Mes_content);
                
                context.StartActivity(intent);
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