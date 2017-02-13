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
        private RadioGroup radioLanguage;
        private RadioButton radioArabic;
        private RadioButton radioEnglish;
        private RadioButton radioIndigo;
        private RadioButton radioTeal;
        public static TextView total_credit;
        private int LanguageId = 0;
        private int ThemeId = 0;
        protected override void OnPause()
        {
            base.OnPause();
            this.Finish();
        }

        protected async override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            this.Title = CScore.FixdStrings.Settings.SettingsLable();
            Values.changeTheme(this);
            SetContentView(Resource.Layout.Settings);

            findViews();
            SetSupportActionBar(toolBar);
            setUpActionBar(actionbar);
            setUpNavigationView(navigationView);

            var SaveSettingsButton = FindViewById<FloatingActionButton>(Resource.Id.fab);
            SaveSettingsButton.SetImageDrawable(ContextCompat.GetDrawable(this, Resource.Drawable.ic_save));
            SaveSettingsButton.Visibility = ViewStates.Visible;

           
            // set the spinner selected item to the current language setting
            var Currentlanuage = CScore.FixdStrings.LanguageSetter.getLanguage();
           // id of Language spinner
            switch(Currentlanuage)
            {
                case CScore.FixdStrings.Language.EN:
                    LanguageId = 1;
                    radioEnglish.Checked = true;
                    radioArabic.Checked = false;
                    break;
                case CScore.FixdStrings.Language.AR:
                    LanguageId = 0;
                    radioArabic.Checked = true;
                    radioEnglish.Checked = false;
                    break;
            }
            var currentTheme = CScore.FixdStrings.ThemeSetter.getTheme();
            switch (currentTheme)
            {
                case CScore.FixdStrings.Theme.Indigo:
                    ThemeId = 0;
                    radioIndigo.Checked = true;
                    radioTeal.Checked = false;
                    break;
                case CScore.FixdStrings.Theme.Teal:
                    ThemeId = 0;
                    radioTeal.Checked = true;
                    radioIndigo.Checked = false;
                    break;
            }


      
            SaveSettingsButton.Click += (sender, args) => 
            {
                if(LanguageId == 0)
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
                if (ThemeId == 0)
                {
                    CScore.FixdStrings.ThemeSetter.setTheme(CScore.FixdStrings.Theme.Indigo);                    
                }
                else
                {
                    CScore.FixdStrings.ThemeSetter.setTheme(CScore.FixdStrings.Theme.Teal);
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
            radioArabic = FindViewById<RadioButton>(Resource.Id.radio_arabic);
            radioEnglish = FindViewById<RadioButton>(Resource.Id.radio_english);
            radioIndigo = FindViewById<RadioButton>(Resource.Id.radio_indigo);
            radioTeal = FindViewById<RadioButton>(Resource.Id.radio_teal);
            TextView langauge = FindViewById<TextView>(Resource.Id.language_label);
            TextView theme = FindViewById<TextView>(Resource.Id.theme_label);
            TextView aboutusLabel = FindViewById<TextView>(Resource.Id.aboutus_label);
            TextView aboutusText = FindViewById<TextView>(Resource.Id.aboutus_text);
            theme.Text = CScore.FixdStrings.Settings.ThemeLabel();
            langauge.Text = CScore.FixdStrings.Settings.LanguageLabel();
            radioArabic.Text = CScore.FixdStrings.Settings.LanguageLabelArabic();
            radioEnglish.Text = CScore.FixdStrings.Settings.LanguageLabelEnglish();
            radioIndigo.Text = CScore.FixdStrings.Settings.ThemeLabelIndigo();
            radioTeal.Text = CScore.FixdStrings.Settings.ThemeLabelTeal();
            aboutusLabel.Text = CScore.FixdStrings.Settings.aboutusLabel();
            aboutusText.Text = CScore.FixdStrings.Settings.aboutusText();
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
        [Java.Interop.Export("onRadioButtonClickedTheme")]
        public void onRadioButtonClickedTheme(View v)
        {
            switch (v.Id)
            {
                case Resource.Id.radio_indigo:
                    ThemeId = 0;
                    break;
                case Resource.Id.radio_teal:
                    ThemeId = 1;
                    break;
            }
        }
        [Java.Interop.Export("onRadioButtonClickedLanguage")]
        public void onRadioButtonClickedLanguage(View v)
        {
            switch (v.Id)
            {
                case Resource.Id.radio_arabic:
                    LanguageId = 0; 
                    break;
                case Resource.Id.radio_english:
                    LanguageId = 1;
                    break;
            }
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
                Values.handleSwitchActivities(this, e.MenuItem.ItemId, navigationView);

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
            alert.SetMessage(CScore.FixdStrings.Settings.SettingsSaved());
            alert.SetPositiveButton(CScore.FixdStrings.Buttons.OK(), (senderAlert, args) => {
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