using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Graph;

/* Difference Between BFS and DFS

Breadth-First Search (BFS): 
    - Explores a graph level by level, visiting all neighbors of a node before moving to the next level.
        It uses a queue to keep track of nodes to visit next. 
    - BFS is ideal for finding the shortest path in an unweighted graph because it 
        examines all nodes at the current depth before moving deeper.

Depth-First Search (DFS): 
    - On the other hand, dives as deep as possible along one path before backtracking to explore 
        other paths. It uses a stack or recursion. 
    - DFS is better suited for problems that require exploring all possible solutions, 
        such as puzzles or finding connected components in a graph.

 */

/* When to Use Each
Use BFS:
    When you need to find the shortest path in an unweighted graph.
    For problems requiring traversal by levels, such as hierarchical structures or games where the shortest move matters.

Use DFS:
    When you need to explore all paths, such as in backtracking problems or detecting cycles in a graph.
    For memory efficiency when the graph is deep but has fewer branches.

 */

/* Which is Faster?
Both BFS and DFS have the same time complexity of O(V+E), 
where V is the number of vertices and E is the number of edges.

However:
    BFS can be faster for finding the shortest path due to its level-order traversal.
    DFS can feel faster for quickly locating a single solution without considering path optimality.
    The choice depends on the problem at hand rather than inherent speed differences.
 */

public class Graph
{
    public enum enGraphDirectionType { Directed, unDirected }

    private int[,] _adjacencyMatrix; 
    private Dictionary<string, int> _vertexDictionary; 
    private int _numberOfVertices; 
    private enGraphDirectionType _GraphDirectionType = enGraphDirectionType.unDirected;

    public Graph(List<string> vertices, enGraphDirectionType GraphDirectionType)
    {
        _GraphDirectionType = GraphDirectionType;
        _numberOfVertices = vertices.Count; 
        _adjacencyMatrix = new int[_numberOfVertices, _numberOfVertices]; 
        _vertexDictionary = new Dictionary<string, int>(); 

        for (int i = 0; i < vertices.Count; i++)
        {
            _vertexDictionary[vertices[i]] = i;
        }
    }

    public void AddEdge(string source, string destination, int weight)
    {
        if (_vertexDictionary.ContainsKey(source) && _vertexDictionary.ContainsKey(destination))
        {
            int sourceIndex = _vertexDictionary[source];
            int destinationIndex = _vertexDictionary[destination];
            _adjacencyMatrix[sourceIndex, destinationIndex] = weight;

            // Add the reverse edge for undirected graphs
            if (_GraphDirectionType == enGraphDirectionType.unDirected)
            {
                _adjacencyMatrix[destinationIndex, sourceIndex] = weight;
            }
        }
        else
        {
            Console.WriteLine($"Invalid vertices: {source} or {destination}");
        }
    }

    public void DisplayGraph(string message)
    {
        Console.WriteLine("\n" + message + "\n");
        Console.Write("  ");
        foreach (var vertex in _vertexDictionary.Keys)
        {
            Console.Write(vertex + " ");
        }
        Console.WriteLine();

        foreach (var source in _vertexDictionary)
        {
            Console.Write(source.Key + " ");
            for (int j = 0; j < _numberOfVertices; j++)
            {
                Console.Write(_adjacencyMatrix[source.Value, j] + " ");
            }
            Console.WriteLine();
        }
    }

    // Perform Breadth-First Search (BFS)
    public void BFS(string startVertex)
    {
        if (!_vertexDictionary.ContainsKey(startVertex))
        {
            Console.WriteLine("Invalid start vertex.");
            return;
        }

        bool[] visited = new bool[_numberOfVertices]; // Keep track of visited vertices

        Queue<int> queue = new Queue<int>(); // Queue for BFS

        int startIndex = _vertexDictionary[startVertex];

        visited[startIndex] = true; // Mark start vertex as visited
        queue.Enqueue(startIndex);

        Console.WriteLine("\nBreadth-First Search:");

        while (queue.Count > 0)
        {
            int currentVertex = queue.Dequeue();
            Console.Write($"{GetVertexName(currentVertex)} "); // Print the current vertex

            // Add all unvisited neighbors to the queue
            for (int i = 0; i < _numberOfVertices; i++)
            {
                if (_adjacencyMatrix[currentVertex, i] > 0 && !visited[i])
                {
                    visited[i] = true;
                    queue.Enqueue(i);
                }
            }
        }

        Console.WriteLine();
    }

    // Perform Depth-First Search (DFS)
    public void DFS(string startVertex)
    {
        if (!_vertexDictionary.ContainsKey(startVertex))
        {
            Console.WriteLine("Invalid start vertex.");
            return;
        }

        bool[] visited = new bool[_numberOfVertices]; 

        Stack<int> stack = new Stack<int>(); // Stack for DFS

        int startIndex = _vertexDictionary[startVertex];
        stack.Push(startIndex);


        Console.WriteLine("\nDepth-First Search:");

        while (stack.Count > 0)
        {
            int currentVertex = stack.Pop();

            // Skip already visited nodes
            if (visited[currentVertex])
                continue;

            visited[currentVertex] = true; // Mark current vertex as visited
            Console.Write($"{GetVertexName(currentVertex)} "); // Print the current vertex

            // Add all unvisited neighbors to the stack
            for (int i = _numberOfVertices - 1; i >= 0; i--) // Reverse order for stack-based traversal
            {
                if (_adjacencyMatrix[currentVertex, i] > 0 && !visited[i])
                {
                    stack.Push(i);
                }
            }
        }

        Console.WriteLine();
    }

    private string GetVertexName(int index)
    {
        foreach (var pair in _vertexDictionary)
        {
            if (pair.Value == index)
                return pair.Key;
        }
        return null;
    }

}
internal class Program
{
    public static void Main(string[] args)
    {
        List<string> vertices = new List<string> { "0", "1", "2", "3", "4", "9", "8" };

        Graph graph = new Graph(vertices, enGraphDirectionType.unDirected);

        graph.AddEdge("0", "1", 1);
        graph.AddEdge("0", "2", 1);

        graph.AddEdge("1", "2", 1);
        graph.AddEdge("1", "3", 1);

        graph.AddEdge("2", "3", 1);
        graph.AddEdge("2", "4", 1);

        graph.AddEdge("9", "4", 1);

        graph.DisplayGraph("Adjacency Matrix (Undirected Graph):");

        // Perform Breadth-First Search (BFS)
        graph.BFS("0");

        // Perform Depth-First Search (DFS)
        graph.DFS("0");
        

    }

}