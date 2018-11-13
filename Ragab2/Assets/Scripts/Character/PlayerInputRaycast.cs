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
    public class UnityEventVector3 : UnityEvent<Vector3>
    {

    }
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
        UnityEventVector3 eventShoot;

        [SerializeField]
        UnityEvent eventSlide;

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
                case State.TraceDashing:
                    InputTraceDashing();
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

            if (Input.GetButton("Fire2"))
            {
                eventSlide.Invoke();
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


            if (Input.GetButton("Fire3"))
            {
                eventShoot.Invoke(characterToControl.GetSpeed());
            }


            if (Input.GetButtonDown("Fire1"))
            {
                eventTraceDash.Invoke();
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

            if (Input.GetButton("Fire3"))
            {
                eventShoot.Invoke(characterToControl.GetSpeed());
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

            if (Input.GetButton("Fire3"))
            {
                eventShoot.Invoke(characterToControl.GetSpeed());
            }
        }


        private void InputSliding()
        {
            if (Input.GetButton("Fire2"))
            {
                characterToControl.Sliding();
            }
            if (Input.GetButtonUp("Fire2"))
            {
                characterToControl.StopSliding();
            }

            if (Input.GetButtonDown("Jump") && characterToControl.JumpAvailable == true)
            {
                characterToControl.StopSliding();
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

            if (Input.GetButton("Fire3"))
            {
                eventShoot.Invoke(characterToControl.GetSpeed());
            }
        }


        private void InputTraceDashing()
        {
            if(Input.GetButtonDown("Fire1"))
            {
                eventStopTraceDash.Invoke();
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

            if (Input.GetButton("Fire3"))
            {
                eventShoot.Invoke(characterToControl.GetSpeed());
            }
        }

        #endregion

    } // PlayerInput class

} // #Ragab# namespace