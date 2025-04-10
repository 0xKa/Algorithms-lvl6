using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

/// AVL Tree is a Balanced BST ///

/// AVL Tree Time Complexity (insert, delete, search):
/// All Cases: O(log n)

public class AVLNode<T>
{
    public T Value { get; set; }
    public AVLNode<T> Left { get; set; }
    public AVLNode<T> Right { get; set; }
    public int Height { get; set; }

    public AVLNode(T value)
    {
        this.Value = value;
        this.Height = 1;
    }


}

public class AVLTree<T> : BinaryTree<T> where T : IComparable<T>
{
    private new AVLNode<T> Root;

    
    public override void Insert(BinaryTreeNode<T> node)
    {
        this.Insert(node.Value);
    }
    public void Insert(AVLNode<T> node)
    {
        this.Insert(node.Value);
    }
    public override void Insert(T Value)
    {
        Root = Insert(Root, Value);
    }

    private AVLNode<T> Insert(AVLNode<T> node, T value)
    {
        if (node == null) 
            return new AVLNode<T>(value);

        if (value.CompareTo(node.Value) < 0)
            node.Left = Insert(node.Left, value);
        else if (value.CompareTo(node.Value) > 0)
            node.Right = Insert(node.Right, value);
        else
            return node;

        UpdateHeight(node);

        return Balance(node);
    }

    private void UpdateHeight(AVLNode<T> node) => 
        node.Height = 1 + Math.Max(Height(node.Left), Height(node.Right));
    
    private int Height(AVLNode<T> node) => 
        (node != null) ? node.Height : 0;

    private int GetBalanceFactor(AVLNode<T> node) =>
        (node != null) ? Height(node.Left) - Height(node.Right) : 0;

    private AVLNode<T> Balance(AVLNode<T> node)
    {
        int BalanceFactor = GetBalanceFactor(node);

        //decide which rotation to use

        // Right Rotation (RR) Case: Parent BF = -2, Right Child BF= -1 or 0
        if (BalanceFactor < -1 && GetBalanceFactor(node.Right) <= 0)
            return LeftRotate(node);
        
        // Left Rotation (LL) Case: Parent BF = 2, Left Child BF= 1 or 0
        if (BalanceFactor > 1 && GetBalanceFactor(node.Left) >= 0)
            return RightRotate(node);

        // Left-Right Rotation (LR) Case: Parent BF = 2, Left Child BF= -1
        if (BalanceFactor > 1 && GetBalanceFactor(node.Left) < 0)
        {
            node.Left = LeftRotate(node.Left);

            return RightRotate(node);
        }


        // Right-Left Rotation (RL) Case: Parent BF = -2, Right Child BF= 1
        if (BalanceFactor < -1 && GetBalanceFactor(node.Right) > 0)
        {
            node.Right = RightRotate(node.Right);

            return LeftRotate(node);
        }
        return node;
    }


    private AVLNode<T> RightRotate(AVLNode<T> OriginalRoot)
    {
        AVLNode<T> NewRoot  = OriginalRoot.Left;
        AVLNode<T> OriginalRightChild  = NewRoot.Right;

        NewRoot.Right = OriginalRoot;
        OriginalRoot.Left = OriginalRightChild;

        UpdateHeight(OriginalRoot);
        UpdateHeight(NewRoot);
        return NewRoot;
    }
    private AVLNode<T> LeftRotate(AVLNode<T> OriginalRoot)
    {
        AVLNode<T> NewRoot = OriginalRoot.Right;
        AVLNode<T> OriginalLeftChild = NewRoot.Left;

        NewRoot.Left = OriginalRoot;
        OriginalRoot.Right = OriginalLeftChild;

        UpdateHeight(OriginalRoot);
        UpdateHeight(NewRoot);
        return NewRoot;
    }


    public void Delete(T value)
    {
        Root = DeleteNode(Root,  value);
    }
    public AVLNode<T> DeleteNode(AVLNode<T> Node, T value)
    {
        if (Node == null) return Node;

        //to delete nodes in AVL tree, we perform standerd BST tree deletion then we Balance() the tree
        if (value.CompareTo(Node.Value) < 0)
            Node.Left = DeleteNode(Node.Left, value);
        else if (value.CompareTo(Node.Value) > 0)
            Node.Right = DeleteNode(Node.Right, value);
        else
        {


            // Case 2: Node has one child
            if (Node.Left == null)
                return Node.Right;
            else if (Node.Right == null)
                return Node.Left;

            // Case 3: Node has two children
            AVLNode<T> temp = FindMinValueChild(Node.Right);
            Node.Value = temp.Value;
            Node.Right = DeleteNode(Node.Right, temp.Value);

        }

        UpdateHeight(Node);
        return Balance(Node);
    }
    private AVLNode<T> FindMinValueChild(AVLNode<T> node)
    {
        AVLNode<T> current = node;

        // Go down to the leftmost leaf
        while (current.Left != null)
        {
            current = current.Left;
        }

        return current;
    }

    public AVLNode<T> Search(T value)
    {
        return Search(Root, value);
    }
    private AVLNode<T> Search(AVLNode<T> node, T value)
    {
        if (node == null)
            return null; 

        if (value.CompareTo(node.Value) < 0)
            return Search(node.Left, value); 
        else if (value.CompareTo(node.Value) > 0)
            return Search(node.Right, value);
        else
            return node; 
    }


    public bool Exists(T value)
    {
        return Exists(Root, value);
    }
    private bool Exists(AVLNode<T> node, T value)
    {
        if (node == null)
            return false;

        if (value.CompareTo(node.Value) < 0)
            return Exists(node.Left, value);
        else if (value.CompareTo(node.Value) > 0)
            return Exists(node.Right, value);
        else
            return true;
    }


    public override void PrintStructure()
    {
        Console.WriteLine("\nAVL Tree Structure:");

        if (Root == null)
        {
            Console.WriteLine("Tree is empty.");
            return;
        }

        Queue<AVLNode<T>> queue = new Queue<AVLNode<T>>();
        queue.Enqueue(Root);
        int level = 0;

        while (queue.Count > 0)
        {
            int levelSize = queue.Count;
            Console.Write("Level " + level + ": ");

            for (int i = 0; i < levelSize; i++)
            {
                AVLNode<T> current = queue.Dequeue();
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

    public override void PrintTree()
    {
        Console.WriteLine();
        PrintTree(this.Root, "", true);
    }
    private void PrintTree(AVLNode<T> node, string indent, bool last)
    {
        if (node != null)
        {
            Console.Write(indent);
            if (last)
            {
                Console.Write("R----");
                indent += "     ";
            }
            else
            {
                Console.Write("L----");
                indent += "|    ";
            }
            Console.WriteLine(node.Value);
            PrintTree(node.Left, indent, false);
            PrintTree(node.Right, indent, true);
        }
    }


    //Auto Complete feature for strings 
    public List<T> AutoComplete(T prefix)
    {
        List<T> results = new List<T>();
        AutoComplete(Root, prefix, results);
        return results;
    }
    private void AutoComplete(AVLNode<T> node, T prefix, List<T> results)
    {
        if (node != null)
        {
            if (node.Value.ToString().StartsWith(prefix.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                results.Add(node.Value);
                AutoComplete(node.Left, prefix, results);
                AutoComplete(node.Right, prefix, results);
            }
            else
            {
                // If the current node's value does not start with the prefix,
                // the method decides which subtree to explore next based on alphabetical order:
                // If the prefix is lexicographically smaller than the node's value, it recurses into the left subtree, as any potential matches must be in the left due to the properties of the binary search tree (BST).
                // Conversely, if the prefix is larger, it searches the right subtree.

                if (string.Compare(prefix.ToString(), node.Value.ToString(), StringComparison.OrdinalIgnoreCase) < 0)
                    AutoComplete(node.Left, prefix, results);
                else
                    AutoComplete(node.Right, prefix, results);
            }
        }
    }
}

internal class Program
{
    public static void Test_Operations()
    {
        AVLTree<int> avlTree = new AVLTree<int>();

        //RR
        int[] values1 = { 10, 20, 30 };

        //LL
        int[] values2 = { 30, 20, 10 };

        //LR
        int[] values3 = { 30, 10, 20 };

        //RL
        int[] values4 = { 10, 30, 20 };

        //
        int[] values = { 10, 20, 30, 40, 50, 25, 432, 43, 5, 1, 75 };

        foreach (int value in values)
        {
            Console.WriteLine($"Inserting {value} into the AVL tree...");
            avlTree.Insert(value);

            avlTree.PrintStructure();
            avlTree.PrintTree();

            Console.WriteLine("--------------------------------------");

        }


        Console.WriteLine("Deleteing Value: [50]");
        avlTree.Delete(50);
        avlTree.PrintStructure();
        avlTree.PrintTree();


        Console.WriteLine("\nIs Exists -> Value: [50]");
        Console.WriteLine("Exists() Result: " + avlTree.Exists(50));

        Console.WriteLine("\nSearching for Value: [50]");
        Console.WriteLine("Search() Result: " + avlTree.Search(50));


        Console.WriteLine("\nIs Exists -> Value: [20]");
        Console.WriteLine("Exists() Result: " + avlTree.Exists(20));

        Console.WriteLine("\nSearching for Value: [20]");
        Console.WriteLine("Search() Result: " + avlTree.Search(20).Value);
    }

    public static void Test_AutoCompleteFeature()
    {
        AVLTree<string> avlTree = new AVLTree<string>();
        string[] names = { "Reda", "Ahmed", "Khalid", "Salim", "Fahed", "Mohammed", "Omar", "Ali", "Issa", "Yousif", "Abdullah", "Marwan", "Nasser" };

        foreach (string name in names)
        {
            avlTree.Insert(name);
        }

        avlTree.PrintTree();


        Console.WriteLine("\nEnter a prefix to search:");
        string prefix = Console.ReadLine();
 
        var completions = avlTree.AutoComplete(prefix);
        Console.WriteLine($"\nSuggestions for '{prefix}':\n");
        foreach (var completion in completions)
        {
            Console.WriteLine(completion);
        }


    }
    static void Main(string[] args)
    {
        //Test_Operations();
        Test_AutoCompleteFeature();


    }
}

/* AVL Tree 
Introduction to AVL Trees
    AVL trees, named after their inventors Adelson-Velsky and Landis, are self-balancing binary search trees. In an AVL tree, the heights of the two child subtrees of any node differ by no more than one. If at any time they differ by more than one, rebalancing is done to restore this property.

Key Concepts
It's important to understand some key concepts:
    Balance Factor: The difference between the heights of the left and right subtrees. It helps in deciding whether a subtree needs rebalancing.
    Height of a Tree: The height of a node is the number of edges on the longest downward path between that node and a leaf.
    Balance Factor = Difference between Hight(Left Subtree) - Hight(Right Subtree)
    If Abs(BF) > 1 then tree is not balanced, otherwise it is balanced.
 */
