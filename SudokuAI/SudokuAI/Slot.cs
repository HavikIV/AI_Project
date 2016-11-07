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
    class Slot
    {
        private byte value;                 // The value for the slot; Set to 0 by Default
        private byte maxAV;                 // Indicates how big the availableValues array is going to be
        private byte[] availableValues;     // Possible values the slot can have, if not already has one.
        private bool isHint;                // Boolean Check to indicate whether or not the value was given as a Hint

        // Default constructor
        public Slot()
        {
            value = 0;
            isHint = false;
            availableValues = new byte[] { 1, 1, 1, 1, 1, 1, 1, 1, 1}; // The Slot is available to be assigned any value between 1-9
        }

        //  Constructor for setting up the Slot as a Hint
        public Slot(byte v)
        {
            value = v;
            isHint = true;
            availableValues = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0}; // As the Slot is a Hint, it can't have any other value
        }

        // Checks to see if a value has been assigned to the slot yet
        public bool isEmpty()
        {
            return (value == 0);
        }

        // Returns isHint
        public bool isSlotAHint()
        {
            return isHint;
        }

        // Returns the Slot's Value
        public byte getValue()
        {
            return value;
        }

        // Sets the value to the given v
        public void setValue(byte v)
        {
            value = v;
        }

        // Check if the value is available to be assigned
        public bool isAvailable(byte num)
        {
            if (availableValues[num - 1] == 1)
            {
                return true;
            }
            return false;
        }

        // Updates the Slot's availableValues to the passed av
        public void updateAV(byte[] av)
        {
            for (byte i = 0; i < 9; i++)
            {
                availableValues[i] = av[i];
            }
        }
    }
}