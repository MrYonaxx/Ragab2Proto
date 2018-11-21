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

        [Header("---  Knockback  ---")]
        [SerializeField]
        protected float superArmor = 3; 
        [SerializeField]
        protected float knockbackTime = 0.12f;

        [SerializeField]
        protected bool canCombo = false;

        [Header("---  Break  ---")]
        [SerializeField]
        protected float breakSuperArmor = 3;
        [SerializeField]
        protected float breakTime = 1f;
        [SerializeField]
        protected float breakInitialYForce = 3;
        [SerializeField]
        protected float breakAdditionalYForce = 1;
        [SerializeField]
        protected bool canComboBreak = false;

        [Header("---  Knockback Force  ---")]
        [SerializeField]
        protected float groundForce = 10;
        [SerializeField]
        protected float aerialForce = 3;
        [SerializeField]
        protected bool bounceGround = true;


        [Header("Pattern Debug")]
        public int currentPattern = 0;
        public float timePattern = 0;
        public float timePatternStartup = 0;

        protected float actualKnockbackTime = 0;
        protected float actualSuperArmor = 0;
        protected float actualBreakArmor = 0;
        protected float actualKnockbackForce = 0;
        public bool bounce = false;

        bool isKnockbackGround = true;

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

        protected override void Start()
        {
            base.Start();
            actualSuperArmor = superArmor;
            actualBreakArmor = breakSuperArmor;
    }

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










        public override void SetOnGround(bool b)
        {
            if (characterState == State.Knockback)
            {
                isKnockbackGround = b;
                return;
            }

            if (b == true)
            {
                characterState = State.Grouded;
            }
            else
            {
                if (characterState != State.Jumping)
                    characterState = State.Falling;
            }
        }


        protected override void CollisionY()
        {
            base.CollisionY();
            if (actualBreakArmor == -1)
            {
                if (bounce == false)
                {
                    actualSpeedY *= -1;
                    bounce = true;
                }
                else
                {
                    actualBreakArmor = breakSuperArmor;
                    actualKnockbackTime = 0;
                    characterState = State.Grouded;
                }
            }

            if(statManager.Hp <= 0)
            {
                feedback.PlayFeedback(1);
                Destroy(this.gameObject);
            }
        }



        protected void UpdateKnockback()
        {
            if (statManager.Hp <= 0)
            {
                return;
            }


            if (actualBreakArmor == -1)
                BreakJump();

            if(isKnockbackGround == true)
                NoMoveOnGround();

            actualKnockbackTime -= Time.deltaTime * SlowMotionManager.Instance.enemyTime;

            if (actualKnockbackTime < 0)
            {
                actualSpeedX = 0;
                actualSpeedY = 0;
                actualSuperArmor = superArmor;
                actualKnockbackTime = 0;
                characterState = State.Falling;
            }
        }

        private void BreakJump()
        {
            actualSpeedY += breakAdditionalYForce * SlowMotionManager.Instance.playerTime;
        }








        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.tag == "ProjectilePlayer")
            {
                Hit(collision.transform.eulerAngles.z);
            }
        }

        protected virtual void Hit(float angle, float forceMultiplier = 1)
        {
            statManager.Hp -= 1;
            feedback.PlayFeedback(0);
            if (statManager.Hp == 0)
            {
                //feedback.PlayFeedback(1);
                characterState = State.Knockback;
                actualBreakArmor = 0;
                GetComponent<SpriteRenderer>().color = new Color(0, 0, 1);
            }

            if (characterState == State.Grouded)
                isKnockbackGround = true;
            else if (characterState == State.Jumping || characterState == State.Falling)
                isKnockbackGround = false;

            if (isKnockbackGround == true)
                actualKnockbackForce = groundForce;
            else
                actualKnockbackForce = aerialForce;


            if (actualBreakArmor > 0)
            {
                if (actualSuperArmor == 0) // Is Knockback
                {

                    Knockback(knockbackTime);

                    SetSpeed(new Vector2(actualKnockbackForce * Mathf.Cos(angle * Mathf.PI / 180f),
                                         actualKnockbackForce * Mathf.Sin(angle * Mathf.PI / 180f)));

                    actualSuperArmor -= 1;
                }
                else if (actualSuperArmor < 0 && canCombo == true) // Is Knockback && can combo
                {

                    Knockback(knockbackTime);

                    SetSpeed(new Vector2(actualKnockbackForce * Mathf.Cos(angle * Mathf.PI / 180f),
                                         actualKnockbackForce * Mathf.Sin(angle * Mathf.PI / 180f)));
                }
                else if(actualSuperArmor > 0)
                {
                    actualSuperArmor -= 1;
                }
            }
            HitBreak(angle, forceMultiplier);
        }



        public void HitBreak(float angle, float forceMultiplier)
        {
            if (actualBreakArmor == 0) // Is Break
            {

                actualSpeedX = 0;
                if (isKnockbackGround == true)
                {
                    actualSpeedX += aerialForce * Mathf.Cos(angle * Mathf.PI / 180f);
                    actualSpeedY += breakInitialYForce;
                    isKnockbackGround = false;
                }

                Knockback(breakTime);
                bounce = false;


                Debug.Log("Break !");

                actualBreakArmor -= 1;
            }
            else if (actualBreakArmor < 0 && canComboBreak == true) // Is Break && can combo
            {

                Knockback(breakTime);
                bounce = false;

                SetSpeed(new Vector2((actualKnockbackForce * forceMultiplier) * Mathf.Cos(angle * Mathf.PI / 180f),
                                     (actualKnockbackForce * forceMultiplier) * Mathf.Sin(angle * Mathf.PI / 180f)));
                actualSpeedY += (actualKnockbackForce);
            }
            else if(actualBreakArmor > 0)
            {
                actualBreakArmor -= 1;
            }
        }




        private void Knockback(float timeKnockback)
        {
            currentPattern = 0;
            timePattern = 0;
            timePatternStartup = 0;

            characterAnimation.SetDefaultAnimation();
            characterAnimation.ChangeKnockbackAnim();
            actualKnockbackTime = timeKnockback;
            characterState = State.Knockback;
        }




        public void HitPunch(float angle)
        {
            Hit(angle,3);
        }

        #endregion

    } // Enemy class

} // #PROJECTNAME# namespace