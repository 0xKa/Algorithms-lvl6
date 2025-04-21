using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

/* AVL vs RBT Conclusion:
For Lookup Efficiency: 
    Choose AVL Trees if the application involves frequent searches and lookup operations and the cost of rebalancing the tree for insertions and deletions is less critical.

For Insert/Remove Efficiency: 
    Choose Red-Black Trees if the application requires frequent insertions and deletions and can tolerate slightly longer search times for the benefit of faster updates.
 */

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
        return _Find(Root, value);
    }
    private Node _Find(Node node, int value)
    {
        if (node == null || value == node.Value)
            return node;

        if (value < node.Value)
            return _Find(node.Left, value);
        else
            return _Find(node.Right, value);
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


    // Public method to delete a value from the tree
    public bool Delete(int value)
    {
        Node nodeToDelete = _Find(Root, value);
        if (nodeToDelete == null)
            return false; // Node to delete not found

        _DeleteNode(nodeToDelete);
        return true;
    }
    // Helper method to delete a node and maintain Red-Black properties
    private void _DeleteNode(Node nodeToDelete)
    {
        Node nodeToFix = null;  // Node that may require fixing the Red-Black properties
        Node child = null;      // Child of the node to delete or its successor
        Node parent = null;     // Parent of the node to delete

        bool originalColor = nodeToDelete.IsRed;  // Store the original color of the node to delete

        // Case 1: The node to delete has no left child
        if (nodeToDelete.Left == null)
        {
            child = nodeToDelete.Right;  // The child is the right child of the node
            _Transplant(nodeToDelete, child);  // Replace nodeToDelete with its right child
        }

        // Case 2: The node to delete has no right child
        else if (nodeToDelete.Right == null)
        {
            child = nodeToDelete.Left;  // The child is the left child of the node
            _Transplant(nodeToDelete, child);  // Replace nodeToDelete with its left child
        }
        // Case 3: The node to delete has both left and right children
        else
        {
            // Find the in-order successor (smallest node in the right subtree)
            Node successor = _Minimum(nodeToDelete.Right);
            originalColor = successor.IsRed;  // Store the original color of the successor
            child = successor.Right;  // The child is the right child of the successor

            // If the successor is the immediate child of the node to delete
            if (successor.Parent == nodeToDelete)
            {
                if (child != null)
                    child.Parent = successor;  // Update the parent of the child
            }
            else
            {
                // Replace the successor with its right child in its original position
                _Transplant(successor, successor.Right);
                successor.Right = nodeToDelete.Right;  // Connect the right child of the node to delete to the successor
                successor.Right.Parent = successor;  // Update the parent of the right child
            }

            // Replace the node to delete with the successor
            _Transplant(nodeToDelete, successor);
            successor.Left = nodeToDelete.Left;  // Connect the left child of the node to delete to the successor
            successor.Left.Parent = successor;  // Update the parent of the left child
            successor.IsRed = nodeToDelete.IsRed;  // Maintain the original color of the node being deleted
        }

        // If the original color of the node was black, fix the Red-Black properties
        if (!originalColor && child != null)
        {
            _FixDelete(child);  // Call the fix-up method to maintain Red-Black properties
        }
    }
    // _Transplant replaces one subtree as a child of its parent with another subtree
    private void _Transplant(Node target, Node with)
    {
        // If the target node is the Root of the tree (i.e., it has no parent),
        // then the new subtree (with) becomes the new Root of the tree.
        if (target.Parent == null)
            Root = with;

        // If the target node is the left child of its parent,
        // then update the parent's left child to be the new subtree (with).
        else if (target == target.Parent.Left)
            target.Parent.Left = with;
        // If the target node is the right child of its parent,
        // then update the parent's right child to be the new subtree (with).
        else
            target.Parent.Right = with;

        // If the new subtree (with) is not null, 
        // update its parent to be the parent of the target node.
        if (with != null)
            with.Parent = target.Parent;
    }
    // Method to fix Red-Black properties after deletion
    private void _FixDelete(Node node)
    {
        // Loop until the node is the Root or until the node is red
        while (node != Root && !node.IsRed)
        {
            // If the node is the left child of its parent
            if (node == node.Parent.Left)
            {
                // Get the sibling of the node
                Node sibling = node.Parent.Right;

                // Case 1: If the sibling is red, perform a rotation and recolor
                if (sibling.IsRed)
                {
                    sibling.IsRed = false; // Recolor sibling to black
                    node.Parent.IsRed = true; // Recolor parent to red
                    RotateLeft(node.Parent); // Rotate the parent to the left
                    sibling = node.Parent.Right; // Update sibling after rotation
                }

                // Case 2.1: If both of sibling's children are black
                if (!sibling.Left.IsRed && !sibling.Right.IsRed)
                {
                    sibling.IsRed = true; // Recolor sibling to red
                    node = node.Parent; // Move up the tree to continue fixing
                }
                else
                {
                    // Case 2.2.2: If sibling's right child  is black and left child is red (Near child Red)
                    if (!sibling.Right.IsRed)
                    {
                        sibling.Left.IsRed = false; // Recolor sibling's left child to black
                        sibling.IsRed = true; // Recolor sibling to red
                        RotateRight(sibling); // Rotate sibling to the right
                        sibling = node.Parent.Right; // Update sibling after rotation
                    }

                    // Case 2.2.1: Sibling's right child is red (Far child Red)
                    sibling.IsRed = node.Parent.IsRed; // Recolor sibling with parent's color
                    node.Parent.IsRed = false; // Recolor parent to black
                    sibling.Right.IsRed = false; // Recolor sibling's right child to black
                    RotateLeft(node.Parent); // Rotate parent to the left
                    node = Root; // Set node to Root to break out of the loop
                }
            }
            else // If the node is the right child of its parent
            {
                // Get the sibling of the node
                Node sibling = node.Parent.Left;

                // Case 1: If the sibling is red, perform a rotation and recolor
                if (sibling.IsRed)
                {
                    sibling.IsRed = false; // Recolor sibling to black
                    node.Parent.IsRed = true; // Recolor parent to red
                    RotateRight(node.Parent); // Rotate the parent to the right
                    sibling = node.Parent.Left; // Update sibling after rotation
                }

                // Case 2.1: If both of sibling's children are black
                if (!sibling.Left.IsRed && !sibling.Right.IsRed)
                {
                    sibling.IsRed = true; // Recolor sibling to red
                    node = node.Parent; // Move up the tree to continue fixing
                }
                else
                {
                    // Case 2.2.2: If sibling's left child is black and right child is red (Near Child is Red)
                    if (!sibling.Left.IsRed)
                    {
                        sibling.Right.IsRed = false; // Recolor sibling's right child to black
                        sibling.IsRed = true; // Recolor sibling to red
                        RotateLeft(sibling); // Rotate sibling to the left
                        sibling = node.Parent.Left; // Update sibling after rotation
                    }

                    // Case 2.2.1: Sibling's left child is red (Far Child is Red)
                    sibling.IsRed = node.Parent.IsRed; // Recolor sibling with parent's color
                    node.Parent.IsRed = false; // Recolor parent to black
                    sibling.Left.IsRed = false; // Recolor sibling's left child to black
                    RotateRight(node.Parent); // Rotate parent to the right
                    node = Root; // Set node to Root to break out of the loop
                }
            }
        }
        node.IsRed = false; // Ensure the node is black before exiting
    }
    // Helper method to find the minimum value node in the tree
    private Node _Minimum(Node node)
    {
        // Traverse down the left subtree until the leftmost node is reached
        while (node.Left != null)
            node = node.Left; // Move to the left child

        // Return the leftmost (minimum value) node
        return node;
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

        Console.WriteLine("\nFinding Node 32...");
        RedBlackTree.Node node1 = tree.Find(32);
        Console.WriteLine("Node Value: " + node1.Value + ", Color: " + (node1.IsRed ? "Red" : "Black"));
        
        Console.WriteLine("\nFinding Node 13...");
        RedBlackTree.Node node2 = tree.Find(13);
        Console.WriteLine("Node Value: " + node2.Value + ", Color: " + (node2.IsRed ? "Red" : "Black"));


        tree.Delete(13);
        Console.WriteLine("\nDeleting Node 13...\n");
        tree.PrintTree();

    }
}

/*Red Black Tree Deletion 
Fixing Violations During Deletion in Red-Black Trees
When a black node is deleted in a Red-Black tree, a "double black" situation may arise, which violates the tree's properties. The resolution strategy depends on the color of the sibling node and its children.

Case 1: Sibling is Red
Action:

When you delete the node, it becomes double black.
Perform a rotation to move the red sibling to the parent's position.
Swap the colors of the sibling and the parent (color the sibling black and the parent red).
The double black situation still exists, but now the sibling of the double black node is black, allowing you to continue with the appropriate steps for Case 2.
Case 2: Sibling is Black
Sub-case 2.1: Sibling's Children Are Both Black
Action:

When you delete the node, it becomes double black.
Color the sibling red.
Move the double black up to the parent (effectively reducing the problem to the parent).
If the parent is red, color it black to resolve the double black.
If the parent is black, the double black situation may continue, requiring further handling up the tree.
Sub-case 2.2: At Least One of the Sibling's Children is Red
Sub-sub-case 2.2.1: Sibling's Far Child is Red
Action:

When you delete the node, it becomes double black.
Perform a rotation on the parent and sibling (if the sibling is a right child, perform a left rotation; if the sibling is a left child, perform a right rotation).
Color the far child black.
Color the original sibling with the parent's original color.
Color the parent black.
The double black situation My be resolved and may be not: further steps are required to resolve the double black, potentially involving moving the double black up the tree and applying the rules from Case 2.1 or Case 2.2 again.


Sub-sub-case 2.2.2: Sibling's Near Child is Red
Action:

When you delete the node, it becomes double black.
Perform a rotation on the sibling and its parent (right rotation if the sibling is a left child; left rotation if the sibling is a right child).
Swap the colors of the sibling and its near child.
Now, the sibling is red, turning this into Sub-sub-case 2.2.1.
Follow the actions in Sub-sub-case 2.2.1 to resolve the double black.


Key Points:
The primary goal is to restore the Red-Black properties: maintaining the balancing of the tree and preserving the black height.
Actions taken depend on the relative colors of the sibling, the sibling's children, and the double black node.
The process may involve multiple iterations, propagating the double black situation upward if necessary.
 * 
 */