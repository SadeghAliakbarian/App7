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
using Android.Database.Sqlite;


namespace App7
{
    [Activity(Label = "PlayerSelectionActivity")]
    public class PlayerSelectionActivity : Activity
    {
        int capacity = 0;
        SQLiteHelper dbHelper;
        ListView PlayerList; Button Next;
        bool limitation; String[] myArr;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PlayGamePlayers);

            Next = FindViewById<Button>(Resource.Id.btn_go_to_game);
            PlayerList = FindViewById<ListView>(Resource.Id.list_select_game_players);
            //Next.Enabled = false;
            limitation = Intent.GetBooleanExtra("limitation", false);
            if (limitation) { capacity = 2; }

            dbHelper = new SQLiteHelper(this);
            System.Collections.ArrayList dataList = dbHelper.getAllPlayersData();

            
            myArr = (String[])dataList.ToArray(typeof(string));
            // SimpleExpandableListItem1
            PlayerList.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItemChecked, myArr);
            PlayerList.ChoiceMode = ChoiceMode.Multiple;

            Next.Click += Next_Click;
            
        }
        List<int> participants;
        private void Next_Click(object sender, EventArgs e)
        {
            RadioButton one_leg = FindViewById<RadioButton>(Resource.Id.radio_one_leg);
            RadioButton two_leg = FindViewById<RadioButton>(Resource.Id.radio_two_leg);
            //int count = PlayerList.GetCheckedItemIds().Length;
            int count = PlayerList.GetCheckItemIds().Length;
            long[] selectedPlayers = PlayerList.GetCheckItemIds();
            if (limitation)
            {
                if (count == capacity)
                {
                    Toast.MakeText(this, "Successfully selected players", ToastLength.Short).Show();
                    participants = new List<int>();
                    int[] participantID = new int[count];
                    for (int i = 0; i < count; i++)
                    {
                        //Android.Database.ICursor c = dbHelper.getSingleEntry((int)selectedPlayers[i]);
                        participantID[i] = ((int)selectedPlayers[i]);
                    }
                    //send participant to the next page
                    var play_game_activity = new Intent(this, typeof(PlayGameActivity));
                    play_game_activity.PutExtra("participant", participantID);
                    play_game_activity.PutExtra("one_leg", one_leg.Checked);
                    StartActivity(play_game_activity);
                }
                else
                    Toast.MakeText(this, "You should select only two players", ToastLength.Short).Show();
            }
            else
            {
                if (count > 0)
                {
                    Toast.MakeText(this, "Successfully selected players", ToastLength.Short).Show();
                    participants = new List<int>();
                    int[] participantID = new int[count];
                    for (int i = 0; i < count; i++)
                    {
                        //Android.Database.ICursor c = dbHelper.getSingleEntry((int)selectedPlayers[i]);
                        participantID[i] = ((int)selectedPlayers[i]);
                    }
                    //send participant to the next page
                    var play_game_activity = new Intent(this, typeof(PlayGameActivity));
                    play_game_activity.PutExtra("participant", participantID);
                    play_game_activity.PutExtra("one_leg", one_leg.Checked);
                    StartActivity(play_game_activity);
                }
                else
                    Toast.MakeText(this, "You should select at least two players", ToastLength.Short).Show();
            }
        }
        
    }
}