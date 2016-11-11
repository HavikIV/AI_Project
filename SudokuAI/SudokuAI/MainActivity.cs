using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace SudokuAI
{
    [Activity(Theme = "@android:style/Theme.Material.Light", Label = "SudokuAI", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            ActionBar.Hide();

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            //Button openButton = FindViewById<Button>(Resource.Id.MyButton);

            ImageView openImage = FindViewById<ImageView>(Resource.Id.MyImage);

            openImage.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(GridActivity));
                StartActivity(intent);
            };

            //openButton.Click += (ssender, e) =>
            //{
            //    var intent = new Intent(this, typeof(GridActivity));
            //    StartActivity(intent);
            //};
        }
    }
}

