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
        float shootStartup = 1f;
        [SerializeField]
        CrapoProjectile projectile;

        [SerializeField]
        Vector3 origin;
        [SerializeField]
        float originRadius;

        Vector3 MovementTarget;

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

            if (currentPattern == 3) // Initialize
            {
                currentPattern = 2;
                if (CheckCac() == true)
                {
                    currentPattern = 4;
                }
            }
            if (currentPattern == 0) // Initialize
            {
                currentPattern = Random.Range(1, 4);
                if(CheckCac() == true)
                {
                    currentPattern = 4;
                }
            }

            switch (currentPattern)
            {
                case 1: // Corbeau tire sur l'ennemi
                    timePattern = shootStartup;
                    break;
                case 2: // Corbeau se pose des questions sur son existence
                    timePattern = Random.Range(0.2f, 1f);
                    break;
                case 3: // Corbeau se promène
                    timePattern = Random.Range(0.5f, 1f);
                    MovementTarget = origin + new Vector3(Random.Range(-originRadius, originRadius), Random.Range(-originRadius, originRadius), 0);
                    //actualSpeedX = Random.Range(-speedMax, speedMax);
                    //actualSpeedY = Random.Range(-speedMax, speedMax);
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
                case 1: // Corbeau tire
                    EnemyShoot();
                    break;
                case 2: // Corbeau regarde l'ennemi
                    EnemyLookAtPlayer();
                    break;
                case 3: // Corbeau se ballade
                    this.transform.position = Vector3.MoveTowards(this.transform.position, MovementTarget, speedMax * Time.deltaTime * SlowMotionManager.Instance.enemyTime);
                    EnemyLookAtPlayer();
                    break;
            }
        }


        private void EnemyShoot()
        {
            if (timePattern == shootStartup)
            {
                CrapoProjectile proj = Instantiate(projectile, this.transform.position, Quaternion.identity);
                float angle = Vector2.Angle((characterToKill.transform.position - this.transform.position).normalized, transform.right);
                proj.transform.eulerAngles = new Vector3(0, 0, -angle);
                //proj.transform.LookAt(characterToKill.transform, Vector3.right);
            }

        }


        #endregion

    } // EnemyCrow class

} // #PROJECTNAME# namespace