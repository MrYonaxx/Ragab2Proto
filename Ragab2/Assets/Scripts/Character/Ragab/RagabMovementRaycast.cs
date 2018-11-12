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
        protected float slideSpeed = 12;
        // à changer
        [SerializeField]
        protected BoxCollider2D defaultCollider;

        private bool jumpAvailable = true;

        public bool JumpAvailable
        {
            get { return jumpAvailable; }
        }



        #endregion

        // =============================================
        // Functions
        // =============================================

        public Vector3 GetSpeed()
        {
            return new Vector3(Mathf.Abs(actualSpeedX), Mathf.Abs(actualSpeedY), 0);
        }


        public void ChangeCollider(BoxCollider2D newCollider)
        {
            characterCollider.enabled = false;
            characterCollider = newCollider;
            characterCollider.enabled = true;

        }

        public virtual void NuanceJump()
        {
            actualSpeedY += additionalJumpForce;
        }



        public override void SetOnGround(bool b)
        {
            if (b == true)
            {
                jumpAvailable = true;
                if (characterState != State.Sliding)
                    characterState = State.Grouded;
            }
            else
            {
                StopSliding();
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
            bonusSpeedY += -(slideSpeed * 2);
        }

        public void StopSliding()
        {
            //CheckIfCanStopSlide();
            bonusSpeedY = 0;
            characterState = State.Grouded;
            ChangeCollider(defaultCollider);
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


        void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("touche");
        }

    }

}
