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


        [Header("Ragab Autres")]
        [SerializeField]
        protected float secondBeforeJumpDisabled = 0.05f;

        private bool jumpAvailable = true;
        public bool JumpAvailable { get { return jumpAvailable; } }





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

            if (characterState == State.TraceDashing || characterState == State.TraceDashingAiming)
            {
                return;
            }

            if (b == true)
            {
                jumpAvailable = true;
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
            SlowMotionManager.Instance.SetSlowMotionGradually(aimingBulletTimeRatio);
            characterState = State.TraceDashingAiming;
        }

        public void ReleaseTraceDashAim()
        {
            SlowMotionManager.Instance.SetSlowMotion(1f);
            TraceDash();
        }



        public void TraceDash()
        {
            if (crystals.getCrystalNumber() <= 1)
            {
                return;
            }
            crystals.ConsumeCrystal();

            comboTrace += 1;
            if (comboTrace == bulletTimeRatio.Count)
            {
                comboTrace -= 1;
            }
            characterState = State.TraceDashing;
            SlowMotionManager.Instance.SetSlowMotionGradually(bulletTimeRatio[comboTrace]);
            SetSpeed(new Vector2(traceSpeed * Mathf.Cos(viseur.eulerAngles.z * Mathf.PI / 180f),
                                                    traceSpeed * Mathf.Sin(viseur.eulerAngles.z * Mathf.PI / 180f)));

            ragabArm.SetActive(true);
            CharacterAnimation.SetSpriteRotation(viseur);
            crystals.StartConsumptionTraceDashing();
            feedbacks.PlayFeedback(1);
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
            }
        }





        protected override void CollisionY()
        {
            if (characterState == State.TraceDashing)
            {
                characterAnimation.SetSpriteRotation();
                feedbacks.PlayFeedback(0);
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



        void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("touche");
        }

    }

}
