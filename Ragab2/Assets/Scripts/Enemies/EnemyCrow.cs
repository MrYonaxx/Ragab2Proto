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
    /// Definition of the EnemyCrow class
    /// </summary>
    public class EnemyCrow : Enemy
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [Header("Robot Corbeau")]
        [SerializeField]
        float shootStartup;
        [SerializeField]
        CrapoProjectile projectile;

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
                case 1: // Corbeau tire sur l'ennemi
                    timePatternStartup = shootStartup;
                    timePattern = 1f;
                    break;
                case 2: // Corbeau se pose des questions sur son existence
                    timePattern = Random.Range(1f, 2f);
                    break;
                case 3: // Corbeau se promène
                    timePattern = 3f;
                    break;
                case 4: // Corbeau kamikaze
                    timePatternStartup = shootStartup;
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
            CrapoProjectile proj = Instantiate(projectile, this.transform.position, Quaternion.identity);
            float angle = Vector3.Angle(characterToKill.transform.position, this.transform.position);
            projectile.transform.eulerAngles = new Vector3(0,0,angle);

        }


        #endregion

    } // EnemyCrow class

} // #PROJECTNAME# namespace