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
using Refractored.Controls;

namespace UOTCS_android
{
    
    [Activity(Label = "Settings", Icon = "@drawable/icon", Theme = "@style/Theme.Student", ParentActivity = typeof(Profile))]
    public class Settings : AppCompatActivity
    {
        private SupportToolbar toolBar;
        private SupportActionBar actionbar;
        private DrawerLayout drawerLayout;
        private NavigationView navigationView;
        private View view;
        private CircleImageView profileImage;

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
            findViews();
            SetSupportActionBar(toolBar);
            setUpActionBar(actionbar);
            setUpNavigationView(navigationView);

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
           

            
            handleEvents();
        }

     

        private void findViews()
        {
            toolBar = FindViewById<SupportToolbar>(Resource.Id.toolBar);
            SetSupportActionBar(toolBar);
            actionbar = SupportActionBar;
            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            view = navigationView.GetHeaderView(0);
            profileImage = view.FindViewById<CircleImageView>(Resource.Id.nav_profile);

        }
        private void setUpActionBar(SupportActionBar actionBar)
        {
            actionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);
            actionBar.SetDisplayHomeAsUpEnabled(true);
        }
        private void setUpNavigationView(NavigationView navigationView)
        {
            Values.changeNavigationItems(navigationView, this);
            if (navigationView != null)
            {
                SetUpDrawerContent(navigationView);
            }
            navigationView.SetCheckedItem(Resource.Id.nav_settings);

        }
        private void SetUpDrawerContent(NavigationView navigationView)
        {
            Values.handleSetUpDrawerContent(navigationView, drawerLayout);
        }
        public int getCurrentActvity()
        {
            return Resource.Id.nav_settings;
        }
        private void handleEvents()
        {
            navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;
            profileImage.Click += ProfileImage_Click;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    drawerLayout.OpenDrawer((int)GravityFlags.Start);
                    return true;


                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
        public override void OnBackPressed()
        {
            MoveTaskToBack(true);
        }

        private void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            drawerLayout.CloseDrawers();
            if (e.MenuItem.ItemId != getCurrentActvity())
                Values.handleSwitchActivities(this, e.MenuItem.ItemId);

        }
        private void ProfileImage_Click(object sender, EventArgs e)
        {
            drawerLayout.CloseDrawers();
            Intent intent = new Intent(this, typeof(Profile));
            this.StartActivity(intent);
            Finish();
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