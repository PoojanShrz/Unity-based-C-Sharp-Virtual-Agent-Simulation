using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AStar
{
    public AStar()
    { }
    public List<Connections> PathfindAStar(Graph aGraph, GameObject start, GameObject end, Heuristic myHeuristic)
    {
        // Set up the start record.
        NodeRecord StartRecord = new NodeRecord();
        StartRecord.Node = start;
        StartRecord.Connections = null;
        StartRecord.CostSoFar = 0;
        StartRecord.EstimatedTotalCost = myHeuristic.Estimate(start, end);
        // Create the lists.
        PathfindingList OpenList = new PathfindingList();
        PathfindingList ClosedList = new PathfindingList();
        // Add the start record to the open list.
        OpenList.AddNodeRecord(StartRecord);
        // Iterate through and process each node.
        NodeRecord CurrentRecord = null;
        List<Connections> Connections;
        while (OpenList.GetSize() > 0)
        {
            // Find the smallest element in the open list (using the estimatedTotalCost).
            CurrentRecord = OpenList.GetSmallestElement();
            // If it is the goal node, then terminate.
            if (CurrentRecord.Node.Equals(end))
            {
                break;
            }
            // Otherwise get its outgoing connections.
            Connections = aGraph.GetConnections(CurrentRecord.Node);
            // Loop through each connection in turn.
            GameObject EndNode;
            float EndNodeCost;
            NodeRecord EndNodeRecord;
            float EndNodeHeuristic;
            foreach (Connections aConnections in Connections)
            {
                // Get the cost estimate for the end node.
                EndNode = aConnections.GetToNode();
                EndNodeCost = CurrentRecord.CostSoFar + aConnections.GetCost();
                // If the node is closed we may have to skip, or remove it from the closed list.
                if (ClosedList.Contains(EndNode))
                {
                    // Here we find the record in the closed list corresponding to the endNode.
                    EndNodeRecord = ClosedList.Find(EndNode);
                    // If we didn’t find a shorter route, skip.
                    if (EndNodeRecord.CostSoFar <= EndNodeCost)
                    {
                        continue;
                    }
                    // Otherwise remove it from the closed list.
                    ClosedList.RemoveNodeRecord(EndNodeRecord);
                    // We can use the node’s old cost values to calculate its heuristic without calling
                    // the possibly expensive heuristic function.
                    EndNodeHeuristic = EndNodeRecord.EstimatedTotalCost - EndNodeRecord.CostSoFar;
                }
                // Skip if the node is open and we’ve not found a better route.
                else if (OpenList.Contains(EndNode))
                {
                    // Here we find the record in the open list corresponding to the endNode.
                    EndNodeRecord = OpenList.Find(EndNode);
                    // If our route is no better, then skip.
                    if (EndNodeRecord.CostSoFar <= EndNodeCost)
                    {
                        continue;
                    }
                    // We can use the node’s old cost values to calculate its heuristic without calling
                    // the possibly expensive heuristic function.
                    EndNodeHeuristic = EndNodeRecord.EstimatedTotalCost - EndNodeRecord.CostSoFar;
                }
                // Otherwise we know we’ve got an unvisited node, so make a record for it.
                else
                {
                    EndNodeRecord = new NodeRecord();
                    EndNodeRecord.Node = EndNode;
                    // We’ll need to calculate the heuristic value using the function, since we don’t have an existing record to use.
                    EndNodeHeuristic = myHeuristic.Estimate(EndNode, end);
                }

                // We’re here if we need to update the node Update the cost, estimate and connection.
                EndNodeRecord.CostSoFar = EndNodeCost;
                EndNodeRecord.Connections = aConnections;
                EndNodeRecord.EstimatedTotalCost = EndNodeCost + EndNodeHeuristic;
                // And add it to the open list.
                if (!(OpenList.Contains(EndNode)))
                {
                    OpenList.AddNodeRecord(EndNodeRecord);
                }
            } //#END: Looping through Connections.
              // We’ve finished looking at the connections for the current node, so add it to the closed list
// and remove it from the open list
OpenList.RemoveNodeRecord(CurrentRecord);
            ClosedList.AddNodeRecord(CurrentRecord);
        }
        // We’re here if we’ve either found the goal, or if we’ve no more nodes to search, find which.
        List<Connections> tempList = new List<Connections>();
        if (!CurrentRecord.Node.Equals(end))
        {
            // We’ve run out of nodes without finding the goal, so there’s no solution
            return tempList;
        }
        else
        {
            while (!CurrentRecord.Node.Equals(start))
            {
                tempList.Add(CurrentRecord.Connections);
                CurrentRecord = ClosedList.Find(CurrentRecord.Connections.GetFromNode());
            }
            // The path is in the wrong order. Reverse the path, and return it.
            List<Connections> tempList2 = new List<Connections>();
            for (int i = (tempList.Count - 1); i >= 0; i--)
            {
                tempList2.Add(tempList[i]);
            }
            return tempList2;
        }
    }
}