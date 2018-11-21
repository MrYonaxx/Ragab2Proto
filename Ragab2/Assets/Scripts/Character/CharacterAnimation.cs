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
        Animator animator = null;

        [SerializeField]
        SpriteRenderer spriteRenderer = null;

        [Header("TraceDash")]
        [SerializeField]
        Transform objectToRotate;


        [Header("Sound")]
        [SerializeField]
        AudioSource audioSource;
        [SerializeField]
        AudioClip[] sounds;

        bool isTraceDashing = false;

        int direction = 0;

        bool customAnim = false;

        bool knockbackAnim = false;

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



        public void CheckAnimation(State actualState, int actualDirection, float actualSpeedX)
        {
            direction = actualDirection;
            if (actualDirection == 1)
                spriteRenderer.flipX = false;
            else
                spriteRenderer.flipX = true;

            if (animator == null || customAnim == true)
                return;

            if (actualState == State.Grouded)
            {
                if(actualSpeedX == 0)
                    animator.Play("Anim_Idle");
                else
                    animator.Play("Anim_Walk");
            }

            if (actualState == State.Falling)
            {
                animator.Play("Anim_Jump");
            }

            if (actualState == State.Sliding)
            {
                animator.Play("Anim_Slide");
            }

            if (actualState == State.Jumping)
            {
                animator.Play("Anim_Jump");
            }

            if (actualState == State.TraceDashing)
            {
                animator.Play("Anim_Airdash");
            }

            if (actualState == State.Knockback)
            {
                if(knockbackAnim == false)
                    animator.Play("Anim_Hit");
                else
                    animator.Play("Anim_Idle");
            }

            if (actualState == State.TracePunching)
            {
                animator.Play("Anim_DashPunch");
            }
        }
        
        public void SetDefaultAnimation()
        {
            customAnim = false;
        }

        public void SetAnimation(string animName)
        {
            customAnim = true;
            animator.Play(animName);
        }

        public void SetSpriteRotation()
        {
            objectToRotate.eulerAngles = new Vector3(0, 0, 0);
        }

        public void SetSpriteRotation(Transform newRotation)
        {
            objectToRotate.eulerAngles = newRotation.eulerAngles;
            if (direction == -1)
                objectToRotate.eulerAngles += new Vector3(0, 0, 180);
        }









        public void ChangeKnockbackAnim()
        {
            knockbackAnim = !knockbackAnim;
        }











        public void playSoundWalkRight()
        {
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.PlayOneShot(sounds[0]); // Son droite
        }

        public void playSoundWalkLeft()
        {
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.PlayOneShot(sounds[1]); // Son Gauche
        }

        #endregion

    } // CharacterAnimation class

} // Ragab namespace