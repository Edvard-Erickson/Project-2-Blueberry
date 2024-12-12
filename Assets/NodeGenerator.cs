using UnityEngine;
using System.Collections.Generic;

public class PathfindingGrid : MonoBehaviour
{
    public float nodeSpacing; // Spacing between nodes
    public Vector2 gridSize; // Size of the grid
    public GameObject nodePrefab; // Optional: Prefab for visualizing nodes
    public LayerMask obstacleLayer; // LayerMask for obstacles

    private List<Node> nodes = new List<Node>();

    void Start()
    {
        GenerateNodes();
        ConnectNodes();
        CullNodes();
        Debug.Log(nodes.ToString());
    }

    // Step 1: Generate Nodes
    void GenerateNodes()
    {
        nodes.Clear(); // Ensure the list is empty before generating nodes
        int rowIndex = 0; // To keep track of the row index
        for (float y = -gridSize.y / 2; y <= gridSize.y / 2; y += nodeSpacing)
        {
            for (float x = -gridSize.x / 2; x <= gridSize.x / 2; x += nodeSpacing)
            {
                // Create a diamond pattern by offsetting every other row
                Vector3 offset = (rowIndex % 2 == 0) ? Vector3.right * (nodeSpacing / 2) : Vector3.zero;
                Vector3 position = transform.position + new Vector3(x, y, 0) + offset;

                // Optional: Instantiate a visual node prefab
                GameObject visual = null;
                if (nodePrefab)
                {
                    visual = Instantiate(nodePrefab, position, Quaternion.identity, transform);
                }

                Node newNode = new Node(position);
                newNode.Visual = visual;
                nodes.Add(newNode);
            }
            rowIndex++;
        }

        Debug.Log($"Generated {nodes.Count} valid nodes.");
    }

    // Step 2: Validate Connections
    void ConnectNodes()
    {
        float diagonalThreshold = nodeSpacing * Mathf.Sqrt(2) * 1.1f; // Allow some margin
        float straightThreshold = nodeSpacing * 1.1f; // Slight margin for floating-point errors

        foreach (var nodeA in nodes)
        {
            foreach (var nodeB in nodes)
            {
                if (nodeA == nodeB) continue; // Skip self-connections

                float distance = Vector3.Distance(nodeA.Position, nodeB.Position);

                // Connect nodes within the appropriate thresholds
                if (distance <= diagonalThreshold && !nodeA.Connections.Contains(nodeB))
                {
                    nodeA.Connections.Add(nodeB);
                    nodeB.Connections.Add(nodeA); // Ensure bidirectional connection
                    Debug.Log($"Connected Node {nodeA.Position} to Node {nodeB.Position}");
                }
            }
        }
    }

    // Step 3: Cull Invalid Nodes
    void CullNodes()
    {
        List<Node> culledNodes = new List<Node>();

        foreach (var node in nodes)
        {
            Debug.Log("node" + node.Connections.Count + "connections");
            if (node.Connections.Count < 3)
            {
                culledNodes.Add(node);
            }
            else
            {
                Debug.Log("not culling node");
            }
        }

        foreach (var culledNode in culledNodes)
        {
            foreach (var neighbor in culledNode.Connections)
            {
                neighbor.Connections.Remove(culledNode);
            }

            if (culledNode.Visual != null)
            {
                Destroy(culledNode.Visual);
            }

            nodes.Remove(culledNode);
            Debug.Log("Removed a node");
        }
    }

    // Utility: Visualize Nodes in the Scene View
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (nodes != null)
        {
            foreach (var node in nodes)
            {
                Gizmos.DrawSphere(node.Position, 0.1f); // Visualize nodes
            }
        }
    }

    // Utility: Expose Nodes for A* Pathfinding
    public List<Node> GetNodes() => nodes;
}

// Node class to represent each waypoint
public class Node
{
    public Vector3 Position { get; private set; }
    public List<Node> Connections { get; private set; }
    public GameObject Visual { get; set; }

    public Node(Vector3 position)
    {
        Position = position;
        Connections = new List<Node>();
        Visual = null;
    }
}
