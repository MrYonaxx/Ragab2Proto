using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagabMovement : MonoBehaviour {


    private Rigidbody2D ragabRigidbody;

    [SerializeField]
    private float speed = 5;

    [Header("Jump")]
    [SerializeField]
    private float aerialFriction = 50;

    [SerializeField]
    private float gravity = 200;
    [SerializeField]
    private float initialJumpForce = 100;
    [SerializeField]
    private float additionalJumpForce = 50;


    [Header("Debug")]
    public bool isJumping = false;
    public bool isGrounded = false;




    private float actualSpeedX = 0;
    private float actualSpeedY = 0;

	// Use this for initialization
	void Start () {
        Application.targetFrameRate = 60;
        ragabRigidbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        CheckInput();
        ApplyGravity();
        UpdatePosition();
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

        if (Input.GetKey("up"))
        {
            Jump();
        }

    }

    private void MoveRight()
    {
        if(actualSpeedX < speed)
        {
            if (isJumping == true)
            {
                actualSpeedX -= aerialFriction;
            }
            actualSpeedX += speed;
        }
        else
        {
            actualSpeedX = speed;
        }
    }

    private void MoveLeft()
    {
        if (actualSpeedX > -speed)
        {
            if (isJumping == true)
            {
                actualSpeedX += aerialFriction;
            }
            actualSpeedX -= speed;
        }
        else
        {
            actualSpeedX = -speed;
        }
    }

    private void NoMove()
    {
        if (actualSpeedX != 0)
        {
            actualSpeedX = 0;
        }
    }




    private void Jump()
    {
        if(isJumping == true)
        {
            if (actualSpeedY > 0)
            {
                actualSpeedY += additionalJumpForce;
            }
        }
        if (isJumping == false && isGrounded == true)
        {
            actualSpeedY += initialJumpForce;
            isJumping = true;
        }
    }


    private void ApplyGravity()
    {
        if (actualSpeedY > -gravity)
        {
            actualSpeedY -= gravity;
        }
    }



    private void UpdatePosition()
    {
        ragabRigidbody.velocity = new Vector2(actualSpeedX * Time.deltaTime, actualSpeedY * Time.deltaTime);
    }


    private void OnCollisionEnter2D(Collision2D theCollision)
    {
        if (theCollision.gameObject.name == "Sol")
        {
            isGrounded = true;
            isJumping = false;
        }

    }

    private void OnCollisionExit2D(Collision2D theCollision)
    {
        if (theCollision.gameObject.name == "Sol")
        {
            isGrounded = false;
        }
    }
}
