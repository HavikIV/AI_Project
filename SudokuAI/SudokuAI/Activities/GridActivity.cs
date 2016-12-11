using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        //Button startButton;
        //Button nextButton;
        //Button restart;
        //RelativeLayout.LayoutParams startButtonParamsPortrait;
        //RelativeLayout.LayoutParams startButtonParamsLandscape;
        //RelativeLayout.LayoutParams nextButtonParamsPortrait;
        //RelativeLayout.LayoutParams nextButtonParamsLandscape;
        //RelativeLayout.LayoutParams restartParamsPortrait;
        //RelativeLayout.LayoutParams restartParamsLandscape;
        private TextView[,] labels;
        private bool waitingOnNext = true;
        //TextView testLabel;
        //RelativeLayout.LayoutParams nestLayoutParams;
        //byte[,] hintGird;

        //int count = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            // Don't want the action bar to show, find it unnecessary, so gonna hide it.
            ActionBar.Hide();

            base.OnCreate(savedInstanceState);

            // Create your application here

            /**
             * Once the APP has been finished to the point that it can solve the given Sudoku puzzle,
             * come back to make better looking labels. At the time of writing this, I suggest hard coding
             * the height and width of the labels, so they will match with the squares in the grid image.
             * This will also make it easier for the APP know when a label is clicked and it won't need all 
             * of the paddings, just the alignments should be enough. Don't forget to center the text to the 
             * middle of the label. This should also make the OnConfigurationChanged() smaller and tidier.
             **/
            // Create the array of labels
            labels = new TextView[9,9];
            // Populate the array
            for (int i=0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    // Create a ColorStateList that will be used to changed the colors of the labels for each of its different states
                    //int[][] states = new int[][]
                    //{
                    //    new int[] { Android.Resource.Attribute.StatePressed },  // pressed
                    //    new int[] { Android.Resource.Attribute.StateFocused }, // focused
                    //    new int[] { Android.Resource.Attribute.StateEnabled },   // enabled
                    //    new int[] { -Android.Resource.Attribute.StateEnabled },    // disabled
                    //    new int[] { -Android.Resource.Attribute.StateFocused }  // unfocused
                    //};

                    //int[] colors = new int[]
                    //{
                    //    Android.Resource.Color.Black, // green
                    //    Android.Resource.Color.HoloBlueDark, // blue
                    //    Android.Resource.Color.HoloRedDark,  // red
                    //    Android.Resource.Color.HoloPurple,   // purple
                    //    Android.Resource.Color.Black   // orange
                    //};

                    //Android.Content.Res.ColorStateList list = new Android.Content.Res.ColorStateList(states, colors);

                    labels[i,j] = new TextView(this);
                    //labels[i, j].SetTextColor(Android.Graphics.Color.Blue);
                    labels[i,j].Text = "";
                    // Give each of the labels a unique ID; range 4-84
                    labels[i,j].Id = (4 + (9 * i)) + j;
                    //labels[i, j].Text = "" + labels[i, j].Id;
                    labels[i, j].Text = "";
                    labels[i, j].TextSize = 20;

                    //labels[i, j].TextAlignment = TextAlignment.Center;
                    //labels[i, j].SetHeight(180);
                    //labels[i, j].SetWidth(138);
                    //labels[i, j].SetPadding(55, 45, 0, 0);

                    // Assign its parameters here?
                    var textParams = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                    if (i == 0 && j == 0)
                    {
                        textParams.AddRule(LayoutRules.AlignParentTop);
                        //labels[i, j].SetPadding(60, 50, 0, 0);
                    }
                    else if (i == 0 && j != 0)
                    {
                        textParams.AddRule(LayoutRules.AlignParentTop);
                        textParams.AddRule(LayoutRules.RightOf, labels[i, j - 1].Id);
                        //if (j < 3 || j > 5)
                        //{
                        //    labels[i, j].SetPadding(100, 50, 0, 0);
                        //}
                        //else
                        //{
                        //    labels[i, j].SetPadding(120, 50, 0, 0);
                        //}
                    }
                    else if (i != 0)
                    {
                        textParams.AddRule(LayoutRules.Below, labels[i - 1, j].Id);
                        //labels[i, j].SetPadding(60, 100, 0, 0);
                        if (j != 0)
                        {
                            textParams.AddRule(LayoutRules.RightOf, labels[i, j - 1].Id);
                            //if (j < 3 || j < 5)
                            //{
                            //    labels[i, j].SetPadding(100, 100, 0, 0);
                            //}
                            //else
                            //{
                            //    labels[i, j].SetPadding(120, 100, 0, 0);
                            //}
                        }
                    }
                    labels[i, j].LayoutParameters = textParams;

                    // Add a Click event for each label
                    labels[i, j].Click += label_Click;
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
            layoutBase.Id = 98;
            layoutBase.LayoutParameters = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            layoutBase.SetPadding(100, 100, 100, 100);

            // Adding a Imageview to display the Sudoku grid
            ImageView grid = new ImageView(this);
            grid.LayoutParameters = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            grid.Visibility = ViewStates.Visible;
            grid.SetBackgroundResource(Resource.Drawable.SudokuGrid);
            grid.Id = 1;
            layoutBase.AddView(grid);

            // Going to try and add a RelativeLayout to make my view nested
            //testLabel = new TextView(this);
            //testLabel.Text = "9";
            //testLabel.SetWidth(180);
            //testLabel.SetHeight(150);
            //testLabel.SetPadding(60, 50, 0, 0);
            //RelativeLayout labelLayout = new RelativeLayout(this);
            //nestLayoutParams = new RelativeLayout.LayoutParams(grid.Width, grid.Height);
            //nestLayoutParams.AddRule(LayoutRules.AlignStart, 1);
            //labelLayout.LayoutParameters = nestLayoutParams;
            //labelLayout.AddView(testLabel);
            //layoutBase.AddView(labelLayout);

            // Adding a button that will be used to step through the "AI"'s solution
            var nextButton = new Button(this) { Text = "Next" };
            nextButton.Id = 2;
            //nextButton.Visibility = ViewStates.Invisible; // Makes the button invisible
            nextButton.Enabled = false; // Disables the button so it can't be clicked
            nextButton.Click += NextButton_Click;

            // Adding a button that will be used to start the "AI" to solve the puzzle
            var startButton = new Button(this) { Text = "Start" };
            startButton.Id = 3;
            //startButton.Visibility = ViewStates.Invisible; // Makes the button invisible
            startButton.Enabled = false; // Disables the button so it can't be clicked
            startButton.Click += startButton_Click;

            // Layout Parameters for Portrait mode
            // nextButton
            var nextButtonParams = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            //nextButtonParamsPortrait.AddRule(LayoutRules.AlignParentBottom);
            //nextButtonParamsPortrait.AddRule(LayoutRules.AlignParentRight);
            // startButton
            var startButtonParams = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            //startButtonParamsPortrait.AddRule(LayoutRules.AlignParentBottom);
            //startButtonParamsPortrait.AddRule(LayoutRules.LeftOf, nextButton.Id);

            // Layout Parameters for Landscape mode
            // startButton
            startButtonParams = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            //startButtonParamsLandscape.AddRule(LayoutRules.AlignParentRight);
            // nextButton
            nextButtonParams = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            //nextButtonParamsLandscape.AddRule(LayoutRules.AlignParentRight);
            //nextButtonParamsLandscape.AddRule(LayoutRules.Below, startButton.Id);

            var checkBox = new CheckBox(this) { Text = "Show Steps" };
            checkBox.Id = 97;
            checkBox.Checked = true;
            var checkBoxParams = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);

            // Depending on the initial orientation, the buttons will placed at different locations
            if (surfaceOrientation == SurfaceOrientation.Rotation0 || surfaceOrientation == SurfaceOrientation.Rotation180)
            {
                // The screen is in Portrait mode
                // nextButton
                nextButtonParams.AddRule(LayoutRules.AlignParentBottom);
                nextButtonParams.AddRule(LayoutRules.AlignParentRight);
                // startButton
                startButtonParams.AddRule(LayoutRules.AlignParentBottom);
                startButtonParams.AddRule(LayoutRules.LeftOf, nextButton.Id);

                startButton.LayoutParameters = startButtonParams;
                nextButton.LayoutParameters = nextButtonParams;
                checkBoxParams.AddRule(LayoutRules.AlignParentBottom);
                checkBox.LayoutParameters = checkBoxParams;

                //labels[0, 0].SetBackgroundColor(Android.Graphics.Color.Blue);
                //labels[1, 1].SetBackgroundColor(Android.Graphics.Color.Blue);
                //labels[2, 2].SetBackgroundColor(Android.Graphics.Color.Blue);
                //labels[3, 3].SetBackgroundColor(Android.Graphics.Color.Blue);
                //labels[4, 4].SetBackgroundColor(Android.Graphics.Color.Blue);
                //labels[5, 5].SetBackgroundColor(Android.Graphics.Color.Blue);
                //labels[6, 6].SetBackgroundColor(Android.Graphics.Color.Blue);
                //labels[7, 7].SetBackgroundColor(Android.Graphics.Color.Blue);
                //labels[8, 8].SetBackgroundColor(Android.Graphics.Color.Blue);
                //labels[8, 0].SetBackgroundColor(Android.Graphics.Color.Blue);
                // Padding for the labels for Portrait orientation
                for (byte i = 0; i < 9; i++)
                {
                    for (byte j = 0; j < 9; j++)
                    {
                        // Set the height and width of each individual label for Portrait orientation
                        labels[i, j].SetHeight(Resources.GetDimensionPixelSize(Resource.Dimension.Plabel_height));
                        labels[i, j].SetWidth(Resources.GetDimensionPixelSize(Resource.Dimension.Plabel_width));
                        // As each has a specific size, height and width, they no longer
                        // different SetPadding parameters as before
                        labels[i, j].SetPadding(Resources.GetDimensionPixelSize(Resource.Dimension.PLpadding_left), Resources.GetDimensionPixelSize(Resource.Dimension.PLpadding_top), 0, 0);
                    }
                }

                //for (int i = 0; i < 9; i++)
                //{
                //    for (int j = 0; j < 9; j++)
                //    {
                //        if (i == 0 && j == 0)
                //        {
                //            labels[i, j].SetPadding(60, 50, 0, 0);
                //        }
                //        else if (i == 0 && j != 0)
                //        {
                //            if (j < 3 || j > 5)
                //            {
                //                labels[i, j].SetPadding(100, 50, 0, 0);
                //            }
                //            else
                //            {
                //                labels[i, j].SetPadding(120, 50, 0, 0);
                //            }
                //        }
                //        else if (i != 0)
                //        {
                //            labels[i, j].SetPadding(60, 100, 0, 0);
                //            if (j != 0)
                //            {
                //                if (j < 3 || j < 5)
                //                {
                //                    labels[i, j].SetPadding(100, 100, 0, 0);
                //                }
                //                else
                //                {
                //                    labels[i, j].SetPadding(120, 100, 0, 0);
                //                }
                //            }
                //        }
                //    }
                //}
            }
            else
            {
                // The screen is in Landscape mode
                // startButton
                startButtonParams.AddRule(LayoutRules.AlignParentRight);
                // nextButton
                nextButtonParams.AddRule(LayoutRules.AlignParentRight);
                nextButtonParams.AddRule(LayoutRules.Below, startButton.Id);

                startButton.LayoutParameters = startButtonParams;
                nextButton.LayoutParameters = nextButtonParams;
                checkBoxParams.AddRule(LayoutRules.AlignParentRight);
                checkBoxParams.AddRule(LayoutRules.AlignParentBottom);
                checkBox.LayoutParameters = checkBoxParams;

                // Padding for the labels for Landscape orientation
                for (byte i = 0; i < 9; i++)
                {
                    for (byte j = 0; j < 9; j++)
                    {
                        // Set the height and width of each individual label for Landscape orientation
                        labels[i, j].SetHeight(Resources.GetDimensionPixelSize(Resource.Dimension.Llabel_height));
                        labels[i, j].SetWidth(Resources.GetDimensionPixelSize(Resource.Dimension.Llabel_width));
                        // As each has a specific size, height and width, they no longer
                        // different SetPadding parameters as before
                        labels[i, j].SetPadding(Resources.GetDimensionPixelSize(Resource.Dimension.LLpadding_left), Resources.GetDimensionPixelSize(Resource.Dimension.LLpadding_top), 0, 0);
                    }
                }

                //for (int i = 0; i < 9; i++)
                //{
                //    for (int j = 0; j < 9; j++)
                //    {
                //        if (i == 0 && j == 0)
                //        {
                //            labels[i, j].SetPadding(80, 30, 0, 0);
                //        }
                //        else if (i == 0 && j != 0)
                //        {
                //            if (j < 3 || j > 5)
                //            {
                //                labels[i, j].SetPadding(160, 30, 0, 0);
                //            }
                //            else
                //            {
                //                labels[i, j].SetPadding(140, 30, 0, 0);
                //            }
                //        }
                //        else if (i != 0)
                //        {
                //            labels[i, j].SetPadding(80, 40, 0, 0);
                //            if (j != 0)
                //            {
                //                if (j < 3 || j < 5)
                //                {
                //                    labels[i, j].SetPadding(160, 40, 0, 0);
                //                }
                //                else
                //                {
                //                    labels[i, j].SetPadding(140, 40, 0, 0);
                //                }
                //            }
                //        }
                //    }
                //}
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
            //testLabel.Click += (sender, e) =>
            //{
            //    testLabel.Text = "" + count++;
            //};


            // Add the buttons to the layout
            layoutBase.AddView(startButton);
            layoutBase.AddView(nextButton);
            layoutBase.AddView(checkBox);
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

        private void NextButton_Click(object sender, EventArgs e)
        {
            waitingOnNext = false;
        }

        public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);

            var checkBoxParams = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            var startButton = FindViewById<Button>(3);
            var nextButton = FindViewById<Button>(2);
            // Parameters for nextButton
            var nextButtonParams = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            // Parameters for startButton
            var startButtonParams = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);


            if (newConfig.Orientation == Android.Content.Res.Orientation.Portrait)
            {
                // nextButton Rules
                nextButtonParams.AddRule(LayoutRules.AlignParentBottom);
                nextButtonParams.AddRule(LayoutRules.AlignParentRight);
                // startButton Rules
                startButtonParams.AddRule(LayoutRules.AlignParentBottom);
                startButtonParams.AddRule(LayoutRules.LeftOf, nextButton.Id);

                startButton.LayoutParameters = startButtonParams;
                nextButton.LayoutParameters = nextButtonParams;

                checkBoxParams.AddRule(LayoutRules.AlignParentBottom);

                FindViewById<CheckBox>(97).LayoutParameters = checkBoxParams;

                try
                {
                    var restart = FindViewById<Button>(99);
                    if (restart != null)
                    {
                        // Layout Parameters
                        var restartParams = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                        restartParams.AddRule(LayoutRules.AlignParentBottom);
                        restartParams.AddRule(LayoutRules.LeftOf, 3);
                        restart.LayoutParameters = restartParams;
                    }
                }
                catch (ArgumentNullException e) { throw e; }

                // Padding for the labels for Portrait orientation
                for (byte i = 0; i < 9; i++)
                {
                    for (byte j = 0; j < 9; j++)
                    {
                        // Set the height and width of each individual label for Portrait orientation
                        labels[i, j].SetHeight(Resources.GetDimensionPixelSize(Resource.Dimension.Plabel_height));
                        labels[i, j].SetWidth(Resources.GetDimensionPixelSize(Resource.Dimension.Plabel_width));
                        // As each has a specific size, height and width, they no longer
                        // different SetPadding parameters as before
                        labels[i, j].SetPadding(Resources.GetDimensionPixelSize(Resource.Dimension.PLpadding_left), Resources.GetDimensionPixelSize(Resource.Dimension.PLpadding_top), 0, 0);
                    }
                }
            }
            else if (newConfig.Orientation == Android.Content.Res.Orientation.Landscape)
            {
                // startButton Rules
                startButtonParams.AddRule(LayoutRules.AlignParentRight);
                // nextButton Rules
                nextButtonParams.AddRule(LayoutRules.AlignParentRight);
                nextButtonParams.AddRule(LayoutRules.Below, startButton.Id);

                startButton.LayoutParameters = startButtonParams;
                nextButton.LayoutParameters = nextButtonParams;

                checkBoxParams.AddRule(LayoutRules.AlignParentRight);
                checkBoxParams.AddRule(LayoutRules.AlignParentBottom);

                FindViewById<CheckBox>(97).LayoutParameters = checkBoxParams;

                try
                {
                    var restart = FindViewById<Button>(99);
                    if (restart != null)
                    {
                        var restartParams = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                        restartParams.AddRule(LayoutRules.AlignParentRight);
                        restartParams.AddRule(LayoutRules.Below, 2);
                        restart.LayoutParameters = restartParams;
                    }
                }
                catch (ArgumentNullException e) { throw e; }

                // Padding for the labels for Landscape orientation
                for (byte i = 0; i < 9; i++)
                {
                    for (byte j = 0; j < 9; j++)
                    {
                        // Set the height and width of each individual label for Landscape orientation
                        labels[i, j].SetHeight(Resources.GetDimensionPixelSize(Resource.Dimension.Llabel_height));
                        labels[i, j].SetWidth(Resources.GetDimensionPixelSize(Resource.Dimension.Llabel_width));
                        // As each has a specific size, height and width, they no longer
                        // different SetPadding parameters as before
                        labels[i, j].SetPadding(Resources.GetDimensionPixelSize(Resource.Dimension.LLpadding_left), Resources.GetDimensionPixelSize(Resource.Dimension.LLpadding_top), 0, 0);
                    }
                }
            }
        }

        //  Function that is called when the startButton is clicked
        //  This Function will Disable all of the labels so the user can
        //  no longer can any of their values. It will also create the hintGrid
        //  from the values that the user had added to the labels and then pass it
        //  to create a SudokuGrid instance, in order to solve the puzzle
        private async void startButton_Click(object s, EventArgs E)
        {
            var startButton = (s as Button);
            var nextButton = FindViewById<Button>(2);
            var showSteps = FindViewById<CheckBox>(97);
            // Make sure that none of the columns or rows have multiples of a single value
            if (!hasDuplicates())
            {
                byte[,] hintGrid = new byte[9, 9]; // Initializes the all of the values to 0 by default

                // Initialize the hintGrid based on the values in the labels
                // In other words, if the label has a numerical value then 
                // it's given it the same value as the one assigned to the label by the user
                for (byte i = 0; i < 9; i++)
                {
                    for (byte j = 0; j < 9; j++)
                    {
                        if (labels[i, j].Text != "")
                        {
                            // Tries to see if it can parse the string into a byte value.
                            // If it can, then it will assign it within hintGrid
                            byte.TryParse(labels[i, j].Text, out hintGrid[i, j]);
                        }
                    }
                }

                // Disable all of the labels so the user can't change the given puzzle
                disableLabels();

                // Create a SodokuGrid instance; Pass the hintGrid to it
                SudokuGrid sudoku = new SudokuGrid(hintGrid);

                // Disable the availableValues for each slot in the grid

                // No longer want the user to be able to click on the startButton as
                // I've already finished setting up the given puzzle to be solved
                startButton.Enabled = false;

                // Enable the nextButton
                nextButton.Enabled = true;

                // Show the puzzle's beginning state and wait for the user to press next 
                // before starting to solve. Only need to wait if the user wants to see the steps
                if (showSteps.Checked)
                {
                    await waiting(sudoku);
                }

                // Start solving the puzzle
                await solvePuzzle(sudoku);

                // Since the puzzle has been solved need to disable the next button
                nextButton.Enabled = false;

                // Add the restart button
                addRestart();
            }
            else
            {
                // Show a Dialog that informs the user that the given puzzle isn't valid
                AlertDialog.Builder InvalidDialog = new AlertDialog.Builder(this);
                InvalidDialog.SetTitle("Invalid Puzzle");
                RelativeLayout linearLayout = new RelativeLayout(this);
                linearLayout.LayoutParameters = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                TextView label = new TextView(this);
                label.Text = "The given puzzle contains duplicate values either in the same column, row or square.";
                linearLayout.AddView(label);
                InvalidDialog.SetView(linearLayout);

                InvalidDialog.SetPositiveButton("Understood", (y, a) => { });
                InvalidDialog.Show();
            }
        }

        private void addRestart()
        {
            // Add the restart button the screen
            var restart = new Button(this) { Text = "Reset" };
            restart.Id = 99;

            // Layout Parameters
            var restartParams = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);

            var surfaceOrientation = WindowManager.DefaultDisplay.Rotation;

            // Depending on the initial orientation, the buttons will placed at different locations
            if (surfaceOrientation == SurfaceOrientation.Rotation0 || surfaceOrientation == SurfaceOrientation.Rotation180)
            {
                // The screen is in Portrait mode
                restartParams.AddRule(LayoutRules.AlignParentBottom);
                restartParams.AddRule(LayoutRules.LeftOf, 3);
                restart.LayoutParameters = restartParams;

            }
            else
            {
                // The screen is in Landscape mode
                restartParams.AddRule(LayoutRules.AlignParentRight);
                restartParams.AddRule(LayoutRules.Below, 2);
                restart.LayoutParameters = restartParams;
            }

            restart.Click += Restart_Click;

            var layoutbase = FindViewById<RelativeLayout>(98);
            layoutbase.AddView(restart);
        }

        private void Restart_Click(object sender, EventArgs e)
        {
            for (byte i = 0; i < 9; i++)
            {
                for (byte j = 0; j < 9; j++)
                {
                    labels[i, j].Text = "";
                    labels[i, j].SetTextColor(Android.Graphics.Color.Black);
                }
            }
            enableLabels();
            var layoutbase = FindViewById<RelativeLayout>(98);
            layoutbase.RemoveView(FindViewById<Button>(99));
        }

        // This function will disable the labels so that the user may no longer
        // interact with them, especially when the puzzle has been passed to the solver
        private void disableLabels()
        {
            for (byte i = 0; i < 9; i++)
            {
                for (byte j = 0; j < 9; j++)
                {
                    //labels[i, j].Enabled = false; // disables the label
                    labels[i, j].Clickable = false; // disables the label's click event
                }
            }
        }

        // This function will enable the labels
        private void enableLabels()
        {
            for (byte i = 0; i < 9; i++)
            {
                for (byte j = 0; j < 9; j++)
                {
                    labels[i, j].Clickable = true;
                }
            }
        }

        // This method will force the App to wait until the user clicks on the nextButton
        private async Task<bool> waiting(SudokuGrid sudoku)
        {
            // Display all possible values that each empty Slot can have
            displayCurrentState(sudoku);
            while (waitingOnNext)
            {
                await Task.Delay(1);
            }

            // Need to reset the boolean for the next await
            waitingOnNext = true;
            return true;
        }

        // This function will write the current state of the Sudoku Puzzle to the screen
        // By current I'm referring to the what values an empty Slot can have
        private void displayCurrentState(SudokuGrid sudoku)
        {
            byte val = 0;
            for (byte row = 0; row < 9; row++)
            {
                for (byte col = 0; col < 9; col++)
                {
                    if (sudoku.isSlotEmpty(row, col))
                    {
                        byte[] tempAv = sudoku.getSlotAV(row, col);
                        labels[row, col].Text = "";
                        //labels[row, col].SetTextColor(Resources.GetColorStateList(Resource.Drawable.txtColorAV, null));
                        labels[row, col].SetTextColor(Android.Graphics.Color.Orange);
                        foreach (byte item in tempAv)
                        {
                            val++;
                            if (item == 1)
                            {
                                labels[row, col].Text = labels[row, col].Text + val + " ";
                                labels[row, col].TextSize = 9;
                            }
                            if ((val % 3) == 0)
                            {
                                labels[row, col].Text = labels[row, col].Text + "\n";
                            }
                        }
                    }
                    val = 0;
                }
            }
        }

        // A recursive function that will solve the given puzzle
        // It will continue until it can't find any empty slots in the puzzle
        private async Task<bool> solvePuzzle(SudokuGrid sudoku)
        {
            byte row, col;
            row = col = 0;
            var showSteps = FindViewById<CheckBox>(97);
            if (!findEmptySlot(sudoku, ref row, ref col))
            {
                // Check to make sure that it didn't find an empty Slot that can't be assigned anything
                if (row != 9 && col != 9)
                {
                    // It is an empty Slot so need to go back to assign a different value to the last Slot
                    // If all Slots had been filled then both row and col would be equal to 9, otherwise it's an Empty Slot
                    return false;
                }

                // There are no more empty Slots left hence the puzzle has been solved
                return true;
            }

            // How many possible values should be skipped
            byte skip = 0;
            // while the Slot is empty the loop should continue
            while (sudoku.isSlotEmpty(row, col))
            {
                if (sudoku.addAValueToSlot(row, col, skip))
                {
                    labels[row, col].Text = "" + sudoku.getSlotValue(row, col);
                    //labels[row, col].SetTextColor(Resources.GetColorStateList(Resource.Drawable.txtColorSolved, null));
                    labels[row, col].SetTextColor(Android.Graphics.Color.Blue);
                    labels[row, col].TextSize = 20;

                    if (showSteps.Checked)
                    {
                        await waiting(sudoku); // force the App to wait so the current state can be written to the screen
                    }
                    if (await solvePuzzle(sudoku))
                    {
                        // Only way to get here is that the puzzle has been solved
                        return true;
                    }
                    // The puzzle couldn't be solved with the value assigned previously
                    // so it needs to be reset so the while loop can continue
                    sudoku.addValue(row, col, 0);
                    labels[row, col].Text = "";
                    labels[row, col].SetTextColor(Android.Graphics.Color.Orange);
                    skip++;
                    if (showSteps.Checked)
                    {
                        await waiting(sudoku); // force the App to wait so the current state can be written to the screen
                    }
                }
                if (!sudoku.canPlaceAValue(row, col, skip))
                {
                    break;
                }
            }

            //for (byte num = 1; num < 10; num++)
            //{
            //    if (sudoku.canAddValue(row, col, num))
            //    {
            //        sudoku.addValue(row, col, num);
            //        labels[row, col].Text = "" + num;
            //        if (solvePuzzle(sudoku))
            //        {
            //            return true;
            //        }
            //        sudoku.addValue(row, col, 0);
            //        labels[row, col].Text = "_";
            //    }
            //}

            // returns false as nothing could be placed in this Slot, meaning the
            // current placement of values isn't correct
            return false;
        }

        // This function will try to find an empty Slot with in the grid, if found it will
        // return true indicating that there's an empty Slot and will also return its
        // location on the grid through the use of reference variables.
        private bool findEmptySlot(SudokuGrid sudoku, ref byte row, ref byte col)
        {
            byte tempAV = 10;    // If there are no empty slots then this will remain as 10
            byte tempRow = 10;
            byte tempCol= 10;

            // Want the function to find the Slot with the least amount of possible values it can hold
            // If the any given Slot it finds only has one possible value, return it immediately
            for (row = 0; row < 9; row++)
            {
                for (col = 0; col < 9; col++)
                {
                    if (sudoku.isSlotEmpty(row, col))
                    {
                        if (sudoku.getSlotMaxAv(row, col) < tempAV)
                        {
                            tempAV = sudoku.getSlotMaxAv(row, col);
                            if (tempAV == 0)
                            {
                                // found an empty slot that can't be assigned anything
                                return false;
                            }
                            tempRow = row;
                            tempCol = col;
                            // Check to see if the Slot isn't a Hidden Single
                            // i.e has only one available value
                            if (tempAV == 1)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            if (tempAV > 1  && tempAV < 10)
            {
                // The for loop found an empty Slot that has more than one available value
                // Need to update the reference values to point to the accurate Slot
                row = tempRow;
                col = tempCol;
                return true;
            }
            // There are no empty Slots in the grid.
            return false;

            // finds the first empty slot. Searches row-wise
            //for (row = 0; row < 9; row++)
            //{
            //    for (col = 0; col < 9; col++)
            //    {
            //        if (sudoku.isSlotEmpty(row, col))
            //        {
            //            return true;
            //        }
            //    }
            //}
            //return false;
        }

        //bool withinRow(int x, int row, int p[][9])
        //{
        //    for (int col = 0; col < 9; col++)
        //    {
        //        if (p[row][col] == x)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        //bool withinColumn(int x, int col, int p[][9])
        //{
        //    for (int row = 0; row < 9; row++)
        //    {
        //        if (p[row][col] == x)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        //bool withinSquare(int x, int startRow, int startCol, int p[][9])
        //{
        //    for (int row = 0; row < 3; row++)
        //    {
        //        for (int col = 0; col < 3; col++)
        //        {
        //            if (p[row + startRow][col + startCol] == x)
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}

        //bool canAdd(byte x, byte row, byte col, SudokuGrid sudoku)
        //{
        //    return !withinRow(x, row, p) && !withinColumn(x, col, p)
        //        && !withinSquare(x, row - (row % 3), col - (col % 3), p);
        //}

        //  Will go through the labels to determine whether or not the user
        //  provided a valid puzzle.
        private bool hasDuplicates()
        {
            bool duplicates = false;
            for (byte i = 0; i < 9; i++)
            {
                for (byte j = 0; j < 9; j++)
                {
                    if (labels[i, j].Text != "")
                    {
                        // Check to see if the value is within the same column
                        for (byte col = (byte)(j - 1); col != 255; col--)
                        {
                            if (labels[i, j].Text == labels[i, col].Text)
                            {
                                duplicates = true;
                                break;
                            }
                        }

                        // If duplicates is true, there's no reason to continue the inner for loop
                        if (duplicates) { break; }

                        for (byte col = (byte)(j + 1); col < 9; col++)
                        {
                            if (labels[i, j].Text == labels[i, col].Text)
                            {
                                duplicates = true;
                                break;
                            }
                        }

                        // If duplicates is true, there's no reason to continue the inner for loop
                        if (duplicates) { break; }

                        // Check to see if the value is within the same row
                        for (byte row = (byte)(i - 1); row != 255; row--)
                        {
                            if (labels[i, j].Text == labels[row, j].Text)
                            {
                                duplicates = true;
                                break;
                            }
                        }

                        // If duplicates is true, there's no reason to continue the inner for loop
                        if (duplicates) { break; }

                        for (byte row = (byte)(i + 1); row < 9; row++)
                        {
                            if (labels[i, j].Text == labels[row, j].Text)
                            {
                                duplicates = true;
                                break;
                            }
                        }

                        // If duplicates is true, there's no reason to continue the inner for loop
                        if (duplicates) { break; }

                        // Check to see if the value is within the same square
                        byte startRow = (byte)(i - (i % 3));
                        byte startCol = (byte)(j - (j % 3));
                        for (byte row = 0; row < 3; row++)
                        {
                            for (byte col = 0; col < 3; col++)
                            {
                                if (i != row + startRow && j != col + startCol)
                                {
                                    if (labels[i, j].Text == labels[row + startRow, col + startCol].Text)
                                    {
                                        duplicates = true;
                                        break;
                                    }
                                }
                                if (duplicates) { break; }
                            }
                            if (duplicates) { break; }
                        }
                    }

                    // If duplicates is true, there's no reason to continue the inner for loop
                    if (duplicates) { break; }
                }

                // If duplicates is true, there's no reason to continue the outer for loop
                if (duplicates) { break; }
            }
            return duplicates;
        }

        //  This function will display a "picker" that will allow the user to select a number from 1 to 9
        //  and the number will be placed in the slot. This also gets rid of the problem of having to double click before
        private async Task click(TextView label)
        {
            // Create a dialog to get user input
            var dialog = new NumberPickerDialogFragment(this, 1, 9, 3, ref label);
            dialog.Show(FragmentManager, "number");

            await Task.Delay(500); // wait for the dialog to show on to the screen
            
            // Don't want the function to exit until the dialog has been closed
            while (dialog.IsAdded)
            {
                await Task.Delay(50);
            }
        }

        //  Function that is called whenever one of the labels has been pressed/tapped
        private async void label_Click(object s, EventArgs E)
        {
            TextView label = (s as TextView);
            var startButton = FindViewById<Button>(3);
            await click(label); // moved logic to this function to prevent multiple alertDialogs from opening

            // Need to enable the startButton if it hasn't already been enabled
            // It should only be enabled when at least one of the labels has a numeric value
            // It will remain enabled even if the all of the labels were to be cleared
            // In this case, I would need another function that scans all labels before it can be disabled,
            // Or I could just a byte/int type variable that keeps track of number of labels with numeric values
            if (!startButton.Enabled && label.Text != "")
            {
                startButton.Enabled = true;
            }

            //label.Click += async (sender, e) =>
            //{
            //    // Create a dialog to get user input
            //    var dialog = new NumberPickerDialogFragment(this, 1, 9, 3, ref label);
            //    dialog.Show(FragmentManager, "number");

            //    await Task.Delay(500); // wait for the dialog to show on to the screen
            //    // Don't want the function to continue executing until the dialog has been closed
            //    while (dialog.IsAdded)
            //    {
            //        await Task.Delay(50);
            //    }
            //    // Need to enable the startButton if it hasn't already been enabled
            //    // It should only be enabled when at least one of the labels has a nonzero value
            //    if (!startButton.Enabled && label.Text != "_")
            //    {
            //        startButton.Enabled = true;
            //    }

            //    /**  Will use the NumberPickerDialog class instead creating the dialog here
            //    // Create a linerLayout for which hold a NumberPicker
            //    // The NumberPicker will have a range of 1-9 and should have its
            //    // orientation set as horizontal, not sure if it works.
            //    RelativeLayout linearLayout = new RelativeLayout(this);
            //    NumberPicker picker = new NumberPicker(this);
            //    picker.MaxValue = 9;
            //    picker.MinValue = 1;
            //    picker.Orientation = Orientation.Horizontal;

            //    // Set the layout parameters for both the linearLayout and the picker
            //    // The picker will be set to the center, so it is in the displayed dialog, before
            //    // adding it to the linearLayout.
            //    RelativeLayout.LayoutParams Layparams = new RelativeLayout.LayoutParams(50, 50);
            //    RelativeLayout.LayoutParams pickerParams = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            //    pickerParams.AddRule(LayoutRules.CenterInParent);
            //    picker.LayoutParameters = pickerParams;

            //    linearLayout.LayoutParameters = Layparams;
            //    linearLayout.LayoutDirection = LayoutDirection.Ltr;
            //    linearLayout.AddView(picker);

            //    // Create an AlertDialog that will hold the linearLayout with the NumberPicker
            //    // When the OK button is clicked, it will update the clicked label's Text field
            //    // to display the selected value in the picker.
            //    AlertDialog.Builder pickerAlertDialog = new AlertDialog.Builder(this);
            //    pickerAlertDialog.SetTitle("Select a number");
            //    pickerAlertDialog.SetView(linearLayout);

            //    /** Use the NumberPicker created in the NumberPickerDialog.xml **
            //    //var inflater = (LayoutInflater)this.GetSystemService(Context.LayoutInflaterService);
            //    //var view = inflater.Inflate(Resource.Layout.NumberPickerDialog, null);
            //    //var numberPicker = view.FindViewById<NumberPicker>(Resource.Id.numberPicker);
            //    //numberPicker.MaxValue = 9;
            //    //numberPicker.MinValue = 1;
            //    //numberPicker.Value = 2;
            //    //pickerAlertDialog.SetView(view);

            //    pickerAlertDialog.SetPositiveButton("OK", (y, a) => { label.Text = picker.Value.ToString(); });
            //    pickerAlertDialog.SetNegativeButton("Cancel", (y, a) => { });
            //    pickerAlertDialog.Show(); **/

            //};
        }
    }
}