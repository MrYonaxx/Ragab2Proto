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

    /// <summary>
    /// Definition of the PlayerInput class
    /// </summary>
    public class PlayerInputRaycast : MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [SerializeField]
        RagabMovementRaycast characterToControl;

        [SerializeField]
        UnityEventFloat eventAim;

        [SerializeField]
        UnityEvent eventNoAim;

        [SerializeField]
        UnityEvent eventShoot;

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
                    InputGrounded();
                    break;
                case State.Jumping:
                    InputJumping();
                    break;
                case State.Falling:
                    InputFalling();
                    break;
                case State.Sliding:
                    InputSliding();
                    break;
            }
        }



        private void InputGrounded()
        {
            if(Input.GetAxis("Horizontal") > 0.2f)
            {
                characterToControl.SetDirection(1);
                characterToControl.Move();
            }
            else if (Input.GetAxis("Horizontal") < -0.2f)
            {
                characterToControl.SetDirection(-1);
                characterToControl.Move();
            }
            else
            {
                characterToControl.NoMoveOnGround();
            }

            if (Input.GetButtonDown("Jump"))
            {
                characterToControl.Jump();
            }

            if (Input.GetButtonDown("Fire2"))
            {
                characterToControl.Sliding();
            }

            // Aim
            if (Input.GetAxis("AimHorizontal") > 0.2f || Input.GetAxis("AimHorizontal") < -0.2f || Input.GetAxis("AimVertical") > 0.2f || Input.GetAxis("AimVertical") < -0.2f)
            {
                eventAim.Invoke(new Vector2(Input.GetAxis("AimHorizontal"), Input.GetAxis("AimVertical")));
            }
            else
            {
                eventNoAim.Invoke();
            }


        }



        private void InputJumping()
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
            else
            {
                characterToControl.NoMoveOnAir();
            }



            if (Input.GetButton("Jump"))
            {
                characterToControl.NuanceJump();
            }
            else
            {
                characterToControl.characterState = State.Falling;
            }



            // Aim
            if (Input.GetAxis("AimHorizontal") > 0.2f || Input.GetAxis("AimHorizontal") < -0.2f || Input.GetAxis("AimVertical") > 0.2f || Input.GetAxis("AimVertical") < -0.2f)
            {
                eventAim.Invoke(new Vector2(Input.GetAxis("AimHorizontal"), Input.GetAxis("AimVertical")));
            }
            else
            {
                eventNoAim.Invoke();
            }
        }



        private void InputFalling()
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
            else
            {
                characterToControl.NoMoveOnAir();
            }

            if (Input.GetButtonDown("Jump") && characterToControl.JumpAvailable == true)
            {
                characterToControl.Jump();
            }
            else if (Input.GetButton("Jump"))
            {
                characterToControl.NuanceJump();
            }



            // Aim
            if (Input.GetAxis("AimHorizontal") > 0.2f || Input.GetAxis("AimHorizontal") < -0.2f || Input.GetAxis("AimVertical") > 0.2f || Input.GetAxis("AimVertical") < -0.2f)
            {
                eventAim.Invoke(new Vector2(Input.GetAxis("AimHorizontal"), Input.GetAxis("AimVertical")));
            }
            else
            {
                eventNoAim.Invoke();
            }
        }


        private void InputSliding()
        {
            if (Input.GetButtonUp("Fire2"))
            {
                characterToControl.StopSliding();
            }

            if (Input.GetButtonDown("Jump") && characterToControl.JumpAvailable == true)
            {
                characterToControl.Jump();
            }



            // Aim
            if (Input.GetAxis("AimHorizontal") > 0.2f || Input.GetAxis("AimHorizontal") < -0.2f || Input.GetAxis("AimVertical") > 0.2f || Input.GetAxis("AimVertical") < -0.2f)
            {
                eventAim.Invoke(new Vector2(Input.GetAxis("AimHorizontal"), Input.GetAxis("AimVertical")));
            }
            else
            {
                eventNoAim.Invoke();
            }
        }

        #endregion

    } // PlayerInput class

} // #Ragab# namespace