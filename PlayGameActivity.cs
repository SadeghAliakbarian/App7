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
    [Activity(Label = "PlayGameActivity")]
    public class PlayGameActivity : Activity
    {
        int max_fixtures = 0;
        int NoPlayers = 0;
        int current_fixture = 0;
        string[] P_names;
        Player[] ActualPlayers;
        public struct LeagueTableComponent
        {
            public string name;
            public int GA, win, draw, lose, point;
        }

        string HomePlayer = "", AwayPlayer = "";
        bool[] played;
        bool[] playerUpdated;
        string[] HomeGames;
        string[] AwayGames;
        int[] homeGoals;
        int[] awayGoals;
        bool start = false;
        SQLiteHelper dbHelper;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.GamesAndTable);
            dbHelper = new SQLiteHelper(this);
            TextView Fixture = FindViewById<TextView>(Resource.Id.txt_Fixture);
            TextView Home = FindViewById<TextView>(Resource.Id.txt_HomeName);
            TextView Away = FindViewById<TextView>(Resource.Id.txt_AwayName);
            EditText HomeScore = FindViewById<EditText>(Resource.Id.edit_HomeScore);
            EditText AwayScore = FindViewById<EditText>(Resource.Id.edit_AwayScore);
            Button ResultConfirm = FindViewById<Button>(Resource.Id.btn_ResultConfirm);
            Button GoHome = FindViewById<Button>(Resource.Id.btn_table_goHome);
            GoHome.Enabled = false;

            int [] participants = Intent.GetIntArrayExtra("participant");
            bool single = Intent.GetBooleanExtra("one_leg", false);

            if (single) { max_fixtures = FixtureSize(true, participants.Length); }
            if (!single) { max_fixtures = FixtureSize(false, participants.Length); }

            NoPlayers = participants.Length;

            P_names = new string[NoPlayers];
            for (int i = 0; i < participants.Length; i++)
            {
                Android.Database.ICursor c = dbHelper.getSingleEntry(participants[i]+1);
                c.MoveToFirst();
                string name = c.GetString(c.GetColumnIndex(dbHelper.PLAYER_NAME));
                string number = c.GetString(c.GetColumnIndex(dbHelper.PLAYER_NUMBER));
                P_names[i] = name + "("+number+")";
                c.Close();
            }

            played = new bool[max_fixtures]; for (int i = 0; i < max_fixtures; i++) played[i] = false;
            playerUpdated = new bool[max_fixtures]; for (int i = 0; i < max_fixtures; i++) playerUpdated[i] = false;
            HomeGames = new string[max_fixtures]; homeGoals = new int[max_fixtures];
            AwayGames = new string[max_fixtures]; awayGoals = new int[max_fixtures];

            if (single)
            {
                int k = 0;
                for (int i = 0; i < NoPlayers - 1; i++)
                {
                    for (int j = i + 1; j < NoPlayers; j++)
                    {
                        HomeGames[k] = P_names[i];
                        AwayGames[k] = P_names[j];
                        k++;
                    }
                }
            }

            if (!single)
            {
                int k = 0;
                for (int i = 0; i < NoPlayers - 1; i++)
                {
                    for (int j = i + 1; j < NoPlayers; j++)
                    {
                        HomeGames[k] = P_names[i];
                        AwayGames[k] = P_names[j];
                        k++;
                    }
                }
                for (int i = 0; i < NoPlayers - 1; i++)
                {
                    for (int j = i + 1; j < NoPlayers; j++)
                    {
                        AwayGames[k] = P_names[i];
                        HomeGames[k] = P_names[j];
                        k++;
                    }
                }

            }
            HomeScore.Enabled = false;
            AwayScore.Enabled = false;

            GoHome.Click += GoHome_Click;

            ResultConfirm.Click += delegate
            {
                if(!played.Contains(false))
                {
                    GoHome.Enabled = true;
                }
                if (true)
                {
                    if (start == false)
                    {
                        ResultConfirm.Text = "Confirm & Next"; start = true;
                        HomeScore.Enabled = true;
                        AwayScore.Enabled = true;
                        HomePlayer = HomeGames[current_fixture]; AwayPlayer = AwayGames[current_fixture];
                        Fixture.Text = "Games In Order (" + (current_fixture + 1).ToString() + "/" + max_fixtures.ToString() + ")";
                        Home.Text = "Home: " + HomeGames[current_fixture];
                        Away.Text = "Away: " + AwayGames[current_fixture];
                        return;
                    }

                    if (HomeScore.Text.ToString() != "" && AwayScore.Text.ToString() != "")
                    {
                        if (played[current_fixture] == false)
                        {
                            HomeScore.Enabled = true;
                            AwayScore.Enabled = true;
                            played[current_fixture] = true;
                           
                            homeGoals[current_fixture] = int.Parse(HomeScore.Text.ToString());
                            awayGoals[current_fixture] = int.Parse(AwayScore.Text.ToString());
                            if (!playerUpdated[current_fixture])
                            {
                                //update player profile [HOME]
                                {
                                    Android.Database.ICursor c = dbHelper.getSingleEntryByName(HomeGames[current_fixture].Split('(')[0]);
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

                                    game = (Int32.Parse(game) + 1).ToString();
                                    if (homeGoals[current_fixture] > awayGoals[current_fixture])
                                    {
                                        point = ((double.Parse(point) + 3) / (Int32.Parse(game))).ToString();
                                        goal = ((double.Parse(goal) + homeGoals[current_fixture]) / (Int32.Parse(game))).ToString();
                                        stat = (Int32.Parse(stat.Split(' ')[0]) + 1).ToString() + " " + stat.Split(' ')[1] + " " + stat.Split(' ')[2];
                                        score = (double.Parse(point) * 33.333).ToString();
                                    }
                                    if (homeGoals[current_fixture] < awayGoals[current_fixture])
                                    {
                                        point = ((double.Parse(point)) / (Int32.Parse(game))).ToString();
                                        goal = ((double.Parse(goal) + homeGoals[current_fixture]) / (Int32.Parse(game))).ToString();
                                        stat = stat.Split(' ')[0] + " " + stat.Split(' ')[1] + " " + (Int32.Parse(stat.Split(' ')[2]) + 1).ToString();
                                        score = (double.Parse(point) * 33.333).ToString();
                                    }
                                    if (homeGoals[current_fixture] == awayGoals[current_fixture])
                                    {
                                        point = ((double.Parse(point) + 1) / (Int32.Parse(game))).ToString();
                                        goal = ((double.Parse(goal) + homeGoals[current_fixture]) / (Int32.Parse(game))).ToString();
                                        stat = stat.Split(' ')[0] + " " + (Int32.Parse(stat.Split(' ')[1]) + 1).ToString() + " " + stat.Split(' ')[2];
                                        score = (double.Parse(point) * 33.333).ToString();
                                    }

                                    dbHelper.updatePlayerInfo(name, number, photo, game, point, goal, stat, score);
                                }
                                {
                                    Android.Database.ICursor c = dbHelper.getSingleEntryByName(AwayGames[current_fixture].Split('(')[0]);
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

                                    game = (Int32.Parse(game) + 1).ToString();
                                    if (homeGoals[current_fixture] < awayGoals[current_fixture])
                                    {
                                        point = ((double.Parse(point) + 3) / (Int32.Parse(game))).ToString();
                                        goal = ((double.Parse(goal) + awayGoals[current_fixture]) / (Int32.Parse(game))).ToString();
                                        stat = (Int32.Parse(stat.Split(' ')[0]) + 1).ToString() + " " + stat.Split(' ')[1] + " " + stat.Split(' ')[2];
                                        score = (double.Parse(point) * 33.333).ToString();
                                    }
                                    if (homeGoals[current_fixture] > awayGoals[current_fixture])
                                    {
                                        point = ((double.Parse(point)) / (Int32.Parse(game))).ToString();
                                        goal = ((double.Parse(goal) + awayGoals[current_fixture]) / (Int32.Parse(game))).ToString();
                                        stat = stat.Split(' ')[0] + " " + stat.Split(' ')[1] + " " + (Int32.Parse(stat.Split(' ')[2]) + 1).ToString();
                                        score = (double.Parse(point) * 33.333).ToString();
                                    }
                                    if (homeGoals[current_fixture] == awayGoals[current_fixture])
                                    {
                                        point = ((double.Parse(point) + 1) / (Int32.Parse(game))).ToString();
                                        goal = ((double.Parse(goal) + awayGoals[current_fixture]) / (Int32.Parse(game))).ToString();
                                        stat = stat.Split(' ')[0] + " " + (Int32.Parse(stat.Split(' ')[1]) + 1).ToString() + " " + stat.Split(' ')[2];
                                        score = (double.Parse(point)*33.333).ToString();
                                    }

                                    dbHelper.updatePlayerInfo(name, number, photo, game, point, goal, stat, score);
                                }


                                playerUpdated[current_fixture] = true;
                            }
                            HomeScore.Text = ""; AwayScore.Text = "";
                        }
                        else
                        {
                            HomeScore.Enabled = false;
                            AwayScore.Enabled = false;

                            HomeScore.Text = homeGoals[current_fixture].ToString(); AwayScore.Text = awayGoals[current_fixture].ToString();

                        }
                    }
                    current_fixture++;
                    if (current_fixture >= max_fixtures) { current_fixture = 0; }
                    Fixture.Text = "Games In Order (" + (current_fixture + 1).ToString() + "/" + max_fixtures.ToString() + ")";
                    Home.Text = "Home: " + HomeGames[current_fixture];
                    Away.Text = "Away: " + AwayGames[current_fixture];
                    if (played[current_fixture])
                    {
                        HomeScore.Enabled = false;
                        AwayScore.Enabled = false;

                        HomeScore.Text = homeGoals[current_fixture].ToString(); AwayScore.Text = awayGoals[current_fixture].ToString();
                    }

                    //update the table
                    //if (!played.Contains(false))
                    if (true)
                    {
                        LeagueTableComponent[] currentTable = new LeagueTableComponent[NoPlayers];
                        for (int i = 0; i < NoPlayers; i++)
                        {
                            currentTable[i].name = P_names[i];
                            currentTable[i].point = currentTable[i].GA = currentTable[i].win = currentTable[i].draw = currentTable[i].lose = 0;
                        }
                        for (int fixt = 0; fixt < max_fixtures; fixt++)
                        {
                            if (played[fixt])
                                for (int i = 0; i < NoPlayers; i++)
                                {
                                    // update home
                                    if (HomeGames[fixt] == currentTable[i].name)
                                    {
                                        if (homeGoals[fixt] > awayGoals[fixt])
                                        {
                                            currentTable[i].win++;
                                            currentTable[i].point += 3;
                                        }
                                        if (homeGoals[fixt] < awayGoals[fixt])
                                        {
                                            currentTable[i].lose++;
                                        }
                                        if (homeGoals[fixt] == awayGoals[fixt])
                                        {
                                            currentTable[i].draw++;
                                            currentTable[i].point++;
                                        }
                                        currentTable[i].GA += (homeGoals[fixt] - awayGoals[fixt]);
                                    }
                                    // update away
                                    if (AwayGames[fixt] == currentTable[i].name)
                                    {
                                        if (awayGoals[fixt] > homeGoals[fixt])
                                        {
                                            currentTable[i].win++;
                                            currentTable[i].point += 3;
                                        }
                                        if (awayGoals[fixt] < homeGoals[fixt])
                                        {
                                            currentTable[i].lose++;
                                        }
                                        if (awayGoals[fixt] == homeGoals[fixt])
                                        {
                                            currentTable[i].draw++;
                                            currentTable[i].point++;
                                        }
                                        currentTable[i].GA += (awayGoals[fixt] - homeGoals[fixt]);
                                    }
                                }

                            // sort the table
                            Array.Sort<LeagueTableComponent>(currentTable, (x, y) => y.point.CompareTo(x.point));
                            //LeagueTable.Text = "";
                            List<LeagueStats> RankingTable = new List<LeagueStats>();
                            LeagueStats temp = new LeagueStats();
                            temp.name = "Team";
                            temp.win = "Win";
                            temp.draw = "Draw";
                            temp.lose = "Lose";
                            temp.point = "Points";
                            temp.GA = "GA";

                            RankingTable.Add(temp);
                            for (int i = 0; i < NoPlayers; i++)
                            {
                                temp = new LeagueStats();
                                temp.name = currentTable[i].name;
                                temp.win = currentTable[i].win.ToString();
                                temp.draw = currentTable[i].draw.ToString();
                                temp.lose = currentTable[i].lose.ToString();
                                temp.point = currentTable[i].point.ToString();
                                temp.GA = currentTable[i].GA.ToString();

                                RankingTable.Add(temp);
                            }
                            ListView rankingList = FindViewById<ListView>(Resource.Id.list_table);
                            RankingListView adapter = new RankingListView(this, RankingTable);
                            //ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, alaki);
                            rankingList.Adapter = adapter;


                        }
                    }
                }

            };

        }

        private void GoHome_Click(object sender, EventArgs e)
        {
            var home_activity = new Intent(this, typeof(HomePageActivity));
            StartActivity(home_activity);
            Finish();
        }

        private int FixtureSize(bool single, int Size)
        {
            int size = 0;
            if (single)
            {
                size = Factorial(Size) / (2 * Factorial(Size - 2));
            }
            else
            { size = Factorial(Size) / (Factorial(Size - 2)); }
            return size;
        }

        private int Factorial(int i)
        {
            if (i <= 1)
                return 1;
            return i * Factorial(i - 1);
        }

    }
}