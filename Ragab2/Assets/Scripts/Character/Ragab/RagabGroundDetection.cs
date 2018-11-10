using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RagabGroundDetection : MonoBehaviour
{

    [SerializeField]
    UnityEvent eventOnGroundEnter;

    [SerializeField]
    UnityEvent eventOnGroundExit;


    private void OnTriggerEnter2D(Collider2D theCollision)
    {
        if (theCollision.gameObject.tag == "Ground")
        {
            eventOnGroundEnter.Invoke();
        }

    }

    private void OnTriggerExit2D(Collider2D theCollision)
    {
        if (theCollision.gameObject.tag == "Ground")
        {
            eventOnGroundExit.Invoke();
        }
    }
}
