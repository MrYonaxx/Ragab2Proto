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

    public enum State
    {
        Grouded,
        Crouching,
        Jumping,
        Falling
    };


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


        [Header("Debug")]
        public GameObject trailDebug = null;
        public bool activateDebugTrail = true;


        protected float actualAerialDecceleration = 0;
        protected float actualGravityAcceleration = 0;

        protected float actualSpeedX = 0;
        protected float actualSpeedY = 0;

        protected int direction = 1;

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
        }

        protected void Update()
        {
            CheckState();

            ApplyGravity();
            UpdatePosition();

            if (activateDebugTrail)
                Instantiate(trailDebug, this.transform.position, Quaternion.identity);
        }



        public void ChangeState(State newState)
        {

        }

        public void SetOnGround(bool b)
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
            characterRigidbody.velocity = new Vector2(actualSpeedX * Time.deltaTime, actualSpeedY * Time.deltaTime);
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

        public virtual void NuanceJump()
        {
            actualSpeedY += additionalJumpForce;
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

        #endregion

    } // Character class

} // Ragab namespace