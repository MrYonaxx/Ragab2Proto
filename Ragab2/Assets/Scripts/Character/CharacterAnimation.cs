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
    /// Definition of the CharacterAnimation class
    /// </summary>
    public class CharacterAnimation : MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [SerializeField]
        Animator animator;

        [SerializeField]
        SpriteRenderer spriteRenderer;

        [SerializeField]
        Character characterToAnimate;

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
        


        public void CheckAnimation(float actualSpeedX)
        {
            if (characterToAnimate.Direction == 1)
                spriteRenderer.flipX = false;
            else
                spriteRenderer.flipX = true;

            if (characterToAnimate.characterState == State.Grouded)
            {
                if(actualSpeedX == 0)
                    animator.Play("Anim_Idle");
                else
                    animator.Play("Anim_Walk");
            }

            if (characterToAnimate.characterState == State.Falling)
            {
                animator.Play("Anim_Jump");
            }

            if (characterToAnimate.characterState == State.Sliding)
            {
                animator.Play("Anim_Slide");
            }

            if (characterToAnimate.characterState == State.Jumping)
            {
                animator.Play("Anim_Jump");
            }
        }
        
        #endregion

    } // CharacterAnimation class

} // Ragab namespace