using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace SudokuAI
{
    // Found this class online to create a NumberPicker within a Dialog.
    // I was going to use this to allow the user to picker a number to assign
    // to the clicked label, but I can't seem to get this working as the example
    // showed passing Activity as the context and this as the listener, but that didn't
    // work. It allowed me to pass this as context as I had assumed it should, but I wasn't
    // able to think what I needed to pass as the listener. Thought about passing the label,
    // a new NumberPicker.IOnClickListener, but these didn't work.
    // Changed the Class so instead of taking NumberPicker.IOnClickListner, I will pass
    // the Label I want display the value as a reference
    public class NumberPickerDialogFragment : DialogFragment
    {
        private readonly Context _context;
        private readonly int _min, _max, _current;
        private readonly TextView _label;

        public NumberPickerDialogFragment(Context context, int min, int max, int current, ref TextView label)
        {
            _context = context;
            _min = min;
            _max = max;
            _current = current;
            _label = label;
        }

        public override Dialog OnCreateDialog(Bundle savedState)
        {
            //inflater is used to generate UI based on predefined xml files
            var inflater = (LayoutInflater)_context.GetSystemService(Context.LayoutInflaterService);
            // view contains the actual UI from the xml file
            var view = inflater.Inflate(Resource.Layout.NumberPickerDialog, null);
            // Find the numberPicker in the UI, so it can be manipulated
            var numberPicker = view.FindViewById<NumberPicker>(Resource.Id.numberPicker);
            numberPicker.MaxValue = _max;
            numberPicker.MinValue = _min;
            numberPicker.Value = _current;

            // Create an AlertDialog that will contain the UI to display to the user
            var dialog = new AlertDialog.Builder(_context);
            dialog.SetTitle(Resource.String.NumberPickerTitle);
            dialog.SetView(view);
            dialog.SetNegativeButton("Cancel", (s, a) => { });
            dialog.SetPositiveButton("OK", (s, a) => {
                if (!view.FindViewById<CheckBox>(Resource.Id.Clear).Checked)
                    _label.Text = numberPicker.Value.ToString();
                else
                    _label.Text = "";
            });
            return dialog.Create(); // this visually displays the dialog to the screen
        }
    }
}