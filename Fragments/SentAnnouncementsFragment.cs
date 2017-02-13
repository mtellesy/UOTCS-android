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
            recyclerView = inflater.Inflate(Resource.Layout.AnnouncementsF, container, false) as RecyclerView;



            SetUpRecyclerView(recyclerView);
            return recyclerView;
        }
        private void SetUpRecyclerView(RecyclerView recyclerView)
        {
           
            var values = new CScore.BCL.StatusWithObject<List<CScore.BCL.Announcements>>();
            //           var values = GetRandomSubList(y, x.Length);
            var task = Task.Run(async () => { values = await this.getRecievedAnnouncements(); });
            task.Wait();
            recyclerView.SetLayoutManager(new LinearLayoutManager(recyclerView.Context));
            if (values.statusObject != null)
                if (Values.Use_typeID > 1)
                {
                        recyclerView.SetAdapter(new RecyclerViewAdapterAnnouncements(values.statusObject));
                }
                 else
                {
                    List<CScore.BCL.Announcements> final = new List<CScore.BCL.Announcements>();
                    StatusWithObject<List<Course>> lecturers = new StatusWithObject<List<Course>>();
                    var task2 = Task.Run(async () => { lecturers = await this.getStudentLecturers(); });
                    task.Wait();
                    if (lecturers.statusObject != null)
                    {
                        foreach (CScore.BCL.Announcements x in values.statusObject)
                        {
                            foreach (Course y in lecturers.statusObject)
                            {
                                if (x.Ano_sender == y.Tea_id)// queryiong specific announcements for the student.
                                {
                                    final.Add(x);
                                }

                            }
                        }
                        recyclerView.SetAdapter(new RecyclerViewAdapterAnnouncements(final));
                    }
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
            if (Values.Use_typeID > 1)
            {
                results = await CScore.BCL.Announcements.getSentAnnouncements(100, 1, "S", null);

            }
            else
            {
                results = await CScore.BCL.Announcements.getAnnouncements(100, 1, "R", null);

            }

            return results;
        }
    }
}