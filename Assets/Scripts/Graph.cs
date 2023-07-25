using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Graph
{
    // A list of graph connections.
    private List<Connections> WaypointConnections = new List<Connections>();
    public Graph()
    { }
    // Add connection.
    public void AddConnections(Connections aConnections)
    {
        WaypointConnections.Add(aConnections);
    }
    // Get the connections from a node to the nodes it is connected to.
    public List<Connections> GetConnections(GameObject FromNode)
    {
        List<Connections> TmpConnections = new List<Connections>();
        foreach (Connections aConnections in WaypointConnections)
        {
            if (aConnections.GetFromNode().Equals(FromNode))
            {
                TmpConnections.Add(aConnections);
            }
        }
        return TmpConnections;
    }
}