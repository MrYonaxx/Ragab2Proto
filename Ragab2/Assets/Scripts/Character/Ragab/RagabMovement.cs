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

        int indexProjectile = 0;
        List<BaseProjectile> listObject = new List<BaseProjectile>(20);


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

            if (characterState == State.TraceDashing || characterState == State.TraceDashingAiming || characterState == State.Knockback)
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
            characterState = State.Sliding;
            actualSpeedX = slideSpeed * direction;
            bonusSpeedY += -(slideSpeed * 2);
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


        public void TraceDashAim()
        {
            if (canComboTraceDash == false)
                return;

            if (crystals.getCrystalNumber() <= 1)
            {
                return;
            }
            crystals.StartRecovery();
            characterState = State.TraceDashingAiming;
            SlowMotionManager.Instance.SetSlowMotion(0);

            // test
            if (comboTrace == -1)
            {
                cameraAim.ChangeOrthographicSize(3.5f);
            }
            if (comboTrace == 0)
            {
                cameraAim.ChangeOrthographicSize(3.25f);
            }
            if (comboTrace >= 1)
            {
                cameraAim.ChangeOrthographicSize(2.75f);
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

            traceDashCoroutine = TraceDashAimingCoroutine();
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

        private IEnumerator TraceDashAimingCoroutine()
        {
            yield return new WaitForSeconds(0.4f);
            canRelease = true;
            yield return new WaitForSeconds(0.6f);
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
            cameraAim.ChangeOrthographicSize(4);
        }

        public void StopTraceDash()
        {
            if (characterState == State.TraceDashing)
            {
                comboTrace = -1;
                SlowMotionManager.Instance.SetSlowMotionGradually(1f);

                characterState = State.Falling;

                ragabArm.SetActive(false);
                characterAnimation.SetSpriteRotation();
                crystals.StartRecovery();
                feedbacks.StopFeedback(1);
                feedbacks.StopFeedback(3);
                cameraAim.ChangeOrthographicSize(5);
            }
        }





        protected override void CollisionY()
        {
            if (characterState == State.TraceDashing)
            {
                characterAnimation.SetSpriteRotation();
                feedbacks.PlayFeedback(0); // ShakeScreen
                StopTraceDash();
                //slideCollisionEvent.Invoke();
            }
        }


        protected override void CollisionX()
        {
            if (characterState == State.TraceDashing)
            {
                characterAnimation.SetSpriteRotation();
                feedbacks.PlayFeedback(0);
                StopTraceDash();
                //slideCollisionEvent.Invoke();
            }
        }



        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "ProjectileEnemy" && characterState != State.Knockback)
            {
                Hit();
            }
        }

        protected virtual void Hit()
        {
            StartCoroutine(KnockbackCoroutine());
            /*statManager.Hp -= 1;
            if (statManager.Hp == 0)
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
            }*/
        }

        private IEnumerator KnockbackCoroutine()
        {
            StopSliding();
            StopTraceDash();
            characterState = State.Knockback;
            feedbacks.PlayFeedback(0);
            SlowMotionManager.Instance.SetSlowMotion(0);
            yield return new WaitForSeconds(0.4f);
            SlowMotionManager.Instance.SetSlowMotion(1);
            yield return new WaitForSeconds(0.2f);
            characterState = State.Falling;
        }

    }

}
