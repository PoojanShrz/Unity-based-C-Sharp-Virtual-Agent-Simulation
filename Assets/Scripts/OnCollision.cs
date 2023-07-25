using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class OnCollision : MonoBehaviour
{
    [SerializeField] private Animator myAnimationController;

    private IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacles"))
        {
            myAnimationController.SetBool("jump", true);
            float z = GetComponent<PathfindingTester>().speed;
            GetComponent<PathfindingTester>().speed = 7f;
        }


        if (other.CompareTag("Collision"))
        {
            myAnimationController.SetBool("falldead", true);
            float a = GetComponent<PathfindingTester>().speed;
            GetComponent<PathfindingTester>().speed = 0f;
            yield return new WaitForSeconds(3);
            GetComponent<PathfindingTester>().speed = a;
        }
    }
    private IEnumerator OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Obstacles"))
        {
            myAnimationController.SetBool("jump", false);
        }
        if (other.CompareTag("Collision"))
        {
            myAnimationController.SetBool("falldead", false);
           float b = GetComponent<PathfindingTester>().speed;
           GetComponent<PathfindingTester>().speed = 0f;
            yield return new WaitForSeconds(1);
            GetComponent<PathfindingTester>().speed = b;
        }
    }
}