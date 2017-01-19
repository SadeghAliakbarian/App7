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


namespace App7
{
    [Activity(Label = "AddPlayerActivity")]
    public class AddPlayerActivity : Activity
    {
        private SQLiteHelper dbHelper;
        string Name = "", ID = "", PhotoPath = "";
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.AddPlayerLayout);

            Button Done = FindViewById<Button>(Resource.Id.btn_new_player_done);
            Button Cancel = FindViewById<Button>(Resource.Id.btn_new_player_cancel);
            Button AddPhoto = FindViewById<Button>(Resource.Id.btn_add_player_photo);

            dbHelper = new SQLiteHelper(this);

            Done.Click += Done_Click;
            Cancel.Click += Cancel_Click;
            AddPhoto.Click += AddPhoto_Click;
        }

        private void AddPhoto_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            ImageView playerImage = FindViewById<ImageView>(Resource.Id.img_new_player_photo);
            var imageIntent = new Intent();
            imageIntent.SetType("image/*");
            imageIntent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(
                Intent.CreateChooser(imageIntent, "Select photo"), 0);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok)
            {
                var imageView =
                    FindViewById<ImageView>(Resource.Id.img_new_player_photo);
                
                imageView.SetImageURI(data.Data);
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            var home_activity = new Intent(this, typeof(HomePageActivity));
            StartActivity(home_activity);
            Finish();
        }

        private void Done_Click(object sender, EventArgs e)
        {
            EditText playerName = FindViewById<EditText>(Resource.Id.edit_new_player_name);
            EditText playerID = FindViewById<EditText>(Resource.Id.edit_new_player_ID);

            Name = playerName.Text.ToString();
            ID = playerID.Text.ToString();

            //var documents = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            //var filename = System.IO.Path.Combine(documents, "LeagueManagerPlayer_" + ID + ".txt");

            //System.IO.StreamWriter writer = new System.IO.StreamWriter(filename);

            //writer.WriteLine("name " + Name);
            //writer.WriteLine("id " + ID);
            //writer.WriteLine("photo " + PhotoPath);
            //writer.WriteLine("game 0");
            //writer.WriteLine("point 0");
            //writer.WriteLine("goal 0");
            //writer.WriteLine("stat 0 0 0");
            //writer.WriteLine("score 0");

            dbHelper.insertPlayer(Name, ID, "", "0", "0", "0", "0 0 0", "0");
            Toast.MakeText(this, "Data Successfully Stored", ToastLength.Short).Show();

            var home_activity = new Intent(this, typeof(HomePageActivity));
            StartActivity(home_activity);
            Finish();

        }
        
        //private Bitmap GetImageBitmapFromUrl(string url)
        //{
        //    Bitmap imageBitmap = null;

        //    using (var webClient = new WebClient())
        //    {
        //        var imageBytes = webClient.DownloadData(url);
        //        if (imageBytes != null && imageBytes.Length > 0)
        //        {
        //            imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
        //        }
        //    }

        //    return imageBitmap;
        //}


    }
}