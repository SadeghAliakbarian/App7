using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;

namespace App7
{
    [Activity(Label = "App7", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.HomeBannerLayout);

            ImageView bannerImage = FindViewById<ImageView>(Resource.Id.img_banner);
            bannerImage.Click += BannerImage_Click;
        }

        private void BannerImage_Click(object sender, System.EventArgs e)
        {
            var home_activity = new Intent(this, typeof(HomePageActivity));
            StartActivity(home_activity);
        }


        
    }
}

