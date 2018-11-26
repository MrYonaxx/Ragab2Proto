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
    /// Definition of the EnemySoucoupe class
    /// </summary>
    public class EnemySoucoupe : Enemy
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        /*[Header("Patrouille")]
        [SerializeField]
        Transform Patrol[];*/

        [Header("Robot Soucoupe")]
        [SerializeField]
        int shootNumber = 3;
        [SerializeField]
        int shootStartup = 3;
        [SerializeField]
        CrapoProjectile projectilePrefab = null;

        [SerializeField]
        BoxCollider2D movingPlatform = null;


        int actualShootNumber;

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
            actualSpeedY = 0;

            /*if (currentPattern == 0) // Initialize
            {
                currentPattern = Random.Range(1, 4);
            }*/
            if (CheckCac() == true)
            {
                currentPattern = 1;
                actualShootNumber = shootNumber;
                timePatternStartup = shootStartup;
            }
           /*     switch (currentPattern)
            {

            }*/

            // Toujours mettre le select pattern ici juste après
            SelectPattern();
        }




        protected override void SelectPattern()
        {
            switch (currentPattern)
            {
                case 1: // Corbeau tire
                    if (timePatternStartup > 0)
                        EnemyShootStartup();
                    else
                        EnemyShoot();
                    break;
            }
        }


        private void EnemyShootStartup()
        {
            if (timePatternStartup == shootStartup)
            {
                //CrapoProjectile projectile = Instantiate(projectilePrefab, this.transform.position, Quaternion.identity);
                //projectile.transform.position += new Vector3(0, 0.5f, 0);
            }
        }

        private void EnemyShoot()
        {
            CrapoProjectile projectile = Instantiate(projectilePrefab, this.transform.position, Quaternion.identity);
            projectile.transform.position -= new Vector3(0, 0.5f, 0);
            projectile.transform.eulerAngles += new Vector3(0, 0, this.transform.eulerAngles.z - 90);
            projectile.transform.parent = this.transform;

            timePatternStartup = 0.2f;
            actualShootNumber -= 1;
            if (actualShootNumber == 0)
            {
                timePatternStartup = 0;
                timePattern = 0;
            }

        }


        protected override void Hit(float angle, float forceMultiplier = 1, int damage = 1)
        {
            if(statManager.Hp <= 0)
            {
                movingPlatform.enabled = false;
            }
            base.Hit(angle, forceMultiplier);
        }

        #endregion

    } // EnemySoucoupe class

} // #PROJECTNAME# namespace