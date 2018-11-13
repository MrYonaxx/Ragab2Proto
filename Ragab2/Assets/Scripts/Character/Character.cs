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
    public class Character : MonoBehaviour
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
            characterRigidbody = GetComponent<Rigidbody2D>();
            characterCollider = GetComponent<BoxCollider2D>();
        }

        protected void Update()
        {

            CheckState();

            ApplyGravity();
            UpdatePosition();
            characterRigidbody.velocity = new Vector2(actualSpeedX * Time.deltaTime, actualSpeedY * Time.deltaTime);

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
        private void UpdatePosition()
        {
            return;
            if (actualSpeedY < 0)
            {
                int layerMask = 1 << 8;

                Vector2 bottomLeft = new Vector2(characterCollider.bounds.min.x, characterCollider.bounds.min.y);
                Debug.Log(bottomLeft);
                RaycastHit2D raycastY = Physics2D.Raycast(bottomLeft,
                                  new Vector2(0, actualSpeedY * Time.deltaTime),
                                              Mathf.Abs(actualSpeedY * Time.deltaTime),
                                              layerMask);
                Debug.DrawRay(bottomLeft, new Vector2(0, actualSpeedY * Time.deltaTime), Color.green);
                if (raycastY.collider != null)
                {
                    Debug.Log(raycastY.collider.gameObject.name);
                    SetOnGround(true);
                    float distance = raycastY.point.y - transform.position.y;
                    actualSpeedY = distance / Time.deltaTime;
                    return;
                }

                Vector2 bottomRight = new Vector2(characterCollider.bounds.max.x, characterCollider.bounds.min.y);
                Debug.Log(bottomRight);
                raycastY = Physics2D.Raycast(bottomRight,
                                  new Vector2(0, actualSpeedY * Time.deltaTime),
                                              Mathf.Abs(actualSpeedY * Time.deltaTime),
                                              layerMask);
                Debug.DrawRay(bottomRight, new Vector2(0, actualSpeedY * Time.deltaTime), Color.green);
                if (raycastY.collider != null)
                {
                    Debug.Log(raycastY.collider.gameObject.name);
                    SetOnGround(true);
                    float distance = raycastY.point.y - transform.position.y;
                    actualSpeedY = distance / Time.deltaTime;
                    return;
                }
            }

            /*RaycastHit2D raycastX = Physics2D.Raycast(this.transform.position,
                  new Vector2(0, actualSpeedX * Time.deltaTime),
                              Mathf.Abs(actualSpeedX * Time.deltaTime),
                              layerMask);
            //Debug.DrawRay(this.transform.position, new Vector2(0, actualSpeedY), Color.green);
            if (raycastY.collider != null)
            {
                float distance = raycastX.point.x - transform.position.x;
                actualSpeedX = distance / Time.deltaTime;
            }*/

            //transform.Translate(new Vector3(actualSpeedX * Time.deltaTime, actualSpeedY * Time.deltaTime, 0));

            // ====================================================================================================
            //characterRigidbody.velocity = new Vector2(actualSpeedX * Time.deltaTime, actualSpeedY * Time.deltaTime);
            // ====================================================================================================
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