using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ragab
{
    public class RagabMovement : Character
    {

        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [SerializeField]
        protected float secondBeforeJumpDisabled = 0.05f;



        #endregion

        // =============================================
        // Functions
        // =============================================














        /*public void SetGrounded(bool b)
        {
            isGrounded = b;
            // Place une sécurité pour sauter même si le perso tombe un peu
            if (b == false && isJumping == false)
            {
                StartCoroutine(WaitBeforeDisableJump(secondBeforeJumpDisabled));
            }
        }

        private IEnumerator WaitBeforeDisableJump(float second)
        {
            yield return new WaitForSeconds(second);
            jumpAvailable = false;
        }*/




    }

}
