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
    class SudokuGrid : Slot
    {
        private Slot[,] squares;    // A 2D Array to hold all of the Slots on the grid

        // Constructor
        public SudokuGrid(byte[,] hintGrid)
        {
            // Initialize the squares based on the hintGrid
            setupSquares(ref hintGrid);

            // Now that the squares have been setup, their AVs need to be updated
            updateSlotAVs();
        }

        // Method to set up the squares 2D array
        // The hintGrid will be passed as a reference by the Constructor in order to save memory
        private void setupSquares(ref byte[,] hintGrid)
        {
            squares = new Slot[9, 9];
            for (byte i = 0; i < 9; i++)
            {
                for (byte j = 0; j < 9; j++)
                {
                    if (hintGrid[i, j] != 0)
                    {
                        // The hintGrid indicates that this Slot needs to be setup as a hint
                        squares[i, j] = new Slot(hintGrid[i, j]);
                    }
                    else
                    {
                        // This Slot isn't a Hint, so the default constructor is called;
                        // In other words setup AN EMPTY SLOT
                        squares[i, j] = new Slot();
                    }
                }
            }
        }

        // update all of the slots to reflect what values are available for it
        public void updateSlotAVs()
        {
            // Loop through the squares and update their AVs
            // To Update the AVs, will check what values have already been assigned in the Slot's
            // Column and Row
            for (byte i = 0; i < 9; i++)
            {
                for (byte j = 0; j < 9; j++)
                {
                    // Make sure the Slot isn't a Hint
                    if (!squares[i, j].isSlotAHint())
                    {
                        byte[] tempAV = { 1, 1, 1, 1, 1, 1, 1, 1, 1 };  // Will be used to pass on to the Slot's update method

                        // Check within the Slot's Column
                        for (byte col = 0; col < 9; col++)
                        {
                            if (!squares[i, col].isEmpty())
                            {
                                // Update the tempAV 
                                tempAV[(squares[i, col].getValue() - 1)] = 0;
                            }
                        }

                        // Check within the Slot's Row
                        for (byte row = 0; row < 9; row++)
                        {
                            if (!squares[row, j].isEmpty())
                            {
                                // Update the tempAV 
                                tempAV[(squares[row, j].getValue() - 1)] = 0;
                            }
                        }

                        // Check within the Slot's Square
                        byte startRow = (byte)(i - (i % 3));
                        byte startCol = (byte)(j - (j % 3));
                        for (int row = 0; row < 3; row++)
                        {
                            for (int col = 0; col < 3; col++)
                            {
                                if (!squares[row + startRow, col + startCol].isEmpty())
                                {
                                    tempAV[squares[row + startRow, col + startCol].getValue() - 1] = 0;
                                }
                            }
                        }

                        squares[i, j].updateAV(tempAV); // updates the Slot's AV
                    }
                }
            }
        }

        // Checks to see if the specified Slot is empty or not
        public bool isSlotEmpty(byte row, byte col)
        {
            return squares[row, col].isEmpty();
        }

        // Check to see if the value can be placed in the specified Slot
        public bool canAddValue(byte row, byte col, byte val)
        {
            if (squares[row, col].isAvailable(val))
            {
                return true;
            }
            return false;
        }

        // Add a value to a specified Slot
        public void addValue(byte row, byte col, byte val)
        {
            squares[row, col].setValue(val);
            updateSlotAVs();
        }

        // Will find the Slot with the least amount of possible values it can have.
        // Will also try to find the only Slot that can have any one value in it both Column and Row wise
        public Slot findSlotWithLeastAVs()
        {
            return squares[0, 0];
        }
    }
}