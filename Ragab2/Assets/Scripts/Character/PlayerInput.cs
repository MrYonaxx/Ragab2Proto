/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Ragab
{

    [System.Serializable]
    public class UnityEventFloat : UnityEvent<Vector2>
    {

    }

    [System.Serializable]
    public class UnityEventVector3 : UnityEvent<Vector3>
    {

    }
    /// <summary>
    /// Definition of the PlayerInput class
    /// </summary>
    public class PlayerInput : MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [Header("Character To Control")]
        [SerializeField]
        RagabMovement characterToControl;

        [SerializeField]
        UnityEvent eventTraceDash;


        [SerializeField]
        UnityEvent eventStopTraceDash;

        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */


        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */

        ////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Update is called once per frame.
        /// </summary>
        protected void Update()
        {
            CheckState();  
        }


        private void CheckState()
        {
            switch(characterToControl.characterState)
            {
                case State.Grouded:
                    InputMoveOnGround();
                    InputJump();
                    InputSliding();
                    InputAim();
                    InputShoot();
                    InputTraceDash();
                    break;

                case State.Jumping:
                    InputMoveOnAir();
                    InputJump();
                    InputAim();
                    InputShoot();
                    InputTraceDash();
                    break;

                case State.Falling:
                    InputMoveOnAir();
                    InputJump();
                    InputAim();
                    InputShoot();
                    InputTraceDash();
                    break;

                case State.Sliding:
                    InputSliding();
                    InputJump();
                    InputAim();
                    InputShoot();
                    break;

                case State.TraceDashing:
                    InputJump();
                    InputAim();
                    InputShoot();
                    InputTraceDash();
                    break;

                case State.TraceDashingAiming:

                    break;
            }
        }


        private void InputMove()
        {
            if (Input.GetAxis("Horizontal") > 0.2f)
            {
                characterToControl.SetDirection(1);
                characterToControl.Move();
            }
            else if (Input.GetAxis("Horizontal") < -0.2f)
            {
                characterToControl.SetDirection(-1);
                characterToControl.Move();
            }
        }

        private void InputMoveOnGround()
        {
            InputMove();
            if (Input.GetAxis("Horizontal") <= 0.2f && Input.GetAxis("Horizontal") >= -0.2f)
                characterToControl.NoMoveOnGround();
        }

        private void InputMoveOnAir()
        {
            InputMove();
            if (Input.GetAxis("Horizontal") <= 0.2f && Input.GetAxis("Horizontal") >= -0.2f)
                characterToControl.NoMoveOnAir();
        }


        private void InputJump()
        {
            if (Input.GetButtonDown("Jump"))
            {
                characterToControl.Jump();
            }
            else if (Input.GetButton("Jump"))
            {
                characterToControl.NuanceJump();
            }
        }



        private void InputAim()
        {
            if (Input.GetAxis("AimHorizontal") > 0.2f || Input.GetAxis("AimHorizontal") < -0.2f || 
                Input.GetAxis("AimVertical") > 0.2f || Input.GetAxis("AimVertical") < -0.2f)
            {
                characterToControl.Aim(new Vector2(Input.GetAxis("AimHorizontal"), Input.GetAxis("AimVertical")));
            }
            else if (Input.GetAxis("Horizontal") > 0.2f || Input.GetAxis("Horizontal") < -0.2f ||
                     Input.GetAxis("Vertical") > 0.2f || Input.GetAxis("Vertical") < -0.2f)
            {
                characterToControl.Aim(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
            }
            else
            {
                characterToControl.NoAim();
            }
        }


        private void InputSliding()
        {
            if (Input.GetButton("Fire2"))
            {
                characterToControl.Sliding();
            }
            else
            {
                characterToControl.StopSliding();
            }
        }

        private void InputShoot()
        {
            if (Input.GetButton("Fire3"))
            {
                characterToControl.Shoot(characterToControl.GetSpeed());
            }
        }

        private void InputTraceDash()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                characterToControl.TraceDash();
                //eventTraceDash.Invoke();
            }
        }

        #endregion

    } // PlayerInput class

} // #Ragab# namespace