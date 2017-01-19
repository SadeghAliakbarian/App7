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
    class Player
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public string PhotoLocation { get; set; }

        public int GamePlayed { get; set; }
        public double PointPerGame { get; set; }
        public double GoalPerGame { get; set; }
        public int[] Stats { get; set; }
        public int TotalScore { get; set; }
    }
}