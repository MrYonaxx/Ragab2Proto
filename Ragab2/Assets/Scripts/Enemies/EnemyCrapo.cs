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
    /// Definition of the EnemyCrapo class
    /// </summary>
    public class EnemyCrapo : Enemy
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [Header("Robot Crap0")]
        [SerializeField]
        private float jumpStartup = 0.5f;
        [SerializeField]
        private float shootStartup = 1f;
        [SerializeField]
        private CrapoProjectile crapoShootPrefab = null;

        float jumpSpeed = 0;

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

        protected override void InitializePattern(int valMin = 0, int valMax = 0)
        {
            actualSpeedX = 0;
            if (characterState != State.Grouded)
            {
                return;
            }

            if (currentPattern == 0) // Initialize
            {
                currentPattern = Random.Range(1, 5);
            }
            else
            {
                currentPattern = Random.Range(valMin, valMax);
            }

            switch (currentPattern)
            {
                case 1: // Crap0 avance vers l'ennemi
                    EnemyLookAtPlayer();
                    timePattern = 3f;
                    break;
                case 2: // Crap0 tire sur l'ennemi
                    timePatternStartup = shootStartup;
                    timePattern = 1f;
                    break;
                case 3: // Crap0 regarde l'ennemi
                    timePattern = Random.Range(0.4f, 0.8f);
                    break;
                case 4: // Cac - Crap0 saute
                    timePatternStartup = jumpStartup;
                    timePattern = 1.5f;
                    break;
            }

            // Toujours mettre le select pattern ici juste après
            SelectPattern();
        }




        protected override void SelectPattern()
        {
            switch (currentPattern)
            {
                case 1: // Crap0 avance vers l'ennemi
                    LittleJump(3,5);
                    break;
                case 2: // Crap0 tire
                    if (timePatternStartup > 0)
                        EnemyShootStartup();
                    else
                        EnemyShoot();
                    break;
                case 3: // Crap0 regarde l'ennemi
                    EnemyLookAtPlayer();
                    break;
                case 4: // Crap0 saute sur l'ennemi
                    if (timePatternStartup > 0)  
                        EnemyJumpStartup();
                    else
                        EnemyJump();
                    break;
            }
        }


        private void LittleJump(int patternMin, int patternMax)
        {
            if (characterState == State.Grouded)
            {
                if (CheckCac())
                {
                    InitializePattern(patternMin, patternMax);
                    return;
                }
                actualSpeedY = 0;
                actualSpeedY += 8;
                actualSpeedX = speedMax * direction;
                characterState = State.Jumping;
            }
        }







        private void EnemyShootStartup()
        {
            if (timePatternStartup == shootStartup)
            {
                characterAnimation.SetAnimation("Anim_Shoot");
                EnemyLookAtPlayer();
                CrapoProjectile projectile = Instantiate(crapoShootPrefab, this.transform.position, Quaternion.identity);
                projectile.transform.position += new Vector3(0.2f * direction, 0, 0);
                if (direction == -1)
                    projectile.transform.eulerAngles += new Vector3(0, 0, 180);
            }
        }

        private void EnemyShoot()
        {
            if (timePattern > 0.9f)
            {
                characterAnimation.SetDefaultAnimation();
                direction *= -1;
                Move();
                direction *= -1;
            }
            else
            {
                NoMoveOnGround();
            }
        }





        private void EnemyJumpStartup()
        {
            if (timePatternStartup == jumpStartup)
            {
                characterAnimation.SetAnimation("Anim_Crouch");
                EnemyLookAtPlayer();
                jumpSpeed = characterToKill.transform.position.x - this.transform.position.x;
                if(jumpSpeed > speedMax)
                {
                    jumpSpeed = speedMax;
                }
                feedback.PlayFeedback(2);
            }
        }

        private void EnemyJump()
        {
            if(timePattern == 1.5f)
            {
                characterAnimation.SetDefaultAnimation();
                Jump();
            }

            if (characterState != State.Grouded)
            {
                NuanceJump();
                actualSpeedX = jumpSpeed * SlowMotionManager.Instance.playerTime;
            }
            else
            {
                NoMoveOnGround();
            }
        }

        private void NuanceJump()
        {
            actualSpeedY += additionalJumpForce * SlowMotionManager.Instance.playerTime;
        }






        private void Hit()
        {

        }


        #endregion

    } // EnemyCrapo class

} // #PROJECTNAME# namespace