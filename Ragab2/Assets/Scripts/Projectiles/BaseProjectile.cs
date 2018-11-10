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
        protected float damage = 1;
        
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
        protected void Awake()
        {
            StartCoroutine(LongevityCoroutine());
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
        /// </summary>
        protected virtual void Start()
        {
            
        }
        
        ////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Update is called once per frame.
        /// </summary>
        protected void Update()
        {
            
        }


        private IEnumerator LongevityCoroutine()
        {
            yield return new WaitForSeconds(longevity);
            this.gameObject.SetActive(false);
        }
        
        #endregion

    } // BaseProjectile class

} // #PROJECTNAME# namespace