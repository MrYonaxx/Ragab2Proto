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

        [Header("Robot Corbeau Mouvement")]
        [SerializeField]
        Vector3 origin;
        [SerializeField]
        float originRadius;

        [Header("Robot Corbeau Kamikaze")]
        [SerializeField]
        float kamikazeStartup = 1f;
        [SerializeField]
        float kamikazeSpeed = 1f;
        [SerializeField]
        float kamikazeFriction = 1f;

        bool kamikaze = false;

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
            }
            if (currentPattern == 0) // Initialize
            {
                currentPattern = Random.Range(1, 4);
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
                    timePatternStartup = kamikazeStartup;
                    timePattern = 100f;
                    break;
            }

            // Toujours mettre le select pattern ici juste après
            SelectPattern();
        }




        protected override void SelectPattern()
        {
            if (CheckCac() == true && currentPattern != 4)
            {
                currentPattern = 4;
                timePatternStartup = kamikazeStartup;
                timePattern = 100f;
            }
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
                case 4:
                    if (timePatternStartup > 0)
                        EnemyKamikazeStartup();
                    else
                        EnemyKamikaze();
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


        private void EnemyKamikazeStartup()
        {
            if (timePatternStartup == kamikazeStartup)
            {
                characterAnimation.SetAnimation("Anim_Kamikaze");
            }
            actualSpeedY = speedAcceleration;
        }

        private void EnemyKamikaze()
        {
            if (timePattern == 100)
            {
                kamikaze = true;
                float angle = Vector2.Angle((characterToKill.transform.position - this.transform.position).normalized, transform.right);
                SetSpeed(new Vector2(kamikazeSpeed * Mathf.Cos(-angle * Mathf.PI / 180f),
                                     kamikazeSpeed * Mathf.Sin(-angle * Mathf.PI / 180f)));
                characterAnimation.SetAnimation("Anim_Kamikaze");
            }
        }

        protected override void CollisionY()
        {
            if (kamikaze == true)
                statManager.Hp = 0;
            base.CollisionY();
        }

        protected override void CollisionX()
        {
            if (kamikaze == true)
                statManager.Hp = 0;
            base.CollisionY();
        }


        #endregion

    } // EnemyCrow class

} // #PROJECTNAME# namespace