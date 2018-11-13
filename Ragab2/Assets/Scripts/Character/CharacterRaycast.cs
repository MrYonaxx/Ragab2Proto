﻿/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using UnityEngine;
using System.Collections;

namespace Ragab
{

    public enum State
    {
        Grouded,
        Jumping,
        Falling,
        Sliding,
        TraceDashing
    };

    /// <summary>
    /// Definition of the Character class
    /// </summary>
    public class CharacterRaycast : MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        protected Rigidbody2D characterRigidbody;
        protected BoxCollider2D characterCollider;

        [Header("Movement")] // =============================================

        [SerializeField]
        protected float speedAcceleration = 1;
        [SerializeField]
        protected float speedMax = 5;
        [SerializeField]
        protected float aerialFriction = 0.01f;
        [SerializeField]
        protected float aerialInertia = 0.3f;


        [Header("Jump")]  // =============================================

        [SerializeField]
        protected float gravityForce = 1;
        [SerializeField]
        protected float gravityMax = 8;
        [SerializeField]
        protected float initialJumpForce = 10;
        [SerializeField]
        protected float additionalJumpForce = 0.7f;


        [Header("Collision")]
        [SerializeField]
        protected float maxAngle = 40;

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
        public bool activateDebugTrail = false;


        protected float actualAerialDecceleration = 0;
        protected float actualGravityAcceleration = 0;

        protected float actualSpeedX = 0;
        protected float actualSpeedY = 0;

        protected float bonusSpeedX = 0;
        protected float bonusSpeedY = 0;

        protected int direction = 1;

        public int Direction
        {
            get { return direction; }
        }



        // ======== Collisions =========== //
        bool climbingSlopes = false;

        int layerMask;
        Vector2 bottomLeft;
        Vector2 upperLeft;
        Vector2 bottomRight;
        Vector2 upperRight;


        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */

        public void SetDirection(int dir)
        {
            direction = dir;
        }

        public void SetSpeed(Vector2 newSpeed)
        {
            actualSpeedX = newSpeed.x;
            actualSpeedY = newSpeed.y;
        }


        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */



        protected virtual void Start()
        {
            Application.targetFrameRate = 120;
            characterRigidbody = GetComponent<Rigidbody2D>();
            characterCollider = GetComponent<BoxCollider2D>();
        }



        protected void Update()
        {

            CheckState();

            ApplyGravity();
            ApplySpeedBonus();

            UpdateCollision();

            if (characterAnimation != null)
                characterAnimation.CheckAnimation(characterState, direction, actualSpeedX);

        }



        private void UpdateCollision()
        {
            layerMask = 1 << 8;

            bottomLeft = new Vector2(characterCollider.bounds.min.x, characterCollider.bounds.min.y);
            upperLeft = new Vector2(characterCollider.bounds.min.x, characterCollider.bounds.max.y);
            bottomRight = new Vector2(characterCollider.bounds.max.x, characterCollider.bounds.min.y);
            upperRight = new Vector2(characterCollider.bounds.max.x, characterCollider.bounds.max.y);

            UpdatePositionX();
            UpdatePositionY();

            transform.position += new Vector3(actualSpeedX * Time.deltaTime, actualSpeedY * Time.deltaTime, 0);

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


        private void ApplySpeedBonus()
        {
            actualSpeedX += bonusSpeedX;
            actualSpeedY += bonusSpeedY;
            bonusSpeedX = 0;
            bonusSpeedY = 0;
        }

        private void ApplyGravity()
        {
            if (characterState == State.TraceDashing)
                return;

            actualSpeedY -= gravityForce;

            if (actualSpeedY < -gravityMax)
            {
                actualSpeedY = -gravityMax;
            }

        }



        // Update the position of the player
        private void UpdatePositionY()
        {
            if(actualSpeedY == 0)
            {
                return;
            }

            RaycastHit2D raycastY;
            Vector2 originRaycast;

            bool isFalling = true;

            if (actualSpeedY < 0)
            {
                originRaycast = bottomLeft;
            }
            else
            {
                originRaycast = upperLeft;
                transform.parent = null;
            }

            for (int i = 0; i < numberRaycastVertical; i++)
            {
                raycastY = ShootRaycast(originRaycast, new Vector2(0, actualSpeedY * Time.deltaTime), (actualSpeedY * Time.deltaTime), offsetRaycastY);

                if (raycastY.collider != null)
                {
                    // === Collision ==== //

                    isFalling = false;

                    if (raycastY.transform.gameObject.tag == "MovingPlatform" && actualSpeedY < 0)
                    {
                        this.transform.SetParent(raycastY.transform);
                    }
                    else
                    {
                        transform.parent = null;
                    }


                    float slopeAngle = Vector2.Angle(raycastY.normal, Vector2.up);
                    if (slopeAngle > maxAngle)
                    {
                        Debug.Log("je glisse");
                        isFalling = true;
                        //DescendSlope(slopeAngle);
                        //return;
                    }




                    if (actualSpeedY < 0)
                        SetOnGround(true);

                    float distance = raycastY.point.y - originRaycast.y;
                    distance -= offsetRaycastY * Mathf.Sign(actualSpeedY);
                    if (Mathf.Abs(actualSpeedY) > Mathf.Abs(distance / Time.deltaTime))
                    {
                        actualSpeedY = distance / Time.deltaTime;
                        CollisionY();
                    }

                    /*float slopeAngle = Vector2.Angle(raycastY.normal, Vector2.up);
                    if (slopeAngle != 0 && slopeAngle <= maxAngle && Mathf.Sign(raycastY.normal.x) == Mathf.Sign(actualSpeedX))
                    {
                        DescendSlope(slopeAngle);
                    }*/

                    //return;
                    // === Collision ==== //

                }

                if (actualSpeedY < 0)
                    originRaycast += new Vector2(Mathf.Abs(bottomRight.x - bottomLeft.x) / (numberRaycastVertical-1), 0);
                else
                    originRaycast += new Vector2(Mathf.Abs(upperRight.x - upperLeft.x) / (numberRaycastVertical - 1), 0);
            }

            if(actualSpeedY < 0 && isFalling == true)
                SetOnGround(false);  
        }



        private RaycastHit2D ShootRaycast(Vector2 origin, Vector2 direction, float length, float offset)
        {
            Debug.DrawRay(origin, direction, Color.green);
            return Physics2D.Raycast(origin, direction, Mathf.Abs(length) + offset, layerMask);
        }


        private void UpdatePositionX()
        {
            int layerMask = 1 << 8;
            Vector2 bottomLeft = new Vector2(characterCollider.bounds.min.x, characterCollider.bounds.min.y);
            Vector2 upperLeft = new Vector2(characterCollider.bounds.min.x, characterCollider.bounds.max.y);
            Vector2 bottomRight = new Vector2(characterCollider.bounds.max.x, characterCollider.bounds.min.y);
            Vector2 upperRight = new Vector2(characterCollider.bounds.max.x, characterCollider.bounds.max.y);

            RaycastHit2D raycastX;
            Vector2 originRaycast;

            if (actualSpeedX < 0)
            {

                originRaycast = bottomLeft;

                for (int i = 0; i < numberRaycastHorizontal; i++)
                {
                    raycastX = Physics2D.Raycast(originRaycast,
                                                 new Vector2(actualSpeedX * Time.deltaTime, 0),
                                                 Mathf.Abs(actualSpeedX * Time.deltaTime) + offsetRaycastX,
                                                 layerMask);
                    Debug.DrawRay(originRaycast, new Vector2(actualSpeedX * Time.deltaTime, 0), Color.red);
                    if (raycastX.collider != null)
                    {




                        // Déplacement vers la gauche

                        CollisionX();
                        float slopeAngle = Vector2.Angle(raycastX.normal, Vector2.up);
                        if (i == 0 && slopeAngle <= maxAngle)
                        {
                            Debug.Log("Climb");
                            float distance = raycastX.point.x - bottomLeft.x;
                            distance += offsetRaycastX;
                            float bonusX = distance / Time.deltaTime;
                            ClimbSlope(slopeAngle);
                            actualSpeedX += bonusX;

                        }
                        else
                        {
                            float distance = raycastX.point.x - bottomLeft.x;
                            distance += offsetRaycastX;
                            actualSpeedX = distance / Time.deltaTime;
                            return;
                        }



                        // Déplacement vers la gauche




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
                                          Mathf.Abs(actualSpeedX * Time.deltaTime) + offsetRaycastX,
                                          layerMask);
                    Debug.DrawRay(originRaycast, new Vector2(actualSpeedX * Time.deltaTime, 0), Color.red);
                    if (raycastX.collider != null)
                    {




                        // Déplacement vers la droite

                        CollisionX();
                        float slopeAngle = Vector2.Angle(raycastX.normal, Vector2.up);
                        if (i == 0 && slopeAngle <= maxAngle)
                        {
                            float distance = raycastX.point.x - bottomRight.x;
                            distance -= offsetRaycastX;
                            float bonusX = distance / Time.deltaTime;
                            ClimbSlope(slopeAngle);
                            actualSpeedX += bonusX;
                        }
                        else
                        {
                            climbingSlopes = false;
                            float distance = raycastX.point.x - bottomRight.x;
                            distance -= offsetRaycastX;
                            actualSpeedX = distance/ Time.deltaTime;
                            return;
                        }



                        // Déplacement vers la droite




                    }
                    originRaycast += new Vector2(0, Mathf.Abs(upperRight.y - bottomRight.y) / (numberRaycastHorizontal - 1));
                }


            }

        }


        private void ClimbSlope(float angle)
        {
            climbingSlopes = true;
            if (characterState != State.Jumping)
            {
                actualSpeedY = Mathf.Sin(angle * Mathf.Deg2Rad) * Mathf.Abs(actualSpeedX);
                actualSpeedX = Mathf.Cos(angle * Mathf.Deg2Rad) * Mathf.Abs(actualSpeedX) * Mathf.Sign(actualSpeedX);
                SetOnGround(true);
            }
        }

        private void DescendSlope(float angle)
        {

            actualSpeedX = actualSpeedY; // Mathf.Abs(speedAcceleration) * -direction;
            //actualSpeedY = Mathf.Sin(angle * Mathf.Deg2Rad) * Mathf.Abs(speedAcceleration);
            /*climbingSlopes = true;
            if (characterState != State.Jumping)
            {
                actualSpeedX = Mathf.Cos(angle * Mathf.Deg2Rad) * Mathf.Abs(actualSpeedX) * Mathf.Sign(actualSpeedX);
                actualSpeedY -= Mathf.Sin(angle * Mathf.Deg2Rad) * Mathf.Abs(actualSpeedX);
                SetOnGround(true);
            }*/
        }


        private void CollisionY()
        {
            if (characterState == State.TraceDashing)
                characterState = State.Falling;
        }


        private void CollisionX()
        {
            if (characterState == State.TraceDashing)
                characterState = State.Falling;
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