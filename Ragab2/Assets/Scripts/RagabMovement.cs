using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagabMovement : MonoBehaviour {


    // =============================================
    // Attributes
    // =============================================

    private Rigidbody2D ragabRigidbody;

    [Header("Movement")] // =============================================
    [SerializeField]
    private float speedAcceleration = 50;
    [SerializeField]
    private float speedMax = 300;
    [SerializeField]
    private float aerialFriction = 0.7f;
    [SerializeField]
    private float aerialInertia = 20;

    [Header("Jump")]  // =============================================
    [SerializeField]
    private float gravityForce = 20;
    [SerializeField]
    private float gravityMax = 300;
    [SerializeField]
    private float initialJumpForce = 300;
    [SerializeField]
    private float additionalJumpForce = 20;

    [Header("Autres")]  // =============================================
    [SerializeField]
    private float secondBeforeJumpDisabled = 0.5f;



    [Header("Debug")]  // =============================================
    public GameObject trailDebug = null;
    public bool activateDebugTrail = true;



    public bool isJumping = false;
    public bool isGrounded = false;
    private bool jumpAvailable = true;


    private float actualAerialDecceleration = 0;

    private float actualGravityAcceleration = 0;

    private float actualSpeedX = 0;
    private float actualSpeedY = 0;

    private int direction = 1;


    // =============================================
    // Functions
    // =============================================


    // Use this for initialization
    void Start () {
        Application.targetFrameRate = 60;
        ragabRigidbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {

        CheckGround();

        CheckMovement();
        CheckJump();

        ApplyGravity();

        UpdatePosition();

        if (activateDebugTrail)
            Instantiate(trailDebug, this.transform.position, Quaternion.identity);
    }


    // ============================================
    // Check Position Initial
    // ============================================

    private void CheckGround()
    {
        if (isGrounded == true)
        {
            isJumping = false;

            actualAerialDecceleration = 0;
            actualGravityAcceleration = 0;

            jumpAvailable = true;
        }
    }


    // ============================================
    // Mouvement du Personnage
    // ============================================

    // Je checke les mouvements horizontals
    private void CheckMovement()
    {
        if (Input.GetKey("right") || Input.GetAxis("Horizontal") >= 0.4f) // Déplacements gauche et droite
        {
            MoveRight();
        }
        else if (Input.GetKey("left") || Input.GetAxis("Horizontal") <= -0.4f)
        {
            MoveLeft();
        }
        else if (isGrounded == true) // Pas de déplacement
        {
            NoMoveOnGround();
        } 
        else if (isGrounded == false)
        {
            NoMoveOnAir();
        }

    }

    // Set the direction to the right and move
    private void MoveRight()
    {
        if(actualSpeedX < speedMax)
        {
            direction = 1;
            Move();
        }
        else
        {
            actualSpeedX = speedMax;
        }
    }

    // Set the direction to the left and move
    private void MoveLeft()
    {
        if (actualSpeedX > -speedMax)
        {
            direction = -1;
            Move();
        }
        else
        {
            actualSpeedX = -speedMax;
        }
    }

    // Move the player in the direction he's looking at
    private void Move()
    {
        if (isGrounded == false)
        {
            actualSpeedX -= aerialInertia * direction;
        }
        actualSpeedX += speedAcceleration * direction;
    }

    // If the player don't move then make the player deccelerate
    private void NoMoveOnGround()
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


    private void NoMoveOnAir()
    {
        if (-1 < actualSpeedX && actualSpeedX < 1)
        {
            actualSpeedX = 0;
        }
        else if (actualSpeedX <= -1)
        {
            actualAerialDecceleration += aerialFriction;
            actualSpeedX += actualAerialDecceleration;
        }
        else if (1 <= actualSpeedX)
        {
            actualAerialDecceleration += aerialFriction;
            actualSpeedX -= actualAerialDecceleration;
        }
    }




    // ============================================
    // Saut du Personnage
    // ============================================

    private void CheckJump()
    {
        if (Input.GetKeyDown("up") || Input.GetButtonDown("Jump"))
        {
            StartJump();
        }
        else if (Input.GetKey("up") || Input.GetButton("Jump"))
        {
            NuanceJump();
        }
        else if ((Input.GetKeyUp("up") || Input.GetButtonUp("Jump")) && isJumping == true)
        {
            isJumping = false;
        }
    }


    private void StartJump()
    {
        if (jumpAvailable == true)
        {
            actualSpeedY = 0;
            actualSpeedY += initialJumpForce;
            isJumping = true;
            isGrounded = false;
            jumpAvailable = false;
        }
    }

    private void NuanceJump()
    {
        if (isJumping == true)
        {
            actualSpeedY += additionalJumpForce;
        }
    }


    private void ApplyGravity()
    {
        actualSpeedY -= gravityForce;

        if (actualSpeedY  < -gravityMax)
        {
            actualSpeedY = -gravityMax;
        }
     
    }




    public void SetGrounded(bool b)
    {
        isGrounded = b;
        // Place une sécurité pour sauter même si le perso tombe un peu
        if (b == false && isJumping == false)
        {
            StartCoroutine(WaitBeforeDisableJump(secondBeforeJumpDisabled));
        }
    }

    private IEnumerator WaitBeforeDisableJump(float second)
    {
        yield return new WaitForSeconds(second);
        jumpAvailable = false;
    }


    // Update the position of the player
    private void UpdatePosition()
    {
        ragabRigidbody.velocity = new Vector2(actualSpeedX * Time.deltaTime, actualSpeedY * Time.deltaTime);
    }



}
