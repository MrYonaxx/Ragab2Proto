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

        [Header("Event")]
        [SerializeField]
        UnityEvent eventHit;

        private IEnumerator currentPattern = null;

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


        protected void EnemyIA()
        {
            //int pattern = Random.Range();

        }



        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.tag == "ProjectilePlayer")
            {
                eventHit.Invoke();
            }
        }

        #endregion

    } // Enemy class

} // #PROJECTNAME# namespace