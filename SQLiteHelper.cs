
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Database;
using Android.Database.Sqlite;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util;

namespace App7
{
    [Activity(Label = "SQLiteHelper")]
    public class SQLiteHelper : SQLiteOpenHelper
    {
        private static String DATABASE_NAME = "PlayerManagement.db";

        public String PLAYER_ID = "id";
        public String PLAYER_NAME = "name";
        public String PLAYER_NUMBER = "number";
        public String PLAYER_PHOTO = "photo";
        public String GAME_PLAYED = "game";
        public String POINT_PER_GAME = "point";
        public String GOAL_PER_GAME = "goal";
        public String TOTAL_STAT = "stat";
        public String SCORE = "score";
        //constructor to create database


        public SQLiteHelper(Context context)
            : base(context, DATABASE_NAME, null, 2)
        {

        }

        public override void OnCreate(SQLiteDatabase db)
        {
            db.ExecSQL(
                "create table Players " +
                "(id integer primary key, name text, number text,photo text,game text,point text, goal text, stat text, score text)"
                );
        }

        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
        {
            db.ExecSQL("DROP TABLE IF EXISTS Players");
            OnCreate(db);
        }

        // insert data

        public bool insertPlayer(String name, String number,  String photo, String game, String point, String goal, String stat, String score)
        {
            SQLiteDatabase db = this.WritableDatabase;
            ContentValues contentValues = new ContentValues();
            contentValues.Put("name", name);
            contentValues.Put("number", number);
            contentValues.Put("photo", photo);
            contentValues.Put("game", game);
            contentValues.Put("point", point);
            contentValues.Put("goal", goal);
            contentValues.Put("stat", stat);
            contentValues.Put("score", score);

            db.Insert("Players", null, contentValues);
            return true;
        }


        // get complete data/info

        public System.Collections.ArrayList getAllPlayersData()
        {
            System.Collections.ArrayList array_list = new System.Collections.ArrayList();

            //hp = new HashMap();
            SQLiteDatabase db = this.ReadableDatabase;
            ICursor res = db.RawQuery("select * from Players", null);
            res.MoveToFirst();

            while (res.IsAfterLast == false)
            {
                array_list.Add(res.GetString(res.GetColumnIndex(PLAYER_NAME)));
                res.MoveToNext();
            }
            return array_list;
        }

        // get single entry
        public ICursor getSingleEntryByNumber(string number)
        {
            SQLiteDatabase db = this.ReadableDatabase;
            ICursor res = db.RawQuery("select * from Players where number=" + number + "", null);
            return res;
        }
        public ICursor getSingleEntry(int id)
        {
            SQLiteDatabase db = this.ReadableDatabase;
            ICursor res = db.RawQuery("select * from Players where id=" + id + "", null);
            return res;
        }
        public ICursor getSingleEntryByName(string name)
        {
            SQLiteDatabase db = this.ReadableDatabase;
            ICursor res = db.RawQuery("select * from Players where name=\"" + name + "\"", null);
            return res;
        }
        // delete entry
        public int deletePlayer(int number)
        {
            SQLiteDatabase db = this.WritableDatabase;
            return db.Delete("Players",
            "number = ? ", new String[] { Convert.ToString(number) });
        }
        // update entry
        public bool updatePlayerInfo(String name, String number, String photo, String game, String point, String goal, String stat, String score)
        {
            SQLiteDatabase db = this.WritableDatabase;
            ContentValues contentValues = new ContentValues();
            contentValues.Put("name", name);
            contentValues.Put("number", number);
            contentValues.Put("photo", photo);
            contentValues.Put("game", game);
            contentValues.Put("point", point);
            contentValues.Put("goal", goal);
            contentValues.Put("stat", stat);
            contentValues.Put("score", score);


            db.Update("Players", contentValues, "number = ? ", new String[] { Convert.ToString(number) });
            return true;
        }
    }
}

