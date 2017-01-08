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
using CScore;
using Messages = CScore.BCL.Messages;
namespace UOTCS_android.Helpers
{
    public static class MessagesConverter
    {
        public static List<CScore.BCL.Messages> fromSentToRecieved(List<CScore.BCL.Messages> messages)
        {
            List<CScore.BCL.Messages> result = new List<CScore.BCL.Messages>();
            CScore.BCL.Messages temp = new CScore.BCL.Messages();
            foreach(CScore.BCL.Messages x in messages)
            {
                // do something here
            }
            return result;
        }
    }
}