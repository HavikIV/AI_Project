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
    [Activity(Theme = "@android:style/Theme.Material.Light", Label = "GridActivity", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class GridActivity : Activity
    {
        // Global Variables
        Button startButton;
        Button nextButton;
        RelativeLayout.LayoutParams startButtonParamsPortrait;
        RelativeLayout.LayoutParams startButtonParamsLandscape;
        RelativeLayout.LayoutParams nextButtonParamsPortrait;
        RelativeLayout.LayoutParams nextButtonParamsLandscape;
        TextView[,] labels;

        //int count = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            // Don't want the action bar to show, find it unnecessary, so gonna hide it.
            ActionBar.Hide();

            base.OnCreate(savedInstanceState);

            // Create your application here

            // Create the array of labels
            labels = new TextView[9,9];
            // Populate the array
            for (int i=0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    labels[i,j] = new TextView(this);
                    labels[i,j].Text = "";
                    // Give each of the labels a unique ID; range 4-85
                    labels[i,j].Id = (4 + (9 * i)) + j;
                    labels[i, j].Text = "" + labels[i, j].Id;
                    // Assign its parameters here?
                    var textParams = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                    if (i == 0 && j == 0)
                    {
                        textParams.AddRule(LayoutRules.AlignParentTop);
                    }
                    else if (i == 0 && j != 0)
                    {
                        textParams.AddRule(LayoutRules.AlignParentTop);
                        textParams.AddRule(LayoutRules.RightOf, labels[i, j - 1].Id);
                    }
                    else if (i != 0)
                    {
                        textParams.AddRule(LayoutRules.Below, labels[i - 1, j].Id);
                        if (j != 0)
                        {
                            textParams.AddRule(LayoutRules.RightOf, labels[i, j - 1].Id);
                        }
                    }

                    labels[i, j].LayoutParameters = textParams;
                    labels[i, j].TextSize = 20;
                    labels[i, j].SetPadding(50, 50, 0, 0);
                }
            }
            //// Create a test label for displaying
            //TextView testLabel = new TextView(this);
            //testLabel.Text = "";
            //testLabel.Id = 4;
            ////text2
            //TextView testLabel2 = new TextView(this);
            //testLabel2.Text = "9";
            //testLabel2.Id = 5;

            // get the initial orientation
            var surfaceOrientation = WindowManager.DefaultDisplay.Rotation;

            RelativeLayout layoutBase = new RelativeLayout(this);
            layoutBase.LayoutParameters = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            layoutBase.SetPadding(100, 100, 100, 100);

            // Adding a Imageview to display the sudoku grid
            ImageView grid = new ImageView(this);
            grid.LayoutParameters = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            grid.Visibility = ViewStates.Visible;
            grid.SetBackgroundResource(Resource.Drawable.SudokuGrid);
            grid.Id = 1;
            layoutBase.AddView(grid);

            // Adding a button that will be used to step through the "AI"'s solution
            nextButton = new Button(this) { Text = "Next" };
            nextButton.Id = 2;

            // Adding a button that will be used to start the "AI" to solve the puzzle
            startButton = new Button(this) { Text = "Start" };
            startButton.Id = 3;

            // Layout Parameters for Portrait mode
            // nextButton
            nextButtonParamsPortrait = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            nextButtonParamsPortrait.AddRule(LayoutRules.AlignParentBottom);
            nextButtonParamsPortrait.AddRule(LayoutRules.AlignParentRight);
            // startButton
            startButtonParamsPortrait = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            startButtonParamsPortrait.AddRule(LayoutRules.AlignParentBottom);
            startButtonParamsPortrait.AddRule(LayoutRules.LeftOf, nextButton.Id);

            // Layout Parameters for Landscape mode
            // startButton
            startButtonParamsLandscape = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            startButtonParamsLandscape.AddRule(LayoutRules.AlignParentRight);
            // nextButton
            nextButtonParamsLandscape = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            nextButtonParamsLandscape.AddRule(LayoutRules.AlignParentRight);
            nextButtonParamsLandscape.AddRule(LayoutRules.Below, startButton.Id);

            // Add labels in the location of the squares 

            // Depending on the initial orientation, the buttons will placed at different locations
            if (surfaceOrientation == SurfaceOrientation.Rotation0 || surfaceOrientation == SurfaceOrientation.Rotation180)
            {
                // The screen is in Portrait mode
                startButton.LayoutParameters = startButtonParamsPortrait;
                nextButton.LayoutParameters = nextButtonParamsPortrait;
            }
            else
            {
                // The screen is in Landscape mode
                startButton.LayoutParameters = startButtonParamsLandscape;
                nextButton.LayoutParameters = nextButtonParamsLandscape;
            }

            ////RelativeLayout.LayoutParams textParams;
            //var textParams = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            //textParams.AddRule(LayoutRules.AlignParentTop);
            //testLabel.LayoutParameters = textParams;
            //testLabel.TextSize = 20;
            //testLabel.SetPadding(50, 50, 0, 0);
            ////text2
            //var textParams2 = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            //textParams2.AddRule(LayoutRules.AlignParentTop);
            //textParams2.AddRule(LayoutRules.RightOf, 4);
            //testLabel2.LayoutParameters = textParams2;
            //testLabel2.TextSize = 20;
            //testLabel2.SetPadding(100, 50, 0, 0);

            //// test click option for textview
            //testLabel.Click += (ssender, e) =>
            //{
            //    testLabel.Text = "" + count++;
            //};

            // Add the buttons to the layout
            layoutBase.AddView(startButton);
            layoutBase.AddView(nextButton);
            //layoutBase.AddView(testLabel);
            //layoutBase.AddView(testLabel2);
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    layoutBase.AddView(labels[i, j]);
                }
            }

            // Display the layout to the screen
            SetContentView(layoutBase);            
        }

        public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);

            if (newConfig.Orientation == Android.Content.Res.Orientation.Portrait)
            {
                startButton.LayoutParameters = startButtonParamsPortrait;
                nextButton.LayoutParameters = nextButtonParamsPortrait;
            }
            else if (newConfig.Orientation == Android.Content.Res.Orientation.Landscape)
            {
                startButton.LayoutParameters = startButtonParamsLandscape;
                nextButton.LayoutParameters = nextButtonParamsLandscape;
            }
        }
    }
}