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
    [Activity(Label = "HomePageActivity")]
    public class HomePageActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.HomePageLayout);

            Button AddPlayer = FindViewById<Button>(Resource.Id.btn_add_player);
            Button ViewPlayers = FindViewById<Button>(Resource.Id.btn_view_players);
            Button PlayGame = FindViewById<Button>(Resource.Id.btn_play_game);
            Button StartLeague = FindViewById<Button>(Resource.Id.btn_start_league);
            Button exit = FindViewById<Button>(Resource.Id.btn_exit);

            AddPlayer.Click += AddPlayer_Click;
            ViewPlayers.Click += ViewPlayers_Click;
            PlayGame.Click += PlayGame_Click;
            StartLeague.Click += StartLeague_Click;
            exit.Click += Exit_Click;
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Finish();
            Java.Lang.JavaSystem.Exit(0);
        }

        private void StartLeague_Click(object sender, EventArgs e)
        {
            // play league
            var player_selection_activity = new Intent(this, typeof(PlayerSelectionActivity));
            player_selection_activity.PutExtra("limitation", false);
            StartActivity(player_selection_activity);
        }

        private void PlayGame_Click(object sender, EventArgs e)
        {
            // play game
            var player_selection_activity = new Intent(this, typeof(PlayerSelectionActivity));
            player_selection_activity.PutExtra("limitation", true);
            StartActivity(player_selection_activity);
        }

        private void ViewPlayers_Click(object sender, EventArgs e)
        {
            var view_player_activity = new Intent(this, typeof(ViewPlayerActivity));
            StartActivity(view_player_activity);
        }

        private void AddPlayer_Click(object sender, EventArgs e)
        {
            var add_player_activity = new Intent(this, typeof(AddPlayerActivity));
            StartActivity(add_player_activity);
        }
    }
}