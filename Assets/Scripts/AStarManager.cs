using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AStarManager
{
    // The a star algorithm.
    private AStar AStar = new AStar();
    // The waypoint graph.
    Graph aGraph = new Graph();
    // The Heuristic.
    Heuristic aHeuristic = new Heuristic();
    public AStarManager()
    { }
    // Add Connection.
    public void AddConnections(Connections connections)
    {
        aGraph.AddConnections(connections);
    }
    // Find path.
    public List<Connections> PathfindAStar(GameObject start, GameObject end)
    {
        return AStar.PathfindAStar(aGraph, start, end, aHeuristic);
    }
}