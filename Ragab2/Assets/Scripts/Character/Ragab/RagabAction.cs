/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Ragab
{
    /// <summary>
    /// Definition of the RagabAction class
    /// </summary>
    public class RagabAction : MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [Header("Ragab Movement")]
        [SerializeField]
        RagabMovement characterToMove;

        [Header("Ragab Shoot")]
        [SerializeField]
        BaseProjectile projectilePrefab;
        [SerializeField]
        int maxNumberProjectile = 20;
        [SerializeField]
        float timeInterval = 0.1f;

        int indexProjectile = 0;
        List<BaseProjectile> listObject = new List<BaseProjectile>(20);

        [SerializeField]
        Transform viseur;


        [Header("Ragab Dash")]
        [SerializeField]
        protected float traceSpeed = 4;
        [SerializeField]
        protected List<float> bulletTimeRatio = new List<float>();
        [SerializeField]
        protected float aimingBulletTimeRatio = 0;

        int comboTrace = -1;

        bool canShoot = true;


        [Header("Feedbacks")]
        [SerializeField]
        CameraScript cameraAim;

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


        ////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
        /// </summary>
        protected virtual void Start()
        {
            indexProjectile = 0;
            //listObject = new List<BaseProjectile>(maxNumberProjectile);
            for(int i = 0; i < maxNumberProjectile; i++)
            {
                listObject.Add(null);
            }
        }


        public void Shoot(Vector3 playerSpeed)
        {
            if (canShoot == false)
                return;
            if (listObject[indexProjectile] == null)
            {
                BaseProjectile fire = Instantiate(projectilePrefab, this.transform.position + new Vector3(0,0.64f,0), viseur.localRotation);
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
            if (indexProjectile > maxNumberProjectile-1)
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
            viseur.localRotation = Quaternion.Euler(new Vector3(0,0,angleAim));

            cameraAim.FocusOnAim(angle);

        }

        public void NoAim()
        {
            cameraAim.FocusDefault();
        }









        public void TraceDashAim()
        {
            SlowMotionManager.Instance.SetSlowMotionGradually(aimingBulletTimeRatio);
            characterToMove.characterState = State.TraceDashingAiming;
        }

        public void ReleaseTraceDashAim()
        {
            SlowMotionManager.Instance.SetSlowMotion(1f);
            TraceDash();
        }

        public void TraceDash()
        {
            comboTrace += 1;
            if(comboTrace == bulletTimeRatio.Count)
            {
                comboTrace -= 1;
            }
            characterToMove.characterState = State.TraceDashing;
            SlowMotionManager.Instance.SetSlowMotionGradually(bulletTimeRatio[comboTrace]);
            characterToMove.SetSpeed( new Vector2 (traceSpeed * Mathf.Cos(viseur.eulerAngles.z * Mathf.PI / 180f),
                                                    traceSpeed * Mathf.Sin(viseur.eulerAngles.z * Mathf.PI / 180f)));

            characterToMove.CharacterAnimation.SetSpriteRotation(viseur);
        }

        public void StopTraceDash()
        {
            comboTrace = -1;
            SlowMotionManager.Instance.SetSlowMotionGradually(1f);

            characterToMove.characterState = State.Falling;
            characterToMove.CharacterAnimation.SetSpriteRotation();
        }


        #endregion

    } // RagabAction class

} // #PROJECTNAME# namespace