using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class BinaryTreeNode<T>
{
    public T Value { get; set; }
    public BinaryTreeNode<T> Left { get; set; }
    public BinaryTreeNode<T> Right { get; set; }

    public BinaryTreeNode(T value)
    {
        this.Value = value;
        this.Left = null;
        this.Right = null;
    }
}

public class BinaryTree<T>
{
    public BinaryTreeNode<T> Root { get; set; }

    public BinaryTree()
    {
        this.Root = null;
    }

    /* Level-order Insertion Strategy 
          We use Level-order insertion strategy,
          Level-order insertion: in a binary tree is a strategy that fills the tree level by level, 
          from left to right. This approach ensures that every level of the tree is completely 
          filled before any nodes are added to a new level, 
          and each parent node has at most two children before moving on to the next node in the 
          sequence.
    */
    public virtual void Insert(BinaryTreeNode<T> NewNode)
    {
        if (this.Root == null)
        {
            Root = NewNode;
            return;
        }

        Queue<BinaryTreeNode<T>> nodesQ = new Queue<BinaryTreeNode<T>>();
        nodesQ.Enqueue(Root);

        while (nodesQ.Count > 0)
        {
            BinaryTreeNode<T> current = nodesQ.Dequeue();

            //check left
            if (current.Left == null)
            {
                current.Left = NewNode;
                break;
            }
            else
            {
                nodesQ.Enqueue(current.Left);
            }

            //check right
            if (current.Right == null)
            {
                current.Right = NewNode;
                break;
            }
            else
            {
                nodesQ.Enqueue(current.Right);
            }



        }


    }
    public virtual void Insert(T NewNodeValue)
    {
        this.Insert(new BinaryTreeNode<T>(NewNodeValue));
    }
    /* Insert Method Explaination
     * 
                    A
                   / \
                  B   C
                 / \
                D   E

  - Insert "F"
        - Start at A, check left (B) → Full, move to right (C) → Empty? No, continue.

        - Move to B, check left (D) → Full, check right (E) → Empty? No, continue.

        - Find an empty left/right child at C, insert F.

     */

    /// <summary>
    /// Method to visually print the tree structure
    /// </summary>
    public void PrintStructure()
    {
        Console.WriteLine("\nBinary Tree Structure:");

        if (Root == null)
        {
            Console.WriteLine("Tree is empty.");
            return;
        }

        Queue<BinaryTreeNode<T>> queue = new Queue<BinaryTreeNode<T>>();
        queue.Enqueue(Root);
        int level = 0;

        while (queue.Count > 0)
        {
            int levelSize = queue.Count;
            Console.Write("Level " + level + ": ");

            for (int i = 0; i < levelSize; i++)
            {
                BinaryTreeNode<T> current = queue.Dequeue();
                Console.Write(current.Value + " ");

                if (current.Left != null)
                    queue.Enqueue(current.Left);

                if (current.Right != null)
                    queue.Enqueue(current.Right);
            }
            Console.WriteLine(); // Move to the next level
            level++;
        }
    }

    /// <summary>
    /// Method to visually print the tree visualy
    /// </summary>
    public void PrintTree()
    {
        Console.WriteLine("\nBinary Tree:");
        PrintTree(this.Root, 0);
    }
    private void PrintTree(BinaryTreeNode<T> root, int space)
    {

        int COUNT = 10;  // Distance between levels to adjust the visual representation
        if (root == null)
            return;


        space += COUNT;
        PrintTree(root.Right, space); // Print right subtree first, then root, and left subtree last


        Console.WriteLine();
        for (int i = COUNT; i < space; i++)
            Console.Write(" ");
        Console.WriteLine(root.Value); // Print the current node after space


        PrintTree(root.Left, space); // Recur on the left child
    }

    /// PreOrder Traversal ///
    public void PreOrderTraversal()
    {
        Console.WriteLine("\nPreOrder Traversal: root -> left -> right");
        PreOrderTraversal(Root);
        Console.WriteLine();
    }
    private void PreOrderTraversal(BinaryTreeNode<T> node)
    {
        /*
          PreOrder Traversal visits the current node before its child nodes. 
          The process for PreOrder Traversal is as follows:


             - Visit the current node.
             - Recursively perform PreOrder Traversal of the left subtree.
             - Recursively perform PreOrder Traversal of the right subtree.
        */


        if (node != null)
        {
            Console.Write(node.Value + " ");
            PreOrderTraversal(node.Left);
            PreOrderTraversal(node.Right);
        }
    }


    /// PostOrder Traversal ///
    public void PostOrderTraversal()
    {
        Console.WriteLine("\nPostOrder Traversal: left -> right -> root");
        PostOrderTraversal(Root);
        Console.WriteLine();
    }
    private void PostOrderTraversal(BinaryTreeNode<T> node)
    {

        /*
          PostOrder Traversal visits the current node after its child nodes. 
          The process for PostOrder Traversal is:


            - Recursively perform PostOrder Traversal of the left subtree.
            - Recursively perform PostOrder Traversal of the right subtree.
            - Visit the current node.
       */


        if (node != null)
        {
            PostOrderTraversal(node.Left);
            PostOrderTraversal(node.Right);
            Console.Write(node.Value + " ");
        }
    }


    /// InOrder Traversal ///
    public void InOrderTraversal()
    {
        Console.WriteLine("\nInOrder Traversal: left -> root -> right");
        InOrderTraversal(Root);
        Console.WriteLine();
    }
    private void InOrderTraversal(BinaryTreeNode<T> node)
    {

        /*
          Left - Current - Right
         */
        if (node != null)
        {
            InOrderTraversal(node.Left);
            Console.Write(node.Value + " ");
            InOrderTraversal(node.Right);
        }
    }

   
    /// Level-Order Traversal ///
    public void LevelOrderTraversal()
    {
        Console.WriteLine("\nLevel-Order Traversal: root -> left -> right -> next level");
        LevelOrderTraversal(Root);
        Console.WriteLine();
    }
    private void LevelOrderTraversal(BinaryTreeNode<T> node)
    {
        if (node == null) return;

        Queue<BinaryTreeNode<T>> lvlQ = new Queue<BinaryTreeNode<T>>();
        lvlQ.Enqueue(node);

        while (lvlQ.Count > 0)
        {
            BinaryTreeNode<T> Current = lvlQ.Dequeue();
            Console.Write(Current.Value + " ");

            if (Current.Left != null)
                lvlQ.Enqueue(Current.Left);

            if (Current.Right != null)
                lvlQ.Enqueue(Current.Right);

        }
    }


}

internal class Program
{
    static void Main(string[] args)
    {

        BinaryTreeNode<int> node1 = new BinaryTreeNode<int>(10);

        BinaryTree<int> tree = new BinaryTree<int>();

        tree.Insert(node1);
        tree.Insert(19);
        tree.Insert(91);
        tree.Insert(12);
        tree.Insert(32);
        tree.Insert(13);
        tree.Insert(99);
        tree.Insert(11);
        tree.Insert(39);
        tree.Insert(22);


        tree.PrintStructure();
        tree.PrintTree();


        tree.PreOrderTraversal();
        tree.PostOrderTraversal();
        tree.InOrderTraversal();
        tree.LevelOrderTraversal(); //

    }
}