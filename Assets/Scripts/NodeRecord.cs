using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NodeRecord
{
    public GameObject Node;
    public Connections Connections;
    public float CostSoFar;
    public float EstimatedTotalCost;
    public NodeRecord()
    { }
}