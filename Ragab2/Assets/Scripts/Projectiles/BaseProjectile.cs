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
    /// Definition of the BaseProjectile class
    /// </summary>
    public class BaseProjectile : MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */

        [SerializeField]
        protected float speed = 20;
        [SerializeField]
        protected float longevity = 5;

        [SerializeField]
        protected float bulletFriction = 2;

        [SerializeField]
        protected Feedback feedback = null;

        int layerMask = 1 << 8;

        Vector3 initialSpeed;
        
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

        ////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        protected void OnEnable()
        {
            StartCoroutine(LongevityCoroutine());
        }

        
        ////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Update is called once per frame.
        /// </summary>
        protected void Update()
        {
            // note pour opti, faire en sorte que ragab update la vitesse des balles
            if (this.gameObject.activeInHierarchy)
            {
                transform.Translate(((Vector3.right * speed) + initialSpeed) * Time.deltaTime);
                initialSpeed /= bulletFriction;
            }
        }

        public void SetInitialSpeed(Vector3 playerSpeed)
        {
            initialSpeed = playerSpeed;
        }


        private IEnumerator LongevityCoroutine()
        {
            yield return new WaitForSeconds(longevity);
            this.gameObject.SetActive(false);
        }


        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Ground")
            {
                if (SlowMotionManager.Instance.playerTime < 1)
                    this.transform.eulerAngles = new Vector3(0, 0, -transform.eulerAngles.z);
                else
                {
                    feedback.StartFeedback();
                    this.gameObject.SetActive(false);
                }
            }
            if (collision.gameObject.tag == "Enemy")
            {
                feedback.StartFeedback();
                this.gameObject.SetActive(false);
            }

        }

        #endregion

    } // BaseProjectile class

} // #PROJECTNAME# namespace