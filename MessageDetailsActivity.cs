using System;
using Android.App;
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
using Refractored.Controls;
using Android.Support.V4.App;
using Android.Graphics;

namespace UOTCS_android
{
    [Activity(Label = "Message", Icon = "@drawable/icon", Theme = "@style/Theme.Student", ParentActivity = (typeof(Messages)))]
    public class MessageDetailsActivity : AppCompatActivity
    {
        private SupportToolbar toolBar;
        private SupportActionBar actionbar;
        private DrawerLayout drawerLayout;
        private NavigationView navigationView;
        private View view;
        private CircleImageView profileImage;
        //for this activity 
        private TextView messageSender;
        private TextView messageTitle;
        private TextView messageContent;
        private String sender;
        private String title;
        private String content;

        protected override void OnCreate(Bundle bundle)
        {
            
            base.OnCreate(bundle);
            this.title = CScore.FixdStrings.Messages.MessagesLable();
            Intent intent = this.Intent;
            Bundle b = this.Intent.Extras;
           
            sender = b.GetString("sender");
            title = b.GetString("title");
            content = b.GetString("content");

            Values.changeTheme(this);
            SetContentView(Resource.Layout.MessageDetails);

            findViews();
            SetSupportActionBar(toolBar);
            setUpActionBar(actionbar);
            setUpNavigationView(navigationView);
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
            Random rnd = new Random();
            Color color = new Color(Color.Argb(255, rnd.Next(256), rnd.Next(256), rnd.Next(256)));
            CircleImageView image = FindViewById<CircleImageView>(Resource.Id.profile_pic_messageDetails);
            var drawable = Android.Ui.TextDrawable.TextDrawable.TextDrwableBuilder.BeginConfig().UseFont(Typeface.Default).FontSize(30).ToUpperCase().Height(60).Width(60)
                .EndConfig().BuildRound(sender[0].ToString(), color);
            image.SetImageDrawable(drawable);

            messageSender = FindViewById<TextView>(Resource.Id.username_messageDetails);
            messageTitle = FindViewById<TextView>(Resource.Id.title_messageDetails);
            messageContent = FindViewById<TextView>(Resource.Id.content_messageDetails);

            messageSender.Text = sender;
            messageTitle.Text = title;
            messageContent.Text = content;


        }

        private void setUpActionBar(SupportActionBar actionBar)
        {
            actionBar.SetHomeButtonEnabled(true);
            actionBar.SetDisplayHomeAsUpEnabled(true);
        }

        private void handleEvents()
        {
            navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;
            profileImage.Click += ProfileImage_Click;
        }

        private void setUpNavigationView(NavigationView navigationView)
        {
            Values.changeNavigationItems(navigationView, this);
            if (navigationView != null)
            {
                SetUpDrawerContent(navigationView);
            }
            navigationView.SetCheckedItem(Resource.Id.nav_messages);

        }

        private void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            drawerLayout.CloseDrawers();
            if (e.MenuItem.ItemId != getCurrentActvity())
                Values.handleSwitchActivities(this, e.MenuItem.ItemId, navigationView);

        }

        private void SetUpDrawerContent(NavigationView navigationView)
        {
            Values.handleSetUpDrawerContent(navigationView, drawerLayout);
        }

        public override void OnBackPressed()
        {
            MoveTaskToBack(true);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            base.OnOptionsItemSelected(item);
            if (item.ItemId == Android.Resource.Id.Home)
            {
                NavUtils.NavigateUpFromSameTask(this);
                return true;
            }

            return base.OnOptionsItemSelected(item);

        }

        public int getCurrentActvity()
        {
            return Resource.Id.nav_messages;
        }

        private void ProfileImage_Click(object sender, EventArgs e)
        {
            drawerLayout.CloseDrawers();
            Values.startProfile(this);
            Finish();
        }        

    }
}
    
        



    


