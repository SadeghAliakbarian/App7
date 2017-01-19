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
    [Activity(Label = "ViewPlayerStatActivity")]
    public class ViewPlayerStatActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PlayerStatLayout);
            // Create your application here
            TextView PlayerName = FindViewById<TextView>(Resource.Id.txt_view_player_name);
            TextView PlayerID = FindViewById<TextView>(Resource.Id.txt_view_player_id);

            TextView PlayerGame = FindViewById<TextView>(Resource.Id.txt_view_player_games);
            TextView PlayerPoint = FindViewById<TextView>(Resource.Id.txt_view_player_point);
            TextView PlayerGoal = FindViewById<TextView>(Resource.Id.txt_view_player_goal);
            TextView PlayerStat = FindViewById<TextView>(Resource.Id.txt_view_player_stat);
            RatingBar rate = FindViewById<RatingBar>(Resource.Id.rating_view_player_score);
            rate.Enabled = false;
            rate.Max = 100;
            Button Done = FindViewById<Button>(Resource.Id.btn_view_player_done);

            string Name = Intent.GetStringExtra("Name");
            string Number = Intent.GetStringExtra("Number");
            string Photo = Intent.GetStringExtra("Photo");
            string Game = Intent.GetStringExtra("Game");
            string Goal = Intent.GetStringExtra("Goal");
            string Point = Intent.GetStringExtra("Point");
            string Stat = Intent.GetStringExtra("Stat");
            string Score = Intent.GetStringExtra("Score");

            PlayerName.Text = Name;
            PlayerID.Text = Number;
            PlayerGame.Text = "Game Played: " + Game;
            PlayerGoal.Text = "Goal-Per-Game: " + Goal;
            PlayerPoint.Text = "Point-Per-Game: " + Point;
            PlayerStat.Text = "Total Win/Draw/Lose: " + Stat;
            rate.Rating = float.Parse(Score);
            Done.Click += Done_Click;
        }

        private void Done_Click(object sender, EventArgs e)
        {
            var home_activity = new Intent(this, typeof(HomePageActivity));
            StartActivity(home_activity);
            Finish();
        }
    }
}