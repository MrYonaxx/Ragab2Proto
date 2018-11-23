using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Ragab
{
    public class RagabMovement : Character
    {

        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [Header("Ragab Ressources")]
        [SerializeField]
        protected StatManager stats;
        [SerializeField]
        protected CrystalManager crystals;


        [Header("Ragab Feedbacks")]
        // All the feedback applicable on this character
        [SerializeField]
        protected FeedbackManager feedbacks;
        [SerializeField]
        protected AudioSource audioSource;
        [SerializeField]
        protected AudioClip[] feedbacksSound;




        [Header("Ragab Slide")]
        [SerializeField]
        protected float slideSpeed = 12;
        [SerializeField]
        protected BoxCollider2D defaultCollider;
        [SerializeField]
        protected BoxCollider2D slideCollider;


        [Header("Ragab Dash")]
        [SerializeField]
        protected float traceSpeed = 4;
        [SerializeField]
        protected float tracePunchSpeed = 10;
        [SerializeField]
        protected List<float> bulletTimeRatio = new List<float>();
        [SerializeField]
        protected float aimingBulletTimeRatio = 0;

        int comboTrace = -1;

        bool canShoot = true;


        [Header("Ragab Shoot")]
        [SerializeField]
        GameObject ragabArm;
        [SerializeField]
        BaseProjectile projectilePrefab;
        [SerializeField]
        int maxNumberProjectile = 20;
        [SerializeField]
        float timeInterval = 0.1f;
        [SerializeField]
        Transform viseur;
        [SerializeField]
        CameraScript cameraAim;

        [SerializeField]
        SpriteRenderer feedbackDeOuf;

        int indexProjectile = 0;
        List<BaseProjectile> listObject = new List<BaseProjectile>(20);


        [Header("Ragab Punch")]
        [SerializeField]
        float timeTracePunch = 0.5f;

        [Header("Ragab Knockback")]
        [SerializeField]
        float timeStop = 0.1f;
        [SerializeField]
        float timeKnockback = 0.1f;


        [Header("Ragab Autres")]
        [SerializeField]
        protected float secondBeforeJumpDisabled = 0.05f;

        private bool jumpAvailable = true;
        public bool JumpAvailable { get { return jumpAvailable; } }


        private IEnumerator traceDashCoroutine = null;
        bool canRelease = false;
        bool canComboTraceDash = true;

        #endregion

        // =============================================
        // Functions
        // =============================================


        ////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
        /// </summary>
        protected override void Start()
        {
            base.Start();

            indexProjectile = 0;
            //listObject = new List<BaseProjectile>(maxNumberProjectile);
            for (int i = 0; i < maxNumberProjectile; i++)
            {
                listObject.Add(null);
            }
        }



        public void ChangeCollider(BoxCollider2D newCollider)
        {
            characterCollider.enabled = false;
            characterCollider = newCollider;
            characterCollider.enabled = true;
        }


        public override void SetOnGround(bool b)
        {

            if (characterState == State.TraceDashing || characterState == State.TraceDashingAiming || characterState == State.Knockback || characterState == State.TracePunching)
            {
                return;
            }

            if (b == true)
            {
                jumpAvailable = true;
                if (characterState == State.Falling)
                    audioSource.PlayOneShot(feedbacksSound[2]); // Son Fall
                if (characterState != State.Sliding)
                    characterState = State.Grouded;
            }
            else
            {
                StopSliding();
                if (characterState != State.Jumping)
                {
                    characterState = State.Falling;
                    StartCoroutine(WaitBeforeDisableJump(secondBeforeJumpDisabled));
                }
            }
        }






        // ========= JUMP ============ //


        public override void Jump()
        {
            if (jumpAvailable == false && characterState != State.TraceDashing)
                return;

            StopSliding();
            StopTraceDash();

            actualSpeedY = 0;
            actualSpeedY += initialJumpForce;
            characterState = State.Jumping;
            jumpAvailable = false;
            audioSource.PlayOneShot(feedbacksSound[1]); // Son jump
        }

        public virtual void NuanceJump()
        {
            if (characterState == State.TraceDashing)
            {
                return;
            }
            actualSpeedY += additionalJumpForce * SlowMotionManager.Instance.playerTime;
        }

        private IEnumerator WaitBeforeDisableJump(float second)
        {
            yield return new WaitForSeconds(second);
            jumpAvailable = false;
        }




        // ============================== //
        // =========== SHOOT ============ //
        // ============================== //

        public void Shoot(Vector3 playerSpeed)
        {
            if (canShoot == false)
                return;
            if (listObject[indexProjectile] == null)
            {
                BaseProjectile fire = Instantiate(projectilePrefab, this.transform.position + new Vector3(0, 0.64f, 0), viseur.localRotation);
                fire.transform.SetParent(this.transform);
                //fire.SetInitialSpeed(playerSpeed);
                listObject[indexProjectile] = fire;
            }
            else
            {
                listObject[indexProjectile].transform.position = this.transform.position + new Vector3(0, 0.64f, 0);
                listObject[indexProjectile].transform.localRotation = viseur.localRotation;
                //listObject[indexProjectile].SetInitialSpeed(playerSpeed);
                listObject[indexProjectile].gameObject.SetActive(true);

            }
            indexProjectile += 1;
            if (indexProjectile > maxNumberProjectile - 1)
                indexProjectile = 0;
            audioSource.PlayOneShot(feedbacksSound[0]); // Son tir
            StartCoroutine(WaitInterval());

        }

        private IEnumerator WaitInterval()
        {
            canShoot = false;
            yield return new WaitForSeconds(timeInterval);
            canShoot = true;
        }


        public void Aim(Vector2 angle)
        {

            float angleAim = Mathf.Atan2(angle.y, angle.x) * Mathf.Rad2Deg;
            viseur.localRotation = Quaternion.Euler(new Vector3(0, 0, angleAim));

            cameraAim.FocusOnAim(angle);

        }

        public void NoAim()
        {
            cameraAim.FocusDefault();
        }




        // ========= SLIDING ============ //


        public void Sliding()
        {
            if(crystals.getCrystalNumber() == 0)
            {
                StopSliding();
                return;
            }

            if (characterState == State.Knockback)
            {
                if (crystals.getCrystalNumber() <= 1)
                {
                    return;
                }
                feedbacks.PlayFeedback(3);
                crystals.ConsumeCrystal();
            }

            actualSpeedX = slideSpeed * direction;
            bonusSpeedY += -(slideSpeed * 2);

            characterState = State.Sliding;

            ChangeCollider(slideCollider);
            crystals.StartConsumptionSlide();
        }

        public void StopSliding()
        {
            if (characterState == State.Sliding)
            {
                bonusSpeedY = 0;
                characterState = State.Grouded;
                ChangeCollider(defaultCollider);
                crystals.StartRecovery();
            }
        }



        // =========== TRACE DASH ============= //

        public void TraceDashAim(float timeAim = 0.3f)
        {
            if (canComboTraceDash == false)
                return;


            if (crystals.getCrystalNumber() <= 1)
            {
                return;
            }
            crystals.StartRecovery();
            if(characterState != State.TraceDashing && characterState != State.TracePunching)
                SlowMotionManager.Instance.PlayBulletTimeOST(true);

            characterState = State.TraceDashingAiming;

            SlowMotionManager.Instance.SetSlowMotion(0);

            // test
            if (comboTrace == -1)
            {
                cameraAim.ChangeOrthographicSize(-1.5f);
            }
            if (comboTrace == 0)
            {
                cameraAim.ChangeOrthographicSize(-1.75f);
            }
            if (comboTrace >= 1)
            {
                cameraAim.ChangeOrthographicSize(-2.25f);
                canComboTraceDash = false;
            }

            comboTrace += 1;
            if (comboTrace == bulletTimeRatio.Count)
            {
                comboTrace -= 1;
            }
            canRelease = false;
            cameraAim.FocusDefault();
            feedbacks.PlayFeedback(2);


            if (traceDashCoroutine != null)
                StopCoroutine(traceDashCoroutine);

            traceDashCoroutine = TraceDashAimingCoroutine(timeAim);
            StartCoroutine(traceDashCoroutine);
        }

        public void ReleaseTraceDashAim()
        {
            if (canRelease == false)
                return;

            StopCoroutine(traceDashCoroutine);
            traceDashCoroutine = null;
            if (comboTrace == 0)
            {
                SlowMotionManager.Instance.SetSlowMotion(1f);
            }
            canComboTraceDash = true;
            TraceDash();
        }

        private IEnumerator TraceDashAimingCoroutine(float time)
        {
            yield return new WaitForSeconds(time);
            canRelease = true;
            yield return new WaitForSeconds(0.7f);
            ReleaseTraceDashAim();
        }





        public void TraceDash()
        {
            feedbacks.StopFeedback(2);
            feedbacks.PlayFeedback(3);

            crystals.ConsumeCrystal();

            characterState = State.TraceDashing;
            SlowMotionManager.Instance.SetSlowMotionGradually(bulletTimeRatio[comboTrace]);
            SetSpeed(new Vector2(traceSpeed * Mathf.Cos(viseur.eulerAngles.z * Mathf.PI / 180f),
                                                    traceSpeed * Mathf.Sin(viseur.eulerAngles.z * Mathf.PI / 180f)));

            ragabArm.SetActive(true);
            CharacterAnimation.SetSpriteRotation(viseur);
            crystals.StartConsumptionTraceDashing();
            feedbacks.PlayFeedback(1); // Rémanence
            cameraAim.ChangeOrthographicSize(-1);
            //comboTrace = -1;
        }

        public void StopTraceDash()
        {
            if (characterState == State.TraceDashing)
            {
                comboTrace = -1;
                SlowMotionManager.Instance.SetSlowMotionGradually(1f);


                ragabArm.SetActive(false);
                characterAnimation.SetSpriteRotation();
                crystals.StartRecovery();
                feedbacks.StopFeedback(1);
                feedbacks.StopFeedback(3);
                cameraAim.ChangeOrthographicSize(0);
                if(characterState != State.TracePunching)
                {
                    SlowMotionManager.Instance.PlayBulletTimeOST(false);
                    characterState = State.Falling;
                }

            }
        }





        // =========== TRACE DASH PUNCH ============= //

        public void TraceDashPunch()
        {
            characterState = State.TracePunching;
            StopTraceDash();
            //characterState = State.TracePunching;
            canComboTraceDash = true;
            SlowMotionManager.Instance.SetSlowMotion(1);
            SetSpeed(new Vector2(tracePunchSpeed * Mathf.Cos(viseur.eulerAngles.z * Mathf.PI / 180f),
                                 tracePunchSpeed * Mathf.Sin(viseur.eulerAngles.z * Mathf.PI / 180f)));
            CharacterAnimation.SetSpriteRotation(viseur);

            if (traceDashCoroutine != null)
            {
                StopCoroutine(traceDashCoroutine);
            }
            traceDashCoroutine = WaitTracePunch(timeTracePunch);
            StartCoroutine(traceDashCoroutine);
        }

        public void TraceDashPunchHit()
        {
            if (traceDashCoroutine != null)
            {
                StopCoroutine(traceDashCoroutine);
            }
            SlowMotionManager.Instance.SetSlowMotion(0.05f);
            feedbacks.PlayFeedback(0);
        }

        private IEnumerator WaitTracePunch(float second)
        {
            yield return new WaitForSeconds(second);
            traceDashCoroutine = null;
            characterState = State.Falling;
            characterAnimation.SetSpriteRotation();

        }



        protected override void CollisionY()
        {
            if (characterState == State.TracePunching)
            {
                TraceDashAim(1f);
            }
            if (characterState == State.TraceDashing)
            {
                characterAnimation.SetSpriteRotation();
                //feedbacks.PlayFeedback(0); // ShakeScreen
                //StopTraceDash();
                TraceDashAim(1f);
                //TraceDashPunchHit();
                //slideCollisionEvent.Invoke();
            }
        }


        protected override void CollisionX()
        {
            if (characterState == State.TracePunching)
            {
                TraceDashAim(1f);
            }
            if (characterState == State.TraceDashing)
            {
                characterAnimation.SetSpriteRotation();
                //feedbacks.PlayFeedback(0);
                //StopTraceDash();
                TraceDashAim(1f);
                //TraceDashPunchHit();
                //slideCollisionEvent.Invoke();
            }
        }



        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "ProjectileEnemy")
            {
                collision.gameObject.SetActive(false);
                Hit();
                /*if (characterState != State.Knockback )//|| characterState != State.TracePunching)
                    Hit();*/
                // faire un truc si le joueur se prend des dégats durant le punch
            }
            if (collision.gameObject.tag == "Enemy" && characterState == State.TracePunching)
            {
                audioSource.PlayOneShot(feedbacksSound[3]);
                if(crystals.getCrystalNumber() <= 1)
                {
                    TraceDashPunchHit();
                    feedbackDeOuf.enabled = true;
                    collision.GetComponent<Enemy>().HitPunch(viseur.eulerAngles.z);
                    characterAnimation.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 1);
                    collision.GetComponent<SpriteRenderer>().sortingOrder = 2;
                    collision.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 1);
                    this.GetComponent<BoxCollider2D>().enabled = false;
                }
                else
                {
                    TraceDashAim(1f);
                    collision.GetComponent<Enemy>().HitPunch(viseur.eulerAngles.z);
                }
            }
        }



        protected virtual void Hit()
        {
            stats.LoseHP(1);
            StartCoroutine(KnockbackCoroutine());
        }

        private IEnumerator KnockbackCoroutine()
        {
            StopSliding();
            StopTraceDash();
            actualSpeedX = 0;
            actualSpeedY = 0;
            characterState = State.Knockback;
            feedbacks.PlayFeedback(0);
            crystals.ShakeHUD(20, 12);
            stats.ShakeHUD(30, 20);
            SlowMotionManager.Instance.SetSlowMotion(0);
            yield return new WaitForSeconds(0.2f);
            jumpAvailable = true;
            SlowMotionManager.Instance.SetSlowMotion(1);
            yield return new WaitForSeconds(0.7f);
            characterState = State.Falling;
        }

    }

}
