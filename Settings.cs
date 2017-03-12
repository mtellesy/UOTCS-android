using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using System.Linq;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using SupportActionBar = Android.Support.V7.App.ActionBar;

using Android.Support.V4.Widget;
using SupportFragment = Android.Support.V4.App.Fragment;
using SupportFragmentManager = Android.Support.V4.App.FragmentManager;

using Android.Support.V7.App;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Content.Res;
using Java.Util;
using Android.Support.V4.Content;

namespace UOTCS_android
{
    
    [Activity(Label = "Settings", Icon = "@drawable/icon", Theme = "@style/Theme.Student", ParentActivity = typeof(Profile))]
    public class Settings : MainActivity
    {
        public static TextView total_credit;

        protected override void OnPause()
        {
            base.OnPause();
            this.Finish();
        }

        protected async override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            this.Title = CScore.FixdStrings.Settings.SettingsLable();
            SetContentView(Resource.Layout.Settings);
            
            var SaveSettingsButton = FindViewById<FloatingActionButton>(Resource.Id.fab);
            SaveSettingsButton.SetImageDrawable(ContextCompat.GetDrawable(this, Resource.Drawable.ic_save));
            SaveSettingsButton.Visibility = ViewStates.Visible;

            var LanguageSpinner = FindViewById<Spinner>(Resource.Id.LanguageSpinner);

            List<String> languages = new List<String>();
            languages.Add("«·⁄—»Ì…");
            languages.Add("English");

            var adapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleSpinnerItem, languages);
            LanguageSpinner.Adapter = adapter;
            // set the spinner selected item to the current language setting
            var Currentlanuage = CScore.FixdStrings.LanguageSetter.getLanguage();
            int id =0; // id of Language spinner
            switch(Currentlanuage)
            {
                case CScore.FixdStrings.Language.EN:
                    id = 1;
                    break;
                case CScore.FixdStrings.Language.AR:
                    id = 0;
                    break;

            }
            LanguageSpinner.SetSelection(id);
            SaveSettingsButton.Click += (sender, args) => 
            {
                if(LanguageSpinner.SelectedItemPosition == 0)
                {
                    CScore.FixdStrings.LanguageSetter.setLanguage(CScore.FixdStrings.Language.AR);            
                    Configuration configuration = this.Resources.Configuration;
                    configuration.SetLayoutDirection(new Locale("ar"));//= LayoutDirection.Locale;
                    this.Resources.UpdateConfiguration(configuration, this.Resources.DisplayMetrics);
                }
                else
                {
                    CScore.FixdStrings.LanguageSetter.setLanguage(CScore.FixdStrings.Language.EN);
                    Configuration configuration = this.Resources.Configuration;
                    configuration.SetLayoutDirection(Locale.English);//= LayoutDirection.Locale;
                    this.Resources.UpdateConfiguration(configuration, this.Resources.DisplayMetrics);
                }
                this.showMessage();
                
            };

            findViews();
            handleEvents();
        }

     

        private void findViews()
        {
            base.findViews();
        }

        private void handleEvents()
        {

        }

        private void SetUpDrawerContent(NavigationView navigationView)
        {
            base.SetUpDrawerContent(navigationView);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            bool x = base.OnOptionsItemSelected(item);
            return x;
        }
        public int getCurrentActvity()
        {
            return Resource.Id.nav_timetable;
        }

        private void showMessage()
        {
            Android.Support.V7.App.AlertDialog.Builder alert =
           new Android.Support.V7.App.AlertDialog.Builder(this);
            alert.SetTitle(CScore.FixdStrings.Settings.SettingsStatus());
            alert.SetMessage(CScore.FixdStrings.Settings.SettingsSaved());
            alert.SetPositiveButton(CScore.FixdStrings.Buttons.DONE(), (senderAlert, args) => {
            Intent intent = new Intent(this, typeof(Profile));
            StartActivity(intent);
            this.Finish();
            });

            Dialog x = alert.Create();
            x.OnWindowFocusChanged(true);
            x.Show();
        }

        
    }
}