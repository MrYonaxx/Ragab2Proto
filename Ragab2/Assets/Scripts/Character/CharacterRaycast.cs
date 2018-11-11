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


        [Header("Collision")]
        [SerializeField]
        protected float offsetRaycastX = 0.0001f;
        [SerializeField]
        protected float offsetRaycastY = 0.0001f;
        [SerializeField]
        protected int numberRaycastVertical = 2;
        [SerializeField]
        protected int numberRaycastHorizontal = 2;

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
            Application.targetFrameRate = 60;
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
            Vector2 bottomLeft = new Vector2(characterCollider.bounds.min.x + offsetRaycastX, characterCollider.bounds.min.y);
            Vector2 upperLeft = new Vector2(characterCollider.bounds.min.x + offsetRaycastX, characterCollider.bounds.max.y);
            Vector2 bottomRight = new Vector2(characterCollider.bounds.max.x - offsetRaycastX, characterCollider.bounds.min.y);
            Vector2 upperRight = new Vector2(characterCollider.bounds.max.x - offsetRaycastX, characterCollider.bounds.max.y);

            RaycastHit2D raycastY;
            Vector2 originRaycast;

            if (actualSpeedY < 0)
            {
                originRaycast = bottomLeft;
                for (int i = 0; i < numberRaycastVertical; i++)
                {
                    raycastY = Physics2D.Raycast(originRaycast,
                                                 new Vector2(0, actualSpeedY * Time.deltaTime),
                                                 Mathf.Abs(actualSpeedY * Time.deltaTime),
                                                 layerMask);
                    Debug.DrawRay(originRaycast, new Vector2(0, actualSpeedY * Time.deltaTime), Color.green);
                    if (raycastY.collider != null)
                    {
                        //Debug.Log(raycastY.collider.gameObject.name);
                        SetOnGround(true);
                        float distance = raycastY.point.y - transform.position.y;
                        actualSpeedY = distance / Time.deltaTime;
                        return;
                    }
                    originRaycast += new Vector2(Mathf.Abs(bottomRight.x - bottomLeft.x) / (numberRaycastVertical-1), 0);
                }
            }
            else if (actualSpeedY > 0)
            {
                originRaycast = upperLeft;
                for (int i = 0; i < numberRaycastVertical; i++)
                {
                    raycastY = Physics2D.Raycast(originRaycast,
                                                 new Vector2(0, actualSpeedY * Time.deltaTime),
                                                 Mathf.Abs(actualSpeedY * Time.deltaTime),
                                                 layerMask);
                    Debug.DrawRay(originRaycast, new Vector2(0, actualSpeedY * Time.deltaTime), Color.green);
                    if (raycastY.collider != null)
                    {
                        //Debug.Log(raycastY.collider.gameObject.name);
                        //SetOnGround(true);
                        float distance = raycastY.point.y - upperLeft.y;
                        actualSpeedY = distance / Time.deltaTime;
                        //Debug.Log("Ouille je me cogne la tête");
                        return;
                    }
                    originRaycast += new Vector2(Mathf.Abs(upperRight.x - upperLeft.x) / (numberRaycastVertical - 1), 0);
                }
            }
        }


        private void UpdatePositionX()
        {
            int layerMask = 1 << 8;
            Vector2 bottomLeft = new Vector2(characterCollider.bounds.min.x, characterCollider.bounds.min.y + offsetRaycastY);
            Vector2 upperLeft = new Vector2(characterCollider.bounds.min.x, characterCollider.bounds.max.y - offsetRaycastY);
            Vector2 bottomRight = new Vector2(characterCollider.bounds.max.x, characterCollider.bounds.min.y + offsetRaycastY);
            Vector2 upperRight = new Vector2(characterCollider.bounds.max.x, characterCollider.bounds.max.y - offsetRaycastY);

            RaycastHit2D raycastX;
            Vector2 originRaycast;

            if (actualSpeedX < 0)
            {

                originRaycast = bottomLeft;

                for (int i = 0; i < numberRaycastHorizontal; i++)
                {
                    raycastX = Physics2D.Raycast(originRaycast,
                                                 new Vector2(actualSpeedX * Time.deltaTime, 0),
                                                 Mathf.Abs(actualSpeedX * Time.deltaTime),
                                                 layerMask);
                    Debug.DrawRay(originRaycast, new Vector2(actualSpeedX * Time.deltaTime, 0), Color.red);
                    if (raycastX.collider != null)
                    {
                        Debug.Log("Zbla");
                        float distance = raycastX.point.x - bottomLeft.x;
                        actualSpeedX = distance / Time.deltaTime;
                        return;
                    }
                    originRaycast += new Vector2(0, Mathf.Abs(upperLeft.y - bottomLeft.y) / (numberRaycastHorizontal - 1));
                }

            }
            else if (actualSpeedX > 0)
            {

                originRaycast = bottomRight;

                for (int i = 0; i < numberRaycastHorizontal; i++)
                {
                    raycastX = Physics2D.Raycast(originRaycast,
                                          new Vector2(actualSpeedX * Time.deltaTime, 0),
                                          Mathf.Abs(actualSpeedX * Time.deltaTime),
                                          layerMask);
                    Debug.DrawRay(originRaycast, new Vector2(actualSpeedX * Time.deltaTime, 0), Color.green);
                    if (raycastX.collider != null)
                    {
                        Debug.Log("ZblaDroite");
                        float distance = raycastX.point.x - bottomRight.x;
                        actualSpeedX = distance / Time.deltaTime;
                        return;
                    }
                    originRaycast += new Vector2(0, Mathf.Abs(upperRight.y - bottomRight.y) / (numberRaycastHorizontal - 1));
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