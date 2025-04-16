using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


public class RedBlackTree
{
    public class Node
    {
        public int Value;
        public Node Left, Right, Parent;
        public bool IsRed = true;

        public Node(int value) => this.Value = value;
    
    }

    private Node Root;

    public void Insert(int newValue)
    {
        Node newNode = new Node(newValue);

        // handle 1st case (empty tree)
        if (Root == null)
        {
            Root = newNode;
            Root.IsRed = false; 
            return;
        }


        // standard BST insertion
        Node current = Root;
        Node parent = null;
        while (current != null)
        {
            parent = current;
            if (newValue < current.Value)
                current = current.Left;
            else
                current = current.Right;
        }


        // Set the parent of the new node
        newNode.Parent = parent;
        if (newValue < parent.Value)
            parent.Left = newNode;
        else
            parent.Right = newNode;


        _FixInsert(newNode); //restore RBT properties
    }

    private void _FixInsert(Node node)
    {
        Node parent = null;
        Node grandparent = null;


        while ((node != Root) && (node.IsRed) && (node.Parent.IsRed))
        {
            parent = node.Parent;
            grandparent = parent.Parent;


            if (parent == grandparent.Left)
            {
                Node uncle = grandparent.Right;


                // 2nd case: the uncle was red
                if (uncle != null && uncle.IsRed)
                {
                    grandparent.IsRed = true; 
                    parent.IsRed = false;
                    uncle.IsRed = false; 
                    node = grandparent; 
                }
                else
                {
                    // 3rd case: node is right child of its parent (trinagle case)
                    if (node == parent.Right)
                    {
                        RotateLeft(parent);
                        node = parent;
                        parent = node.Parent;
                    }

                    // 4th case: node is left child of its parent (line case)
                    RotateRight(grandparent);

                    (parent.IsRed, grandparent.IsRed) = (grandparent.IsRed, parent.IsRed); //swap colors using tuples
                }
            }
            else // Parent node is a right child
            {
                Node uncle = grandparent.Left; 


                if (uncle != null && uncle.IsRed)
                {
                    grandparent.IsRed = true; 
                    parent.IsRed = false; 
                    uncle.IsRed = false; 
                    node = grandparent; 
                }
                else
                {
                    if (node == parent.Left)
                    {
                        RotateRight(parent);
                        node = parent;
                        parent = node.Parent;
                    }


                    RotateLeft(grandparent);

                    (parent.IsRed, grandparent.IsRed) = (grandparent.IsRed, parent.IsRed); //swap colors using tuples
                }
            }
        }


        Root.IsRed = false; 
    }


    private void RotateLeft(Node node)
    {
        Node right = node.Right; 
        node.Right = right.Left; 
        if (node.Right != null)
            node.Right.Parent = node; 


        right.Parent = node.Parent;


        if (node.Parent == null)
            Root = right; 
        else if (node == node.Parent.Left)
            node.Parent.Left = right; 
        else
            node.Parent.Right = right; 


        right.Left = node; 
        node.Parent = right; 
    }
    private void RotateRight(Node node)
    {
        Node left = node.Left;
        node.Left = left.Right; 
        if (node.Left != null)
            node.Left.Parent = node; 


        left.Parent = node.Parent; 


        if (node.Parent == null)
            Root = left; 
        else if (node == node.Parent.Right)
            node.Parent.Right = left; 
        else
            node.Parent.Left = left; 


        left.Right = node;
        node.Parent = left; 
    }

    public Node Find(int value)
    {
        return Find(Root, value);
    }
    private Node Find(Node node, int value)
    {
        if (node == null || value == node.Value)
            return node;

        if (value < node.Value)
            return Find(node.Left, value);
        else
            return Find(node.Right, value);
    }


    public void PrintTree()
    {
        _PrintHelper(Root, "", true);
    }
    private void _PrintHelper(Node node, string indent, bool last)
    {
        if (node != null)
        {
            Console.Write(indent);
            if (last)
            {
                Console.Write("R----");
                indent += "   ";
            }
            else
            {
                Console.Write("L----");
                indent += "|  ";
            }
            var color = node.IsRed ? "RED" : "BLK";
            Console.WriteLine(node.Value + "(" + color + ")");
            _PrintHelper(node.Left, indent, false);
            _PrintHelper(node.Right, indent, true);
        }
    }
}

internal class Program
{
    static void Main(string[] args)
    {
        int[] nums = { 22, 50, 99, 30, 13, 32, 64, 90, 1 };
        RedBlackTree tree = new RedBlackTree();

        foreach (int num in nums)
        {
            tree.Insert(num);
        }

        tree.PrintTree();

        RedBlackTree.Node node = tree.Find(32);
        Console.WriteLine("\nNode Value: " + node.Value);

    }
}