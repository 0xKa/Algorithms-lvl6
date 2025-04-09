using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

/// Binary Search Tree Time Complexity (insert, delete, search):
/// Best/Avg Case: O(log n)
/// Worst Case: O(n), for degenerate trees

//this class inherits my BinaryTree<T> class and Implement IComparable interface
public class BinarySearchTree<T> : BinaryTree<T> where T : IComparable<T> //“Only allow types T that implement the IComparable<T> interface.”
{
    public override void Insert(BinaryTreeNode<T> NewNode)
    {
        this.Insert(NewNode.Value);
    }
    public override void Insert(T Value)
    {
        this.Root = this.Insert(Root, Value);
    }
    private BinaryTreeNode<T> Insert(BinaryTreeNode<T> node, T value)
    {
        if (node == null) 
            return new BinaryTreeNode<T>(value);
        
        else if (value.CompareTo(node.Value) < 0)
            node.Left = Insert(node.Left, value);
        
        else if (value.CompareTo(node.Value) > 0)
            node.Right = Insert(node.Right, value);
            
        return node;

    }


    public bool Search(T Value)
    {
        return Search(Root, Value) != null;
    }
    public BinaryTreeNode<T> Search(BinaryTreeNode<T> node, T value)
    {
        if (node == null || node.Value.Equals(value))
            return node;
        else if (value.CompareTo(node.Value) < 0)
            return Search(node.Left, value);
        else
            return Search(node.Right, value);


    }

}

internal class Program
{
    static void Main(string[] args)
    {
        //note: the problem with BST is that we need to Insert values in a balance order, else it will results a degenerate tree with a O(n)

        BinarySearchTree<int> bst = new BinarySearchTree<int>();

        bst.Insert(new BinaryTreeNode<int>(45));
        bst.Insert(15);
        bst.Insert(79);
        bst.Insert(10);
        bst.Insert(20);
        bst.Insert(55);
        bst.Insert(90);
        bst.Insert(12);
        bst.Insert(50);
        bst.Insert(22);
        bst.Insert(05);


        bst.PrintStructure();
        bst.PrintTree();


        Console.WriteLine("\nSearching for Value 55...");
        Console.WriteLine($"Found Result: {bst.Search(55)}");

        Console.WriteLine("\nSearching for Value 99...");
        Console.WriteLine($"Found Result: {bst.Search(99)}");

        Console.WriteLine("\nSearching for Value 05...");
        Console.WriteLine($"Found Result: {bst.Search(05)}");



        bst.PreOrderTraversal();
        bst.PostOrderTraversal();
        bst.InOrderTraversal(); //this will sort the BST
        bst.LevelOrderTraversal(); 

    }
}

/* Binary Search Tree (BST) 
A Binary Search Tree (BST) is a fundamental data structure in computer science that organizes elements in a sorted manner for efficient searching, insertion, and deletion operations. It is a binary tree where each node has at most two children, referred to as the left child and the right child, and it satisfies the binary search property.

Binary Search Property
The key feature of a BST is its binary search property, which stipulates that:

    For any node n, all elements in the left subtree of n are less than n.
    All elements in the right subtree of n are greater than n.
    This property ensures that the tree remains balanced in terms of its depth, which in turn guarantees operations such as search, insertion, and deletion can be performed in logarithmic time complexity (O(log n)) under ideal conditions.



Binary search tree: is a non-linear data structure in which one node is connected to n number of nodes.

It is a node-based data structure. A node can be represented in a binary search tree with three fields, i.e., data part, left-child, and right-child. A node can be connected to the utmost two child nodes in a binary search tree, so the node contains two pointers (left child and right child pointer).

Every node in the left subtree must contain a value less than the value of the root node, and the value of each node in the right subtree must be bigger than the value of the root node.



Applications of BST
BSTs are widely used in computer science and applications such as:
    Implementing associative arrays, sets, and multisets.
    Database indices for quick data retrieval.
    Autocomplete features where a prefix search is performed.
    Sorting algorithms.


Advantages of BST
    Efficient Operations: Offers O(log n) search, insertion, and deletion operations in the best and average cases.
    Sorted Data: Maintains data in a sorted order, facilitating operations like minimum, maximum, successor, predecessor, etc., in O(h) time.

Disadvantages of BST
    Worst-Case Performance: In the worst case (e.g., inserting sorted data), the BST can become unbalanced, resembling a linked list with O(n) time complexity for operations.
    Maintenance: Requires additional logic (e.g., tree balancing techniques) to maintain optimal performance.

Conclusion
    Binary Search Trees are a versatile and efficient way to store data for quick search, insertion, and deletion. Understanding how to implement and manipulate BSTs is a valuable skill in computer science, applicable to a wide range of problems and technologies.
 */