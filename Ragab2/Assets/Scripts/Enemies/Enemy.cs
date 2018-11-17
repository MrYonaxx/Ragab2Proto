/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Ragab
{
    /// <summary>
    /// Definition of the Enemy class
    /// </summary>
    public class Enemy : Character
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [Header("Ressource")]
        [SerializeField]
        protected StatManager statManager;
        [SerializeField]
        protected FeedbackManager feedback;

        [Header("Aggro")]
        [SerializeField]
        protected Character characterToKill = null;
        [SerializeField]
        protected bool agressive = false;
        [SerializeField]
        protected float aggroX = 1;
        [SerializeField]
        protected float aggroY = 1;
        [SerializeField]
        protected float aggroAgressiveX = 10;
        [SerializeField]
        protected float aggroAgressiveY = 10;

        [SerializeField]
        protected float distanceCacX = 10;
        [SerializeField]
        protected float distanceCacY = 10;

        [Header("Knockback")]
        [SerializeField]
        protected float superArmor = 3; 
        [SerializeField]
        protected float knockbackTime = 0.12f;
        [SerializeField]
        protected float knockbackForce = 10;

        public int currentPattern = 0;
        public float timePattern = 0;
        public float timePatternStartup = 0;

        protected float actualKnockbackTime = 0;
        protected float actualSuperArmor = 0;

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


        protected override void Update()
        {

            if(characterState == State.Knockback)
            {
                UpdateKnockback();
            }
            else
            {
                if (agressive = CheckAggro())
                    EnemyIA();
            }

            base.Update();
        }


        protected void UpdateKnockback()
        {
            NoMoveOnAir();
            actualKnockbackTime -= Time.deltaTime * SlowMotionManager.Instance.enemyTime;
            if(actualKnockbackTime < 0)
            {
                actualSuperArmor = superArmor;
                actualKnockbackTime = 0;
                characterState = State.Falling;
            }
        }


        protected bool CheckAggro()
        {
            if (agressive)
            {
                if (this.transform.position.x - aggroX < characterToKill.transform.position.x &&
                   characterToKill.transform.position.x < this.transform.position.x + aggroX &&
                   this.transform.position.y - aggroY < characterToKill.transform.position.y &&
                   characterToKill.transform.position.y < this.transform.position.y + aggroY)
                {
                    return true;
                }
            }
            else
            {
                if (this.transform.position.x - aggroAgressiveX < characterToKill.transform.position.x &&
                    characterToKill.transform.position.x < this.transform.position.x + aggroAgressiveX &&
                    this.transform.position.y - aggroAgressiveY < characterToKill.transform.position.y &&
                    characterToKill.transform.position.y < this.transform.position.y + aggroAgressiveY)
                {
                    return true;
                }
            }
            return false;
        }

        protected void EnemyIA()
        {

            if (currentPattern == 0)
            {
                InitializePattern();
            }
            else
            {
                SelectPattern();
            }


            if (timePatternStartup > 0)
            {
                timePatternStartup -= Time.deltaTime * SlowMotionManager.Instance.enemyTime;
            }
            else
            {
                timePatternStartup = 0;
                if (timePattern > 0)
                {
                    timePattern -= Time.deltaTime * SlowMotionManager.Instance.enemyTime;
                }
                else
                {
                    currentPattern = 0;
                }
            }


        }



        protected virtual void InitializePattern(int valMin = 0, int valMax = 0)
        {

        }

        protected virtual void SelectPattern()
        {

        }




        protected void EnemyLookAtPlayer()
        {
            if(characterToKill.transform.position.x < this.transform.position.x)
            {
                direction = -1;
            }
            if (characterToKill.transform.position.x > this.transform.position.x)
            {
                direction = 1;
            }
        }

        protected void EnemyMove(int patternMin, int patternMax)
        {
            if (CheckCac())
            {
                InitializePattern(patternMin, patternMax);
            }
            else
            {
                EnemyLookAtPlayer();
                Move();
            }


        }

        protected bool CheckCac()
        {
            if (this.transform.position.x - distanceCacX < characterToKill.transform.position.x &&
                characterToKill.transform.position.x < this.transform.position.x + distanceCacX &&
                this.transform.position.y - distanceCacY < characterToKill.transform.position.y &&
                characterToKill.transform.position.y < this.transform.position.y + distanceCacY)
            {
                return true;
            }
            return false;
        }




        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.tag == "ProjectilePlayer")
            {
                Hit(collision.transform.eulerAngles.z);
            }
        }

        protected virtual void Hit(float angle)
        {
            statManager.Hp -= 1;
            if(statManager.Hp == 0)
            {
                Destroy(this.gameObject);
                return;
            }
            feedback.PlayFeedback(0);
            if (actualSuperArmor <= 0)
            {
                feedback.PlayFeedback(1);
                characterAnimation.SetDefaultAnimation();
                actualKnockbackTime = knockbackTime;
                characterState = State.Knockback;
                SetSpeed(new Vector2(knockbackForce * Mathf.Cos(angle * Mathf.PI / 180f),
                                     knockbackForce * Mathf.Sin(angle * Mathf.PI / 180f)));
            }
            else
            {
                actualSuperArmor -= 1;
            }
        }

        #endregion

    } // Enemy class

} // #PROJECTNAME# namespace