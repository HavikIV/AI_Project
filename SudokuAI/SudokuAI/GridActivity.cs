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

namespace SudokuAI
{
    [Activity(Label = "GridActivity")]
    public class GridActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            RelativeLayout layoutBase = new RelativeLayout(this);
            layoutBase.LayoutParameters = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            ImageView grid = new ImageView(this);
            grid.LayoutParameters = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            grid.Visibility = ViewStates.Visible;
            grid.SetBackgroundResource(Resource.Drawable.SudokuGrid);
            layoutBase.AddView(grid);
            SetContentView(layoutBase);
            
        }
    }
}