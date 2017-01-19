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
    class RankingListView : BaseAdapter<LeagueStats>
    {
        private List<LeagueStats> mItem;
        private Context mContext;

        public RankingListView(Context context, List<LeagueStats> item)
        {
            mContext = context;
            mItem = item;
        }
        public override int Count
        {
            get
            {
                return mItem.Count;
            }
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override LeagueStats this[int position]
        {
            get
            {
                return mItem[position];
            }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if (row == null)
            {
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.MyCustomListView, null, false);
            }
            TextView txtName = row.FindViewById<TextView>(Resource.Id.txtName);
            TextView txtWin = row.FindViewById<TextView>(Resource.Id.txtWin);
            TextView txtDraw = row.FindViewById<TextView>(Resource.Id.txtDraw);
            TextView txtLose = row.FindViewById<TextView>(Resource.Id.txtLose);
            TextView txtPoint = row.FindViewById<TextView>(Resource.Id.txtPoint);
            TextView txtGA = row.FindViewById<TextView>(Resource.Id.txtGA);
            txtName.Text = mItem[position].name;
            txtWin.Text = mItem[position].win.ToString();
            txtDraw.Text = mItem[position].draw.ToString();
            txtLose.Text = mItem[position].lose.ToString();
            txtPoint.Text = mItem[position].point.ToString();
            txtGA.Text = mItem[position].GA.ToString();
            return row;
        }
    }
}