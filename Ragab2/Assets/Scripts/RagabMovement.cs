using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagabMovement : MonoBehaviour {


    private Rigidbody2D ragabRigidbody;
    [SerializeField]
    private float speed = 5;

    private float actualSpeed = 0;

	// Use this for initialization
	void Start () {
        ragabRigidbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        CheckInput();
        UpdatePosition();
	}


    private void CheckInput()
    {
        if (Input.GetKey("right"))
        {
            MoveRight();
        }
        if (Input.GetKey("left"))
        {
            MoveLeft();
        }
    }

    private void MoveRight()
    {
        actualSpeed = speed;
    }

    private void MoveLeft()
    {
        actualSpeed = -speed;
    }

    private void NoMove()
    {
        actualSpeed = 0;
    }


    private void UpdatePosition()
    {
        ragabRigidbody.AddForce(new Vector2(actualSpeed * Time.deltaTime, 0));
        //actualSpeed = 0;
    }

}
