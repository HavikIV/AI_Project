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
    class LinkedList
    {
        public class Node
        {
            public Node next;
            public byte row, col;

        }

        private byte count = 0;
        private Node root = null;

        // Grab the first Node in the list
        public Node First { get { return root; } }

        // Grab the last Node in the list
        public Node Last
        {
            get
            {
                Node curr = root;
                if (curr == null)
                    return null;
                while (curr.next != null)
                    curr = curr.next;
                return curr;
            }
        }

        // Add a new Node to the end of the list
        public void Append(byte r, byte c)
        {
            Node n = new Node();
            n.row = r;
            n.col = c;
            if (root == null)
                root = n;
            else
                Last.next = n;
            count++;
        }

        // Delete the First Node in the list
        public void deleteFirst()
        {
            root = root.next;
            count--;
        }

        // Get how many Nodes are in the list
        public byte Count()
        {
            return count;
        }
    }
}