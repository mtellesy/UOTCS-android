
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
using System.Collections.Generic;
namespace UOTCS_android.Fragments
{
    public class RecievedAnnouncementsFragment : SupportFragment
    {
        private RecyclerView recyclerView;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            recyclerView = inflater.Inflate(Resource.Layout.AnnouncementsF, container, false) as RecyclerView;



            SetUpRecyclerView(recyclerView);
            return recyclerView;
        }
        private void SetUpRecyclerView(RecyclerView recyclerView)
        {
            
            var values = new CScore.BCL.StatusWithObject<List<CScore.BCL.Announcements>>();
            try
            {
                var task = Task.Run(async () => { values = await this.getRecievedAnnouncements(); });
                task.Wait();
            }
            catch(AggregateException ex)
            {
           //     values.status.message = ex.ToString();
            }

            recyclerView.SetLayoutManager(new LinearLayoutManager(recyclerView.Context));
            if (values.statusObject != null)
            {
                List<CScore.BCL.Announcements> final = new List<CScore.BCL.Announcements>();
                foreach (CScore.BCL.Announcements x in values.statusObject)
                {
                    if (x.Cou_id == null)
                    {
                        final.Add(x);
                    }
                }
                recyclerView.SetAdapter(new RecyclerViewAdapterAnnouncements(final));
            }
                    
                }

        

        private async Task<StatusWithObject<List<Course>>> getStudentLecturers()
        {
            CScore.BCL.StatusWithObject<List<CScore.BCL.Course>> results = new StatusWithObject<List<CScore.BCL.Course>>();
            results = await CScore.BCL.Course.getStudentCourses();
            return results;
        }

        private async Task<CScore.BCL.StatusWithObject<List<CScore.BCL.Announcements>>> getRecievedAnnouncements()
        {
            CScore.BCL.StatusWithObject<List<CScore.BCL.Announcements>> results = new StatusWithObject<List<CScore.BCL.Announcements>>();
            try
            {
                results = await CScore.BCL.Announcements.getAnnouncements(100, 1, "R", null);

            }
            catch (AggregateException ex)
            {
                results.status.status = false;
                results.status.message = ex.ToString();
            }
            int x = 1 + 1;
            return results;
        }
        
    }
}