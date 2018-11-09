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
        //UpdatePosition();
	}


    private void CheckInput()
    {
        if (Input.GetKey("right"))
        {
            MoveRight();
        }
        else if (Input.GetKey("left"))
        {
            MoveLeft();
        }
        else
        {
            NoMove();
        }
    }

    private void MoveRight()
    {
        actualSpeed = speed;
        ragabRigidbody.velocity = new Vector2(actualSpeed * Time.deltaTime, 0);
    }

    private void MoveLeft()
    {
        actualSpeed = -speed;
        ragabRigidbody.velocity = new Vector2(actualSpeed * Time.deltaTime, 0);
    }

    private void NoMove()
    {
        actualSpeed = 0;
        ragabRigidbody.velocity = new Vector2(0, 0);

    }


    private void UpdatePosition()
    {
        ragabRigidbody.AddForce(new Vector2(actualSpeed * Time.deltaTime, 0), ForceMode2D.Impulse);
        actualSpeed = 0;
    }

}
