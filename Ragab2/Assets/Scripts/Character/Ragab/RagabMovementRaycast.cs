using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ragab
{
    public class RagabMovementRaycast : CharacterRaycast
    {

        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [SerializeField]
        protected float secondBeforeJumpDisabled = 0.05f;

        [Header("Slide")]
        [SerializeField]
        protected float slideSpeed = 500;

        private bool jumpAvailable = true;

        public bool JumpAvailable
        {
            get { return jumpAvailable; }
        }


        #endregion

        // =============================================
        // Functions
        // =============================================





        public virtual void NuanceJump()
        {
            actualSpeedY += additionalJumpForce;
        }



        public override void SetOnGround(bool b)
        {
            if (b == true)
            {
                jumpAvailable = true;
                characterState = State.Grouded;
            }
            else
            {
                if (characterState != State.Jumping)
                {
                    characterState = State.Falling;
                    StartCoroutine(WaitBeforeDisableJump(secondBeforeJumpDisabled));
                }
            }
        }

        public void Sliding()
        {
            characterState = State.Sliding;
            actualSpeedX = slideSpeed * direction;
        }

        public void StopSliding()
        {
            characterState = State.Grouded;
        }





        /*public void SetGrounded(bool b)
        {
            isGrounded = b;
            // Place une sécurité pour sauter même si le perso tombe un peu
            if (b == false && isJumping == false)
            {
                StartCoroutine(WaitBeforeDisableJump(secondBeforeJumpDisabled));
            }
        }*/

        private IEnumerator WaitBeforeDisableJump(float second)
        {
            yield return new WaitForSeconds(second);
            jumpAvailable = false;
        }


        public override void Jump()
        {
            actualSpeedY = 0;
            actualSpeedY += initialJumpForce;
            characterState = State.Jumping;
            jumpAvailable = false;
        }

    }

}
