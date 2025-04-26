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


    // Dijkstra's Algorithm: Finds the shortest paths from a source vertex
    
    // using Adjacency Matrix : O(V^2)
    public void Dijkstra_AdjacencyMatrix(string startVertex)
    {

        if (!_vertexDictionary.ContainsKey(startVertex))
        {
            Console.WriteLine("Invalid start vertex.");
            return;
        }

        // Initialize distance and visited arrays
        int startIndex = _vertexDictionary[startVertex];
        int[] distances = new int[_numberOfVertices]; // Stores shortest distances
        bool[] visited = new bool[_numberOfVertices]; // Tracks processed vertices
        string[] predecessors = new string[_numberOfVertices]; // Tracks the previous vertex on the shortest path

        // Initialize all distances to infinity and mark all vertices as unvisited
        for (int i = 0; i < _numberOfVertices; i++)
        {
            distances[i] = int.MaxValue; // Set distance to "infinity"
            visited[i] = false; // Mark as unvisited
            predecessors[i] = null; // No predecessors initially
        }
        distances[startIndex] = 0; // Distance to the source is 0

        // Main loop: Process each vertex
        for (int count = 0; count < _numberOfVertices - 1; count++)
        {
            // Find the unvisited vertex with the smallest distance
            int minVertex = GetMinDistanceVertex(distances, visited);
            visited[minVertex] = true; // Mark this vertex as visited

            // Update distances for all neighbors of the current vertex
            for (int v = 0; v < _numberOfVertices; v++)
            {
                // Update distance if:
                // 1. There is an edge.
                // 2. The vertex is unvisited.
                // 3. The new distance is shorter.
                if (!visited[v] && _adjacencyMatrix[minVertex, v] > 0 &&
                    distances[minVertex] != int.MaxValue &&
                    distances[minVertex] + _adjacencyMatrix[minVertex, v] < distances[v])
                {
                    distances[v] = distances[minVertex] + _adjacencyMatrix[minVertex, v];
                    predecessors[v] = GetVertexName(minVertex); // Record the predecessor, prev node.
                }
            }
        }

        // Display the shortest paths and their distances
        Console.WriteLine("\nShortest paths from vertex " + startVertex + ":");
        for (int i = 0; i < _numberOfVertices; i++)
        {
            Console.WriteLine($"{startVertex} -> {GetVertexName(i)}: Distance = {distances[i]}, Path = {GetPath(predecessors, i)}");
        }
    }

    // using Adjacency List with Min-Heap: O((V+E) log⁡ V)
    public void Dijkstra_AdjacencyList(string startVertex)
    {
        // Validate the starting vertex
        if (!_vertexDictionary.ContainsKey(startVertex))
        {
            Console.WriteLine("Invalid start vertex.");
            return;
        }

        int startIndex = _vertexDictionary[startVertex]; // Get the index of the starting vertex

        // Array to store the shortest distance from the start vertex to each vertex
        int[] distances = new int[_numberOfVertices];

        // Boolean array to track if a vertex has been visited
        bool[] visited = new bool[_numberOfVertices];

        // Array to store the predecessor of each vertex in the shortest path
        string[] predecessors = new string[_numberOfVertices];

        // Initialize distances to infinity and predecessors to null
        for (int i = 0; i < _numberOfVertices; i++)
        {
            distances[i] = int.MaxValue; // Distance set to "infinity"
            predecessors[i] = null; // No predecessor initially
        }
        distances[startIndex] = 0; // Distance to the starting vertex is 0

        // Priority queue (Min-Heap) to store vertices with their distances
        var priorityQueue = new SortedSet<(int distance, int vertexIndex)>(
            Comparer<(int distance, int vertexIndex)>.Create((x, y) =>
                x.distance == y.distance ? x.vertexIndex.CompareTo(y.vertexIndex) : x.distance.CompareTo(y.distance))
        );

        // Add the starting vertex to the priority queue
        priorityQueue.Add((0, startIndex));

        // Process all vertices in the priority queue
        while (priorityQueue.Count > 0)
        {
            // Extract the vertex with the smallest distance
            var (currentDistance, currentIndex) = priorityQueue.Min;
            priorityQueue.Remove(priorityQueue.Min);

            // Skip the vertex if it's already visited
            if (visited[currentIndex]) continue;
            visited[currentIndex] = true; // Mark the vertex as visited

            // Update the distances for all neighbors of the current vertex
            for (int neighbor = 0; neighbor < _numberOfVertices; neighbor++)
            {
                // Check if there is an edge and the neighbor is unvisited
                if (_adjacencyMatrix[currentIndex, neighbor] > 0 && !visited[neighbor])
                {
                    // Calculate the new distance to the neighbor
                    int newDistance = distances[currentIndex] + _adjacencyMatrix[currentIndex, neighbor];

                    // If the new distance is shorter, update it
                    if (newDistance < distances[neighbor])
                    {
                        priorityQueue.Remove((distances[neighbor], neighbor)); // Remove the old distance
                        distances[neighbor] = newDistance; // Update to the new distance
                        predecessors[neighbor] = GetVertexName(currentIndex); // Update the predecessor
                        priorityQueue.Add((newDistance, neighbor)); // Add the updated distance to the queue
                    }
                }
            }
        }

        // Print the shortest paths and their distances
        Console.WriteLine("\nShortest paths from vertex " + startVertex + ":");
        for (int i = 0; i < _numberOfVertices; i++)
        {
            Console.WriteLine($"{startVertex} -> {GetVertexName(i)}: Distance = {distances[i]}, Path = {GetPath(predecessors, i)}");
        }
    }


    // Finds the unvisited vertex with the smallest distance
    private int GetMinDistanceVertex(int[] distances, bool[] visited)
    {
        int minDistance = int.MaxValue; // Start with infinity
        int minIndex = -1;

        // Iterate over all vertices
        for (int i = 0; i < _numberOfVertices; i++)
        {
            // Update the minimum if the vertex is unvisited and has a smaller distance
            if (!visited[i] && distances[i] < minDistance)
            {
                minDistance = distances[i];
                minIndex = i;
            }
        }
        return minIndex;
    }
    private string GetPath(string[] predecessors, int currentIndex)
    {
        // Reconstructs the shortest path from the source to a vertex using predecessors

        // Base case: If there is no predecessor, return the current vertex
        if (predecessors[currentIndex] == null)
            return GetVertexName(currentIndex);

        // Recursive case: Build the path from the predecessor
        return GetPath(predecessors, _vertexDictionary[predecessors[currentIndex]]) + " -> " + GetVertexName(currentIndex);
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

        // Perform Dijkstra's Algorithm using Adjacency Matrix
        graph.Dijkstra_AdjacencyMatrix("0");

        // Perform Dijkstra's Algorithm using Adjacency List
        graph.Dijkstra_AdjacencyList("0");



    }

}