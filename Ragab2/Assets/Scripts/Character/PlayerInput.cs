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

        bool traceDashPushed = false;

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
                    InputTraceDash();
                    break;

                case State.TraceDashing:
                    InputJump();
                    InputAim();
                    InputShoot();
                    InputPunch();
                    InputTraceDash();
                    break;

                case State.TraceDashingAiming:
                    InputAim();
                    InputPunch();
                    InputTraceDash();
                    InputReleaseTraceDash();
                    break;

                case State.TracePunching:
                    characterToControl.NoAim();
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
            if (Input.GetButtonDown("RagabJump"))
            {
                characterToControl.Jump();
            }
            else if (Input.GetButton("RagabJump"))
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
            if (Input.GetButton("RagabSlide"))
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
            if (Input.GetAxis("RagabShoot") > 0.2f)
            {
                characterToControl.Shoot(characterToControl.GetSpeed());
            }
        }

        private void InputPunch()
        {
            if (Input.GetButton("RagabPunch"))
            {
                characterToControl.TraceDashPunch();
            }
        }

        private void InputTraceDash()
        {
            if (Input.GetAxis("RagabTraceDash") > 0.2f && traceDashPushed == false)
            {
                characterToControl.TraceDashAim();
                traceDashPushed = true;
            }
            else if(Input.GetAxis("RagabTraceDash") <= 0.2f)
            {
                traceDashPushed = false;
            }
        }

        private void InputReleaseTraceDash()
        {
            if (Input.GetAxis("RagabTraceDash") <= 0.2f)
            {
                characterToControl.ReleaseTraceDashAim();
            }
        }

        #endregion

    } // PlayerInput class

} // #Ragab# namespace