using Sol3.House.ApiModel;
using System;
using System.Collections.Generic;

namespace Sol3.ListAlgorithms.Model
{
    public enum Color
    {
        Red,
        Black
    }

    public class Rank<T> where T : IDetail, new()
    {
        public void CreateTree(List<T> valueList)
        {
            var tree = new RB();
            foreach(var val in valueList)
                tree.Insert(val);
            tree.DisplayTree();
        }
    }

    class RB
    {
        /// <summary>
        /// Object of type RankNode contains 4 properties
        /// Colour
        /// Left
        /// Right
        /// Parent
        /// Data
        /// </summary>
        public class RankNode
        {
            public RankNode(IDetail objectToBeSorted) { Value = objectToBeSorted; }
            public RankNode(IDetail objectToBeSorted, int data) { Value = objectToBeSorted; this.Data = data; }
            public RankNode(IDetail objectToBeSorted, Color colour) { Value = objectToBeSorted; this.Colour = colour; }
            public RankNode(IDetail objectToBeSorted, int data, Color colour) { Value = objectToBeSorted; this.Data = data; this.Colour = colour; }

            public int Data { get; set; }
            public Color Colour { get; set; }
            public IDetail Value { get; set; }
            public RankNode Left { get; set; }
            public RankNode Right { get; set; }
            public RankNode Parent { get; set; }
        }

        /// <summary>
        /// Root node of the tree (both reference & pointer)
        /// </summary>
        private RankNode root;

        ///// <summary>
        ///// New instance of a Red-Black tree object
        ///// </summary>
        //public RB() { }

        /// <summary>
        /// Left Rotate
        /// </summary>
        /// <param name="X"></param>
        /// <returns>void</returns>
        private void LeftRotate(RankNode X)
        {
            var Y = X.Right;        // set Y
            X.Right = Y.Left;       //turn Y's left subtree into X's right subtree
            if (Y.Left != null)
            {
                Y.Left.Parent = X;
            }
            if (Y != null)
            {
                Y.Parent = X.Parent;    //link X's parent to Y
            }
            if (X.Parent == null)
            {
                root = Y;
            }
            if (X == X.Parent.Left)
            {
                X.Parent.Left = Y;
            }
            else
            {
                X.Parent.Right = Y;
            }
            Y.Left = X;    //put X on Y's left
            if (X != null)
            {
                X.Parent = Y;
            }

        }
        /// <summary>
        /// Rotate Right
        /// </summary>
        /// <param name="Y"></param>
        /// <returns>void</returns>
        private void RightRotate(RankNode Y)
        {
            // right rotate is simply mirror code from left rotate
            RankNode X = Y.Left;
            Y.Left = X.Right;
            if (X.Right != null)
            {
                X.Right.Parent = Y;
            }
            if (X != null)
            {
                X.Parent = Y.Parent;
            }
            if (Y.Parent == null)
            {
                root = X;
            }
            if (Y == Y.Parent?.Right)
            {
                Y.Parent.Right = X;
            }
            if (Y == Y.Parent?.Left)
            {
                Y.Parent.Left = X;
            }

            X.Right = Y;    //put Y on X's right
            if (Y != null)
            {
                Y.Parent = X;
            }
        }
        /// <summary>
        /// Display Tree
        /// </summary>
        public void DisplayTree()
        {
            Console.WriteLine("Display Tree...");
            if (root == null)
            {
                Console.WriteLine("Nothing in the tree!");
                return;
            }
            if (root != null)
            {
                InOrderDisplay(root);
            }
            Console.WriteLine("------------------------------------------------------");
        }
        /// <summary>
        /// Find item in the tree
        /// </summary>
        /// <param name="key"></param>
        public RankNode Find(int key)
        {
            bool isFound = false;
            RankNode temp = root;
            RankNode item = null;
            while (!isFound)
            {
                if (temp == null)
                {
                    break;
                }
                if (key < temp.Data)
                {
                    temp = temp.Left;
                }
                if (key > temp.Data)
                {
                    temp = temp.Right;
                }
                if (key == temp.Data)
                {
                    isFound = true;
                    item = temp;
                }
            }
            if (isFound)
            {
                Console.WriteLine("{0} was found", key);
                return temp;
            }
            else
            {
                Console.WriteLine("{0} not found", key);
                return null;
            }
        }
        /// <summary>
        /// Insert a new object into the RB Tree
        /// </summary>
        /// <param name="item"></param>
        public void Insert(IDetail value)
        {
            var newItem = new RankNode(value, value.Weight);
            if (root == null)
            {
                root = newItem;
                root.Colour = Color.Black;
                return;
            }
            RankNode Y = null;
            RankNode X = root;
            while (X != null)
            {
                Y = X;
                if (newItem.Data < X.Data)
                {
                    X = X.Left;
                }
                else
                {
                    X = X.Right;
                }
            }
            newItem.Parent = Y;
            if (Y == null)
            {
                root = newItem;
            }
            else if (newItem.Data < Y.Data)
            {
                Y.Left = newItem;
            }
            else
            {
                Y.Right = newItem;
            }
            newItem.Left = null;
            newItem.Right = null;
            newItem.Colour = Color.Red;    //colour the new node red
            InsertFixUp(newItem);    //call method to check for violations and fix
        }
        private void InOrderDisplay(RankNode current)
        {
            if (current != null)
            {
                InOrderDisplay(current.Left);
                Console.Write("({0}) ", current.Data);
                InOrderDisplay(current.Right);
            }
        }
        private void InsertFixUp(RankNode item)
        {
            //Checks Red-Black Tree properties
            while (item != root && item.Parent.Colour == Color.Red)
            {
                /*We have a violation*/
                if (item.Parent == item.Parent.Parent.Left)
                {
                    RankNode Y = item.Parent.Parent.Right;
                    if (Y != null && Y.Colour == Color.Red)    //Case 1: uncle is red
                    {
                        item.Parent.Colour = Color.Black;
                        Y.Colour = Color.Black;
                        item.Parent.Parent.Colour = Color.Red;
                        item = item.Parent.Parent;
                    }
                    else //Case 2: uncle is black
                    {
                        if (item == item.Parent.Right)
                        {
                            item = item.Parent;
                            LeftRotate(item);
                        }
                        //Case 3: recolour & rotate
                        item.Parent.Colour = Color.Black;
                        item.Parent.Parent.Colour = Color.Red;
                        RightRotate(item.Parent.Parent);
                    }

                }
                else
                {
                    //mirror image of code above
                    RankNode X = item.Parent.Parent.Left;
                    if (X != null && X.Colour == Color.Black)    //Case 1
                    {
                        item.Parent.Colour = Color.Red;
                        X.Colour = Color.Red;
                        item.Parent.Parent.Colour = Color.Black;
                        item = item.Parent.Parent;
                    }
                    else //Case 2
                    {
                        if (item == item.Parent.Left)
                        {
                            item = item.Parent;
                            RightRotate(item);
                        }
                        //Case 3: recolour & rotate
                        item.Parent.Colour = Color.Black;
                        item.Parent.Parent.Colour = Color.Red;
                        LeftRotate(item.Parent.Parent);

                    }

                }
                root.Colour = Color.Black;    //re-colour the root black as necessary
            }
        }
        /// <summary>
        /// Deletes a specified value from the tree
        /// </summary>
        /// <param name="item"></param>
        public void Delete(int key)
        {
            //first find the node in the tree to delete and assign to item pointer/reference
            RankNode item = Find(key);
            RankNode X = null;
            RankNode Y = null;

            if (item == null)
            {
                Console.WriteLine("Nothing to delete!");
                return;
            }
            if (item.Left == null || item.Right == null)
            {
                Y = item;
            }
            else
            {
                Y = TreeSuccessor(item);
            }
            if (Y.Left != null)
            {
                X = Y.Left;
            }
            else
            {
                X = Y.Right;
            }
            if (X != null)
            {
                X.Parent = Y;
            }
            if (Y.Parent == null)
            {
                root = X;
            }
            else if (Y == Y.Parent.Left)
            {
                Y.Parent.Left = X;
            }
            else
            {
                Y.Parent.Left = X;
            }
            if (Y != item)
            {
                item.Data = Y.Data;
            }
            if (Y.Colour == Color.Black)
            {
                DeleteFixUp(X);
            }

        }
        /// <summary>
        /// Checks the tree for any violations after deletion and performs a fix
        /// </summary>
        /// <param name="X"></param>
        private void DeleteFixUp(RankNode X)
        {

            while (X != null && X != root && X.Colour == Color.Black)
            {
                if (X == X.Parent.Left)
                {
                    RankNode W = X.Parent.Right;
                    if (W.Colour == Color.Red)
                    {
                        W.Colour = Color.Black;    //case 1
                        X.Parent.Colour = Color.Red;    //case 1
                        LeftRotate(X.Parent);    //case 1
                        W = X.Parent.Right;    //case 1
                    }
                    if (W.Left.Colour == Color.Black && W.Right.Colour == Color.Black)
                    {
                        W.Colour = Color.Red;    //case 2
                        X = X.Parent;    //case 2
                    }
                    else if (W.Right.Colour == Color.Black)
                    {
                        W.Left.Colour = Color.Black;    //case 3
                        W.Colour = Color.Red;    //case 3
                        RightRotate(W);    //case 3
                        W = X.Parent.Right;    //case 3
                    }
                    W.Colour = X.Parent.Colour;    //case 4
                    X.Parent.Colour = Color.Black;    //case 4
                    W.Right.Colour = Color.Black;    //case 4
                    LeftRotate(X.Parent);    //case 4
                    X = root;    //case 4
                }
                else //mirror code from above with "right" & "left" exchanged
                {
                    RankNode W = X.Parent.Left;
                    if (W.Colour == Color.Red)
                    {
                        W.Colour = Color.Black;
                        X.Parent.Colour = Color.Red;
                        RightRotate(X.Parent);
                        W = X.Parent.Left;
                    }
                    if (W.Right.Colour == Color.Black && W.Left.Colour == Color.Black)
                    {
                        W.Colour = Color.Black;
                        X = X.Parent;
                    }
                    else if (W.Left.Colour == Color.Black)
                    {
                        W.Right.Colour = Color.Black;
                        W.Colour = Color.Red;
                        LeftRotate(W);
                        W = X.Parent.Left;
                    }
                    W.Colour = X.Parent.Colour;
                    X.Parent.Colour = Color.Black;
                    W.Left.Colour = Color.Black;
                    RightRotate(X.Parent);
                    X = root;
                }
            }
            if (X != null)
                X.Colour = Color.Black;
        }
        private RankNode Minimum(RankNode X)
        {
            while (X.Left.Left != null)
            {
                X = X.Left;
            }
            if (X.Left.Right != null)
            {
                X = X.Left.Right;
            }
            return X;
        }
        private RankNode TreeSuccessor(RankNode X)
        {
            if (X.Left != null)
            {
                return Minimum(X);
            }
            else
            {
                RankNode Y = X.Parent;
                while (Y != null && X == Y.Right)
                {
                    X = Y;
                    Y = Y.Parent;
                }
                return Y;
            }
        }
    }
}
