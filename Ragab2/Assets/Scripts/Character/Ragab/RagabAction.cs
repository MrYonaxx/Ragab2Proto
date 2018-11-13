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
        RagabMovementRaycast characterToMove;

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
        [SerializeField]
        Transform curseur;


        [Header("Ragab Dash")]
        [SerializeField]
        protected float traceSpeed = 4;

        bool canShoot = true;


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
                //fire.transform.SetParent(this.transform);
                fire.SetInitialSpeed(playerSpeed);
                listObject[indexProjectile] = fire;
            }
            else
            {
                listObject[indexProjectile].transform.position = this.transform.position + new Vector3(0, 0.64f, 0);
                listObject[indexProjectile].transform.localRotation = viseur.localRotation;
                listObject[indexProjectile].SetInitialSpeed(playerSpeed);
                listObject[indexProjectile].gameObject.SetActive(true);

            }
            indexProjectile += 1;
            if (indexProjectile > maxNumberProjectile-1)
                indexProjectile = 0;
            StartCoroutine(WaitInterval());

        }

        public void Aim(Vector2 angle)
        {
            /*float length = Mathf.Sqrt(angle.x * angle.x + angle.y * angle.y);
            if(length > 0.7f)
            {
                angle = new Vector2(angle.x / length, angle.y / length);
                Debug.Log(angle);
            }*/
            //angle.Normalize();
            //angle = Vector2.ClampMagnitude(angle, 0.2f);
            float angleAim = Mathf.Atan2(angle.y, angle.x) * Mathf.Rad2Deg;
            viseur.localRotation = Quaternion.Euler(new Vector3(0,0,angleAim));

            //curseur.position = new Vector2(this.transform.position.x, this.transform.position.y) + angle;
            //viseur.localRotation.SetLookRotation(curseur.position);
        }


        private IEnumerator WaitInterval()
        {
            canShoot = false;
            yield return new WaitForSeconds(timeInterval);
            canShoot = true;
        }
        

        public void TraceDash()
        {
            characterToMove.characterState = State.TraceDashing;
            characterToMove.SetSpeed( new Vector2 (traceSpeed * Mathf.Cos(viseur.eulerAngles.z * Mathf.PI / 180f),
                                                    traceSpeed * Mathf.Sin(viseur.eulerAngles.z * Mathf.PI / 180f)));
            //Vector3.right* speed;
        }

        public void StopTraceDash()
        {
            characterToMove.characterState = State.Falling;
            //Vector3.right* speed;
        }

        #endregion

    } // RagabAction class

} // #PROJECTNAME# namespace