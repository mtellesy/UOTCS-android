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
    public class SentAnnouncementsFragment : SupportFragment
    {
        private RecyclerView recyclerView;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
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
            recyclerView.SetAdapter(new RecyclerViewAdapterAnnouncements(values));


        }


        private async Task<CScore.BCL.StatusWithObject<List<CScore.BCL.Messages>>> getRecievedMessages()
        {
            CScore.BCL.StatusWithObject<List<CScore.BCL.Messages>> results = new StatusWithObject<List<CScore.BCL.Messages>>();
            results = await CScore.BCL.Messages.getMessages(10, 1, null);
            if (results.status.status == true)
            {
                return results;
            }
            return results;
        }

    }
}