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
        }

        #endregion

    } // PlayerInput class

} // #Ragab# namespace