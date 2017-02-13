using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CScore.BCL;
using Android.Provider;

namespace UOTCS_android
{
   
    public class MajorAdapter : BaseAdapter
    {
        List<DepartmentItem> DepartmentItemList;
        List<CScore.BCL.Department> _departments;
        List<ImageButton> majorButtons;

        //this is the department id that we are going to send
        public int FinalDepID;

        /// <summary>
        /// Is used to keep information about wither the button is active or not
        /// </summary>
      
        Activity _activity;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="departments"></param>
        /// <param name="dropOnly">If the process is a disEnrollment process set this to true</param>
        public MajorAdapter(Activity activity,List<CScore.BCL.Department> departments,bool newDropOnly)
        {
            
            _departments = new List<Department>();
            _departments = departments;
            _activity = activity;
            FillContacts(departments);
            majorButtons = new List<ImageButton>();
        }

    

         void FillContacts(List<CScore.BCL.Department> departments)
        {

           // int id = 0;
            DepartmentItemList = new List<DepartmentItem>();
            foreach(var department in departments)
            {
                DepartmentItem x = new DepartmentItem();
                x.DepartmentID = department.Dep_id;
                x.DepartmentNameAR = department.DepNameAR;
                x.DepartmentNameEN = department.Dep_nameEN;
                DepartmentItemList.Add(x);
            }


        }

        class DepartmentItem
        {
            public long Id { get; set; }
            public int DepartmentID { get; set; }
            public String DepartmentNameAR { get; set; }
            public String DepartmentNameEN { get; set; } 
        }

        public override int Count
        {
            get { return DepartmentItemList.Count; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            // could wrap a Contact in a Java.Lang.Object
            // to return it here if needed
            return null;
        }

        public override long GetItemId(int position)
        {
            return DepartmentItemList[position].Id;
        }

        /// <summary>
        /// Return the layout view and its content
        /// </summary>
        /// <param name="position">the current position of the list</param>
        /// <param name="convertView"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
           //declaration of the views
            var view = convertView ?? _activity.LayoutInflater.Inflate(
                Resource.Layout.MajorItemView, parent, false);
            var DepartmentName = view.FindViewById<TextView>(Resource.Id.majorDepartment);
          
            var MajorButton = view.FindViewById<ImageButton>(Resource.Id.majorButton);
            majorButtons.Add(MajorButton);

            // to know wither the button has been activated or not
            bool isClicked = false;
            switch(CScore.FixdStrings.LanguageSetter.getLanguage())
            {
                case (CScore.FixdStrings.Language.AR):
                    DepartmentName.Text = DepartmentItemList[position].DepartmentNameAR;
                    break;
                case (CScore.FixdStrings.Language.EN):
                default:
                    DepartmentName.Text = DepartmentItemList[position].DepartmentNameEN;
                    break;
            }


            majorButtons[position].Click += (object s, EventArgs e) => 
            {
                majorButtons[position].Selected=!majorButtons[position].Selected;

                if (!isClicked)
                {
                    //first turn off all the other buttons 
                    for (int i = 0; i < majorButtons.Count; i++)
                    {
                        majorButtons[i].Enabled = false;
                    }
                        

                    // turn this buttom again 
                    majorButtons[position].Enabled = true;

                    FinalDepID = DepartmentItemList[position].DepartmentID;

                    //now because it has been clicked
                    isClicked = true;
                }
                //if it's has been Clicked before
                else
                {
                    //here means the user deleted the department and want to choose another


                    //first turn on all the buttons 
                    for (int i = 0; i < majorButtons.Count; i++)
                        majorButtons[i].Enabled = true;
                    //delete the finalDepID by making it equels -1 
                    //-1 means no depatment has been choosen
                    FinalDepID = -1;

                    isClicked = false;
                }
            };

          
            
            

            return view;
        }

     
        private void showMessage(String message)
        {
            Android.Support.V7.App.AlertDialog.Builder alert =
           new Android.Support.V7.App.AlertDialog.Builder(_activity);
            alert.SetTitle("Login Status");
            alert.SetMessage(message);
            //alert.SetPositiveButton("OK", (senderAlert, args) => {
            //    Toast.MakeText(this, "", ToastLength.Short).Show();
            //});

            Dialog x = alert.Create();
            x.Show();
        }
    }
}