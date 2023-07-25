using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PathfindingTester : MonoBehaviour
{
    // The A* manager.
    private AStarManager AStarManager = new AStarManager();
    // Array of possible waypoints.
    List<GameObject> Waypoints = new List<GameObject>();
    // Array of waypoint map connections. Represents a path.
    List<Connections> ConnectionsArray = new List<Connections>();
    // The start and end target point.
    public GameObject start;
    public GameObject end;
    // Debug line offset.
    Vector3 OffSet = new Vector3(0, 0.3f, 0);
    public float speed;
    private Rigidbody rb;
    private Transform target;
    int current ;
    //float WPradius = 0.5f;
    Connections aConnections;
    private int count;
    public AudioSource collectSound;
    public Text countText;
    public Text winText;

    // Start is called before the first frame update
    void Start()
    {
        if (start == null || end == null)
        {
            Debug.Log("No start or end waypoints.");
            return;
        }
        // Find all the waypoints in the level.
        GameObject[] GameObjectsWithWaypointTag;
        GameObjectsWithWaypointTag = GameObject.FindGameObjectsWithTag("Waypoint");
        foreach (GameObject waypoint in GameObjectsWithWaypointTag)
        {
            WaypointCON tmpWaypointCon = waypoint.GetComponent<WaypointCON>();
            if (tmpWaypointCon)
            {
                Waypoints.Add(waypoint);
            }
        }
        // Go through the waypoints and create connections.
        foreach (GameObject waypoint in Waypoints)
        {
            WaypointCON tmpWaypointCon = waypoint.GetComponent<WaypointCON>();
            // Loop through a waypoints connections.
            foreach (GameObject WaypointConNode in tmpWaypointCon.Connections)
            {
                Connections aConnections = new Connections();
                aConnections.SetFromNode(waypoint);
                aConnections.SetToNode(WaypointConNode);
                AStarManager.AddConnections(aConnections);
            }
        }
        // Run A Star...
        ConnectionsArray = AStarManager.PathfindAStar(start, end);
        //  Debug.Log(ConnectionArray.Count);

        // rb = GetComponent<Rigidbody>();
        //rb.MovePosition((ConnectionArray[0].GetFromNode().transform.position + OffSet));
        //transform.position = ConnectionArray[0].GetFromNode().transform.position;

        count = 0;
        countText.text = "x " + count.ToString();
        winText.text = "";
    }
    // Draws debug objects in the editor and during editor play (if option set).
    void OnDrawGizmos()
    {
        // Draw path.
        foreach (Connections aConnections in ConnectionsArray)
        {
           
            Gizmos.color = Color.red;
            Gizmos.DrawLine((aConnections.GetFromNode().transform.position + OffSet),
            (aConnections.GetToNode().transform.position + OffSet));
          
        }
    }

    // Update is called once per frame
    void Update()
    {
        var LookPos = ConnectionsArray[current].GetToNode().transform.position - transform.position;
        LookPos.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(LookPos), 1);
        
        if (transform.position != ConnectionsArray[current].GetToNode().transform.position)
        {
            Vector3 pos2 = Vector3.MoveTowards(transform.position, ConnectionsArray[current].GetToNode().transform.position, speed * Time.deltaTime);
            GetComponent<Rigidbody>().MovePosition(pos2);
            //Debug.Log(transform.position);
        }
        else
        {
            current = (current + 1) % ((ConnectionsArray.Count));

           // if (current + 2 == (ConnectionsArray.Count - 1) && (transform.position != ConnectionsArray[current].GetToNode().transform.position))
                if (current + (ConnectionsArray.Count - 1) == (ConnectionsArray.Count - 1) && (transform.position != ConnectionsArray[current].GetToNode().transform.position))
                {
                if ((transform.position != ConnectionsArray[(current + (ConnectionsArray.Count - 1))].GetFromNode().transform.position))
                {
                    //carcount = carcount + 1;
                    countText.text = "x " + count.ToString();
                    speed = speed - 1f;

                }
                // Debug.Log(current);
                //  Debug.Log("From Else");
                //ConnectionsArray.Reverse();
                //carcount += 1;

                if (count == 10)
                {
                    speed = 0f;
                }

                Debug.Log(speed);
                Vector3 pos3 = Vector3.MoveTowards(transform.position, ConnectionsArray[current].GetFromNode().transform.position, speed * Time.deltaTime);
                GetComponent<Rigidbody>().MovePosition(pos3);
                ConnectionsArray.Reverse();

            }
           
           
            else
            {
                {

                    current = (current) % ((ConnectionsArray.Count));
                }

            }
           


        }

         
    }
    //Collect Sound
    //Collect Pickup
    void OnTriggerEnter(Collider other)
    {
        
            if (other.gameObject.CompareTag("PickUp")) { 
            speed = speed - 0.5f;
            other.gameObject.SetActive(false);
                count++;
            collectSound.Play();
            countText.text = "x " + count.ToString();

        }
            if (count >= 5)
        {
            winText.text = " *Task Completed* ";
        }
    }






}
