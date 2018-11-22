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
    /// Definition of the CrapoProjectile class
    /// </summary>
    public class CrapoProjectile : MonoBehaviour
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
        protected float timeWaiting = 1;

        float time = 0;

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
        /// Update is called once per frame.
        /// </summary>
        protected void Update()
        {
            time += Time.deltaTime * SlowMotionManager.Instance.enemyTime;
            if(time >= timeWaiting)
            {
                if(transform.parent != null)
                    transform.parent = null;
                transform.Translate((Vector3.right * speed) * Time.deltaTime * SlowMotionManager.Instance.enemyTime);
            }
            if(time >= longevity)
            {
                Destroy(this.gameObject);
            }
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Ground")
                Destroy(this.gameObject);
        }

        #endregion

    } // CrapoProjectile class

} // #PROJECTNAME# namespace