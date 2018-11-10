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

    /// <summary>
    /// Definition of the PlayerInput class
    /// </summary>
    public class PlayerInput : MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [SerializeField]
        RagabMovement characterToControl;

        [SerializeField]
        UnityEventFloat eventAim;

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
                Debug.Log("Hey");
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
        }

        #endregion

    } // PlayerInput class

} // #Ragab# namespace