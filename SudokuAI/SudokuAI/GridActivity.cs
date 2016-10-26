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
        TextView testLabel;
        RelativeLayout.LayoutParams nestLayoutParams;

        //int count = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            // Don't want the action bar to show, find it unnecessary, so gonna hide it.
            ActionBar.Hide();

            base.OnCreate(savedInstanceState);

            // Create your application here

            /**
             * Once the APP has been finished to the point that it can solve the given Sudoku puzzle,
             * come back to make better looking labels. At the time of writing this, I suggest hardcoding
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
                    labels[i,j] = new TextView(this);
                    labels[i,j].Text = "";
                    // Give each of the labels a unique ID; range 4-84
                    labels[i,j].Id = (4 + (9 * i)) + j;
                    //labels[i, j].Text = "" + labels[i, j].Id;
                    labels[i, j].Text = "_";
                    labels[i, j].TextSize = 20;
                    //labels[i, j].TextAlignment = TextAlignment.Gravity;
                    //labels[i, j].SetHeight(180);
                    //labels[i, j].SetWidth(150);
                    //labels[i, j].SetPadding(60, 50, 0, 0);

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
            layoutBase.LayoutParameters = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            layoutBase.SetPadding(100, 100, 100, 100);

            // Adding a Imageview to display the sudoku grid
            ImageView grid = new ImageView(this);
            grid.LayoutParameters = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            grid.Visibility = ViewStates.Visible;
            grid.SetBackgroundResource(Resource.Drawable.SudokuGrid);
            grid.Id = 1;
            layoutBase.AddView(grid);

            // Going to try and add a RelativeLayout to make my view nested
            testLabel = new TextView(this);
            testLabel.Text = "9";
            testLabel.SetWidth(180);
            testLabel.SetHeight(150);
            testLabel.SetPadding(60, 50, 0, 0);
            RelativeLayout labelLayout = new RelativeLayout(this);
            nestLayoutParams = new RelativeLayout.LayoutParams(grid.Width, grid.Height);
            nestLayoutParams.AddRule(LayoutRules.AlignStart, 1);
            labelLayout.LayoutParameters = nestLayoutParams;
            labelLayout.AddView(testLabel);
            layoutBase.AddView(labelLayout);

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


                // Padding for the labels for Portrait orientation
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (i == 0 && j == 0)
                        {
                            labels[i, j].SetPadding(60, 50, 0, 0);
                        }
                        else if (i == 0 && j != 0)
                        {
                            if (j < 3 || j > 5)
                            {
                                labels[i, j].SetPadding(100, 50, 0, 0);
                            }
                            else
                            {
                                labels[i, j].SetPadding(120, 50, 0, 0);
                            }
                        }
                        else if (i != 0)
                        {
                            labels[i, j].SetPadding(60, 100, 0, 0);
                            if (j != 0)
                            {
                                if (j < 3 || j < 5)
                                {
                                    labels[i, j].SetPadding(100, 100, 0, 0);
                                }
                                else
                                {
                                    labels[i, j].SetPadding(120, 100, 0, 0);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                // The screen is in Landscape mode
                startButton.LayoutParameters = startButtonParamsLandscape;
                nextButton.LayoutParameters = nextButtonParamsLandscape;
                
                // Padding for the labels for Landscape orientation
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (i == 0 && j == 0)
                        {
                            labels[i, j].SetPadding(80, 30, 0, 0);
                        }
                        else if (i == 0 && j != 0)
                        {
                            if (j < 3 || j > 5)
                            {
                                labels[i, j].SetPadding(160, 30, 0, 0);
                            }
                            else
                            {
                                labels[i, j].SetPadding(140, 30, 0, 0);
                            }
                        }
                        else if (i != 0)
                        {
                            labels[i, j].SetPadding(80, 40, 0, 0);
                            if (j != 0)
                            {
                                if (j < 3 || j < 5)
                                {
                                    labels[i, j].SetPadding(160, 40, 0, 0);
                                }
                                else
                                {
                                    labels[i, j].SetPadding(140, 40, 0, 0);
                                }
                            }
                        }
                    }
                }
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

                // Padding for the labels for Portrait orientation
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (i == 0 && j == 0)
                        {
                            labels[i, j].SetPadding(60, 50, 0, 0);
                        }
                        else if (i == 0 && j != 0)
                        {
                            if (j < 3 || j > 5)
                            {
                                labels[i, j].SetPadding(100, 50, 0, 0);
                            }
                            else
                            {
                                labels[i, j].SetPadding(120, 50, 0, 0);
                            }
                        }
                        else if (i != 0)
                        {
                            labels[i, j].SetPadding(60, 100, 0, 0);
                            if (j != 0)
                            {
                                if (j < 3 || j < 5)
                                {
                                    labels[i, j].SetPadding(100, 100, 0, 0);
                                }
                                else
                                {
                                    labels[i, j].SetPadding(120, 100, 0, 0);
                                }
                            }
                        }
                    }
                }
            }
            else if (newConfig.Orientation == Android.Content.Res.Orientation.Landscape)
            {
                startButton.LayoutParameters = startButtonParamsLandscape;
                nextButton.LayoutParameters = nextButtonParamsLandscape;

                // Padding for the labels for Landscape orientation
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (i == 0 && j == 0)
                        {
                            labels[i, j].SetPadding(80, 30, 0, 0);
                        }
                        else if (i == 0 && j != 0)
                        {
                            if (j < 3 || j > 5)
                            {
                                labels[i, j].SetPadding(160, 30, 0, 0);
                            }
                            else
                            {
                                labels[i, j].SetPadding(140, 30, 0, 0);
                            }
                        }
                        else if (i != 0)
                        {
                            labels[i, j].SetPadding(80, 40, 0, 0);
                            if (j != 0)
                            {
                                if (j < 3 || j < 5)
                                {
                                    labels[i, j].SetPadding(160, 40, 0, 0);
                                }
                                else
                                {
                                    labels[i, j].SetPadding(140, 40, 0, 0);
                                }
                            }
                        }
                    }
                }
            }
        }

        //  Function that is called whenever one of the labels has been pressed/tapped
        //  This function will display a "picker" that will allow the user to select a number from 1 to 9
        //  and the number will be placed in the slot
        private void label_Click(object s, EventArgs E)
        {
            TextView label = (s as TextView);
            label.Click += (sender, e) =>
            {
                // Create a dialog to get user input
                //var dialog = new DialogFragment();
                //dialog.s
                //dialog.Show(FragmentManager, "Number");

                // Create a linerLayout for which hold a NumberPicker
                // The NumberPicker will have a range of 1-9 and should have its
                // orientation set as horizontal, not sure if it works.
                RelativeLayout linearLayout = new RelativeLayout(this);
                NumberPicker picker = new NumberPicker(this);
                picker.MaxValue = 9;
                picker.MinValue = 1;
                picker.Orientation = Orientation.Horizontal;
                
                // Set the layout parameters for both the linearLayout and the picker
                // The picker will be set to the center, so it is in the displayed dialog, before
                // adding it to the linearLayout.
                RelativeLayout.LayoutParams Layparams = new RelativeLayout.LayoutParams(50, 50);
                RelativeLayout.LayoutParams pickerParams = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                pickerParams.AddRule(LayoutRules.CenterInParent);
                picker.LayoutParameters = pickerParams;

                linearLayout.LayoutParameters = Layparams;
                linearLayout.LayoutDirection = LayoutDirection.Ltr;
                linearLayout.AddView(picker);

                // Create an AlertDialog that will hold the linearLayout with the NumberPicker
                // When the OK button is clicked, it will update the clicked label's Text field
                // to display the selected value in the picker.
                AlertDialog.Builder pickerAlertDialog = new AlertDialog.Builder(this);
                pickerAlertDialog.SetTitle("Select a number");
                pickerAlertDialog.SetView(linearLayout);

                /** Use the NumberPicker created in the NumberPickerDialog.xml **/
                //var inflater = (LayoutInflater)this.GetSystemService(Context.LayoutInflaterService);
                //var view = inflater.Inflate(Resource.Layout.NumberPickerDialog, null);
                //var numberPicker = view.FindViewById<NumberPicker>(Resource.Id.numberPicker);
                //numberPicker.MaxValue = 9;
                //numberPicker.MinValue = 1;
                //numberPicker.Value = 2;
                //pickerAlertDialog.SetView(view);

                pickerAlertDialog.SetPositiveButton("OK", (y, a) => { label.Text = picker.Value.ToString(); });
                pickerAlertDialog.SetNegativeButton("Cancel", (y, a) => { });
                pickerAlertDialog.Show();

                //var dialog = new NumberPickerDialogFragment(this, 1, 9, 3);
                //dialog.Show(FragmentManager, "number");
                

                // Create a new relative layout that contains 9 TextViews for values 1-9
                //RelativeLayout numberPickerLayout = new RelativeLayout(this);
                //numberPickerLayout.Id = 0;
                //for (int i = 1; i < 10; i++)
                //{
                //    TextView npLabel = new TextView(this);
                //    npLabel.Id = i;
                //    npLabel.Text = (i).ToString();
                //    var txtParams = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                //    if (i != 1)
                //    {
                //        txtParams.AddRule(LayoutRules.RightOf, i);
                //    }
                //    npLabel.LayoutParameters = txtParams;
                //    numberPickerLayout.AddView(npLabel);
                //}
                //SetContentView(numberPickerLayout);
            };
        }
    }

    // Found this class online to create a NumberPicker within a Dialog.
    // I was going to use this to allow the user to picker a number to assign
    // to the clicked label, but I can't seem to get this working as the example
    // showed passing Activity as the context and this as the listener, but that didn't
    // work. It allowed me to pass this as context as I had assumed it should, but I wasn't
    // able to think what I needed to pass as the listener. Thought about passing the label,
    // a new NumberPicker.IOnClickListener, but these didn't work.
    public class NumberPickerDialogFragment : DialogFragment
    {
        private readonly Context _context;
        private readonly int _min, _max, _current;
        private readonly NumberPicker.IOnValueChangeListener _listener;

        public NumberPickerDialogFragment(Context context, int min, int max, int current, NumberPicker.IOnValueChangeListener listener)
        {
            _context = context;
            _min = min;
            _max = max;
            _current = current;
            _listener = listener;
        }

        public override Dialog OnCreateDialog(Bundle savedState)
        {
            var inflater = (LayoutInflater)_context.GetSystemService(Context.LayoutInflaterService);
            var view = inflater.Inflate(Resource.Layout.NumberPickerDialog, null);
            var numberPicker = view.FindViewById<NumberPicker>(Resource.Id.numberPicker);
            numberPicker.MaxValue = _max;
            numberPicker.MinValue = _min;
            numberPicker.Value = _current;
            numberPicker.SetOnValueChangedListener(_listener);

            var dialog = new AlertDialog.Builder(_context);
            dialog.SetTitle(Resource.String.NumberPickerTitle);
            dialog.SetView(view);
            dialog.SetNegativeButton("Cancel", (s, a) => { });
            dialog.SetPositiveButton("OK", (s, a) => { });
            return dialog.Create();
        }
    }
}