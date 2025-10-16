
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Node> neighbors;

    public float neighborDistance = 3f;
    internal List<Node> neightbors;

    private void Awake()
    {
        Node[] allNodes = FindObjectsOfType<Node>();

        foreach(Node node in allNodes)
        {
            if (node != this)
            {
                float distance = Vector3.Distance(transform.position, node.transform.position);
                if (distance <= neighborDistance)
                {
                    neighbors.Add(node);
                }
            }
        }
    }
    public void AddNeighbor(Node neighbor)
    {
        if (!neighbors.Contains(neighbor))
        {
            neighbors.Add(neighbor);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (neighbors != null)
        {
            foreach (Node neighbor in neighbors)
            {
                if (neighbor != null)
                {
                    Gizmos.DrawLine(transform.position, neighbor.transform.position);
                }
            }
        }
    }
}
//Hacer que se relaciones los nodos entre si, para que se puedan encontrar caminos entre ellos
