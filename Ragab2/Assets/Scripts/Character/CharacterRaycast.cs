/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using UnityEngine;
using System.Collections;

namespace Ragab
{


    /// <summary>
    /// Definition of the Character class
    /// </summary>
    public class CharacterRaycast : MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        private Rigidbody2D characterRigidbody;
        private BoxCollider2D characterCollider;

        [Header("Movement")] // =============================================

        [SerializeField]
        protected float speedAcceleration = 40;
        [SerializeField]
        protected float speedMax = 400;
        [SerializeField]
        protected float aerialFriction = 0.7f;
        [SerializeField]
        protected float aerialInertia = 20;


        [Header("Jump")]  // =============================================

        [SerializeField]
        protected float gravityForce = 50;
        [SerializeField]
        protected float gravityMax = 400;
        [SerializeField]
        protected float initialJumpForce = 600;
        [SerializeField]
        protected float additionalJumpForce = 35;


        [Header("Autres")]  // =============================================
        [SerializeField]
        public State characterState;

        [Header("Animations")]
        [SerializeField]
        protected CharacterAnimation characterAnimation;


        [Header("Debug")]
        public GameObject trailDebug = null;
        public bool activateDebugTrail = true;


        protected float actualAerialDecceleration = 0;
        protected float actualGravityAcceleration = 0;

        protected float actualSpeedX = 0;
        protected float actualSpeedY = 0;

        protected int direction = 1;

        public int Direction
        {
            get { return direction; }
        }

        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */

        public void SetDirection(int dir)
        {
            /*if(dir != 1 || dir != -1)
            {
                return;
            }*/
            direction = dir;
        }


        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */

        protected void Start()
        {
            Application.targetFrameRate = 144;
            characterRigidbody = GetComponent<Rigidbody2D>();
            characterCollider = GetComponent<BoxCollider2D>();
        }

        protected void Update()
        {

            CheckState();

            ApplyGravity();
            UpdatePositionY();
            transform.position += new Vector3(0, actualSpeedY * Time.deltaTime, 0);
            UpdatePositionX();
            //characterRigidbody.velocity = new Vector2(actualSpeedX * Time.deltaTime, actualSpeedY * Time.deltaTime);
            transform.position += new Vector3(actualSpeedX * Time.deltaTime, 0, 0);

            if (activateDebugTrail)
                Instantiate(trailDebug, this.transform.position, Quaternion.identity);

            if (characterAnimation != null)
                characterAnimation.CheckAnimation(characterState, direction, actualSpeedX);

        }



        public void ChangeState(State newState)
        {

        }

        public virtual void SetOnGround(bool b)
        {
            if (b == true)
            {
                characterState = State.Grouded;
            }
            else
            {
                if (characterState != State.Jumping)
                    characterState = State.Falling;
            }
        }

        public void CheckState()
        {

            if (characterState == State.Grouded)
            {
                actualAerialDecceleration = 0;
                actualGravityAcceleration = 0;
            }


            if (characterState == State.Jumping)
            {
                if (actualSpeedY <= 0)
                    characterState = State.Falling;
            }
        }


        private void ApplyGravity()
        {
            actualSpeedY -= gravityForce;

            if (actualSpeedY < -gravityMax)
            {
                actualSpeedY = -gravityMax;
            }

        }



        // Update the position of the player
        private void UpdatePositionY()
        {
            int layerMask = 1 << 8;
            Vector2 bottomLeft = new Vector2(characterCollider.bounds.min.x, characterCollider.bounds.min.y);
            Vector2 bottomRight = new Vector2(characterCollider.bounds.max.x, characterCollider.bounds.min.y);

            if (actualSpeedY < 0)
            {

                RaycastHit2D raycastY = Physics2D.Raycast(bottomLeft,
                                  new Vector2(0, actualSpeedY * Time.deltaTime),
                                              Mathf.Abs(actualSpeedY * Time.deltaTime),
                                              layerMask);
                Debug.DrawRay(bottomLeft, new Vector2(0, actualSpeedY * Time.deltaTime), Color.green);
                if (raycastY.collider != null)
                {
                    //Debug.Log(raycastY.collider.gameObject.name);
                    SetOnGround(true);
                    float distance = raycastY.point.y - transform.position.y;
                    actualSpeedY = distance / Time.deltaTime;
                    return;
                }


                raycastY = Physics2D.Raycast(bottomRight,
                                  new Vector2(0, actualSpeedY * Time.deltaTime),
                                              Mathf.Abs(actualSpeedY * Time.deltaTime),
                                              layerMask);
                Debug.DrawRay(bottomRight, new Vector2(0, actualSpeedY * Time.deltaTime), Color.green);
                if (raycastY.collider != null)
                {
                    //Debug.Log(raycastY.collider.gameObject.name);
                    SetOnGround(true);
                    float distance = raycastY.point.y - transform.position.y;
                    actualSpeedY = distance / Time.deltaTime;
                    return;
                }
            }
        }


        private void UpdatePositionX()
        {
            int layerMask = 1 << 8;

            float offsetY = 0.01f;
            Vector2 bottomLeft = new Vector2(characterCollider.bounds.min.x, characterCollider.bounds.min.y + offsetY);
            Vector2 bottomRight = new Vector2(characterCollider.bounds.max.x, characterCollider.bounds.min.y + offsetY);

            if (actualSpeedX < 0)
            {
                RaycastHit2D raycastX = Physics2D.Raycast(bottomLeft,
                                                          new Vector2(actualSpeedX * Time.deltaTime, 0),
                                                          Mathf.Abs(actualSpeedX * Time.deltaTime),
                                                          layerMask);
                Debug.DrawRay(bottomLeft, new Vector2(actualSpeedX * Time.deltaTime, 0), Color.green);
                if (raycastX.collider != null)
                {
                    Debug.Log("Zbla");
                    float distance = raycastX.point.x - bottomLeft.x;
                    actualSpeedX = distance / Time.deltaTime;
                }
            }
            else if (actualSpeedX > 0)
            {
                RaycastHit2D raycastX = Physics2D.Raycast(bottomRight,
                                          new Vector2(actualSpeedX * Time.deltaTime, 0),
                                          Mathf.Abs(actualSpeedX * Time.deltaTime),
                                          layerMask);
                Debug.DrawRay(bottomRight, new Vector2(actualSpeedX * Time.deltaTime, 0), Color.green);
                if (raycastX.collider != null)
                {
                    Debug.Log("ZblaDroite");
                    float distance = raycastX.point.x - bottomRight.x;
                    actualSpeedX = distance / Time.deltaTime;
                }
            }

        }

        #endregion

        #region FunctionsMovement 

        /* ================================================= *\
         *                FUNCTIONS MOVEMENT                 *
        \* ================================================= */

        public virtual void Move()
        {
            if (characterState == State.Falling || characterState == State.Jumping)
            {
                actualSpeedX -= aerialInertia * direction;
            }
            actualSpeedX += speedAcceleration * direction;

            if (actualSpeedX > speedMax)
                actualSpeedX = speedMax;

            if (actualSpeedX < -speedMax)
                actualSpeedX = -speedMax;
        }


        public virtual void Jump()
        {
            actualSpeedY = 0;
            actualSpeedY += initialJumpForce;
            characterState = State.Jumping;
        }


        public virtual void NoMoveOnGround()
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

        public virtual void NoMoveOnAir()
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

        public void BounceRoof()
        {
            //characterState = State.Falling;
            //actualGravityAcceleration = gravityMax;
            if (actualSpeedY > 0)
                actualSpeedY = -actualSpeedY;
        }

        #endregion

    } // Character class

} // Ragab namespace