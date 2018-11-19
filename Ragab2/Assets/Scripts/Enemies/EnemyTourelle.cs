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
    /// Definition of the EnemyTourelle class
    /// </summary>
    public class EnemyTourelle : Enemy
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [Header("Robot Tourelle")]
        [SerializeField]
        float shootStartup = 1f;
        [SerializeField]
        float shootNumber = 3;
        [SerializeField]
        private CrapoProjectile crapoShootPrefab = null;

        private float actualShootNumber = 0;

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
                currentPattern = Random.Range(1, 3);
            }
            else
            {
                currentPattern = Random.Range(valMin, valMax);
            }

            switch (currentPattern)
            {
                case 1: // Tourelle tire sur l'ennemi
                    actualShootNumber = 3;
                    timePatternStartup = shootStartup;
                    timePattern = 1f;
                    break;
                case 2: // Tourelle regarde l'ennemi
                    timePattern = Random.Range(0.4f, 0.8f);
                    break;
                case 3: // Tourelle fuit l'ennemi
                    timePattern = 3f;
                    break;
            }

            // Toujours mettre le select pattern ici juste après
            SelectPattern();
        }




        protected override void SelectPattern()
        {
            switch (currentPattern)
            {
                case 1: // Tourelle tire
                    if (timePatternStartup > 0)
                        EnemyShootStartup();
                    else
                        EnemyShoot();
                    break;
                case 2: // Tourelle regarde l'ennemi
                    EnemyLookAtPlayer();
                    break;
                case 3: // 
                    characterAnimation.SetDefaultAnimation();
                    EnemyLookAtPlayer();
                    actualSpeedX *= -1;
                    Move();
                    direction *= -1;
                    actualSpeedX *= -1;
                    break;
            }
        }


        private void EnemyShootStartup()
        {
            if (timePatternStartup == shootStartup)
            {
                characterAnimation.SetAnimation("Anim_Shoot");
                EnemyLookAtPlayer();
            }
        }

        private void EnemyShoot()
        {
            CrapoProjectile projectile = Instantiate(crapoShootPrefab, this.transform.position, Quaternion.identity);
            projectile.transform.position += new Vector3(0.8f * direction, 0, 0);
            if (direction == -1)
                projectile.transform.eulerAngles += new Vector3(0, 0, 180);
            timePatternStartup = 0.2f;
            actualShootNumber -= 1;
            if(actualShootNumber == 0)
            {
                timePatternStartup = 0;
                timePattern = 0;
            }

        }

        #endregion

    } // EnemyTourelle class

} // #PROJECTNAME# namespace