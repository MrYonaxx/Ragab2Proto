using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagabMovement : MonoBehaviour {


    private Rigidbody2D ragabRigidbody;

    [SerializeField]
    private float speedAcceleration = 50;
    [SerializeField]
    private float speedMax = 200;
    [SerializeField]
    private float aerialFriction = 180;

    [Header("Jump")]


    [SerializeField]
    private float gravityAcceleration = 20;
    [SerializeField]
    private float gravityMax = 200;
    [SerializeField]
    private float initialJumpForce = 100;
    [SerializeField]
    private float additionalJumpForce = 500;


    [Header("Debug")]
    public bool isJumping = false;
    public bool isGrounded = false;

    private bool jumpAvailable = true;


    private float actualGravityAcceleration = 0;

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
        else if (Input.GetKeyUp("up"))
        {
            jumpAvailable = true;
        }

    }

    // ============================================
    // Mouvement du Personnage
    // ============================================
    private void MoveRight()
    {
        if(actualSpeedX < speedMax)
        {
            if (isJumping == true)
            {
                actualSpeedX -= aerialFriction;
            }
            actualSpeedX += speedAcceleration;
        }
        else
        {
            actualSpeedX = speedMax;
        }
    }

    private void MoveLeft()
    {
        if (actualSpeedX > -speedMax)
        {
            if (isJumping == true)
            {
                actualSpeedX += aerialFriction;
            }
            actualSpeedX -= speedAcceleration;
        }
        else
        {
            actualSpeedX = -speedMax;
        }
    }

    private void NoMove()
    {
        if (isGrounded == true)
        {
            if (-speedAcceleration < actualSpeedX && actualSpeedX < speedAcceleration)
            {
                actualSpeedX = 0;
            }
            else if (actualSpeedX <= -speedAcceleration)
            {
                actualSpeedX += speedAcceleration;
            }
            else if (speedAcceleration <= actualSpeedX)
            {
                actualSpeedX -= speedAcceleration;
            }
        }
    }




    // ============================================
    // Saut du Personnage
    // ============================================

    private void Jump()
    {
        if(isJumping == true)
        {
            if (actualSpeedY > 0)
            {
                actualSpeedY += additionalJumpForce;
            }
        }
        if (jumpAvailable == true)
        {
            actualGravityAcceleration = 0;
            actualSpeedY = 0;
            actualSpeedY += initialJumpForce;
            isJumping = true;
            jumpAvailable = false;
        }
    }


    private void ApplyGravity()
    {
        if (-gravityMax < actualSpeedY)
        {
            actualGravityAcceleration += gravityAcceleration;
            actualSpeedY -= actualGravityAcceleration;
        }
        else
        {
            actualSpeedY = -gravityMax;
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
