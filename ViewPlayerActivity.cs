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
using System.IO;
using Android.Database.Sqlite;
using Android.Database;

namespace App7
{
    [Activity(Label = "ViewPlayerActivity")]
    public class ViewPlayerActivity : Activity
    {
        SQLiteHelper dbHelper;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.ListOfPlayersLayout);

            Button Done = FindViewById<Button>(Resource.Id.btn_list_player_done);
            ListView PlayerListView = FindViewById<ListView>(Resource.Id.list_player_list);

            dbHelper = new SQLiteHelper(this);
            System.Collections.ArrayList dataList = dbHelper.getAllPlayersData();
            
            String[] myArr = (String[])dataList.ToArray(typeof(string));
            PlayerListView.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleExpandableListItem1, myArr);
            
            PlayerListView.ItemClick += PlayerListView_ItemClick;
            Done.Click += Done_Click;
        }

        private void PlayerListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            // find stat based on e.position
            int click_Player = e.Position + 1;
            Toast.MakeText(this, e.Position.ToString(), ToastLength.Short).Show();
            ICursor c = dbHelper.getSingleEntry(click_Player);
            c.MoveToFirst();
            string name = c.GetString(c.GetColumnIndex(dbHelper.PLAYER_NAME));
            string number = c.GetString(c.GetColumnIndex(dbHelper.PLAYER_NUMBER));
            string photo = c.GetString(c.GetColumnIndex(dbHelper.PLAYER_PHOTO));
            string game = c.GetString(c.GetColumnIndex(dbHelper.GAME_PLAYED));
            string goal = c.GetString(c.GetColumnIndex(dbHelper.GOAL_PER_GAME));
            string point = c.GetString(c.GetColumnIndex(dbHelper.POINT_PER_GAME));
            string stat = c.GetString(c.GetColumnIndex(dbHelper.TOTAL_STAT));
            string score = c.GetString(c.GetColumnIndex(dbHelper.SCORE));
            
            c.Close();

            var player_stat_activity = new Intent(this, typeof(ViewPlayerStatActivity));
            player_stat_activity.PutExtra("Name", name);
            player_stat_activity.PutExtra("Number", number);
            player_stat_activity.PutExtra("Photo", photo);
            player_stat_activity.PutExtra("Game", game);
            player_stat_activity.PutExtra("Point", point);
            player_stat_activity.PutExtra("Goal", goal);
            player_stat_activity.PutExtra("Stat", stat);
            player_stat_activity.PutExtra("Score", score);
            StartActivity(player_stat_activity);

            Finish();

        }

        private void Done_Click(object sender, EventArgs e)
        {
            var home_activity = new Intent(this, typeof(HomePageActivity));
            StartActivity(home_activity);
            Finish();
        }
    }
}