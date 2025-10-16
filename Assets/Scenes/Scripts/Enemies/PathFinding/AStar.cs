using System.Collections.Generic;
using System;
using UnityEngine;

public class AStar : MonoBehaviour
{
    public static List<T> Run<T>(T start, Func<T, bool> isSatisfied, Func<T, List<T>> getConnections, Func<T, T, float> getCost, Func<T, float> heuristic, int watchdog = 500, int watchdogPath = 500)
    {
        Debug.Log($"AStar: start={start}");
        Dictionary<T, T> parents = new Dictionary<T, T>();
        PriorityQueue<T> pending = new PriorityQueue<T>();
        HashSet<T> visited = new HashSet<T>();
        Dictionary<T, float> cost = new Dictionary<T, float>();
        cost[start] = 0;
        pending.Enqueue(start, 0);
        int steps = 0;

        while (!pending.IsEmpty)
        {
            watchdog--;
            steps++;
            if (watchdog <= 0)
            {
                Debug.LogWarning("AStar: watchdog agotado.");
                break;
            }

            T current = pending.Dequeue();
            Debug.Log($"AStar: Dequeued current={current}");

            if (isSatisfied(current))
            {
                Debug.Log($"AStar: isSatisfied en current={current} -> reconstruyendo path");
                List<T> path = new List<T>();
                path.Add(current);
                while (parents.ContainsKey(path[path.Count - 1]))
                {
                    watchdogPath--;
                    if (watchdogPath <= 0)
                    {
                        Debug.LogWarning("AStar: watchdogPath agotado.");
                        break;
                    }
                    path.Add(parents[path[path.Count - 1]]);
                }
                path.Reverse();
                Debug.Log($"AStar: Path encontrado, length={path.Count}");
                return path;
            }
            else
            {
                visited.Add(current);
                List<T> connections = null;
                try
                {
                    connections = getConnections(current);
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"AStar: getConnections lanzó excepción para {current}: {ex}");
                }

                if (connections == null)
                {
                    Debug.Log($"AStar: connections == null para {current}");
                    continue;
                }

                Debug.Log($"AStar: connections.Count={connections.Count} para {current}");

                for (int i = 0; i < connections.Count; i++)
                {
                    T child = connections[i];
                    if (child == null)
                    {
                        Debug.LogWarning($"AStar: child nulo en connections de {current} en index {i}");
                        continue;
                    }
                    if (visited.Contains(child)) continue;
                    float currentCost = cost[current] + getCost(current, child);
                    if (cost.ContainsKey(child) && currentCost > cost[child]) continue;

                    cost[child] = currentCost;

                    // PROTECCIÓN: calculamos heuristic en try/catch por si la función del usuario lanza
                    float heur = 0f;
                    try
                    {
                        heur = heuristic(child);
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogError($"AStar: heuristic lanzó excepción para child={child}: {ex}");
                        heur = float.MaxValue / 2f;
                    }

                    if (float.IsInfinity(heur) || float.IsNaN(heur))
                    {
                        Debug.LogWarning($"AStar: heuristic devolvió inválido para child={child}");
                        continue;
                    }

                    pending.Enqueue(child, currentCost + heur);
                    parents[child] = current;
                    Debug.Log($"AStar: Enqueued child={child} cost={currentCost} priority={currentCost + heur}");
                    //T child = connections[i];
                    //if (child == null)
                    //{
                    //    Debug.Log($"AStar: child nulo en connections de {current} en index {i}");
                    //    continue;
                    //}
                    //if (visited.Contains(child)) continue;
                    //float currentCost = cost[current] + getCost(current, child);

                    //bool hasCost = cost.ContainsKey(child);
                    //if (hasCost && currentCost > cost[child]) continue;

                    //cost[child] = currentCost;
                    //float priority = currentCost + heuristic(child);
                    //pending.Enqueue(child, priority);
                    //parents[child] = current;
                    //Debug.Log($"AStar: Enqueued child={child} cost={currentCost} priority={priority}");
                }
            }
        }

        Debug.Log($"AStar: no se encontró path. pasos={steps}");
        return new List<T>();
    }
    //{
    //    Dictionary<T, T> parents = new Dictionary<T, T>();
    //    PriorityQueue<T> pending = new PriorityQueue<T>();
    //    HashSet<T> visited = new HashSet<T>();
    //    Dictionary<T, float> cost = new Dictionary<T, float>();
    //    cost[start] = 0;
    //    pending.Enqueue(start, 0);
    //    while (!pending.IsEmpty)
    //    {
    //        watchdog--;
    //        if (watchdog <= 0) break;
    //        T current = pending.Dequeue();
    //        //Debug.Log("ASTAR");
    //        if (isSatisfied(current))
    //        {
    //            List<T> path = new List<T>();
    //            path.Add(current);
    //            while (parents.ContainsKey(path[path.Count - 1]))
    //            {
    //                watchdogPath--;
    //                if (watchdogPath <= 0) break;
    //                path.Add(parents[path[path.Count - 1]]);
    //            }
    //            path.Reverse();
    //            return path;
    //        }
    //        else
    //        {
    //            visited.Add(current);
    //            List<T> connections = getConnections(current);
    //            //Debug.Log("Connections: " + connections.Count);
    //            for (int i = 0; i < connections.Count; i++)
    //            {
    //                T child = connections[i];
    //                if (visited.Contains(child)) continue;
    //                float currentCost = cost[current] + getCost(current, child);
    //                if (cost.ContainsKey(child) && currentCost > cost[child]) continue;

    //                cost[child] = currentCost;
    //                pending.Enqueue(child, currentCost + heuristic(child));
    //                parents[child] = current;
    //            }
    //        }
    //    }

    //    return new List<T>();
    //}
}
